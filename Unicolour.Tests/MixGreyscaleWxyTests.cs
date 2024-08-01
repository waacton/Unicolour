using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale WXY has a hue so it should be used (it just can't be seen until there is some purity & luminance)
public class MixGreyscaleWxyTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var wxyBlack = new Unicolour(ColourSpace.Wxy, ToWavelength(180), 1, 0); // no luminance = black
        var wxyWhite = new Unicolour(ColourSpace.Wxy, ToWavelength(180), 0, 1); // no purity = greyscale
        
        var green = new Unicolour(ColourSpace.Wxy, ToWavelength(120), 1, 1);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Wxy, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Wxy, premultiplyAlpha: false);
        var fromWxyBlack = wxyBlack.Mix(green, ColourSpace.Wxy, premultiplyAlpha: false);
        var fromWxyWhite = wxyWhite.Mix(green, ColourSpace.Wxy, premultiplyAlpha: false);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Wxy.Triplet, new(120, 0.5, 0.5));
        AssertTriplet(fromRgbWhite.Wxy.Triplet, new(120, 0.5, 1));
        AssertTriplet(fromWxyBlack.Wxy.Triplet, new(150, 1, 0.5));
        AssertTriplet(fromWxyWhite.Wxy.Triplet, new(150, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var wxyBlack = new Unicolour(ColourSpace.Wxy, ToWavelength(180), 1, 0); // no luminance = black
        var wxyWhite = new Unicolour(ColourSpace.Wxy, ToWavelength(180), 0, 1); // no purity = greyscale
        
        var blue = new Unicolour(ColourSpace.Wxy, ToWavelength(240), 1, 1);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Wxy, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Wxy, premultiplyAlpha: false);
        var toWxyBlack = blue.Mix(wxyBlack, ColourSpace.Wxy, premultiplyAlpha: false);
        var toWxyWhite = blue.Mix(wxyWhite, ColourSpace.Wxy, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Wxy.Triplet, new(240, 0.5, 0.5));
        AssertTriplet(toRgbWhite.Wxy.Triplet, new(240, 0.5, 1));
        AssertTriplet(toWxyBlack.Wxy.Triplet, new(210, 1, 0.5));
        AssertTriplet(toWxyWhite.Wxy.Triplet, new(210, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = new Unicolour(ColourSpace.RgbLinear, 0.0, 0.0, 0.0);
        var white = new Unicolour(ColourSpace.RgbLinear, 1.0, 1.0, 1.0);
        var grey = new Unicolour(ColourSpace.RgbLinear, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Wxy, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Wxy, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Wxy, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.RgbLinear.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.RgbLinear.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.RgbLinear.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from RGB therefore hue does not change
        AssertTriplet(blackToWhite.Wxy.Triplet, new(0, 0, 0.5));
        AssertTriplet(blackToGrey.Wxy.Triplet, new(0, 0, 0.25));
        AssertTriplet(whiteToGrey.Wxy.Triplet, new(0, 0, 0.75));
    }
    
    [Test]
    public void GreyscaleBothWxyColours()
    {
        var black = new Unicolour(ColourSpace.Wxy, ToWavelength(0), 0, 0);
        var white = new Unicolour(ColourSpace.Wxy, ToWavelength(300), 0, 1.0);
        var grey = new Unicolour(ColourSpace.Wxy, ToWavelength(100), 0, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Wxy, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Wxy, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Wxy, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.RgbLinear.Triplet, new(0.5, 0.5, 0.5));
        AssertTriplet(blackToGrey.RgbLinear.Triplet, new(0.25, 0.25, 0.25));
        AssertTriplet(whiteToGrey.RgbLinear.Triplet, new(0.75, 0.75, 0.75));
        
        // colours created from WXY therefore hue changes
        AssertTriplet(blackToWhite.Wxy.Triplet, new(330, 0, 0.5));
        AssertTriplet(blackToGrey.Wxy.Triplet, new(50, 0, 0.25));
        AssertTriplet(whiteToGrey.Wxy.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        var actualWithDegree = actual.WithDegreeMap(ToDegree).WithHueModulo();
        TestUtils.AssertTriplet(actualWithDegree, expected, TestUtils.MixTolerance);
    }
    
    private static double ToWavelength(double wavelength) => Wxy.DegreeToWavelength(wavelength, XyzConfiguration.D65);
    private static double ToDegree(double wavelength) => Wxy.WavelengthToDegree(wavelength, XyzConfiguration.D65);
}