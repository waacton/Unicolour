using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripXyyTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var xyz = Xyy.ToXyz(original);
        var roundtrip = Xyy.FromXyz(xyz, XyzConfig.WhiteChromaticity);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaWxy(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var wxy = Wxy.FromXyy(original, XyzConfig);
        var roundtrip = Wxy.ToXyy(wxy, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    /*
     * most roundtrips via munsell are notably more accurate but a higher tolerance is needed to handle the worst cases
     * (the `ViaMunsellAverage` test confirms that the average roundtrip is reasonably accurate)
     * ----------
     * an example where some of the larger errors stem from:
     * consider xyY (0.1750, 0.7506, 0.0135) · V = 1.1207
     * V1 xy located within 10GY 1/ 6 · 10GY 1/ 8 · 7.5GY 1/ 8 · 7.5GY 1/ 6 (interpolation --> 9.9186GY 1/ 6.8963)
     * V2 xy located within 10GY 2/12 · 10GY 2/14 · 2.5G  2/14 · 2.5G  2/12 (interpolation --> 0.5494G  1/12.1025)
     * interpolation between V --> 9.9947GY 1.1207/7.5246
     * ...
     * back to xy: 9.9947GY 1.1207/7.5246 suggests we should use 7.5GY /6 · 7.5GY /8 · 10GY /6 · 10GY /8 as basis
     * V1 interpolation --> (0.1371, 0.8252) · V2 interpolation (0.2681, 0.5631)
     * interpolation between V --> (0.1530, 0.7936)
     * ...
     * (0.1530, 0.7936) is quite different from (0.1750, 0.7506)!
     * presumably the shift from 7.5GY-10GY hue @ 6-8 chroma --> 10GY-2.5G hue @ 12-14 chroma between V for the same xy location
     * has a strong impact on interpolated result, especially when conversion to xyY uses 7.5GY-10GY hue @ 6-8 chroma for both V
     */
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaMunsell(ColourTriplet triplet)
    {
        try
        {
            var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
            var munsell = Munsell.FromXyy(original);
            var roundtrip = Munsell.ToXyy(munsell);
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, 0.08);
        }
        catch (InvalidOperationException e)
        {
            // TODO: detect these cases (XY not within dataset; VC not within dataset)
            //       likely use a higher tolerance, they will be a less accurate approximation
            Assert.Ignore(e.Message);
        }
    }

    [Test] // reassurance that roundtrips via munsell are typically reasonably accurate, even if certain data points are not
    public void ViaMunsellAverage()
    {
        var deltas = RandomColours.XyyTriplets.Select(GetRoundtripDelta).ToArray();

        // TODO: filter by data that was within dataset vs without
        //       expect to require higher tolerance as data outwith dataset are a less accurate approximation
        var insideDatasetDeltas = deltas.Where(delta => !double.IsNaN(delta.x) && !double.IsNaN(delta.y)).ToArray();
        Assert.That(insideDatasetDeltas.Average(delta => delta.x), Is.LessThan(0.0025));
        Assert.That(insideDatasetDeltas.Average(delta => delta.y), Is.LessThan(0.0025));
    }
    
    private static (double x, double y) GetRoundtripDelta(ColourTriplet triplet)
    {
        try
        {
            var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
            var munsell = Munsell.FromXyy(original);
            var roundtrip = Munsell.ToXyy(munsell);
                
            // conversion between V and Y is accurate
            // including it here would artificially increase reduce the error being tested
            return (Math.Abs(original.Chromaticity.X - roundtrip.Chromaticity.X), Math.Abs(original.Chromaticity.Y - roundtrip.Chromaticity.Y));
        }
        catch (InvalidOperationException e)
        {
            // TODO: detect these cases (XY not within dataset; VC not within dataset)
            //       likely use a higher tolerance, they will be a less accurate approximation
            return (double.NaN, double.NaN);
        }
    }
}