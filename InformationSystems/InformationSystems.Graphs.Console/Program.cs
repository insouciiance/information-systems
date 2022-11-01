using InformationSystems.Graphs;
using InformationSystems.Graphs.Extensions;
using InformationSystems.Graphs.Pathfinders;

IGraph<int> graph = new AdjacencyGraph<int>(
    (1, 8, 1),
    (2, 1, 1),
    (2, 3, 3),
    (2, 4, 3),
    (3, 1, 3),
    (3, 6, 2),
    (4, 1, 5),
    (4, 3, 3),
    (5, 6, 5),
    (5, 7, 6),
    (5, 8, 3),
    (6, 2, 1),
    (6, 3, 3),
    (6, 7, 2),
    (6, 4, 2),
    (7, 3, 3),
    (7, 6, 1),
    (7, 8, 2),
    (8, 3, 2),
    (8, 4, 2),
    (8, 7, 4));

DijkstraPathfinder<int, IGraph<int>> pathfinder = new(graph, 1);

foreach (var path in pathfinder.GetPathResult().Paths)
{
    System.Console.WriteLine($"{path.Start} - {path.End}: {string.Join("-", path.Path)} ({path.Evaluate()})");
}
