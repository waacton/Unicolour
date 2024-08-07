using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripJzazbzTests
{
    private const double Tolerance = 0.00000005;
    
    // cannot test roundtrip via XYZ as Jzazbz <-> XYZ is not 1:1, e.g.
    // - when Jzazbz inputs produces negative XYZ values, which are clamped during XYZ -> Jzazbz
    // - when Jzazbz negative inputs trigger a negative number to a fractional power, producing NaNs
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.JzazbzTriplets))]
    public void ViaJzczhz(ColourTriplet triplet)
    {
        var original = new Jzazbz(triplet.First, triplet.Second, triplet.Third);
        var jzczhz = Jzczhz.FromJzazbz(original);
        var roundtrip = Jzczhz.ToJzazbz(jzczhz);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}