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
        var labBlack = Unicolour.FromLab(0, 0, 0);
        var labWhite = Unicolour.FromLab(100, 0, 0);
        var lchabBlack = Unicolour.FromLchab(0, 100, 180); // no lightness = black
        var lchabWhite = Unicolour.FromLchab(100, 100, 180); // full lightness = white
        
        var green = Unicolour.FromLchab(50, 100, 120);
        var fromLabBlack = labBlack.MixLchab(green, 0.5, false);
        var fromLabWhite = labWhite.MixLchab(green, 0.5, false);
        var fromLchabBlack = lchabBlack.MixLchab(green, 0.5, false);
        var fromLchabWhite = lchabWhite.MixLchab(green, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromLabBlack.Lchab.Triplet, new(25, 50, 120));
        AssertTriplet(fromLabWhite.Lchab.Triplet, new(75, 50, 120));
        AssertTriplet(fromLchabBlack.Lchab.Triplet, new(25, 100, 150));
        AssertTriplet(fromLchabWhite.Lchab.Triplet, new(75, 100, 150));
    }
    
    [Test]
    public void GreyscaleEndColour()
    {
        var labBlack = Unicolour.FromLab(0, 0, 0);
        var labWhite = Unicolour.FromLab(100, 0, 0);
        var lchabBlack = Unicolour.FromLchab(0, 100, 180); // no lightness = black
        var lchabWhite = Unicolour.FromLchab(100, 100, 180); // full lightness = white
        
        var blue = Unicolour.FromLchab(50, 100, 240);
        var toLabBlack = blue.MixLchab(labBlack, 0.5, false);
        var toLabWhite = blue.MixLchab(labWhite, 0.5, false);
        var toLchabBlack = blue.MixLchab(lchabBlack, 0.5, false);
        var toLchabWhite = blue.MixLchab(lchabWhite, 0.5, false);

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toLabBlack.Lchab.Triplet, new(25, 50, 240));
        AssertTriplet(toLabWhite.Lchab.Triplet, new(75, 50, 240));
        AssertTriplet(toLchabBlack.Lchab.Triplet, new(25, 100, 210));
        AssertTriplet(toLchabWhite.Lchab.Triplet, new(75, 100, 210));
    }
    
    [Test]
    public void GreyscaleBothLabColours()
    {
        var black = Unicolour.FromLab(0, 0, 0);
        var white = Unicolour.FromLab(100, 0, 0);
        var grey = Unicolour.FromLab(50, 0, 0);

        var blackToWhite = black.MixLchab(white, 0.5, false);
        var blackToGrey = black.MixLchab(grey, 0.5, false);
        var whiteToGrey = white.MixLchab(grey, 0.5, false);
        
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
        var black = Unicolour.FromLchab(0, 0, 0);
        var white = Unicolour.FromLchab(100, 0, 300);
        var grey = Unicolour.FromLchab(50, 0, 100);

        var blackToWhite = black.MixLchab(white, 0.5, false);
        var blackToGrey = black.MixLchab(grey, 0.5, false);
        var whiteToGrey = white.MixLchab(grey, 0.5, false);
        
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
        AssertUtils.AssertTriplet(actual, expected, AssertUtils.MixTolerance);
    }
}