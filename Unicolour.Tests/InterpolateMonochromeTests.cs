namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour;

public class InterpolateMonochromeTests
{
    [Test]
    // monochrome RGB has no hue - shouldn't assume to start at red (0 degrees) when interpolating
    // monochrome HSB has a hue so it should be used (it just can't be seen until there is some saturation & brightness)
    public void MonochromeStartColour()
    {
        var rgbBlack = Unicolour.FromRgb(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb(255, 255, 255);
        var hsbBlack = Unicolour.FromHsb(180, 1, 0); // no brightness = black
        var hsbWhite = Unicolour.FromHsb(180, 0, 1); // no saturation = greyscale
        var green = Unicolour.FromHsb(120, 1, 1);
        
        var interpolatedFromRgbBlack = rgbBlack.InterpolateHsb(green, 0.5);
        var interpolatedFromRgbWhite = rgbWhite.InterpolateHsb(green, 0.5);
        var interpolatedFromHsbBlack = hsbBlack.InterpolateHsb(green, 0.5);
        var interpolatedFromHsbWhite = hsbWhite.InterpolateHsb(green, 0.5);

        // note that "RGB black" interpolates differently to the "HSB black"
        // since monochrome RGB assumes saturation of 0 (but saturation can be any value)
        AssertHsb(interpolatedFromRgbBlack.Hsb, (120, 0.5, 0.5));
        AssertHsb(interpolatedFromRgbWhite.Hsb, (120, 0.5, 1));
        AssertHsb(interpolatedFromHsbBlack.Hsb, (150, 1, 0.5));
        AssertHsb(interpolatedFromHsbWhite.Hsb, (150, 0.5, 1));
    }
    
    [Test]
    // monochrome RGB has no hue - shouldn't assume to end at red (0 degrees) when interpolating
    // monochrome HSB has a hue so it should be used (it just can't be seen until there is some saturation & brightness)
    public void MonochromeEndColour()
    {
        var rgbBlack = Unicolour.FromRgb(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb(255, 255, 255);
        var hsbBlack = Unicolour.FromHsb(180, 1, 0); // no brightness = black
        var hsbWhite = Unicolour.FromHsb(180, 0, 1); // no saturation = greyscale
        var blue = Unicolour.FromHsb(240, 1, 1);
        
        var interpolatedToRgbBlack = blue.InterpolateHsb(rgbBlack, 0.5);
        var interpolatedToRgbWhite = blue.InterpolateHsb(rgbWhite, 0.5);
        var interpolatedToHsbBlack = blue.InterpolateHsb(hsbBlack, 0.5);
        var interpolatedToHsbWhite = blue.InterpolateHsb(hsbWhite, 0.5);

        // note that "RGB black" interpolates differently to the "HSB black"
        // since monochrome RGB assumes saturation of 0 (but saturation can be any value)
        AssertHsb(interpolatedToRgbBlack.Hsb, (240, 0.5, 0.5));
        AssertHsb(interpolatedToRgbWhite.Hsb, (240, 0.5, 1));
        AssertHsb(interpolatedToHsbBlack.Hsb, (210, 1, 0.5));
        AssertHsb(interpolatedToHsbWhite.Hsb, (210, 0.5, 1));
    }
    
    [Test]
    // monochrome RGB has no hue, so it should be ignored when interpolating
    public void MonochromeBothRgbColours()
    {
        var black = Unicolour.FromRgb(0.0, 0.0, 0.0);
        var white = Unicolour.FromRgb(1.0, 1.0, 1.0);
        var grey = Unicolour.FromRgb(0.5, 0.5, 0.5);

        var blackToWhite = black.InterpolateHsb(white, 0.5);
        var blackToGrey = black.InterpolateHsb(grey, 0.5);
        var whiteToGrey = white.InterpolateHsb(grey, 0.5);
        
        AssertRgb(blackToWhite.Rgb, (0.5, 0.5, 0.5));
        AssertRgb(blackToGrey.Rgb, (0.25, 0.25, 0.25));
        AssertRgb(whiteToGrey.Rgb, (0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertHsb(blackToWhite.Hsb, (0, 0, 0.5));
        AssertHsb(blackToGrey.Hsb, (0, 0, 0.25));
        AssertHsb(whiteToGrey.Hsb, (0, 0, 0.75));
    }
    
    [Test]
    // monochrome HSB has a hue so it should be used when interpolating
    public void MonochromeBothHsbColours()
    {
        var black = Unicolour.FromHsb(0, 0, 0);
        var white = Unicolour.FromHsb(300, 0, 1.0);
        var grey = Unicolour.FromHsb(100, 0, 0.5);

        var blackToWhite = black.InterpolateHsb(white, 0.5);
        var blackToGrey = black.InterpolateHsb(grey, 0.5);
        var whiteToGrey = white.InterpolateHsb(grey, 0.5);
        
        AssertRgb(blackToWhite.Rgb, (0.5, 0.5, 0.5));
        AssertRgb(blackToGrey.Rgb, (0.25, 0.25, 0.25));
        AssertRgb(whiteToGrey.Rgb, (0.75, 0.75, 0.75));
        
        // colours created from HSB therefore hue changes
        AssertHsb(blackToWhite.Hsb, (330, 0, 0.5));
        AssertHsb(blackToGrey.Hsb, (50, 0, 0.25));
        AssertHsb(whiteToGrey.Hsb, (20, 0, 0.75));
    }

    private static void AssertHsb(Hsb actualHsb, (double h, double s, double b) expectedHsb)
    {
        Assert.That(actualHsb.H, Is.EqualTo(expectedHsb.h).Within(0.00000000005));
        Assert.That(actualHsb.S, Is.EqualTo(expectedHsb.s).Within(0.00000000005));
        Assert.That(actualHsb.B, Is.EqualTo(expectedHsb.b).Within(0.00000000005));
    }
    
    private static void AssertRgb(Rgb actualRgb, (double r, double g, double b) expectedRgb)
    {
        Assert.That(actualRgb.R, Is.EqualTo(expectedRgb.r).Within(0.00000000005));
        Assert.That(actualRgb.G, Is.EqualTo(expectedRgb.g).Within(0.00000000005));
        Assert.That(actualRgb.B, Is.EqualTo(expectedRgb.b).Within(0.00000000005));
    }
}