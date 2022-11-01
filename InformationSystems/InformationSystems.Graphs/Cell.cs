using System.Diagnostics.CodeAnalysis;

namespace InformationSystems.Graphs;

public readonly struct Cell : ICell
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

    public override int GetHashCode()
    {
        return X ^ Y;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Cell cell && X == cell.X && Y == cell.Y;
    }
}
