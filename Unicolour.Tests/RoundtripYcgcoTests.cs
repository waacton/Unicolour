namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripYcgcoTests
{
    private const double Tolerance = 0.0000000005;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YcgcoTriplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new Ycgco(triplet.First, triplet.Second, triplet.Third);
        var rgb = Ycgco.ToRgb(original);
        var roundtrip = Ycgco.FromRgb(rgb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}