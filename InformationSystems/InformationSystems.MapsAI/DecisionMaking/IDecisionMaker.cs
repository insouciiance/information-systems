using System.Collections.Generic;
using System.Collections.Immutable;
using InformationSystems.Graphs;

namespace InformationSystems.MapsAI.DecisionMaking;

public interface IDecisionMaker<TCell>
    where TCell : ICell
{
    GameBoard<TCell> Board { get; }

    IEnumerable<TCell> GetPossibleMoves(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null);

    TCell MoveNext(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null);
}
