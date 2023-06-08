namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;

internal static class RandomColours
{
    private static readonly Random Random = new();
    
    // ReSharper disable CollectionNeverQueried.Global - used in some test case sources by name
    public static readonly List<string> HexStrings = new();
    public static readonly List<ColourTriplet> Rgb255Triplets = new();
    public static readonly List<ColourTriplet> RgbTriplets = new();
    public static readonly List<ColourTriplet> HsbTriplets = new();
    public static readonly List<ColourTriplet> HslTriplets = new();
    public static readonly List<ColourTriplet> HwbTriplets = new();
    public static readonly List<ColourTriplet> XyzTriplets = new();
    public static readonly List<ColourTriplet> XyyTriplets = new();
    public static readonly List<ColourTriplet> LabTriplets = new();
    public static readonly List<ColourTriplet> LchabTriplets = new();
    public static readonly List<ColourTriplet> LuvTriplets = new();
    public static readonly List<ColourTriplet> LchuvTriplets = new();
    public static readonly List<ColourTriplet> HsluvTriplets = new();
    public static readonly List<ColourTriplet> HpluvTriplets = new();
    public static readonly List<ColourTriplet> IctcpTriplets = new();
    public static readonly List<ColourTriplet> JzazbzTriplets = new();
    public static readonly List<ColourTriplet> JzczhzTriplets = new();
    public static readonly List<ColourTriplet> OklabTriplets = new();
    public static readonly List<ColourTriplet> OklchTriplets = new();
    public static readonly List<ColourTriplet> Cam02Triplets = new();
    public static readonly List<ColourTriplet> Cam16Triplets = new();
    // ReSharper restore CollectionNeverQueried.Global
    
    static RandomColours()
    {
        for (var i = 0; i < 1000; i++)
        {
            HexStrings.Add(Hex());
            Rgb255Triplets.Add(Rgb255());
            RgbTriplets.Add(Rgb());
            HsbTriplets.Add(Hsb());
            HslTriplets.Add(Hsl());
            HwbTriplets.Add(Hwb());
            XyzTriplets.Add(Xyz());
            XyyTriplets.Add(Xyy());
            LabTriplets.Add(Lab());
            LchabTriplets.Add(Lchab());
            LuvTriplets.Add(Luv());
            LchuvTriplets.Add(Lchuv());
            HsluvTriplets.Add(Hsluv());
            HpluvTriplets.Add(Hpluv());
            IctcpTriplets.Add(Ictcp());
            JzazbzTriplets.Add(Jzazbz());
            JzczhzTriplets.Add(Jzczhz());
            OklabTriplets.Add(Oklab());
            OklchTriplets.Add(Oklch());
            Cam02Triplets.Add(Cam02());
            Cam16Triplets.Add(Cam16());
        }
    }

    // W3C has useful information about the practical range of values (e.g. https://www.w3.org/TR/css-color-4/#serializing-oklab-oklch)
    private static ColourTriplet Rgb255() => new(Random.Next(256), Random.Next(256), Random.Next(256));
    public static ColourTriplet Rgb() => new(Rng(), Rng(), Rng());
    public static ColourTriplet Hsb() => new(Rng(0, 360), Rng(), Rng());
    public static ColourTriplet Hsl() => new(Rng(0, 360), Rng(), Rng());
    public static ColourTriplet Hwb() => new(Rng(0, 360), Rng(), Rng());
    public static ColourTriplet Xyz() => new(Rng(), Rng(), Rng());
    public static ColourTriplet Xyy() => new(Rng(), Rng(), Rng());
    public static ColourTriplet Lab() => new(Rng(0, 100), Rng(-128, 128), Rng(-128, 128));
    public static ColourTriplet Lchab() => new(Rng(0, 100), Rng(0, 230), Rng(0, 360));
    public static ColourTriplet Luv() => new(Rng(0, 100), Rng(-100, 100), Rng(-100, 100));
    public static ColourTriplet Lchuv() => new(Rng(0, 100), Rng(0, 230), Rng(0, 360));
    public static ColourTriplet Hsluv() => new(Rng(0, 360), Rng(0, 100), Rng(0, 100));
    public static ColourTriplet Hpluv() => new(Rng(0, 360), Rng(0, 100), Rng(0, 100));
    public static ColourTriplet Ictcp() => new(Rng(), Rng(-0.5, 0.5), Rng(-0.5, 0.5)); 
    public static ColourTriplet Jzazbz() => new(Rng(0, 0.17), Rng(-0.10, 0.11), Rng(-0.16, 0.12)); // from own test values since ranges suggested by paper (0->1, -0.5->0.5, -0.5->0.5) easily produce XYZ with NaNs [https://doi.org/10.1364/OE.25.015131]
    public static ColourTriplet Jzczhz() => new(Rng(0, 0.17), Rng(0, 0.16), Rng(0, 360)); // from own test values
    public static ColourTriplet Oklab() => new(Rng(), Rng(-0.5, 0.5), Rng(-0.5, 0.5));
    public static ColourTriplet Oklch() => new(Rng(), Rng(0, 0.5), Rng(0, 360));
    public static ColourTriplet Cam02() => new(Rng(0, 100), Rng(-50, 50), Rng(-50, 50)); // from own test values 
    public static ColourTriplet Cam16() => new(Rng(0, 100), Rng(-50, 50), Rng(-50, 50)); // from own test values 
    public static double Alpha() => Random.NextDouble();
    
