using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class GamutMapTests
{
    private static GamutMap[] GamutMaps = [GamutMap.RgbClipping, GamutMap.OklchChromaReduction, GamutMap.WxyPurityReduction];

    private static List<(double, double, double)> GamutInsideValues =
    [
        (0, 0, 0),
        (0.00000000001, 0, 0),
        (0, 0.00000000001, 0),
        (0, 0, 0.00000000001),
        (1, 1, 0.99999999999),
        (1, 0.99999999999, 1),
        (0.99999999999, 1, 1),
        (1, 1, 1)
    ];

    [Test, Combinatorial]
    public void GamutInside(
        [ValueSource(nameof(GamutInsideValues))] (double, double, double) rgb,
        [ValueSource(nameof(GamutMaps))] GamutMap gamutMap)
    {
        var original = new Unicolour(ColourSpace.Rgb, rgb);
        var gamutMapped = original.MapToRgbGamut(gamutMap);
        Assert.That(original.IsInRgbGamut, Is.True);
        Assert.That(gamutMapped.IsInRgbGamut, Is.True);
        TestUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.Triplet, 1e-16);
    }
    
    private static List<(double, double, double)> GamutOutsideValues =
    [
        (-0.00000000001, 0, 0),
        (0, -0.00000000001, 0),
        (0, 0, -0.00000000001),
        (1, 1, 1.00000000001),
        (1, 1.00000000001, 1),
        (1.00000000001, 1, 1)
    ];
    
    [Test, Combinatorial]
    public void GamutOutside(
        [ValueSource(nameof(GamutOutsideValues))] (double, double, double) rgb,
        [ValueSource(nameof(GamutMaps))] GamutMap gamutMap)
    {
        var original = new Unicolour(ColourSpace.Rgb, rgb);
        var gamutMapped = original.MapToRgbGamut(gamutMap);
        Assert.That(original.IsInRgbGamut, Is.False);
        Assert.That(gamutMapped.IsInRgbGamut, Is.True);
    }
    
    private static ColourTriplet[] RandomInGamutValues = RandomColours.RgbTriplets.Take(100).ToArray();
    private static ColourTriplet[] RandomOutGamutValues = RandomInGamutValues.Select(MakeOutOfGamut).ToArray();
    
    [Test, Combinatorial]
    public void RandomGamutInside(
        [ValueSource(nameof(RandomInGamutValues))] ColourTriplet triplet,
        [ValueSource(nameof(GamutMaps))] GamutMap gamutMap)
    {
        var original = new Unicolour(ColourSpace.Rgb, triplet.Tuple);
        var gamutMapped = original.MapToRgbGamut(gamutMap);
        Assert.That(gamutMapped.Rgb.IsInGamut, Is.True);
        Assert.That(gamutMapped.Rgb.Triplet, Is.EqualTo(gamutMapped.Rgb.ConstrainedTriplet));
        TestUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.Triplet, 0);
    }
    
    [Test, Combinatorial]
    public void RandomGamutOutside(
        [ValueSource(nameof(RandomOutGamutValues))] ColourTriplet triplet,
        [ValueSource(nameof(GamutMaps))] GamutMap gamutMap)
    {
        var original = new Unicolour(ColourSpace.Rgb, triplet.Tuple);
        var gamutMapped = original.MapToRgbGamut(gamutMap);
        Assert.That(original.IsInRgbGamut, Is.False);
        Assert.That(gamutMapped.IsInRgbGamut, Is.True);
    }

    // regression test of values calculated for https://unicolour.wacton.xyz/wxy-colour-space#%EF%B8%8F-gamut-mapping
    [TestCase(GamutMap.RgbClipping, "#00ED00")]
    [TestCase(GamutMap.OklchChromaReduction, "#00D367")]
    [TestCase(GamutMap.WxyPurityReduction, "#04D77D")]
    public void Green(GamutMap gamutMap, string expected)
    {
        var colour = new Unicolour(ColourSpace.Wxy, 530, 1, 0.5);
        var gamutMapped = colour.MapToRgbGamut(gamutMap);
        Assert.That(gamutMapped.Hex, Is.EqualTo(expected));
    }
    
    // regression test of values calculated for https://unicolour.wacton.xyz/wxy-colour-space#%EF%B8%8F-gamut-mapping
    [TestCase(GamutMap.RgbClipping, "#FF00FF")]
    [TestCase(GamutMap.OklchChromaReduction, "#FFC6F8")]
    [TestCase(GamutMap.WxyPurityReduction, "#FF9AE6")]
    public void Magenta(GamutMap gamutMap, string expected)
    {
        var colour = new Unicolour(ColourSpace.Wxy, -530, 1, 0.5);
        var gamutMapped = colour.MapToRgbGamut(gamutMap);
        Assert.That(gamutMapped.Hex, Is.EqualTo(expected));
    }
    
    [Test, Combinatorial]
    public void ExtremeValues(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double value,
        [ValueSource(nameof(GamutMaps))] GamutMap gamutMap)
    {
        var colourSpace = gamutMap switch
        {
            GamutMap.RgbClipping => ColourSpace.Rgb,
            GamutMap.OklchChromaReduction => ColourSpace.Oklch,
            GamutMap.WxyPurityReduction => ColourSpace.Wxy,
            _ => throw new ArgumentOutOfRangeException(nameof(gamutMap), gamutMap, null)
        };
        
        // if extreme values are being used for the colour space in which mapping takes place
        // mapping should still return an in-gamut colour, with the exception of NaNs
        var original = new Unicolour(colourSpace, value, value, value);
        var gamutMapped = original.MapToRgbGamut(gamutMap);
        Assert.That(gamutMapped.IsInRgbGamut, gamutMapped.Rgb.UseAsNaN ? Is.False : Is.True);
    }
    
    private static ColourTriplet MakeOutOfGamut(ColourTriplet triplet) => new(MakeOutOfGamut(triplet.First), MakeOutOfGamut(triplet.Second), MakeOutOfGamut(triplet.Third));
    private static double MakeOutOfGamut(double x) => Math.Sign(x) * (Math.Abs(x) + 1);
}