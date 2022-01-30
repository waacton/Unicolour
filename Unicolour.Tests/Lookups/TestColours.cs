namespace Wacton.Unicolour.Tests.Lookups;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// NamedColours.csv is a list of 145 colours taken from https://en.wikipedia.org/wiki/X11_color_names
internal static class TestColours
{
    private static readonly Random Random = new();
    
    public static readonly List<TestColour> NamedColours;
    public static readonly List<TestColour> OpenCvColours;
    public static readonly List<(int r, int g, int b)> RandomRGB255s = new();
    public static readonly List<(double r255, double g255, double b255)> RandomRGBs = new();
    public static readonly List<(double h, double s, double b)> RandomHSBs = new();
    
    static TestColours()
    {
        NamedColours = File.ReadAllLines(Path.Combine("Lookups", "NamedColours.csv"))
            .Skip(1).Select(CreateNamedColour).ToList();
        
        OpenCvColours = File.ReadAllLines(Path.Combine("Lookups", "OpenCvColours.csv"))
            .Select(CreateOpenCvColour).ToList();

        for (var i = 0; i < 1000; i++)
        {
            RandomRGB255s.Add((Random.Next(256), Random.Next(256), Random.Next(256)));
            RandomRGBs.Add((Random.NextDouble(), Random.NextDouble(), Random.NextDouble()));
            RandomHSBs.Add((Random.NextDouble(), Random.NextDouble(), Random.NextDouble()));
        }
    }

    public static TestColour GetOpenCvColour(string name) => OpenCvColours.Single(x => x.Name == name);

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