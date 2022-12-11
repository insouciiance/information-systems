using System;
using System.Runtime.CompilerServices;
using InformationSystems.Shared.Extensions;

namespace InformationSystems.NonLinearProgramming;

public readonly struct Vector
{
    public readonly float[] Values;

    public Vector(params float[] values)
    {
        Values = values;
    }

    public static bool operator ==(Vector lhs, Vector rhs)
    {
        if (lhs.Values.Length != rhs.Values.Length)
            return false;

        for (int i = 0; i < lhs.Values.Length; i++)
        {
            if (!lhs.Values[i].IsEqualTo(rhs.Values[i]))
                return false;
        }

        return true;
    }

    public static bool operator !=(Vector lhs, Vector rhs)
    {
        return !(lhs == rhs);
    }

    public static Vector operator +(Vector lhs, Vector rhs)
    {
        if (lhs.Values.Length != rhs.Values.Length)
            throw new ArgumentException();

        float[] sum = new float[lhs.Values.Length];

        for (int i = 0; i < sum.Length; i++)
            sum[i] = lhs.Values[i] + rhs.Values[i];

        return new(sum);
    }

    public static Vector operator -(Vector lhs, Vector rhs)
    {
        if (lhs.Values.Length != rhs.Values.Length)
            throw new ArgumentException();

        float[] sum = new float[lhs.Values.Length];

        for (int i = 0; i < sum.Length; i++)
            sum[i] = lhs.Values[i] - rhs.Values[i];

        return new(sum);
    }

    public static Vector operator /(Vector lhs, float rhs)
    {
        return lhs * (1 / rhs);
    }

    public static Vector operator *(Vector lhs, float rhs)
    {
        float[] values = new float[lhs.Values.Length];

        for (int i = 0; i < values.Length; i++)
            values[i] = lhs.Values[i] * rhs;

        return new(values);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        Vector other = Unsafe.Unbox<Vector>(obj);
        return this == other;
    }

    public override int GetHashCode()
    {
        int hash = 17;

        foreach (var value in Values)
            hash = hash * 31 + value.GetHashCode();

        return hash;
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", Values)}]";
    }
}
