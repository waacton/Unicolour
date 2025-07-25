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

        var tolerance = munsell.XyyToMunsellSearchResult!.Converged ? 0.00001 : 0.15;
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [tolerance, tolerance, 5e-15]);
    }

    // [Test]
    // public void X()
    // {
    //     while (true)
    //     {
    //         // var original = new Xyy(0.3920859907774519, 0.4766220631732515, 0.006886734146503759);
    //         var original = new Xyy(TestUtils.RandomDouble(), TestUtils.RandomDouble(), TestUtils.RandomDouble());
    //         Munsell munsell = null!;
    //         Xyy roundtrip = null!;
    //
    //         try
    //         {
    //             munsell = MunsellFuncs.FromXyy(original);
    //             roundtrip = MunsellFuncs.ToXyy(munsell);
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine(original.Triplet);
    //             Console.WriteLine(munsell?.Triplet?.ToString() ?? "no munsell");
    //             Console.WriteLine(roundtrip?.Triplet?.ToString() ?? "no xyy roundtrip");
    //             Console.WriteLine(e);
    //             throw;
    //         }
    //     }
    // }
    
    // [Test]
    // public void Data()
    // {
    //     var cThresholds = Enumerable.Range(0, 41).Select(x => x  / 4.0).ToArray();
    //     var lut = cThresholds.ToDictionary(c => c, _ => -1.0);
    //     var worstNoConverge = 0.0;
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
    //         if (!munsell.XyyToMunsellSearchResult.Converged)
    //         {
    //             if (delta > worstNoConverge)
    //             {
    //                 worstNoConverge = delta;
    //             }
    //             
    //             continue;
    //         }
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
    //     Console.WriteLine("No converge : " + worstNoConverge);
    //     Console.WriteLine("Sparse C : " + worstSparseC);
    //     Console.WriteLine("Low C : " + worstLowC);
    //     foreach (var item in lut)
    //     {
    //         Console.WriteLine($"{item.Key} : {item.Value}");
    //     }
    // }
}