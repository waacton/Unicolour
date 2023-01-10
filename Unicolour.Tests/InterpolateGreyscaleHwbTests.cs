namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// greyscale HWB has a hue so it should be used (it just can't be seen while whiteness + blackness >= 1)
public class InterpolateGreyscaleHwbTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hwbBlack = Unicolour.FromHwb(180, 0, 1); // full blackness = black
        var hwbWhite = Unicolour.FromHwb(180, 1, 0); // full whiteness = white
        
        var green = Unicolour.FromHwb(120, 0, 0);
        var fromRgbBlack = rgbBlack.InterpolateHwb(green, 0.5);
        var fromRgbWhite = rgbWhite.InterpolateHwb(green, 0.5);
        var fromHwbBlack = hwbBlack.InterpolateHwb(green, 0.5);
        var fromHwbWhite = hwbWhite.InterpolateHwb(green, 0.5);
        
        // greyscale interpolates differently depending on the initial colour space
        // since greyscale RGB assumes saturation of 0 (but saturation can be any value)
        AssertColourTriplet(fromRgbBlack.Hwb.Triplet, new(120, 0, 0.5));
        AssertColourTriplet(fromRgbWhite.Hwb.Triplet, new(120, 0.5, 0));
        AssertColourTriplet(fromHwbBlack.Hwb.Triplet, new(150, 0, 0.5));
        AssertColourTriplet(fromHwbWhite.Hwb.Triplet, new(150, 0.5, 0));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hwbBlack = Unicolour.FromHwb(180, 0, 1); // full blackness = black
        var hwbWhite = Unicolour.FromHwb(180, 1, 0); // full whiteness = white
        
        var blue = Unicolour.FromHwb(240, 0, 0);
        var toRgbBlack = blue.InterpolateHwb(rgbBlack, 0.5);
        var toRgbWhite = blue.InterpolateHwb(rgbWhite, 0.5);
        var toHwbBlack = blue.InterpolateHwb(hwbBlack, 0.5);
        var toHwbWhite = blue.InterpolateHwb(hwbWhite, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since greyscale RGB assumes saturation of 0 (but saturation can be any value)
        AssertColourTriplet(toRgbBlack.Hwb.Triplet, new(240, 0, 0.5));
        AssertColourTriplet(toRgbWhite.Hwb.Triplet, new(240, 0.5, 0));
        AssertColourTriplet(toHwbBlack.Hwb.Triplet, new(210, 0, 0.5));
        AssertColourTriplet(toHwbWhite.Hwb.Triplet, new(210, 0.5, 0));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = Unicolour.FromRgb(0.0, 0.0, 0.0);
        var white = Unicolour.FromRgb(1.0, 1.0, 1.0);
        var grey = Unicolour.FromRgb(0.5, 0.5, 0.5);

        var blackToWhite = black.InterpolateHwb(white, 0.5);
        var blackToGrey = black.InterpolateHwb(grey, 0.5);
        var whiteToGrey = white.InterpolateHwb(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertColourTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertColourTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertColourTriplet(blackToWhite.Hwb.Triplet, new(0, 0.5, 0.5));
        AssertColourTriplet(blackToGrey.Hwb.Triplet, new(0, 0.25, 0.75));
        AssertColourTriplet(whiteToGrey.Hwb.Triplet, new(0, 0.75, 0.25));
    }
    
    [Test]
    public void GreyscaleBothHwbColours()
    {
        var black = Unicolour.FromHwb(0, 0, 1.0);
        var white = Unicolour.FromHwb(300, 1.0, 0);
        var grey = Unicolour.FromHwb(100, 0.5, 0.5);

        var blackToWhite = black.InterpolateHwb(white, 0.5);
        var blackToGrey = black.InterpolateHwb(grey, 0.5);
        var whiteToGrey = white.InterpolateHwb(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertColourTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertColourTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from hwb therefore hue changes
        AssertColourTriplet(blackToWhite.Hwb.Triplet, new(330, 0.5, 0.5));
        AssertColourTriplet(blackToGrey.Hwb.Triplet, new(50, 0.25, 0.75));
        AssertColourTriplet(whiteToGrey.Hwb.Triplet, new(20, 0.75, 0.25));
    }
    
    private static void AssertColourTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        AssertUtils.AssertColourTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}