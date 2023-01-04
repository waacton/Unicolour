namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class DifferenceTests
{
    private static Unicolour GetRandomColour() => Unicolour.FromRgb(RandomColours.Rgb().Tuple);

    [Test]
    public static void KnownDeltaE76()
    {
        var black = ColourLimits.Rgb["black"];
        var white = ColourLimits.Rgb["white"];
        var red = ColourLimits.Rgb["red"];
        var green = ColourLimits.Rgb["green"];
        var blue = ColourLimits.Rgb["blue"];
        var random = GetRandomColour();
        
        // delta-E76 colour differences are symmetric, i.e. green from red == red from green
        AssertKnownDeltaE76(black, white, 100.000000);
        AssertKnownDeltaE76(red, green, 170.565257);
        AssertKnownDeltaE76(green, blue, 258.682686);
        AssertKnownDeltaE76(blue, red, 176.314083);
        AssertKnownDeltaE76(random, random, 0.000000);

        for (var i = 0; i < 1000; i++)
        {
            var reference = GetRandomColour();
            var sample = GetRandomColour();
            Assert.That(reference.DeltaE76(sample), Is.EqualTo(sample.DeltaE76(reference)));
        }
    }
    
    [Test]
    public static void KnownDeltaE94ForGraphics()
    {
        var black = ColourLimits.Rgb["black"];
        var white = ColourLimits.Rgb["white"];
        var red = ColourLimits.Rgb["red"];
        var green = ColourLimits.Rgb["green"];
        var blue = ColourLimits.Rgb["blue"];
        var random = GetRandomColour();
        
        // delta-E94 colour differences are not symmetric, i.e. green from red != red from green
        AssertKnownDeltaE94(black, white, 100.000000, false);
        AssertKnownDeltaE94(red, green, 73.430410, false);
        AssertKnownDeltaE94(green, red, 68.800069, false);
        AssertKnownDeltaE94(green, blue, 105.90500, false);
        AssertKnownDeltaE94(blue, green, 100.577051, false);
        AssertKnownDeltaE94(blue, red, 61.242091, false);
        AssertKnownDeltaE94(red, blue, 70.580743, false);
        AssertKnownDeltaE94(random, random, 0.000000, false);
    }
    
    [Test]
    public static void KnownDeltaE94ForTextiles()
    {
        var black = ColourLimits.Rgb["black"];
        var white = ColourLimits.Rgb["white"];
        var red = ColourLimits.Rgb["red"];
        var green = ColourLimits.Rgb["green"];
        var blue = ColourLimits.Rgb["blue"];
        var random = GetRandomColour();
        
        // delta-E94 colour differences are not symmetric, i.e. green from red != red from green
        AssertKnownDeltaE94(black, white, 50.000000, true);
        AssertKnownDeltaE94(red, green, 69.731867, true);
        AssertKnownDeltaE94(green, red, 64.530477, true);
        AssertKnownDeltaE94(green, blue, 98.259347, true);
        AssertKnownDeltaE94(blue, green, 92.093048, true);
        AssertKnownDeltaE94(blue, red, 61.104684, true);
        AssertKnownDeltaE94(red, blue, 71.003011, true);
        AssertKnownDeltaE94(random, random, 0.000000, true);
    }
    
    [Test]
    public static void KnownDeltaE00()
    {
        var black = ColourLimits.Rgb["black"];
        var white = ColourLimits.Rgb["white"];
        var red = ColourLimits.Rgb["red"];
        var green = ColourLimits.Rgb["green"];
        var blue = ColourLimits.Rgb["blue"];
        var random = GetRandomColour();
        
        // delta-E00 colour differences are apparently not symmetric, according to http://www.brucelindbloom.com/ColorDifferenceCalcHelp.html
        // but I've not come across an asymmetric pair after testing millions of random colours
        AssertKnownDeltaE00(black, white, 100.000000);
        AssertKnownDeltaE00(red, green, 86.608245);
        AssertKnownDeltaE00(green, blue, 83.185881);
        AssertKnownDeltaE00(blue, red, 52.881375);
        AssertKnownDeltaE00(random, random, 0.000000);
        
        for (var i = 0; i < 1000; i++)
        {
            var reference = GetRandomColour();
            var sample = GetRandomColour();
            Assert.That(reference.DeltaE00(sample), Is.EqualTo(sample.DeltaE00(reference)));
        }
    }
    
    [Test]
    public static void KnownDeltaEz()
    {
        var black = ColourLimits.Rgb["black"];
        var white = ColourLimits.Rgb["white"];
        var red = ColourLimits.Rgb["red"];
        var green = ColourLimits.Rgb["green"];
        var blue = ColourLimits.Rgb["blue"];
        var random = GetRandomColour();
        
        // delta-Ez colour differences are symmetric, i.e. green from red == red from green
        AssertKnownDeltaEz(black, white, 0.167174);
        AssertKnownDeltaEz(red, green, 0.195524);
        AssertKnownDeltaEz(green, blue, 0.271571);
        AssertKnownDeltaEz(blue, red, 0.281457);
        AssertKnownDeltaEz(random, random, 0.000000);

        for (var i = 0; i < 1000; i++)
        {
            var reference = GetRandomColour();
            var sample = GetRandomColour();
            Assert.That(reference.DeltaEz(sample), Is.EqualTo(sample.DeltaEz(reference)));
        }
    }

    [Test]
    public static void RelativeDeltaE76() => AssertRelativeLabBasedDeltas(Comparison.DeltaE76);
    
    [Test]
    public static void RelativeDeltaE94ForGraphics() => AssertRelativeLabBasedDeltas((reference, sample) => reference.DeltaE94(sample));
    
    [Test]
    public static void RelativeDeltaE94ForTextiles() => AssertRelativeLabBasedDeltas((reference, sample) => reference.DeltaE94(sample, true));
    
    [Test]
    public static void RelativeDeltaE00() => AssertRelativeLabBasedDeltas(Comparison.DeltaE00);

    [Test]
    public static void RelativeDeltaEz() => AssertRelativeJchBasedDeltas();

    [Test]
    public static void NotNumberDeltaE76() => AssertNotNumberDeltas(Comparison.DeltaE76);
    
    [Test]
    public static void NotNumberDeltaE94ForGraphics() => AssertNotNumberDeltas((reference, sample) => reference.DeltaE94(sample));
    
    [Test]
    public static void NotNumberDeltaE94ForTextiles() => AssertNotNumberDeltas((reference, sample) => reference.DeltaE94(sample, true));
    
    [Test]
    public static void NotNumberDeltaE00() => AssertNotNumberDeltas(Comparison.DeltaE00);

    [Test]
    public static void NotNumberDeltaEz() => AssertNotNumberDeltas(Comparison.DeltaEz);
    
    private static void AssertKnownDeltaE76(Unicolour reference, Unicolour sample, double expectedDelta)
    {
        var delta = reference.DeltaE76(sample);
        var symmetricDelta = sample.DeltaE76(reference);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.00005));
        Assert.That(symmetricDelta, Is.EqualTo(delta));
    }
    
    private static void AssertKnownDeltaE94(Unicolour reference, Unicolour sample, double expectedDelta, bool isForTextiles)
    {
        var delta = reference.DeltaE94(sample, isForTextiles);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.0005));
    }
    
    private static void AssertKnownDeltaE00(Unicolour reference, Unicolour sample, double expectedDelta)
    {
        var delta = reference.DeltaE00(sample);
        var symmetricDelta = sample.DeltaE00(reference);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.0005));
        Assert.That(symmetricDelta, Is.EqualTo(delta));
    }
    
    private static void AssertKnownDeltaEz(Unicolour reference, Unicolour sample, double expectedDelta)
    {
        var delta = reference.DeltaEz(sample);
        var symmetricDelta = sample.DeltaEz(reference);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.00005));
        Assert.That(symmetricDelta, Is.EqualTo(delta));
    }

    private static void AssertRelativeLabBasedDeltas(Func<Unicolour, Unicolour, double> getDelta)
    {
        var red = ColourLimits.Lab["red"];
        var green = ColourLimits.Lab["green"];
        var yellow = ColourLimits.Lab["yellow"];
        var blue = ColourLimits.Lab["blue"];

        var redGreenDelta = getDelta(red, green);
        var redBlueDelta = getDelta(red, blue);
        var redYellowDelta = getDelta(red, yellow);
        
        var greenRedDelta = getDelta(green, red);
        var greenBlueDelta = getDelta(green, blue);
        var greenYellowDelta = getDelta(green, yellow);
        
        var blueRedDelta = getDelta(blue, red);
        var blueGreenDelta = getDelta(blue, green);
        var blueYellowDelta = getDelta(blue, yellow);
        
        var yellowRedDelta = getDelta(yellow, red);
        var yellowGreenDelta = getDelta(yellow, green);
        var yellowBlueDelta = getDelta(yellow, blue);

        // opposite LAB A values
        Assert.That(redGreenDelta, Is.GreaterThan(redBlueDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(blueRedDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(redYellowDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(yellowRedDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(greenBlueDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(blueGreenDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(greenYellowDelta));
        Assert.That(redGreenDelta, Is.GreaterThan(yellowGreenDelta));
        
        Assert.That(greenRedDelta, Is.GreaterThan(redBlueDelta));
        Assert.That(greenRedDelta, Is.GreaterThan(blueRedDelta));
        Assert.That(greenRedDelta, Is.GreaterThan(redYellowDelta));
        Assert.That(greenRedDelta, Is.GreaterThan(yellowRedDelta));
        Assert.That(greenRedDelta, Is.GreaterThan(greenBlueDelta));
        Assert.That(greenRedDelta, Is.GreaterThan(blueGreenDelta));
        Assert.That(greenRedDelta, Is.GreaterThan(greenYellowDelta));
        Assert.That(greenRedDelta, Is.GreaterThan(yellowGreenDelta));
        
        // opposite LAB B values
        Assert.That(blueYellowDelta, Is.GreaterThan(blueRedDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(redBlueDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(blueGreenDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(greenBlueDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(yellowRedDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(redYellowDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(yellowGreenDelta));
        Assert.That(blueYellowDelta, Is.GreaterThan(greenYellowDelta));
        
        Assert.That(yellowBlueDelta, Is.GreaterThan(blueRedDelta));
        Assert.That(yellowBlueDelta, Is.GreaterThan(redBlueDelta));
        Assert.That(yellowBlueDelta, Is.GreaterThan(blueGreenDelta));
        Assert.That(yellowBlueDelta, Is.GreaterThan(greenBlueDelta));
        Assert.That(yellowBlueDelta, Is.GreaterThan(yellowRedDelta));
        Assert.That(yellowBlueDelta, Is.GreaterThan(redYellowDelta));
        Assert.That(yellowBlueDelta, Is.GreaterThan(yellowGreenDelta));
        Assert.That(yellowBlueDelta, Is.GreaterThan(greenYellowDelta));
    }
    
    private static void AssertRelativeJchBasedDeltas()
    {
        for (var i = 0; i < 100; i++)
        {
            AssertRelativeJchBasedDeltas(RandomColours.Jzczhz(), RandomColours.Jzczhz(), RandomColours.Jzczhz());
        }
    }

    private static void AssertRelativeJchBasedDeltas(ColourTriplet triplet1, ColourTriplet triplet2, ColourTriplet triplet3)
    {
        var jzczhzTriplets = new List<ColourTriplet> {triplet1, triplet2, triplet3};
        var js = jzczhzTriplets.Select(x => x.First).OrderBy(x => x).ToList();
        var cs = jzczhzTriplets.Select(x => x.Second).OrderBy(x => x).ToList();
        var hs = jzczhzTriplets.Select(x => x.Third).OrderBy(x => x).ToList();

        void AssertChangesInJ()
        {
            var uni1 = Unicolour.FromJzczhz(js[0], cs[0], hs[0]);
            var uni2 = Unicolour.FromJzczhz(js[1], cs[0], hs[0]);
            var uni3 = Unicolour.FromJzczhz(js[2], cs[0], hs[0]);

            var minMidDelta = uni1.DeltaEz(uni2);
            var midMaxDelta = uni2.DeltaEz(uni3);
            var minMaxDelta = uni1.DeltaEz(uni3);

            Assert.That(minMidDelta, Is.LessThan(minMaxDelta));
            Assert.That(midMaxDelta, Is.LessThan(minMaxDelta));
        }

        void AssertChangesInC()
        {
            var uni1 = Unicolour.FromJzczhz(js[0], cs[0], hs[0]);
            var uni2 = Unicolour.FromJzczhz(js[0], cs[1], hs[0]);
            var uni3 = Unicolour.FromJzczhz(js[0], cs[2], hs[0]);

            var minMidDelta = uni1.DeltaEz(uni2);
            var midMaxDelta = uni2.DeltaEz(uni3);
            var minMaxDelta = uni1.DeltaEz(uni3);

            Assert.That(minMidDelta, Is.LessThan(minMaxDelta));
            Assert.That(midMaxDelta, Is.LessThan(minMaxDelta));
        }

        void AssertChangesInH()
        {
            double Distance(double lowHue, double highHue) => Math.Min(highHue - lowHue, lowHue + 360 - highHue);
            var isInHueOrder = Distance(hs[0], hs[1]) < Distance(hs[0], hs[2]);
            var nearHue = isInHueOrder ? hs[1] : hs[2];
            var farHue = isInHueOrder ? hs[2] : hs[1];

            var startColour = Unicolour.FromJzczhz(js[0], cs[0], hs[0]);
            var nearColour = Unicolour.FromJzczhz(js[0], cs[0], nearHue);
            var farColour = Unicolour.FromJzczhz(js[0], cs[0], farHue);

            var smallerDelta = startColour.DeltaEz(nearColour);
            var largerDelta = startColour.DeltaEz(farColour);

            Assert.That(smallerDelta, Is.LessThan(largerDelta));
        }

        AssertChangesInJ();
        AssertChangesInC();
        AssertChangesInH();
    }
    
    private static void AssertNotNumberDeltas(Func<Unicolour, Unicolour, double> getDelta)
    {
        var unicolour = Unicolour.FromLab(double.NaN, double.NaN, double.NaN);
        var delta = getDelta(unicolour, unicolour);
        Assert.That(delta, Is.NaN);
    }
}