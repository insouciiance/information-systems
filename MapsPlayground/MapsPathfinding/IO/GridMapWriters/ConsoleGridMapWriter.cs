using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapsPathfinding.IO.GridMapWriters;

public class ConsoleGridMapWriter : IGridMapWriter
{
    public void Write(GridOutputMap map)
    {
        for (int i = 0; i < map.Height; i++)
        {
            for (int j = 0; j < map.Width; j++)
            {
                if (map.TryGetCell(j, i, out var cell))
                {
                    Console.BackgroundColor = GetColorFromRGB(cell.Color);
                    Console.Write($"   ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    continue;
                }

                Console.Write($"   ");
            }

            Console.WriteLine();
        }
    }

    private static ConsoleColor GetColorFromRGB((int, int, int) color)
    {
        var (r, g, b) = color;

        ConsoleColor bestFit = ConsoleColor.Black;
        int bestFitDelta = r * r + g * g + b * b;

        foreach (var consoleColor in Enum.GetNames(typeof(ConsoleColor)))
        {
            Color colorFromConsole = Color.FromName(consoleColor);
            int colorDelta = (int)(Math.Pow(r - colorFromConsole.R, 2) + Math.Pow(g - colorFromConsole.G, 2) + Math.Pow(b - colorFromConsole.B, 2));
        
            if (colorDelta < bestFitDelta)
            {
                bestFit = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), consoleColor);
                bestFitDelta = colorDelta;
            }
        }

        return bestFit;
    }
}
