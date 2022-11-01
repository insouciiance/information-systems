using System.Collections.Generic;
using System.Linq;
using InformationSystems.MapsAI.DecisionMaking;
using InformationSystems.Graphs;
using InformationSystems.Graphs.Generators.Cells;
using InformationSystems.Graphs.Generators.Grids;
using InformationSystems.Shared.Utils;

namespace InformationSystems.MapsAI.Console;

public static class GenerationHelper
{
    public static GameBoard<TCell> GenerateGameBoard<TGrid, TCell, TGridGenerator, TCellGenerator>(
        int width,
        int height,
        int aStarOpponents,
        int randomOpponents)
        where TGrid : IGrid<TCell>
        where TCell : ICell
        where TGridGenerator : IGridGenerator<TGrid, TCell, TCellGenerator>
        where TCellGenerator : ICellGenerator<TCell>
    {
        TGrid grid = TGridGenerator.Generate(width, height);

        List<Player<TCell>> players = new();

        TCell destination = GenerateRandomOpenCell();

        GameBoard<TCell> board = new(grid, players, destination);

        Player<TCell> ally = GeneratePlayer(new NegaScoutDecisionMaker<TCell>(board, players), PlayerKind.Ally, new[] { destination });
        players.Add(ally);

        for (int i = 0; i < aStarOpponents; i++)
        {
            AStarDecisionMaker<TCell> decisionMaker = new(board, ally);
            Player<TCell> opponent = GeneratePlayer(decisionMaker, PlayerKind.Opponent, players.Select(p => p.Cell).Append(destination));
            players.Add(opponent);
        }

        for (int i = 0; i < randomOpponents; i++)
        {
            RandomDecisionMaker<TCell> decisionMaker = new(board);
            Player<TCell> opponent = GeneratePlayer(decisionMaker, PlayerKind.Opponent, players.Select(p => p.Cell).Append(destination));
            players.Add(opponent);
        }

        return board;

        TCell GenerateRandomOpenCell()
        {
            while(true)
            {
                var (x, y) = (Randomizer.Instance.Next(width), Randomizer.Instance.Next(height));

                if (grid.TryGetCell(x, y, out var cell) && !cell.IsBlocker)
                    return cell;
            }
        }

        Player<TCell> GeneratePlayer(IDecisionMaker<TCell> decisionMaker, PlayerKind kind, IEnumerable<TCell> occupiedCells)
        {
            while (true)
            {
                TCell position = GenerateRandomOpenCell();

                if (!occupiedCells.Contains(position))
                    return new Player<TCell>(grid, position!, decisionMaker, kind);
            }
        }
    }
}
