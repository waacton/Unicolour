namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripHsluvTests
{
    private const double Tolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsluvTriplets))]
    public void ViaLchuv(ColourTriplet triplet)
    {
        var original = new Hsluv(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Hsluv.FromLchuv(Hsluv.ToLchuv(original));
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}