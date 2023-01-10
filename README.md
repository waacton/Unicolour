# <img src="https://gitlab.com/Wacton/Unicolour/-/raw/main/Unicolour/Resources/Unicolour.png" width="32" height="32"> Unicolour
[![pipeline status](https://gitlab.com/Wacton/Unicolour/badges/main/pipeline.svg)](https://gitlab.com/Wacton/Unicolour/-/commits/main)
[![coverage report](https://gitlab.com/Wacton/Unicolour/badges/main/coverage.svg)](https://gitlab.com/Wacton/Unicolour/-/pipelines)
[![tests passed](https://badgen.net/https/waacton.npkn.net/gitlab-test-badge/)](https://gitlab.com/Wacton/Unicolour/-/pipelines)
[![NuGet](https://badgen.net/nuget/v/Wacton.Unicolour?icon)](https://www.nuget.org/packages/Wacton.Unicolour/)

Unicolour is a small set of utilities for working with colour:
- Colour space conversion
- Colour interpolation
- Colour comparison

A `Unicolour` encapsulates a single colour and its representation across different colour spaces. It supports:
- RGB
- HSB/HSV
- HSL
- HWB
- CIEXYZ
- CIExyY
- CIELAB
- CIELCh<sub>ab</sub>
- CIELUV
- CIELCh<sub>uv</sub>
- HSLuv
- HPLuv
- J<sub>z</sub>a<sub>z</sub>b<sub>z</sub>
- J<sub>z</sub>C<sub>z</sub>h<sub>z</sub>
- Oklab
- Oklch

Unicolour uses sRGB as the default RGB model and standard illuminant D65 (2° observer) as the default white point of the XYZ colour space.
These [can be overridden](#advanced-configuration-) using the `Configuration` parameter.

This library was initially written for personal projects since existing libraries had complex APIs or missing features.
The goal of this library is to be accurate, intuitive, and easy to use.
Although performance is not a priority, conversions are only calculated only once;
when first required — either on access or as part of an intermediate calculation — the result is evaluated and stored.
It is also [extensively tested](Unicolour.Tests), including comparisons against known colour values and other .NET libraries.

Targets [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0) for use in .NET 5.0+, .NET Core 2.0+ and .NET Framework 4.6.1+ applications.

## Quickstart ⚡
| Colour space                            | Construction             | Access         | Interpolation          |
|-----------------------------------------|--------------------------|----------------|------------------------|
| RGB (Hex)                               | `Unicolour.FromHex()`    | `.Hex`         | `.InterpolateRgb()`    |
| RGB (0-255)                             | `Unicolour.FromRgb255()` | `.Rgb.Byte255` | `.InterpolateRgb()`    |
| RGB                                     | `Unicolour.FromRgb()`    | `.Rgb`         | `.InterpolateRgb()`    |
| HSB/HSV                                 | `Unicolour.FromHsb()`    | `.Hsb`         | `.InterpolateHsb()`    |
| HSL                                     | `Unicolour.FromHsl()`    | `.Hsl`         | `.InterpolateHsl()`    |
| HWB                                     | `Unicolour.FromHwb()`    | `.Hwb`         | `.InterpolateHwb()`    |
| CIEXYZ                                  | `Unicolour.FromXyz()`    | `.Xyz`         | `.InterpolateXyz()`    |
| CIExyY                                  | `Unicolour.FromXyy()`    | `.Xyy`         | `.InterpolateXyy()`    |
| CIELAB                                  | `Unicolour.FromLab()`    | `.Lab`         | `.InterpolateLab()`    |
| CIELCh<sub>ab</sub>                     | `Unicolour.FromLchab()`  | `.Lchab`       | `.InterpolateLchab()`  |
| CIELUV                                  | `Unicolour.FromLuv()`    | `.Luv`         | `.InterpolateLuv()`    |
| CIELCh<sub>uv</sub>                     | `Unicolour.FromLchuv()`  | `.Lchuv`       | `.InterpolateLchuv()`  |
| HSLuv                                   | `Unicolour.FromHsluv()`  | `.Hsluv`       | `.InterpolateHsluv()`  |
| HPLuv                                   | `Unicolour.FromHpluv()`  | `.Hpluv`       | `.InterpolateHpluv()`  |
| J<sub>z</sub>a<sub>z</sub>b<sub>z</sub> | `Unicolour.FromJzazbz()` | `.Jzazbz`      | `.InterpolateJzazbz()` |
| J<sub>z</sub>C<sub>z</sub>h<sub>z</sub> | `Unicolour.FromJzczhz()` | `.Jzczhz`      | `.InterpolateJzczhz()` |
| Oklab                                   | `Unicolour.FromOklab()`  | `.Oklab`       | `.InterpolateOklab()`  |
| Oklch                                   | `Unicolour.FromOklch()`  | `.Oklch`       | `.InterpolateOklch()`  |

## How to use 🎨
1. Install the package from [NuGet](https://www.nuget.org/packages/Wacton.Unicolour/)
```
dotnet add package Wacton.Unicolour
```

2. Create a `Unicolour` from values:
```c#
using Wacton.Unicolour;
...
var unicolour = Unicolour.FromHex("#FF1493");
var unicolour = Unicolour.FromRgb255(255, 20, 147);
var unicolour = Unicolour.FromRgb(1.0, 0.078, 0.576);
var unicolour = Unicolour.FromHsb(327.6, 0.922, 1.0);
var unicolour = Unicolour.FromHsl(327.6, 1.0, 0.539);
var unicolour = Unicolour.FromHwb(327.6, 0.078, 0.0);
var unicolour = Unicolour.FromXyz(0.4676, 0.2387, 0.2974);
var unicolour = Unicolour.FromXyy(0.4658, 0.2378, 0.2387)
var unicolour = Unicolour.FromLab(55.96, +84.54, -5.7);
var unicolour = Unicolour.FromLchab(55.96, 84.73, 356.1);
var unicolour = Unicolour.FromLuv(55.96, +131.47, -24.35);
var unicolour = Unicolour.FromLchuv(55.96, 133.71, 349.5);
var unicolour = Unicolour.FromHsluv(349.5, 100, 56);
var unicolour = Unicolour.FromHpluv(349.5, 303.2, 56);
var unicolour = Unicolour.FromJzazbz(0.106, +0.107, +0.005);
var unicolour = Unicolour.FromJzczhz(0.106, 0.107, 2.6);
var unicolour = Unicolour.FromOklab(0.65, 0.26, -0.01);
var unicolour = Unicolour.FromOklch(0.65, 0.26, 356.9);
```

3. Get representation of colour in different colour spaces:
```c#
var rgb = unicolour.Rgb;
var hsb = unicolour.Hsb;
var hsl = unicolour.Hsl;
var hwb = unicolour.Hwb;
var xyz = unicolour.Xyz;
var xyy = unicolour.Xyy;
var lab = unicolour.Lab;
var lchab = unicolour.Lchab;
var luv = unicolour.Luv;
var lchuv = unicolour.Lchuv;
var hsluv = unicolour.Hsluv;
var hpluv = unicolour.Hpluv;
var jzazbz = unicolour.Jzazbz;
var jzczhz = unicolour.Jzczhz;
var oklab = unicolour.Oklab;
var oklch = unicolour.Oklch;
```

4. Interpolate between colours:
```c#
var interpolated = unicolour1.InterpolateRgb(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateHsb(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateHsl(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateHwb(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateXyz(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateXyy(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateLab(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateLchab(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateLuv(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateLchuv(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateHsluv(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateHpluv(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateJzazbz(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateJzczhz(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateOklab(unicolour2, 0.5);
var interpolated = unicolour1.InterpolateOklch(unicolour2, 0.5);
```

5. Compare colours:
```c#
var contrast = unicolour1.Contrast(unicolour2);
var difference = unicolour1.DeltaE76(unicolour2);
var difference = unicolour1.DeltaE94(unicolour2);
var difference = unicolour1.DeltaE00(unicolour2);
var difference = unicolour1.DeltaEz(unicolour2);
```

See also the [example code](Unicolour.Example/Program.cs), which uses `Unicolour` to generate gradients through different colour spaces:
![Gradients generate from Unicolour](Unicolour.Example/gradients.png)

## Advanced configuration 💡
A `Configuration` parameter can be used to change the RGB model (e.g. Adobe RGB, wide-gamut RGB)
and the white point of the XYZ colour space (e.g. D50 reference white used by ICC profiles).

RGB configuration requires red, green, and blue chromaticity coordinates, the companding functions, and the reference white point.
XYZ configuration only requires the reference white point.


```c#
var config = new Configuration(
    new(0.7347, 0.2653), // RGB red chromaticity coordinates
    new(0.1152, 0.8264), // RGB green chromaticity coordinates
    new(0.1566, 0.0177), // RGB blue chromaticity coordinates
    value => Companding.Gamma(value, 2.2),        // RGB companding function
    value => Companding.InverseGamma(value, 2.2), // RGB inverse companding function
    WhitePoint.From(Illuminant.D50),  // RGB white point
    WhitePoint.From(Illuminant.D50)); // XYZ white point
    
var unicolour = Unicolour.FromRgb(config, 255, 20, 147);
```

---

[Wacton.Unicolour](https://gitlab.com/Wacton/Unicolour) is licensed under the [MIT License](https://choosealicense.com/licenses/mit/), copyright © 2022-2023 William Acton.
