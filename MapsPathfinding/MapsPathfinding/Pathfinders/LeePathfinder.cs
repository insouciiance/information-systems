using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MapsPathfinding.Pathfinders;

public class LeePathfinder<TGrid, TCell, TCellEnumerator>
    : IGridPathfinder<TGrid, TCell, TCellEnumerator, LeePathfinder<TGrid, TCell, TCellEnumerator>.LeePathfinderResult>
    where TGrid : IGrid<TCell, TCellEnumerator>
    where TCell : ICell<TCell>
    where TCellEnumerator : IEnumerator<TCell>
{
    private TGrid _grid;

    public LeePathfinder(TGrid grid)
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
                TCellEnumerator adjacentCellsEnumerator = _grid.GetAdjacent(cell);

                while (adjacentCellsEnumerator.MoveNext())
                {
                    TCell current = adjacentCellsEnumerator.Current;

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

    public struct LeePathfinderResult : IGridPathfinderResult<TGrid, TCell, TCellEnumerator>
    {
        public Dictionary<int, Dictionary<TCell, TCell?>> Waves { get; init; }

        public TGrid Grid { get; init; }

        public TCell Start { get; init; }

        public TCell End { get; init; }

        public ImmutableArray<TCell> Path { get; init; }
    }
}
