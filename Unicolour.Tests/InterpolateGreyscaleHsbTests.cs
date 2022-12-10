namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// greyscale HSB has a hue so it should be used (it just can't be seen until there is some saturation & brightness)
public class InterpolateGreyscaleHsbTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hsbBlack = Unicolour.FromHsb(180, 1, 0); // no brightness = black
        var hsbWhite = Unicolour.FromHsb(180, 0, 1); // no saturation = greyscale
        
        var green = Unicolour.FromHsb(120, 1, 1);
        var fromRgbBlack = rgbBlack.InterpolateHsb(green, 0.5);
        var fromRgbWhite = rgbWhite.InterpolateHsb(green, 0.5);
        var fromHsbBlack = hsbBlack.InterpolateHsb(green, 0.5);
        var fromHsbWhite = hsbWhite.InterpolateHsb(green, 0.5);
        
        // greyscale interpolates differently depending on the initial colour space
        // since greyscale RGB assumes saturation of 0 (but saturation can be any value)
        AssertColourTriplet(fromRgbBlack.Hsb.Triplet, new(120, 0.5, 0.5));
        AssertColourTriplet(fromRgbWhite.Hsb.Triplet, new(120, 0.5, 1));
        AssertColourTriplet(fromHsbBlack.Hsb.Triplet, new(150, 1, 0.5));
        AssertColourTriplet(fromHsbWhite.Hsb.Triplet, new(150, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hsbBlack = Unicolour.FromHsb(180, 1, 0); // no brightness = black
        var hsbWhite = Unicolour.FromHsb(180, 0, 1); // no saturation = greyscale
        
        var blue = Unicolour.FromHsb(240, 1, 1);
        var toRgbBlack = blue.InterpolateHsb(rgbBlack, 0.5);
        var toRgbWhite = blue.InterpolateHsb(rgbWhite, 0.5);
        var toHsbBlack = blue.InterpolateHsb(hsbBlack, 0.5);
        var toHsbWhite = blue.InterpolateHsb(hsbWhite, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since greyscale RGB assumes saturation of 0 (but saturation can be any value)
        AssertColourTriplet(toRgbBlack.Hsb.Triplet, new(240, 0.5, 0.5));
        AssertColourTriplet(toRgbWhite.Hsb.Triplet, new(240, 0.5, 1));
        AssertColourTriplet(toHsbBlack.Hsb.Triplet, new(210, 1, 0.5));
        AssertColourTriplet(toHsbWhite.Hsb.Triplet, new(210, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = Unicolour.FromRgb(0.0, 0.0, 0.0);
        var white = Unicolour.FromRgb(1.0, 1.0, 1.0);
        var grey = Unicolour.FromRgb(0.5, 0.5, 0.5);

        var blackToWhite = black.InterpolateHsb(white, 0.5);
        var blackToGrey = black.InterpolateHsb(grey, 0.5);
        var whiteToGrey = white.InterpolateHsb(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertColourTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertColourTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertColourTriplet(blackToWhite.Hsb.Triplet, new(0, 0, 0.5));
        AssertColourTriplet(blackToGrey.Hsb.Triplet, new(0, 0, 0.25));
        AssertColourTriplet(whiteToGrey.Hsb.Triplet, new(0, 0, 0.75));
    }
    
    [Test]
    public void GreyscaleBothHsbColours()
    {
        var black = Unicolour.FromHsb(0, 0, 0);
        var white = Unicolour.FromHsb(300, 0, 1.0);
        var grey = Unicolour.FromHsb(100, 0, 0.5);

        var blackToWhite = black.InterpolateHsb(white, 0.5);
        var blackToGrey = black.InterpolateHsb(grey, 0.5);
        var whiteToGrey = white.InterpolateHsb(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertColourTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertColourTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from HSB therefore hue changes
        AssertColourTriplet(blackToWhite.Hsb.Triplet, new(330, 0, 0.5));
        AssertColourTriplet(blackToGrey.Hsb.Triplet, new(50, 0, 0.25));
        AssertColourTriplet(whiteToGrey.Hsb.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertColourTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        AssertUtils.AssertColourTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}