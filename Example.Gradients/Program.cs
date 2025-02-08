using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Wacton.Unicolour;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Example.Gradients;
using Experimental = Wacton.Unicolour.Experimental;

const string outputDirectory = "../../../../Unicolour.Readme/docs/";

Unicolour black = new("#000000");
Unicolour white = new("#FFFFFF");

Simple();
ColourSpaces();
Temperature();
VisionDeficiency();
AlphaInterpolation();
ColourMaps();
Pigments();
SpectralJs();
return;

void Simple()
{
    const int width = 800;
    const int rowHeight = 100;
    const int blocks = 8;
    const int columnsPerBlock = width / blocks;
    
    // "deep pink" to "aquamarine" (CSS colours)
    Unicolour start = new("FF1493");    
    Unicolour end = new("7FFFD4");

    // calculate each column by mixing on demand at every distance between start and end
    // NOTE: can achieve same result by doing the palette approach below with 1024 colours
    Unicolour GetMixedColour(int column) => start.Mix(end, ColourSpace.Oklch, column / (double)width, HueSpan.Decreasing);
    var mixingRow = Utils.Draw(width, rowHeight, GetMixedColour);

    // calculate each column according to a pre-generated palette between start and end
    var palette = start.Palette(end, ColourSpace.Oklch, blocks, HueSpan.Decreasing).ToArray();
    Unicolour GetPaletteColour(int column) => palette[column / columnsPerBlock];
    var paletteRow = Utils.Draw(width, rowHeight, GetPaletteColour);
    
    var imageMixing = Utils.DrawRows([mixingRow], width, rowHeight);
    imageMixing.Save(Path.Combine(outputDirectory, "gradient-simple-mixing.png"));
    
    var imagePalette = Utils.DrawRows([paletteRow], width, rowHeight);
    imagePalette.Save(Path.Combine(outputDirectory, "gradient-simple-palette.png"));
}

void ColourSpaces()
{
    const int width = 1000;
    const int rowHeight = 100;
    
    Unicolour purple = new(ColourSpace.Hsb, 260, 1.0, 0.333);
    Unicolour orange = new(ColourSpace.Hsl, 30, 1.0, 0.666);
    Unicolour green = new(ColourSpace.Rgb, 0, 1, 0);

    List<ColourSpace> colourSpaces =
    [
        ColourSpace.Rgb, ColourSpace.Rgb255, ColourSpace.RgbLinear,
        ColourSpace.Hsb, ColourSpace.Hsl, ColourSpace.Hwb, ColourSpace.Hsi,
        ColourSpace.Xyz, ColourSpace.Xyy, ColourSpace.Wxy,
        ColourSpace.Lab, ColourSpace.Lchab, ColourSpace.Luv, ColourSpace.Lchuv, 
        ColourSpace.Hsluv, ColourSpace.Hpluv,
        ColourSpace.Ypbpr, ColourSpace.Ycbcr, ColourSpace.Ycgco, ColourSpace.Yuv, ColourSpace.Yiq, ColourSpace.Ydbdr,
        ColourSpace.Tsl, ColourSpace.Xyb,
        ColourSpace.Ipt, ColourSpace.Ictcp, ColourSpace.Jzazbz, ColourSpace.Jzczhz,
        ColourSpace.Oklab, ColourSpace.Oklch, ColourSpace.Okhsv, ColourSpace.Okhsl, ColourSpace.Okhwb,
        ColourSpace.Cam02, ColourSpace.Cam16,
        ColourSpace.Hct
    ];

    DrawGradients(purple, orange, "gradient-spaces-purple-orange.png");
    DrawGradients(black, green, "gradient-spaces-black-green.png");
    return;

    void DrawGradients(Unicolour start, Unicolour end, string filename)
    {
        var rows = colourSpaces
            .Select(colourSpace => Utils.Draw((colourSpace.ToString(), white), width, rowHeight, GetColour(colourSpace)))
            .ToList();

        var image = Utils.DrawRows(rows, width, rowHeight);
        image.Save(Path.Combine(outputDirectory, filename));
        return;

        Utils.GetColour GetColour(ColourSpace colourSpace)
        {
            return column => start.Mix(end, colourSpace, column / (double)width);
        }
    }
}

