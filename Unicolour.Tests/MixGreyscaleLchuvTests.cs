namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale LUV has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale LCHuv has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class MixGreyscaleLchuvTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var luvBlack = new Unicolour(ColourSpace.Luv, 0, 0, 0);
        var luvWhite = new Unicolour(ColourSpace.Luv, 100, 0, 0);
        var lchuvBlack = new Unicolour(ColourSpace.Lchuv, 0, 100, 180); // no lightness = black
        var lchuvWhite = new Unicolour(ColourSpace.Lchuv, 100, 100, 180); // full lightness = white
        
        var green = new Unicolour(ColourSpace.Lchuv, 50, 100, 120);
        var fromLuvBlack = luvBlack.Mix(green, ColourSpace.Lchuv, 0.5, false);
        var fromLuvWhite = luvWhite.Mix(green, ColourSpace.Lchuv, 0.5, false);
        var fromLchuvBlack = lchuvBlack.Mix(green, ColourSpace.Lchuv, 0.5, false);
        var fromLchuvWhite = lchuvWhite.Mix(green, ColourSpace.Lchuv, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromLuvBlack.Lchuv.Triplet, new(25, 50, 120));
        AssertTriplet(fromLuvWhite.Lchuv.Triplet, new(75, 50, 120));
        AssertTriplet(fromLchuvBlack.Lchuv.Triplet, new(25, 100, 150));
        AssertTriplet(fromLchuvWhite.Lchuv.Triplet, new(75, 100, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var luvBlack = new Unicolour(ColourSpace.Luv, 0, 0, 0);
        var luvWhite = new Unicolour(ColourSpace.Luv, 100, 0, 0);
        var lchuvBlack = new Unicolour(ColourSpace.Lchuv, 0, 100, 180); // no lightness = black
        var lchuvWhite = new Unicolour(ColourSpace.Lchuv, 100, 100, 180); // full lightness = white
        
        var blue = new Unicolour(ColourSpace.Lchuv, 50, 100, 240);
        var toLuvBlack = blue.Mix(luvBlack, ColourSpace.Lchuv, 0.5, false);
        var toLuvWhite = blue.Mix(luvWhite, ColourSpace.Lchuv, 0.5, false);
        var toLchuvBlack = blue.Mix(lchuvBlack, ColourSpace.Lchuv, 0.5, false);
        var toLchuvWhite = blue.Mix(lchuvWhite, ColourSpace.Lchuv, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toLuvBlack.Lchuv.Triplet, new(25, 50, 240));
        AssertTriplet(toLuvWhite.Lchuv.Triplet, new(75, 50, 240));
        AssertTriplet(toLchuvBlack.Lchuv.Triplet, new(25, 100, 210));
        AssertTriplet(toLchuvWhite.Lchuv.Triplet, new(75, 100, 210));
    }
    
    [Test]
    public void GreyscaleBothLuvColours()
    {
        var black = new Unicolour(ColourSpace.Luv, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Luv, 100, 0, 0);
        var grey = new Unicolour(ColourSpace.Luv, 50, 0, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Lchuv, 0.5, false);
        var blackToGrey = black.Mix(grey, ColourSpace.Lchuv, 0.5, false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Lchuv, 0.5, false);
        
        AssertTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LUV therefore hue does not change
        AssertTriplet(blackToWhite.Lchuv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Lchuv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Lchuv.Triplet, new(75, 0, 0));
    }
    
    [Test]
    public void GreyscaleBothLchuvColours()
    {
        var black = new Unicolour(ColourSpace.Lchuv, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Lchuv, 100, 0, 300);
        var grey = new Unicolour(ColourSpace.Lchuv, 50, 0, 100);

        var blackToWhite = black.Mix(white, ColourSpace.Lchuv, 0.5, false);
        var blackToGrey = black.Mix(grey, ColourSpace.Lchuv, 0.5, false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Lchuv, 0.5, false);
        
        AssertTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LCHuv therefore hue changes
        AssertTriplet(blackToWhite.Lchuv.Triplet, new(50, 0, 330));
        AssertTriplet(blackToGrey.Lchuv.Triplet, new(25, 0, 50));
        AssertTriplet(whiteToGrey.Lchuv.Triplet, new(75, 0, 20));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}