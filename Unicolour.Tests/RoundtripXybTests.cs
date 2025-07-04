using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripXybTests
{
    private const double Tolerance = 0.000000125;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XybTriplets))]
    public void ViaRgbLinear(ColourTriplet triplet)
    {
        var original = new Xyb(triplet.First, triplet.Second, triplet.Third);
        var rgbLinear = Xyb.ToRgbLinear(original);
        var roundtrip = Xyb.FromRgbLinear(rgbLinear);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}