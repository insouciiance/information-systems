using System.Collections.Immutable;
using System.Numerics;

namespace InformationSystems.LinearProgramming;

public class SimplexTableau<T>
	where T : INumber<T>
{
	private SimplexTableau(ImmutableArray<SimplexRow<T>> rows, SimplexRow<T> targetFunction)
	{
		Rows = rows;
		TargetFunction = targetFunction;
	}

	public ImmutableArray<SimplexRow<T>> Rows { get; }

	public SimplexRow<T> TargetFunction { get; }

	public Builder ToBuilder() => new(Rows, TargetFunction);

	public static Builder CreateBuilder() => new();

	public class Builder
	{
		private readonly ImmutableArray<SimplexRow<T>>.Builder _builder;

		private SimplexRow<T> _targetFunction;

		internal Builder()
		{
			_builder = ImmutableArray.CreateBuilder<SimplexRow<T>>();
		}

		internal Builder(ImmutableArray<SimplexRow<T>> rows, SimplexRow<T> targetFunction)
		{
			_builder = rows.ToBuilder();
			_targetFunction = targetFunction;
		}

		public Builder AddRow(SimplexRow<T> row)
		{
			_builder.Add(row);
			return this;
		}

		public Builder SetRow(SimplexRow<T> row, int index)
		{
			_builder[index] = row;
			return this;
		}

		public Builder SetTargetFunction(SimplexRow<T> targetFunction)
		{
			_targetFunction = targetFunction;
			return this;
		}

		public SimplexTableau<T> ToTableau() => new(_builder.ToImmutable(), _targetFunction);
	}
}
