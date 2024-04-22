namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale Oklab has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Okhsv has a hue so it should be used (it just can't be seen until there is some saturation & value)
public class MixGreyscaleOkhsvTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var okhsvBlack = new Unicolour(ColourSpace.Okhsv, 180, 1, 0); // no brightness = black
        var okhsvWhite = new Unicolour(ColourSpace.Okhsv, 180, 0, 1); // no saturation = greyscale
        
        var green = new Unicolour(ColourSpace.Okhsv, 120, 1, 1);
        var fromOklabBlack = oklabBlack.Mix(green, ColourSpace.Okhsv, premultiplyAlpha: false);
        var fromOklabWhite = oklabWhite.Mix(green, ColourSpace.Okhsv, premultiplyAlpha: false);
        var fromOkhsvBlack = okhsvBlack.Mix(green, ColourSpace.Okhsv, premultiplyAlpha: false);
        var fromOkhsvWhite = okhsvWhite.Mix(green, ColourSpace.Okhsv, premultiplyAlpha: false);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromOklabBlack.Okhsv.Triplet, new(120, 0.5, 0.5));
        AssertTriplet(fromOklabWhite.Okhsv.Triplet, new(120, 0.5, 1));
        AssertTriplet(fromOkhsvBlack.Okhsv.Triplet, new(150, 1, 0.5));
        AssertTriplet(fromOkhsvWhite.Okhsv.Triplet, new(150, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var okhsvBlack = new Unicolour(ColourSpace.Okhsv, 180, 1, 0); // no brightness = black
        var okhsvWhite = new Unicolour(ColourSpace.Okhsv, 180, 0, 1); // no saturation = greyscale
        
        var blue = new Unicolour(ColourSpace.Okhsv, 240, 1, 1);
        var toOklabBlack = blue.Mix(oklabBlack, ColourSpace.Okhsv, premultiplyAlpha: false);
        var toOklabWhite = blue.Mix(oklabWhite, ColourSpace.Okhsv, premultiplyAlpha: false);
        var toOkhsvBlack = blue.Mix(okhsvBlack, ColourSpace.Okhsv, premultiplyAlpha: false);
        var toOkhsvWhite = blue.Mix(okhsvWhite, ColourSpace.Okhsv, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toOklabBlack.Okhsv.Triplet, new(240, 0.5, 0.5));
        AssertTriplet(toOklabWhite.Okhsv.Triplet, new(240, 0.5, 1));
        AssertTriplet(toOkhsvBlack.Okhsv.Triplet, new(210, 1, 0.5));
        AssertTriplet(toOkhsvWhite.Okhsv.Triplet, new(210, 0.5, 1));
    }
    
    [Test]
    public void GreyscaleBothOklabColours()
    {
        var black = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var grey = new Unicolour(ColourSpace.Oklab, 0.5, 0, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Okhsv, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Okhsv, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Okhsv, premultiplyAlpha: false);

        double GetL(double first, double second) => Okhsv.ToeInverse((Okhsv.Toe(first) + Okhsv.Toe(second)) / 2.0);
        AssertTriplet(blackToWhite.Oklab.Triplet, new(GetL(0, 1), 0, 0));
        AssertTriplet(blackToGrey.Oklab.Triplet, new(GetL(0, 0.5), 0, 0));
        AssertTriplet(whiteToGrey.Oklab.Triplet, new(GetL(0.5, 1), 0, 0));
        
        // colours created from Oklab therefore hue does not change
        double GetV(double first, double second) => Okhsv.Toe(GetL(first, second));
        AssertTriplet(blackToWhite.Okhsv.Triplet, new(0, 0, GetV(0, 1)));
        AssertTriplet(blackToGrey.Okhsv.Triplet, new(0, 0, GetV(0, 0.5)));
        AssertTriplet(whiteToGrey.Okhsv.Triplet, new(0, 0, GetV(0.5, 1)));
    }
    
    [Test]
    public void GreyscaleBothOkhsvColours()
    {
        var black = new Unicolour(ColourSpace.Okhsv, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Okhsv, 300, 0, 1.0);
        var grey = new Unicolour(ColourSpace.Okhsv, 100, 0, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Okhsv, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Okhsv, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Okhsv, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Oklab.Triplet, new(Okhsv.ToeInverse(0.5), 0, 0));
        AssertTriplet(blackToGrey.Oklab.Triplet, new(Okhsv.ToeInverse(0.25), 0, 0));
        AssertTriplet(whiteToGrey.Oklab.Triplet, new(Okhsv.ToeInverse(0.75), 0, 0));
        
        // colours created from HSB therefore hue changes
        AssertTriplet(blackToWhite.Okhsv.Triplet, new(330, 0, 0.5));
        AssertTriplet(blackToGrey.Okhsv.Triplet, new(50, 0, 0.25));
        AssertTriplet(whiteToGrey.Okhsv.Triplet, new(20, 0, 0.75));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, 0.0000001);
    }
}