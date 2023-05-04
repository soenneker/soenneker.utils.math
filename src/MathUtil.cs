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

        decimal result = sum / values.Length;
        return result;
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

        decimal result = (final - initial) / final;

        return result;
    }

    [Pure]
    public static decimal GetLinearSlopeValue(decimal first, decimal second, decimal point)
    {
        decimal slope = second / first;
        decimal yIntercept = slope * first;
        decimal result = -slope * point + yIntercept;

        return result;
    }
}