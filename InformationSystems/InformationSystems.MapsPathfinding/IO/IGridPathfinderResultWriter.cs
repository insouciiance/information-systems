using InformationSystems.MapsPathfinding.Pathfinders;

namespace InformationSystems.MapsPathfinding.IO;

public interface IGridPathfinderResultWriter<TGridPathfinderResult, TCell>
    where TGridPathfinderResult : IGridPathfinderResult<TCell>
    where TCell : ICell
{
    static abstract GridOutputMap Write(TGridPathfinderResult result);
}