void Temperature()
{
    const int width = 1200;
    const int rowHeight = 120;

    var scaledPoints = new List<Unicolour>();
    for (var i = 1000; i <= 13000; i += 100)
    {
        var rgb = new Unicolour(i).Rgb;
        var rgbComponents = new[] { rgb.R, rgb.G, rgb.B };
        var max = rgbComponents.Max();
        var scaledRgb = rgbComponents.Select(x => x / max).ToArray();
        var scaledUnicolour = new Unicolour(ColourSpace.Rgb, scaledRgb[0], scaledRgb[1], scaledRgb[2]);
        scaledPoints.Add(scaledUnicolour);
    }

    var rows = new List<Image<Rgba32>>
    {
        Utils.Draw(("CCT (1,000 K - 13,000 K)", black), width, rowHeight, GetColour())
    };

    var image = Utils.DrawRows(rows, width, rowHeight);
    image.Save(Path.Combine(outputDirectory, "gradient-temperature.png"));
    return;

    Utils.GetColour GetColour()
    {
        Utils.Mix mix = (start, end, amount) => start.Mix(end, ColourSpace.Rgb, amount);
        return column => Utils.GetMixedColour(scaledPoints.ToArray(), column, width, mix);
    }
}

void VisionDeficiency()
{
    const int width = 1000;
    const int rowHeight = 100;

    Unicolour start = new(ColourSpace.Hsb, 0, 0.666, 1);
    Unicolour end = new(ColourSpace.Hsb, 360, 0.666, 1);

    var cvds = new List<Cvd?> { null, Cvd.Protanopia, Cvd.Deuteranopia, Cvd.Tritanopia, Cvd.Achromatopsia };
    var rows = cvds
        .Select(cvd => Utils.Draw((cvd?.ToString() ?? "No deficiency", black), width, rowHeight, GetColour(cvd)))
        .ToList();
    
    var image = Utils.DrawRows(rows, width, rowHeight);
    image.Save(Path.Combine(outputDirectory, "gradient-vision-deficiency.png"));
    return;
    
    Utils.GetColour GetColour(Cvd? cvd)
    {
        return column =>
        {
            // not interpolating through OKLCH for the spectrum
            // because the uniform luminance results in flat gradient for achromatopsia
            var mixed = start.Mix(end, ColourSpace.Hsb, column / (double)width, HueSpan.Increasing);
            return cvd.HasValue ? mixed.Simulate(cvd.Value) : mixed;
        };
    }
}

void AlphaInterpolation()
{
    const int width = 1000;
    const int rowHeight = 100;

    var colourPoints = new[] { Css.Red, Css.Transparent, Css.Blue };
    var rows = new List<Image<Rgba32>>
    {
        Utils.Draw(("With premultiplied alpha", black), width, rowHeight, GetColour(true)),
        Utils.Draw(("Without premultiplied alpha", black), width, rowHeight, GetColour(false))
    };
    
    var image = Utils.DrawRows(rows, width, rowHeight);
    image.Save(Path.Combine(outputDirectory, "gradient-alpha-interpolation.png"));
    return;

    Utils.GetColour GetColour(bool premultiplyAlpha)
    {
        Utils.Mix mix = (start, end, amount) => start.Mix(end, ColourSpace.Rgb, amount, premultiplyAlpha: premultiplyAlpha);
        return column => Utils.GetMixedColour(colourPoints, column, width, mix);
    }
}

