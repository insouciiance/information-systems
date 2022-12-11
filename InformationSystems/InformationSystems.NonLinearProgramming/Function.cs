namespace InformationSystems.NonLinearProgramming;

public record Function(int ArgumentsCount, Function.FunctionDelegate Delegate)
{
    public delegate float FunctionDelegate(Vector args);
}
