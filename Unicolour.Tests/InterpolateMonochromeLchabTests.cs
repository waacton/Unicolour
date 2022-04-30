namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

// monochrome LAB has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// monochrome LCHab has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class InterpolateMonochromeLchabTests
{
    [Test]
    public void MonochromeStartColour()
    {
        var labBlack = Unicolour.FromLab(0, 0, 0);
        var labWhite = Unicolour.FromLab(100, 0, 0);
        var lchabBlack = Unicolour.FromLchab(0, 100, 180); // no lightness = black
        var lchabWhite = Unicolour.FromLchab(100, 100, 180); // full lightness = white
        
        var green = Unicolour.FromLchab(50, 100, 120);
        var fromLabBlack = labBlack.InterpolateLchab(green, 0.5);
        var fromLabWhite = labWhite.InterpolateLchab(green, 0.5);
        var fromLchabBlack = lchabBlack.InterpolateLchab(green, 0.5);
        var fromLchabWhite = lchabWhite.InterpolateLchab(green, 0.5);

        // monochrome interpolates differently depending on the initial colour space
        // since LAB black/white assumes chroma of 0 (but chroma can be any value)
        AssertLchab(fromLabBlack.Lchab, (25, 50, 120));
        AssertLchab(fromLabWhite.Lchab, (75, 50, 120));
        AssertLchab(fromLchabBlack.Lchab, (25, 100, 150));
        AssertLchab(fromLchabWhite.Lchab, (75, 100, 150));
    }
    
    [Test]
    public void MonochromeEndColour()
    {
        var labBlack = Unicolour.FromLab(0, 0, 0);
        var labWhite = Unicolour.FromLab(100, 0, 0);
        var lchabBlack = Unicolour.FromLchab(0, 100, 180); // no lightness = black
        var lchabWhite = Unicolour.FromLchab(100, 100, 180); // full lightness = white
        
        var blue = Unicolour.FromLchab(50, 100, 240);
        var toLabBlack = blue.InterpolateLchab(labBlack, 0.5);
        var toLabWhite = blue.InterpolateLchab(labWhite, 0.5);
        var toLchabBlack = blue.InterpolateLchab(lchabBlack, 0.5);
        var toLchabWhite = blue.InterpolateLchab(lchabWhite, 0.5);

        // monochrome interpolates differently depending on the initial colour space
        // since LAB black/white assumes chroma of 0 (but chroma can be any value)
        AssertLchab(toLabBlack.Lchab, (25, 50, 240));
        AssertLchab(toLabWhite.Lchab, (75, 50, 240));
        AssertLchab(toLchabBlack.Lchab, (25, 100, 210));
        AssertLchab(toLchabWhite.Lchab, (75, 100, 210));
    }
    
    [Test]
    public void MonochromeBothLabColours()
    {
        var black = Unicolour.FromLab(0, 0, 0);
        var white = Unicolour.FromLab(100, 0, 0);
        var grey = Unicolour.FromLab(50, 0, 0);

        var blackToWhite = black.InterpolateLchab(white, 0.5);
        var blackToGrey = black.InterpolateLchab(grey, 0.5);
        var whiteToGrey = white.InterpolateLchab(grey, 0.5);
        
        AssertLab(blackToWhite.Lab, (50, 0, 0));
        AssertLab(blackToGrey.Lab, (25, 0, 0));
        AssertLab(whiteToGrey.Lab, (75, 0, 0));
        
        // colours created from LAB therefore hue does not change
        AssertLchab(blackToWhite.Lchab, (50, 0, 0));
        AssertLchab(blackToGrey.Lchab, (25, 0, 0));
        AssertLchab(whiteToGrey.Lchab, (75, 0, 0));
    }
    
    [Test]
    public void MonochromeBothLchabColours()
    {
        var black = Unicolour.FromLchab(0, 0, 0);
        var white = Unicolour.FromLchab(100, 0, 300);
        var grey = Unicolour.FromLchab(50, 0, 100);

        var blackToWhite = black.InterpolateLchab(white, 0.5);
        var blackToGrey = black.InterpolateLchab(grey, 0.5);
        var whiteToGrey = white.InterpolateLchab(grey, 0.5);
        
        AssertLab(blackToWhite.Lab, (50, 0, 0));
        AssertLab(blackToGrey.Lab, (25, 0, 0));
        AssertLab(whiteToGrey.Lab, (75, 0, 0));
        
        // colours created from LCHab therefore hue changes
        AssertLchab(blackToWhite.Lchab, (50, 0, 330));
        AssertLchab(blackToGrey.Lchab, (25, 0, 50));
        AssertLchab(whiteToGrey.Lchab, (75, 0, 20));
    }
    
    private static void AssertLab(Lab actualLab, (double l, double a, double b) expectedLab)
    {
        Assert.That(actualLab.L, Is.EqualTo(expectedLab.l).Within(0.00000000005));
        Assert.That(actualLab.A, Is.EqualTo(expectedLab.a).Within(0.00000000005));
        Assert.That(actualLab.B, Is.EqualTo(expectedLab.b).Within(0.00000000005));
    }

    private static void AssertLchab(Lchab actualLchab, (double l, double c, double h) expectedLchab)
    {
        Assert.That(actualLchab.L, Is.EqualTo(expectedLchab.l).Within(0.00000000005));
        Assert.That(actualLchab.C, Is.EqualTo(expectedLchab.c).Within(0.00000000005));
        Assert.That(actualLchab.H, Is.EqualTo(expectedLchab.h).Within(0.00000000005));
    }
}