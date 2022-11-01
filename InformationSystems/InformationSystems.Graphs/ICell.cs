namespace InformationSystems.Graphs;

public interface ICell
{
    int X { get; }
    
    int Y { get; }

    bool IsBlocker { get; }
}
