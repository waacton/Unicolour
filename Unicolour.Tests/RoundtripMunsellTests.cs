using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripMunsellTests
{
    private const double Tolerance = 0.1; // 0.0000000005; TODO:
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.MunsellTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
        var xyy = Munsell.ToXyy(original);
        var roundtrip = Munsell.FromXyy(xyy);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [Test]
    public void Single()
    {
        // var triplet = new ColourTriplet(135.11046227062917, 0.5145344724126621, 24.76778017938645);
        var triplet = new ColourTriplet(101.84421453470341, 5.566935582295958, 21.599939414511006);
        var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
        var xyy = Munsell.ToXyy(original);
        var roundtrip = Munsell.FromXyy(xyy);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}