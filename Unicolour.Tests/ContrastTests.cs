namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class ContrastTests
{
    [Test]
    public static void KnownContrasts()
    {
        var black = ColourLimits.Rgb[ColourLimit.Black];
        var white = ColourLimits.Rgb[ColourLimit.White];
        var red = ColourLimits.Rgb[ColourLimit.Red];
        var green = ColourLimits.Rgb[ColourLimit.Green];
        var blue = ColourLimits.Rgb[ColourLimit.Blue];
        var random = RandomColours.UnicolourFromRgb();
        
        AssertKnownContrast(black, white, 21);
        AssertKnownContrast(red, green, 2.91);
        AssertKnownContrast(green, blue, 6.26);
        AssertKnownContrast(blue, red, 2.15);
        AssertKnownContrast(random, random, 1);
    }

    [Test]
    public static void UnconstrainedRgbLuminance()
    {
        var black = ColourLimits.Rgb[ColourLimit.Black];
        var white = ColourLimits.Rgb[ColourLimit.White];
        var beyondMinRgb = Unicolour.FromRgb(-0.25, -0.5, -0.75);
        var beyondMaxRgb = Unicolour.FromRgb(1.25, 1.5, 1.75);
        
        Assert.That(beyondMinRgb.RelativeLuminance, Is.EqualTo(black.RelativeLuminance));
        Assert.That(beyondMaxRgb.RelativeLuminance, Is.EqualTo(white.RelativeLuminance));
    }
    
    [Test]
    public static void NaNContrast()
    {
        var notNumber = Unicolour.FromRgb(double.NaN, double.NaN, double.NaN);
        var grey = Unicolour.FromRgb(0.5, 0.5, 0.5);
        
        AssertKnownContrast(notNumber, grey, double.NaN);
        AssertKnownContrast(grey, notNumber, double.NaN);
        AssertKnownContrast(notNumber, notNumber, double.NaN);
    }
    
    private static void AssertKnownContrast(Unicolour colour1, Unicolour colour2, double expectedContrast)
    {
        var delta1 = colour1.Contrast(colour2);
        var delta2 = colour2.Contrast(colour1);
        Assert.That(delta1, Is.EqualTo(expectedContrast).Within(0.005));
        Assert.That(delta1, Is.EqualTo(delta2));
    }
}