namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale LAB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale LCHab has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class MixGreyscaleLchabTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var labBlack = new Unicolour(ColourSpace.Lab, 0, 0, 0);
        var labWhite = new Unicolour(ColourSpace.Lab, 100, 0, 0);
        var lchabBlack = new Unicolour(ColourSpace.Lchab, 0, 100, 180); // no lightness = black
        var lchabWhite = new Unicolour(ColourSpace.Lchab, 100, 100, 180); // full lightness = white
        
        var green = new Unicolour(ColourSpace.Lchab, 50, 100, 120);
        var fromLabBlack = labBlack.Mix(ColourSpace.Lchab, green, 0.5, false);
        var fromLabWhite = labWhite.Mix(ColourSpace.Lchab, green, 0.5, false);
        var fromLchabBlack = lchabBlack.Mix(ColourSpace.Lchab, green, 0.5, false);
        var fromLchabWhite = lchabWhite.Mix(ColourSpace.Lchab, green, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromLabBlack.Lchab.Triplet, new(25, 50, 120));
        AssertTriplet(fromLabWhite.Lchab.Triplet, new(75, 50, 120));
        AssertTriplet(fromLchabBlack.Lchab.Triplet, new(25, 100, 150));
        AssertTriplet(fromLchabWhite.Lchab.Triplet, new(75, 100, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var labBlack = new Unicolour(ColourSpace.Lab, 0, 0, 0);
        var labWhite = new Unicolour(ColourSpace.Lab, 100, 0, 0);
        var lchabBlack = new Unicolour(ColourSpace.Lchab, 0, 100, 180); // no lightness = black
        var lchabWhite = new Unicolour(ColourSpace.Lchab, 100, 100, 180); // full lightness = white
        
        var blue = new Unicolour(ColourSpace.Lchab, 50, 100, 240);
        var toLabBlack = blue.Mix(ColourSpace.Lchab, labBlack, 0.5, false);
        var toLabWhite = blue.Mix(ColourSpace.Lchab, labWhite, 0.5, false);
        var toLchabBlack = blue.Mix(ColourSpace.Lchab, lchabBlack, 0.5, false);
        var toLchabWhite = blue.Mix(ColourSpace.Lchab, lchabWhite, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toLabBlack.Lchab.Triplet, new(25, 50, 240));
        AssertTriplet(toLabWhite.Lchab.Triplet, new(75, 50, 240));
        AssertTriplet(toLchabBlack.Lchab.Triplet, new(25, 100, 210));
        AssertTriplet(toLchabWhite.Lchab.Triplet, new(75, 100, 210));
    }
    
    [Test]
    public void GreyscaleBothLabColours()
    {
        var black = new Unicolour(ColourSpace.Lab, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Lab, 100, 0, 0);
        var grey = new Unicolour(ColourSpace.Lab, 50, 0, 0);

        var blackToWhite = black.Mix(ColourSpace.Lchab, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Lchab, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Lchab, grey, 0.5, false);
        
        AssertTriplet(blackToWhite.Lab.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Lab.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Lab.Triplet, new(75, 0, 0));
        
        // colours created from LAB therefore hue does not change
        AssertTriplet(blackToWhite.Lchab.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Lchab.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Lchab.Triplet, new(75, 0, 0));
    }
    
    [Test]
    public void GreyscaleBothLchabColours()
    {
        var black = new Unicolour(ColourSpace.Lchab, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Lchab, 100, 0, 300);
        var grey = new Unicolour(ColourSpace.Lchab, 50, 0, 100);

        var blackToWhite = black.Mix(ColourSpace.Lchab, white, 0.5, false);
        var blackToGrey = black.Mix(ColourSpace.Lchab, grey, 0.5, false);
        var whiteToGrey = white.Mix(ColourSpace.Lchab, grey, 0.5, false);
        
        AssertTriplet(blackToWhite.Lab.Triplet, new(50, 0, 0));
        AssertTriplet(blackToGrey.Lab.Triplet, new(25, 0, 0));
        AssertTriplet(whiteToGrey.Lab.Triplet, new(75, 0, 0));
        
        // colours created from LCHab therefore hue changes
        AssertTriplet(blackToWhite.Lchab.Triplet, new(50, 0, 330));
        AssertTriplet(blackToGrey.Lchab.Triplet, new(25, 0, 50));
        AssertTriplet(whiteToGrey.Lchab.Triplet, new(75, 0, 20));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}