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
    
    [Test] // assumes Ictcp scalar of 100
    public static void KnownDeltaEItp()
    {
        var black = ColourLimits.Rgb["black"];
        var white = ColourLimits.Rgb["white"];
        var red = ColourLimits.Rgb["red"];
        var green = ColourLimits.Rgb["green"];
        var blue = ColourLimits.Rgb["blue"];
        var random = GetRandomColour();
        
        // delta-EItp colour differences are symmetric, i.e. green from red == red from green
        AssertKnownDeltaEItp(black, white, 365.816926);
        AssertKnownDeltaEItp(red, green, 239.982435);
        AssertKnownDeltaEItp(green, blue, 234.838743);
        AssertKnownDeltaEItp(blue, red, 322.659678);
        AssertKnownDeltaEItp(random, random, 0.000000);

        for (var i = 0; i < 1000; i++)
        {
            var reference = GetRandomColour();
            var sample = GetRandomColour();
            Assert.That(reference.DeltaEItp(sample), Is.EqualTo(sample.DeltaEItp(reference)));
        }
    }
    
    [Test] // assumes Jzczhz scalar of 100
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
    public static void KnownDeltaEHyab()
    {
        var black = ColourLimits.Rgb["black"];
        var white = ColourLimits.Rgb["white"];
        var red = ColourLimits.Rgb["red"];
        var green = ColourLimits.Rgb["green"];
        var blue = ColourLimits.Rgb["blue"];
        var random = GetRandomColour();
        
        AssertKnownDeltaEHyab(black, white, 100.000000);
        AssertKnownDeltaEHyab(red, green, 201.534889);
        AssertKnownDeltaEHyab(green, blue, 308.110214);
        AssertKnownDeltaEHyab(blue, red, 196.009473);
        AssertKnownDeltaEHyab(random, random, 0.000000);
        
        for (var i = 0; i < 1000; i++)
        {
            var reference = GetRandomColour();
            var sample = GetRandomColour();
            Assert.That(reference.DeltaEHyab(sample), Is.EqualTo(sample.DeltaEHyab(reference)));
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
    public static void RelativeDeltaEItp() => AssertRelativeIctcpBasedDeltas();

    [Test]
    public static void RelativeDeltaEz() => AssertRelativeJzczhzBasedDeltas();
    
    [Test]
    public static void RelativeDeltaEHyab() => AssertRelativeLabBasedDeltas(Comparison.DeltaEHyab);

    [Test]
    public static void NotNumberDeltaE76() => AssertNotNumberDeltas(Comparison.DeltaE76, ColourSpace.Lab);
    
    [Test]
    public static void NotNumberDeltaE94ForGraphics() => AssertNotNumberDeltas((reference, sample) => reference.DeltaE94(sample), ColourSpace.Lab);
    
    [Test]
    public static void NotNumberDeltaE94ForTextiles() => AssertNotNumberDeltas((reference, sample) => reference.DeltaE94(sample, true), ColourSpace.Lab);
    
    [Test]
    public static void NotNumberDeltaE00() => AssertNotNumberDeltas(Comparison.DeltaE00, ColourSpace.Lab);
    
    [Test]
    public static void NotNumberDeltaEItp() => AssertNotNumberDeltas(Comparison.DeltaEItp, ColourSpace.Ictcp);

    [Test]
    public static void NotNumberDeltaEz() => AssertNotNumberDeltas(Comparison.DeltaEz, ColourSpace.Jzczhz);
    
    [Test]
    public static void NotNumberDeltaEHyab() => AssertNotNumberDeltas(Comparison.DeltaEHyab, ColourSpace.Lab);
    
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
    
    private static void AssertKnownDeltaEItp(Unicolour reference, Unicolour sample, double expectedDelta)
    {
        var delta = reference.DeltaEItp(sample);
        var symmetricDelta = sample.DeltaEItp(reference);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.00005));
        Assert.That(symmetricDelta, Is.EqualTo(delta));
    }
    
    private static void AssertKnownDeltaEz(Unicolour reference, Unicolour sample, double expectedDelta)
    {
        var delta = reference.DeltaEz(sample);
        var symmetricDelta = sample.DeltaEz(reference);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.00005));
        Assert.That(symmetricDelta, Is.EqualTo(delta));
    }
    
    private static void AssertKnownDeltaEHyab(Unicolour reference, Unicolour sample, double expectedDelta)
    {
        var delta = reference.DeltaEHyab(sample);
        var symmetricDelta = sample.DeltaEHyab(reference);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.0005));
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
    
    private static void AssertRelativeIctcpBasedDeltas()
    {
        for (var i = 0; i < 100; i++)
        {
            AssertRelativeIctcpBasedDeltas(RandomColours.Ictcp(), RandomColours.Ictcp(), RandomColours.Ictcp());
        }
    }
    
    private static void AssertRelativeIctcpBasedDeltas(ColourTriplet triplet1, ColourTriplet triplet2, ColourTriplet triplet3)
    {
        var ictcpTriplets = new List<ColourTriplet> {triplet1, triplet2, triplet3};
        var firsts = ictcpTriplets.Select(x => x.First).OrderBy(x => x).ToList();
        var seconds = ictcpTriplets.Select(x => x.Second).OrderBy(x => x).ToList();
        var thirds = ictcpTriplets.Select(x => x.Third).OrderBy(x => x).ToList();

        AssertDeltas(
            min: Unicolour.FromIctcp(firsts[0], seconds[0], thirds[0]),
            mid: Unicolour.FromIctcp(firsts[1], seconds[0], thirds[0]),
            max: Unicolour.FromIctcp(firsts[2], seconds[0], thirds[0])
        );
        
        AssertDeltas(
            min: Unicolour.FromIctcp(firsts[0], seconds[0], thirds[0]),
            mid: Unicolour.FromIctcp(firsts[0], seconds[1], thirds[0]),
            max: Unicolour.FromIctcp(firsts[0], seconds[2], thirds[0])
        );
        
        AssertDeltas(
            min: Unicolour.FromIctcp(firsts[0], seconds[0], thirds[0]),
            mid: Unicolour.FromIctcp(firsts[0], seconds[0], thirds[1]),
            max: Unicolour.FromIctcp(firsts[0], seconds[0], thirds[2])
        );
        
        void AssertDeltas(Unicolour min, Unicolour mid, Unicolour max)
        {
            var minMidDelta = min.DeltaEItp(mid);
            var midMaxDelta = mid.DeltaEItp(max);
            var minMaxDelta = min.DeltaEItp(max);
            Assert.That(minMidDelta, Is.LessThan(minMaxDelta));
            Assert.That(midMaxDelta, Is.LessThan(minMaxDelta));
        }
    }
    
    private static void AssertRelativeJzczhzBasedDeltas()
    {
        for (var i = 0; i < 100; i++)
        {
            AssertRelativeJzczhzBasedDeltas(RandomColours.Jzczhz(), RandomColours.Jzczhz(), RandomColours.Jzczhz());
        }
    }

    private static void AssertRelativeJzczhzBasedDeltas(ColourTriplet triplet1, ColourTriplet triplet2, ColourTriplet triplet3)
    {
        var jzczhzTriplets = new List<ColourTriplet> {triplet1, triplet2, triplet3};
        var firsts = jzczhzTriplets.Select(x => x.First).OrderBy(x => x).ToList();
        var seconds = jzczhzTriplets.Select(x => x.Second).OrderBy(x => x).ToList();
        var thirds = jzczhzTriplets.Select(x => x.Third).OrderBy(x => x).ToList();
        
        AssertDeltas(
            min: Unicolour.FromJzczhz(firsts[0], seconds[0], thirds[0]),
            mid: Unicolour.FromJzczhz(firsts[1], seconds[0], thirds[0]),
            max: Unicolour.FromJzczhz(firsts[2], seconds[0], thirds[0])
        );
        
        AssertDeltas(
            min: Unicolour.FromJzczhz(firsts[0], seconds[0], thirds[0]),
            mid: Unicolour.FromJzczhz(firsts[0], seconds[1], thirds[0]),
            max: Unicolour.FromJzczhz(firsts[0], seconds[2], thirds[0])
        );
        
        double Distance(double lowHue, double highHue) => Math.Min(highHue - lowHue, lowHue + 360 - highHue);
        var isInHueOrder = Distance(thirds[0], thirds[1]) < Distance(thirds[0], thirds[2]);
        var startHue = thirds[0];
        var nearHue = isInHueOrder ? thirds[1] : thirds[2];
        var farHue = isInHueOrder ? thirds[2] : thirds[1];
        
        AssertHueDeltas(
            start: Unicolour.FromJzczhz(firsts[0], seconds[0], startHue),
            near: Unicolour.FromJzczhz(firsts[0], seconds[0], nearHue),
            far: Unicolour.FromJzczhz(firsts[0], seconds[0], farHue)
        );
        
        void AssertDeltas(Unicolour min, Unicolour mid, Unicolour max)
        {
            var minMidDelta = min.DeltaEz(mid);
            var midMaxDelta = mid.DeltaEz(max);
            var minMaxDelta = min.DeltaEz(max);
            Assert.That(minMidDelta, Is.LessThan(minMaxDelta));
            Assert.That(midMaxDelta, Is.LessThan(minMaxDelta));
        }

        void AssertHueDeltas(Unicolour start, Unicolour near, Unicolour far)
        {
            var smallerDelta = start.DeltaEz(near);
            var largerDelta = start.DeltaEz(far);
            Assert.That(smallerDelta, Is.LessThan(largerDelta));
        }
    }
    
    private delegate Unicolour UnicolourCreator(double first, double second, double third, double alpha = 1.0);
    private static void AssertNotNumberDeltas(Func<Unicolour, Unicolour, double> getDelta, ColourSpace fromSpace)
    {
        UnicolourCreator CreateUnicolour()
        {
            return fromSpace switch
            {
                ColourSpace.Lab => Unicolour.FromLab,
                ColourSpace.Ictcp => Unicolour.FromIctcp,
                ColourSpace.Jzczhz => Unicolour.FromJzczhz
            };
        }
        
        var unicolour = CreateUnicolour().Invoke(double.NaN, double.NaN, double.NaN);
        var delta = getDelta(unicolour, unicolour);
        Assert.That(delta, Is.NaN);
    }
}