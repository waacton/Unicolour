namespace Wacton.Unicolour.Example.Web;

using Wacton.Unicolour;

internal static class ColourLookup
{
    internal static readonly Dictionary<ColourSpace, Range[]> RangeLookup = new()
    {
        { ColourSpace.Rgb255, [new(0, 255), new(0, 255), new(0, 255)] },
        { ColourSpace.Rgb, [new(0, 1), new(0, 1), new(0, 1)] },
        { ColourSpace.RgbLinear, [new(0, 1), new(0, 1), new(0, 1)] },
        { ColourSpace.Hsb, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Hsl, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Hwb, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Hsi, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Xyz, [new(0, 1.1), new(0, 1.1), new(0, 1.1)] },
        { ColourSpace.Xyy, [new(0, 1), new(0, 1), new(0, 1)] },
        { ColourSpace.Lab, [new(0, 100), new(-128, 128), new(-128, 128)] },
        { ColourSpace.Lchab, [new(0, 100), new(0, 230), new(0, 360)] },
        { ColourSpace.Luv, [new(0, 100), new(-100, 100), new(-100, 100)] },
        { ColourSpace.Lchuv, [new(0, 100), new(0, 230), new(0, 360)] },
        { ColourSpace.Hsluv, [new(0, 360), new(0, 100), new(0, 100)] },
        { ColourSpace.Hpluv, [new(0, 360), new(0, 100), new(0, 100)] },
        { ColourSpace.Ypbpr, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Ycbcr, [new(0, 255), new(0, 255), new(0, 255)] },
        { ColourSpace.Ycgco, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Yuv, [new(0, 1), new(-0.44, 0.44), new(-0.62, 0.62)] },
        { ColourSpace.Yiq, [new(0, 1), new(-0.60, 0.60), new(-0.53, 0.53)] },
        { ColourSpace.Ydbdr, [new(0, 1), new(-1.333, 1.333), new(-1.333, 1.333)] },
        { ColourSpace.Tsl, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Xyb, [new(-0.03, 0.03), new(0, 1.0), new(-0.4, 0.4)] },
        { ColourSpace.Ipt, [new(0, 1), new(-0.75, 0.75), new(-0.75, 0.75)] },
        { ColourSpace.Ictcp, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Jzazbz, [new(0, 0.17), new(-0.11, 0.11), new(-0.16, 0.16)] },
        { ColourSpace.Jzczhz, [new(0, 0.17), new(0, 0.16), new(0, 360)] },
        { ColourSpace.Oklab, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Oklch, [new(0, 1), new(0, 0.5), new(0, 360)] },
        { ColourSpace.Okhsv, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Okhsl, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Okhwb, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Cam02, [new(0, 100), new(-50, 50), new(-50, 50)] },
        { ColourSpace.Cam16, [new(0, 100), new(-50, 50), new(-50, 50)] },
        { ColourSpace.Hct, [new(0, 360), new(0, 120), new(0, 100)] }
    };
    
    internal static readonly Dictionary<ColourSpace, string[]> AxisLookup = new()
    {
        { ColourSpace.Rgb255, ["R", "G", "B"] },
        { ColourSpace.Rgb, ["R", "G", "B"] },
        { ColourSpace.RgbLinear, ["R", "G", "B"] },
        { ColourSpace.Hsb, ["H", "S", "B"] },
        { ColourSpace.Hsl, ["H", "S", "L"] },
        { ColourSpace.Hwb, ["H", "W", "B"] },
        { ColourSpace.Hsi, ["H", "S", "I"] },
        { ColourSpace.Xyz, ["X", "Y", "Z"] },
        { ColourSpace.Xyy, ["x", "y", "Y"] },
        { ColourSpace.Lab, ["L", "A", "B"] },
        { ColourSpace.Lchab, ["L", "C", "H"] },
        { ColourSpace.Luv, ["L", "U", "V"] },
        { ColourSpace.Lchuv, ["L", "C", "H"] },
        { ColourSpace.Hsluv, ["H", "S", "L"] },
        { ColourSpace.Hpluv, ["H", "S", "L"] },
        { ColourSpace.Ypbpr, ["Y", "Pb", "Pr"] },
        { ColourSpace.Ycbcr, ["Y", "Cb", "Cr"] },
        { ColourSpace.Ycgco, ["Y", "Cg", "Co"] },
        { ColourSpace.Yuv, ["Y", "U", "V"] },
        { ColourSpace.Yiq, ["Y", "I", "Q"] },
        { ColourSpace.Ydbdr, ["Y", "Db", "Dr"] },
        { ColourSpace.Tsl, ["T", "S", "L"] },
        { ColourSpace.Xyb, ["X", "Y", "B"] },
        { ColourSpace.Ipt, ["I", "P", "T"] },
        { ColourSpace.Ictcp, ["I", "Ct", "Cp"] },
        { ColourSpace.Jzazbz, ["J", "A", "B"] },
        { ColourSpace.Jzczhz, ["J", "C", "H"] },
        { ColourSpace.Oklab, ["L", "A", "B"] },
        { ColourSpace.Oklch, ["L", "C", "H"] },
        { ColourSpace.Okhsv, ["H", "S", "V"] },
        { ColourSpace.Okhsl, ["H", "S", "L"] },
        { ColourSpace.Okhwb, ["H", "W", "B"] },
        { ColourSpace.Cam02, ["J", "A", "B"] },
        { ColourSpace.Cam16, ["J", "A", "B"] },
        { ColourSpace.Hct, ["H", "C", "T"] }
    };
}

internal record Range(double Min, double Max)
{
    internal readonly double Distance = Math.Abs(Max - Min);
}