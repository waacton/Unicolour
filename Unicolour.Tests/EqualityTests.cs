namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour;

public class EqualityTests
{
    private static readonly Random Random = new();
    
    [Test]
    public void EqualRgbGivesEqualObjects()
    {
        var unicolour1 = GetRandomRgbUnicolour();
        var unicolour2 = Unicolour.FromRgb(unicolour1.Rgb.R, unicolour1.Rgb.G, unicolour1.Rgb.B, unicolour1.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualHsbGivesEqualObjects()
    {
        var unicolour1 = GetRandomHsbUnicolour();
        var unicolour2 = Unicolour.FromHsb(unicolour1.Hsb.H, unicolour1.Hsb.S, unicolour1.Hsb.B, unicolour1.A);
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
            (unicolour1.A + 0.1).Modulo(1));
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
            (unicolour1.A + 0.1).Modulo(1));
        AssertUnicoloursNotEqual(unicolour1, unicolour2);
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
    
    private static void AssertUnicoloursEqual(Unicolour unicolour1, Unicolour unicolour2)
    {
        AssertEqual(unicolour1.Rgb, unicolour2.Rgb);
        AssertEqual(unicolour1.Hsb, unicolour2.Hsb);
        AssertEqual(unicolour1.Xyz, unicolour2.Xyz);
        AssertEqual(unicolour1.Lab, unicolour2.Lab);
        AssertEqual(unicolour1.A, unicolour2.A);
        AssertEqual(unicolour1.Luminance, unicolour2.Luminance);
        AssertEqual(unicolour1, unicolour2);
    }

    private static void AssertUnicoloursNotEqual(Unicolour unicolour1, Unicolour unicolour2)
    {
        AssertNotEqual(unicolour1.Rgb, unicolour2.Rgb);
        AssertNotEqual(unicolour1.Hsb, unicolour2.Hsb);
        AssertNotEqual(unicolour1.Xyz, unicolour2.Xyz);
        AssertNotEqual(unicolour1.Lab, unicolour2.Lab);
        AssertNotEqual(unicolour1.A, unicolour2.A);
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
        Assert.That(object1.Equals(object2), Is.True);
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