using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripMunsellTests
{
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.MunsellTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        try
        {
            var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
            var xyy = Munsell.ToXyy(original);
            var roundtrip = Munsell.FromXyy(xyy);
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [5, 5e-15, 3]);
        }
        catch (InvalidOperationException e)
        {
            Assert.Ignore(e.Message);
        }
    }
    
    [Test] // reassurance that roundtrips via munsell are typically reasonably accurate, even if certain data points are not
    public void ViaXyyAverage()
    {
        var deltas = RandomColours.MunsellTriplets.Select(GetRoundtripDelta).ToArray();

        // TODO: filter by data that was within dataset vs without
        //       expect to require higher tolerance as data outwith dataset are a less accurate approximation
        var insideDatasetDeltas = deltas.Where(delta => !double.IsNaN(delta.h) && !double.IsNaN(delta.c)).ToArray();
        Assert.That(insideDatasetDeltas.Average(delta => delta.h), Is.LessThan(0.25)); // degrees (0 - 360)
        Assert.That(insideDatasetDeltas.Average(delta => delta.c), Is.LessThan(0.1)); // chroma (0 - 50)
    }

    private static (double h, double c) GetRoundtripDelta(ColourTriplet triplet)
    {
        try
        {
            var original = new Munsell(triplet.First, triplet.Second, triplet.Third);
            var xyy = Munsell.ToXyy(original);
            var roundtrip = Munsell.FromXyy(xyy);

            var (originalHue, roundtripHue) = Hue.Adapt(original.HueDegrees, roundtrip.HueDegrees, HueSpan.Shorter);
                
            // conversion between V and Y is accurate
            // including it here would artificially increase reduce the error being tested
            return (Math.Abs(originalHue - roundtripHue), Math.Abs(original.Chroma - roundtrip.Chroma));
        }
        catch (InvalidOperationException e)
        {
            // TODO: detect these cases (XY not within dataset; VC not within dataset)
            //       likely use a higher tolerance, they will be a less accurate approximation
            return (double.NaN, double.NaN);
        }
    }
}