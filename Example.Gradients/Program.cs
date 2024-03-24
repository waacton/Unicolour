using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Wacton.Unicolour;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Example.Gradients;

const string outputDirectory = "../../../../Unicolour.Readme/docs/";

ColourSpaces();
Temperature();
VisionDeficiency();
AlphaInterpolation();
ColourMaps();
return;

void ColourSpaces()
{
    const int columnWidth = 800;
    const int rowHeight = 100;
    
    var text = new Unicolour("#E8E8FF");
    var purple = new Unicolour(ColourSpace.Hsb, 260, 1.0, 0.33);
    var orange = new Unicolour(ColourSpace.Hsb, 30, 0.66, 1.0);
    var pink = new Unicolour("#FF1493");
    var cyan = new Unicolour(ColourSpace.Rgb255, 0, 255, 255);
    var black = new Unicolour(ColourSpace.Rgb, 0, 0, 0);
    var green = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
    var columns = new List<Image<Rgba32>>
    {
        DrawColumn(new[] { purple, orange }),
        DrawColumn(new[] { pink, cyan }),
        DrawColumn(new[] { black, green })
    };

    var columnHeight = columns.First().Height;
    var image = Utils.DrawColumns(columns, columnWidth, columnHeight);
    image.Save(Path.Combine(outputDirectory, "gradient-spaces.png"));
    return;
    
    Image<Rgba32> DrawColumn(Unicolour[] colourPoints)
    {
        var gradients = new List<Image<Rgba32>>
        {
            Utils.Draw(("RGB", text), columnWidth, rowHeight, GetColour(ColourSpace.Rgb)),
            Utils.Draw(("RGB Linear", text), columnWidth, rowHeight, GetColour(ColourSpace.RgbLinear)),
            Utils.Draw(("HSB", text), columnWidth, rowHeight, GetColour(ColourSpace.Hsb)),
            Utils.Draw(("HSL", text), columnWidth, rowHeight, GetColour(ColourSpace.Hsl)),
            Utils.Draw(("HWB", text), columnWidth, rowHeight, GetColour(ColourSpace.Hwb)),
            Utils.Draw(("XYZ", text), columnWidth, rowHeight, GetColour(ColourSpace.Xyz)),
            Utils.Draw(("xyY", text), columnWidth, rowHeight, GetColour(ColourSpace.Xyy)),
            Utils.Draw(("LAB", text), columnWidth, rowHeight, GetColour(ColourSpace.Lab)),
            Utils.Draw(("LCHab", text), columnWidth, rowHeight, GetColour(ColourSpace.Lchab)),
            Utils.Draw(("LUV", text), columnWidth, rowHeight, GetColour(ColourSpace.Luv)),
            Utils.Draw(("LCHuv", text), columnWidth, rowHeight, GetColour(ColourSpace.Lchuv)),
            Utils.Draw(("HSLuv", text), columnWidth, rowHeight, GetColour(ColourSpace.Hsluv)),
            Utils.Draw(("HPLuv", text), columnWidth, rowHeight, GetColour(ColourSpace.Hpluv)),
            Utils.Draw(("YPbPr", text), columnWidth, rowHeight, GetColour(ColourSpace.Ypbpr)),
            Utils.Draw(("YCbCr", text), columnWidth, rowHeight, GetColour(ColourSpace.Ycbcr)),
            Utils.Draw(("YCgCo", text), columnWidth, rowHeight, GetColour(ColourSpace.Ycgco)),
            Utils.Draw(("YUV", text), columnWidth, rowHeight, GetColour(ColourSpace.Yuv)),
            Utils.Draw(("YIQ", text), columnWidth, rowHeight, GetColour(ColourSpace.Yiq)),
            Utils.Draw(("YDbDr", text), columnWidth, rowHeight, GetColour(ColourSpace.Ydbdr)),
            Utils.Draw(("IPT", text), columnWidth, rowHeight, GetColour(ColourSpace.Ipt)),
            Utils.Draw(("ICtCp", text), columnWidth, rowHeight, GetColour(ColourSpace.Ictcp)),
            Utils.Draw(("JzAzBz", text), columnWidth, rowHeight, GetColour(ColourSpace.Jzazbz)),
            Utils.Draw(("JzCzHz", text), columnWidth, rowHeight, GetColour(ColourSpace.Jzczhz)),
            Utils.Draw(("OKLAB", text), columnWidth, rowHeight, GetColour(ColourSpace.Oklab)),
            Utils.Draw(("OKLCH", text), columnWidth, rowHeight, GetColour(ColourSpace.Oklch)),
            Utils.Draw(("CAM02", text), columnWidth, rowHeight, GetColour(ColourSpace.Cam02)),
            Utils.Draw(("CAM16", text), columnWidth, rowHeight, GetColour(ColourSpace.Cam16)),
            Utils.Draw(("HCT", text), columnWidth, rowHeight, GetColour(ColourSpace.Hct))
        };

        return Utils.DrawRows(gradients, columnWidth, rowHeight);
        
        Utils.GetColour GetColour(ColourSpace colourSpace)
        {
            Utils.Mix mix = (start, end, amount) => start.Mix(end, colourSpace, amount);
            return column => Utils.GetMixedColour(colourPoints, column, columnWidth, mix);
        }
    }
}

