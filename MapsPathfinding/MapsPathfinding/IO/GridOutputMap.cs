namespace MapsPathfinding.IO;

public class GridOutputMap
{
    private GridMapCell?[,] _cells = new GridMapCell?[0, 0];

    public int Width { get; private set; }

    public int Height { get; private set; }

    public GridOutputMap() { }

    public GridOutputMap(int width, int height)
    {
        _cells = new GridMapCell?[height, width];
        Width = width;
        Height = height;
    }

    public void SetCell(int x, int y, GridMapCell cell)
    {
        ResizeIfNeeded(x, y);
        _cells[y, x] = cell;
    }

    public bool TryGetCell(int x, int y, out GridMapCell cell)
    {
        if (_cells[y, x] is GridMapCell notNull)
        {
            cell = notNull;
            return true;
        }

        cell = default;
        return false;
    }

    private void ResizeIfNeeded(int width, int height)
    {
        int newWidth = Width;
        int newHeight = Height;

        if (height >= Height)
            newHeight = height;

        if (width >= Width)
            newWidth = width;

        if ((newWidth, newHeight) == (Width, Height))
            return;

        GridMapCell?[,] newCells = new GridMapCell?[newHeight, newWidth];

        for (int i = 0; i < newHeight; i++)
        {
            for (int j = 0; j < newWidth; j++)
                newCells[i, j] = _cells[i, j];
        }

        Width = newWidth;
        Height = newHeight;
        _cells = newCells;
    }

    public readonly struct GridMapCell
    {
        public string Text { get; init; }

        public (byte R, byte G, byte B) Color { get; init; }
    }
}
