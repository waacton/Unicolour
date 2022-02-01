# <img src="https://gitlab.com/Wacton/Unicolour/-/raw/main/Unicolour/Resources/Unicolour.png" width="32" height="32"> Unicolour
[![pipeline status](https://gitlab.com/Wacton/Unicolour/badges/main/pipeline.svg)](https://gitlab.com/Wacton/Unicolour/-/commits/main)
[![coverage report](https://gitlab.com/Wacton/Unicolour/badges/main/coverage.svg)](https://gitlab.com/Wacton/Unicolour/-/commits/main)
[![NuGet](https://badgen.net/nuget/v/Wacton.Unicolour?icon)](https://www.nuget.org/packages/Wacton.Unicolour/)

Unicolour is a small set of utilities for working with colour:
- Colour space conversion
- Colour interpolation
- Colour comparison

A `Unicolour` encapsulates a single colour and its representation across different colour spaces. It supports:
- RGB
- HSB/HSV
- CIE XYZ
- CIE LAB

This library was initially written for personal projects since existing libraries had complex APIs or missing features.
The goal of this library is to be intuitive and easy to use; performance is not a priority.

More functionality will be added over time.

Targets .NET Standard 2.0 for use in .NET 5.0+, .NET Core 2.0+ and .NET Framework 4.6.1+ applications.

## How to use 🔴🟢🔵
🚧 Still in alpha 🚧

1. Install the package from [NuGet](https://www.nuget.org/packages/Wacton.Unicolour/) 
```
dotnet add package Wacton.Unicolour
```

2. Create a `Unicolour` from RGB or HSB/HSV values:
```c#
using Wacton.Unicolour;
...
var unicolour = Unicolour.FromHex("#FF1493");
var unicolour = Unicolour.FromRgb255(255, 20, 147);
var unicolour = Unicolour.FromRgb(1.0, 0.078, 0.576);
var unicolour = Unicolour.FromHsb(327.6, 0.922, 1.0);
```

3. Get representation of colour in different colour spaces:
```c#
var rgb = unicolour.Rgb;
var hsb = unicolour.Hsb;
var xyz = unicolour.Xyz;
var lab = unicolour.Lab;
```

4. Interpolate between colours:
```c#
var interpolated = unicolour1.InterpolateHsb(unicolour2, 0.5);
```

5. Compare colours:
```c#
var contrast = unicolour1.Contrast(unicolour2);
var difference = unicolour1.DeltaE76(unicolour2);
```

---

[Wacton.Unicolour](https://gitlab.com/Wacton/Unicolour) is licensed under the [MIT License](https://choosealicense.com/licenses/mit/), copyright © 2022 William Acton.
