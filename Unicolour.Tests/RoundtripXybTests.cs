namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripXybTests
{
    private const double Tolerance = 0.0000000005;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XybTriplets))]
    public void ViaRgbLinear(ColourTriplet triplet)
    {
        var original = new Xyb(triplet.First, triplet.Second, triplet.Third);
        var rgbLinear = Xyb.ToRgbLinear(original);
        var roundtrip = Xyb.FromRgbLinear(rgbLinear);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}