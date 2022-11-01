using System.Collections.Immutable;

namespace InformationSystems.Graphs.Pathfinders;

public interface ISinglePathPathfinderResult<T, TGraph> : IPathfinderResult<T, TGraph>
    where TGraph : IGraph<T>
{
    bool HasPath => Path.Length > 0;

    T Start { get; }

    T End { get; }

    ImmutableArray<T> Path { get; }
}
