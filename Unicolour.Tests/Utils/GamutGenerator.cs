using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Wacton.Unicolour.Tests.Utils;

// this class is deliberately unused
// seems to come in handy once every few months
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class GamutGenerator
{
    private static readonly ColourSpace[] ColourSpaces = TestUtils.AllColourSpaces;
    
    private static readonly Dictionary<ColourSpace, double> channel1Min = ColourSpaces.ToDictionary(x => x, _ => double.MaxValue);
    private static readonly Dictionary<ColourSpace, double> channel2Min = ColourSpaces.ToDictionary(x => x, _ => double.MaxValue);
    private static readonly Dictionary<ColourSpace, double> channel3Min = ColourSpaces.ToDictionary(x => x, _ => double.MaxValue);
    
    private static readonly Dictionary<ColourSpace, double> channel1Max = ColourSpaces.ToDictionary(x => x, _ => double.MinValue);
    private static readonly Dictionary<ColourSpace, double> channel2Max = ColourSpaces.ToDictionary(x => x, _ => double.MinValue);
    private static readonly Dictionary<ColourSpace, double> channel3Max = ColourSpaces.ToDictionary(x => x, _ => double.MinValue);
    
    public static void Generate()
    {
        for (var r = 0; r < 256; r++)
        for (var g = 0; g < 256; g++)
        for (var b = 0; b < 256; b++)
        {
            var colour = new Unicolour(ColourSpace.Rgb255, r, g, b);

            foreach (var colourSpace in ColourSpaces)
            {
                var (first, second, third) = colour.GetRepresentation(colourSpace);
                
                if (first < channel1Min[colourSpace]) channel1Min[colourSpace] = first;
                if (second < channel2Min[colourSpace]) channel2Min[colourSpace] = second;
                if (third < channel3Min[colourSpace]) channel3Min[colourSpace] = third;
                
                if (first > channel1Max[colourSpace]) channel1Max[colourSpace] = first;
                if (second > channel2Max[colourSpace]) channel2Max[colourSpace] = second;
                if (third > channel3Max[colourSpace]) channel3Max[colourSpace] = third;
            }
        }

        var results = ColourSpaces.Select(GetCsv).ToList();
        var outputPath = Path.Combine(".", "gamuts.csv");
        File.WriteAllLines(outputPath, results);
    }

    private static string GetCsv(ColourSpace colourSpace)
    {
        return $"{colourSpace}," +
               $"{channel1Min[colourSpace]},{channel1Max[colourSpace]}," +
               $"{channel2Min[colourSpace]},{channel2Max[colourSpace]}," +
               $"{channel3Min[colourSpace]},{channel3Max[colourSpace]}";
    }
}