    private static double Rng() => Random.NextDouble();
    private static double Rng(double min, double max) => Random.NextDouble() * (max - min) + min;
    
    public static Unicolour UnicolourFromRgb() => Unicolour.FromRgb(Rgb().Tuple, Alpha());
    public static Unicolour UnicolourFromHsb() => Unicolour.FromHsb(Hsb().Tuple, Alpha());
    public static Unicolour UnicolourFromHsl() => Unicolour.FromHsl(Hsl().Tuple, Alpha());
    public static Unicolour UnicolourFromHwb() => Unicolour.FromHwb(Hwb().Tuple, Alpha());
    public static Unicolour UnicolourFromXyz() => Unicolour.FromXyz(Xyz().Tuple, Alpha());
    public static Unicolour UnicolourFromXyy() => Unicolour.FromXyy(Xyy().Tuple, Alpha());
    public static Unicolour UnicolourFromLab() => Unicolour.FromLab(Lab().Tuple, Alpha());
    public static Unicolour UnicolourFromLchab() => Unicolour.FromLchab(Lchab().Tuple, Alpha());
    public static Unicolour UnicolourFromLuv() => Unicolour.FromLuv(Luv().Tuple, Alpha());
    public static Unicolour UnicolourFromLchuv() => Unicolour.FromLchuv(Lchuv().Tuple, Alpha());
    public static Unicolour UnicolourFromHsluv() => Unicolour.FromHsluv(Hsluv().Tuple, Alpha());
    public static Unicolour UnicolourFromHpluv() => Unicolour.FromHpluv(Hpluv().Tuple, Alpha());
    public static Unicolour UnicolourFromIctcp() => Unicolour.FromIctcp(Ictcp().Tuple, Alpha());
    public static Unicolour UnicolourFromJzazbz() => Unicolour.FromJzazbz(Jzazbz().Tuple, Alpha());
    public static Unicolour UnicolourFromJzczhz() => Unicolour.FromJzczhz(Jzczhz().Tuple, Alpha());
    public static Unicolour UnicolourFromOklab() => Unicolour.FromOklab(Oklab().Tuple, Alpha());
    public static Unicolour UnicolourFromOklch() => Unicolour.FromOklch(Oklch().Tuple, Alpha());
    public static Unicolour UnicolourFromCam02() => Unicolour.FromCam02(Cam02().Tuple, Alpha());
    public static Unicolour UnicolourFromCam16() => Unicolour.FromCam16(Cam16().Tuple, Alpha());

    private static string Hex()
    {
        const string hexChars = "0123456789abcdefABCDEF";
        var useHash = Random.Next(0, 2) == 0;
        var length = Random.Next(0, 2) == 0 ? 6 : 8;

        var hex = useHash ? "#" : string.Empty;
        for (var i = 0; i < length; i++)
        {
            hex += hexChars[Random.Next(hexChars.Length)];
        }

        return hex;
    }
}