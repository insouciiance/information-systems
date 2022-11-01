using System.Collections.Immutable;

namespace InformationSystems.Graphs.Pathfinders;

public readonly struct DefaultManyPathsPathfinderResult<T, TGraph> : IManyPathsPathfinderResult<T, TGraph>
    where TGraph : IGraph<T>
{
    public TGraph Graph { get; init; }

    public ImmutableArray<ISinglePathPathfinderResult<T, TGraph>> Paths { get; init; }
}
