namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripRgb255Tests
{
    private const double Tolerance = 0.00000005;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaRgbLinear(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var roundtrip = Rgb.FromRgbLinear(Rgb.ToRgbLinear(original, RgbConfig), RgbConfig);
        AssertRoundtrip(triplet, original, roundtrip);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaHsb(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var roundtrip = Hsb.ToRgb(Hsb.FromRgb(original));
        AssertRoundtrip(triplet, original, roundtrip);
    }

    private static void AssertRoundtrip(ColourTriplet triplet, Rgb original, Rgb roundtrip)
    {
        TestUtils.AssertTriplet(original.Byte255.Triplet, triplet, Tolerance);
        TestUtils.AssertTriplet(roundtrip.Byte255.Triplet, triplet, Tolerance);
        TestUtils.AssertTriplet(roundtrip.Byte255.Triplet, original.Byte255.Triplet, Tolerance);
    }
}