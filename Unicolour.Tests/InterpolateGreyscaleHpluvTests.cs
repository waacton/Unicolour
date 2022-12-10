namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// note: HPLuv is a transformation from LCHuv, therefore there is no obvious cartesian/hueless space to compare against
// so using RGB for greyscale -> colour behaviour, and LUV for greyscale -> greyscale behaviour
// ----------
// greyscale RGB & LUV have no hue - shouldn't assume to start at red (0 degrees) when interpolating
// greyscale HPLuv has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
public class InterpolateGreyscaleHpluvTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hpluvBlack = Unicolour.FromHpluv(180, 100, 0); // no lightness = black
        var hpluvWhite = Unicolour.FromHpluv(180, 0, 100); // no saturation = greyscale
        
        var green = Unicolour.FromHpluv(120, 100, 50);
        var fromRgbBlack = rgbBlack.InterpolateHpluv(green, 0.5);
        var fromRgbWhite = rgbWhite.InterpolateHpluv(green, 0.5);
        var fromHpluvBlack = hpluvBlack.InterpolateHpluv(green, 0.5);
        var fromHpluvWhite = hpluvWhite.InterpolateHpluv(green, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since greyscale RGB assumes saturation of 0 (but saturation can be any value)
        AssertColourTriplet(fromRgbBlack.Hpluv.Triplet, new(120, 50, 25));
        AssertColourTriplet(fromRgbWhite.Hpluv.Triplet, new(120, 50, 75));
        AssertColourTriplet(fromHpluvBlack.Hpluv.Triplet, new(150, 100, 25));
        AssertColourTriplet(fromHpluvWhite.Hpluv.Triplet, new(150, 50, 75));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = Unicolour.FromRgb255(0, 0, 0);
        var rgbWhite = Unicolour.FromRgb255(255, 255, 255);
        var hpluvBlack = Unicolour.FromHpluv(180, 100, 0); // no brightness = black
        var hpluvWhite = Unicolour.FromHpluv(180, 0, 100); // no saturation = greyscale
        
        var blue = Unicolour.FromHpluv(240, 100, 50);
        var toRgbBlack = blue.InterpolateHpluv(rgbBlack, 0.5);
        var toRgbWhite = blue.InterpolateHpluv(rgbWhite, 0.5);
        var toHpluvBlack = blue.InterpolateHpluv(hpluvBlack, 0.5);
        var toHpluvWhite = blue.InterpolateHpluv(hpluvWhite, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since greyscale RGB assumes saturation of 0 (but saturation can be any value)
        AssertColourTriplet(toRgbBlack.Hpluv.Triplet, new(240, 50, 25));
        AssertColourTriplet(toRgbWhite.Hpluv.Triplet, new(240, 50, 75));
        AssertColourTriplet(toHpluvBlack.Hpluv.Triplet, new(210, 100, 25));
        AssertColourTriplet(toHpluvWhite.Hpluv.Triplet, new(210, 50, 75));
    }

    [Test]
    public void GreyscaleBothLuvColours()
    {
        var black = Unicolour.FromLuv(0, 0, 0);
        var white = Unicolour.FromLuv(100, 0, 0);
        var grey = Unicolour.FromLuv(50, 0, 0);

        var blackToWhite = black.InterpolateHpluv(white, 0.5);
        var blackToGrey = black.InterpolateHpluv(grey, 0.5);
        var whiteToGrey = white.InterpolateHpluv(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertColourTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertColourTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LUV therefore hue does not change
        AssertColourTriplet(blackToWhite.Hpluv.Triplet, new(0, 0, 50));
        Assert.That(blackToWhite.Hpluv.IsEffectivelyHued, Is.False);
        AssertColourTriplet(blackToGrey.Hpluv.Triplet, new(0, 0, 25));
        Assert.That(blackToGrey.Hpluv.IsEffectivelyHued, Is.False);
        AssertColourTriplet(whiteToGrey.Hpluv.Triplet, new(0, 0, 75));
        Assert.That(whiteToGrey.Hpluv.IsEffectivelyHued, Is.False);
    }
    
    [Test]
    public void GreyscaleBothHpluvColours()
    {
        var black = Unicolour.FromHpluv(0, 0, 0);
        var white = Unicolour.FromHpluv(300, 0, 100);
        var grey = Unicolour.FromHpluv(100, 0, 50);

        var blackToWhite = black.InterpolateHpluv(white, 0.5);
        var blackToGrey = black.InterpolateHpluv(grey, 0.5);
        var whiteToGrey = white.InterpolateHpluv(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertColourTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertColourTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from HPLuv therefore hue changes
        AssertColourTriplet(blackToWhite.Lchuv.Triplet, new(50, 0, 330));
        AssertColourTriplet(blackToGrey.Lchuv.Triplet, new(25, 0, 50));
        AssertColourTriplet(whiteToGrey.Lchuv.Triplet, new(75, 0, 20));
    }

    private static void AssertColourTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        AssertUtils.AssertColourTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}