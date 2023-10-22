namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class DifferenceTests
{
    private const double Tolerance = 0.00005;
    private static Unicolour RandomColour() => RandomColours.UnicolourFromRgb();

    [TestCase(ColourLimit.Black, ColourLimit.White, 100.000000)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 170.565257)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 258.682686)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 176.314083)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 100.000000)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 170.565257)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 258.682686)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 176.314083)]
    public void DeltaE76(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaE76(sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }

    // ΔE94 is not symmetric
    [TestCase(ColourLimit.Black, ColourLimit.White, 100.000000)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 73.430410)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 105.90500)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 61.242091)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 100.000000)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 68.800069)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 100.577051)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 70.580743)]
    public void DeltaE94Graphics(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaE94(sample, false);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.0005));
    }

    // ΔE94 is not symmetric
    [TestCase(ColourLimit.Black, ColourLimit.White, 50.000000)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 69.731867)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 98.259347)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 61.104684)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 50.000000)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 64.530477)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 92.093048)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 71.003011)]
    public void DeltaE94Textiles(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaE94(sample, true);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }

    // apparently not symmetric, according to http://www.brucelindbloom.com/ColorDifferenceCalcHelp.html
    // but I've not come across an asymmetric pair after testing millions of random colours
    [TestCase(ColourLimit.Black, ColourLimit.White, 100.000000)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 86.608245)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 83.185881)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 52.881375)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 100.000000)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 86.608245)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 83.185881)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 52.881375)]
    public void DeltaE00(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaE00(sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    // assumes Ictcp scalar of 100
    [TestCase(ColourLimit.Black, ColourLimit.White, 365.816926)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 239.982435)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 234.838743)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 322.659678)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 365.816926)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 239.982435)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 234.838743)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 322.659678)]
    public void DeltaEItp(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaEItp(sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    // assumes Jzczhz scalar of 100
    [TestCase(ColourLimit.Black, ColourLimit.White, 0.167174)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 0.195524)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 0.271571)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 0.281457)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 0.167174)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 0.195524)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 0.271571)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 0.281457)]
    public void DeltaEz(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaEz(sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }

    [TestCase(ColourLimit.Black, ColourLimit.White, 100.000000)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 201.534889)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 308.110214)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 196.009473)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 100.000000)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 201.534889)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 308.110214)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 196.009473)]
    public void DeltaEHyab(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaEHyab(sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    [TestCase(ColourLimit.Black, ColourLimit.White, 1.000000)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 0.519797)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 0.673409)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 0.537117)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 1.000000)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 0.519797)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 0.673409)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 0.537117)]
    public void DeltaEOk(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaEOk(sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    [TestCase(ColourLimit.Black, ColourLimit.White, 100.024844)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 76.105436)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 92.321874)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 84.119853)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 100.024844)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 76.105436)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 92.321874)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 84.119853)]
    public void DeltaCam02(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaECam02(sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }

    [TestCase(ColourLimit.Black, ColourLimit.White, 25.661622)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 22.523082)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 24.596320)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 20.689226)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 25.661622)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 22.523082)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 24.596320)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 20.689226)]
    public void DeltaCam16(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.DeltaECam16(sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    [Test]
    public void RandomSymmetricDeltaE76() => AssertRandomSymmetricDeltas(Comparison.DeltaE76);
    
    [Test]
    public void RandomSymmetricDeltaE00() => AssertRandomSymmetricDeltas(Comparison.DeltaE00);
    
    [Test]
    public void RandomSymmetricDeltaEItp() => AssertRandomSymmetricDeltas(Comparison.DeltaEItp);
    
    [Test]
    public void RandomSymmetricDeltaEz() => AssertRandomSymmetricDeltas(Comparison.DeltaEz);
    
    [Test]
    public void RandomSymmetricDeltaEHyab() => AssertRandomSymmetricDeltas(Comparison.DeltaEHyab);
    
    [Test]
    public void RandomSymmetricDeltaEOk() => AssertRandomSymmetricDeltas(Comparison.DeltaEOk);
    
    [Test]
    public void RandomSymmetricDeltaECam02() => AssertRandomSymmetricDeltas(Comparison.DeltaECam02);

    [Test]
    public void RandomSymmetricDeltaECam16() => AssertRandomSymmetricDeltas(Comparison.DeltaECam16);

    [Test]
    public void NotNumberDeltaE76() => AssertNotNumberDeltas(Comparison.DeltaE76, ColourSpace.Lab);
    
    [Test]
    public void NotNumberDeltaE94ForGraphics() => AssertNotNumberDeltas((reference, sample) => reference.DeltaE94(sample), ColourSpace.Lab);
    
    [Test]
    public void NotNumberDeltaE94ForTextiles() => AssertNotNumberDeltas((reference, sample) => reference.DeltaE94(sample, true), ColourSpace.Lab);
    
    [Test]
    public void NotNumberDeltaE00() => AssertNotNumberDeltas(Comparison.DeltaE00, ColourSpace.Lab);
    
    [Test]
    public void NotNumberDeltaEItp() => AssertNotNumberDeltas(Comparison.DeltaEItp, ColourSpace.Ictcp);

    [Test]
    public void NotNumberDeltaEz() => AssertNotNumberDeltas(Comparison.DeltaEz, ColourSpace.Jzczhz);
    
    [Test]
    public void NotNumberDeltaEHyab() => AssertNotNumberDeltas(Comparison.DeltaEHyab, ColourSpace.Lab);
    
    [Test]
    public void NotNumberDeltaEOk() => AssertNotNumberDeltas(Comparison.DeltaEOk, ColourSpace.Oklab);
    
    [Test]
    public void NotNumberDeltaECam02() => AssertNotNumberDeltas(Comparison.DeltaECam02, ColourSpace.Cam02);
    
    [Test]
    public void NotNumberDeltaECam16() => AssertNotNumberDeltas(Comparison.DeltaECam16, ColourSpace.Cam16);

    private static void AssertRandomSymmetricDeltas(Func<Unicolour, Unicolour, double> getDelta)
    {
        for (var i = 0; i < 100; i++)
        {
            var reference = RandomColour();
            var sample = RandomColour();
            Assert.That(getDelta(reference, sample), Is.EqualTo(getDelta(sample, reference)));
        }
    }

    private static void AssertNotNumberDeltas(Func<Unicolour, Unicolour, double> getDelta, ColourSpace colourSpace)
    {
        var unicolour = GetUnicolour(colourSpace, double.NaN, double.NaN, double.NaN);
        var delta = getDelta(unicolour, unicolour);
        Assert.That(delta, Is.NaN);
    }
    
    private delegate Unicolour UnicolourConstructor(double first, double second, double third, double alpha = 1.0);
    private static Unicolour GetUnicolour(ColourSpace colourSpace, double first, double second, double third)
    {
        UnicolourConstructor constructor = colourSpace switch
        {
            ColourSpace.Lab => Unicolour.FromLab,
            ColourSpace.Ictcp => Unicolour.FromIctcp,
            ColourSpace.Jzczhz => Unicolour.FromJzczhz,
            ColourSpace.Oklab => Unicolour.FromOklab,
            ColourSpace.Cam02 => Unicolour.FromCam02,
            ColourSpace.Cam16 => Unicolour.FromCam16
        };

        return constructor.Invoke(first, second, third);
    }
}