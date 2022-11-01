using System.Collections.Generic;
using System.Collections.Immutable;
using InformationSystems.Graphs;
using InformationSystems.Graphs.Pathfinders;

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

    public IEnumerable<TCell> GetPossibleMoves(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        yield return MoveNext(cell, cells)!;
    }

    public TCell MoveNext(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        TCell dest = cells is { } && cells.TryGetValue(_dest, out var c) ? c : _dest.Cell;
        AStarPathfinder<TCell, IGrid<TCell>> pathfinder = new(Board.Grid, cell, dest, RectangularGrid<TCell>.DefaultHeuristic);
        var result = pathfinder.GetPathResult();
        return result.Path.Length > 0 ? result.Path[1] : default!;
    }
}
