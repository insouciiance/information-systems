namespace MapsPathfinding.Generators.Cells;

public interface ICellGenerator<TCell>
    where TCell : ICell<TCell>
{
    static abstract TCell Generate(int x, int y);
}
