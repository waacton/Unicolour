using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripYiqTests
{
    private const double Tolerance = 0.0000000005;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Yiq, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaYuv(ColourTriplet triplet)
    {
        var original = new Yiq(triplet.First, triplet.Second, triplet.Third);
        var yuv = Yiq.ToYuv(original);
        var roundtrip = Yiq.FromYuv(yuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}