namespace Wacton.Unicolour.Tests;

using System;
using System.Drawing;
using NUnit.Framework;
using Wacton.Unicolour;
using Wacton.Unicolour.Tests.Lookups;
using Wacton.Unicolour.Tests.Utils;

public class ConversionTests
{
    private const double RgbTolerance = 0.00000000001;
    private const double HsbTolerance = 0.00000001;
    
    [Test]
    public void NamedRgbMatchesNamedHsb() => AssertUtils.AssertNamedColours(AssertRgbToHsbConversion);

    [Test]
    public void RgbSameAfterUnconversion()
    {
        AssertUtils.AssertNamedColours(AssertRgbUnconversion);
        AssertUtils.AssertRandomRgbColours(AssertRgbUnconversion);
        AssertUtils.AssertRandomRgb255Colours(AssertRgbUnconversion);
    }

    [Test]
    public void HsbSameAfterUnconversion()
    {
        AssertUtils.AssertNamedColours(AssertHsbUnconversion);
        AssertUtils.AssertRandomHsbColours(AssertHsbUnconversion);
    }
    
    private static void AssertRgbToHsbConversion(TestColour namedColour)
    {
        var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
        var rgb = new Rgb(systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0, Configuration.Default);
        var hsb = Conversion.RgbToHsb(rgb);
        var expectedRoundedHsb = namedColour.Hsb;
        
        Assert.That(Math.Round(hsb.H), Is.EqualTo(expectedRoundedHsb.Value.h));
        Assert.That(Math.Round(hsb.S, 2), Is.EqualTo(expectedRoundedHsb.Value.s));
        Assert.That(Math.Round(hsb.B, 2), Is.EqualTo(expectedRoundedHsb.Value.b));
    }
    
    private static void AssertRgbUnconversion(TestColour namedColour)
    {
        var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
        var originalRgb = new Rgb(systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0, Configuration.Default);
        AssertRgbUnconversion(originalRgb);
    }
    
    private static void AssertRgbUnconversion(int r, int g, int b)
    {
        var originalRgb = new Rgb(r / 255.0, g / 255.0, b / 255.0, Configuration.Default);
        AssertRgbUnconversion(originalRgb);
    }
    
    private static void AssertRgbUnconversion(double r, double g, double b)
    {
        var originalRgb = new Rgb(r, g, b, Configuration.Default);
        AssertRgbUnconversion(originalRgb);
    }
    
    private static void AssertRgbUnconversion(Rgb originalRgb)
    {
        var convertedRgb = Conversion.HsbToRgb(Conversion.RgbToHsb(originalRgb), Configuration.Default);
        
        Assert.That(convertedRgb.R, Is.EqualTo(originalRgb.R).Within(RgbTolerance));
        Assert.That(convertedRgb.G, Is.EqualTo(originalRgb.G).Within(RgbTolerance));
        Assert.That(convertedRgb.B, Is.EqualTo(originalRgb.B).Within(RgbTolerance));
        Assert.That(convertedRgb.Tuple, Is.EqualTo(originalRgb.Tuple).Within(RgbTolerance));
        
        Assert.That(convertedRgb.RLinear, Is.EqualTo(originalRgb.RLinear).Within(RgbTolerance));
        Assert.That(convertedRgb.GLinear, Is.EqualTo(originalRgb.GLinear).Within(RgbTolerance));
        Assert.That(convertedRgb.BLinear, Is.EqualTo(originalRgb.BLinear).Within(RgbTolerance));
        Assert.That(convertedRgb.TupleLinear, Is.EqualTo(originalRgb.TupleLinear).Within(RgbTolerance));

        Assert.That(convertedRgb.R255, Is.EqualTo(originalRgb.R255));
        Assert.That(convertedRgb.G255, Is.EqualTo(originalRgb.G255));
        Assert.That(convertedRgb.B255, Is.EqualTo(originalRgb.B255));
        Assert.That(convertedRgb.Tuple255, Is.EqualTo(originalRgb.Tuple255));
    }
    
    private static void AssertHsbUnconversion(TestColour namedColour)
    {
        var (h, s, b) = namedColour.Hsb.Value;
        var originalHsb = new Hsb(h, s, b);
        AssertHsbUnconversion(originalHsb);
    }
    
    private static void AssertHsbUnconversion(double h, double s, double b)
    {
        var originalHsb = new Hsb(h, s, b);
        AssertHsbUnconversion(originalHsb);
    }
    
    private static void AssertHsbUnconversion(Hsb originalHsb)
    {
        var convertedHsb = Conversion.RgbToHsb(Conversion.HsbToRgb(originalHsb, Configuration.Default));
        Assert.That(convertedHsb.H, Is.EqualTo(originalHsb.H).Within(HsbTolerance));
        Assert.That(convertedHsb.S, Is.EqualTo(originalHsb.S).Within(HsbTolerance));
        Assert.That(convertedHsb.B, Is.EqualTo(originalHsb.B).Within(HsbTolerance));
        Assert.That(convertedHsb.Tuple, Is.EqualTo(originalHsb.Tuple).Within(HsbTolerance));
    }
}