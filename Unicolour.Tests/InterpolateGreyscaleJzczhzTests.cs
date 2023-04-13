namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale Jzazbz has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// greyscale Jzczhz has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class InterpolateGreyscaleJzczhzTests
{
    [Test]
    public void GreyscaleStartColour()
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

        // greyscale interpolates differently depending on the initial colour space
        // since Jzazbz black/white assumes chroma of 0 (but chroma can be any value)
        AssertTriplet(fromJabBlack.Jzczhz.Triplet, new(0.25, 0.25, 120));
        AssertTriplet(fromJabWhite.Jzczhz.Triplet, new(0.75, 0.25, 120));
        AssertTriplet(fromJchBlack.Jzczhz.Triplet, new(0.25, 0.5, 150));
        AssertTriplet(fromJchWhite.Jzczhz.Triplet, new(0.75, 0.5, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
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

        // greyscale interpolates differently depending on the initial colour space
        // since Jzazbz black/white assumes chroma of 0 (but chroma can be any value)
        AssertTriplet(toJzazbzBlack.Jzczhz.Triplet, new(0.25, 0.25, 240));
        AssertTriplet(toJzazbzWhite.Jzczhz.Triplet, new(0.75, 0.25, 240));
        AssertTriplet(toJzczhzBlack.Jzczhz.Triplet, new(0.25, 0.5, 210));
        AssertTriplet(toJzczhzWhite.Jzczhz.Triplet, new(0.75, 0.5, 210));
    }
    
    [Test]
    public void GreyscaleBothJzazbzColours()
    {
        var black = Unicolour.FromJzazbz(0, 0, 0);
        var white = Unicolour.FromJzazbz(1, 0, 0);
        var grey = Unicolour.FromJzazbz(0.5, 0, 0);

        var blackToWhite = black.InterpolateJzczhz(white, 0.5);
        var blackToGrey = black.InterpolateJzczhz(grey, 0.5);
        var whiteToGrey = white.InterpolateJzczhz(grey, 0.5);
        
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
        var black = Unicolour.FromJzczhz(0, 0, 0);
        var white = Unicolour.FromJzczhz(1, 0, 300);
        var grey = Unicolour.FromJzczhz(0.5, 0, 100);

        var blackToWhite = black.InterpolateJzczhz(white, 0.5);
        var blackToGrey = black.InterpolateJzczhz(grey, 0.5);
        var whiteToGrey = white.InterpolateJzczhz(grey, 0.5);
        
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
        AssertUtils.AssertTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}