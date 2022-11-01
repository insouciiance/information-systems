using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace InformationSystems.Graphs.Pathfinders;

public class LeePathfinder<T, TGraph>
    : IPathfinder<T, TGraph, LeePathfinder<T, TGraph>.LeePathfinderResult>
    where T : notnull
    where TGraph : IGraph<T>
{
    private readonly TGraph _graph;

    private readonly T _start;

    private readonly T _end;

    public LeePathfinder(TGraph graph, T start, T end)
    {
        _graph = graph;
        _start = start;
        _end = end;
    }

    public LeePathfinderResult GetPathResult()
    {
        Dictionary<int, Dictionary<T, T?>> pathGraph = new()
        {
            { 0, new() { { _start, default } } }
        };

        return new LeePathfinderResult
        {
            Waves = pathGraph,
            Graph = _graph,
            Start = _start,
            End = _end,
            Path = PropagateWave(1) ? ConstructPath() : ImmutableArray<T>.Empty
        };

        bool PropagateWave(int wave)
        {
            if (!pathGraph.TryGetValue(wave - 1, out var previousWave))
                throw new KeyNotFoundException($"Unable to find the wave with index {wave - 1}.");

            ref Dictionary<T, T?>? waveGraph = ref CollectionsMarshal.GetValueRefOrAddDefault(pathGraph, wave, out _);
            waveGraph ??= new();

            foreach (var (cell, _) in previousWave)
            {
                IEnumerable<T> adjacentCells = _graph.GetOutgoing(cell);

                foreach (var current in adjacentCells)
                {
                    if (IsCellVisited(current) || waveGraph.ContainsKey(current))
                        continue;

                    waveGraph.Add(current, cell);

                    if (EqualityComparer<T>.Default.Equals(current, _end))
                        return true;
                }
            }

            if (waveGraph.Count == 0)
                return false;

            return PropagateWave(wave + 1);
        }

        ImmutableArray<T> ConstructPath()
        {
            int wavesCount = pathGraph.Count;

            ImmutableArray<T>.Builder builder = ImmutableArray.CreateBuilder<T>(wavesCount);

            T current = _end;

            while (!EqualityComparer<T>.Default.Equals(current, _start))
            {
                builder.Insert(0, current);
                current = pathGraph[--wavesCount][current]!;
            }

            builder.Insert(0, current);

            return builder.MoveToImmutable();
        }

        bool IsCellVisited(T cell)
        {
            if (pathGraph[0].ContainsKey(cell))
                return true;

            foreach (var waveGraph in pathGraph.Values)
            {
                if (waveGraph.ContainsKey(cell))
                    return true;
            }

            return false;
        }
    }

    public struct LeePathfinderResult : ISinglePathPathfinderResult<T, TGraph>
    {
        public Dictionary<int, Dictionary<T, T?>> Waves { get; init; }

        public TGraph Graph { get; init; }

        public T Start { get; init; }

        public T End { get; init; }

        public ImmutableArray<T> Path { get; init; }
    }
}
