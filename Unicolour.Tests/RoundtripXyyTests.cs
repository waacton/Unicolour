using System;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
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
        try
        {
            var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
            var munsell = MunsellFuncs.FromXyy(original);
            var roundtrip = MunsellFuncs.ToXyy(munsell);
            
            double tolerance;
            
            if (munsell.Bounds.IsSparseChroma)
            {
                Assert.Ignore($"⚠️ sparse chroma data; chroma ranges for {munsell} are {string.Join(", ", munsell.Bounds.ChromaRanges)}");
                return;
            }
            
            if (munsell.C < 0.5)
            {
                Console.WriteLine("⚠️ low chroma");
                tolerance = 0.000001;
            }
            else
            {
                /*
                 * "chroma limit scale" is a measure of how far outside the munsell dataset the chroma is
                 * e.g. for 10Y 4/ the max measured chroma is 12, so 10Y 4/24 returns max chroma scale of 2x
                 * from running 100,000s of conversions, large deltas during roundtrip conversion correlates pretty well with this
                 * although there are rare outliers where large deltas appear at lower scales (but never <= 1, i.e. within known data points)
                 * these tolerances are based on gathering this data
                 * ----------
                 * NOTE:
                 * for now the tolerances will not include the outliers, and can be expected to fail occasionally
                 * so that they can be reviewed to determine if they are indeed outliers, or actually an issue in conversion
                 * once confident that conversion is robust, and occasional errors are outliers
                 * it is likely the tolerances for simplicity will become 1) very small for scale <= 1 and 2) very large for >= 1
                 */
                Console.WriteLine($"{(munsell.Bounds.ChromaLimitScale > 1 ? "⚠️" : string.Empty)} {munsell.Bounds.ChromaLimitScale}x above max chroma");
                tolerance = munsell.Bounds.ChromaLimitScale switch
                {
                    <= 1 => 0.00001,
                    <= 5 => 0.025,
                    _ => 0.035
                };
            }
            
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [tolerance, tolerance, 5e-15]);
        }
        catch (NotImplementedException e)
        {
            Assert.Ignore(e.Message);
        }
    }
    
    // [Test]
    // public void Data()
    // {
    //     double[] cThresholds = Enumerable.Range(0, 100).Select(x => x  / 10.0).ToArray();
    //     var lut = cThresholds.ToDictionary(c => c, _ => -1.0);
    //     var worstLowC = 0.0;
    //     var triplets = Enumerable.Range(0, 100000).Select(_ => RandomColours.Xyy()).ToArray();
    //
    //     foreach (var triplet in triplets)
    //     {
    //         try
    //         {
    //             var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
    //             var munsell = MunsellFuncs.FromXyy(original);
    //             var roundtrip = MunsellFuncs.ToXyy(munsell);
    //
    //             var maxChromaScale = munsell.Bounds.Value.MaxChromaScale;
    //             var threshold = cThresholds.Last(c => c <= maxChromaScale);
    //             var delta = Math.Max(Math.Abs(original.Chromaticity.X - roundtrip.Chromaticity.X), Math.Abs(original.Chromaticity.Y - roundtrip.Chromaticity.Y));
    //
    //             if (munsell.C < 0.5)
    //             {
    //                 if (delta > worstLowC)
    //                 {
    //                     worstLowC = delta;
    //                 }
    //                 
    //                 continue;
    //             }
    //
    //             if (delta > lut[threshold])
    //             {
    //                 lut[threshold] = delta;
    //             }
    //         }
    //         catch (NotImplementedException e)
    //         {
    //             // Console.WriteLine(e.Message);
    //         }
    //     }
    //
    //     Console.WriteLine("XY deltas");
    //     Console.WriteLine("Low C : " + worstLowC);
    //     foreach (var item in lut)
    //     {
    //         Console.WriteLine($"{item.Key} : {item.Value}");
    //     }
    // }
}