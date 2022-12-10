namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale LUV has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// greyscale LCHuv has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class InterpolateGreyscaleLchuvTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var luvBlack = Unicolour.FromLuv(0, 0, 0);
        var luvWhite = Unicolour.FromLuv(100, 0, 0);
        var lchuvBlack = Unicolour.FromLchuv(0, 100, 180); // no lightness = black
        var lchuvWhite = Unicolour.FromLchuv(100, 100, 180); // full lightness = white
        
        var green = Unicolour.FromLchuv(50, 100, 120);
        var fromLuvBlack = luvBlack.InterpolateLchuv(green, 0.5);
        var fromLuvWhite = luvWhite.InterpolateLchuv(green, 0.5);
        var fromLchuvBlack = lchuvBlack.InterpolateLchuv(green, 0.5);
        var fromLchuvWhite = lchuvWhite.InterpolateLchuv(green, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since LUV black/white assumes chroma of 0 (but chroma can be any value)
        AssertColourTriplet(fromLuvBlack.Lchuv.Triplet, new(25, 50, 120));
        AssertColourTriplet(fromLuvWhite.Lchuv.Triplet, new(75, 50, 120));
        AssertColourTriplet(fromLchuvBlack.Lchuv.Triplet, new(25, 100, 150));
        AssertColourTriplet(fromLchuvWhite.Lchuv.Triplet, new(75, 100, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var luvBlack = Unicolour.FromLuv(0, 0, 0);
        var luvWhite = Unicolour.FromLuv(100, 0, 0);
        var lchuvBlack = Unicolour.FromLchuv(0, 100, 180); // no lightness = black
        var lchuvWhite = Unicolour.FromLchuv(100, 100, 180); // full lightness = white
        
        var blue = Unicolour.FromLchuv(50, 100, 240);
        var toLuvBlack = blue.InterpolateLchuv(luvBlack, 0.5);
        var toLuvWhite = blue.InterpolateLchuv(luvWhite, 0.5);
        var toLchuvBlack = blue.InterpolateLchuv(lchuvBlack, 0.5);
        var toLchuvWhite = blue.InterpolateLchuv(lchuvWhite, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since LUV black/white assumes chroma of 0 (but chroma can be any value)
        AssertColourTriplet(toLuvBlack.Lchuv.Triplet, new(25, 50, 240));
        AssertColourTriplet(toLuvWhite.Lchuv.Triplet, new(75, 50, 240));
        AssertColourTriplet(toLchuvBlack.Lchuv.Triplet, new(25, 100, 210));
        AssertColourTriplet(toLchuvWhite.Lchuv.Triplet, new(75, 100, 210));
    }
    
    [Test]
    public void GreyscaleBothLuvColours()
    {
        var black = Unicolour.FromLuv(0, 0, 0);
        var white = Unicolour.FromLuv(100, 0, 0);
        var grey = Unicolour.FromLuv(50, 0, 0);

        var blackToWhite = black.InterpolateLchuv(white, 0.5);
        var blackToGrey = black.InterpolateLchuv(grey, 0.5);
        var whiteToGrey = white.InterpolateLchuv(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertColourTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertColourTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LUV therefore hue does not change
        AssertColourTriplet(blackToWhite.Lchuv.Triplet, new(50, 0, 0));
        AssertColourTriplet(blackToGrey.Lchuv.Triplet, new(25, 0, 0));
        AssertColourTriplet(whiteToGrey.Lchuv.Triplet, new(75, 0, 0));
    }
    
    [Test]
    public void GreyscaleBothLchuvColours()
    {
        var black = Unicolour.FromLchuv(0, 0, 0);
        var white = Unicolour.FromLchuv(100, 0, 300);
        var grey = Unicolour.FromLchuv(50, 0, 100);

        var blackToWhite = black.InterpolateLchuv(white, 0.5);
        var blackToGrey = black.InterpolateLchuv(grey, 0.5);
        var whiteToGrey = white.InterpolateLchuv(grey, 0.5);
        
        AssertColourTriplet(blackToWhite.Luv.Triplet, new(50, 0, 0));
        AssertColourTriplet(blackToGrey.Luv.Triplet, new(25, 0, 0));
        AssertColourTriplet(whiteToGrey.Luv.Triplet, new(75, 0, 0));
        
        // colours created from LCHuv therefore hue changes
        AssertColourTriplet(blackToWhite.Lchuv.Triplet, new(50, 0, 330));
        AssertColourTriplet(blackToGrey.Lchuv.Triplet, new(25, 0, 50));
        AssertColourTriplet(whiteToGrey.Lchuv.Triplet, new(75, 0, 20));
    }
    
    private static void AssertColourTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        AssertUtils.AssertColourTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}