using System.Collections.Generic;

namespace InformationSystems.Graphs.Extensions;

public static class GridExtensions
{
    public static IReadOnlyList<TCell> GetAdjacentNonBlockers<TGrid, TCell>(this TGrid grid, TCell cell)
        where TGrid : IGrid<TCell>
        where TCell : ICell
    {
        List<TCell> nonBlockers = new();

        foreach (var adjacent in grid.GetOutgoing(cell))
        {
            if (adjacent is { IsBlocker: false } nonBlocker)
                nonBlockers.Add(nonBlocker);
        }

        return nonBlockers;
    }
}
