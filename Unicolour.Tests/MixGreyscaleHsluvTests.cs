namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// note: HSLuv is a transformation from LCHuv, therefore there is no obvious rectangular/hueless space to compare against
// so using RGB for greyscale -> colour behaviour, and LUV for greyscale -> greyscale behaviour
// ----------
// greyscale RGB & LUV have no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale HSLuv has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
public class MixGreyscaleHsluvTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hsluvBlack = new Unicolour(ColourSpace.Hsluv, 180, 100, 0); // no lightness = black
        var hsluvWhite = new Unicolour(ColourSpace.Hsluv, 180, 0, 100); // no saturation = greyscale
        
        var green = new Unicolour(ColourSpace.Hsluv, 120, 100, 50);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Hsluv, 0.5, false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Hsluv, 0.5, false);
        var fromHsluvBlack = hsluvBlack.Mix(green, ColourSpace.Hsluv, 0.5, false);
        var fromHsluvWhite = hsluvWhite.Mix(green, ColourSpace.Hsluv, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Hsluv.Triplet, new(120, 50, 25));
        AssertTriplet(fromRgbWhite.Hsluv.Triplet, new(120, 50, 75));
        AssertTriplet(fromHsluvBlack.Hsluv.Triplet, new(150, 100, 25));
        AssertTriplet(fromHsluvWhite.Hsluv.Triplet, new(150, 50, 75));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hsluvBlack = new Unicolour(ColourSpace.Hsluv, 180, 100, 0); // no brightness = black
        var hsluvWhite = new Unicolour(ColourSpace.Hsluv, 180, 0, 100); // no saturation = greyscale
        
        var blue = new Unicolour(ColourSpace.Hsluv, 240, 100, 50);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Hsluv, 0.5, false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Hsluv, 0.5, false);
        var toHsluvBlack = blue.Mix(hsluvBlack, ColourSpace.Hsluv, 0.5, false);
        var toHsluvWhite = blue.Mix(hsluvWhite, ColourSpace.Hsluv, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Hsluv.Triplet, new(240, 50, 25));
        AssertTriplet(toRgbWhite.Hsluv.Triplet, new(240, 50, 75));
        AssertTriplet(toHsluvBlack.Hsluv.Triplet, new(210, 100, 25));
        AssertTriplet(toHsluvWhite.Hsluv.Triplet, new(210, 50, 75));
    }

    [Test]
    public void GreyscaleBothLuvColours()
    {
        var black = new Unicolour(ColourSpace.Luv, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Luv, 100, 0, 0);
        var grey = new Unicolour(ColourSpace.Luv, 50, 0, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Hsluv, 0.5, false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hsluv, 0.5, false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hsluv, 0.5, false);
        
        AssertTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LUV therefore hue does not change
        AssertTriplet(blackToWhite.Hsluv.Triplet, new(0, 0, 50));
        AssertTriplet(blackToGrey.Hsluv.Triplet, new(0, 0, 25));
        AssertTriplet(whiteToGrey.Hsluv.Triplet, new(0, 0, 75));
    }
    
    [Test]
    public void GreyscaleBothHsluvColours()
    {
        var black = new Unicolour(ColourSpace.Hsluv, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Hsluv, 300, 0, 100);
        var grey = new Unicolour(ColourSpace.Hsluv, 100, 0, 50);

        var blackToWhite = black.Mix(white, ColourSpace.Hsluv, 0.5, false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hsluv, 0.5, false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hsluv, 0.5, false);
        
        AssertTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from HSLuv therefore hue changes
        AssertTriplet(blackToWhite.Lchuv.Triplet, new(50, 0, 330));
        AssertTriplet(blackToGrey.Lchuv.Triplet, new(25, 0, 50));
        AssertTriplet(whiteToGrey.Lchuv.Triplet, new(75, 0, 20));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}