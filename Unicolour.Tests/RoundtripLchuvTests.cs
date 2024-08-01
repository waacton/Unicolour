using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripLchuvTests
{
    private const double Tolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchuvTriplets))]
    public void ViaLuv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var luv = Lchuv.ToLuv(original);
        var roundtrip = Lchuv.FromLuv(luv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchuvTriplets))]
    public void ViaHsluv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var hsluv = Hsluv.FromLchuv(original);
        var roundtrip = Hsluv.ToLchuv(hsluv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchuvTriplets))]
    public void ViaHpluv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var hpluv = Hpluv.FromLchuv(original);
        var roundtrip = Hpluv.ToLchuv(hpluv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}