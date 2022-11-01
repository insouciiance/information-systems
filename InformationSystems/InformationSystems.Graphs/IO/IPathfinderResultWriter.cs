using InformationSystems.Graphs.Pathfinders;

namespace InformationSystems.Graphs.IO;

public interface IPathfinderResultWriter<T, TGraph, TPathfinderResult>
    where TGraph : IGraph<T>
    where TPathfinderResult : ISinglePathPathfinderResult<T, TGraph>
{
    static abstract GridOutputMap Write(TPathfinderResult result);
}
