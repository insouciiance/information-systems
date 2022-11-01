using System.Collections.Generic;
using System.Collections.Immutable;
using InformationSystems.MapsPathfinding;
using InformationSystems.MapsPathfinding.Pathfinders;

namespace InformationSystems.MapsAI.DecisionMaking;

public class AStarDecisionMaker<TCell> : IDecisionMaker<TCell>
    where TCell : ICell
{
    private readonly Player<TCell> _dest;

    public GameBoard<TCell> Board { get; }

    public AStarDecisionMaker(GameBoard<TCell> board, Player<TCell> dest)
    {
        Board = board;
        _dest = dest;
    }

    public ImmutableArray<TCell> GetPossibleMoves(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        return ImmutableArray<TCell>.Empty.Add(MoveNext(cell, cells)!);
    }

    public TCell MoveNext(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        AStarPathfinder<TCell> pathfinder = new(Board.Grid);
        var result = pathfinder.GetPathResult(cell, cells is { } && cells.TryGetValue(_dest, out var c) ? c : _dest.Cell);
        return result.Path.Length > 0 ? result.Path[1] : default!;
    }
}
