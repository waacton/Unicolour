﻿namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ColourTripletTests
{
    private static readonly List<TestCaseData> GetHueTestData = new()
    {
        new TestCaseData(new ColourTriplet(7.7, 8.8, 9.9, null), null),
        new TestCaseData(new ColourTriplet(7.7, 8.8, 9.9, 0), 7.7),
        new TestCaseData(new ColourTriplet(7.7, 8.8, 9.9, 1), null),
        new TestCaseData(new ColourTriplet(7.7, 8.8, 9.9, 2), 9.9)
    };
    
    private static readonly List<TestCaseData> OverrideHueTestData = new()
    {
        new TestCaseData(new ColourTriplet(7.7, 8.8, 9.9, null), 6.6, null),
        new TestCaseData(new ColourTriplet(7.7, 8.8, 9.9, 0), 6.6, new ColourTriplet(6.6, 8.8, 9.9, 0)),
        new TestCaseData(new ColourTriplet(7.7, 8.8, 9.9, 1), 6.6, null),
        new TestCaseData(new ColourTriplet(7.7, 8.8, 9.9, 2), 6.6, new ColourTriplet(7.7, 8.8, 6.6, 0))
    };
    
    private static readonly List<TestCaseData> ModuloHueTestData = new()
    {
        new TestCaseData(new ColourTriplet(-270, 450, 810, null), new ColourTriplet(-270, 450, 810, null)),
        new TestCaseData(new ColourTriplet(-270, 450, 810, 0), new ColourTriplet(90, 450, 810, 0)),
        new TestCaseData(new ColourTriplet(-270, 450, 810, 1), null),
        new TestCaseData(new ColourTriplet(-270, 450, 810, 2), new ColourTriplet(-270, 450, 90, 2))
    };
    
    private static readonly List<TestCaseData> PremultipliedAlphaTestData = new()
    {
        new TestCaseData(new ColourTriplet(2, 10, -8.8, null), 0.5, new ColourTriplet(1, 5, -4.4, null)),
        new TestCaseData(new ColourTriplet(2, 10, -8.8, 0), 0.5, new ColourTriplet(2, 5, -4.4, 0)),
        new TestCaseData(new ColourTriplet(2, 10, -8.8, 1), 0.5, null),
        new TestCaseData(new ColourTriplet(2, 10, -8.8, 2), 0.5, new ColourTriplet(1, 5, -8.8, 2))
    };
    
    private static readonly List<TestCaseData> UnpremultipliedAlphaTestData = new()
    {
        new TestCaseData(new ColourTriplet(1, 5, -4.4, null), 0.5, new ColourTriplet(2, 10, -8.8, null)),
        new TestCaseData(new ColourTriplet(1, 5, -4.4, 0), 0.5, new ColourTriplet(1, 10, -8.8, 0)),
        new TestCaseData(new ColourTriplet(1, 5, -4.4, 1), 0.5, null),
        new TestCaseData(new ColourTriplet(1, 5, -4.4, 2), 0.5, new ColourTriplet(2, 10, -4.4, 2))
    };
    
    private static readonly List<TestCaseData> UnpremultipliedZeroAlphaTestData = new()
    {
        new TestCaseData(new ColourTriplet(1, 5, -4.4, null), 0.0, new ColourTriplet(1, 5, -4.4, null)),
        new TestCaseData(new ColourTriplet(1, 5, -4.4, 0), 0.0, new ColourTriplet(1, 5, -4.4, 0)),
        new TestCaseData(new ColourTriplet(1, 5, -4.4, 1), 0.0, null),
        new TestCaseData(new ColourTriplet(1, 5, -4.4, 2), 0.0, new ColourTriplet(1, 5, -4.4, 2))
    };
    
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
        var array = triplet.AsArray();
        Assert.That(array[0], Is.EqualTo(first));
        Assert.That(array[1], Is.EqualTo(second));
        Assert.That(array[2], Is.EqualTo(third));
        Assert.That(array[0], Is.EqualTo(triplet.First));
        Assert.That(array[1], Is.EqualTo(triplet.Second));
        Assert.That(array[2], Is.EqualTo(triplet.Third));
    }
    
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