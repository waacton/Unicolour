using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripYcbcrTests
{
    private const double Tolerance = 0.0000000005;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.YcbcrTriplets))]
    public void ViaYpbpr(ColourTriplet triplet) => AssertViaYpbpr(triplet, YbrConfiguration.Rec601);
    
    // testing YCbCr ↔ YPbPr with all configurations to ensure roundtrip of digital range mapping
    [Test, Combinatorial]
    public void ViaYpbprDifferentConfig(
        [ValueSource(typeof(RandomColours), nameof(RandomColours.YcbcrTripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultYbrConfigs))] YbrConfiguration ybrConfig) 
        => AssertViaYpbpr(triplet, ybrConfig);
    
    private static void AssertViaYpbpr(ColourTriplet triplet, YbrConfiguration ybrConfig)
    {
        var original = new Ycbcr(triplet.First, triplet.Second, triplet.Third);
        var ypbpr = Ycbcr.ToYpbpr(original, ybrConfig);
        var roundtrip = Ycbcr.FromYpbpr(ypbpr, ybrConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}