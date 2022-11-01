using InformationSystems.Graphs.Generators.Cells;
using InformationSystems.Graphs.Generators.Grids;
using InformationSystems.Graphs.IO;
using InformationSystems.Graphs.IO.GridMapWriters;
using InformationSystems.Graphs.Pathfinders;

namespace InformationSystems.Graphs.Console;

public static class GridPathfinderRunner
{
    public static void Run()
    {
        var (grid, start, end) = GenerationHelper.GenerateGrid<RectangularGrid<Cell>, Cell,
    RectangularMazeGenerator<Cell, CellGenerator>, CellGenerator>(11, 11);

        do
        {
            var leeResult = new LeePathfinder<Cell, RectangularGrid<Cell>>(grid, start, end).GetPathResult();
            GridOutputMap leeMap = LeePathfinderResultWriter<Cell, RectangularGrid<Cell>>.Write(leeResult);

            var aStarResult = new AStarPathfinder<Cell, RectangularGrid<Cell>>(grid, start, end, RectangularGrid<Cell>.DefaultHeuristic).GetPathResult();
            GridOutputMap aStarMap = DefaultPathfinderResultWriter<Cell, RectangularGrid<Cell>>.Write(aStarResult);

            new BitmapGridMapWriter("LeePathfinder").Write(leeMap);
            new BitmapGridMapWriter("AStarPathfinder").Write(aStarMap);
            new FileGridMapWriter("LeePathfinder").Write(leeMap);
            new FileGridMapWriter("AStarPathfinder").Write(aStarMap);
            new ConsoleGridMapWriter().Write(leeMap);
            new ConsoleGridMapWriter().Write(aStarMap);

            int xStart = int.Parse(System.Console.ReadLine()!);
            int yStart = int.Parse(System.Console.ReadLine()!);

            int xEnd = int.Parse(System.Console.ReadLine()!);
            int yEnd = int.Parse(System.Console.ReadLine()!);

            grid.TryGetCell(xStart, yStart, out start);
            grid.TryGetCell(xEnd, yEnd, out end);
        } while (true);

    }
}
