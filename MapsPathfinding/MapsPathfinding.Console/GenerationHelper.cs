using System;
using System.Collections.Generic;
using MapsPathfinding.Generators.Cells;
using MapsPathfinding.Generators.Grids;

namespace MapsPathfinding.Console;

public static class GenerationHelper
{
    private static readonly Random _random = new();

    public static (TGrid Grid, TCell Start, TCell End) GenerateGrid<TGrid, TCell, TCellEnumerator, TGridGenerator, TCellGenerator>(int width, int height)
        where TGrid : IGrid<TCell, TCellEnumerator>
        where TCell : ICell<TCell>
        where TCellEnumerator : IEnumerator<TCell>
        where TGridGenerator : IGridGenerator<TGrid, TCell, TCellEnumerator, TCellGenerator>
        where TCellGenerator : ICellGenerator<TCell>
    {
        TGrid grid = TGridGenerator.Generate(width, height);

        double minDistance = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)) * 0.6f;
        
        TCell start;
        TCell end;

        do
        {
            while (true)
            {
                var (x, y) = GenerateRandomPosition();

                if (!(start = grid.GetCell(x, y)).IsBlocker)
                    break;
            }

            while (true)
            {
                var (x, y) = GenerateRandomPosition();

                if (!(end = grid.GetCell(x, y)).IsBlocker && !EqualityComparer<TCell>.Default.Equals(start, end))
                    break;
            }
        } while (IsDistanceSufficient());

        return (grid, start, end);

        (int X, int Y) GenerateRandomPosition() => (_random.Next(width), _random.Next(height));

        bool IsDistanceSufficient() => Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2)) < minDistance;
    }
}
