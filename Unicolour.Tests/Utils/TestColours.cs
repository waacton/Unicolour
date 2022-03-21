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
    public static readonly List<ColourTuple> RandomRgb255Colours = new();
    public static readonly List<ColourTuple> RandomRgbColours = new();
    public static readonly List<ColourTuple> RandomHsbColours = new();
    public static readonly List<ColourTuple> RandomHslColours = new();
    public static readonly List<string> RandomHexColours = new();
    
    static TestColours()
    {
        NamedColours = File.ReadAllLines(Path.Combine("Utils", "NamedColours.csv"))
            .Skip(1).Select(CreateNamedColour).ToList();

        for (var i = 0; i < 1000; i++)
        {
            RandomRgb255Colours.Add(new(Random.Next(256), Random.Next(256), Random.Next(256)));
            RandomRgbColours.Add(new(Random.NextDouble(), Random.NextDouble(), Random.NextDouble()));
            RandomHsbColours.Add(new(Random.NextDouble(), Random.NextDouble(), Random.NextDouble()));
            RandomHslColours.Add(new(Random.NextDouble(), Random.NextDouble(), Random.NextDouble()));
            RandomHexColours.Add(GenerateRandomHex());
        }
    }

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