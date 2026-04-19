using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripYcbcrTests
{
    private const double Tolerance = 0.0000000005;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Ycbcr, 1500);
    internal static readonly List<ColourTriplet> TripletsSubset = Triplets.Take(300).ToList();
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaYpbpr(ColourTriplet triplet) => AssertViaYpbpr(triplet, YbrConfiguration.Rec601);
    
    // testing YCbCr ↔ YPbPr with all configurations to ensure roundtrip of digital range mapping
    [Test, Combinatorial]
    public void ViaYpbprDifferentConfig(
        [ValueSource(nameof(TripletsSubset))] ColourTriplet triplet,
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