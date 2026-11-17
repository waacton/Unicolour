using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale Hellwig has a hue so it should be used (it just can't be seen until there is some J/Q & C/M/S)
public class MixGreyscaleHellwigTests
{
    private static readonly Dictionary<ColourSpace, ColourTriplet> RgbWhiteAsCam = new()
    {
        { ColourSpace.HellwigJch, new(100, 1, 210) },
        { ColourSpace.HellwigJmh, new(100, 1, 210) },
        { ColourSpace.HellwigJsh, new(100, 1, 210) },
        { ColourSpace.HellwigQch, new(74, 1, 210) },
        { ColourSpace.HellwigQmh, new(74, 1, 210) },
        { ColourSpace.HellwigQsh, new(74, 1, 210) }
    };
    
    [Test]
    public void GreyscaleStartColour(
        [Values(ColourSpace.HellwigJch, ColourSpace.HellwigJmh, ColourSpace.HellwigJsh, ColourSpace.HellwigQch, ColourSpace.HellwigQmh, ColourSpace.HellwigQsh)]
        ColourSpace hellwigSpace)
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hellwigBlack = new Unicolour(hellwigSpace, 0, 0, 180); // no J/Q = black
        var hellwigWhite = new Unicolour(hellwigSpace, 100, 120, 180); // no C/M/S = greyscale

        var green = new Unicolour(hellwigSpace, 100, 120, 120);
        var fromRgbBlack = rgbBlack.Mix(green, hellwigSpace, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, hellwigSpace, premultiplyAlpha: false);
        var fromHellwigBlack = hellwigBlack.Mix(green, hellwigSpace, premultiplyAlpha: false);
        var fromHellwigWhite = hellwigWhite.Mix(green, hellwigSpace, premultiplyAlpha: false);

        // because RGB white has a non-zero CAM chroma, cannot know exactly what the mixed result will be
        // however, not really a problem as this test focuses on hue
        var (rgbWhiteJQ, rgbWhiteCMS, _) = RgbWhiteAsCam[hellwigSpace];

        var expectedJQ = (rgbWhiteJQ + 100) * 0.5;
        var actualJQ = fromRgbWhite.GetTriplet(hellwigSpace)[0];
        Assert.That(actualJQ, Is.EqualTo(expectedJQ).Within(0.5));
        
        var expectedCMS = (rgbWhiteCMS + 120) * 0.5;
        var actualCMS = fromRgbWhite.GetTriplet(hellwigSpace)[1];
        Assert.That(actualCMS, Is.EqualTo(expectedCMS).Within(0.5));

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.GetTriplet(hellwigSpace), new(50, 60, 120));
        AssertTriplet(fromRgbWhite.GetTriplet(hellwigSpace), new(actualJQ, actualCMS, 120));
        AssertTriplet(fromHellwigBlack.GetTriplet(hellwigSpace), new(50, 60, 150));
        AssertTriplet(fromHellwigWhite.GetTriplet(hellwigSpace), new(100, 120, 150));
    }

    [Test]
    public void GreyscaleEndColour(
        [Values(ColourSpace.HellwigJch, ColourSpace.HellwigJmh, ColourSpace.HellwigJsh, ColourSpace.HellwigQch, ColourSpace.HellwigQmh, ColourSpace.HellwigQsh)]
        ColourSpace hellwigSpace)
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var hellwigBlack = new Unicolour(hellwigSpace, 0, 0, 180); // no J/Q = black
        var hellwigWhite = new Unicolour(hellwigSpace, 100, 120, 180); // no C/M/S = greyscale
        
        var blue = new Unicolour(hellwigSpace, 100, 120, 240);
        var toRgbBlack = blue.Mix(rgbBlack, hellwigSpace, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, hellwigSpace, premultiplyAlpha: false);
        var toHellwigBlack = blue.Mix(hellwigBlack, hellwigSpace, premultiplyAlpha: false);
        var toHellwigWhite = blue.Mix(hellwigWhite, hellwigSpace, premultiplyAlpha: false);
        
        // because RGB white has a non-zero CAM chroma, cannot know exactly what the mixed result will be
        // however, not really a problem as this test focuses on hue
        var (rgbWhiteJQ, rgbWhiteCMS, _) = RgbWhiteAsCam[hellwigSpace];

        var expectedJQ = (rgbWhiteJQ + 100) * 0.5;
        var actualJQ = toRgbWhite.GetTriplet(hellwigSpace)[0];
        Assert.That(actualJQ, Is.EqualTo(expectedJQ).Within(0.5));
        
        var expectedCMS = (rgbWhiteCMS + 120) * 0.5;
        var actualCMS = toRgbWhite.GetTriplet(hellwigSpace)[1];
        Assert.That(actualCMS, Is.EqualTo(expectedCMS).Within(0.5));

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.GetTriplet(hellwigSpace), new(50, 60, 240));
        AssertTriplet(toRgbWhite.GetTriplet(hellwigSpace), new(actualJQ, actualCMS, 240));
        AssertTriplet(toHellwigBlack.GetTriplet(hellwigSpace), new(50, 60, 210));
        AssertTriplet(toHellwigWhite.GetTriplet(hellwigSpace), new(100, 120, 210));
    }
    
    [Test]
    public void GreyscaleBothRgbColours(
        [Values(ColourSpace.HellwigJch, ColourSpace.HellwigJmh, ColourSpace.HellwigJsh, ColourSpace.HellwigQch, ColourSpace.HellwigQmh, ColourSpace.HellwigQsh)]
        ColourSpace hellwigSpace)
    {
        var black = new Unicolour(ColourSpace.Rgb, 0, 0, 0.0);
        var white = new Unicolour(ColourSpace.Rgb, 1, 1, 1.0);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, hellwigSpace, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, hellwigSpace, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, hellwigSpace, premultiplyAlpha: false);

        // colours created from RGB therefore hue does not change
        // (except for Hellwig for RGB-black, which converts to a different hue than other greyscales)
        var blackHue = black.GetTriplet(hellwigSpace)[2];
        var whiteHue = white.GetTriplet(hellwigSpace)[2];
        var greyHue = grey.GetTriplet(hellwigSpace)[2];
        var blackToWhiteHue = blackToWhite.GetTriplet(hellwigSpace)[2];
        var blackToGreyHue = blackToGrey.GetTriplet(hellwigSpace)[2];
        var whiteToGreyHue = whiteToGrey.GetTriplet(hellwigSpace)[2];
        
        Assert.That(blackHue, Is.EqualTo(0));
        Assert.That(blackToWhiteHue, Is.EqualTo(blackToGreyHue).Within(0.001));
        Assert.That(whiteToGreyHue, Is.EqualTo(whiteHue).Within(0.001));
        Assert.That(whiteToGreyHue, Is.EqualTo(greyHue).Within(0.001));
    }
    
    [Test]
    public void GreyscaleBothHellwigColours(
        [Values(ColourSpace.HellwigJch, ColourSpace.HellwigJmh, ColourSpace.HellwigJsh, ColourSpace.HellwigQch, ColourSpace.HellwigQmh, ColourSpace.HellwigQsh)]
        ColourSpace hellwigSpace)
    {
        var black = new Unicolour(hellwigSpace, 0, 0, 0);
        var white = new Unicolour(hellwigSpace, 100, 0, 300);
        var grey = new Unicolour(hellwigSpace, 50, 0, 100);

        var blackToWhite = black.Mix(white, hellwigSpace, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, hellwigSpace, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, hellwigSpace, premultiplyAlpha: false);
        
        AssertGrey(blackToWhite.Rgb);
        AssertGrey(blackToGrey.Rgb);
        AssertGrey(whiteToGrey.Rgb);
        
        // colours created from CAM02 therefore hue changes
        AssertTriplet(blackToWhite.GetTriplet(hellwigSpace), new(50, 0, 330, hueIndex: 2));
        AssertTriplet(blackToGrey.GetTriplet(hellwigSpace), new(25, 0, 50, hueIndex: 2));
        AssertTriplet(whiteToGrey.GetTriplet(hellwigSpace), new(75, 0, 20, hueIndex: 2));
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