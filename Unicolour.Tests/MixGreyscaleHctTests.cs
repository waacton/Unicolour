namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// note: HCT is a composite of LAB & CAM16, therefore there is no obvious rectangular/hueless space to compare against
// so using RGB to generate non-HCT greyscales
// ----------
// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale HCT has a hue so it should be used (it just can't be seen until there is some chroma & tone)
public class MixGreyscaleHctTests
{
    private const double RgbWhiteChroma = 2.8690369750685738;
    
    [Test]
    public void GreyscaleStartColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hctBlack = new Unicolour(ColourSpace.Hct, 180, 100, 0); // no tone = black
        var hctWhite = new Unicolour(ColourSpace.Hct, 180, 100, 100); // full tone = white
        
        var green = new Unicolour(ColourSpace.Hct, 120, 100, 50);
        var fromRgbBlack = rgbBlack.Mix(green, ColourSpace.Hct, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, ColourSpace.Hct, premultiplyAlpha: false);
        var fromHctBlack = hctBlack.Mix(green, ColourSpace.Hct, premultiplyAlpha: false);
        var fromHctWhite = hctWhite.Mix(green, ColourSpace.Hct, premultiplyAlpha: false);
        
        // no obvious way to create known HCT value when starting from non-HCT space
        // so need to calculate what the expected Chroma will be for RGB-white
        // however, not really a problem as this test focuses on hue
        const double expectedFromRgbWhiteChroma = (RgbWhiteChroma + 100) * 0.5;
        
        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.Hct.Triplet, new(120, 50, 25));
        AssertTriplet(fromRgbWhite.Hct.Triplet, new(120, expectedFromRgbWhiteChroma, 75));
        AssertTriplet(fromHctBlack.Hct.Triplet, new(150, 100, 25));
        AssertTriplet(fromHctWhite.Hct.Triplet, new(150, 100, 75));
    }

    [Test]
    public void GreyscaleEndColour()
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hctBlack = new Unicolour(ColourSpace.Hct, 180, 100, 0); // no tone = black
        var hctWhite = new Unicolour(ColourSpace.Hct, 180, 100, 100); // full tone = white
        
        var blue = new Unicolour(ColourSpace.Hct, 240, 100, 50);
        var toRgbBlack = blue.Mix(rgbBlack, ColourSpace.Hct, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, ColourSpace.Hct, premultiplyAlpha: false);
        var toHctBlack = blue.Mix(hctBlack, ColourSpace.Hct, premultiplyAlpha: false);
        var toHctWhite = blue.Mix(hctWhite, ColourSpace.Hct, premultiplyAlpha: false);
        
        // no obvious way to create known HCT value when starting from non-HCT space
        // so need to calculate what the expected Chroma will be for RGB-white
        // however, not really a problem as this test focuses on hue
        const double expectedFromRgbWhiteChroma = (RgbWhiteChroma + 100) * 0.5;

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.Hct.Triplet, new(240, 50, 25));
        AssertTriplet(toRgbWhite.Hct.Triplet, new(240, expectedFromRgbWhiteChroma, 75));
        AssertTriplet(toHctBlack.Hct.Triplet, new(210, 100, 25));
        AssertTriplet(toHctWhite.Hct.Triplet, new(210, 100, 75));
    }
    
    // note: due to how HCT is derived
    // there is no obvious way of mapping non-HCT greyscales to specific HCT hues
    // so this just tests what it can
    [Test]
    public void GreyscaleBothRgbColours()
    {
        var black = new Unicolour(ColourSpace.Rgb, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Rgb, 1, 1, 1);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, ColourSpace.Hct, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hct, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hct, premultiplyAlpha: false);
        
        // colours created from RGB therefore hue does not change
        // (except for HCT for RGB-black, which converts to a different hue than other greyscales)
        Assert.That(black.Hct.H, Is.EqualTo(0));
        Assert.That(blackToWhite.Hct.H, Is.EqualTo(blackToGrey.Hct.H).Within(0.001));
        Assert.That(whiteToGrey.Hct.H, Is.EqualTo(white.Hct.H).Within(0.001));
        Assert.That(whiteToGrey.Hct.H, Is.EqualTo(grey.Hct.H).Within(0.001));
    }
    
    // note: due to how HCT is derived
    // there is no obvious way of mapping non-HCT greyscales to specific HCT hues
    // so this just tests what it can
    [Test]
    public void GreyscaleBothHctColours()
    {
        var black = new Unicolour(ColourSpace.Hct, 0, 0, 0);
        var white = new Unicolour(ColourSpace.Hct, 300, 0, 100);
        var grey = new Unicolour(ColourSpace.Hct, 100, 0, 50);

        var blackToWhite = black.Mix(white, ColourSpace.Hct, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, ColourSpace.Hct, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, ColourSpace.Hct, premultiplyAlpha: false);
        
        AssertGrey(blackToWhite.Rgb);
        AssertGrey(blackToGrey.Rgb);
        AssertGrey(whiteToGrey.Rgb);
        
        // colours created from HCT therefore hue changes
        AssertTriplet(blackToWhite.Hct.Triplet, new(330, 0, 50));
        AssertTriplet(blackToGrey.Hct.Triplet, new(50, 0, 25));
        AssertTriplet(whiteToGrey.Hct.Triplet, new(20, 0, 75));
    }

    private static void AssertGrey(Rgb rgb)
    {
        Assert.That(rgb.R, Is.EqualTo(rgb.G).Within(0.05));
        Assert.That(rgb.G, Is.EqualTo(rgb.B).Within(0.05));
        Assert.That(rgb.B, Is.EqualTo(rgb.R).Within(0.05));
    }
    
    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        TestUtils.AssertTriplet(actual, expected, TestUtils.MixTolerance);
    }
}