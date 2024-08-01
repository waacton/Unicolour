using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHsbTests
{
    private const double Tolerance = 0.000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void ViaRgb(ColourTriplet triplet) => AssertViaRgb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaRgbFromNamed(TestColour namedColour) => AssertViaRgb(namedColour.Hsb!);
    
    private static void AssertViaRgb(ColourTriplet triplet)
    {
        var original = new Hsb(triplet.First, triplet.Second, triplet.Third);
        var rgb = Hsb.ToRgb(original);
        var roundtrip = Hsb.FromRgb(rgb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void ViaHsl(ColourTriplet triplet) => AssertViaHsl(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHslFromNamed(TestColour namedColour) => AssertViaHsl(namedColour.Hsb!);
    
    private static void AssertViaHsl(ColourTriplet triplet)
    {
        var original = new Hsb(triplet.First, triplet.Second, triplet.Third);
        var hsl = Hsl.FromHsb(original);
        var roundtrip = Hsl.ToHsb(hsl);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void ViaHwb(ColourTriplet triplet) => AssertViaHwb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHwbFromNamed(TestColour namedColour) => AssertViaHwb(namedColour.Hsb!);
    
    private static void AssertViaHwb(ColourTriplet triplet)
    {
        var original = new Hsb(triplet.First, triplet.Second, triplet.Third);
        var hwb = Hwb.FromHsb(original);
        var roundtrip = Hwb.ToHsb(hwb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}