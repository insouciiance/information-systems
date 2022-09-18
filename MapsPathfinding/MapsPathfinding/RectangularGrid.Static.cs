using System.Collections.Immutable;

namespace MapsPathfinding;

public partial class RectangularGrid<TCell> : IGrid<TCell, RectangularGrid<TCell>.Enumerator>
{
    private static readonly ImmutableArray<CellSelector> _adjacentSelectors;

    static RectangularGrid()
    {
        ImmutableArray<CellSelector>.Builder builder = ImmutableArray.CreateBuilder<CellSelector>(8);

        builder.Add(new CellSelector
        {
            Predicate = (cell, _) => cell.X > 0,
            Selector = (cell, grid) => grid[cell.X - 1, cell.Y]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.X < grid.Height - 1,
            Selector = (cell, grid) => grid[cell.X + 1, cell.Y]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.Y > 0,
            Selector = (cell, grid) => grid[cell.X, cell.Y - 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.Y < grid.Width - 1,
            Selector = (cell, grid) => grid[cell.X, cell.Y + 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.X > 0 && cell.Y > 0,
            Selector = (cell, grid) => grid[cell.X - 1, cell.Y - 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.X > 0 && cell.Y < grid.Width - 1,
            Selector = (cell, grid) => grid[cell.X - 1, cell.Y + 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.X < grid.Height - 1 && cell.Y < grid.Width - 1,
            Selector = (cell, grid) => grid[cell.X + 1, cell.Y + 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.X < grid.Height - 1 && cell.Y > 0,
            Selector = (cell, grid) => grid[cell.X + 1, cell.Y - 1]
        });

        _adjacentSelectors = builder.MoveToImmutable();
    }
}
