using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// greyscale RGB has no hue - shouldn't assume to start at red (0 degrees) when mixing
// greyscale CAM02 has a hue so it should be used (it just can't be seen until there is some J/Q & C/M/S)
public class MixGreyscaleCam02Tests
{
    private static readonly Dictionary<ColourSpace, ColourTriplet> RgbWhiteAsCam = new()
    {
        { ColourSpace.Cam02Jch, new(100, 3, 211) },
        { ColourSpace.Cam02Jmh, new(100, 2, 211) },
        { ColourSpace.Cam02Jsh, new(100, 14, 211) },
        { ColourSpace.Cam02Qch, new(124, 3, 211) },
        { ColourSpace.Cam02Qmh, new(124, 2, 211) },
        { ColourSpace.Cam02Qsh, new(124, 14, 211) }
    };
    
    [Test]
    public void GreyscaleStartColour(
        [Values(ColourSpace.Cam02Jch, ColourSpace.Cam02Jmh, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qmh, ColourSpace.Cam02Qsh)]
        ColourSpace cam02Space)
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var cam02Black = new Unicolour(cam02Space, 0, 0, 180); // no J/Q = black
        var cam02White = new Unicolour(cam02Space, 100, 120, 180); // no C/M/S = greyscale

        var green = new Unicolour(cam02Space, 100, 120, 120);
        var fromRgbBlack = rgbBlack.Mix(green, cam02Space, premultiplyAlpha: false);
        var fromRgbWhite = rgbWhite.Mix(green, cam02Space, premultiplyAlpha: false);
        var fromCam02Black = cam02Black.Mix(green, cam02Space, premultiplyAlpha: false);
        var fromCam02White = cam02White.Mix(green, cam02Space, premultiplyAlpha: false);

        // because RGB white has a non-zero CAM chroma, cannot know exactly what the mixed result will be
        // however, not really a problem as this test focuses on hue
        var (rgbWhiteJQ, rgbWhiteCMS, _) = RgbWhiteAsCam[cam02Space];

        var expectedJQ = (rgbWhiteJQ + 100) * 0.5;
        var actualJQ = fromRgbWhite.GetTriplet(cam02Space)[0];
        Assert.That(actualJQ, Is.EqualTo(expectedJQ).Within(0.5));
        
