using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHsluvTests
{
    private const double Tolerance = 0.00000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Hsluv, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaLchuv(ColourTriplet triplet)
    {
        var original = new Hsluv(triplet.First, triplet.Second, triplet.Third);
        var lchuv = Hsluv.ToLchuv(original);
        var roundtrip = Hsluv.FromLchuv(lchuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}