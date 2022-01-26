# <img src="https://gitlab.com/Wacton/Unicolour/-/raw/main/Unicolour/Resources/Unicolour.png" width="32" height="32"> Unicolour
Unicolour is a small set of utilities for working with colour:
- Conversion between RGB, HSB/HSV, LAB, and XYZ colour spaces
- Interpolation between colours
- Comparisons of colours

A `Unicolour` encapsulates a single colour and its representation across different colour spaces.

This library was initially written for personal projects since existing libraries had complex APIs or missing features.
The goal of this library is to be intuitive and easy to use; performance is not a priority.

## How to use 🔴🟢🔵
🚧 Still in alpha 🚧

1. Install the package from [NuGet](https://www.nuget.org/packages/Wacton.Unicolour/) [![NuGet](https://img.shields.io/nuget/v/Wacton.Unicolour.svg?maxAge=2592000)](https://www.nuget.org/packages/Wacton.Unicolour/)
```
dotnet add package Wacton.Unicolour
```

2. Create a `Unicolour` from RGB or HSB/HSV values:
```c#
using Wacton.Unicolour;
...
var unicolour = Unicolour.FromRgb(r, g, b, a);
var unicolour = Unicolour.FromHsb(h, s, b, a);
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