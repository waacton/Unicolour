using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripMunsellTests
{
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.MunsellTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
        var xyy = MunsellFuncs.ToXyy(original);
        var roundtrip = MunsellFuncs.FromXyy(xyy);

        (double h, double c) tolerance;
        if (!roundtrip.XyyToMunsellSearchResult!.Converged)
        {
            Console.WriteLine($"⚠️ did not converge ({roundtrip})");
            tolerance = (h: 7.5, c: 53.5);
        }
        else if (xyy.Chromaticity.X is < 0 or > 1 || xyy.Chromaticity.Y is < 0 or > 1)
        {
            Console.WriteLine($"⚠️ xy outside reasonable range ({xyy})");
            tolerance = (h: 21.5, c: 62.5);
        }
        else if (original.Bounds.IsSparseChroma || roundtrip.Bounds.IsSparseChroma)
        {
            Console.WriteLine($"⚠️ sparse chroma data ({string.Join(", ", roundtrip.Bounds.ChromaRanges)})");
            tolerance = (h: 8.75, c: 21.5);
        }
        else if (original.C < 0.5)
        {
            Console.WriteLine($"⚠️ low chroma ({original.C})");
            tolerance = (h: 4, c: 0.0035);
        }
        else
        {
            /*
             * "chroma limit scale" is a measure of how far outside the munsell dataset the chroma is
             * e.g. for 10Y 4/ the max measured chroma is 12, so 10Y 4/24 returns max chroma scale of 2x
             * from running 10,000,000 roundtrips conversions, large deltas during roundtrip conversion correlates pretty well with this
             * although there are rare outliers where large deltas appear at lower scales (but never <= 1, i.e. within known data points)
             * these tolerances are based on gathering this data
             * ----------
             * NOTE:
             * for now the tolerances will not include the outliers, and can be expected to fail occasionally
             * so that they can be reviewed to determine if they are indeed outliers, or actually an issue in conversion
             * once confident that conversion is robust, and occasional errors are outliers
             * it is likely the tolerances for simplicity will become 1) very small for scale <= 1 and 2) very large for >= 1
             */
            var maxChromaScale = Math.Max(original.Bounds.ChromaLimitScale, roundtrip.Bounds.ChromaLimitScale);
            Console.WriteLine($"{(original.Bounds.ChromaLimitScale > 1 ? "⚠️ " : string.Empty)}{original.Bounds.ChromaLimitScale}x above max chroma");
            Console.WriteLine($"{(roundtrip.Bounds.ChromaLimitScale > 1 ? "⚠️ " : string.Empty)}{roundtrip.Bounds.ChromaLimitScale}x above max chroma");
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
            var xyy = MunsellFuncs.ToXyy(original);
            var roundtrip = MunsellFuncs.FromXyy(xyy);

            var hues = Hue.Unwrap(original.Hue.Degrees, roundtrip.Hue.Degrees);
            hDeltas.Add(Math.Abs(hues.start - hues.end));
            vDeltas.Add(Math.Abs(original.V - roundtrip.V));
            cDeltas.Add(Math.Abs(original.C - roundtrip.C));
        }
        
        Assert.That(hDeltas.Average(), Is.LessThan(0.05));
        Assert.That(vDeltas.Average(), Is.LessThan(5e-15));
        Assert.That(cDeltas.Average(), Is.LessThan(0.05));
    }

    // [Test]
    // public void Data()
    // {
    //     var cThresholds = Enumerable.Range(0, 41).Select(x => x  / 4.0).ToArray();
    //     var hLut = cThresholds.ToDictionary(c => c, _ => -1.0);
    //     var cLut = cThresholds.ToDictionary(c => c, _ => -1.0);
    //     var hWorstLowC = 0.0;
    //     var cWorstLowC = 0.0;
    //     var hWorstSparseC = 0.0;
    //     var cWorstSparseC = 0.0;
    //     var hWorstBadXy = 0.0;
    //     var cWorstBadXy = 0.0;
    //     var hNoConverge = 0.0;
    //     var cNoConverge = 0.0;
    //     var triplets = Enumerable.Range(0, 10_000_000).Select(_ => RandomColours.Munsell()).ToArray();
    //
    //     foreach (var triplet in triplets)
    //     {
    //         try
    //         {
    //             var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
    //             Xyy xyy;
    //             Munsell roundtrip;
    //             
    //             try
    //             {
    //                 xyy = MunsellFuncs.ToXyy(original);
    //                 roundtrip = MunsellFuncs.FromXyy(xyy);
    //             }
    //             catch (Exception e)
    //             {
    //                 Console.WriteLine($"Exception occurred processing {original} ({original.Triplet})");
    //                 continue;
    //             }
    //             
    //             var chromaLimitScale = Math.Max(original.Bounds.ChromaLimitScale, roundtrip.Bounds.ChromaLimitScale);
    //             var threshold = cThresholds.Last(c => c <= chromaLimitScale);
    //             var (originalHue, roundtripHue) = Hue.Unwrap(original.Hue.Degrees, roundtrip.Hue.Degrees);
    //             var hDelta = Math.Abs(originalHue - roundtripHue);
    //             var cDelta = Math.Abs(original.C - roundtrip.C);
    //
    //             if (!roundtrip.XyyToMunsellSearchResult.Converged)
    //             {
    //                 if (hDelta > hNoConverge)
    //                 {
    //                     hNoConverge = hDelta;
    //                 }
    //                 
    //                 if (cDelta > cNoConverge)
    //                 {
    //                     cNoConverge = cDelta;
    //                 }
    //                 
    //                 continue;
    //             }
    //
    //             if (xyy.Chromaticity.X is < 0 or > 1 || xyy.Chromaticity.Y is < 0 or > 1)
    //             {
    //                 if (hDelta > hWorstBadXy)
    //                 {
    //                     hWorstBadXy = hDelta;
    //                 }
    //                 
    //                 if (cDelta > cWorstBadXy)
    //                 {
    //                     cWorstBadXy = cDelta;
    //                 }
    //                 
    //                 continue;
    //             }
    //
    //             if (original.Bounds.IsSparseChroma || roundtrip.Bounds.IsSparseChroma)
    //             {
    //                 if (hDelta > hWorstSparseC)
    //                 {
    //                     hWorstSparseC = hDelta;
    //                 }
    //                 
    //                 if (cDelta > cWorstSparseC)
    //                 {
    //                     cWorstSparseC = cDelta;
    //                 }
    //                 
    //                 continue;
    //             }
    //             
    //             if (original.C < 0.5)
    //             {
    //                 if (hDelta > hWorstLowC)
    //                 {
    //                     hWorstLowC = hDelta;
    //                 }
    //                 
    //                 if (cDelta > cWorstLowC)
    //                 {
    //                     cWorstLowC = cDelta;
    //                 }
    //                 
    //                 continue;
    //             }
    //
    //             if (hDelta > hLut[threshold])
    //             {
    //                 hLut[threshold] = hDelta;
    //             }
    //         
    //             if (cDelta > cLut[threshold])
    //             {
    //                 cLut[threshold] = cDelta;
    //             }
    //         }
    //         catch (NotImplementedException e)
    //         {
    //             // Console.WriteLine(e.Message);
    //         }
    //     }
    //
    //     Console.WriteLine("H deltas");
    //     Console.WriteLine("=========");
    //     Console.WriteLine("No converge : " + hNoConverge);
    //     Console.WriteLine("Bad XY : " + hWorstBadXy);
    //     Console.WriteLine("Sparse C : " + hWorstSparseC);
    //     Console.WriteLine("Low C : " + hWorstLowC);
    //     foreach (var item in hLut)
    //     {
    //         Console.WriteLine($"{item.Key} : {item.Value}");
    //     }
    //     
    //     Console.WriteLine("");
    //     
    //     Console.WriteLine("C deltas");
    //     Console.WriteLine("=========");
    //     Console.WriteLine("No converge : " + cNoConverge);
    //     Console.WriteLine("Bad XY : " + cWorstBadXy);
    //     Console.WriteLine("Sparse C : " + cWorstSparseC);
    //     Console.WriteLine("Low C : " + cWorstLowC);
    //     foreach (var item in cLut)
    //     {
    //         Console.WriteLine($"{item.Key} : {item.Value}");
    //     }
    // }
}