using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ColourTripletTests
{
    [TestCase(0, 0, 0)]
    [TestCase(0, 0.5, 1)]
    [TestCase(1, 1, 1)]
    [TestCase(double.MinValue, 0.5, 0.5)]
    [TestCase(0.5, double.MinValue, 0.5)]
    [TestCase(0.5, 0.5, double.MinValue)]
    [TestCase(double.MaxValue, 0.5, 0.5)]
    [TestCase(0.5, double.MaxValue, 0.5)]
    [TestCase(0.5, 0.5, double.MaxValue)]
    [TestCase(double.Epsilon, 0.5, 0.5)]
    [TestCase(0.5, double.Epsilon, 0.5)]
    [TestCase(0.5, 0.5, double.Epsilon)]
    [TestCase(double.NegativeInfinity, 0.5, 0.5)]
    [TestCase(0.5, double.NegativeInfinity, 0.5)]
    [TestCase(0.5, 0.5, double.NegativeInfinity)]
    [TestCase(double.PositiveInfinity, 0.5, 0.5)]
    [TestCase(0.5, double.PositiveInfinity, 0.5)]
    [TestCase(0.5, 0.5, double.PositiveInfinity)]
    [TestCase(double.NaN, 0.5, 0.5)]
    [TestCase(0.5, double.NaN, 0.5)]
    [TestCase(0.5, 0.5, double.NaN)]
    public void AsArray(double first, double second, double third)
    {
        var triplet = new ColourTriplet(first, second, third);
        var array = triplet.ToArray();
        Assert.That(array[0], Is.EqualTo(first));
        Assert.That(array[1], Is.EqualTo(second));
        Assert.That(array[2], Is.EqualTo(third));
        Assert.That(array[0], Is.EqualTo(triplet.First));
        Assert.That(array[1], Is.EqualTo(triplet.Second));
        Assert.That(array[2], Is.EqualTo(triplet.Third));
    }
    
    private static readonly TestCaseData[] GetHueTestData =
    [
        new(new ColourTriplet(7.7, 8.8, 9.9, null), null),
        new(new ColourTriplet(7.7, 8.8, 9.9, 0), 7.7),
        new(new ColourTriplet(7.7, 8.8, 9.9, 1), null),
        new(new ColourTriplet(7.7, 8.8, 9.9, 2), 9.9)
    ];
    
    [TestCaseSource(nameof(GetHueTestData))]
    public void GetHue(ColourTriplet triplet, double? expectedHue)
    {
        if (triplet.HueIndex is null or 1)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => triplet.HueValue());
            return;
        }

        var hue = double.NaN;
        Assert.DoesNotThrow(() => hue = triplet.HueValue());
        Assert.That(hue, Is.EqualTo(expectedHue));
    }
    
    private static readonly TestCaseData[] OverrideHueTestData =
    [
        new(new ColourTriplet(7.7, 8.8, 9.9, null), 6.6, null),
        new(new ColourTriplet(7.7, 8.8, 9.9, 0), 6.6, new ColourTriplet(6.6, 8.8, 9.9, 0)),
        new(new ColourTriplet(7.7, 8.8, 9.9, 1), 6.6, null),
        new(new ColourTriplet(7.7, 8.8, 9.9, 2), 6.6, new ColourTriplet(7.7, 8.8, 6.6, 2))
    ];
    
    [TestCaseSource(nameof(OverrideHueTestData))]
    public void OverrideHue(ColourTriplet triplet, double hueOverride, ColourTriplet expectedTriplet)
    {
        if (triplet.HueIndex is null or 1)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => triplet.WithHueOverride(hueOverride));
            return;
        }

        ColourTriplet hueOverrideTriplet = null!;
        Assert.DoesNotThrow(() => hueOverrideTriplet = triplet.WithHueOverride(hueOverride));
        Assert.That(hueOverrideTriplet.HueValue(), Is.EqualTo(hueOverride));
        TestUtils.AssertTriplet(hueOverrideTriplet, expectedTriplet, 0.00000000001);
    }
    
    private static readonly TestCaseData[] DegreeMapTestData =
    [
        new(new ColourTriplet(7.7, 8.8, 9.9, null), new Func<double, double>(x => x - 6.6), new ColourTriplet(7.7, 8.8, 9.9, null)),
        new(new ColourTriplet(7.7, 8.8, 9.9, 0), new Func<double, double>(x => x - 6.6), new ColourTriplet(1.1, 8.8, 9.9, 0)),
        new(new ColourTriplet(7.7, 8.8, 9.9, 1), new Func<double, double>(x => x - 6.6), null),
        new(new ColourTriplet(7.7, 8.8, 9.9, 2), new Func<double, double>(x => x - 6.6), new ColourTriplet(7.7, 8.8, 3.3, 2)),
        new(new ColourTriplet(7.7, 8.8, 9.9, null), new Func<double, double>(x => x * 10), new ColourTriplet(7.7, 8.8, 9.9, null)),
        new(new ColourTriplet(7.7, 8.8, 9.9, 0), new Func<double, double>(x => x * 10), new ColourTriplet(77.0, 8.8, 9.9, 0)),
        new(new ColourTriplet(7.7, 8.8, 9.9, 1), new Func<double, double>(x => x * 10), null),
        new(new ColourTriplet(7.7, 8.8, 9.9, 2), new Func<double, double>(x => x * 10), new ColourTriplet(7.7, 8.8, 99.0, 2))
    ];
    
    [TestCaseSource(nameof(DegreeMapTestData))]
    public void DegreeMap(ColourTriplet triplet, Func<double, double> degreeMap, ColourTriplet expectedTriplet)
    {
        if (triplet.HueIndex is 1)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => triplet.WithDegreeMap(degreeMap));
            return;
        }

        ColourTriplet degreeMapTriplet = null!;
        Assert.DoesNotThrow(() => degreeMapTriplet = triplet.WithDegreeMap(degreeMap));
        TestUtils.AssertTriplet(degreeMapTriplet, expectedTriplet, 0.00000000001);
    }
    
    private static readonly TestCaseData[] ModuloHueTestData =
    [
        new(new ColourTriplet(-270, 450, 810, null), new ColourTriplet(-270, 450, 810, null)),
        new(new ColourTriplet(-270, 450, 810, 0), new ColourTriplet(90, 450, 810, 0)),
        new(new ColourTriplet(-270, 450, 810, 1), null),
        new(new ColourTriplet(-270, 450, 810, 2), new ColourTriplet(-270, 450, 90, 2))
    ];
    
    [TestCaseSource(nameof(ModuloHueTestData))]
    public void ModuloHue(ColourTriplet triplet, ColourTriplet expectedTriplet)
    {
        if (triplet.HueIndex is 1)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => triplet.WithHueModulo());
            return;
        }

        ColourTriplet hueModuloTriplet = null!;
        Assert.DoesNotThrow(() => hueModuloTriplet = triplet.WithHueModulo());
        TestUtils.AssertTriplet(hueModuloTriplet, expectedTriplet, 0.00000000001);
    }
    
    private static readonly TestCaseData[] ModuloHue360TestData =
    [
        new TestCaseData(new ColourTriplet(360, 360, 360, null), false, new ColourTriplet(360, 360, 360, null)).SetName("Hue index null, 360 not allowed"),
        new TestCaseData(new ColourTriplet(360, 360, 360, null), true, new ColourTriplet(360, 360, 360, null)).SetName("Hue index null, 360 allowed"),
        new TestCaseData(new ColourTriplet(360, 360, 360, 0), false, new ColourTriplet(0, 360, 360, 0)).SetName("Hue index 0, 360 not allowed"),
        new TestCaseData(new ColourTriplet(360, 360, 360, 0), true, new ColourTriplet(360, 360, 360, 0)).SetName("Hue index 0, 360 allowed"),
        new TestCaseData(new ColourTriplet(360, 360, 360, 1), false, null).SetName("Hue index 1, 360 not allowed"),
        new TestCaseData(new ColourTriplet(360, 360, 360, 1), true, null).SetName("Hue index 1, 360 allowed"),
        new TestCaseData(new ColourTriplet(360, 360, 360, 2), false, new ColourTriplet(360, 360, 0, 2)).SetName("Hue index 2, 360 not allowed"),
        new TestCaseData(new ColourTriplet(360, 360, 360, 2), true, new ColourTriplet(360, 360, 360, 2)).SetName("Hue index 2, 360 allowed")
    ];
    
    [TestCaseSource(nameof(ModuloHue360TestData))]
    public void ModuloHue360(ColourTriplet triplet, bool allow360, ColourTriplet expectedTriplet)
    {
        if (triplet.HueIndex is 1)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => triplet.WithHueModulo());
            return;
        }

        ColourTriplet hueModuloTriplet = null!;
        Assert.DoesNotThrow(() => hueModuloTriplet = triplet.WithHueModulo(allow360));
        TestUtils.AssertTriplet(hueModuloTriplet, expectedTriplet, 0.00000000001);
    }
    
    private static readonly TestCaseData[] PremultipliedAlphaTestData =
    [
        new(new ColourTriplet(2, 10, -8.8, null), 0.5, new ColourTriplet(1, 5, -4.4, null)),
        new(new ColourTriplet(2, 10, -8.8, 0), 0.5, new ColourTriplet(2, 5, -4.4, 0)),
        new(new ColourTriplet(2, 10, -8.8, 1), 0.5, null),
        new(new ColourTriplet(2, 10, -8.8, 2), 0.5, new ColourTriplet(1, 5, -8.8, 2))
    ];
    
    [TestCaseSource(nameof(PremultipliedAlphaTestData))]
    public void PremultipliedAlpha(ColourTriplet triplet, double alpha, ColourTriplet expectedTriplet)
    {
        if (triplet.HueIndex is 1)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => triplet.WithPremultipliedAlpha(alpha));
            return;
        }

        ColourTriplet premultipliedAlphaTriplet = null!;
        Assert.DoesNotThrow(() => premultipliedAlphaTriplet = triplet.WithPremultipliedAlpha(alpha));
        TestUtils.AssertTriplet(premultipliedAlphaTriplet, expectedTriplet, 0.00000000001);
    }
    
    private static readonly TestCaseData[] UnpremultipliedAlphaTestData =
    [
        new(new ColourTriplet(1, 5, -4.4, null), 0.5, new ColourTriplet(2, 10, -8.8, null)),
        new(new ColourTriplet(1, 5, -4.4, 0), 0.5, new ColourTriplet(1, 10, -8.8, 0)),
        new(new ColourTriplet(1, 5, -4.4, 1), 0.5, null),
        new(new ColourTriplet(1, 5, -4.4, 2), 0.5, new ColourTriplet(2, 10, -4.4, 2))
    ];
    
    private static readonly TestCaseData[] UnpremultipliedZeroAlphaTestData =
    [
        new(new ColourTriplet(1, 5, -4.4, null), 0.0, new ColourTriplet(1, 5, -4.4, null)),
        new(new ColourTriplet(1, 5, -4.4, 0), 0.0, new ColourTriplet(1, 5, -4.4, 0)),
        new(new ColourTriplet(1, 5, -4.4, 1), 0.0, null),
        new(new ColourTriplet(1, 5, -4.4, 2), 0.0, new ColourTriplet(1, 5, -4.4, 2))
    ];
    
    [TestCaseSource(nameof(UnpremultipliedAlphaTestData))]
    [TestCaseSource(nameof(UnpremultipliedZeroAlphaTestData))]
    public void UnpremultipliedAlpha(ColourTriplet triplet, double alpha, ColourTriplet expectedTriplet)
    {
        if (triplet.HueIndex is 1)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => triplet.WithUnpremultipliedAlpha(alpha));
            return;
        }

        ColourTriplet unpremultipliedAlphaTriplet = null!;
        Assert.DoesNotThrow(() => unpremultipliedAlphaTriplet = triplet.WithUnpremultipliedAlpha(alpha));
        TestUtils.AssertTriplet(unpremultipliedAlphaTriplet, expectedTriplet, 0.00000000001);
    }
}