using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale Oklrab has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Oklrch has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class MixGreyscaleOklrchTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var oklrabBlack = new Unicolour(ColourSpace.Oklrab, 0, 0, 0);
        var oklrabWhite = new Unicolour(ColourSpace.Oklrab, 1, 0, 0);
        var oklrchBlack = new Unicolour(ColourSpace.Oklrch, 0, 0.5, 180); // no lightness = black
        var oklrchWhite = new Unicolour(ColourSpace.Oklrch, 1, 0.5, 180); // full lightness = white
        
        var green = new Unicolour(ColourSpace.Oklrch, 0.5, 0.5, 120);
        var fromOklrabBlack = oklrabBlack.Mix(green, ColourSpace.Oklrch, premultiplyAlpha: false);
        var fromOklrabWhite = oklrabWhite.Mix(green, ColourSpace.Oklrch, premultiplyAlpha: false);
        var fromOklrchBlack = oklrchBlack.Mix(green, ColourSpace.Oklrch, premultiplyAlpha: false);
        var fromOklrchWhite = oklrchWhite.Mix(green, ColourSpace.Oklrch, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromOklrabBlack.Oklrch.Triplet, new(0.25, 0.25, 120));
        AssertTriplet(fromOklrabWhite.Oklrch.Triplet, new(0.75, 0.25, 120));
        AssertTriplet(fromOklrchBlack.Oklrch.Triplet, new(0.25, 0.5, 150));
        AssertTriplet(fromOklrchWhite.Oklrch.Triplet, new(0.75, 0.5, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var oklrabBlack = new Unicolour(ColourSpace.Oklrab, 0, 0, 0);
        var oklrabWhite = new Unicolour(ColourSpace.Oklrab, 1, 0, 0);
        var oklrchBlack = new Unicolour(ColourSpace.Oklrch, 0, 0.5, 180); // no lightness = black
        var oklrchWhite = new Unicolour(ColourSpace.Oklrch, 1, 0.5, 180); // full lightness = white
        
        var blue = new Unicolour(ColourSpace.Oklrch, 0.5, 0.5, 240);
        var toOklrabBlack = blue.Mix(oklrabBlack, ColourSpace.Oklrch, premultiplyAlpha: false);
        var toOklrabWhite = blue.Mix(oklrabWhite, ColourSpace.Oklrch, premultiplyAlpha: false);
        var toOklrchBlack = blue.Mix(oklrchBlack, ColourSpace.Oklrch, premultiplyAlpha: false);
        var toOklrchWhite = blue.Mix(oklrchWhite, ColourSpace.Oklrch, premultiplyAlpha: false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toOklrabBlack.Oklrch.Triplet, new(0.25, 0.25, 240));
        AssertTriplet(toOklrabWhite.Oklrch.Triplet, new(0.75, 0.25, 240));
        AssertTriplet(toOklrchBlack.Oklrch.Triplet, new(0.25, 0.5, 210));
        AssertTriplet(toOklrchWhite.Oklrch.Triplet, new(0.75, 0.5, 210));
    }
    
    [Test]
    public void GreyscaleBothOklrabColours()
    {
        var black = new Unicolour(ColourSpace.Oklrab, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Oklrab, 1, 0, 0);
        var grey = new Unicolour(ColourSpace.Oklrab, 0.5, 0, 0);

        var blackToWhite = black.Mix(white, ColourSpace.Oklrch, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Oklrch, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Oklrch, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Oklrab.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Oklrab.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Oklrab.Triplet, new(0.75, 0, 0));
        
        // colours created from Oklrab therefore hue does not change
        AssertTriplet(blackToWhite.Oklrch.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Oklrch.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Oklrch.Triplet, new(0.75, 0, 0));
    }
    
    [Test]
    public void GreyscaleBothOklrchColours()
    {
        var black = new Unicolour(ColourSpace.Oklrch, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Oklrch, 1, 0, 300);
        var grey = new Unicolour(ColourSpace.Oklrch, 0.5, 0, 100);

        var blackToWhite = black.Mix(white, ColourSpace.Oklrch, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Oklrch, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Oklrch, premultiplyAlpha: false);
        
        AssertTriplet(blackToWhite.Oklrab.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Oklrab.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Oklrab.Triplet, new(0.75, 0, 0));
        
        // colours created from Oklrch therefore hue changes
        AssertTriplet(blackToWhite.Oklrch.Triplet, new(0.5, 0, 330));
        AssertTriplet(blackToGrey.Oklrch.Triplet, new(0.25, 0, 50));
        AssertTriplet(whiteToGrey.Oklrch.Triplet, new(0.75, 0, 20));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}