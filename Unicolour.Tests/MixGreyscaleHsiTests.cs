using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale HSI has a hue so it should be used (it just can't be seen until there is some saturation & brightness)
public class MixGreyscaleHsiTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hsiBlack = new Unicolour(ColourSpace.Hsi, 180, 1, 0); // no brightness = black
        var hsiWhite = new Unicolour(ColourSpace.Hsi, 180, 0, 1); // no intensity = greyscale
        
        var green = new Unicolour(ColourSpace.Hsi, 120, 1, 1);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Hsi, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Hsi, premultiplyAlpha: false);
        var fromHsiBlack = hsiBlack.Mix(green, ColourSpace.Hsi, premultiplyAlpha: false);
        var fromHsiWhite = hsiWhite.Mix(green, ColourSpace.Hsi, premultiplyAlpha: false);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Hsi.Triplet, new(120, 0.5, 0.5));
        AssertTriplet(fromRgbWhite.Hsi.Triplet, new(120, 0.5, 1));
        AssertTriplet(fromHsiBlack.Hsi.Triplet, new(150, 1, 0.5));
        AssertTriplet(fromHsiWhite.Hsi.Triplet, new(150, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hsiBlack = new Unicolour(ColourSpace.Hsi, 180, 1, 0); // no brightness = black
        var hsiWhite = new Unicolour(ColourSpace.Hsi, 180, 0, 1); // no saturation = greyscale
        
        var blue = new Unicolour(ColourSpace.Hsi, 240, 1, 1);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Hsi, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Hsi, premultiplyAlpha: false);
        var toHsiBlack = blue.Mix(hsiBlack, ColourSpace.Hsi, premultiplyAlpha: false);
        var toHsiWhite = blue.Mix(hsiWhite, ColourSpace.Hsi, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Hsi.Triplet, new(240, 0.5, 0.5));
        AssertTriplet(toRgbWhite.Hsi.Triplet, new(240, 0.5, 1));
        AssertTriplet(toHsiBlack.Hsi.Triplet, new(210, 1, 0.5));
        AssertTriplet(toHsiWhite.Hsi.Triplet, new(210, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = new Unicolour(ColourSpace.Rgb, 0.0, 0.0, 0.0);
        var white = new Unicolour(ColourSpace.Rgb, 1.0, 1.0, 1.0);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Hsi, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hsi, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hsi, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertTriplet(blackToWhite.Hsi.Triplet, new(0, 0, 0.5));
        AssertTriplet(blackToGrey.Hsi.Triplet, new(0, 0, 0.25));
        AssertTriplet(whiteToGrey.Hsi.Triplet, new(0, 0, 0.75));
    }
    
    [Test]
    public void GreyscaleBothHsiColours()
    {
        var black = new Unicolour(ColourSpace.Hsi, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Hsi, 300, 0, 1.0);
        var grey = new Unicolour(ColourSpace.Hsi, 100, 0, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Hsi, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hsi, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hsi, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from HSI therefore hue changes
        AssertTriplet(blackToWhite.Hsi.Triplet, new(330, 0, 0.5));
        AssertTriplet(blackToGrey.Hsi.Triplet, new(50, 0, 0.25));
        AssertTriplet(whiteToGrey.Hsi.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}