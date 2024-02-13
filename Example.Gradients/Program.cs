using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Wacton.Unicolour;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Example.Gradients;

const string outputDirectory = "../../../../Unicolour.Readme/docs/";

ColourSpaces();
Temperature();
VisionDeficiency();
AlphaInterpolation();
return;

void ColourSpaces()
{
    const int columns = 3;
    const int columnWidth = 800;
    const int rows = 21; 
    const int rowHeight = 100;
    
    var purple = new Unicolour(ColourSpace.Hsb, 260, 1.0, 0.33);
    var orange = new Unicolour(ColourSpace.Hsb, 30, 0.66, 1.0);
    var pink = new Unicolour("#FF1493");
    var cyan = new Unicolour(ColourSpace.Rgb255, 0, 255, 255);
    var black = new Unicolour(ColourSpace.Rgb, 0, 0, 0);
    var green = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
    
    var lightText = new Unicolour("#E8E8FF");
    var column1 = DrawColumn(new[] { purple, orange });
    var column2 = DrawColumn(new[] { pink, cyan });
    var column3 = DrawColumn(new[] { black, green });

    var image = new Image<Rgba32>(columnWidth * columns, rowHeight * rows);
    image.Mutate(context => context
        .DrawImage(column1, new Point(columnWidth * 0, 0), 1f)
        .DrawImage(column2, new Point(columnWidth * 1, 0), 1f)
        .DrawImage(column3, new Point(columnWidth * 2, 0), 1f)
    );

    image.Save(Path.Combine(outputDirectory, "gradient-colour-spaces.png"));
    return;

    Image<Rgba32> DrawColumn(Unicolour[] colourPoints)
    {
        var rgb = Utils.Draw(("RGB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Rgb, amount));
        var rgbLinear = Utils.Draw(("RGB Linear", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.RgbLinear, amount));
        var hsb = Utils.Draw(("HSB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Hsb, amount));
        var hsl = Utils.Draw(("HSL", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Hsl, amount));
        var hwb = Utils.Draw(("HWB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Hwb, amount));
        var xyz = Utils.Draw(("XYZ", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Xyz, amount));
        var xyy = Utils.Draw(("xyY", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Xyy, amount));
        var lab = Utils.Draw(("LAB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Lab, amount));
        var lchab = Utils.Draw(("LCHab", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Lchab, amount));
        var luv = Utils.Draw(("LUV", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Luv, amount));
        var lchuv = Utils.Draw(("LCHuv", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Lchuv, amount));
        var hsluv = Utils.Draw(("HSLuv", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Hsluv, amount));
        var hpluv = Utils.Draw(("HPLuv", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Hpluv, amount));
        var ictcp = Utils.Draw(("ICtCp", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Ictcp, amount));
        var jzazbz = Utils.Draw(("JzAzBz", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Jzazbz, amount));
        var jzczhz = Utils.Draw(("JzCzHz", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Jzczhz, amount));
        var oklab = Utils.Draw(("OKLAB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Oklab, amount));
        var oklch = Utils.Draw(("OKLCH", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Oklch, amount));
        var cam02 = Utils.Draw(("CAM02", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Cam02, amount));
        var cam16 = Utils.Draw(("CAM16", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Cam16, amount));
        var hct = Utils.Draw(("HCT", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(end, ColourSpace.Hct, amount));

        var columnImage = new Image<Rgba32>(columnWidth, rowHeight * rows);
        columnImage.Mutate(context => context
            .DrawImage(rgb, new Point(0, rowHeight * 0), 1f)
            .DrawImage(rgbLinear, new Point(0, rowHeight * 1), 1f)
            .DrawImage(hsb, new Point(0, rowHeight * 2), 1f)
            .DrawImage(hsl, new Point(0, rowHeight * 3), 1f)
            .DrawImage(hwb, new Point(0, rowHeight * 4), 1f)
            .DrawImage(xyz, new Point(0, rowHeight * 5), 1f)
            .DrawImage(xyy, new Point(0, rowHeight * 6), 1f)
            .DrawImage(lab, new Point(0, rowHeight * 7), 1f)
            .DrawImage(lchab, new Point(0, rowHeight * 8), 1f)
            .DrawImage(luv, new Point(0, rowHeight * 9), 1f)
            .DrawImage(lchuv, new Point(0, rowHeight * 10), 1f)
            .DrawImage(hsluv, new Point(0, rowHeight * 11), 1f)
            .DrawImage(hpluv, new Point(0, rowHeight * 12), 1f)
            .DrawImage(ictcp, new Point(0, rowHeight * 13), 1f)
            .DrawImage(jzazbz, new Point(0, rowHeight * 14), 1f)
            .DrawImage(jzczhz, new Point(0, rowHeight * 15), 1f)
            .DrawImage(oklab, new Point(0, rowHeight * 16), 1f)
            .DrawImage(oklch, new Point(0, rowHeight * 17), 1f)
            .DrawImage(cam02, new Point(0, rowHeight * 18), 1f)
            .DrawImage(cam16, new Point(0, rowHeight * 19), 1f)
            .DrawImage(hct, new Point(0, rowHeight * 20), 1f)
        );

        return columnImage;
    }
}

void Temperature()
{
    const int width = 1200;
    const int rows = 1; 
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
    
    var text = Css.Black;
    
    var scaled = Utils.Draw(("CCT (1,000 K - 13,000 K)", text), width, rowHeight, scaledPoints.ToArray(), 
        (start, end, amount) => start.Mix(end, ColourSpace.Rgb, amount));
    
    var image = new Image<Rgba32>(width, rowHeight * rows);
    image.Mutate(context => context
        .DrawImage(scaled, new Point(0, rowHeight * 0), 1f)
    );

    image.Save(Path.Combine(outputDirectory, "gradient-temperature.png"));
}

void VisionDeficiency()
{
    const int width = 1200;
    const int rows = 5; 
    const int rowHeight = 100;

    // not using OKLCH for the spectrum because the uniform luminance results in flat gradient for Achromatopsia
    var colourPoints = new Unicolour[]
    {
        new(ColourSpace.Hsb, 0, 0.666, 1),
        new(ColourSpace.Hsb, 360, 0.666, 1)
    };

    var darkText = new Unicolour("#404046");
    
    var none = Utils.Draw(("No deficiency", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(end, ColourSpace.Hsb, amount, HueSpan.Increasing));
    var protanopia = Utils.Draw(("Protanopia", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(end, ColourSpace.Hsb, amount, HueSpan.Increasing).SimulateProtanopia());
    var deuteranopia = Utils.Draw(("Deuteranopia", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(end, ColourSpace.Hsb, amount, HueSpan.Increasing).SimulateDeuteranopia());
    var tritanopia = Utils.Draw(("Tritanopia", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(end, ColourSpace.Hsb, amount, HueSpan.Increasing).SimulateTritanopia());
    var achromatopsia = Utils.Draw(("Achromatopsia", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(end, ColourSpace.Hsb, amount, HueSpan.Increasing).SimulateAchromatopsia());

    var image = new Image<Rgba32>(width, rowHeight * rows);
    image.Mutate(context => context
        .DrawImage(none, new Point(0, rowHeight * 0), 1f)
        .DrawImage(protanopia, new Point(0, rowHeight * 1), 1f)
        .DrawImage(deuteranopia, new Point(0, rowHeight * 2), 1f)
        .DrawImage(tritanopia, new Point(0, rowHeight * 3), 1f)
        .DrawImage(achromatopsia, new Point(0, rowHeight * 4), 1f)
    );

    image.Save(Path.Combine(outputDirectory, "gradient-vision-deficiency.png"));
}

void AlphaInterpolation()
{
    const int width = 1000;
    const int rows = 2; 
    const int rowHeight = 120;

    var colourPoints = new[] { Css.Red, Css.Transparent, Css.Blue };
    var text = Css.Black;
    
    var premultiplied = Utils.Draw(("With premultiplied alpha", text), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(end, ColourSpace.Rgb, amount, premultiplyAlpha: true));
    var notPremultiplied = Utils.Draw(("Without premultiplied alpha", text), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(end, ColourSpace.Rgb, amount, premultiplyAlpha: false));
    
    var image = new Image<Rgba32>(width, rowHeight * rows);
    image.Mutate(context => context
        .DrawImage(premultiplied, new Point(0, rowHeight * 0), 1f)
        .DrawImage(notPremultiplied, new Point(0, rowHeight * 1), 1f)
    );

    image.Save(Path.Combine(outputDirectory, "gradient-alpha-interpolation.png"));
}