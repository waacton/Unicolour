namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;

internal static class RandomColours
{
    private static readonly Random Random = new();
    
    public static readonly List<string> HexStrings = new();
    public static readonly List<ColourTriplet> Rgb255Triplets = new();
    public static readonly List<ColourTriplet> RgbTriplets = new();
    public static readonly List<ColourTriplet> HsbTriplets = new();
    public static readonly List<ColourTriplet> HslTriplets = new();
    public static readonly List<ColourTriplet> XyzTriplets = new();
    public static readonly List<ColourTriplet> XyyTriplets = new();
    public static readonly List<ColourTriplet> LabTriplets = new();
    public static readonly List<ColourTriplet> LchabTriplets = new();
    public static readonly List<ColourTriplet> LuvTriplets = new();
    public static readonly List<ColourTriplet> LchuvTriplets = new();
    public static readonly List<ColourTriplet> HsluvTriplets = new();
    public static readonly List<ColourTriplet> HpluvTriplets = new();
    public static readonly List<ColourTriplet> JzazbzTriplets = new();
    public static readonly List<ColourTriplet> JzczhzTriplets = new();
    public static readonly List<ColourTriplet> OklabTriplets = new();
    public static readonly List<ColourTriplet> OklchTriplets = new();

    static RandomColours()
    {
        for (var i = 0; i < 1000; i++)
        {
            HexStrings.Add(Hex());
            Rgb255Triplets.Add(Rgb255());
            RgbTriplets.Add(Rgb());
            HsbTriplets.Add(Hsb());
            HslTriplets.Add(Hsl());
            XyzTriplets.Add(Xyz());
            XyyTriplets.Add(Xyy());
            LabTriplets.Add(Lab());
            LchabTriplets.Add(Lchab());
            LuvTriplets.Add(Luv());
            LchuvTriplets.Add(Lchuv());
            HsluvTriplets.Add(Hsluv());
            HpluvTriplets.Add(Hpluv());
            JzazbzTriplets.Add(Jzazbz());
            JzczhzTriplets.Add(Jzczhz());
            OklabTriplets.Add(Oklab());
            OklchTriplets.Add(Oklch());
        }
    }

    // W3C has useful information about the practical range of values (e.g. https://www.w3.org/TR/css-color-4/#serializing-oklab-oklch)
    internal static ColourTriplet Rgb255() => new(Random.Next(256), Random.Next(256), Random.Next(256));
    internal static ColourTriplet Rgb() => new(Rng(), Rng(), Rng());
    internal static ColourTriplet Hsb() => new(Rng(0, 360), Rng(), Rng());
    internal static ColourTriplet Hsl() => new(Rng(0, 360), Rng(), Rng());
    internal static ColourTriplet Xyz() => new(Rng(), Rng(), Rng());
    internal static ColourTriplet Xyy() => new(Rng(), Rng(), Rng());
    internal static ColourTriplet Lab() => new(Rng(0, 100), Rng(-128, 128), Rng(-128, 128));
    internal static ColourTriplet Lchab() => new(Rng(0, 100), Rng(0, 230), Rng(0, 360));
    internal static ColourTriplet Luv() => new(Rng(0, 100), Rng(-100, 100), Rng(-100, 100));
    internal static ColourTriplet Lchuv() => new(Rng(0, 100), Rng(0, 230), Rng(0, 360));
    internal static ColourTriplet Hsluv() => new(Rng(0, 360), Rng(0, 100), Rng(0, 100));
    internal static ColourTriplet Hpluv() => new(Rng(0, 360), Rng(0, 100), Rng(0, 100));
    internal static ColourTriplet Oklab() => new(Rng(), Rng(-0.5, 0.5), Rng(-0.5, 0.5));
    internal static ColourTriplet Oklch() => new(Rng(), Rng(0, 0.5), Rng(0, 360));
    internal static ColourTriplet Jzazbz() => new(Rng(0, 0.17), Rng(-0.10, 0.11), Rng(-0.16, 0.12)); // from own test values since ranges suggested by paper (0>1, -0.5>0.5, -0.5>0.5) easily produce XYZ with NaNs [https://opg.optica.org/oe/fulltext.cfm?uri=oe-25-13-15131&id=368272]
    internal static ColourTriplet Jzczhz() => new(Rng(0, 0.17), Rng(0, 0.16), Rng(0, 360)); // from own test values since
    internal static double Alpha() => Random.NextDouble();

    private static double Rng() => Random.NextDouble();
    private static double Rng(double min, double max) => Random.NextDouble() * (max - min) + min;
    
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