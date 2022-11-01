using InformationSystems.Graphs.Pathfinders;

namespace InformationSystems.Graphs.IO;

public class LeePathfinderResultWriter<TCell, TGrid>
    : IPathfinderResultWriter<TCell, TGrid, LeePathfinder<TCell, TGrid>.LeePathfinderResult>
    where TCell : ICell
    where TGrid : IGrid<TCell>
{
    public static GridOutputMap Write(LeePathfinder<TCell, TGrid>.LeePathfinderResult result)
    {
        GridOutputMap writer = new(result.Graph.Width, result.Graph.Height);

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
