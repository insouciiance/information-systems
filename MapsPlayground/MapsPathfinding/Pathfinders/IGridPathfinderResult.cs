using System.Collections.Immutable;

namespace MapsPathfinding.Pathfinders;

public interface IGridPathfinderResult<TCell>
    where TCell : ICell
{
    IGrid<TCell> Grid { get; }

    bool HasPath => Path.Length > 0;

    TCell Start { get; }

    TCell End { get; }

    ImmutableArray<TCell> Path { get; }
}
