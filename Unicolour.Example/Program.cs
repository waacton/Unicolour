using Wacton.Unicolour;
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
    
    var purple = Unicolour.FromHsb(260, 1.0, 0.33);
    var orange = Unicolour.FromHsb(30, 0.66, 1.0);
    var pink = Unicolour.FromHex("#FF1493");
    var cyan = Unicolour.FromRgb255(0, 255, 255);
    var black = Unicolour.FromRgb(0, 0, 0);
    var green = Unicolour.FromRgb(0, 1, 0);
    
    var light = Unicolour.FromHex("#E8E8FF");
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
        var rgb = Gradient.Draw(("RGB", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixRgb(end, amount));
        var rgbLinear = Gradient.Draw(("RGB Linear", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixRgbLinear(end, amount));
        var hsb = Gradient.Draw(("HSB", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixHsb(end, amount));
        var hsl = Gradient.Draw(("HSL", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixHsl(end, amount));
        var hwb = Gradient.Draw(("HWB", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixHwb(end, amount));
        var xyz = Gradient.Draw(("XYZ", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixXyz(end, amount));
        var xyy = Gradient.Draw(("xyY", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixXyy(end, amount));
        var lab = Gradient.Draw(("LAB", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixLab(end, amount));
        var lchab = Gradient.Draw(("LCHab", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixLchab(end, amount));
        var luv = Gradient.Draw(("LUV", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixLuv(end, amount));
        var lchuv = Gradient.Draw(("LCHuv", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixLchuv(end, amount));
        var hsluv = Gradient.Draw(("HSLuv", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixHsluv(end, amount));
        var hpluv = Gradient.Draw(("HPLuv", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixHpluv(end, amount));
        var ictcp = Gradient.Draw(("ICtCp", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixIctcp(end, amount));
        var jzazbz = Gradient.Draw(("JzAzBz", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixJzazbz(end, amount));
        var jzczhz = Gradient.Draw(("JzCzHz", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixJzczhz(end, amount));
        var oklab = Gradient.Draw(("OKLAB", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixOklab(end, amount));
        var oklch = Gradient.Draw(("OKLCH", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixOklch(end, amount));
        var cam02 = Gradient.Draw(("CAM02", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixCam02(end, amount));
        var cam16 = Gradient.Draw(("CAM16", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixCam16(end, amount));
        var hct = Gradient.Draw(("HCT", light), columnWidth, rowHeight, colourPoints, (start, end, amount) => start.MixHct(end, amount));

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
    var colourPoints = new[]
    {
        Unicolour.FromHsb(0, 0.666, 1), Unicolour.FromHsb(30, 0.666, 1), Unicolour.FromHsb(60, 0.666, 1),
        Unicolour.FromHsb(90, 0.666, 1), Unicolour.FromHsb(120, 0.666, 1), Unicolour.FromHsb(150, 0.666, 1),
        Unicolour.FromHsb(180, 0.666, 1), Unicolour.FromHsb(210, 0.666, 1), Unicolour.FromHsb(240, 0.666, 1),
        Unicolour.FromHsb(270, 0.666, 1), Unicolour.FromHsb(300, 0.666, 1), Unicolour.FromHsb(330, 0.666, 1),
        Unicolour.FromHsb(360, 0.666, 1)
    };

    var dark = Unicolour.FromHex("#404046");
    var none = Gradient.Draw(("No deficiency", dark), width, rowHeight, colourPoints, 
        (start, end, amount) => start.MixHsb(end, amount));
    var protanopia = Gradient.Draw(("Protanopia", dark), width, rowHeight, colourPoints, 
        (start, end, amount) => start.MixHsb(end, amount).SimulateProtanopia());
    var deuteranopia = Gradient.Draw(("Deuteranopia", dark), width, rowHeight, colourPoints, 
        (start, end, amount) => start.MixHsb(end, amount).SimulateDeuteranopia());
    var tritanopia = Gradient.Draw(("Tritanopia", dark), width, rowHeight, colourPoints, 
        (start, end, amount) => start.MixHsb(end, amount).SimulateTritanopia());
    var achromatopsia = Gradient.Draw(("Achromatopsia", dark), width, rowHeight, colourPoints, 
        (start, end, amount) => start.MixHsb(end, amount).SimulateAchromatopsia());

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
    
    var colourPoints = new[]
    {
        Unicolour.FromRgb(1, 0, 0, 1), 
        Unicolour.FromRgb(0, 0, 0, 0), 
        Unicolour.FromRgb(0, 0, 1, 1)
    };
    
    var black = Unicolour.FromHex("#000000");
    var premultiplied = Gradient.Draw(("With premultiplied alpha", black), width, rowHeight, colourPoints, 
        (start, end, amount) => start.MixRgb(end, amount, true));
    var notPremultiplied = Gradient.Draw(("Without premultiplied alpha", black), width, rowHeight, colourPoints, 
        (start, end, amount) => start.MixRgb(end, amount, false));
    
    var image = new Image<Rgba32>(width, rowHeight * rows);
    image.Mutate(context => context
        .DrawImage(premultiplied, new Point(0, rowHeight * 0), 1f)
        .DrawImage(notPremultiplied, new Point(0, rowHeight * 1), 1f)
    );

    image.Save("alpha-interpolation.png");
}