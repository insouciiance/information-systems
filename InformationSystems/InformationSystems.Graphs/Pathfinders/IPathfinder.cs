namespace InformationSystems.Graphs.Pathfinders;

public interface IPathfinder<T, TGraph, TPathfinderResult>
    where TGraph : IGraph<T>
    where TPathfinderResult : IPathfinderResult<T, TGraph>
{
    TPathfinderResult GetPathResult();
}
