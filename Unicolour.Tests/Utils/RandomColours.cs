using System;
using System.Collections.Generic;
using System.Linq;

namespace Wacton.Unicolour.Tests.Utils;

internal static class RandomColours
{
    // ReSharper disable CollectionNeverQueried.Global - used in test case sources by name
    public static readonly List<string> HexStrings = [];
    public static readonly List<ColourTriplet> Rgb255Triplets = [];
    public static readonly List<ColourTriplet> RgbTriplets = [];
    public static readonly List<ColourTriplet> RgbLinearTriplets = [];
    public static readonly List<ColourTriplet> HsbTriplets = [];
    public static readonly List<ColourTriplet> HslTriplets = [];
    public static readonly List<ColourTriplet> HwbTriplets = [];
    public static readonly List<ColourTriplet> HsiTriplets = [];
    public static readonly List<ColourTriplet> XyzTriplets = [];
    public static readonly List<ColourTriplet> XyyTriplets = [];
    public static readonly List<ColourTriplet> WxyTriplets = [];
    public static readonly List<ColourTriplet> LabTriplets = [];
    public static readonly List<ColourTriplet> LchabTriplets = [];
    public static readonly List<ColourTriplet> LuvTriplets = [];
    public static readonly List<ColourTriplet> LchuvTriplets = [];
    public static readonly List<ColourTriplet> HsluvTriplets = [];
    public static readonly List<ColourTriplet> HpluvTriplets = [];
    public static readonly List<ColourTriplet> YpbprTriplets = [];
    public static readonly List<ColourTriplet> YcbcrTriplets = [];
    public static readonly List<ColourTriplet> YcgcoTriplets = [];
    public static readonly List<ColourTriplet> YuvTriplets = [];
    public static readonly List<ColourTriplet> YiqTriplets = [];
    public static readonly List<ColourTriplet> YdbdrTriplets = [];
    public static readonly List<ColourTriplet> TslTriplets = [];
    public static readonly List<ColourTriplet> XybTriplets = [];
    public static readonly List<ColourTriplet> IptTriplets = [];
    public static readonly List<ColourTriplet> IctcpTriplets = [];
    public static readonly List<ColourTriplet> JzazbzTriplets = [];
    public static readonly List<ColourTriplet> JzczhzTriplets = [];
    public static readonly List<ColourTriplet> OklabTriplets = [];
    public static readonly List<ColourTriplet> OklchTriplets = [];
    public static readonly List<ColourTriplet> OkhsvTriplets = [];
    public static readonly List<ColourTriplet> OkhslTriplets = [];
    public static readonly List<ColourTriplet> OkhwbTriplets = [];
    public static readonly List<ColourTriplet> Cam02Triplets = [];
    public static readonly List<ColourTriplet> Cam16Triplets = [];
    public static readonly List<ColourTriplet> HctTriplets = [];
    public static readonly List<Temperature> Temperatures = [];
    public static readonly List<Chromaticity> Chromaticities = [];

    private static double UnboundRgb(double x) => TestUtils.RandomInt(2) == 0 ? -x : x + 1;
    private static double UnboundRgb255(double x) => TestUtils.RandomInt(2) == 0 ? -x : x + 255;
    private static ColourTriplet ToUnboundRgb(ColourTriplet triplet) => new(UnboundRgb(triplet.First), UnboundRgb(triplet.Second), UnboundRgb(triplet.Third));
    private static ColourTriplet ToUnboundRgb255(ColourTriplet triplet) => new(UnboundRgb255(triplet.First), UnboundRgb255(triplet.Second), UnboundRgb255(triplet.Third));
    
    public static List<ColourTriplet> UnboundRgb255Triplets => Rgb255Triplets.Select(ToUnboundRgb255).ToList();
    public static List<ColourTriplet> UnboundRgbTriplets => RgbTriplets.Select(ToUnboundRgb).ToList();
    public static List<ColourTriplet> UnboundRgbLinearTriplets => RgbLinearTriplets.Select(ToUnboundRgb).ToList();
    
    public static List<ColourTriplet> Rgb255TripletsSubset => Rgb255Triplets.Take(100).ToList();
    public static List<ColourTriplet> UnboundRgb255TripletsSubset => UnboundRgb255Triplets.Take(100).ToList();
    
    public static List<ColourTriplet> RgbTripletsSubset => RgbTriplets.Take(100).ToList();
    public static List<ColourTriplet> UnboundRgbTripletsSubset => UnboundRgbTriplets.Take(100).ToList();
    
    public static List<ColourTriplet> RgbLinearTripletsSubset => RgbLinearTriplets.Take(100).ToList();
    public static List<ColourTriplet> UnboundRgbLinearTripletsSubset => UnboundRgbLinearTriplets.Take(100).ToList();
    
