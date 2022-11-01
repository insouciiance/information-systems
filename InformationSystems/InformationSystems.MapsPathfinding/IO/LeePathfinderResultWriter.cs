using InformationSystems.MapsPathfinding.Pathfinders;

namespace InformationSystems.MapsPathfinding.IO;

public class LeePathfinderResultWriter<TCell>
    : IGridPathfinderResultWriter<LeePathfinder<TCell>.LeePathfinderResult, TCell>
    where TCell : ICell
{
    public static GridOutputMap Write(LeePathfinder<TCell>.LeePathfinderResult result)
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
