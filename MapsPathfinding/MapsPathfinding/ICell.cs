namespace MapsPathfinding;

public interface ICell<TCell>
    where TCell : ICell<TCell>
{
    int X { get; }
    
    int Y { get; }

    bool IsBlocker { get; }

    static abstract TCell CreateInstance(int x, int y);
}
