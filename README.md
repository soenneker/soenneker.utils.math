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

## Common operations

- `GetWeightedMean()` - Calculates the weighted mean of two values using their respective weights, with optional rounding to a specified number of decimal digits. If both weights are zero, the method will throw a division by zero exception.
- `GetMean()` - Calculates the arithmetic mean of the specified sequence of decimal values. This method does not throw an exception for empty sequences; instead, it returns 0.
- `GetRelativeChange()` - (final / initial) / final. If final = 0, sets final to .000001.
- `GetLinearSlopeValue()` - 1. Finds the slope (second / first) 2. Finds the yIntercept (slope * first) 3. Returns negative slope * point + yIntercept.
- `Sigmoid()` - Computes the sigmoid activation function for the specified input value.
- `SigmoidFast()` - Not a real sigmoid, but fast and S-shaped.
