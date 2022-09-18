using System.Collections.Generic;

namespace MapsPathfinding.Pathfinders;

public interface IGridPathfinder<TGrid, TCell, TCellEnumerator, TGridPathfinderResult>
    where TGrid : IGrid<TCell, TCellEnumerator>
    where TCell : ICell<TCell>
    where TCellEnumerator : IEnumerator<TCell>
    where TGridPathfinderResult : IGridPathfinderResult<TGrid, TCell, TCellEnumerator>
{
    TGridPathfinderResult GetPathResult(TCell start, TCell end);
}
