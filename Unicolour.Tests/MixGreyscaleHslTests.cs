using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale HSL has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
public class MixGreyscaleHslTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hslBlack = new Unicolour(ColourSpace.Hsl, 180, 1, 0); // no lightness = black
        var hslWhite = new Unicolour(ColourSpace.Hsl, 180, 0, 1); // no saturation = greyscale
        
        var green = new Unicolour(ColourSpace.Hsl, 120, 1, 0.5);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Hsl, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Hsl, premultiplyAlpha: false);
        var fromHslBlack = hslBlack.Mix(green, ColourSpace.Hsl, premultiplyAlpha: false);
        var fromHslWhite = hslWhite.Mix(green, ColourSpace.Hsl, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Hsl.Triplet, new(120, 0.5, 0.25));
        AssertTriplet(fromRgbWhite.Hsl.Triplet, new(120, 0.5, 0.75));
        AssertTriplet(fromHslBlack.Hsl.Triplet, new(150, 1, 0.25));
        AssertTriplet(fromHslWhite.Hsl.Triplet, new(150, 0.5, 0.75));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hslBlack = new Unicolour(ColourSpace.Hsl, 180, 1, 0); // no brightness = black
        var hslWhite = new Unicolour(ColourSpace.Hsl, 180, 0, 1); // no saturation = greyscale
        
        var blue = new Unicolour(ColourSpace.Hsl, 240, 1, 0.5);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Hsl, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Hsl, premultiplyAlpha: false);
        var toHslBlack = blue.Mix(hslBlack, ColourSpace.Hsl, premultiplyAlpha: false);
        var toHslWhite = blue.Mix(hslWhite, ColourSpace.Hsl, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Hsl.Triplet, new(240, 0.5, 0.25));
        AssertTriplet(toRgbWhite.Hsl.Triplet, new(240, 0.5, 0.75));
        AssertTriplet(toHslBlack.Hsl.Triplet, new(210, 1, 0.25));
        AssertTriplet(toHslWhite.Hsl.Triplet, new(210, 0.5, 0.75));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = new Unicolour(ColourSpace.Rgb, 0.0, 0.0, 0.0);
        var white = new Unicolour(ColourSpace.Rgb, 1.0, 1.0, 1.0);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Hsl, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hsl, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hsl, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertTriplet(blackToWhite.Hsl.Triplet, new(0, 0, 0.5));
        AssertTriplet(blackToGrey.Hsl.Triplet, new(0, 0, 0.25));
        AssertTriplet(whiteToGrey.Hsl.Triplet, new(0, 0, 0.75));
    }
    
    [Test]
    public void GreyscaleBothHslColours()
    {
        var black = new Unicolour(ColourSpace.Hsl, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Hsl, 300, 0, 1.0);
        var grey = new Unicolour(ColourSpace.Hsl, 100, 0, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Hsl, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hsl, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hsl, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from HSL therefore hue changes
        AssertTriplet(blackToWhite.Hsl.Triplet, new(330, 0, 0.5));
        AssertTriplet(blackToGrey.Hsl.Triplet, new(50, 0, 0.25));
        AssertTriplet(whiteToGrey.Hsl.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}