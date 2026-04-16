using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Hue;

namespace Wacton.Unicolour.Tests;

public class ColourWheelHueTests
{
    private static readonly TestCaseData[] PureHsbData =
    [
        new(0, StandardRgb.Red), new(0.00000000001, StandardRgb.Red), new(360, StandardRgb.Red), new(720, StandardRgb.Red), new(-0.00000000001, StandardRgb.Red), new(-360, StandardRgb.Red), new(-720, StandardRgb.Red),
        new(5, StandardRgb.Red),
        new(115, StandardRgb.Green),
        new(120, StandardRgb.Green), new(480, StandardRgb.Green), new(-240, StandardRgb.Green), new(120.00000000001, StandardRgb.Green), new(119.99999999999, StandardRgb.Green),
        new(125, StandardRgb.Green),
        new(235, StandardRgb.Blue),
        new(240, StandardRgb.Blue), new(600, StandardRgb.Blue), new(-120, StandardRgb.Blue), new(240.00000000001, StandardRgb.Blue), new(239.99999999999, StandardRgb.Blue),
        new(245, StandardRgb.Blue),
        new(355, StandardRgb.Red)
    ];
    
    private static readonly TestCaseData[] PureOklchData =
    [
        new(24.2, StandardRgb.Red),
        new(29.2, StandardRgb.Red), new(389.2, StandardRgb.Red), new(749.2, StandardRgb.Red), new(-330.8, StandardRgb.Red), new(-690.8, StandardRgb.Red),
        new(34.2, StandardRgb.Red),
        new(137.5, StandardRgb.Green),
        new(142.5, StandardRgb.Green), new(502.5, StandardRgb.Green), new(-217.5, StandardRgb.Green),
        new(147.5, StandardRgb.Green),
        new(259.1, StandardRgb.Blue),
        new(264.1, StandardRgb.Blue), new(624.1, StandardRgb.Blue), new(-95.9, StandardRgb.Blue),
        new(269.1, StandardRgb.Blue)
    ];

    [TestCaseSource(nameof(PureHsbData))]
    public void PureHsb(double hue, Unicolour expectedClosest) => Pure(hue, ColourSpace.Hsb, expectedClosest);
    
    [TestCaseSource(nameof(PureOklchData))]
    public void PureOklch(double hue, Unicolour expectedClosest) => Pure(hue, ColourSpace.Oklch, expectedClosest);

    private static void Pure(double hue, ColourSpace colourSpace, Unicolour expectedClosest)
    {
        var colourWheel = ColourWheel.From(colourSpace, StandardRgb.Yellow);
        var colour = colourWheel.Pure(hue);
        var expectedDistant = new[] { StandardRgb.Red, StandardRgb.Green, StandardRgb.Blue }.Except([expectedClosest]);

        var expectedSmallestDelta = colour.Difference(expectedClosest, DeltaE.Ciede2000);
        var expectedLargerDeltas = expectedDistant.Select(x => colour.Difference(x, DeltaE.Ciede2000)).ToArray();
        foreach (var expectedLargerDelta in expectedLargerDeltas)
        {
            Assert.That(expectedSmallestDelta, Is.LessThan(expectedLargerDelta));
        }
    }
    
