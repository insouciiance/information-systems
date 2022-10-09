using MapsPathfinding.Pathfinders;

namespace MapsPathfinding.Extensions;

public static class PathfinderResultExtensions
{
    public static float Evaluate<TCell>(this IGridPathfinderResult<TCell> result)
        where TCell : ICell
    {
        float cost = 0;

        for (int i = 0; i < result.Path.Length - 1; i++)
            cost += result.Grid.GetCost(result.Path[i], result.Path[i + 1]);

        return cost;
    }
}