        var expectedCMS = (rgbWhiteCMS + 120) * 0.5;
        var actualCMS = fromRgbWhite.GetTriplet(cam02Space)[1];
        Assert.That(actualCMS, Is.EqualTo(expectedCMS).Within(0.5));

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(fromRgbBlack.GetTriplet(cam02Space), new(50, 60, 120));
        AssertTriplet(fromRgbWhite.GetTriplet(cam02Space), new(actualJQ, actualCMS, 120));
        AssertTriplet(fromCam02Black.GetTriplet(cam02Space), new(50, 60, 150));
        AssertTriplet(fromCam02White.GetTriplet(cam02Space), new(100, 120, 150));
    }

    [Test]
    public void GreyscaleEndColour(
        [Values(ColourSpace.Cam02Jch, ColourSpace.Cam02Jmh, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qmh, ColourSpace.Cam02Qsh)]
        ColourSpace cam02Space)
    {
        var rgbBlack = new Unicolour(ColourSpace.Rgb255, 0, 0, 0);
        var rgbWhite = new Unicolour(ColourSpace.Rgb255, 255, 255, 255);
        var cam02Black = new Unicolour(cam02Space, 0, 0, 180); // no J/Q = black
        var cam02White = new Unicolour(cam02Space, 100, 120, 180); // no C/M/S = greyscale
        
        var blue = new Unicolour(cam02Space, 100, 120, 240);
        var toRgbBlack = blue.Mix(rgbBlack, cam02Space, premultiplyAlpha: false);
        var toRgbWhite = blue.Mix(rgbWhite, cam02Space, premultiplyAlpha: false);
        var toCam02Black = blue.Mix(cam02Black, cam02Space, premultiplyAlpha: false);
        var toCam02White = blue.Mix(cam02White, cam02Space, premultiplyAlpha: false);
        
        // because RGB white has a non-zero CAM chroma, cannot know exactly what the mixed result will be
        // however, not really a problem as this test focuses on hue
        var (rgbWhiteJQ, rgbWhiteCMS, _) = RgbWhiteAsCam[cam02Space];

        var expectedJQ = (rgbWhiteJQ + 100) * 0.5;
        var actualJQ = toRgbWhite.GetTriplet(cam02Space)[0];
        Assert.That(actualJQ, Is.EqualTo(expectedJQ).Within(0.5));
        
        var expectedCMS = (rgbWhiteCMS + 120) * 0.5;
        var actualCMS = toRgbWhite.GetTriplet(cam02Space)[1];
        Assert.That(actualCMS, Is.EqualTo(expectedCMS).Within(0.5));

        // greyscale mixes differently depending on the initial colour space
        AssertTriplet(toRgbBlack.GetTriplet(cam02Space), new(50, 60, 240));
        AssertTriplet(toRgbWhite.GetTriplet(cam02Space), new(actualJQ, actualCMS, 240));
        AssertTriplet(toCam02Black.GetTriplet(cam02Space), new(50, 60, 210));
        AssertTriplet(toCam02White.GetTriplet(cam02Space), new(100, 120, 210));
    }
    
    [Test]
    public void GreyscaleBothRgbColours(
        [Values(ColourSpace.Cam02Jch, ColourSpace.Cam02Jmh, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qmh, ColourSpace.Cam02Qsh)]
        ColourSpace cam02Space)
    {
        var black = new Unicolour(ColourSpace.Rgb, 0, 0, 0.0);
        var white = new Unicolour(ColourSpace.Rgb, 1, 1, 1.0);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);

        var blackToWhite = black.Mix(white, cam02Space, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, cam02Space, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, cam02Space, premultiplyAlpha: false);

        // colours created from RGB therefore hue does not change
        // (except for CAM02 for RGB-black, which converts to a different hue than other greyscales)
        var blackHue = black.GetTriplet(cam02Space)[2];
        var whiteHue = white.GetTriplet(cam02Space)[2];
        var greyHue = grey.GetTriplet(cam02Space)[2];
        var blackToWhiteHue = blackToWhite.GetTriplet(cam02Space)[2];
        var blackToGreyHue = blackToGrey.GetTriplet(cam02Space)[2];
        var whiteToGreyHue = whiteToGrey.GetTriplet(cam02Space)[2];
        
        Assert.That(blackHue, Is.EqualTo(0));
        Assert.That(blackToWhiteHue, Is.EqualTo(blackToGreyHue).Within(0.001));
        Assert.That(whiteToGreyHue, Is.EqualTo(whiteHue).Within(0.001));
        Assert.That(whiteToGreyHue, Is.EqualTo(greyHue).Within(0.001));
    }
    
    [Test]
    public void GreyscaleBothCam02Colours(
        [Values(ColourSpace.Cam02Jch, ColourSpace.Cam02Jmh, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qmh, ColourSpace.Cam02Qsh)]
        ColourSpace cam02Space)
    {
        var black = new Unicolour(cam02Space, 0, 0, 0);
        var white = new Unicolour(cam02Space, 100, 0, 300);
        var grey = new Unicolour(cam02Space, 50, 0, 100);

        var blackToWhite = black.Mix(white, cam02Space, premultiplyAlpha: false);
        var blackToGrey = black.Mix(grey, cam02Space, premultiplyAlpha: false);
        var whiteToGrey = white.Mix(grey, cam02Space, premultiplyAlpha: false);
        
        AssertGrey(blackToWhite.Rgb);
        AssertGrey(blackToGrey.Rgb);
        AssertGrey(whiteToGrey.Rgb);
        
        // colours created from CAM02 therefore hue changes
        AssertTriplet(blackToWhite.GetTriplet(cam02Space), new(50, 0, 330, hueIndex: 2));
        AssertTriplet(blackToGrey.GetTriplet(cam02Space), new(25, 0, 50, hueIndex: 2));
        AssertTriplet(whiteToGrey.GetTriplet(cam02Space), new(75, 0, 20, hueIndex: 2));
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