void ColourMaps()
{
    const int width = 1000;
    const int rowHeight = 100;
    
    List<Colourmap> colourmaps =
    [
        Colourmaps.Viridis, Colourmaps.Plasma, Colourmaps.Inferno, Colourmaps.Magma, Colourmaps.Cividis,
        Colourmaps.Mako, Colourmaps.Rocket, Colourmaps.Crest, Colourmaps.Flare,
        Colourmaps.Vlag, Colourmaps.Icefire, Colourmaps.Twilight, Colourmaps.TwilightShifted,
        Colourmaps.Turbo, Colourmaps.Cubehelix
    ];
    
    DrawMaps();
    DrawPalettes();
    return;

    void DrawMaps()
    {
        var rows = colourmaps
            .Select(map => Utils.Draw((map.ToString()!, GetLabelColour(map)), width, rowHeight, GetColour(map)))
            .ToList();

        var image = Utils.DrawRows(rows, width, rowHeight);
        image.Save(Path.Combine(outputDirectory, "gradient-maps.png"));
        return;
        
        Utils.GetColour GetColour(Colourmap colourmap)
        {
            return column => colourmap.Map(column / (double)(width - 1));
        }
    }

    void DrawPalettes()
    {
        const int blocks = 10;
        const int columnsPerBlock = width / blocks;

        var colourmapToPalette = colourmaps.ToDictionary(
            map => map, 
            map => map.Palette(blocks).ToArray());
        
        var rows = colourmapToPalette
            .Select(kvp =>
            {
                var (map, palette) = kvp;
                return Utils.Draw((map.ToString()!, GetLabelColour(map)), width, rowHeight, GetColour(palette));
            })
            .ToList();

        var image = Utils.DrawRows(rows, width, rowHeight);
        image.Save(Path.Combine(outputDirectory, "gradient-maps-palette.png"));
        return;

        Utils.GetColour GetColour(Unicolour[] palette)
        {
            return column => palette[column / columnsPerBlock];
        }
    }
    
    Unicolour GetLabelColour(Colourmap colourmap)
    {
        Colourmap[] light = [Colourmaps.Crest, Colourmaps.Flare, Colourmaps.Vlag, Colourmaps.Icefire, Colourmaps.Twilight];
        return light.Contains(colourmap) ? black : white;
    }
}

void Pigments()
{
    const int width = 800;
    const int rowHeight = 100;
    
    List<Pigment> pigments =
    [
        ArtistPaint.BoneBlack, 
        ArtistPaint.BismuthVanadateYellow, ArtistPaint.HansaYellowOpaque, ArtistPaint.DiarylideYellow,
        ArtistPaint.CadmiumOrange, ArtistPaint.PyrroleOrange,
        ArtistPaint.CadmiumRedLight, ArtistPaint.PyrroleRed, ArtistPaint.QuinacridoneRed,
        ArtistPaint.QuinacridoneMagenta, ArtistPaint.DioxazinePurple,
        ArtistPaint.PhthaloBlueRedShade, ArtistPaint.PhthaloBlueGreenShade,
        ArtistPaint.UltramarineBlue, ArtistPaint.CobaltBlue, ArtistPaint.CeruleanBlueChromium,
        ArtistPaint.PhthaloGreenBlueShade, ArtistPaint.PhthaloGreenYellowShade
    ];

    var endPigment = ArtistPaint.TitaniumWhite;
    
    DrawMixes();
    DrawPalettes();
    return;

    // NOTE: slow for smooth gradient, each column calculates a reflectance curve based on concentrations
    // which is then used for integration alongside SPD and CMF values for every wavelength to calculate XYZ
    void DrawMixes()
    {
        var rows = pigments
            .Select(pigment => Utils.Draw((pigment.Name, GetLabelColour(pigment)), width, rowHeight, GetColour(pigment, endPigment)))
            .ToList();

        var image = Utils.DrawRows(rows, width, rowHeight);
        image.Save(Path.Combine(outputDirectory, "gradient-pigments-mix.png"));
        return;
        
        Utils.GetColour GetColour(Pigment start, Pigment end)
        {
            return column =>
            {
                var distance = column / (double)(width - 1);
                return new Unicolour([start, end], [1 - distance, distance]);
            };
        }
    }

    void DrawPalettes()
    {
        const int blocks = 8;
        const int columnsPerBlock = width / blocks;
        
        var weights = Enumerable.Range(0, blocks).Select(i => new[] { 1 - Distance(i), Distance(i) }).ToArray();
        var pigmentToPalette = pigments.ToDictionary(
            pigment => pigment, 
            pigment => weights.Select(weight => new Unicolour([pigment, endPigment], weight)).ToArray());
    
        var rows = pigmentToPalette
            .Select(kvp =>
            {
                var (pigment, palette) = kvp;
                return Utils.Draw((pigment.Name, GetLabelColour(pigment)), width, rowHeight, GetColour(palette));
            })
            .ToList();
    
        var image = Utils.DrawRows(rows, width, rowHeight);
        image.Save(Path.Combine(outputDirectory, "gradient-pigments-palette.png"));
        return;
        
        double Distance(int i) => i / (double)(blocks - 1);

        Utils.GetColour GetColour(Unicolour[] palette)
        {
            return column => palette[column / columnsPerBlock];
        }
    }

    Unicolour GetLabelColour(Pigment pigment)
    {
        Pigment[] light = [ArtistPaint.BismuthVanadateYellow, ArtistPaint.HansaYellowOpaque, ArtistPaint.DiarylideYellow];
        return light.Contains(pigment) ? black : white;
    }
}

