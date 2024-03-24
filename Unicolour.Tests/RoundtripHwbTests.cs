namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripHwbTests
{
    private const double Tolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HwbTriplets))]
    public void ViaHsb(ColourTriplet triplet)
    {
        // note: cannot test round trip of all HWB values as HWB <-> HSB is not 1:1
        // since when HWB W + B > 100%, it is the same as another HWB where W + B = 100%
        // (e.g. W 100 B 50 == W 66.666 B 33.333)
        // and HSB -> HWB will always produce HWB that results in W + B <= 100%
        var original = new Hwb(triplet.First, triplet.Second, triplet.Third);
        var scale = original.ConstrainedW + original.ConstrainedB;
        var scaled = new Hwb(original.H, original.ConstrainedW / scale, original.ConstrainedB / scale);

        var needsScaling = scale > 1.0;
        if (needsScaling)
        {
            var hsbFromOriginal = Hwb.ToHsb(original);
            var hsbFromScaled = Hwb.ToHsb(scaled);
            TestUtils.AssertTriplet(hsbFromOriginal.Triplet, hsbFromScaled.Triplet, Tolerance);
        }

        var hsb = Hwb.ToHsb(original);
        var roundtrip = Hwb.FromHsb(hsb);
        var expected = needsScaling ? scaled.Triplet : original.Triplet;
        TestUtils.AssertTriplet(roundtrip.Triplet, expected, Tolerance);
    }
}