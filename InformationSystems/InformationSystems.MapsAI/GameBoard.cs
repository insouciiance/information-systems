using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using InformationSystems.MapsPathfinding;

namespace InformationSystems.MapsAI;

public class GameBoard<TCell>
    where TCell : ICell
{
    public IGrid<TCell> Grid { get; }

    public IEnumerable<Player<TCell>> Players { get; }

    public TCell Destination { get; }

    public event Action<GameBoard<TCell>>? OnMove;

    public GameBoard(IGrid<TCell> grid, IEnumerable<Player<TCell>> players, TCell destination)
    {
        Grid = grid;
        Players = players;
        Destination = destination;
    }

    public void Run()
    {
        Player<TCell> ally = Players.First(p => p.Kind == PlayerKind.Ally);
        List<Player<TCell>> opponents = Players.Where(p => p.Kind == PlayerKind.Opponent).ToList();

        while(true)
        {
            ally.MakeMove();

            if (IsGameOver())
                break;

            OnMove?.Invoke(this);

            foreach (var opponent in CollectionsMarshal.AsSpan(opponents))
                opponent.MakeMove();

            if (IsGameOver())
                break;

            OnMove?.Invoke(this);

            bool IsGameOver()
            {
                foreach (var opponent in CollectionsMarshal.AsSpan(opponents))
                {
                    if (EqualityComparer<TCell>.Default.Equals(ally.Cell, opponent.Cell))
                        return true;
                }

                if (EqualityComparer<TCell>.Default.Equals(Destination, ally.Cell))
                    return true;

                return false;
            }
        }
    }
}
