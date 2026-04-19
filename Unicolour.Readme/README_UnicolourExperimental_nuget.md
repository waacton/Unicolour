# Unicolour.Experimental

Experimental additions to 🌈 [Wacton.Unicolour](https://www.nuget.org/packages/Wacton.Unicolour/)

## 🥽 Experimental
There are additional useful features that are considered too ambiguous, indeterminate, or opinionated to be included as part of the core [Unicolour library](https://github.com/waacton/Unicolour#-features).
These have been assembled in the [Unicolour.Experimental](https://github.com/waacton/Unicolour/tree/main/Unicolour.Experimental) project.

1. Install the package from [NuGet](https://www.nuget.org/packages/Wacton.Unicolour.Experimental/)
```
dotnet add package Wacton.Unicolour.Experimental
```

2. Import the package
```cs
using Wacton.Unicolour.Experimental;
```

### Generate colour harmonies
Traditionally, harmonious colour combinations are based on their relationship within a colour wheel.
A colour wheel can be generated using [pigments](https://github.com/waacton/Unicolour#model-pigment-and-paint-colours) or a hued colour space.
```cs
/* populate pigment k and s with measurement data */
var quinaRed = new Pigment(380, 1, k: [], s: []);
var bismuthYellow = new Pigment(380, 1, k: [], s: []);
var ceruleanBlue = new Pigment(380, 1, k: [], s: []);
var titaniumWhite = new Pigment(380, 1, k: [], s: []);
var boneBlack = new Pigment(380, 1, k: [], s: []);

var colourWheel = usePigments 
    ? ColourWheel.From(quinaRed, bismuthYellow, ceruleanBlue, titaniumWhite, boneBlack)
    : ColourWheel.From(ColourSpace.Oklch, reference: new Unicolour("ff0000"));

var orange = colourWheel.Pure(hue: 60);
var lightGreen = colourWheel.Tint(hue: 180, weight: 1);
var darkPurple = colourWheel.Shade(hue: 300, weight: 1);
var greyRed = colourWheel.Tone(hue: 0, weight: 1);

var orangePalette = colourWheel.Harmony(hue: 60, Harmony.Analogous);
```

| Colour&nbsp;harmony                           | Enum                         |
|-----------------------------------------------|------------------------------|
| Monochromatic&nbsp;(tint)                     | `Harmony.MonochromaticTint`  |
| Monochromatic&nbsp;(shade)                    | `Harmony.MonochromaticShade` |
| Monochromatic&nbsp;(tone)                     | `Harmony.MonochromaticTone`  |
| Monochromatic&nbsp;(tint&nbsp;and&nbsp;shade) | `Harmony.Monochromatic`      |
| Analogous                                     | `Harmony.Analogous`          |
| Complementary                                 | `Harmony.Complementary`      |
| Split-complementary                           | `Harmony.SplitComplementary` |
| Triadic                                       | `Harmony.Triadic`            |
| Tetradic&nbsp;(rectangle)                     | `Harmony.TetradicRectangle`  |
| Tetradic&nbsp;(square)                        | `Harmony.TetradicSquare`     |

### Approximate pigments
A reflectance curve can be generated for any colour, approximating a single-constant pigment.
This enables Kubelka-Munk pigment mixing without taking reflectance measurements.
Note that, similar to metamerism, there are infinitely many reflectance curves that can generate a single colour; this will find just one.
A reflectance curve can be generated for any colour, approximating a single-constant pigment.
This enables Kubelka-Munk pigment mixing without taking reflectance measurements.
Note that, similar to metamerism, there are infinitely many reflectance curves that can generate a single colour; this will find just one.
```cs
var redPigment = PigmentGenerator.From(new Unicolour("#FF0000"));
var bluePigment = PigmentGenerator.From(new Unicolour("#0000FF"));
var magenta = new Unicolour([redPigment, bluePigment], [0.5, 0.5]);
```

### Emulate Spectral.js
[Spectral.js](https://onedayofcrypto.art/) uses artificial reflectance curves to perform single-constant pigment mixing.
However, input concentrations are adjusted according to luminance and a custom weighting curve that the author found to give aesthetically pleasing results.
This behaviour has been replicated here except 1) reflectance curves are more accurately generated at a performance cost
and 2) it has been extended to be able to mix more than two colours.
```cs
var blue = new Unicolour("#0000FF");
var yellow = new Unicolour("#FFFF00");
var green = SpectralJs.Mix([blue, yellow], [0.5, 0.5]);
var palette = SpectralJs.Palette(blue, yellow, 9);
```

---

[Wacton.Unicolour](https://github.com/waacton/Unicolour) is licensed under the [MIT License](https://choosealicense.com/licenses/mit/), copyright © 2022-2026 William Acton.

[![Not by AI](https://raw.githubusercontent.com/waacton/Unicolour/main/docs/not-by-ai.png)](https://notbyai.fyi/)