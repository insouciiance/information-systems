namespace InformationSystems.NonLinearProgramming;

public interface IFunctionOptimizer
{
    Function Function { get; }

    float Optimize(in FunctionOptimizationContext context, out Vector args);
}
