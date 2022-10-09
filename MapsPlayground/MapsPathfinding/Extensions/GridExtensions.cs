using System.Collections.Immutable;

namespace MapsPathfinding.Extensions;

public static class GridExtensions
{
    public static ImmutableArray<TCell> GetAdjacentNonBlockers<TGrid, TCell>(this TGrid grid, TCell cell)
        where TGrid : IGrid<TCell>
        where TCell : ICell
    {
        ImmutableArray<TCell> adjacentCells = grid.GetAdjacent(cell);
        
        ImmutableArray<TCell>.Builder builder = ImmutableArray.CreateBuilder<TCell>(adjacentCells.Length);

        foreach (var adjacent in adjacentCells)
        {
            if (adjacent is { IsBlocker: false } nonBlocker)
                builder.Add(nonBlocker);
        }

        return builder.ToImmutable();
    }
}
