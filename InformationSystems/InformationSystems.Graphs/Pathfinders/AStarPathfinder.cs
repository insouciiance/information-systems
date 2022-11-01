using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace InformationSystems.Graphs.Pathfinders;

public class AStarPathfinder<T, TGraph>
    : IPathfinder<T, TGraph, DefaultSinglePathPathfinderResult<T, TGraph>>
    where T : notnull
    where TGraph : IGraph<T>
{
    private readonly Func<T, T, float> _heuristic;

    private readonly TGraph _graph;

    private readonly T _start;

    private readonly T _end;

    public AStarPathfinder(TGraph graph, T start, T end, Func<T, T, float> heuristic)
    {
        _graph = graph;
        _heuristic = heuristic;
        _start = start;
        _end = end;
    }

    public DefaultSinglePathPathfinderResult<T, TGraph> GetPathResult()
    {
        HashSet<T> visited = new();

        Dictionary<T, CellInfo> queue = new()
        {
            { _start, new(this, _start, _end) }
        };

        CellInfo? endInfo = null;

        while(true)
        {
            if (queue.Count == 0)
                break;

            CellInfo bestCell = GetBestCell();

            IEnumerable<T> adjacentCells = _graph.GetOutgoing(bestCell.Cell);

            foreach (var current in adjacentCells)
            {
                if (visited.Contains(current))
                    continue;

                CellInfo currentInfo = new(this, current, _end) { Parent = bestCell };

                ref CellInfo? existingInfo = ref CollectionsMarshal.GetValueRefOrAddDefault(queue, current, out bool existed);
                existingInfo ??= currentInfo;

                if (existed && existingInfo!.ElapsedCost > currentInfo.ElapsedCost)
                    existingInfo.Parent = bestCell;

                if (EqualityComparer<T>.Default.Equals(current, _end))
                {
                    endInfo = currentInfo;
                    break;
                }
            }

            queue.Remove(bestCell.Cell);
            visited.Add(bestCell.Cell);

            CellInfo GetBestCell()
            {
                CellInfo best = null!;

                foreach (var (_, info) in queue)
                {
                    if (best is null || info.TotalCost < best.TotalCost)
                        best = info;
                }

                return best;
            }
        }

        return new()
        {
            Graph = _graph,
            Start = _start,
            End = _end,
            Path = endInfo is null ? ImmutableArray<T>.Empty : ConstructPath(endInfo)
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
        private readonly AStarPathfinder<T, TGraph> _this;

        private T _end;

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

        public float CostLeft => _this._heuristic.Invoke(Cell, _end);

        public float TotalCost => ElapsedCost + CostLeft;

        public CellInfo(AStarPathfinder<T, TGraph> @this, T cell, T end)
        {
            _this = @this;
            Cell = cell;
            _end = end;
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
