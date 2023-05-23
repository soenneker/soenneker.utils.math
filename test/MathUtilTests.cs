using System;
using FluentAssertions;
using Soenneker.Extensions.Decimal;
using Soenneker.Tests.Unit;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Utils.Math.Tests;

public class MathUtilTests : UnitTest
{
    public MathUtilTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public void GetMean_should_return_mean()
    {
        const int value1 = 10;
        const int value2 = 20;

        MathUtil.GetMean(value1, value2).Should().Be(15);
    }

    [Fact]
    public void GetWeightedMean_should_return_mean()
    {
        const int value1 = 100;
        const int value2 = 10;

        const decimal weight1 = 100;
        const decimal weight2 = 10;

        int result = MathUtil.GetWeightedMean(value1, value2, weight1, weight2).ToInt();
        result.Should().Be(92);
    }

    [Fact]
    public void GetWeightedMean_with_tuples_should_return_mean()
    {
        var values = new List<Tuple<decimal, decimal>>
        {
            new(100, 100),
            new (10, 10)
        };

        int result = MathUtil.GetWeightedMean(values).ToInt();
        result.Should().Be(92);
    }

    [Theory]
    [InlineData(.50, .25, .40, .05)]
    [InlineData(.50, .25, .30, .10)]
    [InlineData(.50, .25, .20, .15)]
    [InlineData(.50, .25, .10, .20)]
    [InlineData(0, .25, .25, 0)]
    [InlineData(.50, 0, .25, 0)]
    [InlineData(.50, .25, 0, .25)]
    public void GetLinearSlopeValue_should_not_throw(decimal first, decimal second, decimal point, decimal expected)
    {
        decimal result = MathUtil.GetLinearSlopeValue(first, second, point);

        result.Should().Be(expected);
    }
}