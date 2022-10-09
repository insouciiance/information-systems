using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace MapsPathfinding.Pathfinders;

public class AStarPathfinder<TCell>
    : IGridPathfinder<TCell, AStarPathfinder<TCell>.AStarPathfinderResult>
    where TCell : ICell
{
    public static readonly Func<TCell, TCell, float> DefaultHeuristic = (lhs, rhs) => (float)Math.Sqrt(Math.Pow(lhs.X - rhs.X, 2) + Math.Pow(lhs.Y - rhs.Y, 2));

    private readonly Func<TCell, TCell, float> _heuristic;

    private IGrid<TCell> _grid;

    public AStarPathfinder(IGrid<TCell> grid, Func<TCell, TCell, float>? heuristic = null)
    {
        _grid = grid;
        _heuristic = heuristic ?? DefaultHeuristic;
    }

    public AStarPathfinder<TCell>.AStarPathfinderResult GetPathResult(TCell start, TCell end)
    {
        HashSet<TCell> visited = new();

        Dictionary<TCell, CellInfo> queue = new()
        {
            { start, new(this, start, end) }
        };

        CellInfo? endInfo = null;

        while(true)
        {
            if (queue.Count == 0)
                break;

            CellInfo bestCell = GetBestCell();

            ImmutableArray<TCell> adjacentCells = _grid.GetAdjacent(bestCell.Cell);

            foreach (var current in adjacentCells)
            {
                if (current.IsBlocker || visited.Contains(current))
                    continue;

                CellInfo currentInfo = new(this, current, end) { Parent = bestCell };

                ref CellInfo? existingInfo = ref CollectionsMarshal.GetValueRefOrAddDefault(queue, current, out bool existed);
                existingInfo ??= currentInfo;

                if (existed && existingInfo!.ElapsedCost > currentInfo.ElapsedCost)
                    existingInfo.Parent = bestCell;

                if (EqualityComparer<TCell>.Default.Equals(current, end))
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
            Grid = _grid,
            Start = start,
            End = end,
            Path = endInfo is null ? ImmutableArray<TCell>.Empty : ConstructPath(endInfo)
        };

        ImmutableArray<TCell> ConstructPath(CellInfo endInfo)
        {
            ImmutableArray<TCell>.Builder builder = ImmutableArray.CreateBuilder<TCell>();

            CellInfo? current = endInfo;

            while (current is not null)
            {
                builder.Insert(0, current.Cell);
                current = current.Parent;
            }

            return builder.ToImmutable();
        }
    }

    public readonly struct AStarPathfinderResult : IGridPathfinderResult<TCell>
    {
        public IGrid<TCell> Grid { get; init; }

        public TCell Start { get; init; }

        public TCell End { get; init; }

        public ImmutableArray<TCell> Path { get; init; }
    }

    private class CellInfo
    {
        private readonly AStarPathfinder<TCell> _this;

        private TCell _end;

        private CellInfo? _parent;

        public TCell Cell { get; init; }

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

        public CellInfo(AStarPathfinder<TCell> @this, TCell cell, TCell end)
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

            ElapsedCost = Parent.ElapsedCost + _this._grid.GetCost(Parent.Cell, Cell);
        }
    }
}
