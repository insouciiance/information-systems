namespace MapsPathfinding.Generators.Cells;

public interface ICellGenerator<TCell>
    where TCell : ICell
{
    static abstract TCell Generate(int x, int y, bool isBlocker);
}
