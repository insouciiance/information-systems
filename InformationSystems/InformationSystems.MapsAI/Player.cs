using InformationSystems.MapsAI.DecisionMaking;
using InformationSystems.Graphs;

namespace InformationSystems.MapsAI;

public class Player<TCell>
    where TCell : ICell
{
    public PlayerKind Kind { get; }

    public IGrid<TCell> Grid { get; }

    public IDecisionMaker<TCell> DecisionMaker { get; }

    public TCell Cell { get; private set; }

    public Player(IGrid<TCell> grid, TCell cell, IDecisionMaker<TCell> decisionMaker, PlayerKind kind)
    {
        Grid = grid;
        Cell = cell;
        DecisionMaker = decisionMaker;
        Kind = kind;
    }

    public void MakeMove()
    {
        Cell = DecisionMaker.MoveNext(Cell)!;
    }
}
