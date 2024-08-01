using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale TSL has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
public class MixGreyscaleTslTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var tslBlack = new Unicolour(ColourSpace.Tsl, 180, 1, 0); // no lightness = black
        var tslWhite = new Unicolour(ColourSpace.Tsl, 180, 0, 1); // no saturation = greyscale
        
        var green = new Unicolour(ColourSpace.Tsl, 120, 1, 1);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Tsl, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Tsl, premultiplyAlpha: false);
        var fromTslBlack = tslBlack.Mix(green, ColourSpace.Tsl, premultiplyAlpha: false);
        var fromTslWhite = tslWhite.Mix(green, ColourSpace.Tsl, premultiplyAlpha: false);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Tsl.Triplet, new(120, 0.5, 0.5));
        AssertTriplet(fromRgbWhite.Tsl.Triplet, new(120, 0.5, 1));
        AssertTriplet(fromTslBlack.Tsl.Triplet, new(150, 1, 0.5));
        AssertTriplet(fromTslWhite.Tsl.Triplet, new(150, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var tslBlack = new Unicolour(ColourSpace.Tsl, 180, 1, 0); // no brightness = black
        var tslWhite = new Unicolour(ColourSpace.Tsl, 180, 0, 1); // no saturation = greyscale
        
        var blue = new Unicolour(ColourSpace.Tsl, 240, 1, 1);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Tsl, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Tsl, premultiplyAlpha: false);
        var toTslBlack = blue.Mix(tslBlack, ColourSpace.Tsl, premultiplyAlpha: false);
        var toTslWhite = blue.Mix(tslWhite, ColourSpace.Tsl, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Tsl.Triplet, new(240, 0.5, 0.5));
        AssertTriplet(toRgbWhite.Tsl.Triplet, new(240, 0.5, 1));
        AssertTriplet(toTslBlack.Tsl.Triplet, new(210, 1, 0.5));
        AssertTriplet(toTslWhite.Tsl.Triplet, new(210, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = new Unicolour(ColourSpace.Rgb, 0.0, 0.0, 0.0);
        var white = new Unicolour(ColourSpace.Rgb, 1.0, 1.0, 1.0);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Tsl, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Tsl, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Tsl, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertTriplet(blackToWhite.Tsl.Triplet, new(0, 0, 0.5));
        AssertTriplet(blackToGrey.Tsl.Triplet, new(0, 0, 0.25));
        AssertTriplet(whiteToGrey.Tsl.Triplet, new(0, 0, 0.75));
    }
    
    [Test]
    public void GreyscaleBothTslColours()
    {
        var black = new Unicolour(ColourSpace.Tsl, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Tsl, 300, 0, 1.0);
        var grey = new Unicolour(ColourSpace.Tsl, 100, 0, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Tsl, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Tsl, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Tsl, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from TSL therefore hue changes
        AssertTriplet(blackToWhite.Tsl.Triplet, new(330, 0, 0.5));
        AssertTriplet(blackToGrey.Tsl.Triplet, new(50, 0, 0.25));
        AssertTriplet(whiteToGrey.Tsl.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}