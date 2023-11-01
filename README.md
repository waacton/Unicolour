# <img src="https://gitlab.com/Wacton/Unicolour/-/raw/main/Unicolour/Resources/Unicolour.png" width="32" height="32"> Unicolour
[![pipeline status](https://gitlab.com/Wacton/Unicolour/badges/main/pipeline.svg)](https://gitlab.com/Wacton/Unicolour/-/commits/main)
[![coverage report](https://gitlab.com/Wacton/Unicolour/badges/main/coverage.svg)](https://gitlab.com/Wacton/Unicolour/-/pipelines)
[![tests passed](https://badgen.net/https/waacton.npkn.net/gitlab-test-badge/)](https://gitlab.com/Wacton/Unicolour/-/pipelines)
[![NuGet](https://badgen.net/nuget/v/Wacton.Unicolour?icon)](https://www.nuget.org/packages/Wacton.Unicolour/)

Unicolour is a .NET library written in C# for working with colour:
- Colour space conversion
- Colour mixing / colour interpolation
- Colour comparison
- Colour properties
- Colour gamut mapping

## Overview 🧭
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
- IC<sub>T</sub>C<sub>P</sub>
- J<sub>z</sub>a<sub>z</sub>b<sub>z</sub>
- J<sub>z</sub>C<sub>z</sub>h<sub>z</sub>
- Oklab
- Oklch
- CIECAM02
- CAM16
- HCT

<details>
<summary>Diagram of colour space relationships</summary>

![Diagram of colour space relationships](Unicolour/Resources/diagram.png)

This diagram summarises how colour space conversions are implemented in Unicolour.
Arrows indicate forward transformations from one space to another.
For each forward transformation there is a corresponding reverse transformation.
XYZ is considered the root colour space.
</details>

The following colour properties are available on each `Unicolour`:
- Hex representation
- Relative luminance
- Temperature (CCT and Duv)

Unicolour can be used to calculate colour difference via:
- ΔE<sub>76</sub> (CIE76)
- ΔE<sub>94</sub> (CIE94)
- ΔE<sub>00</sub> (CIEDE2000)
- ΔE<sub>ITP</sub>
- ΔE<sub>z</sub>
- ΔE<sub>HyAB</sub>
- ΔE<sub>OK</sub>
- ΔE<sub>CAM02</sub>
- ΔE<sub>CAM16</sub>

Simulation of colour vision deficiency (CVD) / colour blindness is supported for:
- Protanopia (no red perception)
- Deuteranopia (no green perception)
- Tritanopia (no blue perception)
- Achromatopsia (no colour perception)

If a colour is outwith the display gamut, the closest in-gamut colour can be obtained using the provided
gamut mapping algorithm, which meets CSS specifications.

Unicolour uses sRGB as the default RGB model and standard illuminant D65 (2° observer) as the default white point of the XYZ colour space.
These [can be overridden](#advanced-configuration-) using the `Configuration` parameter.

This library was initially written for personal projects since existing libraries had complex APIs or missing features.
The goal of this library is to be accurate, intuitive, and easy to use.
Although performance is not a priority, conversions are only calculated once — when first evaluated (either on access or as part of an intermediate conversion step) the result is stored for future use.
It is also [extensively tested](Unicolour.Tests), including verification of roundtrip conversions and validation using known colour values.

Targets [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0) for use in .NET 5.0+, .NET Core 2.0+ and .NET Framework 4.6.1+ applications.

## Quickstart ⚡
| Colour space                            | Construction             | Access         | Interpolation  |
|-----------------------------------------|--------------------------|----------------|----------------|
| RGB (Hex)                               | `Unicolour.FromHex()`    | `.Hex`         | `.MixRgb()`    |
| RGB (0-255)                             | `Unicolour.FromRgb255()` | `.Rgb.Byte255` | `.MixRgb()`    |
| RGB                                     | `Unicolour.FromRgb()`    | `.Rgb`         | `.MixRgb()`    |
| HSB/HSV                                 | `Unicolour.FromHsb()`    | `.Hsb`         | `.MixHsb()`    |
| HSL                                     | `Unicolour.FromHsl()`    | `.Hsl`         | `.MixHsl()`    |
| HWB                                     | `Unicolour.FromHwb()`    | `.Hwb`         | `.MixHwb()`    |
| CIEXYZ                                  | `Unicolour.FromXyz()`    | `.Xyz`         | `.MixXyz()`    |
| CIExyY                                  | `Unicolour.FromXyy()`    | `.Xyy`         | `.MixXyy()`    |
| CIELAB                                  | `Unicolour.FromLab()`    | `.Lab`         | `.MixLab()`    |
| CIELCh<sub>ab</sub>                     | `Unicolour.FromLchab()`  | `.Lchab`       | `.MixLchab()`  |
| CIELUV                                  | `Unicolour.FromLuv()`    | `.Luv`         | `.MixLuv()`    |
| CIELCh<sub>uv</sub>                     | `Unicolour.FromLchuv()`  | `.Lchuv`       | `.MixLchuv()`  |
| HSLuv                                   | `Unicolour.FromHsluv()`  | `.Hsluv`       | `.MixHsluv()`  |
| HPLuv                                   | `Unicolour.FromHpluv()`  | `.Hpluv`       | `.MixHpluv()`  |
| IC<sub>T</sub>C<sub>P</sub>             | `Unicolour.FromIctcp()`  | `.Ictcp`       | `.MixIctcp()`  |
| J<sub>z</sub>a<sub>z</sub>b<sub>z</sub> | `Unicolour.FromJzazbz()` | `.Jzazbz`      | `.MixJzazbz()` |
| J<sub>z</sub>C<sub>z</sub>h<sub>z</sub> | `Unicolour.FromJzczhz()` | `.Jzczhz`      | `.MixJzczhz()` |
| Oklab                                   | `Unicolour.FromOklab()`  | `.Oklab`       | `.MixOklab()`  |
| Oklch                                   | `Unicolour.FromOklch()`  | `.Oklch`       | `.MixOklch()`  |
| CIECAM02                                | `Unicolour.FromCam02()`  | `.Cam02`       | `.MixCam02()`  |
| CAM16                                   | `Unicolour.FromCam16()`  | `.Cam16`       | `.MixCam16()`  |
| HCT                                     | `Unicolour.FromHct()`    | `.Hct`         | `.MixHct()`    |

## How to use 🌈
1. Install the package from [NuGet](https://www.nuget.org/packages/Wacton.Unicolour/)
```
dotnet add package Wacton.Unicolour
```

2. Import the package
```c#
using Wacton.Unicolour;
```

3. Create a `Unicolour`
```c#
var unicolour = Unicolour.FromHex("#FF1493");
var unicolour = Unicolour.FromRgb255(255, 20, 147);
var unicolour = Unicolour.FromRgb(1.0, 0.08, 0.58);
var unicolour = Unicolour.FromHsb(327.6, 0.922, 1.0);
var unicolour = Unicolour.FromHsl(327.6, 1.0, 0.539);
var unicolour = Unicolour.FromHwb(327.6, 0.078, 0.0);
var unicolour = Unicolour.FromXyz(0.4676, 0.2387, 0.2974);
var unicolour = Unicolour.FromXyy(0.4658, 0.2378, 0.2387);
var unicolour = Unicolour.FromLab(55.96, 84.54, -5.7);
var unicolour = Unicolour.FromLchab(55.96, 84.73, 356.1);
var unicolour = Unicolour.FromLuv(55.96, 131.47, -24.35);
var unicolour = Unicolour.FromLchuv(55.96, 133.71, 349.5);
var unicolour = Unicolour.FromHsluv(349.5, 100, 56);
var unicolour = Unicolour.FromHpluv(349.5, 303.2, 56);
var unicolour = Unicolour.FromIctcp(0.38, 0.12, 0.19);
var unicolour = Unicolour.FromJzazbz(0.106, 0.107, 0.005);
var unicolour = Unicolour.FromJzczhz(0.106, 0.107, 2.6);
var unicolour = Unicolour.FromOklab(0.65, 0.26, -0.01);
var unicolour = Unicolour.FromOklch(0.65, 0.26, 356.9);
var unicolour = Unicolour.FromCam02(62.86, 40.81, -1.18);
var unicolour = Unicolour.FromCam16(62.47, 42.60, -1.36);
var unicolour = Unicolour.FromHct(358.2, 100.38, 55.96);
```

4. Get colour space representations
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
var ictcp = unicolour.Ictcp;
var jzazbz = unicolour.Jzazbz;
var jzczhz = unicolour.Jzczhz;
var oklab = unicolour.Oklab;
var oklch = unicolour.Oklch;
var cam02 = unicolour.Cam02;
var cam16 = unicolour.Cam16;
var hct = unicolour.Hct;
```

5. Get colour properties
```c#
var hex = unicolour.Hex;
var relativeLuminance = unicolour.RelativeLuminance;
var temperature = unicolour.Temperature;
var inGamut = unicolour.IsInDisplayGamut;
```

6. Mix colours (interpolate between them)
```c#
var mixed = unicolour1.MixRgb(unicolour2, 0.5);
var mixed = unicolour1.MixHsb(unicolour2, 0.5);
var mixed = unicolour1.MixHsl(unicolour2, 0.5);
var mixed = unicolour1.MixHwb(unicolour2, 0.5);
var mixed = unicolour1.MixXyz(unicolour2, 0.5);
var mixed = unicolour1.MixXyy(unicolour2, 0.5);
var mixed = unicolour1.MixLab(unicolour2, 0.5);
var mixed = unicolour1.MixLchab(unicolour2, 0.5);
var mixed = unicolour1.MixLuv(unicolour2, 0.5);
var mixed = unicolour1.MixLchuv(unicolour2, 0.5);
var mixed = unicolour1.MixHsluv(unicolour2, 0.5);
var mixed = unicolour1.MixHpluv(unicolour2, 0.5);
var mixed = unicolour1.MixIctcp(unicolour2, 0.5);
var mixed = unicolour1.MixJzazbz(unicolour2, 0.5);
var mixed = unicolour1.MixJzczhz(unicolour2, 0.5);
var mixed = unicolour1.MixOklab(unicolour2, 0.5);
var mixed = unicolour1.MixOklch(unicolour2, 0.5);
var mixed = unicolour1.MixCam02(unicolour2, 0.5);
var mixed = unicolour1.MixCam16(unicolour2, 0.5);
var mixed = unicolour1.MixHct(unicolour2, 0.5);
```

7. Compare colours
```c#
var contrast = unicolour1.Contrast(unicolour2);
var difference = unicolour1.DeltaE76(unicolour2);
var difference = unicolour1.DeltaE94(unicolour2);
var difference = unicolour1.DeltaE00(unicolour2);
var difference = unicolour1.DeltaEItp(unicolour2);
var difference = unicolour1.DeltaEz(unicolour2);
var difference = unicolour1.DeltaEHyab(unicolour2);
var difference = unicolour1.DeltaEOk(unicolour2);
var difference = unicolour1.DeltaECam02(unicolour2);
var difference = unicolour1.DeltaECam16(unicolour2);
```

8. Map to display gamut
```c#
var mapped = unicolour.MapToGamut();
```

9. Simulate colour vision deficiency
```c#
var protanopia = unicolour.SimulateProtanopia();
var deuteranopia = unicolour.SimulateDeuteranopia();
var tritanopia = unicolour.SimulateTritanopia();
var achromatopsia = unicolour.SimulateAchromatopsia();
```

## Examples ✨

This repo contains an [example project](Unicolour.Example/Program.cs) that uses `Unicolour` to generate gradients through different colour spaces
and for different colour vision deficiencies...

![Gradients through different colour spaces generated from Unicolour](Unicolour.Example/gradients.png)

![Gradients for different colour vision deficiencies generated from Unicolour](Unicolour.Example/vision-deficiency.png)

... and a [console application](Unicolour.Console/Program.cs) that uses `Unicolour` to show colour information for a given hex value.

![Colour information from hex value](Unicolour.Console/colour-info.png)

## Advanced configuration 💡
A `Configuration` parameter can be used to customise the RGB model (e.g. Display P3, Rec. 2020)
and the white point of the XYZ colour space (e.g. D50 reference white used by ICC profiles).

- RGB configuration requires red, green, and blue chromaticity coordinates, the reference white point, and the companding functions.
  Default configuration for sRGB, Display P3, and Rec. 2020 is provided.

- XYZ configuration only requires the reference white point.
  Default configuration for D65 and D50 (2° observer) is provided.

```c#
// built-in configuration for Rec. 2020 RGB + D65 XYZ
var config = new Configuration(RgbConfiguration.Rec2020, XyzConfiguration.D65);
var unicolour = Unicolour.FromRgb255(config, 255, 20, 147);
```

```c#
// manual configuration for wide-gamut RGB
var rgbConfig = new RgbConfiguration(
    chromaticityR: new(0.7347, 0.2653),
    chromaticityG: new(0.1152, 0.8264),
    chromaticityB: new(0.1566, 0.0177),
    whitePoint: WhitePoint.From(Illuminant.D50),
    fromLinear: value => Companding.Gamma(value, 2.2),
    toLinear: value => Companding.InverseGamma(value, 2.2)
);

// manual configuration for equal-energy (10° observer) XYZ
var xyzConfig = new XyzConfiguration(
    whitePoint: WhitePoint.From(Illuminant.E, Observer.Supplementary10)
);

var config = new Configuration(rgbConfig, xyzConfig);
var unicolour = Unicolour.FromRgb255(config, 255, 20, 147);
```

Configuration is also available for CAM02 & CAM16 viewing conditions,
IC<sub>T</sub>C<sub>P</sub> scalar,
and J<sub>z</sub>a<sub>z</sub>b<sub>z</sub> scalar.

A `Unicolour` can be converted to a different configuration, which enables conversions between different RGB and XYZ models.

```c#
// pure sRGB green
var srgbConfig = new Configuration(RgbConfiguration.StandardRgb);
var unicolourSrgb = Unicolour.FromRgb(srgbConfig, 0, 1, 0);                         
Console.WriteLine(unicolourSrgb.Rgb); // 0.00 1.00 0.00

// pure sRGB green -> Display P3
var displayP3Config = new Configuration(RgbConfiguration.DisplayP3);
var unicolourDisplayP3 = unicolourSrgb.ConvertToConfiguration(displayP3Config); 
Console.WriteLine(unicolourDisplayP3.Rgb); // 0.46 0.99 0.30

// pure sRGB green -> Rec. 2020
var rec202Config = new Configuration(RgbConfiguration.Rec2020);
var unicolourRec2020 = unicolourDisplayP3.ConvertToConfiguration(rec202Config);
Console.WriteLine(unicolourRec2020.Rgb); // 0.57 0.96 0.27
```

---

[Wacton.Unicolour](https://github.com/waacton/Unicolour) is licensed under the [MIT License](https://choosealicense.com/licenses/mit/), copyright © 2022-2023 William Acton.
