using System.Collections.Immutable;

namespace InformationSystems.Graphs.SpanningTrees;

public interface ISpanningTreeConstructor<T, TGraph>
    where TGraph : IGraph<T>
{
    ImmutableArray<(T From, T To)> ConstructSpanningTree();
}
