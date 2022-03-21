namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class EqualityTests
{
    private static readonly Random Random = new();
    
    [Test]
    public void EqualRgbGivesEqualObjects()
    {
        var unicolour1 = GetRandomRgbUnicolour();
        var unicolour2 = Unicolour.FromRgb(unicolour1.Rgb.R, unicolour1.Rgb.G, unicolour1.Rgb.B, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualHsbGivesEqualObjects()
    {
        var unicolour1 = GetRandomHsbUnicolour();
        var unicolour2 = Unicolour.FromHsb(unicolour1.Hsb.H, unicolour1.Hsb.S, unicolour1.Hsb.B, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualHslGivesEqualObjects()
    {
        var unicolour1 = GetRandomHslUnicolour();
        var unicolour2 = Unicolour.FromHsl(unicolour1.Hsl.H, unicolour1.Hsl.S, unicolour1.Hsl.L, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void NotEqualRgbGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomRgbUnicolour();
        var unicolour2 = Unicolour.FromRgb(
            (unicolour1.Rgb.R + 0.1).Modulo(1),
            (unicolour1.Rgb.G + 0.1).Modulo(1),
            (unicolour1.Rgb.B + 0.1).Modulo(1),
            (unicolour1.Alpha.A + 0.1).Modulo(1));
        AssertUnicoloursNotEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void NotEqualHsbGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomHsbUnicolour();
        var unicolour2 = Unicolour.FromHsb(
            (unicolour1.Hsb.H + 0.1).Modulo(360),
            (unicolour1.Hsb.S + 0.1).Modulo(1),
            (unicolour1.Hsb.B + 0.1).Modulo(1),
            (unicolour1.Alpha.A + 0.1).Modulo(1));
        AssertUnicoloursNotEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void NotEqualHslGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomHslUnicolour();
        var unicolour2 = Unicolour.FromHsl(
            (unicolour1.Hsl.H + 0.1).Modulo(360),
            (unicolour1.Hsl.S + 0.1).Modulo(1),
            (unicolour1.Hsl.L + 0.1).Modulo(1),
            (unicolour1.Alpha.A + 0.1).Modulo(1));
        AssertUnicoloursNotEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void DifferentConfigurationObjects()
    {
        var config1 = new Configuration(
            Chromaticity.StandardRgbR,
            new Chromaticity(0.25, 0.75),
            new Chromaticity(0.5, 0.5),
            Companding.InverseStandardRgb, 
            new WhitePoint(0.9, 1.0, 1.1), 
            new WhitePoint(0.95, 1.0, 1.05));

        var config2 = new Configuration(
            Chromaticity.StandardRgbR,
            new Chromaticity(0.75, 0.25),
            new Chromaticity(0.5, 0.5),
            Companding.InverseStandardRgb, 
            new WhitePoint(0.9, 1.0, 1.1), 
            new WhitePoint(0.95001, 1.0001, 1.05001));
        
        AssertEqual(config1.ChromaticityR, config2.ChromaticityR);
        AssertNotEqual(config1.ChromaticityG, config2.ChromaticityG);
        AssertEqual(config1.ChromaticityB, config2.ChromaticityB);
        AssertEqual(config1.RgbWhitePoint, config2.RgbWhitePoint);
        AssertNotEqual(config1.XyzWhitePoint, config2.XyzWhitePoint);
        AssertEqual(config1.InverseCompanding, config2.InverseCompanding);
        AssertNotEqual(config1, config2);
    }

    private static Unicolour GetRandomRgbUnicolour()
    {
        var (r, g, b, a) = (Random.NextDouble(), Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
        return Unicolour.FromRgb(r, g, b, a);
    }
    
    private static Unicolour GetRandomHsbUnicolour()
    {
        var (h, s, b, a) = (Random.NextDouble(), Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
        return Unicolour.FromHsb(h, s, b, a);
    }
    
    private static Unicolour GetRandomHslUnicolour()
    {
        var (h, s, l, a) = (Random.NextDouble(), Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
        return Unicolour.FromHsl(h, s, l, a);
    }
    
    private static void AssertUnicoloursEqual(Unicolour unicolour1, Unicolour unicolour2)
    {
        AssertEqual(unicolour1.Rgb, unicolour2.Rgb);
        AssertEqual(unicolour1.Hsb, unicolour2.Hsb);
        AssertEqual(unicolour1.Xyz, unicolour2.Xyz);
        AssertEqual(unicolour1.Lab, unicolour2.Lab);
        AssertEqual(unicolour1.Alpha, unicolour2.Alpha);
        AssertEqual(unicolour1.Luminance, unicolour2.Luminance);
        AssertEqual(unicolour1, unicolour2);
    }

    private static void AssertUnicoloursNotEqual(Unicolour unicolour1, Unicolour unicolour2)
    {
        AssertNotEqual(unicolour1.Rgb, unicolour2.Rgb);
        AssertNotEqual(unicolour1.Hsb, unicolour2.Hsb);
        AssertNotEqual(unicolour1.Xyz, unicolour2.Xyz);
        AssertNotEqual(unicolour1.Lab, unicolour2.Lab);
        AssertNotEqual(unicolour1.Alpha, unicolour2.Alpha);
        AssertNotEqual(unicolour1.Luminance, unicolour2.Luminance);
        AssertNotEqual(unicolour1, unicolour2);
    }
    
    private static void AssertEqual<T>(T object1, T object2)
    {
        if (object1 == null || object2 == null)
        {
            Assert.Fail();
            return;
        }
        
        Assert.That(object1, Is.EqualTo(object2));
        Assert.That(object1.Equals(object2));
        Assert.That(object1.GetHashCode(), Is.EqualTo(object2.GetHashCode()));
        Assert.That(object1.ToString(), Is.EqualTo(object2.ToString()));
    }

    private static void AssertNotEqual<T>(T object1, T object2)
    {
        if (object1 == null || object2 == null)
        {
            Assert.Fail();
            return;
        }
        
        Assert.That(object1, Is.Not.EqualTo(object2));
        Assert.That(object1.Equals(object2), Is.False);
        Assert.That(object1.GetHashCode(), Is.Not.EqualTo(object2.GetHashCode()));
        Assert.That(object1.ToString(), Is.Not.EqualTo(object2.ToString()));
    }
}