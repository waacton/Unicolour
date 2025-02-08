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

### Generate pigments
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

[Wacton.Unicolour](https://github.com/waacton/Unicolour) is licensed under the [MIT License](https://choosealicense.com/licenses/mit/), copyright © 2022-2025 William Acton.