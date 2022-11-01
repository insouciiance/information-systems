using InformationSystems.Graphs.Pathfinders;

namespace InformationSystems.Graphs.Extensions;

public static class PathfinderResultExtensions
{
    public static float Evaluate<T, TGraph>(this ISinglePathPathfinderResult<T, TGraph> result)
        where TGraph : IGraph<T>
    {
        float cost = 0;

        for (int i = 0; i < result.Path.Length - 1; i++)
            cost += result.Graph.GetCost(result.Path[i], result.Path[i + 1]);

        return cost;
    }
}
