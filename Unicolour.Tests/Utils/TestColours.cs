namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// NamedColours.csv is a list of 145 colours taken from https://en.wikipedia.org/wiki/X11_color_names
internal static class TestColours
{
    private static readonly Random Random = new();
    
    public static readonly List<TestColour> NamedColours;
    public static readonly List<string> RandomHexColours = new();
    public static readonly List<ColourTriplet> RandomRgb255Colours = new();
    public static readonly List<ColourTriplet> RandomRgbColours = new();
    public static readonly List<ColourTriplet> RandomHsbColours = new();
    public static readonly List<ColourTriplet> RandomHslColours = new();
    public static readonly List<ColourTriplet> RandomXyzColours = new();
    public static readonly List<ColourTriplet> RandomLabColours = new();
    public static readonly List<ColourTriplet> RandomLchabColours = new();
    public static readonly List<ColourTriplet> RandomLuvColours = new();
    public static readonly List<ColourTriplet> RandomLchuvColours = new();
    public static readonly List<ColourTriplet> RandomJzazbzColours = new();
    public static readonly List<ColourTriplet> RandomJzczhzColours = new();
    public static readonly List<ColourTriplet> RandomOklabColours = new();
    public static readonly List<ColourTriplet> RandomOklchColours = new();

    static TestColours()
    {
        NamedColours = File.ReadAllLines(Path.Combine("Utils", "NamedColours.csv"))
            .Skip(1).Select(CreateNamedColour).ToList();
        
        for (var i = 0; i < 1000; i++)
        {
            RandomHexColours.Add(GenerateRandomHex());
            RandomRgb255Colours.Add(GetRandomRgb255());
            RandomRgbColours.Add(GetRandomRgb());
            RandomHsbColours.Add(GetRandomHsb());
            RandomHslColours.Add(GetRandomHsl());
            RandomXyzColours.Add(GetRandomXyz());
            RandomLabColours.Add(GetRandomLab());
            RandomLchabColours.Add(GetRandomLchab());
            RandomLuvColours.Add(GetRandomLuv());
            RandomLchuvColours.Add(GetRandomLchuv());
            RandomJzazbzColours.Add(GetRandomJzazbz());
            RandomJzczhzColours.Add(GetRandomJzczhz());
            RandomOklabColours.Add(GetRandomOklab());
            RandomOklchColours.Add(GetRandomOklch());
        }
    }

    // W3C has useful information about the practical range of values (e.g. https://www.w3.org/TR/css-color-4/#serializing-oklab-oklch)
    internal static ColourTriplet GetRandomRgb255() => new(Random.Next(256), Random.Next(256), Random.Next(256));
    internal static ColourTriplet GetRandomRgb() => new(GetRandom(), GetRandom(), GetRandom());
    internal static ColourTriplet GetRandomHsb() => new(GetRandom(0, 360), GetRandom(), GetRandom());
    internal static ColourTriplet GetRandomHsl() => new(GetRandom(0, 360), GetRandom(), GetRandom());
    internal static ColourTriplet GetRandomXyz() => new(GetRandom(), GetRandom(), GetRandom());
    internal static ColourTriplet GetRandomLab() => new(GetRandom(0, 100), GetRandom(-128, 128), GetRandom(-128, 128));
    internal static ColourTriplet GetRandomLchab() => new(GetRandom(0, 100), GetRandom(0, 230), GetRandom(0, 360));
    internal static ColourTriplet GetRandomLuv() => new(GetRandom(0, 100), GetRandom(-100, 100), GetRandom(-100, 100));
    internal static ColourTriplet GetRandomLchuv() => new(GetRandom(0, 100), GetRandom(0, 230), GetRandom(0, 360));
    internal static ColourTriplet GetRandomOklab() => new(GetRandom(), GetRandom(-0.5, 0.5), GetRandom(-0.5, 0.5));
    internal static ColourTriplet GetRandomOklch() => new(GetRandom(), GetRandom(0, 0.5), GetRandom(0, 360));
    internal static ColourTriplet GetRandomJzazbz() => new(GetRandom(0, 0.17), GetRandom(-0.10, 0.11), GetRandom(-0.16, 0.12)); // from own test values since ranges suggested by paper (0>1, -0.5>0.5, -0.5>0.5) easily produce XYZ with NaNs [https://opg.optica.org/oe/fulltext.cfm?uri=oe-25-13-15131&id=368272]
    internal static ColourTriplet GetRandomJzczhz() => new(GetRandom(0, 0.17), GetRandom(0, 0.16), GetRandom(0, 360)); // from own test values since
    internal static double GetRandomAlpha() => Random.NextDouble();

    private static double GetRandom() => Random.NextDouble();
    private static double GetRandom(double min, double max) => Random.NextDouble() * (max - min) + min;

    private static TestColour CreateNamedColour(string csvRow)
    {
        var items = csvRow.Split(",", StringSplitOptions.TrimEntries);
        
        return new TestColour
        {
            Name = items[0],
            Hex = items[1],
            Rgb = new(FromPercentage(items[2]), FromPercentage(items[3]), FromPercentage(items[4])),
            Hsl = new(FromDegrees(items[5]), FromPercentage(items[6]), FromPercentage(items[7])),
            Hsb = new(FromDegrees(items[5]), FromPercentage(items[8]), FromPercentage(items[9]))
        };
    }

    private static double FromPercentage(string text) => double.Parse(text.TrimEnd('%')) / 100.0;
    private static double FromDegrees(string text) => double.Parse(text.TrimEnd('°'));
    
    private static string GenerateRandomHex()
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