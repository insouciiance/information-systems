using System;

namespace MapsAI.Extensions;

public static class PlayerKindExtensions
{
    public static PlayerKind Inverse(this PlayerKind kind)
    {
        return kind switch
        {
            PlayerKind.Ally => PlayerKind.Opponent,
            PlayerKind.Opponent => PlayerKind.Ally,
            _ => throw new InvalidOperationException($"Unvalid {nameof(PlayerKind)}: {kind}.")
        };
    }
}
