using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripYdbdrTests
{
    private const double Tolerance = 0.0000000005;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Ydbdr, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaYuv(ColourTriplet triplet)
    {
        var original = new Ydbdr(triplet.First, triplet.Second, triplet.Third);
        var yuv = Ydbdr.ToYuv(original);
        var roundtrip = Ydbdr.FromYuv(yuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}