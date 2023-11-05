namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class DifferenceTests
{
    private const double Tolerance = 0.00005;
    
    // ReSharper disable once CollectionNeverQueried.Local
    private static readonly List<(Unicolour reference, Unicolour sample)> ReferenceSamplePairs = new();
    static DifferenceTests()
    {
        for (var i = 0; i < 100; i++)
        {
            var reference = RandomColours.UnicolourFrom(ColourSpace.Rgb);
            var sample = RandomColours.UnicolourFrom(ColourSpace.Rgb);
            ReferenceSamplePairs.Add((reference, sample));
        }
    }

    [TestCase(ColourLimit.Black, ColourLimit.White, 100.000000)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 170.565257)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 258.682686)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 176.314083)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 100.000000)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 170.565257)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 258.682686)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 176.314083)]
    public void Cie76(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Cie76, sample);
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
    public void Cie94Graphics(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Cie94, sample);
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
    public void Cie94Textiles(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Cie94Textiles, sample);
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
    public void Ciede2000(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Ciede2000, sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    // Cmc is not symmetric
    [TestCase(ColourLimit.Black, ColourLimit.White, 195.694716)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 108.449110)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 100.565250)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 76.450069)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 67.480171)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 65.837219)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 124.001222)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 109.761942)]
    public void CmcAcceptability(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.CmcAcceptability, sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    // Cmc is not symmetric
    [TestCase(ColourLimit.Black, ColourLimit.White, 97.847358)]
    [TestCase(ColourLimit.Red, ColourLimit.Green, 105.146203)]
    [TestCase(ColourLimit.Green, ColourLimit.Blue, 94.630593)]
    [TestCase(ColourLimit.Blue, ColourLimit.Red, 73.359104)]
    [TestCase(ColourLimit.White, ColourLimit.Black, 33.740085)]
    [TestCase(ColourLimit.Green, ColourLimit.Red, 62.338288)]
    [TestCase(ColourLimit.Blue, ColourLimit.Green, 110.145001)]
    [TestCase(ColourLimit.Red, ColourLimit.Blue, 108.570712)]
    public void CmcPerceptibility(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.CmcPerceptibility, sample);
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
    public void Itp(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Itp, sample);
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
    public void Z(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Z, sample);
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
    public void Hyab(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Hyab, sample);
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
    public void Ok(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Ok, sample);
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
    public void Cam02(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Cam02, sample);
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
    public void Cam16(ColourLimit referenceColour, ColourLimit sampleColour, double expectedDelta)
    {
        var reference = ColourLimits.Rgb[referenceColour];
        var sample = ColourLimits.Rgb[sampleColour];
        var delta = reference.Difference(DeltaE.Cam16, sample);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    [Test, Combinatorial]
    public static void RandomSymmetric(
        [Values(DeltaE.Cie76, DeltaE.Ciede2000, DeltaE.Itp, DeltaE.Z, DeltaE.Hyab, DeltaE.Ok, DeltaE.Cam02, DeltaE.Cam16)] DeltaE deltaE, 
        [ValueSource(nameof(ReferenceSamplePairs))] (Unicolour reference, Unicolour sample) pair)
    {
        var reference = pair.reference;
        var sample = pair.sample;
        Assert.That(reference.Difference(deltaE, sample), Is.EqualTo(sample.Difference(deltaE, reference)));
    }
    
    [TestCase(DeltaE.Cie76, ColourSpace.Lab)]
    [TestCase(DeltaE.Cie94, ColourSpace.Lab)]
    [TestCase(DeltaE.Cie94Textiles, ColourSpace.Lab)]
    [TestCase(DeltaE.Ciede2000, ColourSpace.Lab)]
    [TestCase(DeltaE.CmcAcceptability, ColourSpace.Lab)]
    [TestCase(DeltaE.CmcPerceptibility, ColourSpace.Lab)]
    [TestCase(DeltaE.Itp, ColourSpace.Ictcp)]
    [TestCase(DeltaE.Z, ColourSpace.Jzczhz)]
    [TestCase(DeltaE.Hyab, ColourSpace.Lab)]
    [TestCase(DeltaE.Ok, ColourSpace.Oklab)]
    [TestCase(DeltaE.Cam02, ColourSpace.Cam02)]
    [TestCase(DeltaE.Cam16, ColourSpace.Cam16)]
    public void AssertNotNumberDeltas(DeltaE deltaE, ColourSpace colourSpace)
    {
        var unicolour = new Unicolour(colourSpace, double.NaN, double.NaN, double.NaN);
        var delta = unicolour.Difference(deltaE, unicolour);
        Assert.That(delta, Is.NaN);
    }
}