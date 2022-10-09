using System;
using System.Collections.Generic;
using MapsPathfinding.Generators.Cells;
using MapsPathfinding.Generators.Grids;
using MapsPathfinding.Utils;

namespace MapsPathfinding.Console;

public static class GenerationHelper
{
    public static (TGrid Grid, TCell Start, TCell End) GenerateGrid<TGrid, TCell, TGridGenerator, TCellGenerator>(int width, int height)
        where TGrid : IGrid<TCell>
        where TCell : ICell
        where TGridGenerator : IGridGenerator<TGrid, TCell, TCellGenerator>
        where TCellGenerator : ICellGenerator<TCell>
    {
        TGrid grid = TGridGenerator.Generate(width, height);

        double minDistance = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)) * 0.6f;
        
        TCell? start;
        TCell? end;

        do
        {
            while (true)
            {
                var (x, y) = GenerateRandomPosition();

                if (grid.TryGetCell(x, y, out start) && !start.IsBlocker)
                    break;
            }

            while (true)
            {
                var (x, y) = GenerateRandomPosition();

                if (grid.TryGetCell(x, y, out end) && !end.IsBlocker && !EqualityComparer<TCell>.Default.Equals(start, end))
                    break;
            }
        } while (IsDistanceSufficient());

        return (grid, start, end);

        (int X, int Y) GenerateRandomPosition() => (Randomizer.Instance.Next(width), Randomizer.Instance.Next(height));

        bool IsDistanceSufficient() => Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2)) < minDistance;
    }
}