    [Test]
    public void PureNotNumber(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [Values(double.NaN, double.PositiveInfinity, double.NegativeInfinity)] double hue)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(colourSpace, reference);
        var colour = colourWheel.Pure(hue);
        Assert.That(colour.GetRepresentation(colourSpace).IsNaN, Is.True);
    }
    
    [Test]
    public void Tint([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(colourSpace, reference);
        double[] weights = [-1, -0.5, 0, 0.5, 1];
        var colours = weights.Select(x => colourWheel.Tint(180, x)).ToArray();
        var deltas = colours.Select(x => x.Difference(StandardRgb.White, DeltaE.Ciede2000)).ToArray();
        
        if (!colours[0].GetRepresentation(colourSpace).HasHueComponent)
        {
            Assert.That(deltas, Has.All.NaN);
            return;
        }
        
        for (var i = 0; i < deltas.Length - 1; i++)
        {
            Assert.That(deltas[i], Is.GreaterThan(deltas[i + 1]));
        }
    }
        
    [Test]
    public void TintNotNumber(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [Values(double.NaN, double.PositiveInfinity, double.NegativeInfinity)] double weight)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(colourSpace, reference);
        var colour = colourWheel.Tint(180, weight);
        Assert.That(colour.GetRepresentation(colourSpace).IsNaN, Is.True);
    }
    
    [Test]
    public void Shade([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(colourSpace, reference);
        double[] weights = [-1, -0.5, 0, 0.5, 1];
        var colours = weights.Select(x => colourWheel.Shade(180, x)).ToArray();
        var deltas = colours.Select(x => x.Difference(StandardRgb.Black, DeltaE.Ciede2000)).ToArray();
        
        if (!colours[0].GetRepresentation(colourSpace).HasHueComponent)
        {
            Assert.That(deltas, Has.All.NaN);
            return;
        }
        
        for (var i = 0; i < deltas.Length - 1; i++)
        {
            Assert.That(deltas[i], Is.GreaterThan(deltas[i + 1]));
        }
    }
        
    [Test]
    public void ShadeNotNumber(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [Values(double.NaN, double.PositiveInfinity, double.NegativeInfinity)] double weight)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(colourSpace, reference);
        var colour = colourWheel.Shade(180, weight);
        Assert.That(colour.GetRepresentation(colourSpace).IsNaN, Is.True);
    }
    
    [Test]
    public void Tone([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(colourSpace, reference);
        double[] weights = [-1, -0.5, 0, 0.5, 1];
        var colours = weights.Select(x => colourWheel.Tone(180, x)).ToArray();
        var deltas = colours.Select(x => x.Difference(StandardRgb.White, DeltaE.Ciede2000)).ToArray();
        
        if (!colours[0].GetRepresentation(colourSpace).HasHueComponent)
        {
            Assert.That(deltas, Has.All.NaN);
            return;
        }

        for (var i = 0; i < deltas.Length - 1; i++)
        {
            Assert.That(deltas[i], Is.GreaterThan(deltas[i + 1]));
        }
    }
        
    [Test]
    public void ToneNotNumber(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [Values(double.NaN, double.PositiveInfinity, double.NegativeInfinity)] double weight)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(colourSpace, reference);
        var colour = colourWheel.Tone(180, weight);
        Assert.That(colour.GetRepresentation(colourSpace).IsNaN, Is.True);
    }
    
    private static readonly Dictionary<Harmony, double[]> HarmonyOffsets = new()
    {
        { Harmony.Monochromatic, [0.0, 0, 0, 0, 0] },
        { Harmony.MonochromaticTint, [0.0, 0, 0, 0, 0] },
        { Harmony.MonochromaticShade, [0.0, 0, 0, 0, 0] },
        { Harmony.MonochromaticTone, [0.0, 0, 0, 0, 0] },
        { Harmony.Analogous, [-30, 0.0, 30] },
        { Harmony.Complementary, [0.0, 180] },
        { Harmony.SplitComplementary, [0.0, 150, -150] },
        { Harmony.Triadic, [0.0, 120, -120] },
        { Harmony.TetradicRectangle, [0.0, 60, 180, 240] },
        { Harmony.TetradicSquare, [0.0, 90, 180, 270] }
    };

    private static Harmony[] AllHarmonies = Enum.GetValues<Harmony>();
    
    [Test]
    public void HarmonyHues(
        [ValueSource(nameof(AllHarmonies))] Harmony harmony, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [Values(0, 0.00000000001, 45, 90, 180, 360, 720, -0.00000000001, -45, -90, -180, -360, -720)] double hue)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(colourSpace, reference);
        var palette = colourWheel.Harmony(hue, harmony);
        
        var expectedColourSpace = colourSpace == ColourSpace.Rgb255 ? ColourSpace.Rgb : colourSpace;
        Assert.That(palette.Select(x => x.SourceColourSpace), Has.All.EqualTo(expectedColourSpace));
        
        var representations = palette.Select(x => x.GetRepresentation(colourSpace)).ToArray();
        if (!representations[0].HasHueComponent)
        {
            Assert.That(representations.Select(x => x.Triplet), Has.All.EqualTo(new ColourTriplet(double.NaN, double.NaN, double.NaN)));
            return;
        }
        
        var hues = representations.Select(x => x.ToArray()[x.HueIndex!.Value]).ToArray();
        if (colourSpace == ColourSpace.Wxy)
        {
            hues = hues.Select(x => FromWavelength(x, reference.Configuration.Xyz)).ToArray();
        }
        
        var expected = HarmonyOffsets[harmony].Select(offset => (hue + offset).WithHueModulo()).ToArray();
        var tolerance = colourSpace == ColourSpace.Wxy ? 00000000001 : 0;
        Assert.That(hues, Is.EqualTo(expected).Within(tolerance));
    }
    
    private static readonly TestCaseData[] RedHsbData =
    [
        new(Harmony.MonochromaticTint, new[] { "#FF0000", "#FF2020", "#FF4040", "#FF6060", "#FF7F7F" }),    // HSB (0, 1.0, 1.0) · (0, 0.875, 1.000) · (0, 0.75, 1.000) · (0, 0.625, 1.000) · (0, 0.5, 1.000)
        new(Harmony.MonochromaticShade, new[] { "#FF0000", "#DF1C1C", "#BF3030", "#9F3C3C", "#804040" }),   // HSB (0, 1.0, 1.0) · (0, 0.875, 0.875) · (0, 0.75, 0.750) · (0, 0.625, 0.625) · (0, 0.5, 0.500)
        new(Harmony.MonochromaticTone, new[] { "#FF0000", "#F71F1F", "#EE3C3C", "#E65656", "#DD6F6F" }),    // HSB (0, 1.0, 1.0) · (0, 0.875, 0.967) · (0, 0.75, 0.934) · (0, 0.625, 0.901) · (0, 0.5, 0.868) <-- [0.5 relative luminance ~= 0.74 RGB grey or HSB.B]
        new(Harmony.Monochromatic, new[] { "#804040", "#BF3030", "#FF0000", "#FF4040", "#FF7F7F" }),        // HSB (0, 0.5, 0.5) · (0, 0.75, 0.75) · (0, 1.0, 1.0) · (0, 0.75, 1.0) · (0, 0.5, 1.0)
        new(Harmony.Analogous, new[] { "#FF0000", "#FF8000", "#FF0080" }),
        new(Harmony.Complementary, new[] { "#FF0000", "#00FFFF" }),
        new(Harmony.SplitComplementary, new[] { "#FF0000", "#0080FF", "#00FF80" }),
        new(Harmony.Triadic, new[] { "#FF0000", "#0000FF", "#00FF00" }),
        new(Harmony.TetradicRectangle, new[] { "#FF0000", "#00FFFF", "#0000FF", "#FFFF00" }),
        new(Harmony.TetradicSquare, new[] { "#FF0000", "#00FFFF", "#8000FF", "#80FF00" })
    ];
    
    [TestCaseSource(nameof(RedHsbData))]
    public void RedHsb(Harmony harmony, string[] expected)
    {
        var colourWheel = ColourWheel.From(ColourSpace.Hsb, StandardRgb.Red);
        var hues = colourWheel.Harmony(0, harmony).Select(x => x.Hex).OrderBy(x => x).ToArray();
        expected = expected.OrderBy(x => x).ToArray();
        Assert.That(hues, Is.EqualTo(expected));
    }

    private static readonly TestCaseData[] RedOklchData =
    [
        new(Harmony.MonochromaticTint, new[] { 29.234, 29.234, 29.234, 29.234, 29.234 }),
        new(Harmony.MonochromaticShade, new[] { 29.234, 29.234, 29.234, 29.234, 29.234 }),
        new(Harmony.MonochromaticTone, new[] { 29.234, 29.234, 29.234, 29.234, 29.234 }),
        new(Harmony.Monochromatic, new[] { 29.234, 29.234, 29.234, 29.234, 29.234 }),
        new(Harmony.Analogous, new[] { -0.76612, 29.234, 59.234 }),
        new(Harmony.Complementary, new[] { 29.234, 209.23 }),
        new(Harmony.SplitComplementary, new[] { 239.23, 29.234, -180.77 }),
        new(Harmony.Triadic, new[] { 29.234, 149.23, 269.23 }),
        new(Harmony.TetradicRectangle, new[] { 29.234, 59.234 + 30, 209.23, 239.23 + 30 }),
        new(Harmony.TetradicSquare, new[] { 29.234, 119.23, 209.23, 299.23 })
    ];
    
    [TestCaseSource(nameof(RedOklchData))]
    public void RedOklch(Harmony harmony, double[] expected)
    {
        var reference = StandardRgb.Red;
        var colourWheel = ColourWheel.From(ColourSpace.Oklch, reference);
        var hues = colourWheel.Harmony(reference.Oklch.H, harmony).Select(x => x.Oklch.H).OrderBy(x => x).ToArray();
        expected = expected.Select(x => x.WithHueModulo()).OrderBy(x => x).ToArray();
        Assert.That(hues, Is.EqualTo(expected).Within(0.005));
    }
}