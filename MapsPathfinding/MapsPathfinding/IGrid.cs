using System.Collections.Generic;

namespace MapsPathfinding;

public interface IGrid<TCell, TCellEnumerator>
    where TCell : ICell<TCell>
    where TCellEnumerator : IEnumerator<TCell>
{
    int Width { get; }

    int Height { get; }

    TCell GetCell(int x, int y);

    TCellEnumerator GetAdjacent(TCell cell);

    float GetCost(TCell lhs, TCell rhs);
}
