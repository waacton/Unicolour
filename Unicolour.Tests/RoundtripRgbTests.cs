namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripRgbTests
{
    private const double Tolerance = 0.00000005;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaHsb(ColourTriplet triplet) => AssertViaHsb(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaHsb255(ColourTriplet triplet) => AssertViaHsb(GetNormalisedRgb255Triplet(triplet));
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHsbNamed(TestColour namedColour) => AssertViaHsb(GetRgbTripletFromHex(namedColour.Hex!));
    
    private static void AssertViaHsb(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third, RgbConfig);
        var roundtrip = Hsb.ToRgb(Hsb.FromRgb(original), RgbConfig);
        AssertRoundtrip(original, roundtrip);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaXyz(ColourTriplet triplet) => AssertViaXyz(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaXyz255(ColourTriplet triplet) => AssertViaXyz(GetNormalisedRgb255Triplet(triplet));
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaXyzNamed(TestColour namedColour) => AssertViaXyz(GetRgbTripletFromHex(namedColour.Hex!));
    
    private static void AssertViaXyz(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third, RgbConfig);
        var roundtrip = Rgb.FromXyz(Rgb.ToXyz(original, RgbConfig, XyzConfig), RgbConfig, XyzConfig);
        AssertRoundtrip(original, roundtrip);
    }

    private static void AssertRoundtrip(Rgb original, Rgb roundtrip)
    {
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        AssertUtils.AssertTriplet(roundtrip.ConstrainedTriplet, original.ConstrainedTriplet, Tolerance);
        AssertUtils.AssertTriplet(roundtrip.Linear.Triplet, original.Linear.Triplet, Tolerance);
        AssertUtils.AssertTriplet(roundtrip.Linear.ConstrainedTriplet, original.Linear.ConstrainedTriplet, Tolerance);
        AssertUtils.AssertTriplet(roundtrip.Byte255.Triplet, original.Byte255.Triplet, Tolerance);
        AssertUtils.AssertTriplet(roundtrip.Byte255.ConstrainedTriplet, original.Byte255.ConstrainedTriplet, Tolerance);
    }
    
    private static ColourTriplet GetRgbTripletFromHex(string hex)
    {
        var (r255, g255, b255, _) = Wacton.Unicolour.Utils.ParseColourHex(hex);
        return new(r255 / 255.0, g255 / 255.0, b255 / 255.0);
    }
    
    private static ColourTriplet GetNormalisedRgb255Triplet(ColourTriplet triplet)
    {
        var (r255, g255, b255) = triplet;
        return new(r255 / 255.0, g255 / 255.0, b255 / 255.0);
    }
}