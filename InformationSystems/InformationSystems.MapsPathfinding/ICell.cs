namespace InformationSystems.MapsPathfinding;

public interface ICell
{
    int X { get; }
    
    int Y { get; }

    bool IsBlocker { get; }
}
