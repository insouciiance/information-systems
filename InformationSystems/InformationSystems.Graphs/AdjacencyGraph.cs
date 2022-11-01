using System;
using System.Collections.Generic;
using System.Linq;

namespace InformationSystems.Graphs;

public class AdjacencyGraph<T> : IGraph<T>
{
    private readonly float?[,] _adjacencyMatrix;

    private readonly T[] _vertices;

    public AdjacencyGraph(params (T From, T To, float Cost)[] edges)
    {
        HashSet<T> vertices = new();

        foreach (var (from, to, _) in edges)
        {
            vertices.Add(from);
            vertices.Add(to);
        }

        _vertices = vertices.ToArray();
        _adjacencyMatrix = new float?[_vertices.Length, _vertices.Length];

        foreach (var (from, to, cost) in edges)
            _adjacencyMatrix[Array.IndexOf(_vertices, from), Array.IndexOf(_vertices, to)] = cost;
   
    }

    public float GetCost(T lhs, T rhs)
    {
        int lhsIndex = Array.IndexOf(_vertices, lhs);
        int rhsIndex = Array.IndexOf(_vertices, rhs);

        if (lhsIndex == rhsIndex)
            return 0;

        return _adjacencyMatrix[lhsIndex, rhsIndex] ?? float.PositiveInfinity;
    }

    public IEnumerable<T> GetOutgoing(T vertex)
    {
        int index = Array.IndexOf(_vertices, vertex);

        for (int i = 0; i < _vertices.Length; i++)
        {
            if (_adjacencyMatrix[index, i] is not null)
                yield return _vertices[i];
        }
    }

    public IEnumerable<T> GetVertices() => _vertices;
}
