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
    private const double XyzTolerance = 0.00000001;
    private const double LabTolerance = 0.00000001;
    
    [Test]
    // no point doing this test starting with Wikipedia's HSB / HSL values since they're rounded
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
    
    [Test]
    public void XyzSameAfterDeconversion() => AssertUtils.AssertRandomXyzColours(AssertXyzDeconversion);

    [Test]
    public void LabSameAfterDeconversion() => AssertUtils.AssertRandomLabColours(AssertLabDeconversion);

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
        var deconvertedViaHsb = Conversion.HsbToRgb(Conversion.RgbToHsb(original), Configuration.Default);
        AssertUtils.AssertColourTuple(deconvertedViaHsb.Tuple, original.Tuple, RgbTolerance);
        AssertUtils.AssertColourTuple(deconvertedViaHsb.TupleLinear, original.TupleLinear, RgbTolerance);
        AssertUtils.AssertColourTuple(deconvertedViaHsb.Tuple255, original.Tuple255, RgbTolerance);
        
        var deconvertedViaXyz = Conversion.XyzToRgb(Conversion.RgbToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTuple(deconvertedViaXyz.Tuple, original.Tuple, RgbTolerance);
        AssertUtils.AssertColourTuple(deconvertedViaXyz.TupleLinear, original.TupleLinear, RgbTolerance);
        AssertUtils.AssertColourTuple(deconvertedViaXyz.Tuple255, original.Tuple255, RgbTolerance);
    }

    private static void AssertHsbDeconversion(TestColour namedColour) => AssertHsbDeconversion(namedColour.Hsb!);
    private static void AssertHsbDeconversion(ColourTuple tuple) => AssertHsbDeconversion(new Hsb(tuple.First, tuple.Second, tuple.Third));
    private static void AssertHsbDeconversion(Hsb original)
    {
        var deconvertedViaRgb = Conversion.RgbToHsb(Conversion.HsbToRgb(original, Configuration.Default));
        AssertUtils.AssertColourTuple(deconvertedViaRgb.Tuple, original.Tuple, HsbTolerance, true);
        
        var deconvertedViaHsl = Conversion.HslToHsb(Conversion.HsbToHsl(original));
        AssertUtils.AssertColourTuple(deconvertedViaHsl.Tuple, original.Tuple, HsbTolerance, true);
    }
    
    private static void AssertHslDeconversion(TestColour namedColour) => AssertHslDeconversion(namedColour.Hsl!);
    private static void AssertHslDeconversion(ColourTuple tuple) => AssertHslDeconversion(new Hsl(tuple.First, tuple.Second, tuple.Third));
    private static void AssertHslDeconversion(Hsl original)
    {
        var deconverted = Conversion.RgbToHsl(Conversion.HsbToRgb(Conversion.HslToHsb(original), Configuration.Default));
        AssertUtils.AssertColourTuple(deconverted.Tuple, original.Tuple, HslTolerance, true);
    }
    
    private static void AssertXyzDeconversion(ColourTuple tuple) => AssertXyzDeconversion(new Xyz(tuple.First, tuple.Second, tuple.Third));
    private static void AssertXyzDeconversion(Xyz original)
    {
        // note: cannot test deconversion via RGB space as XYZ <-> RGB is not 1:1
        var deconverted = Conversion.LabToXyz(Conversion.XyzToLab(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTuple(deconverted.Tuple, original.Tuple, XyzTolerance);
    }
    
    private static void AssertLabDeconversion(ColourTuple tuple) => AssertLabDeconversion(new Lab(tuple.First, tuple.Second, tuple.Third));
    private static void AssertLabDeconversion(Lab original)
    {
        // note: cannot test deconversion via RGB space as XYZ <-> RGB is not 1:1
        var deconverted = Conversion.XyzToLab(Conversion.LabToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTuple(deconverted.Tuple, original.Tuple, LabTolerance);
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