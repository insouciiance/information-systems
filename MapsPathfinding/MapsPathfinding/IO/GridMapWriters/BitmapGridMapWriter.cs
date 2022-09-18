using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MapsPathfinding.IO.GridMapWriters;

public class BitmapGridMapWriter : IGridMapWriter
{
    public const int PIXELS_COUNT = 1920 * 1080;

    private readonly string _fileName;

    private readonly ImageFormat _imageFormat;

    public BitmapGridMapWriter(string fileName, ImageFormat? format = null)
    {
        if (!OperatingSystem.IsOSPlatform("Windows"))
            throw new NotSupportedException("Unable to use bitmap output for non-Windows OS.");

        _imageFormat = format ?? ImageFormat.Png;
        _fileName = fileName;
    }

    public void Write(GridOutputMap map)
    {
        if (!OperatingSystem.IsOSPlatform("Windows"))
            throw new NotSupportedException("Unable to use bitmap output for non-Windows OS.");

        var (width, height, blockSize) = GetBitmapSize(map);

        Bitmap bitmap = new(width, height);

        for (int i = 0; i < map.Height; i++)
        {
            for (int j = 0; j < map.Width; j++)
                SetBlock(i, j);
        }

        string extension = _imageFormat.ToString().ToLowerInvariant();
        using var stream = File.OpenWrite($"{_fileName}.{extension}");
        bitmap.Save(stream, ImageFormat.Jpeg);

        void SetBlock(int x, int y)
        {
            if (!map.TryGetCell(x, y, out var cell))
                return;

            for (int i = blockSize * x; i < blockSize * (x + 1); i++)
            {
                for (int j = blockSize * y; j < blockSize * (y + 1); j++)
                    bitmap.SetPixel(j, i, Color.FromArgb(cell.Color.R, cell.Color.G, cell.Color.B));
            }
        }
    }

    private static (int Width, int Height, int BlockSize) GetBitmapSize(GridOutputMap map)
    {
        float ratio = (float)map.Width / map.Height;
        int widthPixels = (int)Math.Sqrt(PIXELS_COUNT * ratio);
        int heightPixels = PIXELS_COUNT / widthPixels;
        return (widthPixels, heightPixels, Math.Min(widthPixels / map.Width, heightPixels / map.Height));
    }
}
