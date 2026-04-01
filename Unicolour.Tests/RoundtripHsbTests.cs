using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHsbTests
{
    private const double Tolerance = 0.000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Hsb, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaRgb(ColourTriplet triplet) => AssertViaRgb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaRgbFromNamed(TestColour namedColour) => AssertViaRgb(namedColour.Hsb!);
    
    private static void AssertViaRgb(ColourTriplet triplet)
    {
        var original = new Hsb(triplet.First, triplet.Second, triplet.Third);
        var rgb = Hsb.ToRgb(original);
        var roundtrip = Hsb.FromRgb(rgb);

        var negativeChroma = original.S < 0 ^ original.B < 0;
        if (negativeChroma)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(0, 0, original.B, HueIndex: 0), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaHsl(ColourTriplet triplet) => AssertViaHsl(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHslFromNamed(TestColour namedColour) => AssertViaHsl(namedColour.Hsb!);
    
    private static void AssertViaHsl(ColourTriplet triplet)
    {
        var original = new Hsb(triplet.First, triplet.Second, triplet.Third);
        var hsl = Hsl.FromHsb(original);
        var roundtrip = Hsl.ToHsb(hsl);
        
        if (original.B == 0.0)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.H, 0, original.B, HueIndex: 0), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaHwb(ColourTriplet triplet) => AssertViaHwb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHwbFromNamed(TestColour namedColour) => AssertViaHwb(namedColour.Hsb!);
    
    private static void AssertViaHwb(ColourTriplet triplet)
    {
        var original = new Hsb(triplet.First, triplet.Second, triplet.Third);
        var hwb = Hwb.FromHsb(original);
        var roundtrip = Hwb.ToHsb(hwb);

        var greyness = hwb.W + hwb.B;
        if (greyness > 1.0)
        {
            var greyLevel = hwb.W / greyness;
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.H, 0, greyLevel, HueIndex: 0), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}