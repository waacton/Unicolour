using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripRgbTests
{
    private const double Tolerance = 0.00000005;
    private static readonly YbrConfiguration YbrConfig = YbrConfiguration.Rec601;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaRgbLinear(ColourTriplet triplet) => AssertViaRgbLinear(triplet, RgbConfiguration.StandardRgb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.UnboundRgbTriplets))]
    public void ViaRgbLinearUnbound(ColourTriplet triplet) => AssertViaRgbLinear(triplet, RgbConfiguration.StandardRgb);
    
    // testing RGB ↔ RGB Linear with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbLinearDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.RgbTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgbLinear(triplet, rgbConfig);
    
    // testing unbound RGB ↔ RGB Linear with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbLinearUnboundDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.UnboundRgbTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgbLinear(triplet, rgbConfig);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaRgbLinearFromNamed(TestColour namedColour) => AssertViaRgbLinear(namedColour.Rgb!, RgbConfiguration.StandardRgb);
    
    private static void AssertViaRgbLinear(ColourTriplet triplet, RgbConfiguration rgbConfig)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var rgbLinear = Rgb.ToRgbLinear(original, rgbConfig);
        var roundtrip = Rgb.FromRgbLinear(rgbLinear, rgbConfig);
        
        // ACEScc is not fully roundtrip compatible as it does not support linear <= 0 or nonlinear >= ~1.468
        if (rgbConfig == RgbConfiguration.Acescc)
        {
            var lower = RgbModels.Acescc.MinNonlinearValue;
            var upper = RgbModels.Acescc.FromLinear(RgbModels.Acescc.MaxLinearValue);
            AssertBoundedRoundtrip(original, roundtrip, lower, upper);
            return;
        }

        // ACEScct is not fully roundtrip compatible as it does not support nonlinear >= ~1.468
        if (rgbConfig == RgbConfiguration.Acescct)
        {
            var upper = RgbModels.Acescct.FromLinear(RgbModels.Acescct.MaxLinearValue);
            AssertBoundedRoundtrip(original, roundtrip, double.MinValue, upper);
            return;
        }
        
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
    public void ViaHsi(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var hsi = Hsi.FromRgb(original);
        var roundtrip = Hsi.ToRgb(hsi);
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
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaTsl(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var tsl = Tsl.FromRgb(original, YbrConfig);
        var roundtrip = Tsl.ToRgb(tsl, YbrConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    // although RGB -> CMYK -> RGB results in same values, cannot guarantee roundtrip of CMYK -> RGB -> CMYK
    // e.g. CMYK[0.5, 0.5, 0.5, 0.5] = RGB[0.25, 0.25, 0.25] = CMYK[0, 0, 0, 0.75]
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaCmyk(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var cmyk = Channels.UncalibratedFromRgb(original);
        var roundtrip = Channels.UncalibratedToRgb(cmyk);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    /*
     * CMY is not, and will not be, fully integrated into Unicolour
     * just here for the sake of completeness
     */
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaCmy(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First, triplet.Second, triplet.Third);
        var cmy = Cmy.FromRgb(original);
        var roundtrip = Cmy.ToRgb(cmy);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    private static void AssertBoundedRoundtrip(Rgb original, Rgb roundtrip, double lower, double upper)
    {
        var boundOriginal = new Rgb(original.R.Clamp(lower, upper), original.G.Clamp(lower, upper), original.B.Clamp(lower, upper));
        TestUtils.AssertTriplet(roundtrip.Triplet, boundOriginal.Triplet, Tolerance);
    }
}