using System.Collections.Generic;
using System.Collections.Immutable;
using InformationSystems.Graphs;
using InformationSystems.Graphs.Extensions;
using InformationSystems.Shared.Utils;

namespace InformationSystems.MapsAI.DecisionMaking;

public class RandomDecisionMaker<TCell> : IDecisionMaker<TCell>
    where TCell : ICell
{
    public GameBoard<TCell> Board { get; }

    public RandomDecisionMaker(GameBoard<TCell> board)
    {
        Board = board;
    }

    public IEnumerable<TCell> GetPossibleMoves(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        return Board.Grid.GetAdjacentNonBlockers(cell);
    }

    public TCell MoveNext(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        var adjacentCells = Board.Grid.GetAdjacentNonBlockers(cell);
        return adjacentCells.Count > 0 ? adjacentCells[Randomizer.Instance.Next(0, adjacentCells.Count)] : default!;
    }
}
