using System;
using System.Collections.Generic;
using System.Linq;
using MapsAI.Extensions;
using MapsPathfinding;

namespace MapsAI.DecisionMaking;

public class ABMinimaxDecisionMaker<TCell> : MinimaxDecisionMaker<TCell>
    where TCell : ICell
{
    public ABMinimaxDecisionMaker(
        GameBoard<TCell> board,
        IEnumerable<Player<TCell>> players,
        int depth = DEFAULT_DEPTH,
        Func<GameBoard<TCell>, Dictionary<Player<TCell>, TCell>, float>? evaluationFunction = null)
    : base(board, players, depth, evaluationFunction) { }

    public override TCell MoveNext(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        Player<TCell> ally = _players.First(p => p.Kind == PlayerKind.Ally);

        Dictionary<Player<TCell>, TCell> initialPositions = new();

        foreach (var player in _players)
            initialPositions.Add(player, player.Cell);

        Minimax(initialPositions, PlayerKind.Ally, _depth, out var state);

        return state[_players.First(p => p.Kind == PlayerKind.Ally)];

        float Minimax(
            Dictionary<Player<TCell>, TCell> cells,
            PlayerKind kind,
            int depth,
            out Dictionary<Player<TCell>, TCell> state,
            float alpha = float.MinValue,
            float beta = float.MaxValue)
        {
            state = cells;

            if (EqualityComparer<TCell>.Default.Equals(cells[ally], Board.Destination))
                return float.MaxValue / ((_depth + 1) / (depth + 1));

            if (cells.Any(e => !e.Key.Equals(ally) && EqualityComparer<TCell>.Default.Equals(e.Value, cells[ally])))
                return 0;

            if (depth == 0)
                return _evaluationFunction.Invoke(Board, cells);

            float best = kind == PlayerKind.Ally ? float.MinValue : float.MaxValue;
            Dictionary<Player<TCell>, TCell> bestState = cells;

            foreach (var childState in GetChildStates(cells, kind))
            {
                float evaluation = Minimax(childState, kind.Inverse(), depth - 1, out _);

                UpdateEvaluation(evaluation, childState);

                if (beta <= alpha)
                    break;
            }

            state = bestState;
            return best;

            void UpdateEvaluation(float value, Dictionary<Player<TCell>, TCell> state)
            {
                if (kind == PlayerKind.Ally)
                {
                    if (best < value)
                        (best, bestState) = (value, state);

                    alpha = Math.Max(alpha, value);
                    return;
                }

                if (best > value)
                    (best, bestState) = (value, state);

                beta = Math.Min(beta, value);
            }
        }
    }
}
