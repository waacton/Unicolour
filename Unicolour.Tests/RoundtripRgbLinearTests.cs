namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripRgbLinearTests
{
    private const double Tolerance = 0.00000005;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbLinearTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new RgbLinear(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = RgbLinear.FromXyz(RgbLinear.ToXyz(original, RgbConfig, XyzConfig), RgbConfig, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbLinearTriplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new RgbLinear(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Rgb.ToRgbLinear(Rgb.FromRgbLinear(original, RgbConfig), RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}