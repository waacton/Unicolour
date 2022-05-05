namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

// monochrome Oklab has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// monochrome Oklch has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class InterpolateMonochromeOklchTests
{
    [Test]
    public void MonochromeStartColour()
    {
        var oklabBlack = Unicolour.FromOklab(0, 0, 0);
        var oklabWhite = Unicolour.FromOklab(1, 0, 0);
        var lchuvBlack = Unicolour.FromOklch(0, 0.5, 180); // no lightness = black
        var lchuvWhite = Unicolour.FromOklch(1, 0.5, 180); // full lightness = white
        
        var green = Unicolour.FromOklch(0.5, 0.5, 120);
        var fromOklabBlack = oklabBlack.InterpolateOklch(green, 0.5);
        var fromOklabWhite = oklabWhite.InterpolateOklch(green, 0.5);
        var fromOklchBlack = lchuvBlack.InterpolateOklch(green, 0.5);
        var fromOklchWhite = lchuvWhite.InterpolateOklch(green, 0.5);

        // monochrome interpolates differently depending on the initial colour space
        // since Oklab black/white assumes chroma of 0 (but chroma can be any value)
        AssertOklch(fromOklabBlack.Oklch, (0.25, 0.25, 120));
        AssertOklch(fromOklabWhite.Oklch, (0.75, 0.25, 120));
        AssertOklch(fromOklchBlack.Oklch, (0.25, 0.5, 150));
        AssertOklch(fromOklchWhite.Oklch, (0.75, 0.5, 150));
    }
    
    [Test]
    public void MonochromeEndColour()
    {
        var oklabBlack = Unicolour.FromOklab(0, 0, 0);
        var oklabWhite = Unicolour.FromOklab(1, 0, 0);
        var lchuvBlack = Unicolour.FromOklch(0, 0.5, 180); // no lightness = black
        var lchuvWhite = Unicolour.FromOklch(1, 0.5, 180); // full lightness = white
        
        var blue = Unicolour.FromOklch(0.5, 0.5, 240);
        var toOklabBlack = blue.InterpolateOklch(oklabBlack, 0.5);
        var toOklabWhite = blue.InterpolateOklch(oklabWhite, 0.5);
        var toOklchBlack = blue.InterpolateOklch(lchuvBlack, 0.5);
        var toOklchWhite = blue.InterpolateOklch(lchuvWhite, 0.5);

        // monochrome interpolates differently depending on the initial colour space
        // since Oklab black/white assumes chroma of 0 (but chroma can be any value)
        AssertOklch(toOklabBlack.Oklch, (0.25, 0.25, 240));
        AssertOklch(toOklabWhite.Oklch, (0.75, 0.25, 240));
        AssertOklch(toOklchBlack.Oklch, (0.25, 0.5, 210));
        AssertOklch(toOklchWhite.Oklch, (0.75, 0.5, 210));
    }
    
    [Test]
    public void MonochromeBothOklabColours()
    {
        var black = Unicolour.FromOklab(0, 0, 0);
        var white = Unicolour.FromOklab(1, 0, 0);
        var grey = Unicolour.FromOklab(0.5, 0, 0);

        var blackToWhite = black.InterpolateOklch(white, 0.5);
        var blackToGrey = black.InterpolateOklch(grey, 0.5);
        var whiteToGrey = white.InterpolateOklch(grey, 0.5);
        
        AssertOklab(blackToWhite.Oklab, (0.5, 0, 0));
        AssertOklab(blackToGrey.Oklab, (0.25, 0, 0));
        AssertOklab(whiteToGrey.Oklab, (0.75, 0, 0));
        
        // colours created from Oklab therefore hue does not change
        AssertOklch(blackToWhite.Oklch, (0.5, 0, 0));
        AssertOklch(blackToGrey.Oklch, (0.25, 0, 0));
        AssertOklch(whiteToGrey.Oklch, (0.75, 0, 0));
    }
    
    [Test]
    public void MonochromeBothOklchColours()
    {
        var black = Unicolour.FromOklch(0, 0, 0);
        var white = Unicolour.FromOklch(1, 0, 300);
        var grey = Unicolour.FromOklch(0.5, 0, 100);

        var blackToWhite = black.InterpolateOklch(white, 0.5);
        var blackToGrey = black.InterpolateOklch(grey, 0.5);
        var whiteToGrey = white.InterpolateOklch(grey, 0.5);
        
        AssertOklab(blackToWhite.Oklab, (0.5, 0, 0));
        AssertOklab(blackToGrey.Oklab, (0.25, 0, 0));
        AssertOklab(whiteToGrey.Oklab, (0.75, 0, 0));
        
        // colours created from Oklch therefore hue changes
        AssertOklch(blackToWhite.Oklch, (0.5, 0, 330));
        AssertOklch(blackToGrey.Oklch, (0.25, 0, 50));
        AssertOklch(whiteToGrey.Oklch, (0.75, 0, 20));
    }
    
    private static void AssertOklab(Oklab actualOklab, (double l, double a, double b) expectedOklab)
    {
        Assert.That(actualOklab.L, Is.EqualTo(expectedOklab.l).Within(0.00000000005));
        Assert.That(actualOklab.A, Is.EqualTo(expectedOklab.a).Within(0.00000000005));
        Assert.That(actualOklab.B, Is.EqualTo(expectedOklab.b).Within(0.00000000005));
    }

    private static void AssertOklch(Oklch actualOklch, (double l, double c, double h) expectedOklch)
    {
        Assert.That(actualOklch.L, Is.EqualTo(expectedOklch.l).Within(0.00000000005));
        Assert.That(actualOklch.C, Is.EqualTo(expectedOklch.c).Within(0.00000000005));
        Assert.That(actualOklch.H, Is.EqualTo(expectedOklch.h).Within(0.00000000005));
    }
}