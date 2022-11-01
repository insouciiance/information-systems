namespace InformationSystems.MapsPathfinding.Pathfinders;

public interface IGridPathfinder<TCell, TGridPathfinderResult>
    where TCell : ICell
    where TGridPathfinderResult : IGridPathfinderResult<TCell>
{
    TGridPathfinderResult GetPathResult(TCell start, TCell end);
}
