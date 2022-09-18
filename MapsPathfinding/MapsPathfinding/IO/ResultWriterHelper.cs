using System.Collections.Generic;
using MapsPathfinding.Pathfinders;

namespace MapsPathfinding.IO;

public static class ResultWriterHelper
{
    public static void WriteResultBase<TGrid, TCell, TCellEnumerator>(IGridPathfinderResult<TGrid, TCell, TCellEnumerator> result, GridOutputMap map)
        where TGrid : IGrid<TCell, TCellEnumerator>
        where TCell : ICell<TCell>
        where TCellEnumerator : IEnumerator<TCell>
    {
        for (int i = 0; i < result.Grid.Height; i++)
        {
            for (int j = 0; j < result.Grid.Width; j++)
            {
                TCell cell = result.Grid.GetCell(j, i);

                if (map.TryGetCell(i, j, out _))
                    continue;

                map.SetCell(i, j, GetDefaultCell(cell));
            }
        }

        for (int i = 0; i < result.Path.Length; i++)
        {
            TCell cell = result.Path[i];
            map.SetCell(cell.X, cell.Y, GetPathCell(cell));
        }

        map.SetCell(result.Start.X, result.Start.Y, GetStartCell());
        map.SetCell(result.End.X, result.End.Y, GetEndCell());

        GridOutputMap.GridMapCell GetPathCell(TCell current)
        {
            return new()
            {
                Text = GetPathSliceDistance().ToString("F2"),
                Color = (255, 192, 203)
            };

            float GetPathSliceDistance()
            {
                float distance = 0;

                for (int i = 1; i <= result.Path.IndexOf(current); i++)
                    distance += result.Grid.GetCost(result.Path[i - 1], result.Path[i]);

                return distance;
            }
        }

        GridOutputMap.GridMapCell GetDefaultCell(TCell cell)
        {
            if (cell.IsBlocker)
            {
                return new()
                {
                    Text = "X",
                    Color = (0, 0, 0)
                };
            }

            return new()
            {
                Text = ".",
                Color = (255, 255, 255)
            };
        }

        GridOutputMap.GridMapCell GetStartCell()
        {
            return new()
            {
                Text = "s",
                Color = (0, 255, 0)
            };
        }

        GridOutputMap.GridMapCell GetEndCell()
        {
            return new()
            {
                Text = "e",
                Color = (255, 0, 0)
            };
        }
    }
}
