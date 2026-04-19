using System;
using System.Collections.Generic;

namespace Wacton.Unicolour.Tests.Utils;

internal static class Rng
{
    private static readonly Random Random = new();

    internal static bool Bool() => Between(0, 1) >= 0.5;

    internal static double Between(double min, double max, bool integer = false)
    {
        return integer ? Int((int)min, (int)max) : Double(min, max);
    }

    private static double Double(double min, double max) => Random.NextDouble() * (max - min) + min;
    private static int Int(int min, int max) => Random.Next(max - min) + min;

    private static ColourTriplet Triplet(ColourSpace colourSpace, GamutRange gamutRange)
    {
        var gamut = Gamut.Lookup[colourSpace];

        // most spaces have a single range within which to select a random value
        // but WXY has 2 (a positive region and a negative region), which also needs to be chosen at random
        Range RandomRegion(Range[] regions) => regions.Length == 1 ? regions[0] : regions[Int(0, regions.Length)];

        var channel1 = gamutRange != GamutRange.Typical ? gamut.Channel1.full : RandomRegion(gamut.Channel1.regions);
        var channel2 = gamutRange != GamutRange.Typical ? gamut.Channel2.full : RandomRegion(gamut.Channel2.regions);
        var channel3 = gamutRange != GamutRange.Typical ? gamut.Channel3.full : RandomRegion(gamut.Channel3.regions);
        var integer = colourSpace == ColourSpace.Rgb255;
        return gamutRange switch
        {
            GamutRange.Typical => new(Between(channel1, integer), Between(channel2, integer), Between(channel3, integer)),
            GamutRange.Extended => new(Extend(channel1, integer), Extend(channel2, integer), Extend(channel3, integer)),
            GamutRange.Outside => new(Not(channel1, integer), Not(channel2, integer), Not(channel3, integer)),
            _ => throw new ArgumentOutOfRangeException(nameof(gamutRange), gamutRange, null)
        };
    }

    internal static List<ColourTriplet> Triplets(ColourSpace colourSpace, int count) => Triplets(colourSpace, count, GamutRange.Typical, GamutRange.Extended, GamutRange.Outside);
    internal static List<ColourTriplet> Triplets(ColourSpace colourSpace, int count, params GamutRange[] gamuts)
    {
        List<ColourTriplet> triplets = [];
        var countPerGamut = count / gamuts.Length;
        for (var i = 0; i < countPerGamut; i++)
        {
            foreach (var gamut in gamuts)
            {
                triplets.Add(Triplet(colourSpace, gamut));
            }
        }

        return triplets;
    }

    private static double Alpha() => Between(0, 1);

    internal static Unicolour Unicolour(ColourSpace colourSpace, Configuration? configuration = null)
    {
        var triplet = Triplet(colourSpace, GamutRange.Typical);
        return new Unicolour(configuration ?? Configuration.Default, colourSpace, triplet.Tuple, Alpha());
    }

    internal static List<Unicolour> Unicolours(ColourSpace colourSpace, int count)
    {
        List<Unicolour> colours = [];
        for (var i = 0; i < count; i++)
        {
            colours.Add(Unicolour(colourSpace));
        }

        return colours;
    }

    internal static Temperature Temperature() => new(Between(1000, 20000), Between(-0.05, 0.05));
    internal static Chromaticity Chromaticity() => new(Between(0, 0.75), Between(0, 0.85));

    internal static string Hex()
    {
        const string hexChars = "0123456789abcdefABCDEF";
        var useHash = Bool();
        var length = Bool() ? 6 : 8;

        var hex = useHash ? "#" : string.Empty;
        for (var i = 0; i < length; i++)
        {
            hex += hexChars[Int(0, hexChars.Length)];
        }

        return hex;
    }

    private static double Between(Range range, bool integer)
    {
        return Between(range.Lower, range.Upper, integer);
    }

    private static double Extend(Range range, bool integer)
    {
        var min = range.Lower - range.Distance;
        var max = range.Upper + range.Distance;
        return Between(min, max, integer);
    }

    private static double Not(Range range, bool integer)
    {
        var goAbove = Bool();
        if (goAbove)
        {
            var min = range.Upper;
            var max = range.Upper + range.Distance;
            return Between(min, max, integer);
        }
        else
        {
            var min = range.Lower - range.Distance;
            var max = range.Lower;
            return Between(min, max, integer);
        }
    }
}