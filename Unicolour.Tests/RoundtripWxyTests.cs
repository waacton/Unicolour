using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripWxyTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.WxyTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        var original = new Wxy(triplet.First, triplet.Second, triplet.Third);
        var xyy = Wxy.ToXyy(original, XyzConfig);
        var roundtrip = Wxy.FromXyy(xyy, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}