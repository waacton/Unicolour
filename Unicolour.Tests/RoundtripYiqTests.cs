namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripYiqTests
{
    private const double Tolerance = 0.0000000005;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YiqTriplets))]
    public void ViaYuv(ColourTriplet triplet)
    {
        var original = new Yiq(triplet.First, triplet.Second, triplet.Third);
        var yuv = Yiq.ToYuv(original);
        var roundtrip = Yiq.FromYuv(yuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}