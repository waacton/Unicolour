namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// greyscale LAB has no hue - shouldn't assume to start at red (0 degrees) when interpolating
// greyscale LCHab has a hue so it should be used (it just can't be seen until there is some lightness & chroma)
public class InterpolateGreyscaleLchabTests
{
    [Test]
    public void GreyscaleStartColour()
    {
        var labBlack = Unicolour.FromLab(0, 0, 0);
        var labWhite = Unicolour.FromLab(100, 0, 0);
        var lchabBlack = Unicolour.FromLchab(0, 100, 180); // no lightness = black
        var lchabWhite = Unicolour.FromLchab(100, 100, 180); // full lightness = white
        
        var green = Unicolour.FromLchab(50, 100, 120);
        var fromLabBlack = labBlack.InterpolateLchab(green, 0.5);
        var fromLabWhite = labWhite.InterpolateLchab(green, 0.5);
        var fromLchabBlack = lchabBlack.InterpolateLchab(green, 0.5);
        var fromLchabWhite = lchabWhite.InterpolateLchab(green, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since LAB black/white assumes chroma of 0 (but chroma can be any value)
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
        var toLabBlack = blue.InterpolateLchab(labBlack, 0.5);
        var toLabWhite = blue.InterpolateLchab(labWhite, 0.5);
        var toLchabBlack = blue.InterpolateLchab(lchabBlack, 0.5);
        var toLchabWhite = blue.InterpolateLchab(lchabWhite, 0.5);

        // greyscale interpolates differently depending on the initial colour space
        // since LAB black/white assumes chroma of 0 (but chroma can be any value)
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

        var blackToWhite = black.InterpolateLchab(white, 0.5);
        var blackToGrey = black.InterpolateLchab(grey, 0.5);
        var whiteToGrey = white.InterpolateLchab(grey, 0.5);
        
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

        var blackToWhite = black.InterpolateLchab(white, 0.5);
        var blackToGrey = black.InterpolateLchab(grey, 0.5);
        var whiteToGrey = white.InterpolateLchab(grey, 0.5);
        
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
        AssertUtils.AssertTriplet(actual, expected, AssertUtils.InterpolationTolerance);
    }
}