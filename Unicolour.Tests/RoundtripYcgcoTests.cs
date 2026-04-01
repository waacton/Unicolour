using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripYcgcoTests
{
    private const double Tolerance = 0.0000000005;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Ycgco, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new Ycgco(triplet.First, triplet.Second, triplet.Third);
        var rgb = Ycgco.ToRgb(original);
        var roundtrip = Ycgco.FromRgb(rgb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}