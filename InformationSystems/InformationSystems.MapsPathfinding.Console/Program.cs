using System;
using InformationSystems.MapsPathfinding;
using InformationSystems.MapsPathfinding.Console;
using InformationSystems.MapsPathfinding.Generators.Cells;
using InformationSystems.MapsPathfinding.Generators.Grids;
using InformationSystems.MapsPathfinding.IO;
using InformationSystems.MapsPathfinding.IO.GridMapWriters;
using InformationSystems.MapsPathfinding.Pathfinders;

var (grid, start, end) = GenerationHelper.GenerateGrid<RectangularGrid<Cell>, Cell,
    RectangularMazeGenerator<Cell, CellGenerator>, CellGenerator>(11, 11);

do
{
    var leeResult = new LeePathfinder<Cell>(grid).GetPathResult(start, end);
    GridOutputMap leeMap = LeePathfinderResultWriter<Cell>.Write(leeResult);

    var aStarResult = new AStarPathfinder<Cell>(grid).GetPathResult(start, end);
    GridOutputMap aStarMap = AStarPathfinderResultWriter<Cell>.Write(aStarResult);

    new BitmapGridMapWriter("LeePathfinder").Write(leeMap);
    new BitmapGridMapWriter("AStarPathfinder").Write(aStarMap);
    new FileGridMapWriter("LeePathfinder").Write(leeMap);
    new FileGridMapWriter("AStarPathfinder").Write(aStarMap);
    new ConsoleGridMapWriter().Write(leeMap);
    new ConsoleGridMapWriter().Write(aStarMap);

    int xStart = int.Parse(Console.ReadLine()!);
    int yStart = int.Parse(Console.ReadLine()!);

    int xEnd = int.Parse(Console.ReadLine()!);
    int yEnd = int.Parse(Console.ReadLine()!);

    grid.TryGetCell(xStart, yStart, out start);
    grid.TryGetCell(xEnd, yEnd, out end);
} while(true);
