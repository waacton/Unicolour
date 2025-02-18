﻿using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownHsluvTests
{
    private const double Tolerance = 0.05;

    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Hsluv>(red, new(12.177, 100.00, 53.237), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Hsluv>(green, new(127.72, 100.00, 87.736), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Hsluv>(blue, new(265.87, 100.00, 32.301), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Hsluv>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Hsluv>(white, new(180.0, 0.0, 100.0), Tolerance);
    }
    
    /*
     * note that at each step of the conversion process, more tolerance is required to match the HSLuv test data set
     * this divergence is likely due to slight differences in the initial RGB -> XYZ matrix used
     * HSLuv reference implementation uses hardcoded values, for which I can't find the source       (first matrix value: 0.41239079926595)
     * Unicolour calculates matrix from chromaticities and matches Bruce Lindbloom's sRGB D65 matrix (first matrix value: 0.41245643908969187)
     */
    [TestCaseSource(typeof(HsluvTestColour), nameof(HsluvTestColour.All))]
    public void SnapshotTestColour(TestColour testColour)
    {
        var hex = testColour.Hex!;
        var colour = new Unicolour(hex);
        var info = $"{hex} \n-> RGB: {colour.Rgb} \n-> XYZ: {colour.Xyz} \n-> LUV: {colour.Luv} \n-> LCH: {colour.Lchuv}";
        
        ColourTriplet HandleNoHue(ColourTriplet triplet, bool hasHue) => hasHue ? triplet : triplet.WithHueOverride(0);
        var lchuv = HandleNoHue(colour.Lchuv.Triplet, colour.Lchuv.UseAsHued);
        var hsluv = HandleNoHue(colour.Hsluv.Triplet, colour.Hsluv.UseAsHued);
        var hpluv = HandleNoHue(colour.Hpluv.Triplet, colour.Hpluv.UseAsHued);
        
        // accuracy drops off when saturation goes beyond 100% (mostly at 1,500%+), so be slightly more tolerant for larger values
        AssertSnapshot(colour.Rgb.Triplet, testColour.Rgb!, 0.00000000001, info);
        AssertSnapshot(colour.Xyz.Triplet, testColour.Xyz!, 0.0005, info);
        AssertSnapshot(colour.Luv.Triplet, testColour.Luv!, 0.025, info);
        AssertSnapshot(lchuv, testColour.Lchuv!, 0.25, info);
        AssertSnapshot(hsluv, testColour.Hsluv!, tolerance: 0.06, info);
        AssertSnapshot(hpluv, testColour.Hpluv!, tolerance: hpluv.Second > 100 ? 0.135 : 0.06, info);
    }

    private static void AssertSnapshot(ColourTriplet actual, ColourTriplet expected, double tolerance, string info)
    {
        TestUtils.AssertTriplet(actual, expected, tolerance, info);
    }
}