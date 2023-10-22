namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripLchuvTests
{
    private const double Tolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchuvTriplets))]
    public void ViaLuv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Lchuv.FromLuv(Lchuv.ToLuv(original));
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchuvTriplets))]
    public void ViaHsluv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Hsluv.ToLchuv(Hsluv.FromLchuv(original));
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchuvTriplets))]
    public void ViaHpluv(ColourTriplet triplet)
    {
        var original = new Lchuv(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Hpluv.ToLchuv(Hpluv.FromLchuv(original));
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}