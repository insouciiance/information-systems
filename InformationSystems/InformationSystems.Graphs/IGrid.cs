using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace InformationSystems.Graphs;

public interface IGrid<TCell> : IGraph<TCell>
    where TCell : ICell
{
    int Width { get; }

    int Height { get; }

    bool TryGetCell(int x, int y, [MaybeNullWhen(false)] out TCell cell);

    IEnumerable<TCell> IGraph<TCell>.GetVertices()
    {
        for (int i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                if (TryGetCell(i, j, out var cell))
                    yield return cell;
            }
        }
    }
}
