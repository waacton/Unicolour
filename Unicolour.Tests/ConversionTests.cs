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
    private const double LuvTolerance = 0.00000001;
    private const double OklabTolerance = 0.000001;
    
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
    
    [Test]
    public void LuvSameAfterDeconversion() => AssertUtils.AssertRandomLuvColours(AssertLuvDeconversion);
    
    [Test]
    public void OklabSameAfterDeconversion() => AssertUtils.AssertRandomOklabColours(AssertOklabDeconversion);

    private static void AssertRgbConversion(TestColour namedColour)
    {
        var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
        var rgb = new Rgb(systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0, Configuration.Default);
        var hsb = Conversion.RgbToHsb(rgb);
        var hsl = Conversion.HsbToHsl(hsb);
        
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

    private static void AssertRgbDeconversion(TestColour namedColour) => AssertRgbDeconversion(GetRgbTripletFromHex(namedColour.Hex!));
    private static void AssertRgb255Deconversion(ColourTriplet triplet) => AssertRgbDeconversion(GetNormalisedRgb255Triplet(triplet));
    private static void AssertRgbDeconversion(ColourTriplet triplet) => AssertRgbDeconversion(new Rgb(triplet.First, triplet.Second, triplet.Third, Configuration.Default));
    private static void AssertRgbDeconversion(Rgb original)
    {
        var deconvertedViaHsb = Conversion.HsbToRgb(Conversion.RgbToHsb(original), Configuration.Default);
        AssertUtils.AssertColourTriplet(deconvertedViaHsb.Triplet, original.Triplet, RgbTolerance);
        AssertUtils.AssertColourTriplet(deconvertedViaHsb.TripletLinear, original.TripletLinear, RgbTolerance);
        AssertUtils.AssertColourTriplet(deconvertedViaHsb.Triplet255, original.Triplet255, RgbTolerance);
        
        var deconvertedViaXyz = Conversion.XyzToRgb(Conversion.RgbToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(deconvertedViaXyz.Triplet, original.Triplet, RgbTolerance);
        AssertUtils.AssertColourTriplet(deconvertedViaXyz.TripletLinear, original.TripletLinear, RgbTolerance);
        AssertUtils.AssertColourTriplet(deconvertedViaXyz.Triplet255, original.Triplet255, RgbTolerance);
    }

    private static void AssertHsbDeconversion(TestColour namedColour) => AssertHsbDeconversion(namedColour.Hsb!);
    private static void AssertHsbDeconversion(ColourTriplet triplet) => AssertHsbDeconversion(new Hsb(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHsbDeconversion(Hsb original)
    {
        var deconvertedViaRgb = Conversion.RgbToHsb(Conversion.HsbToRgb(original, Configuration.Default));
        AssertUtils.AssertColourTriplet(deconvertedViaRgb.Triplet, original.Triplet, HsbTolerance, true);
        
        var deconvertedViaHsl = Conversion.HslToHsb(Conversion.HsbToHsl(original));
        AssertUtils.AssertColourTriplet(deconvertedViaHsl.Triplet, original.Triplet, HsbTolerance, true);
    }
    
    private static void AssertHslDeconversion(TestColour namedColour) => AssertHslDeconversion(namedColour.Hsl!);
    private static void AssertHslDeconversion(ColourTriplet triplet) => AssertHslDeconversion(new Hsl(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHslDeconversion(Hsl original)
    {
        var deconverted = Conversion.HsbToHsl(Conversion.HslToHsb(original));
        AssertUtils.AssertColourTriplet(deconverted.Triplet, original.Triplet, HslTolerance, true);
    }
    
    private static void AssertXyzDeconversion(ColourTriplet triplet) => AssertXyzDeconversion(new Xyz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertXyzDeconversion(Xyz original)
    {
        // note: cannot test deconversion via RGB space as XYZ <-> RGB is not 1:1
        var deconvertedViaLab = Conversion.LabToXyz(Conversion.XyzToLab(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(deconvertedViaLab.Triplet, original.Triplet, XyzTolerance);
        
        var deconvertedViaLuv = Conversion.LuvToXyz(Conversion.XyzToLuv(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(deconvertedViaLuv.Triplet, original.Triplet, XyzTolerance);
        
        var deconvertedViaOklab = Conversion.OklabToXyz(Conversion.XyzToOklab(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(deconvertedViaOklab.Triplet, original.Triplet, XyzTolerance);
    }
    
    private static void AssertLabDeconversion(ColourTriplet triplet) => AssertLabDeconversion(new Lab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLabDeconversion(Lab original)
    {
        // note: cannot test deconversion via RGB space as XYZ <-> RGB is not 1:1
        var deconverted = Conversion.XyzToLab(Conversion.LabToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(deconverted.Triplet, original.Triplet, LabTolerance);
    }
    
    private static void AssertLuvDeconversion(ColourTriplet triplet) => AssertLuvDeconversion(new Luv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLuvDeconversion(Luv original)
    {
        // note: cannot test deconversion via RGB space as XYZ <-> RGB is not 1:1
        var deconverted = Conversion.XyzToLuv(Conversion.LuvToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(deconverted.Triplet, original.Triplet, LuvTolerance);
    }
    
    private static void AssertOklabDeconversion(ColourTriplet triplet) => AssertOklabDeconversion(new Oklab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertOklabDeconversion(Oklab original)
    {
        var deconverted = Conversion.XyzToOklab(Conversion.OklabToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(deconverted.Triplet, original.Triplet, OklabTolerance);
    }

    private static ColourTriplet GetRgbTripletFromHex(string hex)
    {
        var (r255, g255, b255, _) = Wacton.Unicolour.Utils.ParseColourHex(hex);
        return new(r255 / 255.0, g255 / 255.0, b255 / 255.0);
    }
    
    private static ColourTriplet GetNormalisedRgb255Triplet(ColourTriplet triplet)
    {
        var (r255, g255, b255) = triplet;
        return new(r255 / 255.0, g255 / 255.0, b255 / 255.0);
    }
}