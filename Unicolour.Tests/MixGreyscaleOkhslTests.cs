using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale Oklab has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Okhsl has a hue so it should be used (it just can't be seen until there is some saturation & lightness)
public class MixGreyscaleOkhslTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var okhslBlack = new Unicolour(ColourSpace.Okhsl, 180, 1, 0); // no lightness = black
        var okhslWhite = new Unicolour(ColourSpace.Okhsl, 180, 0, 1); // no saturation = greyscale
        
        var green = new Unicolour(ColourSpace.Okhsl, 120, 1, 0.5);
        var fromOklabBlack = oklabBlack.Mix(green, ColourSpace.Okhsl, premultiplyAlpha: false);
        var fromOklabWhite = oklabWhite.Mix(green, ColourSpace.Okhsl, premultiplyAlpha: false);
        var fromOkhslBlack = okhslBlack.Mix(green, ColourSpace.Okhsl, premultiplyAlpha: false);
        var fromOkhslWhite = okhslWhite.Mix(green, ColourSpace.Okhsl, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromOklabBlack.Okhsl.Triplet, new(120, 0.5, 0.25));
        AssertTriplet(fromOklabWhite.Okhsl.Triplet, new(120, 0.5, 0.75));
        AssertTriplet(fromOkhslBlack.Okhsl.Triplet, new(150, 1, 0.25));
        AssertTriplet(fromOkhslWhite.Okhsl.Triplet, new(150, 0.5, 0.75));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var okhslBlack = new Unicolour(ColourSpace.Okhsl, 180, 1, 0); // no brightness = black
        var okhslWhite = new Unicolour(ColourSpace.Okhsl, 180, 0, 1); // no saturation = greyscale
        
        var blue = new Unicolour(ColourSpace.Okhsl, 240, 1, 0.5);
        var toOklabBlack = blue.Mix(oklabBlack, ColourSpace.Okhsl, premultiplyAlpha: false);
        var toOklabWhite = blue.Mix(oklabWhite, ColourSpace.Okhsl, premultiplyAlpha: false);
        var toOkhslBlack = blue.Mix(okhslBlack, ColourSpace.Okhsl, premultiplyAlpha: false);
        var toOkhslWhite = blue.Mix(okhslWhite, ColourSpace.Okhsl, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toOklabBlack.Okhsl.Triplet, new(240, 0.5, 0.25));
        AssertTriplet(toOklabWhite.Okhsl.Triplet, new(240, 0.5, 0.75));
        AssertTriplet(toOkhslBlack.Okhsl.Triplet, new(210, 1, 0.25));
        AssertTriplet(toOkhslWhite.Okhsl.Triplet, new(210, 0.5, 0.75));
    }
    
    [Test]
    public void GreyscaleBothOklabColours()
    {
        var black = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var grey = new Unicolour(ColourSpace.Oklab, 0.5, 0, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Okhsl, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Okhsl, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Okhsl, premultiplyAlpha: false);
        
        double GetL(double first, double second) => Okhsv.ToeInverse((Okhsv.Toe(first) + Okhsv.Toe(second)) / 2.0);
        AssertTriplet(blackToWhite.Oklab.Triplet, new(GetL(0, 1), 0, 0));
        AssertTriplet(blackToGrey.Oklab.Triplet, new(GetL(0, 0.5), 0, 0));
        AssertTriplet(whiteToGrey.Oklab.Triplet, new(GetL(0.5, 1), 0, 0));
        
        // colours created from Oklab therefore hue does not change
        double GetV(double first, double second) => Okhsv.Toe(GetL(first, second));
        AssertTriplet(blackToWhite.Okhsl.Triplet, new(0, 0, GetV(0, 1)));
        AssertTriplet(blackToGrey.Okhsl.Triplet, new(0, 0, GetV(0, 0.5)));
        AssertTriplet(whiteToGrey.Okhsl.Triplet, new(0, 0, GetV(0.5, 1)));
    }
    
    [Test]
    public void GreyscaleBothOkhslColours()
    {
        var black = new Unicolour(ColourSpace.Okhsl, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Okhsl, 300, 0, 1.0);
        var grey = new Unicolour(ColourSpace.Okhsl, 100, 0, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Okhsl, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Okhsl, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Okhsl, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Oklab.Triplet, new(Okhsv.ToeInverse(0.5), 0, 0));
        AssertTriplet(blackToGrey.Oklab.Triplet, new(Okhsv.ToeInverse(0.25), 0, 0));
        AssertTriplet(whiteToGrey.Oklab.Triplet, new(Okhsv.ToeInverse(0.75), 0, 0));
        
        // colours created from Okhsl therefore hue changes
        AssertTriplet(blackToWhite.Okhsl.Triplet, new(330, 0, 0.5));
        AssertTriplet(blackToGrey.Okhsl.Triplet, new(50, 0, 0.25));
        AssertTriplet(whiteToGrey.Okhsl.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}