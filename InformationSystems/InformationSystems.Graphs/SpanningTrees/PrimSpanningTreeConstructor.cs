using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace InformationSystems.Graphs.SpanningTrees;

public class PrimSpanningTreeConstructor<T, TGraph> : ISpanningTreeConstructor<T, TGraph>
    where TGraph : IGraph<T>
{
    private readonly TGraph _graph;

    public PrimSpanningTreeConstructor(TGraph graph)
    {
        _graph = graph;
    }

    public ImmutableArray<(T From, T To)> ConstructSpanningTree()
    {
        T[] allVertices = _graph.GetVertices().ToArray();

        HashSet<T> visitedVertices = new()
        {
            allVertices[0]
        };

        HashSet<(T, T)> spanningTree = new();

        while (true)
        {
            (T From, T To) bestEdge = default;
            float bestCost = float.PositiveInfinity;

            foreach (var vertex in visitedVertices)
            {
                IEnumerable<T> adjacentVertices = _graph.GetOutgoing(vertex);

                foreach (var adjacentVertex in adjacentVertices)
                {
                    if (visitedVertices.Contains(adjacentVertex))
                        continue;

                    float edgeCost = _graph.GetCost(vertex, adjacentVertex);

                    if (edgeCost < bestCost)
                    {
                        bestCost = edgeCost;
                        bestEdge = (vertex, adjacentVertex);
                    }
                }
            }

            if (float.IsPositiveInfinity(bestCost))
                throw new InvalidOperationException("The provided graph was not connected.");

            spanningTree.Add(bestEdge);
            visitedVertices.Add(bestEdge.To!);

            if (visitedVertices.Count == allVertices.Length)
                break;
        }

        return spanningTree.ToImmutableArray();
    }
}
