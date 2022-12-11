using System.IO;

namespace InformationSystems.NonLinearProgramming;

public readonly ref struct FunctionOptimizationContext
{
    public readonly int IterationsCount;

    public readonly TextWriter? Writer;

    public FunctionOptimizationContext() : this(int.MaxValue) { }

    public FunctionOptimizationContext(int iterationsCount = int.MaxValue, TextWriter? writer = null)
    {
        IterationsCount = iterationsCount;
        Writer = writer;
    }
}
