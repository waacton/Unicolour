using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripRgbLinearTests
{
    private const double Tolerance = 0.00000005;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    // SDR instead of HDR because SDR min luminance is not zero
    // making roundtrip impossible in some cases, therefore a more difficult test case
    private static readonly DynamicRange DynamicRange = DynamicRange.Standard;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbLinearTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new RgbLinear(triplet.First, triplet.Second, triplet.Third);
        var xyz = RgbLinear.ToXyz(original, RgbConfig, XyzConfig);
        var roundtrip = RgbLinear.FromXyz(xyz, RgbConfig, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbLinearTriplets))]
    public void ViaRgb(ColourTriplet triplet) => AssertViaRgb(triplet, RgbConfiguration.StandardRgb);
        
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.UnboundRgbLinearTriplets))]
    public void ViaRgbUnbound(ColourTriplet triplet) => AssertViaRgb(triplet, RgbConfiguration.StandardRgb);
    
    // testing RGB Linear ↔ RGB with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.RgbLinearTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig)
        => AssertViaRgb(triplet, rgbConfig);
    
    // testing unbound RGB Linear ↔ RGB with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbUnboundDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.UnboundRgbLinearTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgb(triplet, rgbConfig);
    
    private static void AssertViaRgb(ColourTriplet triplet, RgbConfiguration rgbConfig)
    {
        var original = new RgbLinear(triplet.First, triplet.Second, triplet.Third);
        var rgb = Rgb.FromRgbLinear(original, rgbConfig, DynamicRange);
        var roundtrip = Rgb.ToRgbLinear(rgb, rgbConfig, DynamicRange);
        
        // ACEScc is not fully roundtrip compatible as it does not support linear <= 0 or nonlinear >= ~1.468
        if (rgbConfig == RgbConfiguration.Acescc)
        {
            var lower = RgbModels.Acescc.ToLinear(RgbModels.Acescc.MinNonlinearValue);
            var upper = RgbModels.Acescc.MaxLinearValue;
            AssertBoundedRoundtrip(original, roundtrip, lower, upper);
            return;
        }

        // ACEScct is not fully roundtrip compatible as it does not support nonlinear >= ~1.468
        if (rgbConfig == RgbConfiguration.Acescct)
        {
            var upper = RgbModels.Acescct.MaxLinearValue;
            AssertBoundedRoundtrip(original, roundtrip, double.MinValue, upper);
            return;
        }
        
        // Rec. 2100 PQ is not fully roundtrip compatible as the PQ function can result in NaN
        if (rgbConfig == RgbConfiguration.Rec2100Pq)
        {
            AssertPqRoundtrip(original, roundtrip);
            return;
        }
        
        // Rec. 2100 HLG is not fully roundtrip compatible as the HLG function maps all linear values between 0 and OETF^-1(beta) to 0 (in OETF)
        if (rgbConfig == RgbConfiguration.Rec2100Hlg)
        {
            AssertHlgRoundtrip(original, roundtrip);
            return;
        }
        
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbLinearTriplets))]
    public void ViaXyb(ColourTriplet triplet)
    {
        var original = new RgbLinear(triplet.First, triplet.Second, triplet.Third);
        var xyb = Xyb.FromRgbLinear(original);
        var roundtrip = Xyb.ToRgbLinear(xyb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    private static void AssertBoundedRoundtrip(RgbLinear original, RgbLinear roundtrip, double lower, double upper)
    {
        var boundOriginal = new RgbLinear(original.R.Clamp(lower, upper), original.G.Clamp(lower, upper), original.B.Clamp(lower, upper));
        TestUtils.AssertTriplet(roundtrip.Triplet, boundOriginal.Triplet, Tolerance);
    }
    
    private static void AssertPqRoundtrip(RgbLinear original, RgbLinear roundtrip)
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
    
    private static void AssertHlgRoundtrip(RgbLinear original, RgbLinear roundtrip)
    {
        var expected = new ColourTriplet(
            GetExpected(original.R),
            GetExpected(original.G),
            GetExpected(original.B)
        );
        
        TestUtils.AssertTriplet(roundtrip.Triplet, expected, Tolerance);
        return;

        double GetExpected(double originalValue)
        {
            return Math.Abs(originalValue) >= DynamicRange.HlgMinLinear 
                ? originalValue 
                : Math.Sign(originalValue) * DynamicRange.HlgMinLinear;
        }
    }
}