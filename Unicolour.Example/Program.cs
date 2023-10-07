using Wacton.Unicolour;
using Wacton.Unicolour.Example;

GenerateColourSpaceGradients();
GenerateVisionDeficiencyGradients();
return;

void GenerateColourSpaceGradients()
{
    const int columns = 3;
    const int columnWidth = 800;
    const int rows = 19; 
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
        var rgb = Gradient.Draw(("RGB", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateRgb(end, distance));
        var hsb = Gradient.Draw(("HSB", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateHsb(end, distance));
        var hsl = Gradient.Draw(("HSL", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateHsl(end, distance));
        var hwb = Gradient.Draw(("HWB", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateHwb(end, distance));
        var xyz = Gradient.Draw(("XYZ", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateXyz(end, distance));
        var xyy = Gradient.Draw(("xyY", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateXyy(end, distance));
        var lab = Gradient.Draw(("LAB", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateLab(end, distance));
        var lchab = Gradient.Draw(("LCHab", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateLchab(end, distance));
        var luv = Gradient.Draw(("LUV", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateLuv(end, distance));
        var lchuv = Gradient.Draw(("LCHuv", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateLchuv(end, distance));
        var hsluv = Gradient.Draw(("HSLuv", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateHsluv(end, distance));
        var hpluv = Gradient.Draw(("HPLuv", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateHpluv(end, distance));
        var ictcp = Gradient.Draw(("ICtCp", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateIctcp(end, distance));
        var jzazbz = Gradient.Draw(("JzAzBz", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateJzazbz(end, distance));
        var jzczhz = Gradient.Draw(("JzCzHz", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateJzczhz(end, distance));
        var oklab = Gradient.Draw(("OKLAB", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateOklab(end, distance));
        var oklch = Gradient.Draw(("OKLCH", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateOklch(end, distance));
        var cam02 = Gradient.Draw(("CAM02", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateCam02(end, distance));
        var cam16 = Gradient.Draw(("CAM16", light), columnWidth, rowHeight, colourPoints, (start, end, distance) => start.InterpolateCam16(end, distance));

        var columnImage = new Image<Rgba32>(columnWidth, rowHeight * rows);
        columnImage.Mutate(context => context
            .DrawImage(rgb, new Point(0, rowHeight * 0), 1f)
            .DrawImage(hsb, new Point(0, rowHeight * 1), 1f)
            .DrawImage(hsl, new Point(0, rowHeight * 2), 1f)
            .DrawImage(hwb, new Point(0, rowHeight * 3), 1f)
            .DrawImage(xyz, new Point(0, rowHeight * 4), 1f)
            .DrawImage(xyy, new Point(0, rowHeight * 5), 1f)
            .DrawImage(lab, new Point(0, rowHeight * 6), 1f)
            .DrawImage(lchab, new Point(0, rowHeight * 7), 1f)
            .DrawImage(luv, new Point(0, rowHeight * 8), 1f)
            .DrawImage(lchuv, new Point(0, rowHeight * 9), 1f)
            .DrawImage(hsluv, new Point(0, rowHeight * 10), 1f)
            .DrawImage(hpluv, new Point(0, rowHeight * 11), 1f)
            .DrawImage(ictcp, new Point(0, rowHeight * 12), 1f)
            .DrawImage(jzazbz, new Point(0, rowHeight * 13), 1f)
            .DrawImage(jzczhz, new Point(0, rowHeight * 14), 1f)
            .DrawImage(oklab, new Point(0, rowHeight * 15), 1f)
            .DrawImage(oklch, new Point(0, rowHeight * 16), 1f)
            .DrawImage(cam02, new Point(0, rowHeight * 17), 1f)
            .DrawImage(cam16, new Point(0, rowHeight * 18), 1f)
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
        (start, end, distance) => start.InterpolateHsb(end, distance));
    var protanopia = Gradient.Draw(("Protanopia", dark), width, rowHeight, colourPoints, 
        (start, end, distance) => start.InterpolateHsb(end, distance).SimulateProtanopia());
    var deuteranopia = Gradient.Draw(("Deuteranopia", dark), width, rowHeight, colourPoints, 
        (start, end, distance) => start.InterpolateHsb(end, distance).SimulateDeuteranopia());
    var tritanopia = Gradient.Draw(("Tritanopia", dark), width, rowHeight, colourPoints, 
        (start, end, distance) => start.InterpolateHsb(end, distance).SimulateTritanopia());
    var achromatopsia = Gradient.Draw(("Achromatopsia", dark), width, rowHeight, colourPoints, 
        (start, end, distance) => start.InterpolateHsb(end, distance).SimulateAchromatopsia());

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