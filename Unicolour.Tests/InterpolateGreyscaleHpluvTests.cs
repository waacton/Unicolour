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
        AssertTriplet(fromRgbBlack.Hpluv.Triplet, new(120, 50, 25));
        AssertTriplet(fromRgbWhite.Hpluv.Triplet, new(120, 50, 75));
        AssertTriplet(fromHpluvBlack.Hpluv.Triplet, new(150, 100, 25));
        AssertTriplet(fromHpluvWhite.Hpluv.Triplet, new(150, 50, 75));
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
        AssertTriplet(toRgbBlack.Hpluv.Triplet, new(240, 50, 25));
        AssertTriplet(toRgbWhite.Hpluv.Triplet, new(240, 50, 75));
        AssertTriplet(toHpluvBlack.Hpluv.Triplet, new(210, 100, 25));
        AssertTriplet(toHpluvWhite.Hpluv.Triplet, new(210, 50, 75));
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
        
        AssertTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LUV therefore hue does not change
        AssertTriplet(blackToWhite.Hpluv.Triplet, new(0, 0, 50));
        Assert.That(blackToWhite.Hpluv.UseAsHued, Is.False);
        AssertTriplet(blackToGrey.Hpluv.Triplet, new(0, 0, 25));
        Assert.That(blackToGrey.Hpluv.UseAsHued, Is.False);
        AssertTriplet(whiteToGrey.Hpluv.Triplet, new(0, 0, 75));
        Assert.That(whiteToGrey.Hpluv.UseAsHued, Is.False);
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
        AssertUtils.AssertTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}