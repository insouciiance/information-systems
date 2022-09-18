using System.Collections.Generic;
using MapsPathfinding.Generators.Cells;

namespace MapsPathfinding.Generators.Grids;

public interface IGridGenerator<TGrid, TCell, TCellEnumerator, TCellGenerator>
    where TGrid : IGrid<TCell, TCellEnumerator>
    where TCell : ICell<TCell>
    where TCellEnumerator : IEnumerator<TCell>
    where TCellGenerator : ICellGenerator<TCell>
{
    static abstract TGrid Generate(int width, int height);
}
