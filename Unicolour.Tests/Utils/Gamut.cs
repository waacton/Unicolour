using System.Collections.Generic;
using System.Linq;

namespace Wacton.Unicolour.Tests.Utils;

internal class Gamut
{ 
    private static readonly Gamut Rgb255 = new(new Range(0, 256), new Range(0, 256), new Range(0, 256));
    private static readonly Gamut Rgb = new(new Range(0, 1), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut RgbLinear = new(new Range(0, 1), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Hsb = new(new Range(0, 360), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Hsl = new(new Range(0, 360), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Hwb = new(new Range(0, 360), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Hsi = new(new Range(0, 360), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Xyz = new(new Range(0, 1), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Xyy = new(new Range(0, 1), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Wxy = new([new Range(360, 700), new Range(-566, -493.5)], new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Lab = new(new Range(0, 100), new Range(-128, 128), new Range(-128, 128));
    private static readonly Gamut Lchab = new(new Range(0, 100), new Range(0, 230), new Range(0, 360));
    private static readonly Gamut Luv = new(new Range(0, 100), new Range(-100, 100), new Range(-100, 100));
    private static readonly Gamut Lchuv = new(new Range(0, 100), new Range(0, 230), new Range(0, 360));
    private static readonly Gamut Hsluv = new(new Range(0, 360), new Range(0, 100), new Range(0, 100));
    private static readonly Gamut Hpluv = new(new Range(0, 360), new Range(0, 100), new Range(0, 100));
    private static readonly Gamut Ypbpr = new(new Range(0, 1), new Range(-0.5, 0.5), new Range(-0.5, 0.5));
    private static readonly Gamut Ycbcr = new(new Range(0, 255), new Range(0, 255), new Range(0, 255));
    private static readonly Gamut Ycgco = new(new Range(0, 1), new Range(-0.5, 0.5), new Range(-0.5, 0.5));
    private static readonly Gamut Yuv = new(new Range(0, 1), new Range(-0.436, 0.436), new Range(-0.614, 0.614));
    private static readonly Gamut Yiq = new(new Range(0, 1), new Range(-0.595, 0.595), new Range(-0.522, 0.522));
    private static readonly Gamut Ydbdr = new(new Range(0, 1), new Range(-1.333, 1.333), new Range(-1.333, 1.333));
    private static readonly Gamut Tsl = new(new Range(0, 360), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Xyb = new(new Range(-0.03, 0.03), new Range(0, 1), new Range(-0.4, 0.4));
    private static readonly Gamut Lms = new(new Range(0, 1), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Ipt = new(new Range(0, 1), new Range(-0.75, 0.75), new Range(-0.75, 0.75));
    private static readonly Gamut Ictcp = new(new Range(0, 0.581), new Range(-0.282, 0.274), new Range(-0.162, 0.279));
    private static readonly Gamut Jzazbz = new(new Range(0, 0.223), new Range(-0.110, 0.130), new Range(-0.186, 0.135));
    private static readonly Gamut Jzczhz = new(new Range(0, 0.223), new Range(0, 0.191), new Range(0, 360));
    private static readonly Gamut Oklab = new(new Range(0, 1), new Range(-0.5, 0.5), new Range(-0.5, 0.5)); // W3C has useful information about the practical range (e.g. https://www.w3.org/TR/css-color-4/#serializing-oklab-oklch)
    private static readonly Gamut Oklch = new(new Range(0, 1), new Range(0, 0.5), new Range(0, 360)); // W3C has useful information about the practical range (e.g. https://www.w3.org/TR/css-color-4/#serializing-oklab-oklch)
    private static readonly Gamut Okhsv = new(new Range(0, 360), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Okhsl = new(new Range(0, 360), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Okhwb = new(new Range(0, 360), new Range(0, 1), new Range(0, 1));
    private static readonly Gamut Oklrab = new(new Range(0, 1), new Range(-0.5, 0.5), new Range(-0.5, 0.5));
    private static readonly Gamut Oklrch = new(new Range(0, 1), new Range(0, 0.5), new Range(0, 360));
    private static readonly Gamut Cam02 = new(new Range(0, 100), new Range(-50, 50), new Range(-50, 50));
    private static readonly Gamut Cam16 = new(new Range(0, 100), new Range(-50, 50), new Range(-50, 50));
    private static readonly Gamut Hct = new(new Range(0, 360), new Range(0, 120), new Range(0, 100));
    private static readonly Gamut Munsell = new(new Range(0, 360), new Range(0, 10), new Range(0, 26));
    
    internal (Range[] regions, Range full) Channel1 { get; }
    internal (Range[] regions, Range full) Channel2 { get; }
    internal (Range[] regions, Range full) Channel3 { get; }

    private Gamut(Range channel1, Range channel2, Range channel3)
    {
        Channel1 = ([channel1], channel1);
        Channel2 = ([channel2], channel2);
        Channel3 = ([channel3], channel3);
    }
    
    private Gamut(Range[] channel1, Range channel2, Range channel3)
    {
        var minLower1 = channel1.Select(x => x.Lower).Min();
        var maxUpper1 = channel1.Select(x => x.Upper).Max();
        
        Channel1 = (channel1, new Range(minLower1, maxUpper1));
        Channel2 = ([channel2], channel2);
        Channel3 = ([channel3], channel3);
    }
    
    internal static readonly Dictionary<ColourSpace, Gamut> Lookup = new()
    {
        { ColourSpace.Rgb255, Rgb255 },
        { ColourSpace.Rgb, Rgb },
        { ColourSpace.RgbLinear, RgbLinear },
        { ColourSpace.Hsb, Hsb },
        { ColourSpace.Hsl, Hsl },
        { ColourSpace.Hwb, Hwb },
        { ColourSpace.Hsi, Hsi },
        { ColourSpace.Xyz, Xyz },
        { ColourSpace.Xyy, Xyy },
        { ColourSpace.Wxy, Wxy },
        { ColourSpace.Lab, Lab },
        { ColourSpace.Lchab, Lchab },
        { ColourSpace.Luv, Luv },
        { ColourSpace.Lchuv, Lchuv },
        { ColourSpace.Hsluv, Hsluv },
        { ColourSpace.Hpluv, Hpluv },
        { ColourSpace.Ypbpr, Ypbpr },
        { ColourSpace.Ycbcr, Ycbcr },
        { ColourSpace.Ycgco, Ycgco },
        { ColourSpace.Yuv, Yuv },
        { ColourSpace.Yiq, Yiq },
        { ColourSpace.Ydbdr, Ydbdr },
        { ColourSpace.Tsl, Tsl },
        { ColourSpace.Xyb, Xyb },
        { ColourSpace.Lms, Lms },
        { ColourSpace.Ipt, Ipt },
        { ColourSpace.Ictcp, Ictcp },
        { ColourSpace.Jzazbz, Jzazbz },
        { ColourSpace.Jzczhz, Jzczhz },
        { ColourSpace.Oklab, Oklab },
        { ColourSpace.Oklch, Oklch },
        { ColourSpace.Okhsv, Okhsv },
        { ColourSpace.Okhsl, Okhsl },
        { ColourSpace.Okhwb, Okhwb },
        { ColourSpace.Oklrab, Oklrab },
        { ColourSpace.Oklrch, Oklrch },
        { ColourSpace.Cam02, Cam02 },
        { ColourSpace.Cam16, Cam16 },
        { ColourSpace.Hct, Hct },
        { ColourSpace.Munsell, Munsell }
    };
}