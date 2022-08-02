namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

// monochrome Jzazbz has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// monochrome Jzczhz has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class InterpolateMonochromeJzczhzTests
{
    [Test]
    public void MonochromeStartColour()
    {
        var jabBlack = Unicolour.FromJzazbz(0, 0, 0);
        var jabWhite = Unicolour.FromJzazbz(1, 0, 0);
        var jchBlack = Unicolour.FromJzczhz(0, 0.5, 180); // no lightness = black
        var jchWhite = Unicolour.FromJzczhz(1, 0.5, 180); // full lightness = white
        
        var green = Unicolour.FromJzczhz(0.5, 0.5, 120);
        var fromJabBlack = jabBlack.InterpolateJzczhz(green, 0.5);
        var fromJabWhite = jabWhite.InterpolateJzczhz(green, 0.5);
        var fromJchBlack = jchBlack.InterpolateJzczhz(green, 0.5);
        var fromJchWhite = jchWhite.InterpolateJzczhz(green, 0.5);

        // monochrome interpolates differently depending on the initial colour space
        // since Jzazbz black/white assumes chroma of 0 (but chroma can be any value)
        AssertJzczhz(fromJabBlack.Jzczhz, (0.25, 0.25, 120));
        AssertJzczhz(fromJabWhite.Jzczhz, (0.75, 0.25, 120));
        AssertJzczhz(fromJchBlack.Jzczhz, (0.25, 0.5, 150));
        AssertJzczhz(fromJchWhite.Jzczhz, (0.75, 0.5, 150));
    }
    
    [Test]
    public void MonochromeEndColour()
    {
        var jabBlack = Unicolour.FromJzazbz(0, 0, 0);
        var jabWhite = Unicolour.FromJzazbz(1, 0, 0);
        var jchBlack = Unicolour.FromJzczhz(0, 0.5, 180); // no lightness = black
        var jcWhite = Unicolour.FromJzczhz(1, 0.5, 180); // full lightness = white
        
        var blue = Unicolour.FromJzczhz(0.5, 0.5, 240);
        var toJzazbzBlack = blue.InterpolateJzczhz(jabBlack, 0.5);
        var toJzazbzWhite = blue.InterpolateJzczhz(jabWhite, 0.5);
        var toJzczhzBlack = blue.InterpolateJzczhz(jchBlack, 0.5);
        var toJzczhzWhite = blue.InterpolateJzczhz(jcWhite, 0.5);

        // monochrome interpolates differently depending on the initial colour space
        // since Jzazbz black/white assumes chroma of 0 (but chroma can be any value)
        AssertJzczhz(toJzazbzBlack.Jzczhz, (0.25, 0.25, 240));
        AssertJzczhz(toJzazbzWhite.Jzczhz, (0.75, 0.25, 240));
        AssertJzczhz(toJzczhzBlack.Jzczhz, (0.25, 0.5, 210));
        AssertJzczhz(toJzczhzWhite.Jzczhz, (0.75, 0.5, 210));
    }
    
    [Test]
    public void MonochromeBothJzazbzColours()
    {
        var black = Unicolour.FromJzazbz(0, 0, 0);
        var white = Unicolour.FromJzazbz(1, 0, 0);
        var grey = Unicolour.FromJzazbz(0.5, 0, 0);

        var blackToWhite = black.InterpolateJzczhz(white, 0.5);
        var blackToGrey = black.InterpolateJzczhz(grey, 0.5);
        var whiteToGrey = white.InterpolateJzczhz(grey, 0.5);
        
        AssertJzazbz(blackToWhite.Jzazbz, (0.5, 0, 0));
        AssertJzazbz(blackToGrey.Jzazbz, (0.25, 0, 0));
        AssertJzazbz(whiteToGrey.Jzazbz, (0.75, 0, 0));
        
        // colours created from Jzazbz therefore hue does not change
        AssertJzczhz(blackToWhite.Jzczhz, (0.5, 0, 0));
        AssertJzczhz(blackToGrey.Jzczhz, (0.25, 0, 0));
        AssertJzczhz(whiteToGrey.Jzczhz, (0.75, 0, 0));
    }
    
    [Test]
    public void MonochromeBothJzczhzColours()
    {
        var black = Unicolour.FromJzczhz(0, 0, 0);
        var white = Unicolour.FromJzczhz(1, 0, 300);
        var grey = Unicolour.FromJzczhz(0.5, 0, 100);

        var blackToWhite = black.InterpolateJzczhz(white, 0.5);
        var blackToGrey = black.InterpolateJzczhz(grey, 0.5);
        var whiteToGrey = white.InterpolateJzczhz(grey, 0.5);
        
        AssertJzazbz(blackToWhite.Jzazbz, (0.5, 0, 0));
        AssertJzazbz(blackToGrey.Jzazbz, (0.25, 0, 0));
        AssertJzazbz(whiteToGrey.Jzazbz, (0.75, 0, 0));
        
        // colours created from Jzczhz therefore hue changes
        AssertJzczhz(blackToWhite.Jzczhz, (0.5, 0, 330));
        AssertJzczhz(blackToGrey.Jzczhz, (0.25, 0, 50));
        AssertJzczhz(whiteToGrey.Jzczhz, (0.75, 0, 20));
    }
    
    private static void AssertJzazbz(Jzazbz actualJzazbz, (double j, double a, double b) expectedJzazbz)
    {
        Assert.That(actualJzazbz.J, Is.EqualTo(expectedJzazbz.j).Within(0.00000000005));
        Assert.That(actualJzazbz.A, Is.EqualTo(expectedJzazbz.a).Within(0.00000000005));
        Assert.That(actualJzazbz.B, Is.EqualTo(expectedJzazbz.b).Within(0.00000000005));
    }

    private static void AssertJzczhz(Jzczhz actualJzczhz, (double j, double c, double h) expectedJzczhz)
    {
        Assert.That(actualJzczhz.J, Is.EqualTo(expectedJzczhz.j).Within(0.00000000005));
        Assert.That(actualJzczhz.C, Is.EqualTo(expectedJzczhz.c).Within(0.00000000005));
        Assert.That(actualJzczhz.H, Is.EqualTo(expectedJzczhz.h).Within(0.00000000005));
    }
}