using System;

namespace MapsPathfinding.Generators.Cells;

public class CellGenerator : ICellGenerator<Cell>
{
    private static readonly Random _random = new();

    public static Cell Generate(int x, int y)
    {
        bool isBlocker = _random.Next(0, 5) <= 0;
        return new Cell(x, y, isBlocker);
    }
}
