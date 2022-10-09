using System.Collections.Immutable;
using System.Collections.Generic;
using MapsPathfinding;

namespace MapsAI.DecisionMaking;

public interface IDecisionMaker<TCell>
    where TCell : ICell
{
    GameBoard<TCell> Board { get; }

    ImmutableArray<TCell> GetPossibleMoves(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null);

    TCell MoveNext(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null);
}
