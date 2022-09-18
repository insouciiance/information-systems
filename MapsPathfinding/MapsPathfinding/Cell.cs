namespace MapsPathfinding;

public readonly struct Cell : ICell<Cell>
{
    public int X { get; }

    public int Y { get; }

    public bool IsBlocker { get; }

    public Cell(int x, int y, bool isBlocker = false)
    {
        X = x;
        Y = y;
        IsBlocker = isBlocker;
    }

    public static Cell CreateInstance(int x, int y) => new(x, y, false);
}
