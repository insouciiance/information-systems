using InformationSystems.MapsPathfinding.Generators.Cells;

namespace InformationSystems.MapsPathfinding.Generators.Grids;

public interface IGridGenerator<TGrid, TCell, TCellGenerator>
    where TGrid : IGrid<TCell>
    where TCell : ICell
    where TCellGenerator : ICellGenerator<TCell>
{
    static abstract TGrid Generate(int width, int height);
}
