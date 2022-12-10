namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// NamedColours.csv is a list of 145 colours taken from https://en.wikipedia.org/wiki/X11_color_names
internal static class NamedColours
{
    public static readonly List<TestColour> All;

    static NamedColours()
    {
        var csvData = File.ReadAllLines(Path.Combine("Utils", "NamedColours.csv"));
        All = csvData.Skip(1).Select(CreateNamedColour).ToList();
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
}