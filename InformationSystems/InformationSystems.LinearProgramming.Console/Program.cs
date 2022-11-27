using System.Text;
using InformationSystems.LinearProgramming;

var tableau = SimplexTableau<double>.CreateBuilder()
    .SetTargetFunction(new SimplexRow<double>(1, -2, 0, 1, -5, 0, 2))
    .AddRow(new SimplexRow<double>(0, 1, 0, -5, -3, 0, 6))
    .AddRow(new SimplexRow<double>(0, 3, 0, 2, 5, 0, -1))
    .AddRow(new SimplexRow<double>(0, 2, 0, -3, 7, 0, 5))
    .ToTableau();

//var tableau = SimplexTableau<double>.CreateBuilder()
//    .SetTargetFunction(new SimplexRow<double>(1, 2, 2, 0, 0, 0, 3))
//    .AddRow(new SimplexRow<double>(0, 5, 2, 0, 1, 0, 1))
//    .AddRow(new SimplexRow<double>(0, 3, 2, 0, 0, 1, 0))
//    .AddRow(new SimplexRow<double>(0, 3, 3, 1, 0, 0, 2))
//    .ToTableau();

SimplexTableauProcessor<double> processor = new(tableau);

processor.IterationStart += (t, _) => PrintTableau(t);
processor.MaxEntryFound += (_, entry, i) => Console.WriteLine($"Found entry: {Round(entry)}, j: {i}\n");
processor.PivotFound += (_, pivot, j) => Console.WriteLine($"Pivot: {Round(pivot)}, i: {j}\n");
processor.RunFinished += PrintTableau;

double result = processor.RunSimplex(false);

Console.WriteLine(Round(result));

void PrintTableau(SimplexTableau<double> tableau)
{
    StringBuilder builder = new();

    for (int i = 0; i < tableau.TargetFunction.VariableCoefficients.Length; i++)
        builder.Append($"{$"x{i + 1}",-6}");

    builder.Append($"{"Z",-6}");
    builder.Append($"{"|",-6}");
    builder.Append($"{"C",-6}");

    builder.AppendLine();

    foreach (var current in tableau.Rows)
        AppendRow(current);

    builder.AppendLine(new string('-', (tableau.TargetFunction.VariableCoefficients.Length + 3) * 6));

    AppendRow(tableau.TargetFunction);

    Console.WriteLine(builder.ToString());

    void AppendRow(SimplexRow<double> row)
    {
        foreach (var c in row.VariableCoefficients)
            builder.Append($"{Round(c),-6}");

        builder.Append($"{Round(row.Z),-6}");
        builder.Append($"{"|",-6}");
        builder.Append($"{Round(row.C),-6}");
        builder.AppendLine();
    }
}

double Round(double d) => Math.Round(d, 2);
