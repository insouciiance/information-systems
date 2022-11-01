using System.Collections.Immutable;

namespace InformationSystems.Graphs.Pathfinders;

public readonly struct DefaultSinglePathPathfinderResult<T, TGraph> : ISinglePathPathfinderResult<T, TGraph>
    where TGraph : IGraph<T>
{
    public TGraph Graph { get; init; }

    public T Start { get; init; }

    public T End { get; init; }

    public ImmutableArray<T> Path { get; init; }
}
