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

            if (original.C < 0.5)
            {
                TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [0.75, 5e-15, 0.5]);
                return;
            }

            var originalBounds = original.GetBounds();
            var roundtripBounds = roundtrip.GetBounds();
            Console.WriteLine($"original ... C {original.C:F2} is above max chroma by {originalBounds.MaxChromaScale}x ({string.Join(", ", originalBounds.UpperChromaLimits)})");
            Console.WriteLine($"roundtrip .. C {roundtrip.C:F2} is above max chroma by {roundtripBounds.MaxChromaScale}x ({string.Join(", ", roundtripBounds.UpperChromaLimits)})");
            if (originalBounds.MaxChromaScale > 2.5 || roundtripBounds.MaxChromaScale > 2.5)
            {
                // roundtrip is almost never this inaccurate even when chroma is not within range
                // but certain rare values deviate a lot, and these coincide with chromas well outside the available data
                TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [7.5, 5e-15, 15]);
                return;
            }
                
            if (originalBounds.MaxChromaScale > 1.5 || roundtripBounds.MaxChromaScale > 1.5)
            {
                // roundtrip is almost never this inaccurate even when chroma is not within range
                // but certain rare values deviate a lot, and these coincide with chromas well outside the available data
                TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [1.25, 5e-15, 7]);
                return;
            }
            
            if (originalBounds.MaxChromaScale > 1 || roundtripBounds.MaxChromaScale > 1)
            {
                // roundtrip is almost never this inaccurate even when chroma is not within range
                // but certain rare values deviate a lot, and these coincide with chromas well outside the available data
                TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [1.25, 5e-15, 1.25]);
                return;
            }
            
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [0.1, 5e-15, 0.1]);
            
            //
            // var bounds = MunsellFuncs.GetBounds(original);
            // Console.WriteLine($"original  {original} average chroma above limits = {bounds.AverageChromaBeyondLimit()}");
            //
            // Console.WriteLine(xyy);
            //
            // foreach (var iteration in roundtrip.XyyToMunsellSearchResult.Iterations)
            // {
            //     Console.WriteLine($"iteration {iteration.Munsell} average chroma above limits = {iteration.Bounds.AverageChromaBeyondLimit()}");
            // }
            //
            // Console.WriteLine(roundtrip.XyyToMunsellSearchResult);
            // if (!roundtrip.XyyToMunsellSearchResult!.Converged)
            // {
            //     Assert.Ignore("Did not converge; TODO: assert with higher tolerance");
            //     return;
            // }
            //
            // TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [0.1, 5e-15, 0.1]);
        }
        catch (NotImplementedException e)
        {
            Assert.Ignore(e.Message);
        }
    }
    
    // [TestCaseSource(typeof(RandomColours), nameof(RandomColours.MunsellTriplets))]
    // public void ViaXyy(ColourTriplet triplet)
    // {
    //     try
    //     {
    //         var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
    //         var xyy = MunsellFuncs.ToXyy(original);
    //         var roundtrip = Munsell.FromXyy(xyy);
    //         TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [5, 5e-15, 3]);
    //     }
    //     catch (InvalidOperationException e)
    //     {
    //         Assert.Ignore(e.Message);
    //     }
    //     catch (NotImplementedException e)
    //     {
    //         Assert.Ignore(e.Message);
    //     }
    // }
    //
    // // [Test] // reassurance that roundtrips via munsell are typically reasonably accurate, even if certain data points are not
    // public void ViaXyyAverage()
    // {
    //     var deltas = RandomColours.MunsellTriplets.Select(GetRoundtripDelta).ToArray();
    //
    //     // TODO: filter by data that was within dataset vs without
    //     //       expect to require higher tolerance as data outwith dataset are a less accurate approximation
    //     var insideDatasetDeltas = deltas.Where(delta => !double.IsNaN(delta.h) && !double.IsNaN(delta.c)).ToArray();
    //     Assert.That(insideDatasetDeltas.Average(delta => delta.h), Is.LessThan(0.25)); // degrees (0 - 360)
    //     Assert.That(insideDatasetDeltas.Average(delta => delta.c), Is.LessThan(0.1)); // chroma (0 - 50)
    // }
    //
    // private static (double h, double c) GetRoundtripDelta(ColourTriplet triplet)
    // {
    //     try
    //     {
    //         var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
    //         var xyy = MunsellFuncs.ToXyy(original);
    //         var roundtrip = Munsell.FromXyy(xyy);
    //
    //         var (originalHue, roundtripHue) = Hue.Wrap(original.Hue.Degrees, roundtrip.Hue.Degrees, HueSpan.Shorter);
    //             
    //         // conversion between V and Y is accurate
    //         // including it here would artificially increase reduce the error being tested
    //         return (Math.Abs(originalHue - roundtripHue), Math.Abs(original.C - roundtrip.C));
    //     }
    //     catch (InvalidOperationException e)
    //     {
    //         // TODO: detect these cases (XY not within dataset; VC not within dataset)
    //         //       likely use a higher tolerance, they will be a less accurate approximation
    //         return (double.NaN, double.NaN);
    //     }
    // }
}