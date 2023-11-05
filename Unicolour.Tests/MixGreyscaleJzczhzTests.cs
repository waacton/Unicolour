namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale Jzazbz has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Jzczhz has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class MixGreyscaleJzczhzTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var jabBlack = new Unicolour(ColourSpace.Jzazbz, 0, 0, 0);
        var jabWhite = new Unicolour(ColourSpace.Jzazbz, 1, 0, 0);
        var jchBlack = new Unicolour(ColourSpace.Jzczhz, 0, 0.5, 180); // no lightness = black
        var jchWhite = new Unicolour(ColourSpace.Jzczhz, 1, 0.5, 180); // full lightness = white
        
        var green = new Unicolour(ColourSpace.Jzczhz, 0.5, 0.5, 120);
        var fromJabBlack = jabBlack.Mix(ColourSpace.Jzczhz, green, 0.5, false);
        var fromJabWhite = jabWhite.Mix(ColourSpace.Jzczhz, green, 0.5, false);
        var fromJchBlack = jchBlack.Mix(ColourSpace.Jzczhz, green, 0.5, false);
        var fromJchWhite = jchWhite.Mix(ColourSpace.Jzczhz, green, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromJabBlack.Jzczhz.Triplet, new(0.25, 0.25, 120));
        AssertTriplet(fromJabWhite.Jzczhz.Triplet, new(0.75, 0.25, 120));
        AssertTriplet(fromJchBlack.Jzczhz.Triplet, new(0.25, 0.5, 150));
        AssertTriplet(fromJchWhite.Jzczhz.Triplet, new(0.75, 0.5, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var jabBlack = new Unicolour(ColourSpace.Jzazbz, 0, 0, 0);
        var jabWhite = new Unicolour(ColourSpace.Jzazbz, 1, 0, 0);
        var jchBlack = new Unicolour(ColourSpace.Jzczhz, 0, 0.5, 180); // no lightness = black
        var jcWhite = new Unicolour(ColourSpace.Jzczhz, 1, 0.5, 180); // full lightness = white
        
        var blue = new Unicolour(ColourSpace.Jzczhz, 0.5, 0.5, 240);
        var toJzazbzBlack = blue.Mix(ColourSpace.Jzczhz, jabBlack, 0.5, false);
        var toJzazbzWhite = blue.Mix(ColourSpace.Jzczhz, jabWhite, 0.5, false);
        var toJzczhzBlack = blue.Mix(ColourSpace.Jzczhz, jchBlack, 0.5, false);
        var toJzczhzWhite = blue.Mix(ColourSpace.Jzczhz, jcWhite, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toJzazbzBlack.Jzczhz.Triplet, new(0.25, 0.25, 240));
        AssertTriplet(toJzazbzWhite.Jzczhz.Triplet, new(0.75, 0.25, 240));
        AssertTriplet(toJzczhzBlack.Jzczhz.Triplet, new(0.25, 0.5, 210));
        AssertTriplet(toJzczhzWhite.Jzczhz.Triplet, new(0.75, 0.5, 210));
    }
    
    [Test]
    public void GreyscaleBothJzazbzColours()
    {
        var black = new Unicolour(ColourSpace.Jzazbz, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Jzazbz, 1, 0, 0);
        var grey = new Unicolour(ColourSpace.Jzazbz, 0.5, 0, 0);

        var blackToWhite = black.Mix(ColourSpace.Jzczhz, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Jzczhz, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Jzczhz, grey, 0.5, false);
        
        AssertTriplet(blackToWhite.Jzazbz.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Jzazbz.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Jzazbz.Triplet, new(0.75, 0, 0));
        
        // colours created from Jzazbz therefore hue does not change
        AssertTriplet(blackToWhite.Jzczhz.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Jzczhz.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Jzczhz.Triplet, new(0.75, 0, 0));
    }
    
    [Test]
    public void GreyscaleBothJzczhzColours()
    {
        var black = new Unicolour(ColourSpace.Jzczhz, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Jzczhz, 1, 0, 300);
        var grey = new Unicolour(ColourSpace.Jzczhz, 0.5, 0, 100);

        var blackToWhite = black.Mix(ColourSpace.Jzczhz, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Jzczhz, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Jzczhz, grey, 0.5, false);
        
        AssertTriplet(blackToWhite.Jzazbz.Triplet, new(0.5, 0, 0));
        AssertTriplet(blackToGrey.Jzazbz.Triplet, new(0.25, 0, 0));
        AssertTriplet(whiteToGrey.Jzazbz.Triplet, new(0.75, 0, 0));
        
        // colours created from Jzczhz therefore hue changes
        AssertTriplet(blackToWhite.Jzczhz.Triplet, new(0.5, 0, 330));
        AssertTriplet(blackToGrey.Jzczhz.Triplet, new(0.25, 0, 50));
        AssertTriplet(whiteToGrey.Jzczhz.Triplet, new(0.75, 0, 20));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}