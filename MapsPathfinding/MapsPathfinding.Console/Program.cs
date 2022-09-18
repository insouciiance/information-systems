﻿using MapsPathfinding;
using MapsPathfinding.Console;
using MapsPathfinding.Generators.Cells;
using MapsPathfinding.Generators.Grids;
using MapsPathfinding.IO;
using MapsPathfinding.IO.GridMapWriters;
using MapsPathfinding.Pathfinders;

var (grid, start, end) = GenerationHelper.GenerateGrid<RectangularGrid<Cell>, Cell, RectangularGrid<Cell>.Enumerator,
    RectangularGridGenerator<Cell, CellGenerator>, CellGenerator>(100, 100);

var leeResult = new LeePathfinder<RectangularGrid<Cell>, Cell, RectangularGrid<Cell>.Enumerator>(grid).GetPathResult(start, end);
GridOutputMap leeMap = LeePathfinderResultWriter<RectangularGrid<Cell>, Cell, RectangularGrid<Cell>.Enumerator>.Write(leeResult);

var aStarResult = new AStarPathfinder<RectangularGrid<Cell>, Cell, RectangularGrid<Cell>.Enumerator>(grid).GetPathResult(start, end);
GridOutputMap aStarMap = AStarPathfinderResultWriter<RectangularGrid<Cell>, Cell, RectangularGrid<Cell>.Enumerator>.Write(aStarResult);

new BitmapGridMapWriter("LeePathfinder").Write(leeMap);
new BitmapGridMapWriter("AStarPathfinder").Write(aStarMap);
new FileGridMapWriter("LeePathfinder").Write(leeMap);
new FileGridMapWriter("AStarPathfinder").Write(aStarMap);
new ConsoleGridMapWriter().Write(leeMap);
new ConsoleGridMapWriter().Write(aStarMap);