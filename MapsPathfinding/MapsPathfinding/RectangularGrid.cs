using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MapsPathfinding;

public partial class RectangularGrid<TCell> : IGrid<TCell, RectangularGrid<TCell>.Enumerator>
    where TCell : ICell<TCell>
{
    private readonly TCell[,] _cells;

    public int Width { get; }

    public int Height { get; }

    public RectangularGrid(int width, int height, IEnumerable<TCell> cells)
    {
        HashSet<(int, int)> providedPositions = new();
        _cells = new TCell[height, width];

        foreach (var cell in cells)
        {
            _cells[cell.X, cell.Y] = cell;
            providedPositions.Add((cell.X, cell.Y));
        }

        Width = width;
        Height = height;

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (!providedPositions.Contains((i, j)))
                    _cells[i, j] = TCell.CreateInstance(i, j);
            }
        }
    }

    public TCell GetCell(int x, int y)
    {
        return _cells[y, x];
    }

    public Enumerator GetAdjacent(TCell cell)
    {
        return new Enumerator(cell, this, _adjacentSelectors.GetEnumerator());
    }

    public float GetCost(TCell lhs, TCell rhs)
    {
        return lhs.X != rhs.X ^ lhs.Y != rhs.Y ? 1 : 1.42f;
    }

    public struct Enumerator : IEnumerator<TCell>
    {
        private readonly TCell _cell;

        private readonly RectangularGrid<TCell> _grid;

        private ImmutableArray<CellSelector>.Enumerator _selectorEnumerator;

        public TCell Current => _selectorEnumerator.Current.Selector.Invoke(_cell, _grid._cells);

        object IEnumerator.Current => Current;

        public Enumerator(TCell cell, RectangularGrid<TCell> grid, ImmutableArray<CellSelector>.Enumerator selectorEnumerator)
        {
            _cell = cell;
            _grid = grid;
            _selectorEnumerator = selectorEnumerator;
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            bool hasNext;

            do
                hasNext = _selectorEnumerator.MoveNext();
            while (hasNext && !_selectorEnumerator.Current.Predicate.Invoke(_cell, _grid));

            return hasNext;
        }

        public void Reset() { }
    }

    public readonly struct CellSelector
    {
        public Func<TCell, RectangularGrid<TCell>, bool> Predicate { get; init; }

        public Func<TCell, TCell[,], TCell> Selector { get; init; }
    }
}
