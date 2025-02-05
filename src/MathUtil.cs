using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Soenneker.Extensions.Decimal;

namespace Soenneker.Utils.Math;

/// <summary>
/// Some useful math related utility methods
/// </summary>
public static class MathUtil
{
    [Pure]
    public static decimal GetWeightedMean(decimal valueA, decimal valueB, decimal weightA, decimal weightB, int? roundDigits = null)
    {
        decimal sumOfWeights = valueA * weightA + valueB * weightB;
        decimal weightedAverage = sumOfWeights / (weightA + weightB);

        if (roundDigits != null)
            return weightedAverage.ToRounded(roundDigits.Value);

        return weightedAverage;
    }

    [Pure]
    public static decimal GetWeightedMean(List<Tuple<decimal, decimal>> valueWeights, int? roundDigits = null)
    {
        decimal sumOfWeightMultiplication = valueWeights.Sum(tuple => tuple.Item1 * tuple.Item2);

        decimal sumOfWeights = valueWeights.Sum(tuple => tuple.Item2);

        decimal weightedAverage = sumOfWeightMultiplication / sumOfWeights;

        if (roundDigits != null)
            return weightedAverage.ToRounded(roundDigits.Value);

        return weightedAverage;
    }

    [Pure]
    public static decimal GetMean(params decimal[] values)
    {
        if (values.Length == 0)
            return 0;

        decimal sum = values.Sum();

        return sum / values.Length;
    }

    /// <summary>
    /// (final / initial) / final.
    /// If final = 0, sets final to .000001
    /// </summary>
    [Pure]
    public static decimal GetRelativeChange(decimal initial, decimal final)
    {
        if (final == 0)
            final = 0.00001M;

        return (final - initial) / final;
    }

    /// <summary>
    /// 1. Finds the slope (second / first) <para/>
    /// 2. Finds the yIntercept (slope * first) <para/>
    /// 3. Returns negative slope * point + yIntercept
    /// </summary>
    /// <remarks>Will never return a negative value (0)</remarks>
    [Pure]
    public static decimal GetLinearSlopeValue(decimal first, decimal second, decimal point)
    {
        if (first == 0 || second == 0)
            return 0;

        decimal slope = second / first;
        decimal yIntercept = slope * first;
        decimal result = -slope * point + yIntercept;

        return result < 0 ? 0 : result;
    }
}