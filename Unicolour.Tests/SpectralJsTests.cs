using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class SpectralJsTests
{
    private const int Count = 9;
    private static readonly double[] Distances = [0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1];
    private static readonly string[] BlueToYellowHex = [ "#002185", "#003857", "#005348", "#007344", "#3C933E", "#6FAF35", "#A2C428", "#D4D015", "#FCD200"];
    private static readonly string[] PhthaloBlueToTitaniumWhiteHex = [ "#1E1439", "#34265A", "#5E4B8D", "#8774B4", "#AB9ACE", "#C7BBE0", "#DED6ED", "#EFECF6", "#F9FAF9"]; // based on Datasets.ArtistPaint at full concentration
    private static readonly string[] BlackToWhiteHex = [ "#000000",  "#262626", "#555555", "#818181", "#A6A6A6", "#C4C4C4", "#DCDCDC", "#EFEFEF", "#FFFFFF" ];
    private static readonly string[] RedToGreenHex = ["#FF0000", "#C11C15", "#96251C", "#81301E", "#7B411F", "#7D5920", "#807D1F", "#75B31B", "#00FF00"];
    private static readonly string[] PinkToCyanHex = [ "#FF1493", "#B03597", "#8C3BA1", "#7D46B2", "#775AC6", "#7378DA", "#6CA0EA", "#59CFF7", "#00FFFF"];
    
    private static readonly Unicolour Blue = new(BlueToYellowHex.First());
    private static readonly Unicolour Yellow = new(BlueToYellowHex.Last());
    private static readonly Unicolour PhthaloBlue = new(PhthaloBlueToTitaniumWhiteHex.First());
    private static readonly Unicolour TitaniumWhite = new(PhthaloBlueToTitaniumWhiteHex.Last());
    private static readonly Unicolour Black = new(BlackToWhiteHex.First());
    private static readonly Unicolour White = new(BlackToWhiteHex.Last());
    private static readonly Unicolour Red = new(RedToGreenHex.First());
    private static readonly Unicolour Green = new(RedToGreenHex.Last());
    private static readonly Unicolour Pink = new(PinkToCyanHex.First());
    private static readonly Unicolour Cyan = new(PinkToCyanHex.Last());
    
    // default example on https://onedayofcrypto.art/
    [Test, Sequential] 
    public void BlueToYellowMix(
        [ValueSource(nameof(Distances))] double distance, 
        [ValueSource(nameof(BlueToYellowHex))] string expected)
    {
        var colour = SpectralJs.Mix([Blue, Yellow], [1 - distance, distance]);
        AssertDifference(colour, expected);
    }

    [Test]
    public void BlueToYellowPalette() => AssertPalette(Blue, Yellow, Count, BlueToYellowHex);
    
    // showcases the difference between single-constant (e.g. spectral.js) vs two-constant (e.g. mixbox)
    [Test, Sequential]
    public void PhthaloBlueToTitaniumWhiteMix(
        [ValueSource(nameof(Distances))] double distance, 
        [ValueSource(nameof(PhthaloBlueToTitaniumWhiteHex))] string expected)
    {
        var colour = SpectralJs.Mix([PhthaloBlue, TitaniumWhite], [1 - distance, distance]);
        AssertDifference(colour, expected);
    }

    [Test]
    public void PhthaloBlueToTitaniumWhitePalette() => AssertPalette(PhthaloBlue, TitaniumWhite, Count, PhthaloBlueToTitaniumWhiteHex);


    [Test, Sequential] 
    public void BlackToWhiteMix(
        [ValueSource(nameof(Distances))] double distance, 
        [ValueSource(nameof(BlackToWhiteHex))] string expected)
    {
        var colour = SpectralJs.Mix([Black, White], [1 - distance, distance]);
        AssertDifference(colour, expected);
    }

    [Test]
    public void BlackToWhitePalette() => AssertPalette(Black, White, Count, BlackToWhiteHex);

    [Test, Sequential]
    public void RedToGreenMix(
        [ValueSource(nameof(Distances))] double distance, 
        [ValueSource(nameof(RedToGreenHex))] string expected)
    {
        var colour = SpectralJs.Mix([Red, Green], [1 - distance, distance]);
        AssertDifference(colour, expected);
    }

    [Test]
    public void RedToGreenPalette() => AssertPalette(Red, Green, Count, RedToGreenHex);
    
    [Test, Sequential]
    public void PinkToCyanMix(
        [ValueSource(nameof(Distances))] double distance, 
        [ValueSource(nameof(PinkToCyanHex))] string expected)
    {
        var colour = SpectralJs.Mix([Pink, Cyan], [1 - distance, distance]);
        AssertDifference(colour, expected);
    }

    [Test]
    public void PinkToCyanPalette() => AssertPalette(Pink, Cyan, Count, PinkToCyanHex);
    
    
    [Test]
    public void MixNegativeRgb()
    {
        var negative = new Unicolour(ColourSpace.RgbLinear, -0.5, -0.5, -0.5);
        var positive = new Unicolour(ColourSpace.RgbLinear, 0.5, 0.5, 0.5);
        var colour = SpectralJs.Mix([negative, positive], [0.5, 0.5]);
        TestUtils.AssertTriplet<Xyz>(colour, new(double.NaN, double.NaN, double.NaN), 0);
    }
    
    [Test]
    public void MixNoConcentration()
    {
        var colour = SpectralJs.Mix([StandardRgb.Black, StandardRgb.White], [-0.5, 0.0]);
        TestUtils.AssertTriplet<Xyz>(colour, new(double.NaN, double.NaN, double.NaN), 0);
    }
    
    [Test]
    public void PaletteTwo() => AssertPalette(Blue, Yellow, 2, [BlueToYellowHex.First(), BlueToYellowHex.Last()]);
    
    [Test]
    public void PaletteOne() => AssertPalette(Blue, Yellow, 1, [BlueToYellowHex[4]]);
    
    [Test]
    public void PaletteZero() => AssertPalette(Blue, Yellow, 0, []);
    
    [Test]
    public void PaletteNegative() => AssertPalette(Blue, Yellow, -1, []);
    
    // Unicolour intentionally does not follow Spectral.js implementation exactly
    // in order to generate more accurate reflectance curves of colours being mixed
    // however it should produce a result relatively close to Spectral.js
    private static void AssertDifference(Unicolour colour, string expectedHex)
    {
        var expected = new Unicolour(expectedHex);

        var expectedRgb = expected.RgbLinear.Triplet.ToArray();
        var actualRgb = colour.RgbLinear.Triplet.ToArray();
        var rgbDifferences = expectedRgb.Zip(actualRgb, (a, b) => Math.Abs(a - b)).ToArray();
        Assert.That(rgbDifferences.Max(), Is.LessThan(0.125));
        Assert.That(rgbDifferences.Sum(), Is.LessThan(0.175));
    }
    
    private static void AssertPalette(Unicolour start, Unicolour end, int count, string[] expected)
    {
        var palette = SpectralJs.Palette(start, end, count).ToArray();
        Assert.That(palette.Length, Is.EqualTo(Math.Max(count, 0)));
        for (var i = 0; i < palette.Length; i++)
        {
            AssertDifference(palette[i], expected[i]);
        }
    }
}