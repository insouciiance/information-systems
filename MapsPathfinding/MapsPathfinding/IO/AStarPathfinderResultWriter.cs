using System.Collections.Generic;
using MapsPathfinding.Pathfinders;

namespace MapsPathfinding.IO;

public class AStarPathfinderResultWriter<TGrid, TCell, TCellEnumerator>
    : IGridPathfinderResultWriter<AStarPathfinder<TGrid, TCell, TCellEnumerator>.AStarPathfinderResult, TGrid, TCell, TCellEnumerator>
    where TGrid : IGrid<TCell, TCellEnumerator>
    where TCell : ICell<TCell>
    where TCellEnumerator : IEnumerator<TCell>
{
    public static GridOutputMap Write(AStarPathfinder<TGrid, TCell, TCellEnumerator>.AStarPathfinderResult result)
    {
        GridOutputMap writer = new(result.Grid.Width, result.Grid.Height);
        ResultWriterHelper.WriteResultBase(result, writer);
        return writer;
    }
}
