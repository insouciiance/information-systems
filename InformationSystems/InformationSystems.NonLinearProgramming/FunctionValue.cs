using System;
using InformationSystems.Shared.Extensions;

namespace InformationSystems.NonLinearProgramming;

public readonly record struct FunctionValue(float Value, Vector Vector) : IComparable<FunctionValue>
{
    public static bool operator >(FunctionValue lhs, FunctionValue rhs)
    {
        return lhs.Value > rhs.Value;
    }

    public static bool operator <(FunctionValue lhs, FunctionValue rhs)
    {
        return lhs.Value < rhs.Value;
    }

    public static bool operator >=(FunctionValue lhs, FunctionValue rhs)
    {
        return lhs.Value.IsEqualTo(rhs.Value) || lhs > rhs;
    }

    public static bool operator <=(FunctionValue lhs, FunctionValue rhs)
    {
        return lhs.Value.IsEqualTo(rhs.Value) || lhs < rhs;
    }

    public int CompareTo(FunctionValue other)
    {
        return Value.CompareTo(other.Value);
    }
}
