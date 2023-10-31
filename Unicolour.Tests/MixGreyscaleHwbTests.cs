namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale HWB has a hue so it should be used (it just can't be seen while whiteness + blackness >= 1)
public class MixGreyscaleHwbTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hwbBlack = Unicolour.FromHwb(180, 0, 1); // full blackness = black
        var hwbWhite = Unicolour.FromHwb(180, 1, 0); // full whiteness = white
        
        var green = Unicolour.FromHwb(120, 0, 0);
        var fromRgbBlack = rgbBlack.MixHwb(green, 0.5);
        var fromRgbWhite = rgbWhite.MixHwb(green, 0.5);
        var fromHwbBlack = hwbBlack.MixHwb(green, 0.5);
        var fromHwbWhite = hwbWhite.MixHwb(green, 0.5);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Hwb.Triplet, new(120, 0, 0.5));
        AssertTriplet(fromRgbWhite.Hwb.Triplet, new(120, 0.5, 0));
        AssertTriplet(fromHwbBlack.Hwb.Triplet, new(150, 0, 0.5));
        AssertTriplet(fromHwbWhite.Hwb.Triplet, new(150, 0.5, 0));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hwbBlack = Unicolour.FromHwb(180, 0, 1); // full blackness = black
        var hwbWhite = Unicolour.FromHwb(180, 1, 0); // full whiteness = white
        
        var blue = Unicolour.FromHwb(240, 0, 0);
        var toRgbBlack = blue.MixHwb(rgbBlack, 0.5);
        var toRgbWhite = blue.MixHwb(rgbWhite, 0.5);
        var toHwbBlack = blue.MixHwb(hwbBlack, 0.5);
        var toHwbWhite = blue.MixHwb(hwbWhite, 0.5);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Hwb.Triplet, new(240, 0, 0.5));
        AssertTriplet(toRgbWhite.Hwb.Triplet, new(240, 0.5, 0));
        AssertTriplet(toHwbBlack.Hwb.Triplet, new(210, 0, 0.5));
        AssertTriplet(toHwbWhite.Hwb.Triplet, new(210, 0.5, 0));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = Unicolour.FromRgb(0.0, 0.0, 0.0);
        var white = Unicolour.FromRgb(1.0, 1.0, 1.0);
        var grey = Unicolour.FromRgb(0.5, 0.5, 0.5);

        var blackToWhite = black.MixHwb(white, 0.5);
        var blackToGrey = black.MixHwb(grey, 0.5);
        var whiteToGrey = white.MixHwb(grey, 0.5);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertTriplet(blackToWhite.Hwb.Triplet, new(0, 0.5, 0.5));
        AssertTriplet(blackToGrey.Hwb.Triplet, new(0, 0.25, 0.75));
        AssertTriplet(whiteToGrey.Hwb.Triplet, new(0, 0.75, 0.25));
    }
    
    [Test]
    public void GreyscaleBothHwbColours()
    {
        var black = Unicolour.FromHwb(0, 0, 1.0);
        var white = Unicolour.FromHwb(300, 1.0, 0);
        var grey = Unicolour.FromHwb(100, 0.5, 0.5);

        var blackToWhite = black.MixHwb(white, 0.5);
        var blackToGrey = black.MixHwb(grey, 0.5);
        var whiteToGrey = white.MixHwb(grey, 0.5);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from hwb therefore hue changes
        AssertTriplet(blackToWhite.Hwb.Triplet, new(330, 0.5, 0.5));
        AssertTriplet(blackToGrey.Hwb.Triplet, new(50, 0.25, 0.75));
        AssertTriplet(whiteToGrey.Hwb.Triplet, new(20, 0.75, 0.25));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        AssertUtils.AssertTriplet(actual, expected, AssertUtils.MixTolerance);
    }
}