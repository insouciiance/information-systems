using System;
using InformationSystems.NonLinearProgramming;

Function f = new(3, args =>
{
    var (x1, x2, x3) = (args.Values[0], args.Values[1], args.Values[2]);

    return 2 * x1 * x2 * x2 + 3 * x1 * x1 * x2 * x3 - 3 * x1 * x2 * x2 * x2 * x3 + 7 * x1 * x1 * x3 * x3;
});

// Himmelblau's function
// https://en.wikipedia.org/wiki/Himmelblau%27s_function
//Function f = new(2, args =>
//{
//    var (x1, x2) = (args.Values[0], args.Values[1]);

//    return (float)Math.Pow(x1 * x1 + x2 - 11, 2) + (float)Math.Pow(x1 + x2 * x2 - 7, 2);
//});

// Rosenbrock function
// https://en.wikipedia.org/wiki/Rosenbrock_function
//Function f = new(2, args =>
//{
//    var (x1, x2) = (args.Values[0], args.Values[1]);

//    return (float)Math.Pow(1 - x1, 2) + 100 * (float)Math.Pow(x2 - x1 * x1, 2);
//});

const float T = 1;

float d1 = T * ((float)Math.Sqrt(f.ArgumentsCount + 1) + f.ArgumentsCount - 1) / (f.ArgumentsCount * (float)Math.Sqrt(2));
float d2 = T * ((float)Math.Sqrt(f.ArgumentsCount + 1) - 1) / (f.ArgumentsCount * (float)Math.Sqrt(2));

Vector x1 = new(2, 2, 2);
Vector x2 = new(d1, d2, d2);
Vector x3 = new(d2, d1, d2);
Vector x4 = new(d2, d2, d1);

NelderMeadOptimizer optimizer = new(f, x1, x2, x3, x4);
FunctionOptimizationContext context = new(50, Console.Out);
float result = optimizer.Optimize(context, out var resultArgs);

Console.WriteLine($"Result: {result}");
Console.WriteLine($"Result vector: {resultArgs}");
