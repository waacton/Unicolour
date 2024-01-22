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
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hwbBlack = new Unicolour(ColourSpace.Hwb, 180, 0, 1); // full blackness = black
        var hwbWhite = new Unicolour(ColourSpace.Hwb, 180, 1, 0); // full whiteness = white
        
        var green = new Unicolour(ColourSpace.Hwb, 120, 0, 0);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Hwb, 0.5, false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Hwb, 0.5, false);
        var fromHwbBlack = hwbBlack.Mix(green, ColourSpace.Hwb, 0.5, false);
        var fromHwbWhite = hwbWhite.Mix(green, ColourSpace.Hwb, 0.5, false);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Hwb.Triplet, new(120, 0, 0.5));
        AssertTriplet(fromRgbWhite.Hwb.Triplet, new(120, 0.5, 0));
        AssertTriplet(fromHwbBlack.Hwb.Triplet, new(150, 0, 0.5));
        AssertTriplet(fromHwbWhite.Hwb.Triplet, new(150, 0.5, 0));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hwbBlack = new Unicolour(ColourSpace.Hwb, 180, 0, 1); // full blackness = black
        var hwbWhite = new Unicolour(ColourSpace.Hwb, 180, 1, 0); // full whiteness = white
        
        var blue = new Unicolour(ColourSpace.Hwb, 240, 0, 0);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Hwb, 0.5, false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Hwb, 0.5, false);
        var toHwbBlack = blue.Mix(hwbBlack, ColourSpace.Hwb, 0.5, false);
        var toHwbWhite = blue.Mix(hwbWhite, ColourSpace.Hwb, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Hwb.Triplet, new(240, 0, 0.5));
        AssertTriplet(toRgbWhite.Hwb.Triplet, new(240, 0.5, 0));
        AssertTriplet(toHwbBlack.Hwb.Triplet, new(210, 0, 0.5));
        AssertTriplet(toHwbWhite.Hwb.Triplet, new(210, 0.5, 0));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = new Unicolour(ColourSpace.Rgb, 0.0, 0.0, 0.0);
        var white = new Unicolour(ColourSpace.Rgb, 1.0, 1.0, 1.0);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Hwb, 0.5, false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hwb, 0.5, false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hwb, 0.5, false);
        
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
        var black = new Unicolour(ColourSpace.Hwb, 0, 0, 1.0);
        var white = new Unicolour(ColourSpace.Hwb, 300, 1.0, 0);
        var grey = new Unicolour(ColourSpace.Hwb, 100, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Hwb, 0.5, false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hwb, 0.5, false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hwb, 0.5, false);
        
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
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}