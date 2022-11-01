namespace InformationSystems.Graphs.Pathfinders;

public interface IPathfinderResult<T, TGraph>
    where TGraph : IGraph<T>
{
    TGraph Graph { get; }
}
