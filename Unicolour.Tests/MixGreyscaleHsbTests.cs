namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale HSB has a hue so it should be used (it just can't be seen until there is some saturation & brightness)
public class MixGreyscaleHsbTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hsbBlack = new Unicolour(ColourSpace.Hsb, 180, 1, 0); // no brightness = black
        var hsbWhite = new Unicolour(ColourSpace.Hsb, 180, 0, 1); // no saturation = greyscale
        
        var green = new Unicolour(ColourSpace.Hsb, 120, 1, 1);
        var fromRgbBlack = rgbBlack.Mix(ColourSpace.Hsb, green, 0.5, false);
        var fromRgbWhite = rgbWhite.Mix(ColourSpace.Hsb, green, 0.5, false);
        var fromHsbBlack = hsbBlack.Mix(ColourSpace.Hsb, green, 0.5, false);
        var fromHsbWhite = hsbWhite.Mix(ColourSpace.Hsb, green, 0.5, false);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Hsb.Triplet, new(120, 0.5, 0.5));
        AssertTriplet(fromRgbWhite.Hsb.Triplet, new(120, 0.5, 1));
        AssertTriplet(fromHsbBlack.Hsb.Triplet, new(150, 1, 0.5));
        AssertTriplet(fromHsbWhite.Hsb.Triplet, new(150, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hsbBlack = new Unicolour(ColourSpace.Hsb, 180, 1, 0); // no brightness = black
        var hsbWhite = new Unicolour(ColourSpace.Hsb, 180, 0, 1); // no saturation = greyscale
        
        var blue = new Unicolour(ColourSpace.Hsb, 240, 1, 1);
        var toRgbBlack = blue.Mix(ColourSpace.Hsb, rgbBlack, 0.5, false);
        var toRgbWhite = blue.Mix(ColourSpace.Hsb, rgbWhite, 0.5, false);
        var toHsbBlack = blue.Mix(ColourSpace.Hsb, hsbBlack, 0.5, false);
        var toHsbWhite = blue.Mix(ColourSpace.Hsb, hsbWhite, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Hsb.Triplet, new(240, 0.5, 0.5));
        AssertTriplet(toRgbWhite.Hsb.Triplet, new(240, 0.5, 1));
        AssertTriplet(toHsbBlack.Hsb.Triplet, new(210, 1, 0.5));
        AssertTriplet(toHsbWhite.Hsb.Triplet, new(210, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = new Unicolour(ColourSpace.Rgb, 0.0, 0.0, 0.0);
        var white = new Unicolour(ColourSpace.Rgb, 1.0, 1.0, 1.0);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(ColourSpace.Hsb, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Hsb, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Hsb, grey, 0.5, false);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertTriplet(blackToWhite.Hsb.Triplet, new(0, 0, 0.5));
        AssertTriplet(blackToGrey.Hsb.Triplet, new(0, 0, 0.25));
        AssertTriplet(whiteToGrey.Hsb.Triplet, new(0, 0, 0.75));
    }
    
    [Test]
    public void GreyscaleBothHsbColours()
    {
        var black = new Unicolour(ColourSpace.Hsb, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Hsb, 300, 0, 1.0);
        var grey = new Unicolour(ColourSpace.Hsb, 100, 0, 0.5);

        var blackToWhite = black.Mix(ColourSpace.Hsb, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Hsb, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Hsb, grey, 0.5, false);
        
        AssertTriplet(blackToWhite.Rgb.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.Rgb.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.Rgb.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from HSB therefore hue changes
        AssertTriplet(blackToWhite.Hsb.Triplet, new(330, 0, 0.5));
        AssertTriplet(blackToGrey.Hsb.Triplet, new(50, 0, 0.25));
        AssertTriplet(whiteToGrey.Hsb.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}