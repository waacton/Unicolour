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
        var oklabBlack = new Unicolour(ColourSpace.Oklab, 0, 0, 0);
        var oklabWhite = new Unicolour(ColourSpace.Oklab, 1, 0, 0);
        var oklchBlack = new Unicolour(ColourSpace.Oklch, 0, 0.5, 180); // no lightness = black
        var oklchWhite = new Unicolour(ColourSpace.Oklch, 1, 0.5, 180); // full lightness = white
        
        var green = new Unicolour(ColourSpace.Oklch, 0.5, 0.5, 120);
        var fromOklabBlack = oklabBlack.Mix(ColourSpace.Oklch, green, 0.5, false);
        var fromOklabWhite = oklabWhite.Mix(ColourSpace.Oklch, green, 0.5, false);
        var fromOklchBlack = oklchBlack.Mix(ColourSpace.Oklch, green, 0.5, false);
        var fromOklchWhite = oklchWhite.Mix(ColourSpace.Oklch, green, 0.5, false);

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
        var toOklabBlack = blue.Mix(ColourSpace.Oklch, oklabBlack, 0.5, false);
        var toOklabWhite = blue.Mix(ColourSpace.Oklch, oklabWhite, 0.5, false);
        var toOklchBlack = blue.Mix(ColourSpace.Oklch, oklchBlack, 0.5, false);
        var toOklchWhite = blue.Mix(ColourSpace.Oklch, oklchWhite, 0.5, false);

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

        var blackToWhite = black.Mix(ColourSpace.Oklch, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Oklch, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Oklch, grey, 0.5, false);
        
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

        var blackToWhite = black.Mix(ColourSpace.Oklch, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Oklch, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Oklch, grey, 0.5, false);
        
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