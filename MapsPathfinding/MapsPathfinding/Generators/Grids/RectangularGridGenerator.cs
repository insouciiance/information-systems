using System.Collections.Generic;
using MapsPathfinding.Generators.Cells;

namespace MapsPathfinding.Generators.Grids;

public class RectangularGridGenerator<TCell, TCellGenerator>
    : IGridGenerator<RectangularGrid<TCell>, TCell, RectangularGrid<TCell>.Enumerator, TCellGenerator>
    where TCell : ICell<TCell>
    where TCellGenerator : ICellGenerator<TCell>
{
    public static RectangularGrid<TCell> Generate(int width, int height)
    {
        List<TCell> cells = new();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
                cells.Add(TCellGenerator.Generate(j, i));
        }

        return new RectangularGrid<TCell>(width, height, cells);
    }
}
