namespace Wacton.Unicolour.Tests.Lookups;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// NamedColours.csv is a list of 145 colours taken from https://en.wikipedia.org/wiki/X11_color_names
internal static class NamedColours
{
    public static readonly List<NamedColor> All;

    static NamedColours()
    {
        var allLines = File.ReadAllLines(Path.Combine("Lookups", "NamedColours.csv")).Skip(1);
        All = allLines.Select(x => new NamedColor(x)).ToList();
    }

    public class NamedColor
    {
        public string Name { get; }
        public string Hex { get; }
        public (double r, double g, double b) Rgb { get; }
        public (double h, double s, double l) Hsl { get; }
        public (double h, double s, double b) Hsb { get; }

        internal NamedColor(string csvRow)
        {
            var items = csvRow.Split(",", StringSplitOptions.TrimEntries);
            Name = items[0];
            Console.Write(Name);
            Hex = items[1];
            Rgb = (FromPercentage(items[2]), FromPercentage(items[3]), FromPercentage(items[4]));
            Hsl = (FromDegrees(items[5]), FromPercentage(items[6]), FromPercentage(items[7]));
            Hsb = (FromDegrees(items[5]), FromPercentage(items[8]), FromPercentage(items[9]));
        }

        private static double FromPercentage(string text) => double.Parse(text.TrimEnd('%')) / 100.0;

        private static double FromDegrees(string text) => double.Parse(text.TrimEnd('°'));

        public override string ToString() => $"{Name} · {Hex} · {Hsb}";
    }
}