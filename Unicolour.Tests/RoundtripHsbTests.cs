namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

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
        var roundtrip = Hsb.FromRgb(Hsb.ToRgb(original));
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void ViaHsl(ColourTriplet triplet) => AssertViaHsl(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHslFromNamed(TestColour namedColour) => AssertViaHsl(namedColour.Hsb!);
    
    private static void AssertViaHsl(ColourTriplet triplet)
    {
        var original = new Hsb(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Hsl.ToHsb(Hsl.FromHsb(original));
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void ViaHwb(ColourTriplet triplet) => AssertViaHwb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHwbFromNamed(TestColour namedColour) => AssertViaHwb(namedColour.Hsb!);
    
    private static void AssertViaHwb(ColourTriplet triplet)
    {
        var original = new Hsb(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Hwb.ToHsb(Hwb.FromHsb(original));
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}