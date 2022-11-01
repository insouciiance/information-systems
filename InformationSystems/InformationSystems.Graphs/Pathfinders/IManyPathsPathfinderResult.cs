using System.Collections.Immutable;

namespace InformationSystems.Graphs.Pathfinders;

public interface IManyPathsPathfinderResult<T, TGraph> : IPathfinderResult<T, TGraph>
    where TGraph : IGraph<T>
{
    ImmutableArray<ISinglePathPathfinderResult<T, TGraph>> Paths { get; }
}
