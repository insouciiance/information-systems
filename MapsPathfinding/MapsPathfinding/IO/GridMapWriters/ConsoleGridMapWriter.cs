using System;

namespace MapsPathfinding.IO.GridMapWriters;

public class ConsoleGridMapWriter : IGridMapWriter
{
    public void Write(GridOutputMap map)
    {
        StringBuilderMapWriter builder = new();
        builder.Write(map);
        Console.WriteLine(builder.Builder);
    }
}
