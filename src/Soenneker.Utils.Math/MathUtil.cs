using Soenneker.Extensions.Decimal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Soenneker.Utils.Math;

/// <summary>
/// Some useful math related utility methods
/// </summary>
public static class MathUtil
{
    /// <summary>
    /// Calculates the weighted mean of two values using their respective weights, with optional rounding to a specified
    /// number of decimal digits.
    /// </summary>
    /// <remarks>If both weights are zero, the method will throw a division by zero exception. The method uses
    /// standard weighted mean calculation: (valueA × weightA + valueB × weightB) / (weightA + weightB).</remarks>
    /// <param name="valueA">The first value to be averaged. Represents one component of the weighted mean calculation.</param>
    /// <param name="valueB">The second value to be averaged. Represents the other component of the weighted mean calculation.</param>
    /// <param name="weightA">The weight associated with <paramref name="valueA"/>. Must be non-negative.</param>
    /// <param name="weightB">The weight associated with <paramref name="valueB"/>. Must be non-negative.</param>
    /// <param name="roundDigits">The number of decimal digits to round the result to. If <see langword="null"/>, the result is not rounded.</param>
    /// <returns>The weighted mean of the two values, optionally rounded to the specified number of decimal digits.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal GetWeightedMean(decimal valueA, decimal valueB, decimal weightA, decimal weightB, int? roundDigits = null)
    {
        decimal denominator = weightA + weightB;

        decimal weightedAverage = (valueA * weightA + valueB * weightB) / denominator;

        return roundDigits is int d ? weightedAverage.ToRounded(d) : weightedAverage;
    }

    /// <summary>
    /// Calculates the weighted mean of a sequence of value-weight pairs, optionally rounding the result to a specified
    /// number of digits.
    /// </summary>
    /// <remarks>If the list is empty, the method returns 0. The calculation divides the sum of value-weight
    /// products by the sum of weights. Rounding is performed only if <paramref name="roundDigits"/> is
    /// specified.</remarks>
    /// <param name="valueWeights">A read-only list of tuples, each containing a value and its corresponding weight. The list must not be null and
    /// may be empty.</param>
    /// <param name="roundDigits">The number of fractional digits to which the result should be rounded. If null, the result is not rounded.</param>
    /// <returns>The weighted mean of the provided values as a decimal. Returns 0 if the list is empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="valueWeights"/> is null.</exception>
    [Pure]
    public static decimal GetWeightedMean(IReadOnlyList<(decimal Value, decimal Weight)> valueWeights, int? roundDigits = null)
    {
        if (valueWeights is null)
            throw new ArgumentNullException(nameof(valueWeights));

        int count = valueWeights.Count;
        if (count == 0)
            return 0m;

        var sumValueTimesWeight = 0m;
        var sumWeights = 0m;

        for (var i = 0; i < count; i++)
        {
            (decimal value, decimal weight) = valueWeights[i];
            sumValueTimesWeight += value * weight;
            sumWeights += weight;
        }

        decimal weightedAverage = sumValueTimesWeight / sumWeights;

        return roundDigits is int d ? weightedAverage.ToRounded(d) : weightedAverage;
    }

    [Pure]
    public static decimal GetWeightedMean(IReadOnlyList<Tuple<decimal, decimal>> valueWeights, int? roundDigits = null)
    {
        if (valueWeights is null)
            throw new ArgumentNullException(nameof(valueWeights));

        int count = valueWeights.Count;
        if (count == 0)
            return 0m;

        var sumValueTimesWeight = 0m;
        var sumWeights = 0m;

        for (var i = 0; i < count; i++)
        {
            Tuple<decimal, decimal> t = valueWeights[i];
            decimal value = t.Item1;
            decimal weight = t.Item2;

            sumValueTimesWeight += value * weight;
            sumWeights += weight;
        }

        decimal weightedAverage = sumValueTimesWeight / sumWeights;

        return roundDigits is int d ? weightedAverage.ToRounded(d) : weightedAverage;
    }

    /// <summary>
    /// Calculates the arithmetic mean of the specified sequence of decimal values.
    /// </summary>
    /// <remarks>This method does not throw an exception for empty sequences; instead, it returns 0. The
    /// calculation uses decimal arithmetic to preserve precision.</remarks>
    /// <param name="values">A read-only span containing the decimal values for which to compute the mean.</param>
    /// <returns>The arithmetic mean of the values in the sequence. Returns 0 if the sequence is empty.</returns>
    [Pure]
    public static decimal GetMean(ReadOnlySpan<decimal> values)
    {
        int len = values.Length;
        if (len == 0)
            return 0m;

        var sum = 0m;
        for (var i = 0; i < len; i++)
            sum += values[i];

        return sum / len;
    }

    /// <summary>
    /// Calculates the arithmetic mean of the specified decimal values.
    /// </summary>
    /// <remarks>If the array is empty, the method may throw an exception. This method is suitable for
    /// financial or high-precision calculations where decimal accuracy is required.</remarks>
    /// <param name="values">An array of decimal values for which to compute the mean. The array must contain at least one element.</param>
    /// <returns>The arithmetic mean of the provided values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal GetMean(params decimal[] values) => GetMean(values.AsSpan());

    /// <summary>
    /// (final / initial) / final.
    /// If final = 0, sets final to .000001
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal GetRelativeChange(decimal initial, decimal final)
    {
        // Note: your comment says .000001 but the code used 0.00001M. Keeping behavior.
        if (final == 0m)
            final = 0.00001m;

        return (final - initial) / final;
    }

    /// <summary>
    /// 1. Finds the slope (second / first)
    /// 2. Finds the yIntercept (slope * first)
    /// 3. Returns negative slope * point + yIntercept
    /// </summary>
    /// <remarks>Will never return a negative value (0)</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal GetLinearSlopeValue(decimal first, decimal second, decimal point)
    {
        if (first == 0m || second == 0m)
            return 0m;

        // yIntercept = (second/first) * first = second
        // result = -slope*point + second
        decimal result = second - second / first * point;

        return result > 0m ? result : 0m;
    }

    /// <summary>
    /// Computes the sigmoid activation function for the specified input value.
    /// </summary>
    /// <remarks>The sigmoid function is commonly used in machine learning and statistics to map real-valued
    /// inputs to a value between 0 and 1. It is defined as 1 / (1 + exp(-x)).</remarks>
    /// <param name="x">The input value for which to calculate the sigmoid function.</param>
    /// <returns>A double value representing the result of the sigmoid function, which will be in the range (0, 1).</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Sigmoid(double x)
    {
        if (x >= 0)
        {
            double expNegX = System.Math.Exp(-x);
            return 1.0 / (1.0 + expNegX);
        }

        double expX = System.Math.Exp(x);
        return expX / (1.0 + expX);
    }

    /// <summary>
    /// Not a real sigmoid, but fast and S-shaped
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SigmoidFast(float x) => x / (1f + MathF.Abs(x));
}