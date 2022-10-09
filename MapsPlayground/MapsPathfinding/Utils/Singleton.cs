﻿namespace MapsPathfinding.Utils;

public abstract class Singleton<T>
    where T : class, new()
{
    public static T Instance { get; } = new();
}
