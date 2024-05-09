namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripRgbLinearTests
{
    private const double Tolerance = 0.00000005;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
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
        var rgb = Rgb.FromRgbLinear(original, rgbConfig);
        var roundtrip = Rgb.ToRgbLinear(rgb, rgbConfig);
        
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
}