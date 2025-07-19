using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripMunsellTests
{
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.MunsellTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        try
        {
            var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
            var xyy = MunsellFuncs.ToXyy(original);
            var roundtrip = MunsellFuncs.FromXyy(xyy);

            (double h, double c) tolerance;
            if (original.C < 0.5)
            {
                tolerance = (h: 0.375, c: 0.002);
            }
            else
            {
                /*
                 * "max chroma scale" is a measure of how far outside the munsell dataset the chroma is
                 * e.g. for 10Y 4/ the max measured chroma is 12, so 10Y 4/24 returns max chroma scale of 2x
                 * from running 100,000s of conversions, large deltas during roundtrip conversion correlates pretty well with this
                 * although there are rare outliers where large deltas appear at lower scales (but never <= 1, i.e. within known data points)
                 * these tolerances are based on gathering this data
                 * ----------
                 * NOTE:
                 * despite these outliers, the tolerances are remaining like this, and this test can be expected to fail occasionally
                 * so that they can be reviewed to determine if they are indeed outliers, or actually an issue in conversion
                 * once confident that conversion is robust, and occasional errors are outliers
                 * it is likely the tolerances for simplicity will become 1) very small for scale <= 1 and 2) very large for >= 1  
                 */
                var maxChromaScale = Math.Max(original.GetBounds().MaxChromaScale, roundtrip.GetBounds().MaxChromaScale);
                tolerance = maxChromaScale switch
                {
                    <= 1 => (h: 0.175, c: 0.005),
                    <= 2 => (h: 4, c: 6.5),
                    <= 3 => (h: 8, c: 17.5),
                    _ => (h: 15, c: 23.5)
                };
            }
            
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [tolerance.h, 5e-15, tolerance.c]);
        }
        catch (NotImplementedException e)
        {
            // TODO: implement the edge case that triggers this (no chroma data), typically due to low values
            //       expect it will need its own set of tolerances, similar to C < 0.5, perhaps V < 1
            Assert.Ignore(e.Message);
        }
    }

    // [Test]
    // public void Data()
    // {
    //     double[] cThresholds = Enumerable.Range(0, 100).Select(x => x  / 10.0).ToArray();
    //     var hLut = cThresholds.ToDictionary(c => c, _ => -1.0);
    //     var cLut = cThresholds.ToDictionary(c => c, _ => -1.0);
    //     var hWorstLowC = 0.0;
    //     var cWorstLowC = 0.0;
    //     var triplets = Enumerable.Range(0, 100000).Select(_ => RandomColours.Munsell()).ToArray();
    //
    //     foreach (var triplet in triplets)
    //     {
    //         try
    //         {
    //             var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
    //             var xyy = MunsellFuncs.ToXyy(original);
    //             var roundtrip = MunsellFuncs.FromXyy(xyy);
    //             
    //             var maxChromaScale = Math.Max(original.GetBounds().MaxChromaScale, roundtrip.GetBounds().MaxChromaScale);
    //             var threshold = cThresholds.Last(c => c <= maxChromaScale);
    //             var (originalHue, roundtripHue) = Hue.Unwrap(original.Hue.Degrees, roundtrip.Hue.Degrees, HueSpan.Shorter);
    //             var hDelta = Math.Abs(originalHue - roundtripHue);
    //             var cDelta = Math.Abs(original.C - roundtrip.C);
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
    //     Console.WriteLine("Low C : " + hWorstLowC);
    //     foreach (var item in hLut)
    //     {
    //         Console.WriteLine($"{item.Key} : {item.Value}");
    //     }
    //     
    //     Console.WriteLine("");
    //     
    //     Console.WriteLine("C deltas");
    //     Console.WriteLine("Low C : " + cWorstLowC);
    //     foreach (var item in cLut)
    //     {
    //         Console.WriteLine($"{item.Key} : {item.Value}");
    //     }
    // }
}