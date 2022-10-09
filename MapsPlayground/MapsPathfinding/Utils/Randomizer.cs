using System;

namespace MapsPathfinding.Utils;

public class Randomizer : Singleton<Random>
{
    private readonly Random Random = new();

    public int Next(int minInclusive, int maxExclusive) => Random.Next(minInclusive, maxExclusive);

    public double NextDouble() => Random.NextDouble();
}
