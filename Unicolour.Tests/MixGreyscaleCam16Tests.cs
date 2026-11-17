using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale CAM16 has a hue so it should be used (it just can't be seen until there is some J/Q & C/M/S)
public class MixGreyscaleCam16Tests
{
    private static readonly Dictionary<ColourSpace, ColourTriplet> RgbWhiteAsCam = new()
    {
        { ColourSpace.Cam16Jch, new(100, 3, 210) },
        { ColourSpace.Cam16Jmh, new(100, 2, 210) },
        { ColourSpace.Cam16Jsh, new(100, 13, 210) },
        { ColourSpace.Cam16Qch, new(124, 3, 210) },
        { ColourSpace.Cam16Qmh, new(124, 2, 210) },
        { ColourSpace.Cam16Qsh, new(124, 13, 210) }
    };
    
    [Test]
    public void GreyscaleStartColour(
        [Values(ColourSpace.Cam16Jch, ColourSpace.Cam16Jmh, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qmh, ColourSpace.Cam16Qsh)]
        ColourSpace cam16Space)
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var cam16Black = new Unicolour(cam16Space, 0, 0, 180); // no J/Q = black
        var cam16White = new Unicolour(cam16Space, 100, 120, 180); // no C/M/S = greyscale

        var green = new Unicolour(cam16Space, 100, 120, 120);
        var fromRgbBlack = rgbBlack.Mix(green, cam16Space, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, cam16Space, premultiplyAlpha: false);
        var fromCam16Black = cam16Black.Mix(green, cam16Space, premultiplyAlpha: false);
        var fromCam16White = cam16White.Mix(green, cam16Space, premultiplyAlpha: false);

        // because RGB white has a non-zero CAM chroma, cannot know exactly what the mixed result will be
        // however, not really a problem as this test focuses on hue
        var (rgbWhiteJQ, rgbWhiteCMS, _) = RgbWhiteAsCam[cam16Space];

        var expectedJQ = (rgbWhiteJQ + 100) * 0.5;
        var actualJQ = fromRgbWhite.GetTriplet(cam16Space)[0];
        Assert.That(actualJQ, Is.EqualTo(expectedJQ).Within(0.5));
        
        var expectedCMS = (rgbWhiteCMS + 120) * 0.5;
        var actualCMS = fromRgbWhite.GetTriplet(cam16Space)[1];
        Assert.That(actualCMS, Is.EqualTo(expectedCMS).Within(0.5));

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.GetTriplet(cam16Space), new(50, 60, 120));
        AssertTriplet(fromRgbWhite.GetTriplet(cam16Space), new(actualJQ, actualCMS, 120));
        AssertTriplet(fromCam16Black.GetTriplet(cam16Space), new(50, 60, 150));
        AssertTriplet(fromCam16White.GetTriplet(cam16Space), new(100, 120, 150));
    }

    [Test]
    public void GreyscaleEndColour(
        [Values(ColourSpace.Cam16Jch, ColourSpace.Cam16Jmh, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qmh, ColourSpace.Cam16Qsh)]
        ColourSpace cam16Space)
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var cam16Black = new Unicolour(cam16Space, 0, 0, 180); // no J/Q = black
        var cam16White = new Unicolour(cam16Space, 100, 120, 180); // no C/M/S = greyscale
        
        var blue = new Unicolour(cam16Space, 100, 120, 240);
        var toRgbBlack = blue.Mix(rgbBlack, cam16Space, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, cam16Space, premultiplyAlpha: false);
        var toCam16Black = blue.Mix(cam16Black, cam16Space, premultiplyAlpha: false);
        var toCam16White = blue.Mix(cam16White, cam16Space, premultiplyAlpha: false);
        
        // because RGB white has a non-zero CAM chroma, cannot know exactly what the mixed result will be
        // however, not really a problem as this test focuses on hue
        var (rgbWhiteJQ, rgbWhiteCMS, _) = RgbWhiteAsCam[cam16Space];

        var expectedJQ = (rgbWhiteJQ + 100) * 0.5;
        var actualJQ = toRgbWhite.GetTriplet(cam16Space)[0];
        Assert.That(actualJQ, Is.EqualTo(expectedJQ).Within(0.5));
        
        var expectedCMS = (rgbWhiteCMS + 120) * 0.5;
        var actualCMS = toRgbWhite.GetTriplet(cam16Space)[1];
        Assert.That(actualCMS, Is.EqualTo(expectedCMS).Within(0.5));

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.GetTriplet(cam16Space), new(50, 60, 240));
        AssertTriplet(toRgbWhite.GetTriplet(cam16Space), new(actualJQ, actualCMS, 240));
        AssertTriplet(toCam16Black.GetTriplet(cam16Space), new(50, 60, 210));
        AssertTriplet(toCam16White.GetTriplet(cam16Space), new(100, 120, 210));
    }
    
    [Test]
    public void GreyscaleBothRgbColours(
        [Values(ColourSpace.Cam16Jch, ColourSpace.Cam16Jmh, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qmh, ColourSpace.Cam16Qsh)]
        ColourSpace cam16Space)
    {
        var black = new Unicolour(ColourSpace.Rgb, 0, 0, 0.0);
        var white = new Unicolour(ColourSpace.Rgb, 1, 1, 1.0);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, cam16Space, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, cam16Space, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, cam16Space, premultiplyAlpha: false);

        // colours created from RGB therefore hue does not change
        // (except for CAM16 for RGB-black, which converts to a different hue than other greyscales)
        var blackHue = black.GetTriplet(cam16Space)[2];
        var whiteHue = white.GetTriplet(cam16Space)[2];
        var greyHue = grey.GetTriplet(cam16Space)[2];
        var blackToWhiteHue = blackToWhite.GetTriplet(cam16Space)[2];
        var blackToGreyHue = blackToGrey.GetTriplet(cam16Space)[2];
        var whiteToGreyHue = whiteToGrey.GetTriplet(cam16Space)[2];
        
        Assert.That(blackHue, Is.EqualTo(0));
        Assert.That(blackToWhiteHue, Is.EqualTo(blackToGreyHue).Within(0.001));
        Assert.That(whiteToGreyHue, Is.EqualTo(whiteHue).Within(0.001));
        Assert.That(whiteToGreyHue, Is.EqualTo(greyHue).Within(0.001));
    }
    
    [Test]
    public void GreyscaleBothCam16Colours(
        [Values(ColourSpace.Cam16Jch, ColourSpace.Cam16Jmh, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qmh, ColourSpace.Cam16Qsh)]
        ColourSpace cam16Space)
    {
        var black = new Unicolour(cam16Space, 0, 0, 0);
        var white = new Unicolour(cam16Space, 100, 0, 300);
        var grey = new Unicolour(cam16Space, 50, 0, 100);

        var blackToWhite = black.Mix(white, cam16Space, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, cam16Space, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, cam16Space, premultiplyAlpha: false);
        
        AssertGrey(blackToWhite.Rgb);
        AssertGrey(blackToGrey.Rgb);
        AssertGrey(whiteToGrey.Rgb);
        
        // colours created from CAM02 therefore hue changes
        AssertTriplet(blackToWhite.GetTriplet(cam16Space), new(50, 0, 330, hueIndex: 2));
        AssertTriplet(blackToGrey.GetTriplet(cam16Space), new(25, 0, 50, hueIndex: 2));
        AssertTriplet(whiteToGrey.GetTriplet(cam16Space), new(75, 0, 20, hueIndex: 2));
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