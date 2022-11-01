using System.Collections.Generic;

namespace InformationSystems.Graphs;

public interface IGraph<T>
{
    IEnumerable<T> GetVertices();

    IEnumerable<T> GetOutgoing(T vertex);

    float GetCost(T lhs, T rhs);
}
