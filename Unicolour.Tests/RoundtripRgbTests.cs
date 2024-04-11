namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripRgbTests
{
    private const double Tolerance = 0.00000005;
    private static readonly YbrConfiguration YbrConfig = YbrConfiguration.Rec601;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaRgbLinear(ColourTriplet triplet) => AssertViaRgbLinear(triplet, RgbConfiguration.StandardRgb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.NegativeRgbTriplets))]
    public void ViaRgbLinearNegative(ColourTriplet triplet) => AssertViaRgbLinear(triplet, RgbConfiguration.StandardRgb);
    
    // testing RGB ↔ RGB Linear with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbLinearDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.RgbTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgbLinear(triplet, rgbConfig);
    
    // testing negative RGB ↔ RGB Linear with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbLinearNegativeDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.NegativeRgbTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgbLinear(triplet, rgbConfig);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaRgbLinearFromNamed(TestColour namedColour) => AssertViaRgbLinear(namedColour.Rgb!, RgbConfiguration.StandardRgb);
    
    private static void AssertViaRgbLinear(ColourTriplet triplet, RgbConfiguration rgbConfig)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var rgbLinear = Rgb.ToRgbLinear(original, rgbConfig);
        var roundtrip = Rgb.FromRgbLinear(rgbLinear, rgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaHsb(ColourTriplet triplet) => AssertViaHsb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHsbFromNamed(TestColour namedColour) => AssertViaHsb(namedColour.Rgb!);
    
    private static void AssertViaHsb(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var hsb = Hsb.FromRgb(original);
        var roundtrip = Hsb.ToRgb(hsb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaYpbpr(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var ypbpr = Ypbpr.FromRgb(original, YbrConfig);
        var roundtrip = Ypbpr.ToRgb(ypbpr, YbrConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaYcgco(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var ycgco = Ycgco.FromRgb(original);
        var roundtrip = Ycgco.ToRgb(ycgco);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaYuv(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var yuv = Yuv.FromRgb(original);
        var roundtrip = Yuv.ToRgb(yuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    /*
     * CMY / CMYK is not integrated into Unicolour
     * and tests will need to change if calibrated CMYK using ICC profiles are implemented
     */
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaCmyk(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var cmyk = Cmyk.FromRgb(original);
        var roundtrip = Cmyk.ToRgb(cmyk);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaCmy(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var cmy = Cmy.FromRgb(original);
        var roundtrip = Cmy.ToRgb(cmy);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}