void Temperature()
{
    const int width = 1200;
    const int rowHeight = 120;

    var text = Css.Black;
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
        Utils.Draw(("CCT (1,000 K - 13,000 K)", text), width, rowHeight, GetColour())
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
    const int width = 1200;
    const int rowHeight = 100;

    var text = new Unicolour("#404046");
    var colourPoints = new Unicolour[]
    {
        // not using OKLCH for the spectrum because the uniform luminance results in flat gradient for Achromatopsia
        new(ColourSpace.Hsb, 0, 0.666, 1),
        new(ColourSpace.Hsb, 360, 0.666, 1)
    };
    
    var rows = new List<Image<Rgba32>>
    {
        Utils.Draw(("No deficiency", text), width, rowHeight, GetColour(Cvd.None)),
        Utils.Draw(("Protanopia", text), width, rowHeight, GetColour(Cvd.Protanopia)),
        Utils.Draw(("Deuteranopia", text), width, rowHeight, GetColour(Cvd.Deuteranopia)),
        Utils.Draw(("Tritanopia", text), width, rowHeight, GetColour(Cvd.Tritanopia)),
        Utils.Draw(("Achromatopsia", text), width, rowHeight, GetColour(Cvd.Achromatopsia))
    };

    var image = Utils.DrawRows(rows, width, rowHeight);
    image.Save(Path.Combine(outputDirectory, "gradient-vision-deficiency.png"));
    return;

    Utils.GetColour GetColour(Cvd cvd)
    {
        Utils.Mix mix = (start, end, amount) => start.Mix(end, ColourSpace.Hsb, amount, HueSpan.Increasing);
        return column =>
        {
            var unicolour = Utils.GetMixedColour(colourPoints, column, width, mix);
            return cvd switch
            {
                Cvd.None => unicolour,
                Cvd.Protanopia => unicolour.SimulateProtanopia(),
                Cvd.Deuteranopia => unicolour.SimulateDeuteranopia(),
                Cvd.Tritanopia => unicolour.SimulateTritanopia(),
                Cvd.Achromatopsia => unicolour.SimulateAchromatopsia(),
                _ => throw new ArgumentOutOfRangeException(nameof(cvd), cvd, null)
            };
        };
    }
}

void AlphaInterpolation()
{
    const int width = 1000;
    const int rowHeight = 120;

    var text = Css.Black;
    var colourPoints = new[] { Css.Red, Css.Transparent, Css.Blue };
    var rows = new List<Image<Rgba32>>
    {
        Utils.Draw(("With premultiplied alpha", text), width, rowHeight, GetColour(true)),
        Utils.Draw(("Without premultiplied alpha", text), width, rowHeight, GetColour(false))
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
    const int width = 1024;
    const int rowHeight = 80;

    var white = Colourmap.White;
    var black = Colourmap.Black;
    var rows = new List<Image<Rgba32>>
    {
        Utils.Draw(("Viridis", white), width, rowHeight, column => Colourmaps.Viridis.Map(Distance(column))),
        Utils.Draw(("Plasma", white), width, rowHeight, column => Colourmaps.Plasma.Map(Distance(column))),
        Utils.Draw(("Inferno", white), width, rowHeight, column => Colourmaps.Inferno.Map(Distance(column))),
        Utils.Draw(("Magma", white), width, rowHeight, column => Colourmaps.Magma.Map(Distance(column))),
        Utils.Draw(("Cividis", white), width, rowHeight, column => Colourmaps.Cividis.Map(Distance(column))),
        Utils.Draw(("Mako", white), width, rowHeight, column => Colourmaps.Mako.Map(Distance(column))),
        Utils.Draw(("Rocket", white), width, rowHeight, column => Colourmaps.Rocket.Map(Distance(column))),
        Utils.Draw(("Crest", black), width, rowHeight, column => Colourmaps.Crest.Map(Distance(column))),
        Utils.Draw(("Flare", black), width, rowHeight, column => Colourmaps.Flare.Map(Distance(column))),
        Utils.Draw(("Vlag", black), width, rowHeight, column => Colourmaps.Vlag.Map(Distance(column))),
        Utils.Draw(("Icefire", black), width, rowHeight, column => Colourmaps.Icefire.Map(Distance(column))),
        Utils.Draw(("Twilight", black), width, rowHeight, column => Colourmaps.Twilight.Map(Distance(column))),
        Utils.Draw(("Twilight Shifted", white), width, rowHeight, column => Colourmaps.TwilightShifted.Map(Distance(column))),
        Utils.Draw(("Turbo", white), width, rowHeight, column => Colourmaps.Turbo.Map(Distance(column))),
        Utils.Draw(("Cubehelix", white), width, rowHeight, column => Colourmaps.Cubehelix.Map(Distance(column)))
    };

    var image = Utils.DrawRows(rows, width, rowHeight);
    image.Save(Path.Combine(outputDirectory, "gradient-maps.png"));
    return;

    double Distance(int columnIndex) => columnIndex / (double) (width - 1);
}

internal enum Cvd
{
    None,
    Protanopia,
    Deuteranopia,
    Tritanopia,
    Achromatopsia
}