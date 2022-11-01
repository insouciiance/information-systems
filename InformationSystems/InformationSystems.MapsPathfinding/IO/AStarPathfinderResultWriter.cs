using InformationSystems.MapsPathfinding.Pathfinders;

namespace InformationSystems.MapsPathfinding.IO;

public class AStarPathfinderResultWriter<TCell>
    : IGridPathfinderResultWriter<AStarPathfinder<TCell>.AStarPathfinderResult, TCell>
    where TCell : ICell
{
    public static GridOutputMap Write(AStarPathfinder<TCell>.AStarPathfinderResult result)
    {
        GridOutputMap writer = new(result.Grid.Width, result.Grid.Height);
        ResultWriterHelper.WriteResultBase(result, writer);
        return writer;
    }
}
