namespace Wacton.Unicolour.Tests;

using System;
using System.Drawing;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ConversionTests
{
    private const double DefaultTolerance = 0.00000000001;
    private const double RbgTolerance = 0.00000005;
    private const double HsbTolerance = 0.000000001;
    private const double HslTolerance = 0.0000000001;
    private const double XyzTolerance = 0.0000000005;
    private const double LuvTolerance = 0.00000001;
    private const double JzazbzTolerance = 0.00000005;
    private const double OklabTolerance = 0.000005;

    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.NamedColours))] // no point doing this test starting with Wikipedia's HSB / HSL values since they're rounded
    public void NamedColoursMatchRgbConversion(TestColour namedColour) => AssertRgbConversion(namedColour);

    [TestCaseSource(typeof(TestColours), nameof(TestColours.NamedColours))]
    public void RgbNamedSameAfterRoundTripConversion(TestColour namedColour) => AssertRgbRoundTrip(namedColour);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomRgb255Colours))]
    public void Rgb255SameAfterRoundTripConversion(ColourTriplet triplet) => AssertRgb255RoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomRgbColours))]
    public void RgbSameAfterRoundTripConversion(ColourTriplet triplet) => AssertRgbRoundTrip(triplet);

    [TestCaseSource(typeof(TestColours), nameof(TestColours.NamedColours))]
    public void HsbNamedSameAfterRoundTripConversion(TestColour namedColour) => AssertHsbRoundTrip(namedColour);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomHsbColours))]
    public void HsbSameAfterRoundTripConversion(ColourTriplet triplet) => AssertHsbRoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.NamedColours))]
    public void HslNamedSameAfterRoundTripConversion(TestColour namedColour) => AssertHslRoundTrip(namedColour);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomHslColours))]
    public void HslSameAfterRoundTripConversion(ColourTriplet triplet) => AssertHslRoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomXyzColours))]
    public void XyzSameAfterRoundTripConversion(ColourTriplet triplet) => AssertXyzRoundTrip(triplet);

    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomLabColours))]
    public void LabSameAfterRoundTripConversion(ColourTriplet triplet) => AssertLabRoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomLchabColours))]
    public void LchabSameAfterRoundTripConversion(ColourTriplet triplet) => AssertLchabRoundTrip(triplet);

    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomLuvColours))]
    public void LuvSameAfterRoundTripConversion(ColourTriplet triplet) => AssertLuvRoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomLchuvColours))]
    public void LchuvSameAfterRoundTripConversion(ColourTriplet triplet) => AssertLchuvRoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomJzazbzColours))]
    public void JzazbzSameAfterRoundTripConversion(ColourTriplet triplet) => AssertJzazbzRoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomJzczhzColours))]
    public void JzczhzSameAfterRoundTripConversion(ColourTriplet triplet) => AssertJzczhzRoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomOklabColours))]
    public void OklabSameAfterRoundTripConversion(ColourTriplet triplet) => AssertOklabRoundTrip(triplet);
    
    [TestCaseSource(typeof(TestColours), nameof(TestColours.RandomOklchColours))]
    public void OklchSameAfterRoundTripConversion(ColourTriplet triplet) => AssertOklchRoundTrip(triplet);
    
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

    private static void AssertRgbRoundTrip(TestColour namedColour) => AssertRgbRoundTrip(GetRgbTripletFromHex(namedColour.Hex!));
    private static void AssertRgb255RoundTrip(ColourTriplet triplet) => AssertRgbRoundTrip(GetNormalisedRgb255Triplet(triplet));
    private static void AssertRgbRoundTrip(ColourTriplet triplet) => AssertRgbRoundTrip(new Rgb(triplet.First, triplet.Second, triplet.Third, Configuration.Default));
    private static void AssertRgbRoundTrip(Rgb original)
    {
        var viaHsb = Conversion.HsbToRgb(Conversion.RgbToHsb(original), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaHsb.Triplet, original.Triplet, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaHsb.ConstrainedTriplet, original.ConstrainedTriplet, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaHsb.TripletLinear, original.TripletLinear, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaHsb.ConstrainedTripletLinear, original.ConstrainedTripletLinear, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaHsb.Triplet255, original.Triplet255, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaHsb.ConstrainedTriplet255, original.ConstrainedTriplet255, RbgTolerance);
        
        var viaXyz = Conversion.XyzToRgb(Conversion.RgbToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaXyz.Triplet, original.Triplet, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaXyz.ConstrainedTriplet, original.ConstrainedTriplet, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaXyz.TripletLinear, original.TripletLinear, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaXyz.ConstrainedTripletLinear, original.ConstrainedTripletLinear, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaXyz.Triplet255, original.Triplet255, RbgTolerance);
        AssertUtils.AssertColourTriplet(viaXyz.ConstrainedTriplet255, original.ConstrainedTriplet255, RbgTolerance);
    }

    private static void AssertHsbRoundTrip(TestColour namedColour) => AssertHsbRoundTrip(namedColour.Hsb!);
    private static void AssertHsbRoundTrip(ColourTriplet triplet) => AssertHsbRoundTrip(new Hsb(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHsbRoundTrip(Hsb original)
    {
        var viaRgb = Conversion.RgbToHsb(Conversion.HsbToRgb(original, Configuration.Default));
        AssertUtils.AssertColourTriplet(viaRgb.Triplet, original.Triplet, HsbTolerance, 0);
        
        var viaHsl = Conversion.HslToHsb(Conversion.HsbToHsl(original));
        AssertUtils.AssertColourTriplet(viaHsl.Triplet, original.Triplet, HsbTolerance, 0);
    }
    
    private static void AssertHslRoundTrip(TestColour namedColour) => AssertHslRoundTrip(namedColour.Hsl!);
    private static void AssertHslRoundTrip(ColourTriplet triplet) => AssertHslRoundTrip(new Hsl(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHslRoundTrip(Hsl original)
    {
        var viaHsb = Conversion.HsbToHsl(Conversion.HslToHsb(original));
        AssertUtils.AssertColourTriplet(viaHsb.Triplet, original.Triplet, HslTolerance, 0);
    }
    
    private static void AssertXyzRoundTrip(ColourTriplet triplet) => AssertXyzRoundTrip(new Xyz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertXyzRoundTrip(Xyz original)
    {
        var viaRgb = Conversion.RgbToXyz(Conversion.XyzToRgb(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaRgb.Triplet, original.Triplet, XyzTolerance);

        var viaLab = Conversion.LabToXyz(Conversion.XyzToLab(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaLab.Triplet, original.Triplet, XyzTolerance);
        
        var viaLuv = Conversion.LuvToXyz(Conversion.XyzToLuv(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaLuv.Triplet, original.Triplet, XyzTolerance);
        
        var viaJzazbz = Conversion.JzazbzToXyz(Conversion.XyzToJzazbz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaJzazbz.Triplet, viaJzazbz.Triplet, XyzTolerance);
        
        var viaOklab = Conversion.OklabToXyz(Conversion.XyzToOklab(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaOklab.Triplet, original.Triplet, XyzTolerance);
    }
    
    private static void AssertLabRoundTrip(ColourTriplet triplet) => AssertLabRoundTrip(new Lab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLabRoundTrip(Lab original)
    {
        var viaXyz = Conversion.XyzToLab(Conversion.LabToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaXyz.Triplet, original.Triplet, DefaultTolerance);
        
        var viaLchab = Conversion.LchabToLab(Conversion.LabToLchab(original));
        AssertUtils.AssertColourTriplet(viaLchab.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertLchabRoundTrip(ColourTriplet triplet) => AssertLchabRoundTrip(new Lchab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLchabRoundTrip(Lchab original)
    {
        var viaLab = Conversion.LabToLchab(Conversion.LchabToLab(original));
        AssertUtils.AssertColourTriplet(viaLab.Triplet, original.Triplet, DefaultTolerance, 2);
    }
    
    private static void AssertLuvRoundTrip(ColourTriplet triplet) => AssertLuvRoundTrip(new Luv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLuvRoundTrip(Luv original)
    {
        var viaXyz = Conversion.XyzToLuv(Conversion.LuvToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaXyz.Triplet, original.Triplet, LuvTolerance);
        
        var viaLchuv = Conversion.LchuvToLuv(Conversion.LuvToLchuv(original));
        AssertUtils.AssertColourTriplet(viaLchuv.Triplet, original.Triplet, LuvTolerance);
    }
    
    private static void AssertLchuvRoundTrip(ColourTriplet triplet) => AssertLchuvRoundTrip(new Lchuv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLchuvRoundTrip(Lchuv original)
    {
        var viaLuv = Conversion.LuvToLchuv(Conversion.LchuvToLuv(original));
        AssertUtils.AssertColourTriplet(viaLuv.Triplet, original.Triplet, DefaultTolerance, 2);
    }
    
    private static void AssertJzazbzRoundTrip(ColourTriplet triplet) => AssertJzazbzRoundTrip(new Jzazbz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertJzazbzRoundTrip(Jzazbz original)
    {
        // note: cannot test round trip of XYZ space as Jzazbz <-> XYZ is not 1:1, e.g.
        // - when Jzazbz inputs produces negative XYZ values, which are clamped during XYZ -> Jzazbz
        // - when Jzazbz negative inputs trigger a negative number to a fractional power, resulting in XYZ containing NaNs
        // var viaXyz = Conversion.XyzToJzazbz(Conversion.JzazbzToXyz(original, Configuration.Default), Configuration.Default);
        
        var viaJzczhz = Conversion.JzczhzToJzazbz(Conversion.JzazbzToJzczhz(original));
        AssertUtils.AssertColourTriplet(viaJzczhz.Triplet, original.Triplet, JzazbzTolerance);
    }
    
    private static void AssertJzczhzRoundTrip(ColourTriplet triplet) => AssertJzczhzRoundTrip(new Jzczhz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertJzczhzRoundTrip(Jzczhz original)
    {
        var viaJzazbz = Conversion.JzazbzToJzczhz(Conversion.JzczhzToJzazbz(original));
        AssertUtils.AssertColourTriplet(viaJzazbz.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertOklabRoundTrip(ColourTriplet triplet) => AssertOklabRoundTrip(new Oklab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertOklabRoundTrip(Oklab original)
    {
        var viaXyz = Conversion.XyzToOklab(Conversion.OklabToXyz(original, Configuration.Default), Configuration.Default);
        AssertUtils.AssertColourTriplet(viaXyz.Triplet, original.Triplet, OklabTolerance);
        
        var viaOklch = Conversion.OklchToOklab(Conversion.OklabToOklch(original));
        AssertUtils.AssertColourTriplet(viaOklch.Triplet, original.Triplet, OklabTolerance);
    }
    
    private static void AssertOklchRoundTrip(ColourTriplet triplet) => AssertOklchRoundTrip(new Oklch(triplet.First, triplet.Second, triplet.Third));
    private static void AssertOklchRoundTrip(Oklch original)
    {
        var viaOklab = Conversion.OklabToOklch(Conversion.OklchToOklab(original));
        AssertUtils.AssertColourTriplet(viaOklab.Triplet, original.Triplet, DefaultTolerance, 2);
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