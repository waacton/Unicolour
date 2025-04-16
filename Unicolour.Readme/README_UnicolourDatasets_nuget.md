# Unicolour.Datasets

Datasets for use with 🌈 [Wacton.Unicolour](https://www.nuget.org/packages/Wacton.Unicolour/)

## 🔮 Datasets
Some colour datasets have been compiled for convenience in the [Unicolour.Datasets](https://github.com/waacton/Unicolour/tree/main/Unicolour.Datasets) project.

Commonly used sets of colours:
- [CSS specification](https://www.w3.org/TR/css-color-4/#named-colors) named colours
- [xkcd](https://xkcd.com/color/rgb/) colour survey results
- [Macbeth ColorChecker](https://en.wikipedia.org/wiki/ColorChecker) colour rendition chart
- [Nord](https://www.nordtheme.com/) theme colours

Perceptually uniform colourmaps / palettes:
- [Viridis, Plasma, Inferno & Magma](https://bids.github.io/colormap/) (sequential)
- [Cividis](https://journals.plos.org/plosone/article?id=10.1371/journal.pone.0199239) (sequential)
- [Mako, Rocket, Crest & Flare](https://seaborn.pydata.org/tutorial/color_palettes.html#perceptually-uniform-palettes) (sequential)
- [Vlag & Icefire](https://seaborn.pydata.org/tutorial/color_palettes.html#perceptually-uniform-diverging-palettes) (diverging)
- [Twilight & Twilight Shifted](https://github.com/bastibe/twilight) (cyclic)
- [Turbo](https://blog.research.google/2019/08/turbo-improved-rainbow-colormap-for.html) (rainbow)
- [Cubehelix](https://people.phy.cam.ac.uk/dag9/CUBEHELIX/) (sequential)

Colour data used in academic literature:
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
```cs
using Wacton.Unicolour.Datasets;
```

3. Reference the predefined `Unicolour`
```cs
var pink = Css.DeepPink;
var green = Xkcd.NastyGreen;
var mapped = Colourmaps.Viridis.Map(0.5);
var palette = Colourmaps.Turbo.Palette(10);
```

---

[Wacton.Unicolour](https://github.com/waacton/Unicolour) is licensed under the [MIT License](https://choosealicense.com/licenses/mit/), copyright © 2022-2025 William Acton.