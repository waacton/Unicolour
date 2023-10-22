namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripLuvTests
{
    private const double Tolerance = 0.00000001;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LuvTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Luv(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Luv.FromXyz(Luv.ToXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LuvTriplets))]
    public void ViaLchuv(ColourTriplet triplet)
    {
        var original = new Luv(triplet.First, triplet.Second, triplet.Third);
        var viaLchuv = Lchuv.ToLuv(Lchuv.FromLuv(original));
        AssertUtils.AssertTriplet(viaLchuv.Triplet, original.Triplet, Tolerance);
    }
}