namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// note: HPLuv is a transformation from LCHuv, therefore there is no obvious rectangular/hueless space to compare against
// so using RGB for greyscale -> colour behaviour, and LUV for greyscale -> greyscale behaviour
// ----------
// greyscale RGB & LUV have no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale HPLuv has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
public class MixGreyscaleHpluvTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hpluvBlack = new Unicolour(ColourSpace.Hpluv, 180, 100, 0); // no lightness = black
        var hpluvWhite = new Unicolour(ColourSpace.Hpluv, 180, 0, 100); // no saturation = greyscale
        
        var green = new Unicolour(ColourSpace.Hpluv, 120, 100, 50);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Hpluv, 0.5, false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Hpluv, 0.5, false);
        var fromHpluvBlack = hpluvBlack.Mix(green, ColourSpace.Hpluv, 0.5, false);
        var fromHpluvWhite = hpluvWhite.Mix(green, ColourSpace.Hpluv, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Hpluv.Triplet, new(120, 50, 25));
        AssertTriplet(fromRgbWhite.Hpluv.Triplet, new(120, 50, 75));
        AssertTriplet(fromHpluvBlack.Hpluv.Triplet, new(150, 100, 25));
        AssertTriplet(fromHpluvWhite.Hpluv.Triplet, new(150, 50, 75));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hpluvBlack = new Unicolour(ColourSpace.Hpluv, 180, 100, 0); // no brightness = black
        var hpluvWhite = new Unicolour(ColourSpace.Hpluv, 180, 0, 100); // no saturation = greyscale
        
        var blue = new Unicolour(ColourSpace.Hpluv, 240, 100, 50);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Hpluv, 0.5, false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Hpluv, 0.5, false);
        var toHpluvBlack = blue.Mix(hpluvBlack, ColourSpace.Hpluv, 0.5, false);
        var toHpluvWhite = blue.Mix(hpluvWhite, ColourSpace.Hpluv, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Hpluv.Triplet, new(240, 50, 25));
        AssertTriplet(toRgbWhite.Hpluv.Triplet, new(240, 50, 75));
        AssertTriplet(toHpluvBlack.Hpluv.Triplet, new(210, 100, 25));
        AssertTriplet(toHpluvWhite.Hpluv.Triplet, new(210, 50, 75));
    }

    [Test]
    public void GreyscaleBothLuvColours()
    {
        var black = new Unicolour(ColourSpace.Luv, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Luv, 100, 0, 0);
        var grey = new Unicolour(ColourSpace.Luv, 50, 0, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Hpluv, 0.5, false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hpluv, 0.5, false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hpluv, 0.5, false);
        
        AssertTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LUV therefore hue does not change
        AssertTriplet(blackToWhite.Hpluv.Triplet, new(0, 0, 50));
        AssertTriplet(blackToGrey.Hpluv.Triplet, new(0, 0, 25));
        AssertTriplet(whiteToGrey.Hpluv.Triplet, new(0, 0, 75));
    }
    
    [Test]
    public void GreyscaleBothHpluvColours()
    {
        var black = new Unicolour(ColourSpace.Hpluv, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Hpluv, 300, 0, 100);
        var grey = new Unicolour(ColourSpace.Hpluv, 100, 0, 50);

        var blackToWhite = black.Mix(white, ColourSpace.Hpluv, 0.5, false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hpluv, 0.5, false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hpluv, 0.5, false);
        
        AssertTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from HPLuv therefore hue changes
        AssertTriplet(blackToWhite.Lchuv.Triplet, new(50, 0, 330));
        AssertTriplet(blackToGrey.Lchuv.Triplet, new(25, 0, 50));
        AssertTriplet(whiteToGrey.Lchuv.Triplet, new(75, 0, 20));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}