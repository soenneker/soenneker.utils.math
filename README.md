[![](https://img.shields.io/nuget/v/Soenneker.Utils.Math.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Utils.Math/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.math/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.math/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Utils.Math.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Utils.Math/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.math/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.utils.math/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Utils.Math
Some useful math related utility methods.

## Installation

```bash
dotnet add package Soenneker.Utils.Math
```

## Quick start

```csharp
using Soenneker.Utils.Math;
```

Call the static `MathUtil` methods directly; no dependency-injection registration is required.

## Means

```csharp
decimal weighted = MathUtil.GetWeightedMean(
    valueA: 80m,
    valueB: 95m,
    weightA: 1m,
    weightB: 2m,
    roundDigits: 2); // 90

decimal mean = MathUtil.GetMean(10m, 20m, 30m); // 20
```

The weighted overloads calculate `sum(value * weight) / sum(weight)`. They do not reject negative
weights. An empty collection returns `0`; a non-empty collection whose weights sum to zero throws
`DivideByZeroException`. `GetMean` also returns `0` for empty input. Decimal accumulation can throw
`OverflowException` for values outside the decimal range.

## Relative and linear values

```csharp
decimal relative = MathUtil.GetRelativeChange(initial: 80m, final: 100m); // 0.2
decimal remaining = MathUtil.GetLinearSlopeValue(first: 10m, second: 100m, point: 4m); // 60
```

`GetRelativeChange` uses `(final - initial) / final`; this is not the conventional
`(final - initial) / initial` formula. When `final` is zero, the method substitutes `0.00001m`
before calculating.

`GetLinearSlopeValue` evaluates `second - (second / first * point)` and clamps negative results to
zero. It also returns zero immediately when either `first` or `second` is zero.

## Sigmoid functions

```csharp
double logistic = MathUtil.Sigmoid(2.0);   // standard logistic value in (0, 1)
float fast = MathUtil.SigmoidFast(-2.0f);  // approximation in (-1, 1)
```

`Sigmoid` computes the numerically stable logistic function `1 / (1 + exp(-x))`.
`SigmoidFast` computes `x / (1 + abs(x))`; it is S-shaped but is not a logistic approximation on
the same output range.
