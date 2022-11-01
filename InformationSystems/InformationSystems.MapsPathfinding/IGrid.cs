using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace InformationSystems.MapsPathfinding;

public interface IGrid<TCell>
    where TCell : ICell
{
    int Width { get; }

    int Height { get; }

    bool TryGetCell(int x, int y, [MaybeNullWhen(false)] out TCell cell);

    ImmutableArray<TCell> GetAdjacent(TCell cell);

    float GetCost(TCell lhs, TCell rhs);
}
