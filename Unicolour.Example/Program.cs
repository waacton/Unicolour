using Wacton.Unicolour;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Example;

GenerateColourSpaceGradients();
GenerateVisionDeficiencyGradients();
GenerateAlphaInterpolation();
return;

void GenerateColourSpaceGradients()
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

    image.Save("gradients.png");
    return;

    Image<Rgba32> DrawColumn(Unicolour[] colourPoints)
    {
        var rgb = Gradient.Draw(("RGB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Rgb, end, amount));
        var rgbLinear = Gradient.Draw(("RGB Linear", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.RgbLinear, end, amount));
        var hsb = Gradient.Draw(("HSB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Hsb, end, amount));
        var hsl = Gradient.Draw(("HSL", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Hsl, end, amount));
        var hwb = Gradient.Draw(("HWB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Hwb, end, amount));
        var xyz = Gradient.Draw(("XYZ", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Xyz, end, amount));
        var xyy = Gradient.Draw(("xyY", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Xyy, end, amount));
        var lab = Gradient.Draw(("LAB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Lab, end, amount));
        var lchab = Gradient.Draw(("LCHab", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Lchab, end, amount));
        var luv = Gradient.Draw(("LUV", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Luv, end, amount));
        var lchuv = Gradient.Draw(("LCHuv", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Lchuv, end, amount));
        var hsluv = Gradient.Draw(("HSLuv", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Hsluv, end, amount));
        var hpluv = Gradient.Draw(("HPLuv", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Hpluv, end, amount));
        var ictcp = Gradient.Draw(("ICtCp", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Ictcp, end, amount));
        var jzazbz = Gradient.Draw(("JzAzBz", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Jzazbz, end, amount));
        var jzczhz = Gradient.Draw(("JzCzHz", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Jzczhz, end, amount));
        var oklab = Gradient.Draw(("OKLAB", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Oklab, end, amount));
        var oklch = Gradient.Draw(("OKLCH", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Oklch, end, amount));
        var cam02 = Gradient.Draw(("CAM02", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Cam02, end, amount));
        var cam16 = Gradient.Draw(("CAM16", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Cam16, end, amount));
        var hct = Gradient.Draw(("HCT", lightText), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.Mix(ColourSpace.Hct, end, amount));

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

void GenerateVisionDeficiencyGradients()
{
    const int width = 1200;
    const int rows = 5; 
    const int rowHeight = 100;

    // not using OKLCH for the spectrum because the uniform luminance results in flat gradient for Achromatopsia
    var colourPoints = new Unicolour[]
    {
        new(ColourSpace.Hsb, 0, 0.666, 1), new(ColourSpace.Hsb, 30, 0.666, 1), new(ColourSpace.Hsb, 60, 0.666, 1),
        new(ColourSpace.Hsb, 90, 0.666, 1), new(ColourSpace.Hsb, 120, 0.666, 1), new(ColourSpace.Hsb, 150, 0.666, 1),
        new(ColourSpace.Hsb, 180, 0.666, 1), new(ColourSpace.Hsb, 210, 0.666, 1), new(ColourSpace.Hsb, 240, 0.666, 1),
        new(ColourSpace.Hsb, 270, 0.666, 1), new(ColourSpace.Hsb, 300, 0.666, 1), new(ColourSpace.Hsb, 330, 0.666, 1),
        new(ColourSpace.Hsb, 360, 0.666, 1)
    };

    var darkText = new Unicolour("#404046");
    
    var none = Gradient.Draw(("No deficiency", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(ColourSpace.Hsb, end, amount));
    var protanopia = Gradient.Draw(("Protanopia", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(ColourSpace.Hsb, end, amount).SimulateProtanopia());
    var deuteranopia = Gradient.Draw(("Deuteranopia", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(ColourSpace.Hsb, end, amount).SimulateDeuteranopia());
    var tritanopia = Gradient.Draw(("Tritanopia", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(ColourSpace.Hsb, end, amount).SimulateTritanopia());
    var achromatopsia = Gradient.Draw(("Achromatopsia", darkText), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(ColourSpace.Hsb, end, amount).SimulateAchromatopsia());

    var image = new Image<Rgba32>(width, rowHeight * rows);
    image.Mutate(context => context
        .DrawImage(none, new Point(0, rowHeight * 0), 1f)
        .DrawImage(protanopia, new Point(0, rowHeight * 1), 1f)
        .DrawImage(deuteranopia, new Point(0, rowHeight * 2), 1f)
        .DrawImage(tritanopia, new Point(0, rowHeight * 3), 1f)
        .DrawImage(achromatopsia, new Point(0, rowHeight * 4), 1f)
    );

    image.Save("vision-deficiency.png");
}

void GenerateAlphaInterpolation()
{
    const int width = 1000;
    const int rows = 2; 
    const int rowHeight = 120;

    var colourPoints = new[] { Css.Red, Css.Transparent, Css.Blue };
    var text = Css.Black;
    
    var premultiplied = Gradient.Draw(("With premultiplied alpha", text), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(ColourSpace.Rgb, end, amount, true));
    var notPremultiplied = Gradient.Draw(("Without premultiplied alpha", text), width, rowHeight, colourPoints, 
        (start, end, amount) => start.Mix(ColourSpace.Rgb, end, amount, false));
    
    var image = new Image<Rgba32>(width, rowHeight * rows);
    image.Mutate(context => context
        .DrawImage(premultiplied, new Point(0, rowHeight * 0), 1f)
        .DrawImage(notPremultiplied, new Point(0, rowHeight * 1), 1f)
    );

    image.Save("alpha-interpolation.png");
}