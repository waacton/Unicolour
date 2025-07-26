using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Munsell has a hue so it should be used (it just can't be seen until there is some value & chroma)
public class MixGreyscaleMunsellTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var munsellBlack = new Unicolour(ColourSpace.Munsell, 180, 0, 20); // no value = black
        var munsellWhite = new Unicolour(ColourSpace.Munsell, 180, 10, 0); // no chroma = greyscale
        
        var green = new Unicolour(ColourSpace.Munsell, 120, 10, 20);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Munsell, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Munsell, premultiplyAlpha: false);
        var fromMunsellBlack = munsellBlack.Mix(green, ColourSpace.Munsell, premultiplyAlpha: false);
        var fromMunsellWhite = munsellWhite.Mix(green, ColourSpace.Munsell, premultiplyAlpha: false);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Munsell.Triplet, new(120, 5, 10));
        AssertTriplet(fromRgbWhite.Munsell.Triplet, new(120, 10, 10));
        AssertTriplet(fromMunsellBlack.Munsell.Triplet, new(150, 5, 20));
        AssertTriplet(fromMunsellWhite.Munsell.Triplet, new(150, 10, 10));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var munsellBlack = new Unicolour(ColourSpace.Munsell, 180, 0, 20); // no value = black
        var munsellWhite = new Unicolour(ColourSpace.Munsell, 180, 10, 0); // no chroma = greyscale
        
        var blue = new Unicolour(ColourSpace.Munsell, 240, 10, 20);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Munsell, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Munsell, premultiplyAlpha: false);
        var toMunsellBlack = blue.Mix(munsellBlack, ColourSpace.Munsell, premultiplyAlpha: false);
        var toMunsellWhite = blue.Mix(munsellWhite, ColourSpace.Munsell, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Munsell.Triplet, new(240, 5, 10));
        AssertTriplet(toRgbWhite.Munsell.Triplet, new(240, 10, 10));
        AssertTriplet(toMunsellBlack.Munsell.Triplet, new(210, 5, 20));
        AssertTriplet(toMunsellWhite.Munsell.Triplet, new(210, 10, 10));
    }
    
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = new Unicolour(ColourSpace.RgbLinear, 0.0, 0.0, 0.0);
        var white = new Unicolour(ColourSpace.RgbLinear, 1.0, 1.0, 1.0);
        var grey = new Unicolour(ColourSpace.RgbLinear, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Munsell, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Munsell, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Munsell, premultiplyAlpha: false);

        AssertTriplet(blackToWhite.RgbLinear.Triplet, new(0.1927184375, 0.1927184375, 0.1927184375));
        AssertTriplet(blackToGrey.RgbLinear.Triplet, new(0.1028100921, 0.1028100921, 0.1028100921));
        AssertTriplet(whiteToGrey.RgbLinear.Triplet, new(0.719449643153, 0.719449643153, 0.719449643153));
        
        // colours created from RGB therefore hue does not change
        AssertTriplet(blackToWhite.Munsell.Triplet, new(0, 5, 0));
        AssertTriplet(blackToGrey.Munsell.Triplet, new(0, 3.768860022566, 0));
        AssertTriplet(whiteToGrey.Munsell.Triplet, new(0, 8.768860022566, 0));
    }
    
    [Test]
    public void GreyscaleBothMunsellColours()
    {
        var black = new Unicolour(ColourSpace.Munsell, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Munsell, 300, 10, 0);
        var grey = new Unicolour(ColourSpace.Munsell, 100, 5, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Munsell, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Munsell, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Munsell, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.RgbLinear.Triplet, new(0.1927184375, 0.1927184375, 0.1927184375));
        AssertTriplet(blackToGrey.RgbLinear.Triplet, new(0.0449879980468, 0.0449879980468, 0.0449879980468));
        AssertTriplet(whiteToGrey.RgbLinear.Triplet, new(0.4940879003906, 0.4940879003906, 0.4940879003906));
        
        // colours created from WXY therefore hue changes
        AssertTriplet(blackToWhite.Munsell.Triplet, new(330, 5, 0));
        AssertTriplet(blackToGrey.Munsell.Triplet, new(50, 2.5, 0));
        AssertTriplet(whiteToGrey.Munsell.Triplet, new(20, 7.5, 0));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        var actualWithDegree = actual.WithDegreeMap(ToDegree).WithHueModulo();
        TestUtils.AssertTriplet(actualWithDegree, expected, TestUtils.MixTolerance);
    }
    
    private static double ToWavelength(double wavelength) => wavelength;
    private static double ToDegree(double wavelength) => wavelength;
}