    public static List<ColourTriplet> YpbprTripletsSubset => YpbprTriplets.Take(100).ToList();
    public static List<ColourTriplet> YcbcrTripletsSubset => YcbcrTriplets.Take(100).ToList();
    // ReSharper restore CollectionNeverQueried.Global
    
    static RandomColours()
    {
        for (var i = 0; i < 1000; i++)
        {
            HexStrings.Add(Hex());
            Rgb255Triplets.Add(Rgb255());
            RgbTriplets.Add(Rgb());
            RgbLinearTriplets.Add(RgbLinear());
            HsbTriplets.Add(Hsb());
            HslTriplets.Add(Hsl());
            HwbTriplets.Add(Hwb());
            HsiTriplets.Add(Hsi());
            XyzTriplets.Add(Xyz());
            XyyTriplets.Add(Xyy());
            WxyTriplets.Add(Wxy());
            LabTriplets.Add(Lab());
            LchabTriplets.Add(Lchab());
            LuvTriplets.Add(Luv());
            LchuvTriplets.Add(Lchuv());
            HsluvTriplets.Add(Hsluv());
            HpluvTriplets.Add(Hpluv());
            YpbprTriplets.Add(Ypbpr());
            YcbcrTriplets.Add(Ycbcr());
            YcgcoTriplets.Add(Ycgco());
            YuvTriplets.Add(Yuv());
            YiqTriplets.Add(Yiq());
            YdbdrTriplets.Add(Ydbdr());
            TslTriplets.Add(Tsl());
            XybTriplets.Add(Xyb());
            IptTriplets.Add(Ipt());
            IctcpTriplets.Add(Ictcp());
            JzazbzTriplets.Add(Jzazbz());
            JzczhzTriplets.Add(Jzczhz());
            OklabTriplets.Add(Oklab());
            OklchTriplets.Add(Oklch());
            OkhsvTriplets.Add(Okhsv());
            OkhslTriplets.Add(Okhsl());
            OkhwbTriplets.Add(Okhwb());
            Cam02Triplets.Add(Cam02());
            Cam16Triplets.Add(Cam16());
            HctTriplets.Add(Hct());
            Temperatures.Add(Temperature());
            Chromaticities.Add(Chromaticity());
        }
    }
    
