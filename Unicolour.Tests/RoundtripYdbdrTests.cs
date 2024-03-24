namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripYdbdrTests
{
    private const double Tolerance = 0.0000000005;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YdbdrTriplets))]
    public void ViaYuv(ColourTriplet triplet)
    {
        var original = new Ydbdr(triplet.First, triplet.Second, triplet.Third);
        var yuv = Ydbdr.ToYuv(original);
        var roundtrip = Ydbdr.FromYuv(yuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}