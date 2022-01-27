namespace Wacton.Unicolour.Tests;

using System;
using System.Drawing;
using NUnit.Framework;
using Wacton.Unicolour;
using Wacton.Unicolour.Tests.Lookups;

public class ExpectedValueTests
{
    [Test]
    public void RgbSameAfterConvertedViaHsb()
    {
        foreach (var name in NamedColours.Names)
        {
            var hex = NamedColours.Hexs[name];
            var systemColour = ColorTranslator.FromHtml(hex);
            
            var originalRgb = new Rgb(systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0);
            var hsb = Converter.RgbToHsb(originalRgb);
            var convertedRgb = Converter.HsbToRgb(hsb);
            
            Assert.That(convertedRgb.R, Is.EqualTo(originalRgb.R).Within(0.00000000001));
            Assert.That(convertedRgb.G, Is.EqualTo(originalRgb.G).Within(0.00000000001));
            Assert.That(convertedRgb.B, Is.EqualTo(originalRgb.B).Within(0.00000000001));
            
            Assert.That(convertedRgb.R255, Is.EqualTo(originalRgb.R255));
            Assert.That(convertedRgb.G255, Is.EqualTo(originalRgb.G255));
            Assert.That(convertedRgb.B255, Is.EqualTo(originalRgb.B255));
        }
    }
    
    [Test]
    public void HsbSameAfterConvertedViaRgb()
    {
        foreach (var name in NamedColours.Names)
        {
            var (h, s, b) = NamedColours.HSBs[name];
            var originalHsb = new Hsb(h, s, b);
            var rgb = Converter.HsbToRgb(originalHsb);
            var convertedHsb = Converter.RgbToHsb(rgb);

            Assert.That(convertedHsb.H, Is.EqualTo(originalHsb.H).Within(0.00000000001));
            Assert.That(convertedHsb.S, Is.EqualTo(originalHsb.S).Within(0.00000000001));
            Assert.That(convertedHsb.B, Is.EqualTo(originalHsb.B).Within(0.00000000001));
        }
    }
    
    [Test]
    public void FromRgbProducesExpectedHsb()
    {
        foreach (var name in NamedColours.Names)
        {
            var hex = NamedColours.Hexs[name];
            var systemColour = ColorTranslator.FromHtml(hex);
            var unicolourFromRgb = Unicolour.FromRgb(systemColour.R, systemColour.G, systemColour.B);
            var hsbFromRgb = unicolourFromRgb.Hsb;
            
            var roundedHsb = NamedColours.HSBs[name];
            Assert.That(Math.Round(hsbFromRgb.H), Is.EqualTo(roundedHsb.h));
            Assert.That(Math.Round(hsbFromRgb.S, 2), Is.EqualTo(roundedHsb.s));
            Assert.That(Math.Round(hsbFromRgb.B, 2), Is.EqualTo(roundedHsb.b));
        }
    }
}