using System.Collections.Immutable;

namespace InformationSystems.Graphs;

public partial class RectangularGrid<TCell>
{
    private static readonly ImmutableArray<CellSelector> _adjacentSelectors;

    static RectangularGrid()
    {
        ImmutableArray<CellSelector>.Builder builder = ImmutableArray.CreateBuilder<CellSelector>(8);

        builder.Add(new CellSelector
        {
            Predicate = (cell, _) => cell.Y > 0,
            Selector = (cell, grid) => grid[cell.Y - 1, cell.X]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.Y < grid.Height - 1,
            Selector = (cell, grid) => grid[cell.Y + 1, cell.X]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.X > 0,
            Selector = (cell, grid) => grid[cell.Y, cell.X - 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.X < grid.Width - 1,
            Selector = (cell, grid) => grid[cell.Y, cell.X + 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.Y > 0 && cell.X > 0,
            Selector = (cell, grid) => grid[cell.Y - 1, cell.X - 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.Y > 0 && cell.X < grid.Width - 1,
            Selector = (cell, grid) => grid[cell.Y - 1, cell.X + 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.Y < grid.Height - 1 && cell.X < grid.Width - 1,
            Selector = (cell, grid) => grid[cell.Y + 1, cell.X + 1]
        });

        builder.Add(new CellSelector
        {
            Predicate = (cell, grid) => cell.Y < grid.Height - 1 && cell.X > 0,
            Selector = (cell, grid) => grid[cell.Y + 1, cell.X - 1]
        });

        _adjacentSelectors = builder.MoveToImmutable();
    }
}
