using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHsiTests
{
    private const double Tolerance = 0.0000000005;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Hsi, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new Hsi(triplet.First, triplet.Second, triplet.Third);
        var rgb = Hsi.ToRgb(original);
        var roundtrip = Hsi.FromRgb(rgb);
        
        var negativeChroma = original.S < 0 ^ original.I < 0;
        if (negativeChroma)
        {
            var greyLevel = rgb.R; // should be same as G and B
            TestUtils.AssertTriplet(roundtrip.Triplet, new(0, 0, greyLevel, HueIndex: 0), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}