void SpectralJs()
{
    const int width = 540;
    const int rowHeight = 60;

    var colours = new List<(Unicolour start, Unicolour end)>
    {
        // default colours on https://onedayofcrypto.art/
        (new Unicolour("#002185"), new Unicolour("#FCD200")),
        
        // colours taken from github example https://github.com/rvanwijnen/spectral.js#usage
        (new Unicolour("#005E72"), new Unicolour("#EAD9A7")), 
        (new Unicolour("#FF8A3E"), new Unicolour("#FF006D")),
        (new Unicolour("#002185"), new Unicolour("#F0F0F0")),
        (new Unicolour("#DFE800"), new Unicolour("#CC3536"))
    };
    
    DrawMixes();
    DrawPalettes();
    return;

    // NOTE: very slow for smooth gradient, generates a reflectance curve by solving a system of linear equations per column, for best accuracy
    // (Spectral.js itself uses 7 hardcoded "target" curves and weights approximates a curve using those, so less accurate but faster)
    void DrawMixes()
    {
        var rows = colours
            .Select(pair => Utils.Draw(width, rowHeight, GetColour(pair.start, pair.end)))
            .ToList();

        var image = Utils.DrawRows(rows, width, rowHeight);
        image.Save(Path.Combine(outputDirectory, "gradient-spectraljs-mix.png"));
        return;
        
        Utils.GetColour GetColour(Unicolour start, Unicolour end)
        {
            return column =>
            {
                var distance = column / (double)(width - 1);
                return Experimental.SpectralJs.Mix([start, end], [1 - distance, distance]);
            };
        }
    }

    void DrawPalettes()
    {
        const int blocks = 9;
        const int columnsPerBlock = width / blocks;

        var palettes = colours.Select(colours => Experimental.SpectralJs.Palette(colours.start, colours.end, blocks).ToArray()).ToList();
        var rows = palettes
            .Select(palette => Utils.Draw(width, rowHeight, GetColour(palette)))
            .ToList();
        
        var image = Utils.DrawRows(rows, width, rowHeight);
        image.Save(Path.Combine(outputDirectory, "gradient-spectraljs-palette.png"));
        return;

        Utils.GetColour GetColour(Unicolour[] palette)
        {
            return column => palette[column / columnsPerBlock];
        }
    }
}