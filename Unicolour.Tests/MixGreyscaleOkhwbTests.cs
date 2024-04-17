namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale Oklab has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Okhwb has a hue so it should be used (it just can't be seen while whiteness + blackness >= 1)
public class MixGreyscaleOkhwbTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var okhwbBlack = new Unicolour(ColourSpace.Okhwb, 180, 0, 1); // full blackness = black
        var okhwbWhite = new Unicolour(ColourSpace.Okhwb, 180, 1, 0); // full whiteness = white
        
        var green = new Unicolour(ColourSpace.Okhwb, 120, 0, 0);
        var fromOklabBlack = oklabBlack.Mix(green, ColourSpace.Okhwb, premultiplyAlpha: false);
        var fromOklabWhite = oklabWhite.Mix(green, ColourSpace.Okhwb, premultiplyAlpha: false);
        var fromOkhwbBlack = okhwbBlack.Mix(green, ColourSpace.Okhwb, premultiplyAlpha: false);
        var fromOkhwbWhite = okhwbWhite.Mix(green, ColourSpace.Okhwb, premultiplyAlpha: false);
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromOklabBlack.Okhwb.Triplet, new(120, 0, 0.5));
        AssertTriplet(fromOklabWhite.Okhwb.Triplet, new(120, 0.5, 0));
        AssertTriplet(fromOkhwbBlack.Okhwb.Triplet, new(150, 0, 0.5));
        AssertTriplet(fromOkhwbWhite.Okhwb.Triplet, new(150, 0.5, 0));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var okhwbBlack = new Unicolour(ColourSpace.Okhwb, 180, 0, 1); // full blackness = black
        var okhwbWhite = new Unicolour(ColourSpace.Okhwb, 180, 1, 0); // full whiteness = white
        
        var blue = new Unicolour(ColourSpace.Okhwb, 240, 0, 0);
        var toOklabBlack = blue.Mix(oklabBlack, ColourSpace.Okhwb, premultiplyAlpha: false);
        var toOklabWhite = blue.Mix(oklabWhite, ColourSpace.Okhwb, premultiplyAlpha: false);
        var toOkhwbBlack = blue.Mix(okhwbBlack, ColourSpace.Okhwb, premultiplyAlpha: false);
        var toOkhwbWhite = blue.Mix(okhwbWhite, ColourSpace.Okhwb, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toOklabBlack.Okhwb.Triplet, new(240, 0, 0.5));
        AssertTriplet(toOklabWhite.Okhwb.Triplet, new(240, 0.5, 0));
        AssertTriplet(toOkhwbBlack.Okhwb.Triplet, new(210, 0, 0.5));
        AssertTriplet(toOkhwbWhite.Okhwb.Triplet, new(210, 0.5, 0));
    }
    
    [Test]
    public void GreyscaleBothOklabColours()
    {
        var black = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var grey = new Unicolour(ColourSpace.Oklab, 0.5, 0, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Okhwb, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Okhwb, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Okhwb, premultiplyAlpha: false);
        
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
    public void GreyscaleBothOkhwbColours()
    {
        var black = new Unicolour(ColourSpace.Okhwb, 0, 0, 1.0);
        var white = new Unicolour(ColourSpace.Okhwb, 300, 1.0, 0);
        var grey = new Unicolour(ColourSpace.Okhwb, 100, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Okhwb, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Okhwb, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Okhwb, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Oklab.Triplet, new(Okhsv.ToeInverse(0.5), 0, 0));
        AssertTriplet(blackToGrey.Oklab.Triplet, new(Okhsv.ToeInverse(0.25), 0, 0));
        AssertTriplet(whiteToGrey.Oklab.Triplet, new(Okhsv.ToeInverse(0.75), 0, 0));
        
        // colours created from Okhwb therefore hue changes
        AssertTriplet(blackToWhite.Okhwb.Triplet, new(330, 0.5, 0.5));
        AssertTriplet(blackToGrey.Okhwb.Triplet, new(50, 0.25, 0.75));
        AssertTriplet(whiteToGrey.Okhwb.Triplet, new(20, 0.75, 0.25));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, 0.0000001);
    }
}