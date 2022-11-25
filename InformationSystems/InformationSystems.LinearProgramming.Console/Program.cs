using InformationSystems.LinearProgramming;

//var tableau = SimplexTableau<double>.CreateBuilder()
//	.SetTargetFunction(new SimplexRow<double>(1, -2, 0, 1, -5, 0, 2))
//	.AddRow(new SimplexRow<double>(0, 1, 0, -5, -3, 0, 6))
//	.AddRow(new SimplexRow<double>(0, 3, 0, 2, 5, 0, -1))
//	.AddRow(new SimplexRow<double>(0, 2, 0, -3, 7, 0, 5))
//	.ToTableau();

var tableau = SimplexTableau<double>.CreateBuilder()
	.SetTargetFunction(new SimplexRow<double>(1, 2, 2, 0, 0, 0, 3))
	.AddRow(new SimplexRow<double>(0, 5, 2, 0, 0, 0, 1))
	.AddRow(new SimplexRow<double>(0, 3, 2, 0, 0, 0, 0))
	.AddRow(new SimplexRow<double>(0, 3, 3, 0, 0, 0, 2))
	.ToTableau();
	

SimplexTableauProcessor<double> processor = new(tableau);
double result = processor.RunSimplex(false);

Console.WriteLine(result);
