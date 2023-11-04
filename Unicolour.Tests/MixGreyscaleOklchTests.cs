namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale Oklab has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Oklch has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class MixGreyscaleOklchTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var oklabBlack = Unicolour.FromOklab(0, 0, 0);
        var oklabWhite = Unicolour.FromOklab(1, 0, 0);
        var oklchBlack = Unicolour.FromOklch(0, 0.5, 180); // no lightness = black
        var oklchWhite = Unicolour.FromOklch(1, 0.5, 180); // full lightness = white
        
        var green = Unicolour.FromOklch(0.5, 0.5, 120);
        var fromOklabBlack = oklabBlack.MixOklch(green, 0.5, false);
        var fromOklabWhite = oklabWhite.MixOklch(green, 0.5, false);
        var fromOklchBlack = oklchBlack.MixOklch(green, 0.5, false);
        var fromOklchWhite = oklchWhite.MixOklch(green, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromOklabBlack.Oklch.Triplet, new(0.25, 0.25, 120));
        AssertTriplet(fromOklabWhite.Oklch.Triplet, new(0.75, 0.25, 120));
        AssertTriplet(fromOklchBlack.Oklch.Triplet, new(0.25, 0.5, 150));
        AssertTriplet(fromOklchWhite.Oklch.Triplet, new(0.75, 0.5, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var oklabBlack = Unicolour.FromOklab(0, 0, 0);
        var oklabWhite = Unicolour.FromOklab(1, 0, 0);
        var oklchBlack = Unicolour.FromOklch(0, 0.5, 180); // no lightness = black
        var oklchWhite = Unicolour.FromOklch(1, 0.5, 180); // full lightness = white
        
        var blue = Unicolour.FromOklch(0.5, 0.5, 240);
        var toOklabBlack = blue.MixOklch(oklabBlack, 0.5, false);
        var toOklabWhite = blue.MixOklch(oklabWhite, 0.5, false);
        var toOklchBlack = blue.MixOklch(oklchBlack, 0.5, false);
        var toOklchWhite = blue.MixOklch(oklchWhite, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toOklabBlack.Oklch.Triplet, new(0.25, 0.25, 240));
        AssertTriplet(toOklabWhite.Oklch.Triplet, new(0.75, 0.25, 240));
        AssertTriplet(toOklchBlack.Oklch.Triplet, new(0.25, 0.5, 210));
        AssertTriplet(toOklchWhite.Oklch.Triplet, new(0.75, 0.5, 210));
    }
    
    [Test]
    public void GreyscaleBothOklabColours()
    {
        var black = Unicolour.FromOklab(0, 0, 0);
        var white = Unicolour.FromOklab(1, 0, 0);
        var grey = Unicolour.FromOklab(0.5, 0, 0);

        var blackToWhite = black.MixOklch(white, 0.5, false);
        var blackToGrey = black.MixOklch(grey, 0.5, false);
        var whiteToGrey = white.MixOklch(grey, 0.5, false);
        
        AssertTriplet(blackToWhite.Oklab.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Oklab.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Oklab.Triplet, new(0.75, 0, 0));
        
        // colours created from Oklab therefore hue does not change
        AssertTriplet(blackToWhite.Oklch.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Oklch.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Oklch.Triplet, new(0.75, 0, 0));
    }
    
    [Test]
    public void GreyscaleBothOklchColours()
    {
        var black = Unicolour.FromOklch(0, 0, 0);
        var white = Unicolour.FromOklch(1, 0, 300);
        var grey = Unicolour.FromOklch(0.5, 0, 100);

        var blackToWhite = black.MixOklch(white, 0.5, false);
        var blackToGrey = black.MixOklch(grey, 0.5, false);
        var whiteToGrey = white.MixOklch(grey, 0.5, false);
        
        AssertTriplet(blackToWhite.Oklab.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Oklab.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Oklab.Triplet, new(0.75, 0, 0));
        
        // colours created from Oklch therefore hue changes
        AssertTriplet(blackToWhite.Oklch.Triplet, new(0.5, 0, 330));
        AssertTriplet(blackToGrey.Oklch.Triplet, new(0.25, 0, 50));
        AssertTriplet(whiteToGrey.Oklch.Triplet, new(0.75, 0, 20));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        AssertUtils.AssertTriplet(actual, expected, AssertUtils.MixTolerance);
    }
}