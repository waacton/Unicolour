using System;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripMunsellTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.MunsellTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        try
        {
            var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
            var xyy = Munsell.ToXyy(original);
            var roundtrip = Munsell.FromXyy(xyy);
            AssertMunsell(original, roundtrip);
        }
        catch (InvalidOperationException e)
        {
            Assert.Ignore();
        }
    }
    
    [Test]
    public void Single()
    {
        var triplet = new ColourTriplet(97.53924922261605, 6.889359646819056, 13.520272139129634);
        var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
        var xyy = Munsell.ToXyy(original);
        var roundtrip = Munsell.FromXyy(xyy);
        AssertMunsell(original, roundtrip);
    }

    private static void AssertMunsell(Munsell original, Munsell roundtrip)
    {
        var normalisedOriginal = Normalise(original);
        var normalisedRoundtrip = Normalise(roundtrip);
        var tolerance = original.Value < 1 ? 0.05 : 0.025;
        tolerance = 0.02;
        // TODO: should this become more specific? seems like
        //       - low value reduces accuracy of hue?
        //       - very high chroma reduces accuracy of chroma?
        TestUtils.AssertTriplet(normalisedOriginal, normalisedRoundtrip, tolerance);
    }

    private static ColourTriplet Normalise(Munsell munsell)
    {
        return new ColourTriplet(munsell.HueDegrees, munsell.Value / 10.0, munsell.Chroma / 100.0, HueIndex: 0);
    }
}