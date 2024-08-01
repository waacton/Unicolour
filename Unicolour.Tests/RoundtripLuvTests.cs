using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripLuvTests
{
    private const double Tolerance = 0.00000001;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LuvTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Luv(triplet.First, triplet.Second, triplet.Third);
        var xyz = Luv.ToXyz(original, XyzConfig);
        var roundtrip = Luv.FromXyz(xyz, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LuvTriplets))]
    public void ViaLchuv(ColourTriplet triplet)
    {
        var original = new Luv(triplet.First, triplet.Second, triplet.Third);
        var lchuv = Lchuv.FromLuv(original);
        var roundtrip = Lchuv.ToLuv(lchuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}