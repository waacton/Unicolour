using SixLabors.ImageSharp;

namespace Wacton.Unicolour.Example.Gradients;

public static class ColourSpaces
{
    private const int Width = 1000;
    private const int RowHeight = 100;
    
    private static readonly Unicolour Purple = new(ColourSpace.Hsb, 260, 1.0, 0.333);
    private static readonly Unicolour Orange = new(ColourSpace.Hsl, 30, 1.0, 0.666);
    private static readonly Unicolour Green = new(ColourSpace.Rgb, 0, 1, 0);
    private static readonly Unicolour Black = new("#000000");
    private static readonly Unicolour White = new("#FFFFFF");
    
    private static readonly ColourSpace[] AllSpaces =
    [
        ColourSpace.Rgb, ColourSpace.Rgb255, ColourSpace.RgbLinear,
        ColourSpace.Hsb, ColourSpace.Hsl, ColourSpace.Hwb, ColourSpace.Hsi,
        ColourSpace.Xyz, ColourSpace.Xyy, ColourSpace.Wxy,
        ColourSpace.Lab, ColourSpace.Lchab, ColourSpace.Luv, ColourSpace.Lchuv, 
        ColourSpace.Hsluv, ColourSpace.Hpluv,
        ColourSpace.Ypbpr, ColourSpace.Ycbcr, ColourSpace.Ycgco, ColourSpace.Yuv, ColourSpace.Yiq, ColourSpace.Ydbdr,
        ColourSpace.Tsl, ColourSpace.Xyb,
        ColourSpace.Lms, ColourSpace.Ipt, ColourSpace.Ictcp, ColourSpace.Jzazbz, ColourSpace.Jzczhz,
        ColourSpace.Oklab, ColourSpace.Oklch, ColourSpace.Okhsv, ColourSpace.Okhsl, ColourSpace.Okhwb, ColourSpace.Oklrab, ColourSpace.Oklrch,
        ColourSpace.Cam02, ColourSpace.Cam16,
        ColourSpace.Hct,
        ColourSpace.Munsell
    ];
    
    public static void PurpleToOrange()
    {
        var rows = AllSpaces
            .Select(colourSpace => Utils.Draw((colourSpace.ToString(), White), Width, RowHeight, GetPurpleToOrangeColour(colourSpace)))
            .ToArray();

        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-spaces-purple-orange.png"));
    }

    private static Utils.GetColour GetPurpleToOrangeColour(ColourSpace colourSpace)
    {
        return column => Purple.Mix(Orange, colourSpace, column / (double)Width);
    }

    public static void BlackToGreen()
    {
        var rows = AllSpaces
            .Select(colourSpace => Utils.Draw((colourSpace.ToString(), White), Width, RowHeight, GetBlackToGreenColour(colourSpace)))
            .ToArray();

        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-spaces-black-green.png"));
    }

    private static Utils.GetColour GetBlackToGreenColour(ColourSpace colourSpace)
    {
        return column => Black.Mix(Green, colourSpace, column / (double)Width);
    }
}