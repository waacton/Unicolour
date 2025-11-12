using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripMunsellTests
{
    /*
     * the tolerances in this test are from an analysis of 10,000,000 roundtrip conversions
     * and demonstrates that a Munsell colour converts more accurately the closer it is to being within the known dataset
     * ----------
     * "sparse chroma" is where there is no chroma measurement for at least one of the bounding points;
     * "chroma limit scale" is a measure of how far outside the munsell dataset the chroma is
     *     e.g. for 10Y 4/ the max measured chroma is 12, so 10Y 4/24 returns max chroma scale of 2x
     */
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.MunsellTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
        var xyy = Munsell.ToXyy(original, TestUtils.CConfig.Xyz);
        var roundtrip = Munsell.FromXyy(xyy, TestUtils.CConfig.Xyz);

        var originalBounds = Munsell.GetBounds(original);
        var roundtripBounds = Munsell.GetBounds(roundtrip);
        
        (double h, double c) tolerance;
        if (!roundtrip.XyyToMunsellSearchResult!.Converged)
        {
            tolerance = (h: 7.5, c: 53.5);
        }
        else if (xyy.Chromaticity.X is < 0 or > 1 || xyy.Chromaticity.Y is < 0 or > 1)
        {
            tolerance = (h: 21.5, c: 62.5);
        }
        else if (IsSparseChroma(originalBounds) || IsSparseChroma(roundtripBounds))
        {
            tolerance = (h: 8.75, c: 21.5);
        }
        else if (original.C < 0.5)
        {
            tolerance = (h: 4, c: 0.0035);
        }
        else
        {
            var maxChromaScale = Math.Max(ChromaLimitScale(originalBounds), ChromaLimitScale(roundtripBounds));
            tolerance = maxChromaScale switch
            {
                >= 1.25 => (h: 6.5, c: 11),
                >= 1 => (h: 1.725, c: 2.1),
                _ => (h: 0.45, c: 0.0035)
            };
        }
        
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [tolerance.h, 5e-15, tolerance.c]);
    }

    /*
     * although some rare extreme cases of munsell conversion can result in significant roundtrip difference
     * most of the time the roundtrip is extremely accurate - even for chroma well outwith the dataset (e.g. 99C can roundtrip accurately)
     * so it seems worthwhile to confirm that, on average, roundtrip is indeed very accurate, regardless of within the dataset or not
     * ----------
     * the tolerances in the above test have to be lenient for the rare case that does not roundtrip nicely
     * but at least those cases are limited to where the algorithm does not have data, and extrapolation has been used instead
     */
    [Test]
    public void ViaXyyAverage()
    {
        var triplets = RandomColours.MunsellTriplets;

        var hDeltas = new List<double>();
        var vDeltas = new List<double>();
        var cDeltas = new List<double>();
        
        foreach (var triplet in triplets)
        {
            var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
            var xyy = Munsell.ToXyy(original, TestUtils.CConfig.Xyz);
            var roundtrip = Munsell.FromXyy(xyy, TestUtils.CConfig.Xyz);

            var hues = Hue.Unwrap(original.H, roundtrip.H);
            hDeltas.Add(Math.Abs(hues.start - hues.end));
            vDeltas.Add(Math.Abs(original.V - roundtrip.V));
            cDeltas.Add(Math.Abs(original.C - roundtrip.C));
        }
        
        Assert.That(hDeltas.Average(), Is.LessThan(0.05));
        Assert.That(vDeltas.Average(), Is.LessThan(5e-15));
        Assert.That(cDeltas.Average(), Is.LessThan(0.05));
    }
    
    private static bool IsSparseChroma(Munsell.Bounds bounds)
    {
        var chromaRanges = new[]
        {
            Munsell.Bounds.GetChromaRange(bounds.LowerH, bounds.LowerV),
            Munsell.Bounds.GetChromaRange(bounds.UpperH, bounds.LowerV),
            Munsell.Bounds.GetChromaRange(bounds.LowerH, bounds.UpperV),
            Munsell.Bounds.GetChromaRange(bounds.UpperH, bounds.UpperV)
        };

        return chromaRanges.Any(x => x == (0, 0));
    }

    private static double ChromaLimitScale(Munsell.Bounds bounds)
    {
        var chromaRanges = new[]
        {
            Munsell.Bounds.GetChromaRange(bounds.LowerH, bounds.LowerV),
            Munsell.Bounds.GetChromaRange(bounds.UpperH, bounds.LowerV),
            Munsell.Bounds.GetChromaRange(bounds.LowerH, bounds.UpperV),
            Munsell.Bounds.GetChromaRange(bounds.UpperH, bounds.UpperV)
        };
        
        var chromaLimit = chromaRanges.Select(x => x.max).Min();
        return chromaLimit == 0.0 ? 0.0 : bounds.UpperC / chromaLimit;
    }
}