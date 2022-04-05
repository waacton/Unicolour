namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public class InterpolateMonochromeHslTests
{
    [Test]
    // monochrome RGB has no hue - shouldn't assume to start at red (0 degrees) when interpolating
    // monochrome HSL has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
    public void MonochromeStartColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hslBlack = Unicolour.FromHsl(180, 1, 0); // no lightness = black
        var hslWhite = Unicolour.FromHsl(180, 0, 1); // no saturation = greyscale
        
        var green = Unicolour.FromHsl(120, 1, 0.5);
        var fromRgbBlack = rgbBlack.InterpolateHsl(green, 0.5);
        var fromRgbWhite = rgbWhite.InterpolateHsl(green, 0.5);
        var fromHslBlack = hslBlack.InterpolateHsl(green, 0.5);
        var fromHslWhite = hslWhite.InterpolateHsl(green, 0.5);

        // note that "RGB black" interpolates differently to the "HSL black"
        // since monochrome RGB assumes saturation of 0 (but saturation can be any value)
        AssertHsl(fromRgbBlack.Hsl, (120, 0.5, 0.25));
        AssertHsl(fromRgbWhite.Hsl, (120, 0.5, 0.75));
        AssertHsl(fromHslBlack.Hsl, (150, 1, 0.25));
        AssertHsl(fromHslWhite.Hsl, (150, 0.5, 0.75));
    }
    
    [Test]
    // monochrome RGB has no hue - shouldn't assume to end at red (0 degrees) when interpolating
    // monochrome HSL has a hue so it should be used (it just can't be seen until there is some saturation & brightness)
    public void MonochromeEndColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hslBlack = Unicolour.FromHsl(180, 1, 0); // no brightness = black
        var hslWhite = Unicolour.FromHsl(180, 0, 1); // no saturation = greyscale
        
        var blue = Unicolour.FromHsl(240, 1, 0.5);
        var toRgbBlack = blue.InterpolateHsl(rgbBlack, 0.5);
        var toRgbWhite = blue.InterpolateHsl(rgbWhite, 0.5);
        var toHslBlack = blue.InterpolateHsl(hslBlack, 0.5);
        var toHslWhite = blue.InterpolateHsl(hslWhite, 0.5);

        // note that "RGB black" interpolates differently to the "HSL black"
        // since monochrome RGB assumes saturation of 0 (but saturation can be any value)
        AssertHsl(toRgbBlack.Hsl, (240, 0.5, 0.25));
        AssertHsl(toRgbWhite.Hsl, (240, 0.5, 0.75));
        AssertHsl(toHslBlack.Hsl, (210, 1, 0.25));
        AssertHsl(toHslWhite.Hsl, (210, 0.5, 0.75));
    }
    
    [Test]
    // monochrome RGB has no hue, so it should be ignored when interpolating
    public void MonochromeBothRgbColours()
    {
        var black = Unicolour.FromRgb(0.0, 0.0, 0.0);
        var white = Unicolour.FromRgb(1.0, 1.0, 1.0);
        var grey = Unicolour.FromRgb(0.5, 0.5, 0.5);

        var blackToWhite = black.InterpolateHsl(white, 0.5);
        var blackToGrey = black.InterpolateHsl(grey, 0.5);
        var whiteToGrey = white.InterpolateHsl(grey, 0.5);
        
        AssertRgb(blackToWhite.Rgb, (0.5, 0.5, 0.5));
        AssertRgb(blackToGrey.Rgb, (0.25, 0.25, 0.25));
        AssertRgb(whiteToGrey.Rgb, (0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertHsl(blackToWhite.Hsl, (0, 0, 0.5));
        AssertHsl(blackToGrey.Hsl, (0, 0, 0.25));
        AssertHsl(whiteToGrey.Hsl, (0, 0, 0.75));
    }
    
    [Test]
    // monochrome HSL has a hue so it should be used when interpolating
    public void MonochromeBothHslColours()
    {
        var black = Unicolour.FromHsl(0, 0, 0);
        var white = Unicolour.FromHsl(300, 0, 1.0);
        var grey = Unicolour.FromHsl(100, 0, 0.5);

        var blackToWhite = black.InterpolateHsl(white, 0.5);
        var blackToGrey = black.InterpolateHsl(grey, 0.5);
        var whiteToGrey = white.InterpolateHsl(grey, 0.5);
        
        AssertRgb(blackToWhite.Rgb, (0.5, 0.5, 0.5));
        AssertRgb(blackToGrey.Rgb, (0.25, 0.25, 0.25));
        AssertRgb(whiteToGrey.Rgb, (0.75, 0.75, 0.75));
        
        // colours created from HSL therefore hue changes
        AssertHsl(blackToWhite.Hsl, (330, 0, 0.5));
        AssertHsl(blackToGrey.Hsl, (50, 0, 0.25));
        AssertHsl(whiteToGrey.Hsl, (20, 0, 0.75));
    }
    
    private static void AssertRgb(Rgb actualRgb, (double r, double g, double b) expectedRgb)
    {
        Assert.That(actualRgb.R, Is.EqualTo(expectedRgb.r).Within(0.00000000005));
        Assert.That(actualRgb.G, Is.EqualTo(expectedRgb.g).Within(0.00000000005));
        Assert.That(actualRgb.B, Is.EqualTo(expectedRgb.b).Within(0.00000000005));
    }

    private static void AssertHsl(Hsl actualHsl, (double h, double s, double l) expectedHsl)
    {
        Assert.That(actualHsl.H, Is.EqualTo(expectedHsl.h).Within(0.00000000005));
        Assert.That(actualHsl.S, Is.EqualTo(expectedHsl.s).Within(0.00000000005));
        Assert.That(actualHsl.L, Is.EqualTo(expectedHsl.l).Within(0.00000000005));
    }
}