using System.Collections.Generic;
using System.Collections.Immutable;

namespace MapsPathfinding.Pathfinders;

public interface IGridPathfinderResult<TGrid, TCell, TCellEnumerator>
    where TGrid : IGrid<TCell, TCellEnumerator>
    where TCell : ICell<TCell>
    where TCellEnumerator : IEnumerator<TCell>
{
    TGrid Grid { get; }

    bool HasPath => Path.Length > 0;

    TCell Start { get; }

    TCell End { get; }

    ImmutableArray<TCell> Path { get; }
}
