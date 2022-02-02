namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Lookups;

public static class DifferenceTests
{
    private static readonly Random Random = new();

    [Test]
    public static void KnownDifferences()
    {
        var black = ColourLimits.Rgb["black"];
        var white = ColourLimits.Rgb["white"];
        var red = ColourLimits.Rgb["red"];
        var green = ColourLimits.Rgb["green"];
        var blue = ColourLimits.Rgb["blue"];
        var random = Unicolour.FromRgb(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
        
        AssertKnownDifference(black, white, 100);
        AssertKnownDifference(red, green, 170.58);
        AssertKnownDifference(green, blue, 258.69);
        AssertKnownDifference(blue, red, 176.33);
        AssertKnownDifference(random, random, 0);
    }
    
    // these tests should be easier and improved if I add a Unicolour.FromLab() function
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
        var red = ColourLimits.Lab["red"];
        var green = ColourLimits.Lab["green"];
        var yellow = ColourLimits.Lab["yellow"];
        var blue = ColourLimits.Lab["blue"];
        
        var redGreenDelta = red.DeltaE76(green);
        var redBlueDelta = red.DeltaE76(blue);
        var redYellowDelta = red.DeltaE76(yellow);
        var greenBlueDelta = green.DeltaE76(blue);
        var greenYellowDelta = green.DeltaE76(yellow);

        Assert.That(redGreenDelta, Is.GreaterThan(redBlueDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(redYellowDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(greenBlueDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(greenYellowDelta));
    }
    
    [Test]
    public static void LabOppositeB()
    {
        var red = ColourLimits.Lab["red"];
        var green = ColourLimits.Lab["green"];
        var yellow = ColourLimits.Lab["yellow"];
        var blue = ColourLimits.Lab["blue"];
        
        var blueYellowDelta = blue.DeltaE76(yellow);
        var blueRedDelta = blue.DeltaE76(red);
        var blueGreenDelta = blue.DeltaE76(green);
        var yellowRedDelta = yellow.DeltaE76(red);
        var yellowGreenDelta = yellow.DeltaE76(green);

        Assert.That(blueYellowDelta, Is.GreaterThan(blueRedDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(blueGreenDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(yellowRedDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(yellowGreenDelta));
    }
}