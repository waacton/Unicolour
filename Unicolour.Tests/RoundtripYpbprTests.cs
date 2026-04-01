using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripYpbprTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly YbrConfiguration YbrConfig = YbrConfiguration.Rec601;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Ypbpr, 1500);
    internal static readonly List<ColourTriplet> TripletsSubset = Triplets.Take(300).ToList();

    [TestCaseSource(nameof(Triplets))]
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
        [ValueSource(nameof(TripletsSubset))] ColourTriplet triplet,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultYbrConfigs))] YbrConfiguration ybrConfig) 
        => AssertViaYcbcr(triplet, ybrConfig);
    
    [TestCaseSource(nameof(TripletsSubset))]
    public void ViaYcbcr(ColourTriplet triplet) => AssertViaYcbcr(triplet, YbrConfiguration.Rec601);
    
    private static void AssertViaYcbcr(ColourTriplet triplet, YbrConfiguration ybrConfig)
    {
        var original = new Ypbpr(triplet.First, triplet.Second, triplet.Third);
        var ycbcr = Ycbcr.FromYpbpr(original, ybrConfig);
        var roundtrip = Ycbcr.ToYpbpr(ycbcr, ybrConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}