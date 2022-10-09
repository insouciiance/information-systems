using System;
using System.Text;
using System.Threading;
using MapsAI;
using MapsAI.DecisionMaking;
using MapsPathfinding;
using MapsPathfinding.IO;
using MapsPathfinding.IO.GridMapWriters;
using MapsPathfinding.Generators.Cells;
using MapsPathfinding.Generators.Grids;

Console.OutputEncoding = Encoding.Unicode;

GameBoard<Cell> board = MapsAI.Console.GenerationHelper.GenerateGameBoard<RectangularGrid<Cell>, Cell,
    RectangularMazeGenerator<Cell, CellGenerator>, CellGenerator>(11, 11, 1, 1);

ConsoleGridMapWriter consoleWriter = new();

board.OnMove += HandleMove;

HandleMove(board);

board.Run();

void HandleMove<TCell>(GameBoard<TCell> board)
    where TCell : ICell
{
    GridOutputMap map = ConstructMap(board);
    Console.Clear();
    consoleWriter.Write(map);
    Thread.Sleep(500);
}

GridOutputMap ConstructMap<TCell>(GameBoard<TCell> board)
    where TCell : ICell
{
    GridOutputMap map = new(board.Grid.Width, board.Grid.Height);

    for (int i = 0; i < board.Grid.Height; i++)
    {
        for (int j = 0; j < board.Grid.Width; j++)
        {
            board.Grid.TryGetCell(j, i, out var cell);
            string text = cell!.IsBlocker ? "x" : ".";
            var color = ((byte, byte, byte))(cell.IsBlocker ? (100, 100, 100) : (0, 0, 0));
            map.SetCell(j, i, new() { Text = text, Color = color });
        }
    }

    foreach (var player in board.Players)
    {
        string text = player.Kind == PlayerKind.Ally ? "A" : "O";
        var color = ((byte, byte, byte))GetPlayerColor();
        map.SetCell(player.Cell.X, player.Cell.Y, new() { Text = text, Color = color });

        (int, int, int) GetPlayerColor()
        {
            if (player!.Kind == PlayerKind.Ally)
                return (0, 0, 255);

            if (player.DecisionMaker.GetType().GetGenericTypeDefinition() == typeof(RandomDecisionMaker<>))
                return (255, 165, 0);

            return (255, 0, 0);
        }
    }

    map.SetCell(board.Destination.X, board.Destination.Y, new() { Text = "*", Color = (0, 255, 0) });

    return map;
}
