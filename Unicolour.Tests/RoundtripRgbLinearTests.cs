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
        var rgb = RgbLinear.ToXyz(original, RgbConfig, XyzConfig);
        var roundtrip = RgbLinear.FromXyz(rgb, RgbConfig, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbLinearTriplets))]
    public void ViaRgb(ColourTriplet triplet) => AssertViaRgb(triplet, RgbConfiguration.StandardRgb);
        
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.NegativeRgbLinearTriplets))]
    public void ViaRgbNegative(ColourTriplet triplet) => AssertViaRgb(triplet, RgbConfiguration.StandardRgb);
    
    // testing RGB Linear ↔ RGB with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.RgbLinearTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig)
        => AssertViaRgb(triplet, rgbConfig);
    
    // testing negative RGB Linear ↔ RGB with all configurations to ensure roundtrip of companding / gamma correction
    [Test, Combinatorial]
    public void ViaRgbNegativeDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.NegativeRgbLinearTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig) 
        => AssertViaRgb(triplet, rgbConfig);
    
    private static void AssertViaRgb(ColourTriplet triplet, RgbConfiguration rgbConfig)
    {
        var original = new RgbLinear(triplet.First, triplet.Second, triplet.Third);
        var rgb = Rgb.FromRgbLinear(original, rgbConfig);
        var roundtrip = Rgb.ToRgbLinear(rgb, rgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}