namespace Wacton.Unicolour.Tests.Lookups;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// NamedColours.csv is a list of 145 colours taken from https://en.wikipedia.org/wiki/X11_color_names
internal static class TestColours
{
    public static readonly List<TestColour> NamedColours;
    public static readonly List<TestColour> OpenCvColours;
    
    static TestColours()
    {
        NamedColours = File.ReadAllLines(Path.Combine("Lookups", "NamedColours.csv"))
            .Skip(1).Select(CreateNamedColour).ToList();
        
        OpenCvColours = File.ReadAllLines(Path.Combine("Lookups", "OpenCvColours.csv"))
            .Select(CreateOpenCvColour).ToList();
    }

    private static TestColour CreateNamedColour(string csvRow)
    {
        var items = csvRow.Split(",", StringSplitOptions.TrimEntries);
        
        return new TestColour
        {
            Name = items[0],
            Hex = items[1],
            Rgb = (FromPercentage(items[2]), FromPercentage(items[3]), FromPercentage(items[4])),
            Hsl = (FromDegrees(items[5]), FromPercentage(items[6]), FromPercentage(items[7])),
            Hsb = (FromDegrees(items[5]), FromPercentage(items[8]), FromPercentage(items[9]))
        };
    }
    
    private static TestColour CreateOpenCvColour(string csvRow)
    {
        var items = csvRow.Split(",", StringSplitOptions.TrimEntries);
        
        return new TestColour
        {
            Name = items[0],
            Rgb = (FromText(items[1]), FromText(items[2]), FromText(items[3])),
            Hsb = (FromText(items[4]), FromText(items[5]), FromText(items[6])),
            Xyz = (FromText(items[7]), FromText(items[8]), FromText(items[9])),
            Lab = (FromText(items[10]), FromText(items[11]), FromText(items[12]))
        };
    }
    
    private static double FromPercentage(string text) => double.Parse(text.TrimEnd('%')) / 100.0;
    private static double FromDegrees(string text) => double.Parse(text.TrimEnd('°'));
    private static double FromText(string text) => double.Parse(text);

}