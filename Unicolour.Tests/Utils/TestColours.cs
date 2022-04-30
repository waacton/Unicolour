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
            RandomOklabColours.Add(GetRandomOklab());
            RandomOklchColours.Add(GetRandomOklch());
        }
    }

    internal static ColourTriplet GetRandomRgb255() => new(Random.Next(256), Random.Next(256), Random.Next(256));
    internal static ColourTriplet GetRandomRgb() => new(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
    internal static ColourTriplet GetRandomHsb() => new(Random.NextDouble() * 360, Random.NextDouble(), Random.NextDouble());
    internal static ColourTriplet GetRandomHsl() => new(Random.NextDouble() * 360, Random.NextDouble(), Random.NextDouble());
    internal static ColourTriplet GetRandomXyz() => new(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
    internal static ColourTriplet GetRandomLab() => new(Random.NextDouble() * 100, Random.NextDouble() * 256 - 128, Random.NextDouble() * 256 - 128);
    internal static ColourTriplet GetRandomLchab() => new(Random.NextDouble() * 100, Random.NextDouble() * 230, Random.NextDouble() * 360);
    internal static ColourTriplet GetRandomLuv() => new(Random.NextDouble() * 100, Random.NextDouble() * 200 - 100, Random.NextDouble() * 200 - 100);
    internal static ColourTriplet GetRandomLchuv() => new(Random.NextDouble() * 100, Random.NextDouble() * 230, Random.NextDouble() * 360);
    internal static ColourTriplet GetRandomOklab() => new(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
    internal static ColourTriplet GetRandomOklch() => new(Random.NextDouble() * 100, Random.NextDouble() * 230, Random.NextDouble() * 360);
    internal static double GetRandomAlpha() => Random.NextDouble();


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