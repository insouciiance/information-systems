using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;

namespace InformationSystems.Graphs.Pathfinders;

public class DijkstraPathfinder<T, TGraph>
    : IPathfinder<T, TGraph, DefaultManyPathsPathfinderResult<T, TGraph>>
    where T : notnull
    where TGraph : IGraph<T>
{
    private readonly TGraph _graph;

    private readonly T _start;

    public DijkstraPathfinder(TGraph graph, T start)
    {
        _graph = graph;
        _start = start;
    }

    public DefaultManyPathsPathfinderResult<T, TGraph> GetPathResult()
    {
        Dictionary<T, CellInfo> queue = new()
        {
            { _start, new(this, _start) }
        };

        Dictionary<T, CellInfo> visited = new();

        while (true)
        {
            if (queue.Count == 0)
                break;

            CellInfo bestCell = GetBestCell();

            IEnumerable<T> outgoingVertices = _graph.GetOutgoing(bestCell.Cell);

            foreach (var current in outgoingVertices)
            {
                if (visited.ContainsKey(current))
                    continue;

                CellInfo currentInfo = new(this, current) { Parent = bestCell };

                ref CellInfo? existingInfo = ref CollectionsMarshal.GetValueRefOrAddDefault(queue, current, out bool existed);
                existingInfo ??= currentInfo;

                if (currentInfo.ElapsedCost < existingInfo.ElapsedCost)
                    existingInfo.Parent = bestCell;
            }

            queue.Remove(bestCell.Cell);
            visited.Add(bestCell.Cell, bestCell);

            CellInfo GetBestCell()
            {
                CellInfo best = null!;

                foreach (var (_, info) in queue)
                {
                    if (best is null || info.ElapsedCost < best.ElapsedCost)
                        best = info;
                }

                return best;
            }
        }

        return new()
        {
            Graph = _graph,
            Paths = visited.Select(c =>
            {
                return (ISinglePathPathfinderResult<T, TGraph>)new DefaultSinglePathPathfinderResult<T, TGraph>()
                {
                    Graph = _graph,
                    Start = _start,
                    End = c.Key,
                    Path = c.Value is null ? ImmutableArray<T>.Empty : ConstructPath(c.Value)
                };
            }).ToImmutableArray()
        };

        ImmutableArray<T> ConstructPath(CellInfo endInfo)
        {
            ImmutableArray<T>.Builder builder = ImmutableArray.CreateBuilder<T>();

            CellInfo? current = endInfo;

            while (current is not null)
            {
                builder.Insert(0, current.Cell);
                current = current.Parent;
            }

            return builder.ToImmutable();
        }
    }

    private class CellInfo
    {
        private readonly DijkstraPathfinder<T, TGraph> _this;

        private CellInfo? _parent;

        public T Cell { get; init; }

        public CellInfo? Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                ReprocessCost();
            }
        }

        public float ElapsedCost { get; private set; }

        public CellInfo(DijkstraPathfinder<T, TGraph> @this, T cell)
        {
            _this = @this;
            Cell = cell;
        }

        private void ReprocessCost()
        {
            if (Parent is null)
            {
                ElapsedCost = 0;
                return;
            }

            ElapsedCost = Parent.ElapsedCost + _this._graph.GetCost(Parent.Cell, Cell);
        }
    }
}
