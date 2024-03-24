namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripYpbprTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly YbrConfiguration YbrConfig = YbrConfiguration.Rec601;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YpbprTriplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new Ypbpr(triplet.First, triplet.Second, triplet.Third);
        var rgb = Ypbpr.ToRgb(original, YbrConfig);
        var roundtrip = Ypbpr.FromRgb(rgb, YbrConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    // testing YPbPr ↔ YCbCr with all configurations to ensure roundtrip of digital range mapping
    [Test, Combinatorial]
    public void ViaYcbcrDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.YpbprTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultYbrConfigs))] YbrConfiguration ybrConfig) 
        => AssertViaYcbcr(triplet, ybrConfig);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YpbprTriplets))]
    public void ViaYcbcr(ColourTriplet triplet) => AssertViaYcbcr(triplet, YbrConfiguration.Rec601);
    
    private static void AssertViaYcbcr(ColourTriplet triplet, YbrConfiguration ybrConfig)
    {
        var original = new Ypbpr(triplet.First, triplet.Second, triplet.Third);
        var ycbcr = Ycbcr.FromYpbpr(original, ybrConfig);
        var roundtrip = Ycbcr.ToYpbpr(ycbcr, ybrConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}