using System;
using System.Linq;

namespace InformationSystems.NonLinearProgramming;

public class NelderMeadOptimizer : IFunctionOptimizer
{
    public const float ALPHA = 1;

    public const float BETA = 0.5f;

    public const float GAMMA = 2;

    public const float DELTA = 0.5f;

    public const float MIN_STDDEV = 0.0001f;

    public Function Function { get; }

    private readonly Vector[] _initialSimplex;

    public NelderMeadOptimizer(Function function, params Vector[] initialSimplex)
    {
        Function = function;
        _initialSimplex = initialSimplex;
    }

    public float Optimize(in FunctionOptimizationContext context, out Vector args)
    {
        int iterationsCount = context.IterationsCount;

        FunctionValue[] values = CalculateValues(_initialSimplex);

        OptimizeInternal(context);

        FunctionValue best = values[0];
        args = best.Vector;
        return best.Value;

        void OptimizeInternal(in FunctionOptimizationContext context)
        {
            while (true)
            {
                context.Writer?.WriteLine($"Begin iteration: {context.IterationsCount - iterationsCount}");

                values = Sort(values);

                context.Writer?.WriteLine($"Sorted values:\n{string.Join("\n", values)}");

                if (iterationsCount <= 0 || ShouldTerminate(values))
                {
                    context.Writer?.WriteLine("Terminating");
                    return;
                }

                ref FunctionValue l = ref values[0];
                ref FunctionValue g = ref values[^2];
                ref FunctionValue h = ref values[^1];

                context.Writer?.WriteLine($"Current best value: {l}");

                Vector c = CalculateCentroid(values, h);

                context.Writer?.WriteLine($"Centroid: {c}");

                FunctionValue r = CalculateReflection(h, g, l, c);

                context.Writer?.WriteLine($"Reflection: {r}");

                UpdateSimplex(context, ref h, ref g, ref l, r, c);

                context.Writer?.WriteLine();
                iterationsCount--;
            }
        }
    }

    private bool ShouldTerminate(FunctionValue[] values)
    {
        double mean = GetMean();
        
        double stdDev = 0;

        foreach (var value in values)
            stdDev += Math.Pow(value.Value - mean, 2);

        stdDev /= values.Length;

        return Math.Sqrt(stdDev) < MIN_STDDEV;

        double GetMean()
        {
            double result = 0;

            foreach (var value in values)
                result += value.Value;

            return result / values.Length;
        }
    }

    private FunctionValue[] CalculateValues(Vector[] args)
    {
        return args
            .Select(a => new FunctionValue(Function.Delegate.Invoke(a), a))
            .ToArray();
    }

    private FunctionValue[] Sort(FunctionValue[] values)
    {
        return values
            .OrderBy(v => v)
            .ToArray();
    }

    private Vector CalculateCentroid(FunctionValue[] values, in FunctionValue h)
    {
        int argCount = 0;
        Vector argSum = new(new float[h.Vector.Values.Length]);

        foreach (var value in values)
        {
            if (value == h)
                continue;

            argCount++;
            argSum += value.Vector;
        }

        return argSum / argCount;
    }

    private FunctionValue CalculateReflection(in FunctionValue h, in FunctionValue g, in FunctionValue l, Vector c)
    {
        Vector reflectionArgs = c * (1 + ALPHA) - h.Vector * ALPHA;
        FunctionValue value = new(Function.Delegate.Invoke(reflectionArgs), reflectionArgs);
        return value;
    }

    private void UpdateSimplex(in FunctionOptimizationContext context, ref FunctionValue h, ref FunctionValue g, ref FunctionValue l, in FunctionValue r, Vector c)
    {
        if (l <= r && r < g)
        {
            context.Writer?.WriteLine("Reflection is between l and g => replacing h = r, return");
            h = r;
            return;
        }   

        if (r < l)
        {
            FunctionValue e = Expand(r);

            context.Writer?.WriteLine($"Reflection is < l => expanding, expanded value: {e}");

            if (e < r)
            {
                context.Writer?.WriteLine("e < r => replacing h with e, return");
                h = e;
                return;
            }

            context.Writer?.WriteLine("e >= r => replacing h with r, return");
            h = r;
            return;
        }

        if (r < h)
        {
            Vector contracted = c + (r.Vector - c) * DELTA;
            FunctionValue contractedValue = new(Function.Delegate.Invoke(contracted), contracted);

            context.Writer?.WriteLine($"Reflection is < h, outside contracted value: {contractedValue}");
            
            if (contractedValue < r)
            {
                context.Writer?.WriteLine("Contracted value is < r, replacing h with contracted value, return");
                h = contractedValue;
                return;
            }
        }
        
        if (r >= h)
        {
            Vector contracted = c + (h.Vector - c) * DELTA;
            FunctionValue contractedValue = new(Function.Delegate.Invoke(contracted), contracted);

            context.Writer?.WriteLine($"Reflection is < h, inside contracted value: {contractedValue}");

            if (contractedValue < r)
            {
                context.Writer?.WriteLine("Contracted value is < r, replacing h with contracted value, return");
                h = contractedValue;
                return;
            }
        }

        Shrink(ref h, ref g, l);

        context.Writer?.WriteLine("Shrinking points");

        FunctionValue Expand(in FunctionValue r)
        {
            Vector expansionArgs = c + (r.Vector - c) * GAMMA;
            FunctionValue value = new(Function.Delegate.Invoke(expansionArgs), expansionArgs);
            return value;
        }

        void Shrink(ref FunctionValue h, ref FunctionValue g, in FunctionValue l)
        {
            Vector hArgs = l.Vector + (h.Vector - l.Vector) * BETA;
            h = new(Function.Delegate.Invoke(hArgs), hArgs);

            Vector gArgs = l.Vector + (g.Vector - l.Vector) * BETA;
            g = new(Function.Delegate.Invoke(gArgs), gArgs);
        }
    }
}
