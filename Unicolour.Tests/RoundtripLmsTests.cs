using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripLmsTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LmsTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Lms(triplet.First, triplet.Second, triplet.Third);
        var xyz = Lms.ToXyz(original, XyzConfig);
        var roundtrip = Lms.FromXyz(xyz, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}