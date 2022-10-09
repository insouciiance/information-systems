using System.Text;

namespace MapsPathfinding.IO.GridMapWriters;

public class StringBuilderMapWriter : IGridMapWriter
{
    public StringBuilder Builder { get; } = new();

    public void Write(GridOutputMap map)
    {
        for (int i = 0; i < map.Height; i++)
        {
            for (int j = 0; j < map.Width; j++)
            {
                if (map.TryGetCell(j, i, out var cell))
                {
                    Builder.Append($"{cell.Text, -5}");
                    continue;
                }

                Builder.Append("     ");
            }

            Builder.AppendLine();
            Builder.AppendLine();
        }
    }
}
