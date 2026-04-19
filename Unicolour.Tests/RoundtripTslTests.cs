using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripTslTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly YbrConfiguration YbrConfig = YbrConfiguration.Rec601;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Tsl, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new Tsl(triplet.First, triplet.Second, triplet.Third);
        var rgb = Tsl.ToRgb(original, YbrConfig);
        var roundtrip = Tsl.FromRgb(rgb, YbrConfig);
        
        var negativeChroma = original.S < 0;
        if (negativeChroma)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(0, 0, original.L, HueIndex: 0), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}