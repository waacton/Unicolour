namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// monochrome RGB has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// monochrome HSL has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
public class InterpolateMonochromeHslTests
{
    [Test]
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

        // monochrome interpolates differently depending on the initial colour space
        // since monochrome RGB assumes saturation of 0 (but saturation can be any value)
        AssertColourTriplet(fromRgbBlack.Hsl.Triplet, new(120, 0.5, 0.25));
        AssertColourTriplet(fromRgbWhite.Hsl.Triplet, new(120, 0.5, 0.75));
        AssertColourTriplet(fromHslBlack.Hsl.Triplet, new(150, 1, 0.25));
        AssertColourTriplet(fromHslWhite.Hsl.Triplet, new(150, 0.5, 0.75));
    }
    
    [Test]
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

        // monochrome interpolates differently depending on the initial colour space
        // since monochrome RGB assumes saturation of 0 (but saturation can be any value)
        AssertColourTriplet(toRgbBlack.Hsl.Triplet, new(240, 0.5, 0.25));
        AssertColourTriplet(toRgbWhite.Hsl.Triplet, new(240, 0.5, 0.75));
        AssertColourTriplet(toHslBlack.Hsl.Triplet, new(210, 1, 0.25));
        AssertColourTriplet(toHslWhite.Hsl.Triplet, new(210, 0.5, 0.75));
    }
    
    [Test]
    public void MonochromeBothRgbColours()
    {
        var black = Unicolour.FromRgb(0.0, 0.0, 0.0);
        var white = Unicolour.FromRgb(1.0, 1.0, 1.0);
        var grey = Unicolour.FromRgb(0.5, 0.5, 0.5);

        var blackToWhite = black.InterpolateHsl(white, 0.5);
        var blackToGrey = black.InterpolateHsl(grey, 0.5);
        var whiteToGrey = white.InterpolateHsl(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertColourTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertColourTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertColourTriplet(blackToWhite.Hsl.Triplet, new(0, 0, 0.5));
        AssertColourTriplet(blackToGrey.Hsl.Triplet, new(0, 0, 0.25));
        AssertColourTriplet(whiteToGrey.Hsl.Triplet, new(0, 0, 0.75));
    }
    
    [Test]
    public void MonochromeBothHslColours()
    {
        var black = Unicolour.FromHsl(0, 0, 0);
        var white = Unicolour.FromHsl(300, 0, 1.0);
        var grey = Unicolour.FromHsl(100, 0, 0.5);

        var blackToWhite = black.InterpolateHsl(white, 0.5);
        var blackToGrey = black.InterpolateHsl(grey, 0.5);
        var whiteToGrey = white.InterpolateHsl(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertColourTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertColourTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from HSL therefore hue changes
        AssertColourTriplet(blackToWhite.Hsl.Triplet, new(330, 0, 0.5));
        AssertColourTriplet(blackToGrey.Hsl.Triplet, new(50, 0, 0.25));
        AssertColourTriplet(whiteToGrey.Hsl.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertColourTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        AssertUtils.AssertColourTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}