    internal static Unicolour UnicolourFrom(ColourSpace colourSpace, Configuration? configuration = null)
    {
        return new Unicolour(configuration ?? Configuration.Default, colourSpace, GetRandomTriplet(colourSpace).Tuple, Alpha());
    }
    private static ColourTriplet GetRandomTriplet(ColourSpace colourSpace)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => Rgb(),
            ColourSpace.Rgb255 => Rgb255(),
            ColourSpace.RgbLinear => RgbLinear(),
            ColourSpace.Hsb => Hsb(),
            ColourSpace.Hsl => Hsl(),
            ColourSpace.Hwb => Hwb(),
            ColourSpace.Hsi => Hsi(),
            ColourSpace.Xyz => Xyz(),
            ColourSpace.Xyy => Xyy(),
            ColourSpace.Wxy => Wxy(),
            ColourSpace.Lab => Lab(),
            ColourSpace.Lchab => Lchab(),
            ColourSpace.Luv => Luv(),
            ColourSpace.Lchuv => Lchuv(),
            ColourSpace.Hsluv => Hsluv(),
            ColourSpace.Hpluv => Hpluv(),
            ColourSpace.Ypbpr => Ypbpr(),
            ColourSpace.Ycbcr => Ycbcr(),
            ColourSpace.Ycgco => Ycgco(),
            ColourSpace.Yuv => Yuv(),
            ColourSpace.Yiq => Yiq(),
            ColourSpace.Ydbdr => Ydbdr(),
            ColourSpace.Tsl => Tsl(),
            ColourSpace.Xyb => Xyb(),
            ColourSpace.Ipt => Ipt(),
            ColourSpace.Ictcp => Ictcp(),
            ColourSpace.Jzazbz => Jzazbz(),
            ColourSpace.Jzczhz => Jzczhz(),
            ColourSpace.Oklab => Oklab(),
            ColourSpace.Oklch => Oklch(),
            ColourSpace.Okhsv => Okhsv(),
            ColourSpace.Okhsl => Okhsl(),
            ColourSpace.Okhwb => Okhwb(),
            ColourSpace.Cam02 => Cam02(),
            ColourSpace.Cam16 => Cam16(),
            ColourSpace.Hct => Hct(),
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }

    private static double Rng() => TestUtils.RandomDouble();
    private static double Rng(double min, double max) => TestUtils.RandomDouble(min, max);

    // W3C has useful information about the practical range of values (e.g. https://www.w3.org/TR/css-color-4/#serializing-oklab-oklch)
    private static ColourTriplet Rgb255() => new(TestUtils.RandomInt(256), TestUtils.RandomInt(256), TestUtils.RandomInt(256));
    private static ColourTriplet Rgb() => new(Rng(), Rng(), Rng());
    private static ColourTriplet RgbLinear() => new(Rng(), Rng(), Rng());
    private static ColourTriplet Hsb() => new(Rng(0, 360), Rng(), Rng());
    private static ColourTriplet Hsl() => new(Rng(0, 360), Rng(), Rng());
    private static ColourTriplet Hwb() => new(Rng(0, 360), Rng(), Rng());
    private static ColourTriplet Hsi() => new(Rng(0, 360), Rng(), Rng());
    private static ColourTriplet Xyz() => new(Rng(), Rng(), Rng());
    private static ColourTriplet Xyy() => new(Rng(), Rng(), Rng());
    private static ColourTriplet Wxy() => new(Rng() >= 0.5 ? Rng(360, 700) : Rng(-566, -493.5), Rng(), Rng());
    private static ColourTriplet Lab() => new(Rng(0, 100), Rng(-128, 128), Rng(-128, 128));
    private static ColourTriplet Lchab() => new(Rng(0, 100), Rng(0, 230), Rng(0, 360));
    private static ColourTriplet Luv() => new(Rng(0, 100), Rng(-100, 100), Rng(-100, 100));
    private static ColourTriplet Lchuv() => new(Rng(0, 100), Rng(0, 230), Rng(0, 360));
    private static ColourTriplet Hsluv() => new(Rng(0, 360), Rng(0, 100), Rng(0, 100));
    private static ColourTriplet Hpluv() => new(Rng(0, 360), Rng(0, 100), Rng(0, 100));
    private static ColourTriplet Ypbpr() => new(Rng(), Rng(-0.5, 0.5), Rng(-0.5, 0.5));
    private static ColourTriplet Ycbcr() => new(Rng(0, 255), Rng(0, 255), Rng(0, 255));
    private static ColourTriplet Ycgco() => new(Rng(), Rng(-0.5, 0.5), Rng(-0.5, 0.5));
    private static ColourTriplet Yuv() => new(Rng(), Rng(-0.436, 0.436), Rng(-0.614, 0.614));
    private static ColourTriplet Yiq() => new(Rng(), Rng(-0.595, 0.595), Rng(-0.522, 0.522));
    private static ColourTriplet Ydbdr() => new(Rng(), Rng(-1.333, 1.333), Rng(-1.333, 1.333));
    private static ColourTriplet Tsl() => new(Rng(0, 360), Rng(), Rng());
    private static ColourTriplet Xyb() => new(Rng(-0.03, 0.03), Rng(), Rng(-0.4, 0.4)); 
    private static ColourTriplet Ipt() => new(Rng(), Rng(-0.75, 0.75), Rng(-0.75, 0.75)); 
    private static ColourTriplet Ictcp() => new(Rng(), Rng(-0.5, 0.5), Rng(-0.5, 0.5)); 
    private static ColourTriplet Jzazbz() => new(Rng(0, 0.17), Rng(-0.10, 0.11), Rng(-0.16, 0.12)); // from own test values since ranges suggested by paper (0->1, -0.5->0.5, -0.5->0.5) easily produce XYZ with NaNs [https://doi.org/10.1364/OE.25.015131]
    private static ColourTriplet Jzczhz() => new(Rng(0, 0.17), Rng(0, 0.16), Rng(0, 360)); // from own test values
    private static ColourTriplet Oklab() => new(Rng(), Rng(-0.5, 0.5), Rng(-0.5, 0.5));
    private static ColourTriplet Oklch() => new(Rng(), Rng(0, 0.5), Rng(0, 360));
    private static ColourTriplet Okhsv() => new(Rng(0, 360), Rng(), Rng());
    private static ColourTriplet Okhsl() => new(Rng(0, 360), Rng(), Rng());
    private static ColourTriplet Okhwb() => new(Rng(0, 360), Rng(), Rng());
    private static ColourTriplet Cam02() => new(Rng(0, 100), Rng(-50, 50), Rng(-50, 50)); // from own test values 
    private static ColourTriplet Cam16() => new(Rng(0, 100), Rng(-50, 50), Rng(-50, 50)); // from own test values
    private static ColourTriplet Hct() => new(Rng(0, 360), Rng(0, 120), Rng(0, 100)); // from own test values 
    private static double Alpha() => TestUtils.RandomDouble();
    
    private static Temperature Temperature() => new(Rng(1000, 20000), Rng(-0.05, 0.05));
    private static Chromaticity Chromaticity() => new(Rng(0, 0.75), Rng(0, 0.85));

    private static string Hex()
    {
        const string hexChars = "0123456789abcdefABCDEF";
        var useHash = TestUtils.RandomInt(2) == 0;
        var length = TestUtils.RandomInt(2) == 0 ? 6 : 8;

        var hex = useHash ? "#" : string.Empty;
        for (var i = 0; i < length; i++)
        {
            hex += hexChars[TestUtils.RandomInt(hexChars.Length)];
        }

        return hex;
    }
}