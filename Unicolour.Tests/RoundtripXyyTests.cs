using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripXyyTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var xyz = Xyy.ToXyz(original);
        var roundtrip = Xyy.FromXyz(xyz, XyzConfig.WhiteChromaticity);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaWxy(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var wxy = Wxy.FromXyy(original, XyzConfig);
        var roundtrip = Wxy.ToXyy(wxy, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaMunsell(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var munsell = MunsellFuncs.FromXyy(original);
        var roundtrip = MunsellFuncs.ToXyy(munsell);
        
        double tolerance;
        
        if (munsell.Bounds.IsSparseChroma)
        {
            Console.WriteLine("⚠️ sparse chroma data");
            tolerance = 0.15;
        }
        else if (munsell.C < 0.5)
        {
            Console.WriteLine("⚠️ low chroma");
            tolerance = 0.00001;
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
             * TODO: lower the thresholds, find some of the extreme edge cases, and investigate in case of true conversion issues
             */
            Console.WriteLine($"{(munsell.Bounds.ChromaLimitScale > 1 ? "⚠️" : string.Empty)} {munsell.Bounds.ChromaLimitScale}x above max chroma");
            tolerance = munsell.Bounds.ChromaLimitScale switch
            {
                >= 5 => 0.0375,
                >= 1.75 => 0.025,
                >= 0.75 => 0.02,
                >= 0.5 => 0.00155,
                _ => 0.00001
            };
        }
        
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [tolerance, tolerance, 5e-15]);
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
        var triplets = RandomColours.XyyTriplets;

        var xDeltas = new List<double>();
        var yDeltas = new List<double>();
        var luminanceDeltas = new List<double>();
        
        foreach (var triplet in triplets)
        {
            var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
            var munsell = MunsellFuncs.FromXyy(original);
            var roundtrip = MunsellFuncs.ToXyy(munsell);

            xDeltas.Add(Math.Abs(original.Chromaticity.X - roundtrip.Chromaticity.X));
            yDeltas.Add(Math.Abs(original.Chromaticity.Y - roundtrip.Chromaticity.Y));
            luminanceDeltas.Add(Math.Abs(original.Luminance - roundtrip.Luminance));
        }
        
        Assert.That(xDeltas.Average(), Is.LessThan(0.00025));
        Assert.That(yDeltas.Average(), Is.LessThan(0.00025));
        Assert.That(luminanceDeltas.Average(), Is.LessThan(5e-15));
    }
    
    // [Test]
    // public void Data()
    // {
    //     var cThresholds = Enumerable.Range(0, 41).Select(x => x  / 4.0).ToArray();
    //     var lut = cThresholds.ToDictionary(c => c, _ => -1.0);
    //     var worstLowC = 0.0;
    //     var worstSparseC = 0.0;
    //     var triplets = Enumerable.Range(0, 100000).Select(_ => RandomColours.Xyy()).ToArray();
    //
    //     foreach (var triplet in triplets)
    //     {
    //         var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
    //         Munsell munsell;
    //         Xyy roundtrip;
    //         
    //         try
    //         {
    //             munsell = MunsellFuncs.FromXyy(original);
    //             roundtrip = MunsellFuncs.ToXyy(munsell);
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine($"Exception occurred processing {original} ({original.Triplet})");
    //             continue;
    //         }
    //
    //
    //         var chromaLimitScale = munsell.Bounds.ChromaLimitScale;
    //         var threshold = cThresholds.Last(c => c <= chromaLimitScale);
    //         var delta = Math.Max(Math.Abs(original.Chromaticity.X - roundtrip.Chromaticity.X), Math.Abs(original.Chromaticity.Y - roundtrip.Chromaticity.Y));
    //
    //         if (munsell.Bounds.IsSparseChroma)
    //         {
    //             if (delta > worstSparseC)
    //             {
    //                 worstSparseC = delta;
    //             }
    //             
    //             continue;
    //         }
    //         
    //         if (munsell.C < 0.5)
    //         {
    //             if (delta > worstLowC)
    //             {
    //                 worstLowC = delta;
    //             }
    //             
    //             continue;
    //         }
    //
    //         if (delta > lut[threshold])
    //         {
    //             lut[threshold] = delta;
    //         }
    //     }
    //
    //     Console.WriteLine("XY deltas");
    //     Console.WriteLine("=========");
    //     Console.WriteLine("Sparse C : " + worstSparseC);
    //     Console.WriteLine("Low C : " + worstLowC);
    //     foreach (var item in lut)
    //     {
    //         Console.WriteLine($"{item.Key} : {item.Value}");
    //     }
    // }
}