using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale Oklab has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Oklch has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class MixGreyscaleOklchTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var oklchBlack = new Unicolour(ColourSpace.Oklch, 0, 0.5, 180); // no lightness = black
        var oklchWhite = new Unicolour(ColourSpace.Oklch, 1, 0.5, 180); // full lightness = white
        
        var green = new Unicolour(ColourSpace.Oklch, 0.5, 0.5, 120);
        var fromOklabBlack = oklabBlack.Mix(green, ColourSpace.Oklch, premultiplyAlpha: false);
        var fromOklabWhite = oklabWhite.Mix(green, ColourSpace.Oklch, premultiplyAlpha: false);
        var fromOklchBlack = oklchBlack.Mix(green, ColourSpace.Oklch, premultiplyAlpha: false);
        var fromOklchWhite = oklchWhite.Mix(green, ColourSpace.Oklch, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromOklabBlack.Oklch.Triplet, new(0.25, 0.25, 120));
        AssertTriplet(fromOklabWhite.Oklch.Triplet, new(0.75, 0.25, 120));
        AssertTriplet(fromOklchBlack.Oklch.Triplet, new(0.25, 0.5, 150));
        AssertTriplet(fromOklchWhite.Oklch.Triplet, new(0.75, 0.5, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var oklchBlack = new Unicolour(ColourSpace.Oklch, 0, 0.5, 180); // no lightness = black
        var oklchWhite = new Unicolour(ColourSpace.Oklch, 1, 0.5, 180); // full lightness = white
        
        var blue = new Unicolour(ColourSpace.Oklch, 0.5, 0.5, 240);
        var toOklabBlack = blue.Mix(oklabBlack, ColourSpace.Oklch, premultiplyAlpha: false);
        var toOklabWhite = blue.Mix(oklabWhite, ColourSpace.Oklch, premultiplyAlpha: false);
        var toOklchBlack = blue.Mix(oklchBlack, ColourSpace.Oklch, premultiplyAlpha: false);
        var toOklchWhite = blue.Mix(oklchWhite, ColourSpace.Oklch, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toOklabBlack.Oklch.Triplet, new(0.25, 0.25, 240));
        AssertTriplet(toOklabWhite.Oklch.Triplet, new(0.75, 0.25, 240));
        AssertTriplet(toOklchBlack.Oklch.Triplet, new(0.25, 0.5, 210));
        AssertTriplet(toOklchWhite.Oklch.Triplet, new(0.75, 0.5, 210));
    }
    
    [Test]
    public void GreyscaleBothOklabColours()
    {
        var black = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var grey = new Unicolour(ColourSpace.Oklab, 0.5, 0, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Oklch, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Oklch, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Oklch, premultiplyAlpha: false);
        
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
        var black = new Unicolour(ColourSpace.Oklch, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Oklch, 1, 0, 300);
        var grey = new Unicolour(ColourSpace.Oklch, 0.5, 0, 100);

        var blackToWhite = black.Mix(white, ColourSpace.Oklch, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Oklch, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Oklch, premultiplyAlpha: false);
        
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
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}