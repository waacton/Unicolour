# <img src="https://gitlab.com/Wacton/Unicolour/-/raw/main/Unicolour/Resources/Unicolour.png" width="32" height="32"> Unicolour
[![GitHub](https://badgen.net/static/github/source/ff1493?icon=github)](https://github.com/waacton/Unicolour)
[![GitLab](https://badgen.net/static/gitlab/source/ff1493?icon=gitlab)](https://gitlab.com/Wacton/Unicolour)
[![NuGet](https://badgen.net/nuget/v/Wacton.Unicolour?icon)](https://www.nuget.org/packages/Wacton.Unicolour/)
[![pipeline status](https://gitlab.com/Wacton/Unicolour/badges/main/pipeline.svg)](https://gitlab.com/Wacton/Unicolour/-/commits/main)
[![tests passed](https://badgen.net/static/tests/237,247/green/)](https://gitlab.com/Wacton/Unicolour/-/pipelines)
[![coverage report](https://gitlab.com/Wacton/Unicolour/badges/main/coverage.svg)](https://gitlab.com/Wacton/Unicolour/-/pipelines)

Unicolour is the most comprehensive .NET library for working with color:
- Color space conversion
- Color mixing / color interpolation
- Color blending
- Color difference / color distance
- Color gamut mapping
- Color chromaticity
- Color temperature
- Wavelength attributes
- Pigments for natural paint mixing
- ICC profiles for CMYK conversion

Written in C# with zero dependencies and fully cross-platform compatible.

Targets [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0) for use in .NET 5.0+, .NET Core 2.0+ and .NET Framework 4.6.1+ applications.

See a [live demo in the browser](https://unicolour.wacton.xyz/colour-picker/) — a color picker for any color space — made with Unicolour.

**Contents**
1. 🧭 [Overview](#-overview)
2. 🔆 [Installation](#-installation)
3. ⚡ [Quickstart](#-quickstart)
4. 🌈 [Features](#-features)
5. 💡 [Configuration](#-configuration)
6. ✨ [Examples](#-examples)
7. 🔮 [Datasets](#-datasets)
8. 🥽 [Experimental](#-experimental)

## 🧭 Overview
A `Unicolour` encapsulates a single color and its representation across [40 color spaces](#convert-between-color-spaces).
It can be used to [mix](#mix-colors) and [compare](#compare-colors) colors, and offers [many useful features](#-features) for working with color.


> **Supported color spaces**
>
> RGB · Linear&nbsp;RGB · HSB&nbsp;/&nbsp;HSV · HSL · HWB · HSI ·
> CIEXYZ · CIExyY · WXY ·
> CIELAB · CIELCh<sub>ab</sub> · CIELUV · CIELCh<sub>uv</sub> · HSLuv · HPLuv ·
> YPbPr · YCbCr&nbsp;/&nbsp;YUV&nbsp;_(digital)_ · YCgCo · YUV&nbsp;_(PAL)_ · YIQ&nbsp;_(NTSC)_ · YDbDr&nbsp;_(SECAM)_ ·
> TSL · XYB ·
> LMS · IPT · IC<sub>T</sub>C<sub>P</sub> · J<sub>z</sub>a<sub>z</sub>b<sub>z</sub> · J<sub>z</sub>C<sub>z</sub>h<sub>z</sub> ·
> Oklab · Oklch · Okhsv · Okhsl · Okhwb · Okl<sub>r</sub>ab · Okl<sub>r</sub>ch ·
> CIECAM02 · CAM16 ·
> HCT ·
> Munsell HVC ·
> CMYK&nbsp;/&nbsp;ICC&nbsp;Profile <sup>[?](#use-icc-profiles-for-cmyk-conversion)</sup>
> ```c#
> Unicolour pink = new("#FF1493");
> Console.WriteLine(pink.Oklab); // 0.65 +0.26 -0.01
> ```

This library was initially written for personal projects since existing libraries had complex APIs, missing features, or inaccurate conversions.
The goal of this library is to be [accurate, intuitive, and easy to use](#-quickstart).
Although performance is not a priority, conversions are only calculated once; when first evaluated (either on access or as part of an intermediate conversion step) the result is stored for future use.

Unicolour is [extensively tested](https://github.com/waacton/Unicolour/tree/main/Unicolour.Tests), including verification of roundtrip conversions, validation using known color values, and 100% line coverage and branch coverage.

## 🔆 Installation
1. Install the package from [NuGet](https://www.nuget.org/packages/Wacton.Unicolour/)
```
dotnet add package Wacton.Unicolour
```

2. Import the package
```c#
using Wacton.Unicolour;
```

3. Use the package
```c#
Unicolour color = new(ColourSpace.Rgb255, 192, 255, 238);
```

## ⚡ Quickstart
The simplest way to get started is to make a `Unicolour` and use it to see how the color is [represented in a different color space](#convert-between-color-spaces).
```c#
var cyan = new Unicolour("#00FFFF");
Console.WriteLine(cyan.Hsl); // 180.0° 100.0% 50.0%

var yellow = new Unicolour(ColourSpace.Rgb255, 255, 255, 0);
Console.WriteLine(yellow.Hex); // #FFFF00
```

Colors can be [mixed or interpolated](#mix-colors) using any color space.
```c#
var red = new Unicolour(ColourSpace.Rgb, 1.0, 0.0, 0.0);
var blue = new Unicolour(ColourSpace.Hsb, 240, 1.0, 1.0);

/* RGB: [1, 0, 0] ⟶ [0, 0, 1] = [0.5, 0, 0.5] */
var purple = red.Mix(blue, ColourSpace.Rgb);
Console.WriteLine(purple.Rgb); // 0.50 0.00 0.50
Console.WriteLine(purple.Hex); // #800080

/* HSL: [0, 1, 0.5] ⟶ [240, 1, 0.5] = [300, 1, 0.5] */
var magenta = red.Mix(blue, ColourSpace.Hsl);
Console.WriteLine(magenta.Rgb); // 1.00 0.00 1.00
Console.WriteLine(magenta.Hex); // #FF00FF

// #FF0000, #FF0080, #FF00FF, #8000FF, #0000FF
var palette = red.Palette(blue, ColourSpace.Hsl, 5);
Console.WriteLine(palette.Select(colour => color.Hex));
```

The [difference or distance](#compare-colors) between colors can be calculated using any delta E metric.
```c#
var white = new Unicolour(ColourSpace.Oklab, 1.0, 0.0, 0.0);
var black = new Unicolour(ColourSpace.Oklab, 0.0, 0.0, 0.0);
var difference = white.Difference(black, DeltaE.Ciede2000);
Console.WriteLine(difference); // 100.0000
```

Other useful color information is available, such as [chromaticity coordinates](#access-colorimetric-components),
[temperature](#derive-temperature-metrics), and [dominant wavelength](#get-wavelength-attributes).
```c#
var equalEnergy = new Unicolour(ColourSpace.Xyz, 0.5, 0.5, 0.5);
Console.WriteLine(equalEnergy.RelativeLuminance);  // 0.5
Console.WriteLine(equalEnergy.Chromaticity.Xy);    // (0.3333, 0.3333)
Console.WriteLine(equalEnergy.Chromaticity.Uv);    // (0.2105, 0.3158)
Console.WriteLine(equalEnergy.Temperature);        // 5455.5 K (Δuv -0.00442)
Console.WriteLine(equalEnergy.DominantWavelength); // 596.1
```

Reference white points (e.g. D65) and the RGB model (e.g. sRGB) [can be configured](#-configuration).

## 🌈 Features

### Convert between color spaces
Unicolour calculates all transformations required to convert from one color space to any other,
so there is no need to manually chain multiple functions and removes the risk of rounding errors.
```c#
Unicolour color = new(ColourSpace.Rgb255, 192, 255, 238);
var (l, c, h) = color.Oklch;
```


>
> RGB colors can also be constructed using their hex values:
> ```c#
> Unicolour pink = new("ff1493");
> var hex = pink.Hex; // #FF1493
> ```

| Color&nbsp;space                                                                       | Enum                    | Property       |
|-----------------------------------------------------------------------------------------|-------------------------|----------------|
| RGB&nbsp;(0–255)                                                                        | `ColourSpace.Rgb255`    | `.Rgb.Byte255` |
| RGB                                                                                     | `ColourSpace.Rgb`       | `.Rgb`         |
| Linear&nbsp;RGB                                                                         | `ColourSpace.RgbLinear` | `.RgbLinear`   |
| HSB&nbsp;/&nbsp;HSV                                                                     | `ColourSpace.Hsb`       | `.Hsb`         |
| HSL                                                                                     | `ColourSpace.Hsl`       | `.Hsl`         |
| HWB                                                                                     | `ColourSpace.Hwb`       | `.Hwb`         |
| HSI                                                                                     | `ColourSpace.Hsi`       | `.Hsi`         |
| CIEXYZ                                                                                  | `ColourSpace.Xyz`       | `.Xyz`         |
| CIExyY                                                                                  | `ColourSpace.Xyy`       | `.Xyy`         |
| [WXY](https://unicolour.wacton.xyz/wxy-colour-space)                                    | `ColourSpace.Wxy`       | `.Wxy`         |
| CIELAB                                                                                  | `ColourSpace.Lab`       | `.Lab`         |
| CIELCh<sub>ab</sub>                                                                     | `ColourSpace.Lchab`     | `.Lchab`       |
| CIELUV                                                                                  | `ColourSpace.Luv`       | `.Luv`         |
| CIELCh<sub>uv</sub>                                                                     | `ColourSpace.Lchuv`     | `.Lchuv`       |
| HSLuv                                                                                   | `ColourSpace.Hsluv`     | `.Hsluv`       |
| HPLuv                                                                                   | `ColourSpace.Hpluv`     | `.Hpluv`       |
| YPbPr                                                                                   | `ColourSpace.Ypbpr`     | `.Ypbpr`       |
| YCbCr&nbsp;/&nbsp;YUV&nbsp;_(digital)_                                                  | `ColourSpace.Ycbcr`     | `.Ycbcr`       |
| YCgCo                                                                                   | `ColourSpace.Ycgco`     | `.Ycgco`       |
| YUV&nbsp;_(PAL)_                                                                        | `ColourSpace.Yuv`       | `.Yuv`         |
| YIQ&nbsp;_(NTSC)_                                                                       | `ColourSpace.Yiq`       | `.Yiq`         |
| YDbDr&nbsp;_(SECAM)_                                                                    | `ColourSpace.Ydbdr`     | `.Ydbdr`       |
| TSL                                                                                     | `ColourSpace.Tsl`       | `.Tsl`         |
| XYB                                                                                     | `ColourSpace.Xyb`       | `.Xyb`         |
| LMS                                                                                     | `ColourSpace.Lms`       | `.Lms`         |
| IPT                                                                                     | `ColourSpace.Ipt`       | `.Ipt`         |
| IC<sub>T</sub>C<sub>P</sub>                                                             | `ColourSpace.Ictcp`     | `.Ictcp`       |
| J<sub>z</sub>a<sub>z</sub>b<sub>z</sub>                                                 | `ColourSpace.Jzazbz`    | `.Jzazbz`      |
| J<sub>z</sub>C<sub>z</sub>h<sub>z</sub>                                                 | `ColourSpace.Jzczhz`    | `.Jzczhz`      |
| Oklab                                                                                   | `ColourSpace.Oklab`     | `.Oklab`       |
| Oklch                                                                                   | `ColourSpace.Oklch`     | `.Oklch`       |
| Okhsv                                                                                   | `ColourSpace.Okhsv`     | `.Okhsv`       |
| Okhsl                                                                                   | `ColourSpace.Okhsl`     | `.Okhsl`       |
| Okhwb                                                                                   | `ColourSpace.Okhwb`     | `.Okhwb`       |
| Okl<sub>r</sub>ab                                                                       | `ColourSpace.Oklrab`    | `.Oklrab`      |
| Okl<sub>r</sub>ch                                                                       | `ColourSpace.Oklrch`    | `.Oklrch`      |
| CIECAM02                                                                                | `ColourSpace.Cam02`     | `.Cam02`       |
| CAM16                                                                                   | `ColourSpace.Cam16`     | `.Cam16`       |
| HCT                                                                                     | `ColourSpace.Hct`       | `.Hct`         |
| Munsell HVC                                                                             | `ColourSpace.Munsell`   | `.Munsell`     |
| CMYK&nbsp;/&nbsp;ICC&nbsp;Profile <sup>[?](#use-icc-profiles-for-cmyk-conversion)</sup> | -                       | `.Icc`         |




>
> Munsell HVC colors are defined by 4 attributes, but are managed in Unicolour using 3.
> The Munsell hue notation is mapped to conventional degrees, with 5R at 0° and 360° and 5BG at 180°.
> This mapping is accessible via the `Hue.FromMunsell()` utility function, e.g. for Munsell color 6.1RP 5.5/19.5:
> ```c#
> Unicolour pink = new(ColourSpace.Munsell, Hue.FromMunsell(6.1, "RP"), 5.5, 19.5);
> Console.WriteLine(pink.Munsell); // 6.1RP 5.5/19.5
> ```

### Mix colors
Two colors can be mixed by [interpolating between them in any color space](#gradients),
taking into account cyclic hue, interpolation distance, and alpha premultiplication.
Palettes provide a range of evenly distributed mixes of two colors.
```c#
var red = new Unicolour(ColourSpace.Rgb, 1.0, 0.0, 0.0, alpha: 1.0);
var blue = new Unicolour(ColourSpace.Hsb, 240, 1.0, 1.0, alpha: 1.0);
var magenta = red.Mix(blue, ColourSpace.Hsl, 0.5, HueSpan.Decreasing);
var green = red.Mix(blue, ColourSpace.Hsl, 0.5, HueSpan.Increasing);
var palette = red.Palette(blue, ColourSpace.Hsl, 10, HueSpan.Longer);
```

| Hue&nbsp;span                  | Enum                 |
|--------------------------------|----------------------|
| Shorter&nbsp;👈&nbsp;_default_ | `HueSpan.Shorter`    |
| Longer                         | `HueSpan.Longer`     |
| Increasing                     | `HueSpan.Increasing` |
| Decreasing                     | `HueSpan.Decreasing` |

### Blend colors
Two colors can be blended as though they are layered elements. Compositing is performed using the source-over operator.
```c#
var blue = new Unicolour(ColourSpace.Rgb, 240, 1.0, 1.0, alpha: 0.5);
var red = new Unicolour(ColourSpace.Rgb, 1.0, 0.0, 0.0);
var purple = blue.Blend(red, BlendMode.Normal);
var pink = blue.Blend(red, BlendMode.Screen);
```

| Blend&nbsp;mode   | Enum                     |
|-------------------|--------------------------|
| Normal            | `BlendMode.Normal`       |
| Multiply          | `BlendMode.Multiply`     |
| Screen            | `BlendMode.Screen`       |
| Overlay           | `BlendMode.Overlay`      |
| Darken            | `BlendMode.Darken`       |
| Lighten           | `BlendMode.Lighten`      |
| Color&nbsp;Dodge | `BlendMode.ColourDodge`  |
| Color&nbsp;Burn  | `BlendMode.ColourBurn`   |
| Hard&nbsp;Light   | `BlendMode.HardLight`    |
| Soft&nbsp;Light   | `BlendMode.SoftLight`    |
| Difference        | `BlendMode.Difference`   |
| Exclusion         | `BlendMode.Exclusion`    |
| Hue               | `BlendMode.Hue`          |
| Saturation        | `BlendMode.Saturation`   |
| Color            | `BlendMode.Colour`       |
| Luminosity        | `BlendMode.Luminosity`   |

### Compare colors
Two methods of comparing colors are available: contrast and difference.
Difference is calculated according to a specific delta E (ΔE) metric.
```c#
var red = new Unicolour(ColourSpace.Rgb, 1.0, 0.0, 0.0);
var blue = new Unicolour(ColourSpace.Hsb, 240, 1.0, 1.0);
var contrast = red.Contrast(blue);
var difference = red.Difference(blue, DeltaE.Cie76);
```

| Delta&nbsp;E                                                      | Enum                       |
|-------------------------------------------------------------------|----------------------------|
| ΔE<sub>76</sub>&nbsp;(CIE76)                                      | `DeltaE.Cie76`             |
| ΔE<sub>94</sub>&nbsp;(CIE94)&nbsp;graphic&nbsp;arts               | `DeltaE.Cie94`             |
| ΔE<sub>94</sub>&nbsp;(CIE94)&nbsp;textiles                        | `DeltaE.Cie94Textiles`     |
| ΔE<sub>00</sub>&nbsp;(CIEDE2000)                                  | `DeltaE.Ciede2000`         |
| ΔE<sub>CMC</sub>&nbsp;(CMC&nbsp;l:c)&nbsp;2:1&nbsp;acceptability  | `DeltaE.CmcAcceptability`  |
| ΔE<sub>CMC</sub>&nbsp;(CMC&nbsp;l:c)&nbsp;1:1&nbsp;perceptibility | `DeltaE.CmcPerceptibility` |
| ΔE<sub>ITP</sub>                                                  | `DeltaE.Itp`               |
| ΔE<sub>z</sub>                                                    | `DeltaE.Z`                 |
| ΔE<sub>HyAB</sub>                                                 | `DeltaE.Hyab`              |
| ΔE<sub>OK</sub>                                                   | `DeltaE.Ok`                |
| ΔE<sub>CAM02</sub>                                                | `DeltaE.Cam02`             |
| ΔE<sub>CAM16</sub>                                                | `DeltaE.Cam16`             |

### Map color into gamut
Colors that cannot be displayed with the [configured RGB model](#rgbconfiguration) can be mapped to the closest in-gamut RGB color.
Mapping to Pointer's gamut will return the closest empirically real surface color of the same lightness and hue.
Mapping to MacAdam limits will return the closest theoretically real surface color of the same wavelength and luminance.
```c#
var veryRed = new Unicolour(ColourSpace.Rgb, 1.25, -0.39, -0.14);

var isDisplayable = veryRed.IsInRgbGamut;
var displayRed = veryRed.MapToRgbGamut();

var isEmpiricalSurface = veryRed.IsInPointerGamut;
var empiricalRed = veryRed.MapToPointerGamut();

var isTheoreticalSurface = veryRed.IsInMacAdamLimits;
var theoreticalRed = veryRed.MapToMacAdamLimits();
```

| RGB&nbsp;gamut&nbsp;mapping&nbsp;method                                                                 | Enum                            |
|---------------------------------------------------------------------------------------------------------|---------------------------------|
| RGB&nbsp;clipping                                                                                       | `GamutMap.RgbClipping`          |
| Oklch&nbsp;chroma&nbsp;reduction&nbsp;(CSS&nbsp;specification)&nbsp;👈&nbsp;_default_                   | `GamutMap.OklchChromaReduction` |
| [WXY&nbsp;purity&nbsp;reduction](https://unicolour.wacton.xyz/wxy-colour-space#%EF%B8%8F-gamut-mapping) | `GamutMap.WxyPurityReduction `  |

### Simulate color vision deficiency
Color vision deficiency (CVD) or color blindness can be simulated, conveying how a particular color might be perceived.
Anomalous trichromacy, where cones are defective instead of missing, can be adjusted using the severity parameter.
```c#
var color = new Unicolour(ColourSpace.Rgb255, 192, 255, 238);
var missingRed = color.Simulate(Cvd.Protanopia);
var defectiveRed = color.Simulate(Cvd.Protanomaly, 0.5);
```

| Color&nbsp;vision&nbsp;deficiency                                                    | Enum                       |
|---------------------------------------------------------------------------------------|----------------------------|
| Protanopia&nbsp;(missing&nbsp;red&nbsp;cones)                                         | `Cvd.Protanopia`           |
| Protanomaly&nbsp;(defective&nbsp;red&nbsp;cones)                                      | `Cvd.Protanomaly`          |
| Deuteranopia&nbsp;(missing&nbsp;green&nbsp;cones)                                     | `Cvd.Deuteranopia`         |
| Deuteranomaly&nbsp;(defective&nbsp;green&nbsp;cones)                                  | `Cvd.Deuteranomaly`        |
| Tritanopia&nbsp;(missing&nbsp;blue&nbsp;cones)                                        | `Cvd.Tritanopia`           |
| Tritanomaly&nbsp;(defective&nbsp;blue&nbsp;cones)                                     | `Cvd.Tritanomaly`          |
| Blue&nbsp;cone&nbsp;monochromacy&nbsp;(missing&nbsp;red&nbsp;&&nbsp;green&nbsp;cones) | `Cvd.BlueConeMonochromacy` |
| Achromatopsia&nbsp;(missing&nbsp;all&nbsp;cones)                                      | `Cvd.Achromatopsia`        |

### Access colorimetric components
Notable colorimetric components are conveniently accessible, and can be used to create a color.
```c#
var gray = new Unicolour(ColourSpace.RgbLinear, 0.5, 0.5, 0.5);
var chromaticity = gray.Chromaticity;
var luminance = gray.RelativeLuminance;

var white = new Unicolour(chromaticity, luminance: 1.0);
```

### Derive temperature metrics
Correlated color temperature (CCT) and delta UV (∆<sub>uv</sub>) can be derived from a color, and can be used to create a color.
CCT from 500 K to 1,000,000,000 K is supported but only CCT from 1,000 K to 20,000 K is guaranteed to have high accuracy.
```c#
var chromaticity = new Chromaticity(0.3457, 0.3585);
var d50 = new Unicolour(chromaticity);
var (cct, duv) = d50.Temperature;

var temperature = new Temperature(6504, 0.0032);
var d65 = new Unicolour(temperature);
var (x, y) = d65.Chromaticity;
```

### Get wavelength attributes
The dominant wavelength and excitation purity of a color can be ascertained using the spectral locus.
They can be used to create a color alongside the [WXY color space](https://unicolour.wacton.xyz/wxy-colour-space).
Wavelengths from 360 nm to 700 nm are supported.
```c#
var chromaticity = new Chromaticity(0.1, 0.8);
var hyperGreen = new Unicolour(chromaticity);
var dominantWavelength = hyperGreen.DominantWavelength;
var excitationPurity = hyperGreen.ExcitationPurity;

var laserRed = new Unicolour(ColourSpace.Wxy, 670, 1.0, 0.5);
```

### Detect imaginary colors
Colors that lie outside the spectral locus — 
the horseshoe-shaped curve of the [CIE xy chromaticity diagram](#diagrams) — 
cannot be produced by the eye.
These imaginary colors are mathematically possible and can be detected.
```c#
var chromaticity = new Chromaticity(0.05, 0.05);
var impossibleBlue = new Unicolour(chromaticity);
var isImaginary = impossibleBlue.IsImaginary;
```

### Interpret spectral power distributions
A color can be created from a spectral power distribution (SPD).
Wavelengths should be provided in either 1 nm or 5 nm intervals, and omitted wavelengths are assumed to have zero spectral power.
```c#
/* [575 nm] -> 0.5 · [580 nm] -> 1.0 · [585 nm] -> 0.5 */
var spd = new Spd(start: 575, interval: 5, coefficients: [0.5, 1.0, 0.5]);
var intenseYellow = new Unicolour(spd);
```

### Model pigment and paint colors
Pigments can be combined using the Kubelka-Munk theory. The result is a color that reflects natural paint mixing.
Pigment measurements are required, either coefficients for absorption _k_ and scattering _s_ (two-constant) or a reflectance curve _r_ (single-constant).
Saunderson correction can be applied when using _k_ and _s_ and assumes measurements were taken in SPEX mode.
```c#
/* populate k and s with measurement data */
var phthaloBlue = new Pigment(startWavelength: 380, wavelengthInterval: 10, k: [], s: []);
var hansaYellow = new Pigment(startWavelength: 380, wavelengthInterval: 10, k: [], s: []);
var green = new Unicolour(pigments: [phthaloBlue, hansaYellow], weights: [0.5, 0.5]);
```

### Use ICC profiles for CMYK conversion
Device-dependent color prints of 4 (e.g. FOGRA39 CMYK) or more (e.g. FOGRA55 CMYKOGV) are supported through ICC profiles.
If no ICC profile is provided, or if the profile is incompatible, naive conversion for uncalibrated CMYK is used instead.
```c#
using Wacton.Unicolour.Icc;

var fogra39 = new IccConfiguration("./Fogra39.icc", Intent.RelativeColorimetric);
var config = new Configuration(iccConfig: fogra39);

var navyRgb = new Unicolour(config, ColourSpace.Rgb255, 0, 0, 128);
Console.WriteLine(navyRgb.Icc); // 1.0000 0.8977 0.0001 0.2867 CMYK

var navyCmyk = new Unicolour(config, new Channels(1.0, 1.0, 0.0, 0.5));
Console.WriteLine(navyCmyk.Rgb.Byte255); // 46 37 87
```

The following tables summarise which ICC profiles are compatible with Unicolour:

|   | Profile version   |
|---|-------------------|
| ✅ | 2                 |
| ✅ | 4                 |
| ❌ | 5 / iccMAX        |

|   | Profile/device class |
|---|----------------------|
| ✅ | Input `scnr`         |
| ✅ | Display `mntr`       |
| ✅ | Output `prtr`        |
| ✅ | ColorSpace `spac`    |
| ❌ | DeviceLink `link`    |
| ❌ | Abstract `abst`      |
| ❌ | NamedColor `nmcl`    |

|   | Transform                                                           |
|---|---------------------------------------------------------------------|
| ✅ | AToB / BToA `A2B0` `A2B1` `A2B2` `B2A0` `B2A1` `B2A2`               |
| ✅ | TRC matrix `rTRC` `gTRC` `bTRC` `rXYZ` `gXYZ` `bXYZ`                |
| ✅ | TRC gray `kTRC`                                                     |
| ❌ | DToB / BToD `D2B0` `D2B1` `D2B2` `D2B3` `B2D0` `B2D1` `B2D2` `B2D3` |

A wider variety of ICC profiles will be supported in future releases.
If a problem is encountered using an ICC profile that meets the above criteria, please [raise an issue](https://github.com/waacton/Unicolour/issues).

### Handle invalid values
It is possible for invalid or unreasonable values to be used in calculations,
either because conversion formulas have limitations or because a user passes them as arguments.
Although these values don't make sense to use, they should propagate safely and avoid triggering exceptions.
```c#
var bad1 = new Unicolour(ColourSpace.Oklab, double.NegativeInfinity, double.NaN, double.Epsilon);
var bad2 = new Unicolour(ColourSpace.Cam16, double.NaN, double.MaxValue, double.MinValue);
var bad3 = bad1.Mix(bad2, ColourSpace.Hct, amount: double.PositiveInfinity);
```

### Sensible defaults, highly configurable
Unicolour uses sRGB as the default RGB model and standard illuminant D65 (2° observer) as the default white point of all color spaces,
ensuring consistency and a suitable starting point for simple applications.
These [can be overridden](#-configuration) using the `Configuration` parameter, and common configurations have been predefined.
```c#
var defaultConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
var color = new Unicolour(defaultConfig, ColourSpace.Rgb255, 192, 255, 238);
```

### Zero dependencies, quality controlled
Each line of artisan code is exquisitely handcrafted in small-batch programming sessions.
No dependencies are used, so there is no risk of reliance on deprecated, obsolete, or unmaintained packages.
Every line of code is tested, and any defect is [Unicolour's responsibility](https://i.giphy.com/pDsCoECKh1Pa.webp).

## 💡 Configuration
The `Configuration` parameter can be used to define the context of the color.

Example configuration with predefined
- Rec. 2020 RGB
- Illuminant D50 (2° observer) XYZ

```c#
Configuration config = new(RgbConfiguration.Rec2020, XyzConfiguration.D50);
Unicolour color = new(config, ColourSpace.Rgb255, 204, 64, 132);
```

Example configuration with manually defined
- Wide-gamut RGB
- Illuminant C (10° observer) XYZ, using Von Kries method for white point adaptation

```c#
var rgbConfig = new RgbConfiguration(
    chromaticityR: new(0.7347, 0.2653),
    chromaticityG: new(0.1152, 0.8264),
    chromaticityB: new(0.1566, 0.0177),
    whitePoint: Illuminant.D50.GetWhitePoint(Observer.Degree2),
    fromLinear: value => Math.Pow(value, 1 / 2.19921875),
    toLinear: value => Math.Pow(value, 2.19921875)
);

var xyzConfig = new XyzConfiguration(Illuminant.C, Observer.Degree10, Adaptation.VonKries);

var config = new Configuration(rgbConfig, xyzConfig);
var color = new Unicolour(config, ColourSpace.Rgb255, 202, 97, 143);
```

A `Configuration` is composed of sub-configurations.
Each sub-configuration is optional and will fall back to a [sensible default](#sensible-defaults-highly-configurable) if not provided.

### `RgbConfiguration`
Defines the RGB color space parameters, often used to specify a wider gamut than standard RGB (sRGB).

| Predefined                           | Property         |
|--------------------------------------|------------------|
| sRGB&nbsp;👈&nbsp;_default_          | `.StandardRgb`   |
| Display&nbsp;P3                      | `.DisplayP3`     |
| Rec.&nbsp;2020                       | `.Rec2020`       |
| Rec.&nbsp;2100&nbsp;PQ               | `.Rec2100Pq`     |
| Rec.&nbsp;2100&nbsp;HLG              | `.Rec2100Hlg`    |
| A98                                  | `.A98`           |
| ProPhoto                             | `.ProPhoto`      |
| ACES&nbsp;2065-1                     | `.Aces20651`     |
| ACEScg                               | `.Acescg`        |
| ACEScct                              | `.Acescct`       |
| ACEScc                               | `.Acescc`        |
| Rec.&nbsp;601&nbsp;(625-line)        | `.Rec601Line625` |
| Rec.&nbsp;601&nbsp;(525-line)        | `.Rec601Line525` |
| Rec.&nbsp;709                        | `.Rec709`        |
| xvYCC                                | `.XvYcc`         |
| PAL&nbsp;(Rec.&nbsp;470)             | `.Pal`           |
| PAL-M&nbsp;(Rec.&nbsp;470)           | `.PalM`          |
| PAL&nbsp;625&nbsp;(Rec.&nbsp;1700)   | `.Pal625`        |
| PAL&nbsp;525&nbsp;(Rec.&nbsp;1700)   | `.Pal525`        |
| NTSC&nbsp;(Rec.&nbsp;470)            | `.Ntsc`          |
| NTSC&nbsp;(SMPTE-C)                  | `.NtscSmpteC`    |
| NTSC&nbsp;525&nbsp;(Rec.&nbsp;1700)  | `.Ntsc525`       |
| SECAM&nbsp;(Rec.&nbsp;470)           | `.Secam`         |
| SECAM&nbsp;625&nbsp;(Rec.&nbsp;1700) | `.Secam625`      |



- Parameters
  - Red, green, and blue chromaticity coordinates
  - Reference white point
  - Companding functions to and from linear values

### `XyzConfiguration`
Defines the XYZ white point (which is also [inherited by color spaces that do not need a specific configuration](#white-points)),
the observer to use when color matching functions (CMFs) are required,
and the chromatic adaptation matrix to use for any white point adaptation (the Bradford method will be used if unspecified).

| Predefined                                         | Property  |
|----------------------------------------------------|-----------|
| D65&nbsp;(2°&nbsp;observer)&nbsp;👈&nbsp;_default_ | `.D65`    |
| D50&nbsp;(2°&nbsp;observer)                        | `.D50`    |

- Parameters
  - Reference white point or illuminant
  - Observer
  - Chromatic adaptation matrix

### `YbrConfiguration`
Defines the constants, scaling, and offsets required to convert to YPbPr and YCbCr.

| Predefined                           | Property   |
|--------------------------------------|------------|
| Rec.&nbsp;601&nbsp;👈&nbsp;_default_ | `.Rec601`  |
| Rec.&nbsp;709                        | `.Rec709`  |
| Rec.&nbsp;2020                       | `.Rec2020` |
| JPEG                                 | `.Jpeg`    |

- Parameters
  - Luma constants for component video separation
  - Mapping ranges for digital encoding

### `CamConfiguration`
Defines the viewing conditions for CAM02 and CAM16, which take into account the surrounding environment to determine how a color is perceived.

| Predefined                  | Property       |
|-----------------------------|----------------|
| sRGB&nbsp;👈&nbsp;_default_ | `.StandardRgb` |
| HCT                         | `.Hct`         |

The predefined sRGB configuration refers to an ambient illumination of 64 lux under a gray world assumption.

- Parameters
  - Reference white point
  - Adapting luminance
  - Background luminance

### `DynamicRange`
Defines luminance values used when evaluating
perceptual quantizer (PQ) transfer functions (IC<sub>T</sub>C<sub>P</sub> · J<sub>z</sub>a<sub>z</sub>b<sub>z</sub> · J<sub>z</sub>C<sub>z</sub>h<sub>z</sub> · Rec. 2100 PQ RGB)
and hybrid log-gamma (HLG) transfer functions (Rec. 2100 HLG RGB).

| Predefined                 | Property    |
|----------------------------|-------------|
| SDR                        | `.Standard` |
| HDR&nbsp;👈&nbsp;_default_ | `.High`     |

The predefined HDR configuration has a white luminance of 203 cd/m² at 75% HLG, and a minimum luminance of 0 cd/m² (no black lift).

- Parameters
  - White luminance
  - Maximum luminance
  - Minimum luminance
  - HLG % white level

### `IccConfiguration`
Defines the ICC profile and rendering intent, typically used for accurate CMYK conversion.

| Predefined                  | Property |
|-----------------------------|----------|
| None&nbsp;👈&nbsp;_default_ | `.None`  |

Unicolour does not embed or distribute ICC profiles.
Some commonly used profiles can be found in the [ICC profile registry](https://www.color.org/registry/index.xalter).

- Parameters
  - ICC profile (`.icc` file)
  - Rendering intent

### White points
Most color spaces are impacted by the reference white point.
Unicolour applies different reference white points to different sets of color spaces, as shown in the table below.
When a [conversion to or from XYZ space](#convert-between-color-spaces) involves a change in white point, a chromatic adaptation transform (CAT) is performed.
The default chromatic adaptation is the Bradford method but [this can be customised](#xyzconfiguration).

| White&nbsp;point&nbsp;configuration | Affected&nbsp;color&nbsp;spaces                                                                                                                                                                            |
|-------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `RgbConfiguration`                  | RGB · Linear&nbsp;RGB · HSB&nbsp;/&nbsp;HSV · HSL · HWB · HSI · YPbPr · YCbCr&nbsp;/&nbsp;YUV&nbsp;_(digital)_ · YCgCo · YUV&nbsp;_(PAL)_ · YIQ&nbsp;_(NTSC)_ · YDbDr&nbsp;_(SECAM)_ · TSL · XYB            |
| `XyzConfiguration`                  | CIEXYZ · CIExyY · WXY · CIELAB · CIELCh<sub>ab</sub> · CIELUV · CIELCh<sub>uv</sub> · HSLuv · HPLuv                                                                                                         |
| `CamConfiguration`                  | CIECAM02 · CAM16                                                                                                                                                                                            |
| None (always D65/2°)                | IPT · IC<sub>T</sub>C<sub>P</sub> · J<sub>z</sub>a<sub>z</sub>b<sub>z</sub> · J<sub>z</sub>C<sub>z</sub>h<sub>z</sub> · Oklab · Oklch · Okhsv · Okhsl · Okhwb · Okl<sub>r</sub>ab · Okl<sub>r</sub>ch · HCT |
| None (always C/2°)                  | Munsell HVC                                                                                                                                                                                                 |

### Convert between configurations
A `Unicolour` can be converted to a different configuration,
in turn enabling conversions between different RGB models, XYZ white points, CAM viewing conditions, ICC profiles, etc.

```c#
/* pure sRGB green */
var srgbConfig = new Configuration(RgbConfiguration.StandardRgb);
var srgbColor = new Unicolour(srgbConfig, ColourSpace.Rgb, 0, 1, 0);
Console.WriteLine(srgbColour.Rgb); // 0.00 1.00 0.00

/* ⟶ Display P3 */
var displayP3Config = new Configuration(RgbConfiguration.DisplayP3);
var displayP3Color = srgbColour.ConvertToConfiguration(displayP3Config);
Console.WriteLine(displayP3Colour.Rgb); // 0.46 0.99 0.30

/* ⟶ Rec. 2020 */
var rec2020Config = new Configuration(RgbConfiguration.Rec2020);
var rec2020Color = displayP3Colour.ConvertToConfiguration(rec2020Config);
Console.WriteLine(rec2020Colour.Rgb); // 0.57 0.96 0.27
```

## ✨ Examples
This repository contains projects showing how Unicolour can be used to create:
1. [Gradient images](#gradients)
2. [Heatmaps of luminance](#heatmaps)
3. [Diagrams of color data](#diagrams)
4. [A colorful console application](#console)
5. [A color picker web application](#web)
6. [3D visualizations of color spaces in Unity](#unity)

### Gradients
Example code to create gradient images using 📷 [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp)
can be seen in the [Example.Gradients](https://github.com/waacton/Unicolour/tree/main/Example.Gradients/Program.cs) project.

| ![Smooth gradient of deep pink to aquamarine gradient, created with Unicolour](gradient-simple-mixing.png) | ![Palette of deep pink to aquamarine gradient, created with Unicolour](gradient-simple-palette.png) |
|-----------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------|
| _Gradient of deep pink to aquamarine through Oklch color space using `Mix()`_                                  | _Gradient of deep pink to aquamarine through Oklch color space using `Palette()`_                       |

| ![Gradient of purple to orange through many color spaces, created with Unicolour](gradient-spaces-purple-orange.png) | ![Gradient of black to green through many color spaces, created with Unicolour](gradient-spaces-black-green.png) |
|----------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------|
| _Gradient of purple to orange generated in every color space_                                                             | _Gradient of black to green generated in every color space_                                                           |

| ![Visualization of temperature from 1,000 K to 13,000 K, created with Unicolour](gradient-temperature.png) |
|-----------------------------------------------------------------------------------------------------------------|
| _Visualization of temperature from 1,000 K to 13,000 K_                                                         |

| ![Color spectrum rendered with different color vision deficiencies, created with Unicolour](gradient-vision-deficiency.png) |
|------------------------------------------------------------------------------------------------------------------------------------|
| _Color spectrum rendered with different color vision deficiencies_                                                               |

| ![Demonstration of interpolating from red to transparent to blue, with and without premultiplied alpha, created with Unicolour](gradient-alpha-interpolation.png) |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| _Demonstration of interpolating from red to transparent to blue, with and without premultiplied alpha_                                                                 |

| ![Perceptually uniform colormaps from Unicolour.Datasets, created with Unicolour](gradient-maps.png) | ![Perceptually uniform color palettes from Unicolour.Datasets, created with Unicolour](gradient-maps-palette.png) |
|------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------|
| _Perceptually uniform colormaps from [Unicolour.Datasets](#-datasets)_                                    | _Perceptually uniform color palettes from [Unicolour.Datasets](#-datasets)_                                            |

| ![Mixes of two-constant pigments to titanium white, created with Unicolour](gradient-pigments-mix.png) | ![Palettes of two-constant pigments to titanium white, created with Unicolour](gradient-pigments-palette.png) |
|-------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------|
| _Mixes of two-constant pigments to titanium white from [Unicolour.Datasets](#-datasets)_                    | _Palettes of two-constant pigments to titanium white from [Unicolour.Datasets](#-datasets)_                        |

| ![Mixes of single-constant pigments emulating Spectral.js, created with Unicolour](gradient-spectraljs-mix.png) | ![Palettes of single-constant pigments emulating Spectral.js, created with Unicolour](gradient-spectraljs-palette.png) |
|----------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------|
| _Mixes of single-constant pigments emulating Spectral.js from [Unicolour.Experimental](#-experimental)_              | _Palettes of single-constant pigments emulating Spectral.js from [Unicolour.Experimental](#-experimental)_                  |

### Heatmaps
Example code to create heatmaps of luminance using 📷 [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) with images from 🚀 [NASA](https://www.nasa.gov/)
can be seen in the [Example.Heatmaps](https://github.com/waacton/Unicolour/tree/main/Example.Heatmaps/Program.cs) project.

| ![Heatmap of the sun using perceptually uniform colormaps from Unicolour.Datasets, created with Unicolour](heatmaps-sun.png)                                              |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| _Heatmap of the ☀️ [sun](https://science.nasa.gov/image-detail/amf-gsfc_20171208_archive_e001435/) using perceptually uniform colormaps from [Unicolour.Datasets](#-datasets)_ |

| ![Heatmap of the moon using perceptually uniform colormaps from Unicolour.Datasets, created with Unicolour](heatmaps-moon.png)                                             |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| _Heatmap of the 🌕 [moon](https://science.nasa.gov/image-detail/amf-gsfc_20171208_archive_e001982/) using perceptually uniform colormaps from [Unicolour.Datasets](#-datasets)_ |

### Diagrams
Example code to create diagrams of color data using 📈 [ScottPlot](https://github.com/scottplot/scottplot)
can be seen in the [Example.Diagrams](https://github.com/waacton/Unicolour/tree/main/Example.Diagrams/Program.cs) project.

| ![CIE xy chromaticity diagram with sRGB gamut, created with Unicolour](diagram-xy-chromaticity-rgb.png) |
|--------------------------------------------------------------------------------------------------------------|
| _CIE xy chromaticity diagram with sRGB gamut_                                                                |

| ![CIE xy chromaticity diagram with Planckian or blackbody locus, created with Unicolour](diagram-xy-chromaticity-blackbody.png) |
|--------------------------------------------------------------------------------------------------------------------------------------|
| _CIE xy chromaticity diagram with Planckian or blackbody locus_                                                                      |

| ![CIE xy chromaticity diagram with MacAdam limits, created with Unicolour](diagram-xy-macadam-limits.png) |
|----------------------------------------------------------------------------------------------------------------|
| _CIE xy chromaticity diagram with MacAdam limits_                                                              |                                              

| ![CIE xy chromaticity diagram with spectral locus plotted at 1 nm intervals, created with Unicolour](diagram-spectral-locus.png) |
|---------------------------------------------------------------------------------------------------------------------------------------|
| _CIE xy chromaticity diagram with spectral locus plotted at 1 nm intervals_                                                           |

| ![CIE 1960 color space with Planckian or blackbody locus, created with Unicolour](diagram-uv-chromaticity-blackbody.png) |
|--------------------------------------------------------------------------------------------------------------------------------|
| _CIE 1960 color space with Planckian or blackbody locus_                                                                      |

| ![CIE 1960 color space with MacAdam limits, created with Unicolour](diagram-uv-macadam-limits.png) |
|----------------------------------------------------------------------------------------------------------|
| _CIE 1960 color space with MacAdam limits_                                                              |

### Console
Example code to create a colorful console application using ⌨️ [Spectre.Console](https://github.com/spectreconsole/spectre.console)
can be seen in the [Example.Console](https://github.com/waacton/Unicolour/tree/main/Example.Console/Program.cs) project.

| ![Console application displaying color information from a hex value, created with Unicolour](console-info.png) |
|----------------------------------------------------------------------------------------------------------------------|
| _Console application displaying color information from a hex value_                                                 |

### Web
Example code to create a client-side color picker web application using 🕸️ [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
can be seen in the [Example.Web](https://github.com/waacton/Unicolour/tree/main/Example.Web) project.

See the [live demo](https://unicolour.wacton.xyz/colour-picker/)!

| ![Web application for picking colors in any color space, created with Unicolour](web-picker.png) |
|---------------------------------------------------------------------------------------------------------|
| _Web application for picking colors in any color space_                                               |

### Unity
Example code to create 3D visualizations of color spaces using 🎮 [Unity](https://unity.com/)
can be seen in the [Example.Unity](https://github.com/waacton/Unicolour/tree/main/Example.Unity) project.

Try it out online in [Unity Play](https://play.unity.com/en/games/6826f61f-3806-4155-b824-7866b1edaed7/3d-colour-space-visualisation-unicolour-demo)!

| ![3D visualization of color spaces in Unity, created with Unicolour](unity-spaces.gif) |
|----------------------------------------------------------------------------------------------|
| _3D visualization of color spaces in Unity_                                                 |

| ![3D movement through color spaces in Unity, created with Unicolour](unity-movement.gif) |
|------------------------------------------------------------------------------------------------|
| _3D movement through color spaces in Unity_                                                   |

## 🔮 Datasets
Some color datasets have been compiled for convenience in the [Unicolour.Datasets](https://github.com/waacton/Unicolour/tree/main/Unicolour.Datasets) project.

Commonly used sets of colors:
- [CSS specification](https://www.w3.org/TR/css-color-4/#named-colors) named colors
- [xkcd](https://xkcd.com/color/rgb/) color survey results
- [Macbeth ColorChecker](https://en.wikipedia.org/wiki/ColorChecker) color rendition chart
- [Nord](https://www.nordtheme.com/) theme colors

Perceptually uniform colormaps / palettes:
- [Viridis, Plasma, Inferno & Magma](https://bids.github.io/colormap/) (sequential)
- [Cividis](https://journals.plos.org/plosone/article?id=10.1371/journal.pone.0199239) (sequential)
- [Mako, Rocket, Crest & Flare](https://seaborn.pydata.org/tutorial/color_palettes.html#perceptually-uniform-palettes) (sequential)
- [Vlag & Icefire](https://seaborn.pydata.org/tutorial/color_palettes.html#perceptually-uniform-diverging-palettes) (diverging)
- [Twilight & Twilight Shifted](https://github.com/bastibe/twilight) (cyclic)
- [Turbo](https://blog.research.google/2019/08/turbo-improved-rainbow-colormap-for.html) (rainbow)
- [Cubehelix](https://people.phy.cam.ac.uk/dag9/CUBEHELIX/) (sequential)

Color data used in academic literature:
- [MacAdam](https://doi.org/10.2307/1420820) limits of surface colors
- [Hung-Berns](https://doi.org/10.1002/col.5080200506) constant hue loci data
- [Ebner-Fairchild](https://doi.org/10.1117/12.298269) constant perceived-hue data

Known pigments:
- [Artist Paint Spectral Database](https://www.rit.edu/science/studio-scientific-imaging-and-archiving-cultural-heritage#publications) two-constant pigment data

Example usage:

1. Install the package from [NuGet](https://www.nuget.org/packages/Wacton.Unicolour.Datasets/)
```
dotnet add package Wacton.Unicolour.Datasets
```

2. Import the package
```c#
using Wacton.Unicolour.Datasets;
```

3. Reference the predefined `Unicolour`
```c#
var pink = Css.DeepPink;
var green = Xkcd.NastyGreen;
var mapped = Colourmaps.Viridis.Map(0.5);
var palette = Colourmaps.Turbo.Palette(10);
```

## 🥽 Experimental
There are additional useful features that are considered too ambiguous, indeterminate, or opinionated to be included as part of the core [Unicolour library](#-features).
These have been assembled in the [Unicolour.Experimental](https://github.com/waacton/Unicolour/tree/main/Unicolour.Experimental) project.

1. Install the package from [NuGet](https://www.nuget.org/packages/Wacton.Unicolour.Experimental/)
```
dotnet add package Wacton.Unicolour.Experimental
```

2. Import the package
```c#
using Wacton.Unicolour.Experimental;
```

### Generate pigments
A reflectance curve can be generated for any color, approximating a single-constant pigment.
This enables Kubelka-Munk pigment mixing without taking reflectance measurements.
Note that, similar to metamerism, there are infinitely many reflectance curves that can generate a single color; this will find just one.
```c#
var redPigment = PigmentGenerator.From(new Unicolour("#FF0000"));
var bluePigment = PigmentGenerator.From(new Unicolour("#0000FF"));
var magenta = new Unicolour([redPigment, bluePigment], [0.5, 0.5]);
```

### Emulate Spectral.js
[Spectral.js](https://onedayofcrypto.art/) uses artificial reflectance curves to perform single-constant pigment mixing.
However, input concentrations are adjusted according to luminance and a custom weighting curve that the author found to give aesthetically pleasing results.
This behaviour has been replicated here except 1) reflectance curves are more accurately generated at a performance cost
and 2) it has been extended to be able to mix more than two colors.
```c#
var blue = new Unicolour("#0000FF");
var yellow = new Unicolour("#FFFF00");
var green = SpectralJs.Mix([blue, yellow], [0.5, 0.5]);
var palette = SpectralJs.Palette(blue, yellow, 9);
```

---

[Wacton.Unicolour](https://github.com/waacton/Unicolour) is licensed under the [MIT License](https://choosealicense.com/licenses/mit/), copyright © 2022-2025 William Acton.

Also available in [British](README.md) 🇬🇧.