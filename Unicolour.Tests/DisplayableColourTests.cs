﻿namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public static class DisplayableColourTests
{
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(0.5, 0.5, 0.5)]
    [TestCase(1.0, 1.0, 1.0)]
    [TestCase(double.Epsilon, double.Epsilon, double.Epsilon)]
    public static void DisplayableRgb(double r, double g, double b)
    {
        var unicolour = Unicolour.FromRgb(r, g, b);
        Assert.That(unicolour.IsDisplayable, Is.True);
    }
    
    [TestCase(-0.00001, 0.0, 0.0)]
    [TestCase(0.0, -0.00001, 0.0)]
    [TestCase(0.0, 0.0, -0.00001)]
    [TestCase(1.00001, 1.0, 1.0)]
    [TestCase(1.0, 1.00001, 1.0)]
    [TestCase(1.0, 1.0, 1.00001)]
    [TestCase(double.MaxValue, 0.5, 0.5)]
    [TestCase(0.5, double.MaxValue, 0.5)]
    [TestCase(0.5, 0.5, double.MaxValue)]
    [TestCase(double.MinValue, 0.5, 0.5)]
    [TestCase(0.5, double.MinValue, 0.5)]
    [TestCase(0.5, 0.5, double.MinValue)]
    [TestCase(double.PositiveInfinity, 0.5, 0.5)]
    [TestCase(0.5, double.PositiveInfinity, 0.5)]
    [TestCase(0.5, 0.5, double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity, 0.5, 0.5)]
    [TestCase(0.5, double.NegativeInfinity, 0.5)]
    [TestCase(0.5, 0.5, double.NegativeInfinity)]
    [TestCase(double.NaN, 0.5, 0.5)]
    [TestCase(0.5, double.NaN, 0.5)]
    [TestCase(0.5, 0.5, double.NaN)]
    public static void UndisplayableRgb(double r, double g, double b)
    {
        var unicolour = Unicolour.FromRgb(r, g, b);
        Assert.That(unicolour.IsDisplayable, Is.False);
    }
}