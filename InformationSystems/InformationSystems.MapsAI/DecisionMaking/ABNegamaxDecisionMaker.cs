using System;
using System.Collections.Generic;
using System.Linq;
using InformationSystems.MapsAI.Extensions;
using InformationSystems.MapsPathfinding;

namespace InformationSystems.MapsAI.DecisionMaking;

public class ABNegamaxDecisionMaker<TCell> : MinimaxDecisionMaker<TCell>
    where TCell : ICell
{
    public ABNegamaxDecisionMaker(
        GameBoard<TCell> board,
        IEnumerable<Player<TCell>> players,
        int depth = 5,
        Func<GameBoard<TCell>, Dictionary<Player<TCell>, TCell>, float>? evaluationFunction = null)
        : base(board, players, depth, evaluationFunction) { }

    public override TCell MoveNext(TCell cell, Dictionary<Player<TCell>, TCell>? cells = null)
    {
        Player<TCell> ally = _players.First(p => p.Kind == PlayerKind.Ally);

        Dictionary<Player<TCell>, TCell> initialPositions = new();

        foreach (var player in _players)
            initialPositions.Add(player, player.Cell);

        Negamax(initialPositions, PlayerKind.Ally, _depth, out var state);

        return state[ally];

        float Negamax(
            Dictionary<Player<TCell>, TCell> cells,
            PlayerKind kind,
            int depth,
            out Dictionary<Player<TCell>, TCell> state,
            float alpha = float.MinValue,
            float beta = float.MaxValue)
        {
            state = cells;

            if (TryEvaluateTerminalNode(ally, cells, depth, out float evaluation))
                return evaluation * (kind == PlayerKind.Ally ? 1 : -1);

            float best = float.MinValue;
            Dictionary<Player<TCell>, TCell> bestState = cells;

            foreach (var childState in GetChildStates(cells, kind))
            {
                evaluation = Negamax(childState, kind.Inverse(), depth - 1, out _, -beta, -alpha);

                UpdateEvaluation(evaluation, childState);
                alpha = Math.Max(alpha, evaluation);

                if (alpha >= beta)
                    break;
            }

            state = bestState;
            return best;

            void UpdateEvaluation(float value, Dictionary<Player<TCell>, TCell> state)
            {
                if (best < -value)
                {
                    best = -value;
                    bestState = state;
                }
            }
        }
    }
}
