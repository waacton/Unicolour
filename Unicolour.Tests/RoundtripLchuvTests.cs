using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripLchuvTests
{
    private const double Tolerance = 0.00000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Lchuv, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaLuv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var luv = Lchuv.ToLuv(original);
        var roundtrip = Lchuv.FromLuv(luv);
        
        if (original.C < 0)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.L, 0, 0, HueIndex: 2), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaHsluv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var hsluv = Hsluv.FromLchuv(original);
        var roundtrip = Hsluv.ToLchuv(hsluv);
        
        if (original.L is > 99.9999999 or < 0.00000001)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.L, 0, original.H, HueIndex: 2), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaHpluv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var hpluv = Hpluv.FromLchuv(original);
        var roundtrip = Hpluv.ToLchuv(hpluv);
        
        if (original.L is > 99.9999999 or < 0.00000001)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.L, 0, original.H, HueIndex: 2), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}