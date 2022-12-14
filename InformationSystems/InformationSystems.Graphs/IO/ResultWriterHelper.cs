using InformationSystems.Graphs.Pathfinders;

namespace InformationSystems.Graphs.IO;

public static class ResultWriterHelper
{
    public static void WriteResultBase<TCell, TGrid>(ISinglePathPathfinderResult<TCell, TGrid> result, GridOutputMap map)
        where TCell : ICell
        where TGrid : IGrid<TCell>
    {
        for (int i = 0; i < result.Graph.Height; i++)
        {
            for (int j = 0; j < result.Graph.Width; j++)
            {
                if (!result.Graph.TryGetCell(j, i, out var cell) || map.TryGetCell(j, i, out _))
                    continue;

                map.SetCell(j, i, GetDefaultCell(cell));
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
                Color = (255, 165, 0)
            };

            float GetPathSliceDistance()
            {
                float distance = 0;

                for (int i = 1; i <= result.Path.IndexOf(current); i++)
                    distance += result.Graph.GetCost(result.Path[i - 1], result.Path[i]);

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
