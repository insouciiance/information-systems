using System.IO;

namespace MapsPathfinding.IO.GridMapWriters;

public class FileGridMapWriter : IGridMapWriter
{
    public const string DEFAULT_EXTENSION = ".grid";

    private readonly string _extension;

    private readonly string _fileName;

    public FileGridMapWriter(string fileName, string extension = DEFAULT_EXTENSION)
    {
        _fileName = fileName;
        _extension = extension;
    }

    public void Write(GridOutputMap map)
    {
        StringBuilderMapWriter builder = new();
        builder.Write(map);

        using var stream = File.OpenWrite($"{_fileName}.{_extension}");
        using var writer = new StreamWriter(stream);
        writer.Write(builder.Builder);
    }
}
