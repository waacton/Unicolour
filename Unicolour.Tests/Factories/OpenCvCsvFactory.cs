namespace Wacton.Unicolour.Tests.Factories;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Wacton.Unicolour.Tests.Utils;

internal static class OpenCvCsvFactory
{
    private static readonly List<TestColour> ColoursFromCsv;

    static OpenCvCsvFactory()
    {
        ColoursFromCsv = File.ReadAllLines(Path.Combine("Utils", "OpenCvColours.csv")).Select(FromCsvRow).ToList();
    }

    public static TestColour FromName(string name) => ColoursFromCsv.Single(x => x.Name == name);
    private static TestColour FromCsvRow(string csvRow)
    {
        var items = csvRow.Split(",", StringSplitOptions.TrimEntries);
        return new TestColour
        {
            Name = items[0],
            Rgb = new(FromText(items[1]), FromText(items[2]), FromText(items[3])),
            Hsb = new(FromText(items[4]), FromText(items[5]), FromText(items[6])),
            Hsl = new(FromText(items[7]), FromText(items[8]), FromText(items[9])),
            Xyz = new(FromText(items[10]), FromText(items[11]), FromText(items[12])),
            Lab = new(FromText(items[13]), FromText(items[14]), FromText(items[15])),
            Tolerances = OpenCvFactory.Tolerances
        };
    }
    
    private static double FromText(string text) => double.Parse(text);

    private static void GenerateCsvFile()
    {
        var rows = new List<string>();
        foreach (var namedColour in TestColours.NamedColours)
        {
            var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
            var (r255, g255, b255) = (systemColour.R, systemColour.G, systemColour.B);

            ITestColourFactory testColourFactory = new OpenCvFactory();
            var testColour = testColourFactory.FromRgb255(r255, g255, b255, namedColour.Name!);

            string Stringify(double value) => value.ToString(CultureInfo.InvariantCulture);
            var row = new List<string>
            {
                testColour.Name!, 
                Stringify(testColour.Rgb!.First), Stringify(testColour.Rgb.Second), Stringify(testColour.Rgb.Third),
                Stringify(testColour.Hsb!.First), Stringify(testColour.Hsb.Second), Stringify(testColour.Hsb.Third),
                Stringify(testColour.Xyz!.First), Stringify(testColour.Xyz.Second), Stringify(testColour.Xyz.Third),
                Stringify(testColour.Lab!.First), Stringify(testColour.Lab.Second), Stringify(testColour.Lab.Third)
            };
            
            rows.Add(string.Join(", ", row));
        }
        
        File.WriteAllLines("OpenCvColours.csv", rows);
    }
}