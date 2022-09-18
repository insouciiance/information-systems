using System.Collections.Generic;
using MapsPathfinding.Pathfinders;

namespace MapsPathfinding.IO;

public interface IGridPathfinderResultWriter<TGridPathfinderResult, TGrid, TCell, TCellEnumerator>
    where TGridPathfinderResult : IGridPathfinderResult<TGrid, TCell, TCellEnumerator>
    where TGrid : IGrid<TCell, TCellEnumerator>
    where TCell : ICell<TCell>
    where TCellEnumerator : IEnumerator<TCell>
{
    static abstract GridOutputMap Write(TGridPathfinderResult result);
}
