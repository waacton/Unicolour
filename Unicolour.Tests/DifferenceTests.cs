using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class DifferenceTests
{
    private const double Tolerance = 0.00005;
    
    // ReSharper disable CollectionNeverQueried.Local - used in test case sources by name
    private static readonly List<(Unicolour reference, Unicolour sample)> ReferenceSamplePairs = [];
    // ReSharper restore CollectionNeverQueried.Local
    
    static DifferenceTests()
    {
        for (var i = 0; i < 100; i++)
        {
            var reference = RandomColours.UnicolourFrom(ColourSpace.Rgb);
            var sample = RandomColours.UnicolourFrom(ColourSpace.Rgb);
            ReferenceSamplePairs.Add((reference, sample));
        }
    }

    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 100.000000)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 170.565257)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 258.682686)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 176.314083)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 100.000000)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 170.565257)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 258.682686)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 176.314083)]
    public void Cie76(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Cie76);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }

    // ΔE94 is not symmetric
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 100.000000)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 73.430410)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 105.90500)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 61.242091)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 100.000000)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 68.800069)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 100.577051)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 70.580743)]
    public void Cie94Graphics(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Cie94);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(0.0005));
    }

    // ΔE94 is not symmetric
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 50.000000)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 69.731867)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 98.259347)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 61.104684)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 50.000000)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 64.530477)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 92.093048)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 71.003011)]
    public void Cie94Textiles(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Cie94Textiles);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }

    // apparently not symmetric, according to http://www.brucelindbloom.com/ColorDifferenceCalcHelp.html
    // but I've not come across an asymmetric pair after testing millions of random colours
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 100.000000)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 86.608245)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 83.185881)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 52.881375)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 100.000000)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 86.608245)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 83.185881)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 52.881375)]
    public void Ciede2000(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Ciede2000);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    // Cmc is not symmetric
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 195.694716)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 108.449110)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 100.565250)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 76.450069)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 67.480171)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 65.837219)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 124.001222)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 109.761942)]
    public void CmcAcceptability(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.CmcAcceptability);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    // Cmc is not symmetric
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 97.847358)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 105.146203)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 94.630593)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 73.359104)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 33.740085)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 62.338288)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 110.145001)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 108.570712)]
    public void CmcPerceptibility(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.CmcPerceptibility);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    // assumes Ictcp scalar of 100
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 365.816926)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 239.982435)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 234.838743)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 322.659678)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 365.816926)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 239.982435)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 234.838743)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 322.659678)]
    public void Itp(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Itp);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    // assumes Jzczhz scalar of 100
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 0.167174)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 0.195524)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 0.271571)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 0.281457)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 0.167174)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 0.195524)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 0.271571)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 0.281457)]
    public void Z(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Z);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }

    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 100.000000)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 201.534889)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 308.110214)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 196.009473)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 100.000000)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 201.534889)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 308.110214)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 196.009473)]
    public void Hyab(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Hyab);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 1.000000)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 0.519797)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 0.673409)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 0.537117)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 1.000000)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 0.519797)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 0.673409)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 0.537117)]
    public void Ok(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Ok);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 100.024844)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 76.105436)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 92.321874)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 84.119853)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 100.024844)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 76.105436)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 92.321874)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 84.119853)]
    public void Cam02(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Cam02);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }

    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 25.661622)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 22.523082)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 24.596320)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 20.689226)]
    [TestCase(nameof(StandardRgb.White), nameof(StandardRgb.Black), 25.661622)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Red), 22.523082)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Green), 24.596320)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Blue), 20.689226)]
    public void Cam16(string referenceName, string sampleName, double expectedDelta)
    {
        var reference = StandardRgb.Lookup[referenceName];
        var sample = StandardRgb.Lookup[sampleName];
        var delta = reference.Difference(sample, DeltaE.Cam16);
        Assert.That(delta, Is.EqualTo(expectedDelta).Within(Tolerance));
    }
    
    [Test, Combinatorial]
    public static void RandomSymmetric(
        [Values(DeltaE.Cie76, DeltaE.Ciede2000, DeltaE.Itp, DeltaE.Z, DeltaE.Hyab, DeltaE.Ok, DeltaE.Cam02, DeltaE.Cam16)] DeltaE deltaE, 
        [ValueSource(nameof(ReferenceSamplePairs))] (Unicolour reference, Unicolour sample) pair)
    {
        var reference = pair.reference;
        var sample = pair.sample;
        Assert.That(reference.Difference(sample, deltaE), Is.EqualTo(sample.Difference(reference, deltaE)));
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
        var delta = unicolour.Difference(unicolour, deltaE);
        Assert.That(delta, Is.NaN);
    }
}