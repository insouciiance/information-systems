using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

namespace InformationSystems.LinearProgramming;

public readonly struct SimplexRow<T>
	where T : INumber<T>
{
	public readonly ImmutableArray<T> VariableCoefficients;

	public readonly T Z;

	public readonly T C;

	public SimplexRow(ImmutableArray<T> variableCoefficients, T z, T c)
	{
		VariableCoefficients = variableCoefficients;
		Z = z;
		C = c;
	}

	public SimplexRow(IEnumerable<T> variableCoefficients, T z, T c)
		: this(variableCoefficients.ToImmutableArray(), z, c) { }

	public SimplexRow(T z, T c, params T[] variableCoefficients)
		: this(variableCoefficients.ToImmutableArray(), z, c) { }

	public static SimplexRow<T> operator +(SimplexRow<T> lhs, SimplexRow<T> rhs)
	{
		return new SimplexRow<T>(lhs.VariableCoefficients.Select((c, i) => c + rhs.VariableCoefficients[i]), lhs.Z + rhs.Z, lhs.C + rhs.C);
	}

	public static SimplexRow<T> operator *(SimplexRow<T> row, T factor)
	{
		return new SimplexRow<T>(row.VariableCoefficients.Select(c => c * factor), row.Z * factor, row.C * factor);
	}
}
