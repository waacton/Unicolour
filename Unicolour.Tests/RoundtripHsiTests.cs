namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripHsiTests
{
    private const double Tolerance = 0.000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsiTriplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new Hsi(triplet.First, triplet.Second, triplet.Third);
        var rgb = Hsi.ToRgb(original);
        var roundtrip = Hsi.FromRgb(rgb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}