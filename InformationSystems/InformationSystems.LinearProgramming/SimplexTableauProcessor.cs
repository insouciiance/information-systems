using System.Linq;
using System.Numerics;

namespace InformationSystems.LinearProgramming;

public class SimplexTableauProcessor<T>
	where T : INumber<T>
{
	private SimplexTableau<T> _tableau;

	public SimplexTableauProcessor(SimplexTableau<T> tableau)
	{
		_tableau = tableau;
	}

	public T RunSimplex(bool maximize = true)
	{
		return maximize ? RunSimplexInternal<Maximizer>() : RunSimplexInternal<Minimizer>();
	}

	private T RunSimplexInternal<TOptimizer>()
		where TOptimizer : IOptimizer
	{
		while (!CheckFinished())
		{
			var (entry, x) = FindMaxEntry();
			var (pivot, y) = FindPivot(x);
			_tableau = NormalizeTableau(pivot, x, y);
		}

		return _tableau.TargetFunction.C;

		bool CheckFinished() => _tableau.TargetFunction.VariableCoefficients.All(c => !TOptimizer.ShouldOptimize(c));

		(T Entry, int Index) FindMaxEntry()
		{
			T maxEntry = _tableau.TargetFunction.VariableCoefficients[0];
			int index = 0;

			for (int i = 1; i < _tableau.TargetFunction.VariableCoefficients.Length; i++)
			{
				T current = _tableau.TargetFunction.VariableCoefficients[i];

				if (TOptimizer.Compare(maxEntry, current) < T.Zero)
				{
					maxEntry = current;
					index = i;
				}
			}

			return (maxEntry, index);
		}

		(T Pivot, int Index) FindPivot(int x)
		{
			int index = -1;
			T pivot = default!;
			T quotient = default!;

			for (int i = 0; i < _tableau.Rows.Length; i++)
			{
				SimplexRow<T> row = _tableau.Rows[i];

				T current = row.VariableCoefficients[x];

				if (current < T.Zero)
					continue;

				T currentQuotient = row.C / current;

				if (index == -1 || currentQuotient < quotient)
				{
					pivot = current;
					quotient = currentQuotient;
					index = i;
				}
			}

			return (pivot, index);
		}

		SimplexTableau<T> NormalizeTableau(T pivot, int x, int y)
		{
			var builder = _tableau.ToBuilder();

			SimplexRow<T> pivotRow = _tableau.Rows[y] * (T.One / pivot!);
			builder.SetRow(pivotRow, y);

			builder.SetTargetFunction(NormalizeRow(_tableau.TargetFunction));

			for (int i = 0; i < _tableau.Rows.Length; i++)
			{
				if (i == y)
					continue;

				SimplexRow<T> current = _tableau.Rows[i];
				builder.SetRow(NormalizeRow(current), i);
			}

			return builder.ToTableau();

			SimplexRow<T> NormalizeRow(SimplexRow<T> row)
			{
				T factor = row.VariableCoefficients[x] / -pivot!;
				return pivotRow * factor + row;
			}
		}
	}

	private interface IOptimizer
	{
		static abstract T Compare(T lhs, T rhs);

		static abstract bool ShouldOptimize(T value);
	}

	private class Maximizer : IOptimizer
	{
		public static T Compare(T lhs, T rhs) => rhs - lhs;

		public static bool ShouldOptimize(T value) => value < T.Zero;
	}

	private class Minimizer : IOptimizer
	{
		public static T Compare(T lhs, T rhs) => lhs - rhs;

		public static bool ShouldOptimize(T value) => value > T.Zero;
	}
}
