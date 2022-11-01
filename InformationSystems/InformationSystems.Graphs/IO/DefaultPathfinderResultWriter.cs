using InformationSystems.Graphs.Pathfinders;

namespace InformationSystems.Graphs.IO;

public class DefaultPathfinderResultWriter<TCell, TGrid>
    : IPathfinderResultWriter<TCell, TGrid, DefaultSinglePathPathfinderResult<TCell, TGrid>>
    where TCell : ICell
    where TGrid : IGrid<TCell>
{
    public static GridOutputMap Write(DefaultSinglePathPathfinderResult<TCell, TGrid> result)
    {
        GridOutputMap writer = new(result.Graph.Width, result.Graph.Height);
        ResultWriterHelper.WriteResultBase(result, writer);
        return writer;
    }
}
