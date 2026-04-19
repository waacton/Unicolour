using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Experimental;

namespace Wacton.Unicolour.Tests;

public class ColourWheelPigmentTests
{
    private static readonly ColourWheel ColourWheel = ColourWheel.From(
        red: ArtistPaint.QuinacridoneRed,
        yellow: ArtistPaint.BismuthVanadateYellow,
        blue: ArtistPaint.CeruleanBlueChromium,
        white: ArtistPaint.TitaniumWhite,
        black: ArtistPaint.BoneBlack
    );

    private static readonly Unicolour Red = new([ArtistPaint.QuinacridoneRed], [1.0]);
    private static readonly Unicolour Yellow = new([ArtistPaint.BismuthVanadateYellow], [1.0]);
    private static readonly Unicolour Blue = new([ArtistPaint.CeruleanBlueChromium], [1.0]);
    private static readonly Unicolour White = new([ArtistPaint.TitaniumWhite], [1.0]);
    private static readonly Unicolour Black = new([ArtistPaint.BoneBlack], [1.0]);
    
    private static readonly TestCaseData[] PureData =
    [
        new(0, Red), new(0.00000000001, Red), new(360, Red), new(720, Red), new(-0.00000000001, Red), new(-360, Red), new(-720, Red),
        new(5, Red),
        new(115, Yellow),
        new(120, Yellow), new(480, Yellow), new(-240, Yellow), new(120.00000000001, Yellow), new(119.99999999999, Yellow),
        new(125, Yellow),
        new(235, Blue),
        new(240, Blue), new(600, Blue), new(-120, Blue), new(240.00000000001, Blue), new(239.99999999999, Blue),
        new(245, Blue),
        new(355, Red)
    ];
    
    [TestCaseSource(nameof(PureData))]
    public void Pure(double hue, Unicolour expectedClosest)
    {
        var colour = ColourWheel.Pure(hue);
        var expectedDistant = new[] { Red, Yellow, Blue }.Except([expectedClosest]);

        var expectedSmallestDelta = colour.Difference(expectedClosest, DeltaE.Ciede2000);
        var expectedLargerDeltas = expectedDistant.Select(x => colour.Difference(x, DeltaE.Ciede2000)).ToArray();
        foreach (var expectedLargerDelta in expectedLargerDeltas)
        {
            Assert.That(expectedSmallestDelta, Is.LessThan(expectedLargerDelta));
        }
    }
    
    [Test]
    public void PureNotNumber([Values(double.NaN, double.PositiveInfinity, double.NegativeInfinity)] double hue)
    {
        var colour = ColourWheel.Pure(hue);
        Assert.That(colour.Xyz.IsNaN, Is.True);
    }
    
    [Test]
    public void Tint()
    {
        // pigment clamps negative weights to zero, hence "or equal to"
        double[] weights = [double.NegativeInfinity, -1, -0.5, 0, 0.5, 1];
        var colours = weights.Select(x => ColourWheel.Tint(180, x)).ToArray();
        var deltas = colours.Select(x => x.Difference(White, DeltaE.Ciede2000)).ToArray();
        for (var i = 0; i < deltas.Length - 1; i++)
        {
            Assert.That(deltas[i], Is.GreaterThanOrEqualTo(deltas[i + 1]));
        }
    }
        
    [Test]
    public void TintNotNumber([Values(double.NaN, double.PositiveInfinity)] double weight)
    {
        var colour = ColourWheel.Tint(180, weight);
        Assert.That(colour.Xyz.IsNaN, Is.True);
    }
    
    [Test]
    public void Shade()
    {
        // pigment clamps negative weights to zero, hence "or equal to"
        double[] weights = [double.NegativeInfinity, -1, -0.5, 0, 0.5, 1];
        var colours = weights.Select(x => ColourWheel.Shade(180, x)).ToArray();
        var deltas = colours.Select(x => x.Difference(Black, DeltaE.Ciede2000)).ToArray();
        for (var i = 0; i < deltas.Length - 1; i++)
        {
            Assert.That(deltas[i], Is.GreaterThanOrEqualTo(deltas[i + 1]));
        }
    }
        
    [Test]
    public void ShadeNotNumber([Values(double.NaN, double.PositiveInfinity)] double weight)
    {
        var colour = ColourWheel.Shade(180, weight);
        Assert.That(colour.Xyz.IsNaN, Is.True);
    }
    
