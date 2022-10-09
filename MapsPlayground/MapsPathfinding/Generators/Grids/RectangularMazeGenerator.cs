using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MapsPathfinding.Generators.Cells;
using MapsPathfinding.Utils;

namespace MapsPathfinding.Generators.Grids;

public class RectangularMazeGenerator<TCell, TCellGenerator>
    : IGridGenerator<RectangularGrid<TCell>, TCell, TCellGenerator>
    where TCell : ICell
    where TCellGenerator : ICellGenerator<TCell>
{
    public static RectangularGrid<TCell> Generate(int width, int height)
    {
        if (width % 2 == 0 || height % 2 == 0)
            throw new InvalidOperationException("Can't create a maze for a rectangular grid with an even width / height.");

        Dictionary<(int X, int Y), bool> cells = new();

        Dictionary<(int X, int Y), bool> walls = new();

        HashSet<(int X, int Y)> wallList = new();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (i % 2 == 1 || j % 2 == 1)
                {
                    walls.Add((j, i), true);
                    continue;
                }

                cells.Add((j, i), false);
            }
        }

        var (startX, startY) = GenerateRandomCell();
        cells[(startX, startY)] = true;

        wallList.UnionWith(GetAdjacentCells(startX, startY, walls));

        while (wallList.Count > 0)
        {
            var wall = wallList.ElementAt(Randomizer.Instance.Next(0, wallList.Count));

            var adjacentCells = GetAdjacentCells(wall.X, wall.Y, cells);

            if (adjacentCells.Count(c => cells[(c.X, c.Y)]) == 1)
            {
                walls[wall] = false;

                var unvisitedCell = adjacentCells.First(c => !cells[(c.X, c.Y)]);
                cells[unvisitedCell] = true;

                wallList.UnionWith(GetAdjacentCells(unvisitedCell.X, unvisitedCell.Y, walls));
            }

            wallList.Remove(wall);
        }

        //AddPassages();
        CleanseWalls();

        return new RectangularGrid<TCell>(
            width,
            height,
            cells.Keys.Select(c => TCellGenerator.Generate(c.X, c.Y, false)).Union(walls.Select(w => TCellGenerator.Generate(w.Key.X, w.Key.Y, w.Value))));

        (int X, int Y) GenerateRandomCell()
        {
            return (Randomizer.Instance.Next(width / 2) * 2, Randomizer.Instance.Next(height / 2) * 2);
        }

        List<(int X, int Y)> GetAdjacentCells(int x, int y, Dictionary<(int X, int Y), bool> cells)
        {
            List<(int, int)> adjacent = new();

            if (cells.ContainsKey((x, y - 1)))
                adjacent.Add((x, y - 1));

            if (cells.ContainsKey((x, y + 1)))
                adjacent.Add((x, y + 1));

            if (cells.ContainsKey((x - 1, y)))
                adjacent.Add((x - 1, y));

            if (cells.ContainsKey((x + 1, y)))
                adjacent.Add((x + 1, y));

            return adjacent;
        }

        void CleanseWalls(int iterationsCount = 1)
        {
            for (int i = 0; i < iterationsCount; i++)
                CleanseInternal();

            void CleanseInternal()
            {
                HashSet<(int, int)> wallsToRemove = new();
             
                foreach (var (key, value) in walls)
                {
                    if (!value)
                        continue;

                    if (GetAdjacentCells(key.X, key.Y, walls).Count(c => walls[c]) <= 1)
                        wallsToRemove.Add(key);
                }

                foreach (var wallCell in wallsToRemove)
                    walls[wallCell] = false;
            }
        }

        void AddPassages()
        {
            for (int i = 0; i < width; i++)
            {
                int y = Randomizer.Instance.Next(height);

                if (walls.TryGetValue((y, i), out bool active) || !active)
                    continue;

                walls[(y, i)] = false;
            }

            for (int j = 0; j < height; j++)
            {
                int x = Randomizer.Instance.Next(width);

                if (walls.TryGetValue((j, x), out bool active) || !active)
                    continue;

                walls[(j, x)] = false;
            }
        }
    }
}
