using System.Collections.Generic;
using InformationSystems.MapsPathfinding.Generators.Cells;
using InformationSystems.Shared.Utils;

namespace InformationSystems.MapsPathfinding.Generators.Grids;

public class RectangularGridGenerator<TCell, TCellGenerator>
    : IGridGenerator<RectangularGrid<TCell>, TCell, TCellGenerator>
    where TCell : ICell
    where TCellGenerator : ICellGenerator<TCell>
{
    public static RectangularGrid<TCell> Generate(int width, int height)
    {
        List<TCell> cells = new();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                bool isBlocker = Randomizer.Instance.Next(0, 5) <= 1;
                cells.Add(TCellGenerator.Generate(j, i, isBlocker));
            }
        }

        return new RectangularGrid<TCell>(width, height, cells);
    }
}