    [Test]
    public void Tone()
    {
        // pigment clamps negative weights to zero, hence "or equal to"
        double[] weights = [double.NegativeInfinity, -1, -0.5, 0, 0.5, 1];
        var colours = weights.Select(x => ColourWheel.Tone(180, x)).ToArray();
        var deltas = colours.Select(x => x.Difference(White, DeltaE.Ciede2000)).ToArray();
        for (var i = 0; i < deltas.Length - 1; i++)
        {
            Assert.That(deltas[i], Is.GreaterThanOrEqualTo(deltas[i + 1]));
        }
    }
        
    [Test]
    public void ToneNotNumber([Values(double.NaN, double.PositiveInfinity)] double weight)
    {
        var colour = ColourWheel.Tone(180, weight);
        Assert.That(colour.Xyz.IsNaN, Is.True);
    }

    [TestCase(0, new[] { 1.0, 0.0, 0.0, 0.0, 0.0 })]
    [TestCase(30, new[] { 1.0, 0.5, 0.0, 0.0, 0.0 })]
    [TestCase(60, new[] { 1.0, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(90, new[] { 0.5, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(120, new[] { 0.0, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(150, new[] { 0.0, 1.0, 0.5, 0.0, 0.0 })]
    [TestCase(180, new[] { 0.0, 1.0, 1.0, 0.0, 0.0 })]
    [TestCase(210, new[] { 0.0, 0.5, 1.0, 0.0, 0.0 })]
    [TestCase(240, new[] { 0.0, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(270, new[] { 0.5, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(300, new[] { 1.0, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(330, new[] { 1.0, 0.0, 0.5, 0.0, 0.0 })]
    [TestCase(0 + 360, new[] { 1.0, 0.0, 0.0, 0.0, 0.0 })]
    [TestCase(30 + 360, new[] { 1.0, 0.5, 0.0, 0.0, 0.0 })]
    [TestCase(60 + 360, new[] { 1.0, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(90 + 360, new[] { 0.5, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(120 + 360, new[] { 0.0, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(150 + 360, new[] { 0.0, 1.0, 0.5, 0.0, 0.0 })]
    [TestCase(180 + 360, new[] { 0.0, 1.0, 1.0, 0.0, 0.0 })]
    [TestCase(210 + 360, new[] { 0.0, 0.5, 1.0, 0.0, 0.0 })]
    [TestCase(240 + 360, new[] { 0.0, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(270 + 360, new[] { 0.5, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(300 + 360, new[] { 1.0, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(330 + 360, new[] { 1.0, 0.0, 0.5, 0.0, 0.0 })]
    [TestCase(0 - 360, new[] { 1.0, 0.0, 0.0, 0.0, 0.0 })]
    [TestCase(30 - 360, new[] { 1.0, 0.5, 0.0, 0.0, 0.0 })]
    [TestCase(60 - 360, new[] { 1.0, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(90 - 360, new[] { 0.5, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(120 - 360, new[] { 0.0, 1.0, 0.0, 0.0, 0.0 })]
    [TestCase(150 - 360, new[] { 0.0, 1.0, 0.5, 0.0, 0.0 })]
    [TestCase(180 - 360, new[] { 0.0, 1.0, 1.0, 0.0, 0.0 })]
    [TestCase(210 - 360, new[] { 0.0, 0.5, 1.0, 0.0, 0.0 })]
    [TestCase(240 - 360, new[] { 0.0, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(270 - 360, new[] { 0.5, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(300 - 360, new[] { 1.0, 0.0, 1.0, 0.0, 0.0 })]
    [TestCase(330 - 360, new[] { 1.0, 0.0, 0.5, 0.0, 0.0 })]
    public void Weights(double hue, double[] expected)
    {
        var actual = PigmentColourWheel.Weights(hue);
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    private static Harmony[] AllHarmonies = Enum.GetValues<Harmony>();
    
    [Test]
    public void HarmonyHues(
        [ValueSource(nameof(AllHarmonies))] Harmony harmony, 
        [Values(0, 0.00000000001, 45, 90, 180, 360, 720, -0.00000000001, -45, -90, -180, -360, -720)] double hue)
    {
        // pigment wheel is much harder to test harmonies than hue wheel
        // however, all the underlying components (weights, pigment mixing, tint/shade/tone) are heavily tested
        var palette = ColourWheel.Harmony(hue, harmony);
        var expectedColour = ColourWheel.Pure(hue);
        Assert.Contains(expectedColour, palette);
    }
}