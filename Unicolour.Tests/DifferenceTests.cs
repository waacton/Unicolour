namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;

public static class DifferenceTests
{
    private static readonly Dictionary<string, Unicolour> RgbBounds = new()
    {
        {"black", Unicolour.FromRgb(0, 0, 0)},  // lab --> 0, 0, 0
        {"white", Unicolour.FromRgb(1, 1, 1)},  // lab --> 100, +0.01, -0.0
        {"red", Unicolour.FromRgb(1, 0, 0)},    // lab --> 53.23, +80.11, +67.22
        {"green", Unicolour.FromRgb(0, 1, 0)},  // lab --> 87.74, -86.18, +83.18
        {"blue", Unicolour.FromRgb(0, 0, 1)}    // lab --> 32.30, +79.20, -107.86
    };
    
    /*
     * to the best of my knowledge these represent "extremes" of the LAB colour space
     * where L remains in the middle of the range at 50
     * red      = L:50 A:+128 B:000
     * green    = L:50 A:-128 B:000
     * yellow   = L:50 A:000 B:+128
     * blue     = L:50 A:000 B:-128
     * ----------
     * could easily be wrong...
     * these tests should be easier and improved if I add a Unicolour.FromLab() function
     */
    private static Unicolour GetUnicolour(double r, double g, double b) => Unicolour.FromRgb(r / 255.0, g / 255.0, b / 255.0);
    private static readonly Dictionary<string, Unicolour> LabBounds = new()
    {
        {"red", GetUnicolour(255, 0, 124.7312453744219)},                   // lab via RGB --> 54.80, +84.32, +5.90
        {"green", GetUnicolour(0, 154.5289567694383, 116.42724277049872)},  // lab via RGB --> 56.64, -43.99, +10.41
        {"yellow", GetUnicolour(148.5935680937267, 116.00552286250596, 0)}, // lab via RGB --> 50.50, +3.46, +56.64
        {"blue", GetUnicolour(0, 138.39087099867112, 255)}                  // lab via RGB --> 57.57, +12.37, -66.32
    };
    
    [Test]
    public static void KnownDifferences()
    {
        AssertKnownDifference(RgbBounds["black"], RgbBounds["white"], 100);
        AssertKnownDifference(RgbBounds["red"], RgbBounds["green"], 170.58);
        AssertKnownDifference(RgbBounds["green"], RgbBounds["blue"], 258.69);
        AssertKnownDifference(RgbBounds["blue"], RgbBounds["red"], 176.33);
    }
    
    private static void AssertKnownDifference(Unicolour colour1, Unicolour colour2, double expectedDelta)
    {
        var delta1 = colour1.DeltaE76(colour2);
        var delta2 = colour2.DeltaE76(colour1);
        Assert.That(delta1, Is.EqualTo(expectedDelta).Within(0.005));
        Assert.That(delta1, Is.EqualTo(delta2));
    }

    [Test]
    public static void LabOppositeA()
    {
        var redGreenDelta = LabBounds["red"].DeltaE76(LabBounds["green"]);
        var redBlueDelta = LabBounds["red"].DeltaE76(LabBounds["blue"]);
        var redYellowDelta = LabBounds["red"].DeltaE76(LabBounds["yellow"]);
        var greenBlueDelta = LabBounds["green"].DeltaE76(LabBounds["blue"]);
        var greenYellowDelta = LabBounds["green"].DeltaE76(LabBounds["yellow"]);

        Assert.That(redGreenDelta, Is.GreaterThan(redBlueDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(redYellowDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(greenBlueDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(greenYellowDelta));
    }
    
    [Test]
    public static void LabOppositeB()
    {
        var blueYellowDelta = LabBounds["blue"].DeltaE76(LabBounds["yellow"]);
        var blueRedDelta = LabBounds["blue"].DeltaE76(LabBounds["red"]);
        var blueGreenDelta = LabBounds["blue"].DeltaE76(LabBounds["green"]);
        var yellowRedDelta = LabBounds["yellow"].DeltaE76(LabBounds["red"]);
        var yellowGreenDelta = LabBounds["yellow"].DeltaE76(LabBounds["green"]);

        Assert.That(blueYellowDelta, Is.GreaterThan(blueRedDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(blueGreenDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(yellowRedDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(yellowGreenDelta));
    }
}