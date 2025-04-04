using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripRgb255Tests
{
    private const double Tolerance = 0.00000075;
    private static readonly YbrConfiguration YbrConfig = YbrConfiguration.Rec601;
    private static readonly DynamicRange DynamicRange = DynamicRange.Standard;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaRgbLinear(ColourTriplet triplet) => AssertViaRgbLinear(triplet, RgbConfiguration.StandardRgb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.UnboundRgb255Triplets))]
    public void ViaRgbLinearUnbound(ColourTriplet triplet) => AssertViaRgbLinear(triplet, RgbConfiguration.StandardRgb);
    
    // testing RGB ↔ RGB Linear with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbLinearDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.Rgb255TripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgbLinear(triplet, rgbConfig);
    
    // testing unbound RGB ↔ RGB Linear with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbLinearUnboundDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.UnboundRgb255TripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgbLinear(triplet, rgbConfig);
    
    private static void AssertViaRgbLinear(ColourTriplet triplet, RgbConfiguration rgbConfig)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var rgbLinear = Rgb.ToRgbLinear(original, rgbConfig, DynamicRange);
        var roundtrip = Rgb.FromRgbLinear(rgbLinear, rgbConfig, DynamicRange);
        
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
        
        // Rec. 2100 PQ is not fully roundtrip compatible as the PQ function can result in NaN
        if (rgbConfig == RgbConfiguration.Rec2100Pq)
        {
            AssertPqRoundtrip(original, roundtrip);
            return;
        }
        
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
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ViaHsi(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var hsi = Hsi.FromRgb(original);
        var roundtrip = Hsi.ToRgb(hsi);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
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
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaTsl(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var tsl = Tsl.FromRgb(original, YbrConfig);
        var roundtrip = Tsl.ToRgb(tsl, YbrConfig);
        AssertRoundtrip(triplet, original, roundtrip);
    }
    
    // although RGB -> CMYK -> RGB results in same values, cannot guarantee roundtrip of CMYK -> RGB -> CMYK
    // e.g. CMYK[0.5, 0.5, 0.5, 0.5] = RGB[0.25, 0.25, 0.25] = CMYK[0, 0, 0, 0.75]
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaCmyk(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var cmyk = Channels.UncalibratedFromRgb(original);
        var roundtrip = Channels.UncalibratedToRgb(cmyk);
        AssertRoundtrip(triplet, original, roundtrip);
    }
    
    /*
     * CMY is not, and will not be, fully integrated into Unicolour
     * just here for the sake of completeness
     */
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ViaCmy(ColourTriplet triplet)
    {
        var original = new Rgb(triplet.First / 255.0, triplet.Second / 255.0, triplet.Third / 255.0);
        var cmy = Cmy.FromRgb(original);
        var roundtrip = Cmy.ToRgb(cmy);
        AssertRoundtrip(triplet, original, roundtrip);
    }

    private static void AssertRoundtrip(ColourTriplet triplet, Rgb original, Rgb roundtrip)
    {
        TestUtils.AssertTriplet(original.Byte255.Triplet, triplet, Tolerance);
        TestUtils.AssertTriplet(roundtrip.Byte255.Triplet, triplet, Tolerance);
        TestUtils.AssertTriplet(roundtrip.Byte255.Triplet, original.Byte255.Triplet, Tolerance);
    }
    
    private static void AssertBoundedRoundtrip(Rgb original, Rgb roundtrip, double lower, double upper)
    {
        var boundOriginal = new Rgb(original.R.Clamp(lower, upper), original.G.Clamp(lower, upper), original.B.Clamp(lower, upper));
        TestUtils.AssertTriplet(roundtrip.Byte255.Triplet, boundOriginal.Byte255.Triplet, Tolerance);
    }
    
    private static void AssertPqRoundtrip(Rgb original, Rgb roundtrip)
    {
        var expected = new ColourTriplet(
            GetExpected(roundtrip.R, original.R),
            GetExpected(roundtrip.G, original.G),
            GetExpected(roundtrip.B, original.B)
        );
        
        TestUtils.AssertTriplet(roundtrip.Triplet, expected, Tolerance);
        return;

        double GetExpected(double actualValue, double originalValue)
        {
            if (!double.IsNaN(actualValue))
            {
                return originalValue;
            }
            
            var pqResult = Pq.Smpte.Eotf(originalValue, DynamicRange.WhiteLuminance);
            Assert.That(pqResult, Is.NaN);
            return double.NaN;
        }
    }
}