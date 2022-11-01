using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace InformationSystems.MapsPathfinding;

public partial class RectangularGrid<TCell> : IGrid<TCell>
    where TCell : ICell
{
    private readonly TCell[,] _cells;

    public int Width { get; }

    public int Height { get; }

    public RectangularGrid(int width, int height, IEnumerable<TCell> cells)
    {
        _cells = new TCell[height, width];

        foreach (var cell in cells)
            _cells[cell.Y, cell.X] = cell;

        Width = width;
        Height = height;
    }

    public bool TryGetCell(int x, int y, out TCell cell)
    {
        cell = _cells[y, x];
        return true;
    }

    public ImmutableArray<TCell> GetAdjacent(TCell cell)
    {
        ImmutableArray<TCell>.Builder builder = ImmutableArray.CreateBuilder<TCell>(_adjacentSelectors.Length);
        
        foreach(var selector in _adjacentSelectors)
        {
            if (selector.Predicate.Invoke(cell, this))
                builder.Add(selector.Selector(cell, _cells));
        }

        return builder.ToImmutable();
    }

    public float GetCost(TCell lhs, TCell rhs)
    {
        return (float)Math.Sqrt(Math.Pow(lhs.X - rhs.X, 2) + Math.Pow(lhs.Y - rhs.Y, 2));
    }

    public readonly struct CellSelector
    {
        public Func<TCell, RectangularGrid<TCell>, bool> Predicate { get; init; }

        public Func<TCell, TCell[,], TCell> Selector { get; init; }
    }
}
