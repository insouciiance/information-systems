using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using InformationSystems.MapsAI.Extensions;
using InformationSystems.Graphs;
using InformationSystems.Graphs.Extensions;
using InformationSystems.Graphs.Pathfinders;

namespace InformationSystems.MapsAI.DecisionMaking;

public class MinimaxDecisionMaker<TCell> : IDecisionMaker<TCell>
    where TCell : ICell
{
    public const int DEFAULT_DEPTH = 5;

    public static Func<GameBoard<TCell>, Dictionary<Player<TCell>, TCell>, float> DefaultEvaluationFunction { get; } = EvaluateDefault;

    public GameBoard<TCell> Board { get; }

    protected readonly IEnumerable<Player<TCell>> _players;

    protected readonly Func<GameBoard<TCell>, Dictionary<Player<TCell>, TCell>, float> _evaluationFunction;

    protected readonly int _depth;

    public MinimaxDecisionMaker(
        GameBoard<TCell> board,
        IEnumerable<Player<TCell>> players,
        int depth = DEFAULT_DEPTH,
        Func<GameBoard<TCell>, Dictionary<Player<TCell>, TCell>, float>? evaluationFunction = null)
    {
        Board = board;
        _players = players;
        _evaluationFunction = evaluationFunction ?? DefaultEvaluationFunction;
        _depth = depth;
    }

    public virtual IEnumerable<TCell> GetPossibleMoves(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        return Board.Grid.GetAdjacentNonBlockers(cell);
    }

    public virtual TCell MoveNext(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        Player<TCell> ally = _players.First(p => p.Kind == PlayerKind.Ally);

        Dictionary<Player<TCell>, TCell> initialPositions = new();

        foreach (var player in _players)
            initialPositions.Add(player, player.Cell);

        Minimax(initialPositions, PlayerKind.Ally, _depth, out var state);

        return state[ally];

        float Minimax(
            Dictionary<Player<TCell>, TCell> cells,
            PlayerKind kind,
            int depth,
            out Dictionary<Player<TCell>, TCell> state)
        {
            state = cells;

            if (TryEvaluateTerminalNode(ally, cells, depth, out float evaluation))
                return evaluation;

            float best = kind == PlayerKind.Ally ? float.MinValue : float.MaxValue;
            Dictionary<Player<TCell>, TCell> bestState = cells;

            foreach (var childState in GetChildStates(cells, kind))
            {
                evaluation = Minimax(childState, kind.Inverse(), depth - 1, out _);
                UpdateEvaluation(evaluation, childState);
            }

            state = bestState;
            return best;

            void UpdateEvaluation(float value, Dictionary<Player<TCell>, TCell> state)
            {
                if (kind == PlayerKind.Ally)
                {
                    if (best < value)
                        (best, bestState) = (value, state);
                }

                if (best > value)
                    (best, bestState) = (value, state);
            }
        }
    }

    protected bool TryEvaluateTerminalNode(Player<TCell> ally, Dictionary<Player<TCell>, TCell> cells, int depth, out float evaluation)
    {
        if (EqualityComparer<TCell>.Default.Equals(cells[ally], Board.Destination))
        {
            evaluation = float.MaxValue / ((_depth + 1f) / (depth + 1));
            return true;
        }

        if (cells.Any(e => !e.Key.Equals(ally) && EqualityComparer<TCell>.Default.Equals(e.Value, cells[ally])))
        {
            evaluation = 0;
            return true;
        }

        if (depth == 0)
        {
            evaluation = _evaluationFunction.Invoke(Board, cells);
            return true;
        }

        evaluation = default;
        return false;
    }

    protected static List<Dictionary<Player<TCell>, TCell>> GetChildStates(Dictionary<Player<TCell>, TCell> cells, PlayerKind kind)
    {
        List<Dictionary<Player<TCell>, TCell>> states = new() { new(cells) };

        foreach (var (player, cell) in cells)
        {
            if (player.Kind != kind)
                continue;

            List<Dictionary<Player<TCell>, TCell>> children = new();

            foreach (var state in CollectionsMarshal.AsSpan(states))
            {
                foreach (var playerMove in player.DecisionMaker.GetPossibleMoves(cell, state))
                {
                    Dictionary<Player<TCell>, TCell> childState = new(state);
                    childState[player] = playerMove;
                    children.Add(childState);
                }
            }

            states.Clear();
            states.AddRange(children);
        }

        return states;
    }

    private static float EvaluateDefault(GameBoard<TCell> board, Dictionary<Player<TCell>, TCell> cells)
    {
        var (dest, destCell) = cells.First(entry => entry.Key.Kind == PlayerKind.Ally);

        float distanceSum = 1;

        AStarPathfinder<TCell, IGrid<TCell>> pathfinder = new(board.Grid, board.Destination, destCell, RectangularGrid<TCell>.DefaultHeuristic);

        float distanceToEnd = pathfinder.GetPathResult().Evaluate();

        foreach (var (player, cell) in cells)
        {
            if (player.Equals(dest))
                continue;

            pathfinder = new(board.Grid, cell, destCell, RectangularGrid<TCell>.DefaultHeuristic);
            distanceSum *= pathfinder.GetPathResult().Evaluate();
        }

        return distanceToEnd > 0 ? distanceSum / distanceToEnd : float.MaxValue;
    }
}
