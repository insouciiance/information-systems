using System.Collections.Generic;
using MapsPathfinding.Pathfinders;

namespace MapsPathfinding.IO;

public class LeePathfinderResultWriter<TGrid, TCell, TCellEnumerator>
    : IGridPathfinderResultWriter<LeePathfinder<TGrid, TCell, TCellEnumerator>.LeePathfinderResult, TGrid, TCell, TCellEnumerator>
    where TGrid : IGrid<TCell, TCellEnumerator>
    where TCell : ICell<TCell>
    where TCellEnumerator : IEnumerator<TCell>
{
    public static GridOutputMap Write(LeePathfinder<TGrid, TCell, TCellEnumerator>.LeePathfinderResult result)
    {
        GridOutputMap writer = new(result.Grid.Width, result.Grid.Height);

        foreach (var (wave, values) in result.Waves)
        {
            foreach (var (cell, _) in values)
                writer.SetCell(cell.X, cell.Y, GetWaveCell());

            GridOutputMap.GridMapCell GetWaveCell()
            {
                float opacity = (float)wave / result.Path.Length;

                return new()
                {
                    Text = wave.ToString(),
                    Color = (0, (byte)(50 + 205 * opacity), (byte)(50 + 205 * opacity))
                };
            }
        }

        ResultWriterHelper.WriteResultBase(result, writer);

        return writer;
    }
}
