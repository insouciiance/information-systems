using MapsAI.Extensions;
using MapsPathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MapsAI.DecisionMaking;

public class NegaScoutDecisionMaker<TCell> : MinimaxDecisionMaker<TCell>
    where TCell : ICell
{
    public NegaScoutDecisionMaker(
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

        NegaScout(initialPositions, PlayerKind.Ally, _depth, out var state);

        return state[ally];

        float NegaScout(
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

            Dictionary<Player<TCell>, TCell> bestState = cells;

            var childStates = CollectionsMarshal.AsSpan(GetChildStates(cells, kind));

            for (int i = 0; i < childStates.Length; i++)
            {
                var childState = childStates[i];

                if (i == 0)
                {
                    evaluation = -NegaScout(childState, kind.Inverse(), depth - 1, out state, -beta, -alpha);
                }
                else
                {
                    evaluation = -NegaScout(childState, kind.Inverse(), depth - 1, out state, -alpha - 1, -alpha);

                    if (alpha < evaluation && evaluation < beta)
                        evaluation = -NegaScout(childState, kind.Inverse(), depth - 1, out state, -beta, -evaluation);
                }

                UpdateEvaluation(evaluation, childState);

                if (alpha >= beta)
                    break;
            }

            state = bestState;
            return alpha;

            void UpdateEvaluation(float value, Dictionary<Player<TCell>, TCell> state)
            {
                if (alpha < value)
                {
                    alpha = value;
                    bestState = state;
                }
            }
        }
    }
}
