namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

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
        var fromRgbBlack = rgbBlack.Mix(ColourSpace.Hsl, green, 0.5, false);
        var fromRgbWhite = rgbWhite.Mix(ColourSpace.Hsl, green, 0.5, false);
        var fromHslBlack = hslBlack.Mix(ColourSpace.Hsl, green, 0.5, false);
        var fromHslWhite = hslWhite.Mix(ColourSpace.Hsl, green, 0.5, false);

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
        var toRgbBlack = blue.Mix(ColourSpace.Hsl, rgbBlack, 0.5, false);
        var toRgbWhite = blue.Mix(ColourSpace.Hsl, rgbWhite, 0.5, false);
        var toHslBlack = blue.Mix(ColourSpace.Hsl, hslBlack, 0.5, false);
        var toHslWhite = blue.Mix(ColourSpace.Hsl, hslWhite, 0.5, false);

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

        var blackToWhite = black.Mix(ColourSpace.Hsl, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Hsl, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Hsl, grey, 0.5, false);
        
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

        var blackToWhite = black.Mix(ColourSpace.Hsl, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Hsl, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Hsl, grey, 0.5, false);
        
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