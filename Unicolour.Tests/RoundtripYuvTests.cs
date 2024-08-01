using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripYuvTests
{
    private const double Tolerance = 0.0000000005;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YuvTriplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new Yuv(triplet.First, triplet.Second, triplet.Third);
        var rgb = Yuv.ToRgb(original);
        var roundtrip = Yuv.FromRgb(rgb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YuvTriplets))]
    public void ViaYiq(ColourTriplet triplet)
    {
        var original = new Yuv(triplet.First, triplet.Second, triplet.Third);
        var yiq = Yiq.FromYuv(original);
        var roundtrip = Yiq.ToYuv(yiq);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YuvTriplets))]
    public void ViaYdbdr(ColourTriplet triplet)
    {
        var original = new Yuv(triplet.First, triplet.Second, triplet.Third);
        var ydbdr = Ydbdr.FromYuv(original);
        var roundtrip = Ydbdr.ToYuv(ydbdr);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}