namespace MapsPathfinding.Generators.Cells;

public class CellGenerator : ICellGenerator<Cell>
{
    public static Cell Generate(int x, int y, bool isBlocker)
    {
        return new Cell(x, y, isBlocker);
    }
}
