namespace Wacton.Unicolour.Tests;

using System;
using System.Drawing;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ConversionTests
{
    private const double RgbTolerance = 0.00000000001;
    private const double HsbTolerance = 0.00000001;
    private const double HslTolerance = 0.00000001;
    
    [Test]
    public void NamedColoursMatchRgbConversion() => AssertUtils.AssertNamedColours(AssertRgbConversion);

    [Test]
    public void RgbSameAfterDeconversion()
    {
        AssertUtils.AssertNamedColours(AssertRgbDeconversion);
        AssertUtils.AssertRandomRgbColours(AssertRgbDeconversion);
        AssertUtils.AssertRandomRgb255Colours(AssertRgb255Deconversion);
    }

    [Test]
    public void HsbSameAfterDeconversion()
    {
        AssertUtils.AssertNamedColours(AssertHsbDeconversion);
        AssertUtils.AssertRandomHsbColours(AssertHsbDeconversion);
    }
    
    [Test]
    public void HslSameAfterDeconversion()
    {
        AssertUtils.AssertNamedColours(AssertHslDeconversion);
        AssertUtils.AssertRandomHslColours(AssertHslDeconversion);
    }
    
    private static void AssertRgbConversion(TestColour namedColour)
    {
        var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
        var rgb = new Rgb(systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0, Configuration.Default);
        var hsb = Conversion.RgbToHsb(rgb);
        var hsl = Conversion.RgbToHsl(rgb);
        
        var expectedRoundedHsb = namedColour.Hsb;
        var expectedRoundedHsl = namedColour.Hsl;
        
        Assert.That(Math.Round(hsb.H), Is.EqualTo(expectedRoundedHsb!.First), namedColour.Name!);
        Assert.That(Math.Round(hsb.S, 2), Is.EqualTo(expectedRoundedHsb.Second), namedColour.Name!);
        Assert.That(Math.Round(hsb.B, 2), Is.EqualTo(expectedRoundedHsb.Third), namedColour.Name!);

        // within 0.02 because it seems like some of wikipedia's HSL values have questionable rounding...
        Assert.That(Math.Round(hsl.H), Is.EqualTo(expectedRoundedHsl!.First), namedColour.Name!);
        Assert.That(Math.Round(hsl.S, 2), Is.EqualTo(expectedRoundedHsl.Second).Within(0.02), namedColour.Name!);
        Assert.That(Math.Round(hsl.L, 2), Is.EqualTo(expectedRoundedHsl.Third).Within(0.02), namedColour.Name!);
    }

    private static void AssertRgbDeconversion(TestColour namedColour) => AssertRgbDeconversion(GetRgbTupleFromHex(namedColour.Hex!));
    private static void AssertRgb255Deconversion(ColourTuple tuple) => AssertRgbDeconversion(GetNormalisedRgb255Tuple(tuple));
    private static void AssertRgbDeconversion(ColourTuple tuple) => AssertRgbDeconversion(new Rgb(tuple.First, tuple.Second, tuple.Third, Configuration.Default));
    private static void AssertRgbDeconversion(Rgb original)
    {
        var deconverted = Conversion.HsbToRgb(Conversion.RgbToHsb(original), Configuration.Default);
        
        Assert.That(deconverted.R, Is.EqualTo(original.R).Within(RgbTolerance));
        Assert.That(deconverted.G, Is.EqualTo(original.G).Within(RgbTolerance));
        Assert.That(deconverted.B, Is.EqualTo(original.B).Within(RgbTolerance));
        AssertUtils.AssertColourTuple(deconverted.Tuple, original.Tuple, RgbTolerance);
        
        Assert.That(deconverted.RLinear, Is.EqualTo(original.RLinear).Within(RgbTolerance));
        Assert.That(deconverted.GLinear, Is.EqualTo(original.GLinear).Within(RgbTolerance));
        Assert.That(deconverted.BLinear, Is.EqualTo(original.BLinear).Within(RgbTolerance));
        AssertUtils.AssertColourTuple(deconverted.TupleLinear, original.TupleLinear, RgbTolerance);

        Assert.That(deconverted.R255, Is.EqualTo(original.R255));
        Assert.That(deconverted.G255, Is.EqualTo(original.G255));
        Assert.That(deconverted.B255, Is.EqualTo(original.B255));
        AssertUtils.AssertColourTuple(deconverted.Tuple255, original.Tuple255, RgbTolerance);
    }

    private static void AssertHsbDeconversion(TestColour namedColour) => AssertHsbDeconversion(namedColour.Hsb!);
    private static void AssertHsbDeconversion(ColourTuple tuple) => AssertHsbDeconversion(new Hsb(tuple.First, tuple.Second, tuple.Third));
    private static void AssertHsbDeconversion(Hsb original)
    {
        var deconverted = Conversion.RgbToHsb(Conversion.HsbToRgb(original, Configuration.Default));
        Assert.That(deconverted.H, Is.EqualTo(original.H).Within(HsbTolerance));
        Assert.That(deconverted.S, Is.EqualTo(original.S).Within(HsbTolerance));
        Assert.That(deconverted.B, Is.EqualTo(original.B).Within(HsbTolerance));
        AssertUtils.AssertColourTuple(deconverted.Tuple, original.Tuple, HsbTolerance, true);
    }
    
    private static void AssertHslDeconversion(TestColour namedColour) => AssertHslDeconversion(namedColour.Hsl!);
    private static void AssertHslDeconversion(ColourTuple tuple) => AssertHslDeconversion(new Hsl(tuple.First, tuple.Second, tuple.Third));
    private static void AssertHslDeconversion(Hsl original)
    {
        var deconverted = Conversion.RgbToHsl(Conversion.HsbToRgb(Conversion.HslToHsb(original), Configuration.Default));
        Assert.That(deconverted.H, Is.EqualTo(original.H).Within(HslTolerance));
        Assert.That(deconverted.S, Is.EqualTo(original.S).Within(HslTolerance));
        Assert.That(deconverted.L, Is.EqualTo(original.L).Within(HslTolerance));
        AssertUtils.AssertColourTuple(deconverted.Tuple, original.Tuple, HslTolerance, true);
    }
    
    private static ColourTuple GetRgbTupleFromHex(string hex)
    {
        var (r255, g255, b255, _) = Wacton.Unicolour.Utils.ParseColourHex(hex);
        return new(r255 / 255.0, g255 / 255.0, b255 / 255.0);
    }
    
    private static ColourTuple GetNormalisedRgb255Tuple(ColourTuple tuple)
    {
        var (r255, g255, b255) = tuple;
        return new(r255 / 255.0, g255 / 255.0, b255 / 255.0);
    }
}