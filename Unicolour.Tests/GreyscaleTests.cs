﻿namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class GreyscaleTests
{
    private static readonly List<ColourSpace> JzSpaces = new() {ColourSpace.Jzazbz, ColourSpace.Jzczhz};

    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(-0.00000000001, 0.0, -0.0, true)]
    [TestCase(-0.0, -0.00000000001, 0.0, true)]
    [TestCase(0.0, -0.0, -0.00000000001, true)]
    [TestCase(0.00000000001, 0.0, 0.0, false)]
    [TestCase(0.0, 0.00000000001, 0.0, false)]
    [TestCase(0.0, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.5, 0.5, true)]
    [TestCase(0.50000000001, 0.5, 0.5, false)]
    [TestCase(0.5, 0.50000000001, 0.5, false)]
    [TestCase(0.5, 0.5, 0.50000000001, false)]
    [TestCase(0.49999999999, 0.5, 0.5, false)]
    [TestCase(0.5, 0.49999999999, 0.5, false)]
    [TestCase(0.5, 0.5, 0.49999999999, false)]
    [TestCase(1.0, 1.0, 1.0, true)]
    [TestCase(1.00000000001, 1.0, 1.0, true)]
    [TestCase(1.0, 1.00000000001, 1.0, true)]
    [TestCase(1.0, 1.0, 1.00000000001, true)]
    [TestCase(0.99999999999, 1.0, 1.0, false)]
    [TestCase(1.0, 0.99999999999, 1.0, false)]
    [TestCase(1.0, 1.0, 0.99999999999, false)]
    public static void GreyscaleRgb(double r, double g, double b, bool expected) => AssertUnicolour(Unicolour.FromRgb(r, g, b), expected);

    [TestCase(180.0, 0.0, 0.5, true)]
    [TestCase(180.0, -0.00000000001, 0.5, true)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, true)]
    [TestCase(180.0, 0.5, -0.00000000001, true)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    public static void GreyscaleHsb(double h, double s, double b, bool expected) => AssertUnicolour(Unicolour.FromHsb(h, s, b), expected);

    [TestCase(180.0, 0.0, 0.5, true)]
    [TestCase(180.0, -0.00000000001, 0.5, true)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, true)]
    [TestCase(180.0, 0.5, -0.00000000001, true)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    [TestCase(180.0, 0.5, 1.0, true)]
    [TestCase(180.0, 0.5, 1.00000000001, true)]
    [TestCase(180.0, 0.5, 0.99999999999, false)]
    public static void GreyscaleHsl(double h, double s, double l, bool expected) => AssertUnicolour(Unicolour.FromHsl(h, s, l), expected);
    
    [TestCase(180.0, 1.0, 0.0, true)]
    [TestCase(180.0, 1.00000000001, 0.0, true)]
    [TestCase(180.0, 0.99999999999, 0.0, false)]
    [TestCase(180.0, 0.0, 1.0, true)]
    [TestCase(180.0, 0.0, 1.00000000001, true)]
    [TestCase(180.0, 0.0, 0.99999999999, false)]
    [TestCase(180.0, 0.5, 0.5, true)]
    [TestCase(180.0, 0.50000000001, 0.5, true)]
    [TestCase(180.0, 0.49999999999, 0.5, false)]
    [TestCase(180.0, 0.5, 0.5, true)]
    [TestCase(180.0, 0.5, 0.50000000001, true)]
    [TestCase(180.0, 0.5, 0.49999999999, false)]
    public static void GreyscaleHwb(double h, double w, double b, bool expected) => AssertUnicolour(Unicolour.FromHwb(h, w, b), expected);

    // XYZ does not currently attempt to determine greyscale status from XYZ triplet, too much room for error
    // subsequent colour spaces may later report to be greyscale based on their own triplet values
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(0.5, 0.5, 0.5)]
    [TestCase(0.25, 0.5, 0.75)]
    [TestCase(0.95047, 1.0, 1.08883)]
    public static void GreyscaleXyz(double x, double y, double z) => AssertUnicolour(Unicolour.FromXyz(x, y, z), false);
    
    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(0.0, 0.0, -0.00000000001, true)]
    [TestCase(0.0, 0.0, 0.00000000001, false)]
    [TestCase(1.0, 1.0, 0.0, true)]
    [TestCase(1.0, 1.0, -0.00000000001, true)]
    [TestCase(1.0, 1.0, 0.00000000001, false)]
    public static void GreyscaleXyy(double x, double y, double upperY, bool expected) => AssertUnicolour(Unicolour.FromXyy(x, y, upperY), expected);

    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    public static void GreyscaleLab(double l, double a, double b, bool expected) => AssertUnicolour(Unicolour.FromLab(l, a, b), expected);

    [TestCase(50.0, 0.0, 180.0, true)]
    [TestCase(50.0, -0.00000000001, 180.0, true)]
    [TestCase(50.0, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 50.0, 180.0, true)]
    [TestCase(-0.00000000001, 50.0, 180.0, true)]
    [TestCase(0.00000000001, 50.0, 180.0, false)]
    [TestCase(100.0, 50.0, 180.0, true)]
    [TestCase(100.00000000001, 50.0, 180.0, true)]
    [TestCase(99.99999999999, 50.0, 180.0, false)]
    public static void GreyscaleLchab(double l, double c, double h, bool expected) => AssertUnicolour(Unicolour.FromLchab(l, c, h), expected);

    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    public static void GreyscaleLuv(double l, double u, double v, bool expected) => AssertUnicolour(Unicolour.FromLuv(l, u, v), expected);

    [TestCase(50.0, 0.0, 180.0, true)]
    [TestCase(50.0, -0.00000000001, 180.0, true)]
    [TestCase(50.0, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 50.0, 180.0, true)]
    [TestCase(-0.00000000001, 50.0, 180.0, true)]
    [TestCase(0.00000000001, 50.0, 180.0, false)]
    [TestCase(100.0, 50.0, 180.0, true)]
    [TestCase(100.00000000001, 50.0, 180.0, true)]
    [TestCase(99.99999999999, 50.0, 180.0, false)]
    public static void GreyscaleLchuv(double l, double c, double h, bool expected) => AssertUnicolour(Unicolour.FromLchuv(l, c, h), expected);

    [TestCase(180.0, 0.0, 50, true)]
    [TestCase(180.0, -0.00000000001, 50, true)]
    [TestCase(180.0, 0.00000000001, 50, false)]
    [TestCase(180.0, 50, 0.0, true)]
    [TestCase(180.0, 50, -0.00000000001, true)]
    [TestCase(180.0, 50, 0.00000000001, false)]
    [TestCase(180.0, 50, 100.0, true)]
    [TestCase(180.0, 50, 100.00000000001, true)]
    [TestCase(180.0, 50, 0.99999999999, false)]
    public static void GreyscaleHsluv(double h, double s, double l, bool expected) => AssertUnicolour(Unicolour.FromHsluv(h, s, l), expected);

    [TestCase(180.0, 0.0, 50, true)]
    [TestCase(180.0, -0.00000000001, 50, true)]
    [TestCase(180.0, 0.00000000001, 50, false)]
    [TestCase(180.0, 50, 0.0, true)]
    [TestCase(180.0, 50, -0.00000000001, true)]
    [TestCase(180.0, 50, 0.00000000001, false)]
    [TestCase(180.0, 50, 100.0, true)]
    [TestCase(180.0, 50, 100.00000000001, true)]
    [TestCase(180.0, 50, 0.99999999999, false)]
    public static void GreyscaleHpluv(double h, double s, double l, bool expected) => AssertUnicolour(Unicolour.FromHpluv(h, s, l), expected);

    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    public static void GreyscaleJzazbz(double jz, double az, double bz, bool expected) => AssertUnicolour(Unicolour.FromJzazbz(jz, az, bz), expected);

    [TestCase(0.5, 0.0, 180.0, true)]
    [TestCase(0.5, -0.00000000001, 180.0, true)]
    [TestCase(0.5, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 0.1, 180.0, true)]
    [TestCase(-0.00000000001, 0.1, 180.0, true)]
    [TestCase(0.00000000001, 0.05, 180.0, false)]
    [TestCase(1.0, 0.1, 180.0, true)]
    [TestCase(1.00000000001, 0.1, 180.0, true)]
    [TestCase(0.99999999999, 0.1, 180.0, false)]
    public static void GreyscaleJzczhz(double jz, double cz, double hz, bool expected) => AssertUnicolour(Unicolour.FromJzczhz(jz, cz, hz), expected);

    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    public static void GreyscaleOklab(double l, double a, double b, bool expected) => AssertUnicolour(Unicolour.FromOklab(l, a, b), expected);

    [TestCase(0.5, 0.0, 180.0, true)]
    [TestCase(0.5, -0.00000000001, 180.0, true)]
    [TestCase(0.5, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 0.25, 180.0, true)]
    [TestCase(-0.00000000001, 0.25, 180.0, true)]
    [TestCase(0.00000000001, 0.25, 180.0, false)]
    [TestCase(1.0, 0.25, 180.0, true)]
    [TestCase(1.00000000001, 0.25, 180.0, true)]
    [TestCase(0.99999999999, 0.25, 180.0, false)]
    public static void GreyscaleOklch(double l, double c, double h, bool expected) => AssertUnicolour(Unicolour.FromOklch(l, c, h), expected);

    private static void AssertUnicolour(Unicolour unicolour, bool shouldBeGreyscale)
    {
        var data = new ColourModeData(unicolour);
        var initial = unicolour.InitialRepresentation();
        AssertInitialRepresentation(initial, shouldBeGreyscale);

        if (!initial.IsGreyscale)
        {
            // if initial representation is non-greyscale
            // downstream representations should not have explicit hue (only the initial can have explicit hue)
            // and the first downstream representation should have no explicit behaviour
            // note: it's possible for downstream conversions to result in greyscale or NaN, especially with outlier values e.g. negatives
            var spaces = Enum.GetValues<ColourSpace>().Except(new[] {initial.ColourSpace}).ToList();
            Assert.That(data.Modes(spaces), Has.None.EqualTo(ColourMode.ExplicitHue));
            Assert.That(data.Modes(spaces), Has.Some.EqualTo(ColourMode.NoExplicitBehaviour));
        }
        else
        {
            var isInitiallyJzSpace = JzSpaces.Contains(initial.ColourSpace);
            if (isInitiallyJzSpace)
            {
                AssertGreyscaleDownstreamFromJz(initial.ColourSpace, data);
            }
            else
            {
                AssertGreyscaleDownstreamExceptJz(initial.ColourSpace, data);
                AssertGreyscaleDownstreamOnlyJz(data);
            }
        }
    }
    
    private static void AssertInitialRepresentation(ColourRepresentation initial, bool shouldBeGreyscale)
    {
        Assert.That(initial.ColourMode, Is.EqualTo(initial.HasHueAxis ? ColourMode.ExplicitHue : ColourMode.NoExplicitBehaviour));
        Assert.That(initial.IsGreyscale, Is.EqualTo(shouldBeGreyscale));
        Assert.That(initial.IsEffectivelyGreyscale, Is.EqualTo(shouldBeGreyscale));
        Assert.That(initial.IsEffectivelyHued, Is.EqualTo(initial.HasHueAxis));
        Assert.That(initial.IsEffectivelyNaN, Is.False);
    }

    private static void AssertGreyscaleDownstreamExceptJz(ColourSpace initialColourSpace, ColourModeData data)
    {
        var excludedSpaces = JzSpaces.Concat(new[] {initialColourSpace});
        var spaces = Enum.GetValues<ColourSpace>().Except(excludedSpaces).ToList();
        
        // if initial representation is greyscale, downstream non-Jz* representations should all be greyscale too
        Assert.That(data.Modes(spaces), Has.All.EqualTo(ColourMode.ExplicitGreyscale));
        Assert.That(data.Greyscale(spaces), Has.All.True);
        Assert.That(data.Hued(spaces), Has.All.False);
        Assert.That(data.NaN(spaces), Has.All.False);
    }

    private static void AssertGreyscaleDownstreamOnlyJz(ColourModeData data)
    {
        var spaces = JzSpaces;
        
        // if initial representation is greyscale, downstream Jz* representations should all be either greyscale or NaN
        var greyscaleOrNan = data.Greyscale(spaces).Zip(data.NaN(spaces), (a, b) => a || b).ToList();
        Assert.That(data.Modes(spaces), Has.All.EqualTo(ColourMode.ExplicitGreyscale).Or.EqualTo(ColourMode.ExplicitNaN));
        Assert.That(greyscaleOrNan, Has.All.True);
        Assert.That(data.Hued(spaces), Has.All.False);
    }

    private static void AssertGreyscaleDownstreamFromJz(ColourSpace initialColourSpace, ColourModeData data)
    {
        var excludedSpaces = JzSpaces.Concat(new[] {initialColourSpace});
        var spaces = Enum.GetValues<ColourSpace>().Except(excludedSpaces).ToList();

        // if initial representation is greyscale and Jz*, downstream representations should all be either greyscale or NaN
        var greyscaleOrNan = data.Greyscale(spaces).Zip(data.NaN(spaces), (a, b) => a || b).ToList();
        Assert.That(data.Modes(spaces), Has.None.EqualTo(ColourMode.ExplicitHue));
        Assert.That(data.Modes(spaces), Has.None.EqualTo(ColourMode.NoExplicitBehaviour));
        Assert.That(greyscaleOrNan, Has.All.True);
        Assert.That(data.Hued(spaces), Has.All.False);
    }
}