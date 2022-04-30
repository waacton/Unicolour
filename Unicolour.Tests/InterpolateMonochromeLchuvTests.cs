namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

// monochrome LUV has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// monochrome LCHuv has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class InterpolateMonochromeLchuvTests
{
    [Test]
    public void MonochromeStartColour()
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

        // monochrome interpolates differently depending on the initial colour space
        // since LUV black/white assumes chroma of 0 (but chroma can be any value)
        AssertLchuv(fromLuvBlack.Lchuv, (25, 50, 120));
        AssertLchuv(fromLuvWhite.Lchuv, (75, 50, 120));
        AssertLchuv(fromLchuvBlack.Lchuv, (25, 100, 150));
        AssertLchuv(fromLchuvWhite.Lchuv, (75, 100, 150));
    }
    
    [Test]
    public void MonochromeEndColour()
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

        // monochrome interpolates differently depending on the initial colour space
        // since LUV black/white assumes chroma of 0 (but chroma can be any value)
        AssertLchuv(toLuvBlack.Lchuv, (25, 50, 240));
        AssertLchuv(toLuvWhite.Lchuv, (75, 50, 240));
        AssertLchuv(toLchuvBlack.Lchuv, (25, 100, 210));
        AssertLchuv(toLchuvWhite.Lchuv, (75, 100, 210));
    }
    
    [Test]
    public void MonochromeBothLuvColours()
    {
        var black = Unicolour.FromLuv(0, 0, 0);
        var white = Unicolour.FromLuv(100, 0, 0);
        var grey = Unicolour.FromLuv(50, 0, 0);

        var blackToWhite = black.InterpolateLchuv(white, 0.5);
        var blackToGrey = black.InterpolateLchuv(grey, 0.5);
        var whiteToGrey = white.InterpolateLchuv(grey, 0.5);
        
        AssertLuv(blackToWhite.Luv, (50, 0, 0));
        AssertLuv(blackToGrey.Luv, (25, 0, 0));
        AssertLuv(whiteToGrey.Luv, (75, 0, 0));
        
        // colours created from LUV therefore hue does not change
        AssertLchuv(blackToWhite.Lchuv, (50, 0, 0));
        AssertLchuv(blackToGrey.Lchuv, (25, 0, 0));
        AssertLchuv(whiteToGrey.Lchuv, (75, 0, 0));
    }
    
    [Test]
    public void MonochromeBothLchuvColours()
    {
        var black = Unicolour.FromLchuv(0, 0, 0);
        var white = Unicolour.FromLchuv(100, 0, 300);
        var grey = Unicolour.FromLchuv(50, 0, 100);

        var blackToWhite = black.InterpolateLchuv(white, 0.5);
        var blackToGrey = black.InterpolateLchuv(grey, 0.5);
        var whiteToGrey = white.InterpolateLchuv(grey, 0.5);
        
        AssertLuv(blackToWhite.Luv, (50, 0, 0));
        AssertLuv(blackToGrey.Luv, (25, 0, 0));
        AssertLuv(whiteToGrey.Luv, (75, 0, 0));
        
        // colours created from LCHuv therefore hue changes
        AssertLchuv(blackToWhite.Lchuv, (50, 0, 330));
        AssertLchuv(blackToGrey.Lchuv, (25, 0, 50));
        AssertLchuv(whiteToGrey.Lchuv, (75, 0, 20));
    }
    
    private static void AssertLuv(Luv actualLuv, (double l, double u, double v) expectedLuv)
    {
        Assert.That(actualLuv.L, Is.EqualTo(expectedLuv.l).Within(0.00000000005));
        Assert.That(actualLuv.U, Is.EqualTo(expectedLuv.u).Within(0.00000000005));
        Assert.That(actualLuv.V, Is.EqualTo(expectedLuv.v).Within(0.00000000005));
    }

    private static void AssertLchuv(Lchuv actualLchuv, (double l, double c, double h) expectedLchuv)
    {
        Assert.That(actualLchuv.L, Is.EqualTo(expectedLchuv.l).Within(0.00000000005));
        Assert.That(actualLchuv.C, Is.EqualTo(expectedLchuv.c).Within(0.00000000005));
        Assert.That(actualLchuv.H, Is.EqualTo(expectedLchuv.h).Within(0.00000000005));
    }
}