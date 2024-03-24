namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripRgb255Tests
{
    private const double Tolerance = 0.00000005;
    private static readonly YbrConfiguration YbrConfig = YbrConfiguration.Rec601;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaRgbLinear(ColourTriplet triplet) => AssertViaRgbLinear(triplet, RgbConfiguration.StandardRgb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.NegativeRgb255Triplets))]
    public void ViaRgbLinearNegative(ColourTriplet triplet) => AssertViaRgbLinear(triplet, RgbConfiguration.StandardRgb);
    
    // testing RGB ↔ RGB Linear with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbLinearDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.Rgb255TripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgbLinear(triplet, rgbConfig);
    
    // testing negative RGB ↔ RGB Linear with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbLinearNegativeDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.NegativeRgb255TripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgbLinear(triplet, rgbConfig);
    
    private static void AssertViaRgbLinear(ColourTriplet triplet, RgbConfiguration rgbConfig)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var rgbLinear = Rgb.ToRgbLinear(original, rgbConfig);
        var roundtrip = Rgb.FromRgbLinear(rgbLinear, rgbConfig);
        AssertRoundtrip(triplet, original, roundtrip);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaHsb(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var hsb = Hsb.FromRgb(original);
        var roundtrip = Hsb.ToRgb(hsb);
        AssertRoundtrip(triplet, original, roundtrip);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaYpbpr(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var ypbpr = Ypbpr.FromRgb(original, YbrConfig);
        var roundtrip = Ypbpr.ToRgb(ypbpr, YbrConfig);
        AssertRoundtrip(triplet, original, roundtrip);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaYcgco(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var ycgco = Ycgco.FromRgb(original);
        var roundtrip = Ycgco.ToRgb(ycgco);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaYuv(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var yuv = Yuv.FromRgb(original);
        var roundtrip = Yuv.ToRgb(yuv);
        AssertRoundtrip(triplet, original, roundtrip);
    }

    private static void AssertRoundtrip(ColourTriplet triplet, Rgb original, Rgb roundtrip)
    {
        TestUtils.AssertTriplet(original.Byte255.Triplet, triplet, Tolerance);
        TestUtils.AssertTriplet(roundtrip.Byte255.Triplet, triplet, Tolerance);
        TestUtils.AssertTriplet(roundtrip.Byte255.Triplet, original.Byte255.Triplet, Tolerance);
    }
}