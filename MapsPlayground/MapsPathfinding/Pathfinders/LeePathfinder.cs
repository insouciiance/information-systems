using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MapsPathfinding.Pathfinders;

public class LeePathfinder<TCell>
    : IGridPathfinder<TCell, LeePathfinder<TCell>.LeePathfinderResult>
    where TCell : ICell
{
    private readonly IGrid<TCell> _grid;

    public LeePathfinder(IGrid<TCell> grid)
    {
        _grid = grid;
    }

    public LeePathfinderResult GetPathResult(TCell start, TCell end)
    {
        Dictionary<int, Dictionary<TCell, TCell?>> pathGraph = new()
        {
            { 0, new() { { start, default } } }
        };

        return new LeePathfinderResult
        {
            Waves = pathGraph,
            Grid = _grid,
            Start = start,
            End = end,
            Path = PropagateWave(1) ? ConstructPath() : ImmutableArray<TCell>.Empty
        };

        bool PropagateWave(int wave)
        {
            if (!pathGraph.TryGetValue(wave - 1, out var previousWave))
                throw new KeyNotFoundException($"Unable to find the wave with index {wave - 1}.");

            ref Dictionary<TCell, TCell?>? waveGraph = ref CollectionsMarshal.GetValueRefOrAddDefault(pathGraph, wave, out _);
            waveGraph ??= new();

            foreach (var (cell, _) in previousWave)
            {
                ImmutableArray<TCell> adjacentCells = _grid.GetAdjacent(cell);

                foreach (var current in adjacentCells)
                {
                    if (current.IsBlocker || IsCellVisited(current) || waveGraph.ContainsKey(current))
                        continue;

                    waveGraph.Add(current, cell);

                    if (EqualityComparer<TCell>.Default.Equals(current, end))
                        return true;
                }
            }

            if (waveGraph.Count == 0)
                return false;

            return PropagateWave(wave + 1);
        }

        ImmutableArray<TCell> ConstructPath()
        {
            int wavesCount = pathGraph.Count;

            ImmutableArray<TCell>.Builder builder = ImmutableArray.CreateBuilder<TCell>(wavesCount);

            TCell current = end;

            while (!EqualityComparer<TCell>.Default.Equals(current, start))
            {
                builder.Insert(0, current);
                current = pathGraph[--wavesCount][current]!;
            }

            builder.Insert(0, current);

            return builder.MoveToImmutable();
        }

        bool IsCellVisited(TCell cell)
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

    public struct LeePathfinderResult : IGridPathfinderResult<TCell>
    {
        public Dictionary<int, Dictionary<TCell, TCell?>> Waves { get; init; }

        public IGrid<TCell> Grid { get; init; }

        public TCell Start { get; init; }

        public TCell End { get; init; }

        public ImmutableArray<TCell> Path { get; init; }
    }
}
