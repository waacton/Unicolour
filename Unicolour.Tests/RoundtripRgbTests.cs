namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripRgbTests
{
    private const double Tolerance = 0.00000005;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaRgbLinear(ColourTriplet triplet) => AssertViaRgbLinear(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaRgbLinearFromNamed(TestColour namedColour) => AssertViaRgbLinear(namedColour.Rgb!);
    
    private static void AssertViaRgbLinear(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Rgb.FromRgbLinear(Rgb.ToRgbLinear(original, RgbConfig), RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaHsb(ColourTriplet triplet) => AssertViaHsb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHsbFromNamed(TestColour namedColour) => AssertViaHsb(namedColour.Rgb!);
    
    private static void AssertViaHsb(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Hsb.ToRgb(Hsb.FromRgb(original));
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}