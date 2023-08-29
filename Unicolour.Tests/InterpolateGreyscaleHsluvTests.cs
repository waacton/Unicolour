namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// note: HSLuv is a transformation from LCHuv, therefore there is no obvious cartesian/hueless space to compare against
// so using RGB for greyscale -> colour behaviour, and LUV for greyscale -> greyscale behaviour
// ----------
// greyscale RGB & LUV have no hue - shouldn't assume to start at red (0 degrees) when interpolating
// greyscale HSLuv has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
public class InterpolateGreyscaleHsluvTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hsluvBlack = Unicolour.FromHsluv(180, 100, 0); // no lightness = black
        var hsluvWhite = Unicolour.FromHsluv(180, 0, 100); // no saturation = greyscale
        
        var green = Unicolour.FromHsluv(120, 100, 50);
        var fromRgbBlack = rgbBlack.InterpolateHsluv(green, 0.5);
        var fromRgbWhite = rgbWhite.InterpolateHsluv(green, 0.5);
        var fromHsluvBlack = hsluvBlack.InterpolateHsluv(green, 0.5);
        var fromHsluvWhite = hsluvWhite.InterpolateHsluv(green, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since greyscale RGB assumes saturation of 0 (but saturation can be any value)
        AssertTriplet(fromRgbBlack.Hsluv.Triplet, new(120, 50, 25));
        AssertTriplet(fromRgbWhite.Hsluv.Triplet, new(120, 50, 75));
        AssertTriplet(fromHsluvBlack.Hsluv.Triplet, new(150, 100, 25));
        AssertTriplet(fromHsluvWhite.Hsluv.Triplet, new(150, 50, 75));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hsluvBlack = Unicolour.FromHsluv(180, 100, 0); // no brightness = black
        var hsluvWhite = Unicolour.FromHsluv(180, 0, 100); // no saturation = greyscale
        
        var blue = Unicolour.FromHsluv(240, 100, 50);
        var toRgbBlack = blue.InterpolateHsluv(rgbBlack, 0.5);
        var toRgbWhite = blue.InterpolateHsluv(rgbWhite, 0.5);
        var toHsluvBlack = blue.InterpolateHsluv(hsluvBlack, 0.5);
        var toHsluvWhite = blue.InterpolateHsluv(hsluvWhite, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since greyscale RGB assumes saturation of 0 (but saturation can be any value)
        AssertTriplet(toRgbBlack.Hsluv.Triplet, new(240, 50, 25));
        AssertTriplet(toRgbWhite.Hsluv.Triplet, new(240, 50, 75));
        AssertTriplet(toHsluvBlack.Hsluv.Triplet, new(210, 100, 25));
        AssertTriplet(toHsluvWhite.Hsluv.Triplet, new(210, 50, 75));
    }

    [Test]
    public void GreyscaleBothLuvColours()
    {
        var black = Unicolour.FromLuv(0, 0, 0);
        var white = Unicolour.FromLuv(100, 0, 0);
        var grey = Unicolour.FromLuv(50, 0, 0);

        var blackToWhite = black.InterpolateHsluv(white, 0.5);
        var blackToGrey = black.InterpolateHsluv(grey, 0.5);
        var whiteToGrey = white.InterpolateHsluv(grey, 0.5);
        
        AssertTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LUV therefore hue does not change
        AssertTriplet(blackToWhite.Hsluv.Triplet, new(0, 0, 50));
        Assert.That(blackToWhite.Hsluv.UseAsHued, Is.False);
        AssertTriplet(blackToGrey.Hsluv.Triplet, new(0, 0, 25));
        Assert.That(blackToGrey.Hsluv.UseAsHued, Is.False);
        AssertTriplet(whiteToGrey.Hsluv.Triplet, new(0, 0, 75));
        Assert.That(whiteToGrey.Hsluv.UseAsHued, Is.False);
    }
    
    [Test]
    public void GreyscaleBothHsluvColours()
    {
        var black = Unicolour.FromHsluv(0, 0, 0);
        var white = Unicolour.FromHsluv(300, 0, 100);
        var grey = Unicolour.FromHsluv(100, 0, 50);

        var blackToWhite = black.InterpolateHsluv(white, 0.5);
        var blackToGrey = black.InterpolateHsluv(grey, 0.5);
        var whiteToGrey = white.InterpolateHsluv(grey, 0.5);
        
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
        AssertUtils.AssertTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}