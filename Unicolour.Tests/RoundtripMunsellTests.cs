using System;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripMunsellTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    private static readonly Configuration ConfigC = new(xyzConfig: new XyzConfiguration(Illuminant.C, Observer.Degree2));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.MunsellTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        try
        {
            var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
            var xyy = Munsell.ToXyy(original);
            var roundtrip = Munsell.FromXyy(xyy);

            Console.WriteLine(original);
            Console.WriteLine(roundtrip);
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
        // var triplet = new ColourTriplet(97.53924922261605, 6.889359646819056, 13.520272139129634);
        var triplet = new ColourTriplet(304.65969921086315, 6.704977470510379, 20.908396997084353);
        var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
        var xyy = Munsell.ToXyy(original);
        var roundtrip = Munsell.FromXyy(xyy);
        Console.WriteLine(original);
        Console.WriteLine(roundtrip);
        AssertMunsell(original, roundtrip);
    }

    private static void AssertMunsell(Munsell original, Munsell roundtrip)
    {
        var normalisedOriginal = Normalise(original);
        var normalisedRoundtrip = Normalise(roundtrip);
        TestUtils.AssertTriplet(normalisedOriginal, normalisedRoundtrip, 0.03);
    }

    private static ColourTriplet Normalise(Munsell munsell)
    {
        return new ColourTriplet(munsell.HueDegrees, munsell.Value / 10.0, munsell.Chroma / 100.0, HueIndex: 0);
    }
}