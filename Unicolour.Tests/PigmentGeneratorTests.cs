using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class PigmentGeneratorTests
{
    // ReSharper disable CollectionNeverQueried.Local - used in test case sources by name
    private static readonly List<TestCaseData> TestData = [];
    // ReSharper restore CollectionNeverQueried.Local

    static PigmentGeneratorTests()
    {
        var randomColours = RandomColours.RgbTriplets.Take(100).Select(x => new Unicolour(ColourSpace.Rgb, x.Tuple)).ToArray();
        TestData.Add(new(StandardRgb.Black));
        TestData.Add(new(StandardRgb.White));
        TestData.Add(new(StandardRgb.Grey));
        TestData.Add(new(StandardRgb.Red));
        TestData.Add(new(StandardRgb.Green));
        TestData.Add(new(StandardRgb.Blue));
        TestData.Add(new(StandardRgb.Cyan));
        TestData.Add(new(StandardRgb.Magenta));
        TestData.Add(new(StandardRgb.Yellow));
        TestData.AddRange(randomColours.Select(x => new TestCaseData(x))); 
    }
    
    [TestCaseSource(nameof(TestData))]
    public void Roundtrip(Unicolour original)
    {
        var pigment = PigmentGenerator.From(original);
        var roundtrip = new Unicolour([pigment], [1.0]);
        TestUtils.AssertTriplet(roundtrip.Rgb.Triplet, original.Rgb.Triplet, 0.05);
    }

    [Test]
    public void NegativeRgb()
    {
        var pigment = PigmentGenerator.From(new(ColourSpace.RgbLinear, -0.5, -0.5, -0.5));
        Assert.That(pigment.R!.Coefficients, Is.All.NaN);
        
        var colour = new Unicolour([pigment], [1.0]);
        TestUtils.AssertTriplet<Xyz>(colour, new(double.NaN, double.NaN, double.NaN), 0);
    }

    [Test]
    public void TooManyIterations()
    {
        // initial values <= algorithm's tolerance seem to take longer to converge
        var pigment = PigmentGenerator.From(new(ColourSpace.Xyz, PigmentGenerator.Tolerance, PigmentGenerator.Tolerance, PigmentGenerator.Tolerance));
        Assert.That(pigment.R!.Coefficients, Is.All.NaN);
        
        var colour = new Unicolour([pigment], [1.0]);
        TestUtils.AssertTriplet<Xyz>(colour, new(double.NaN, double.NaN, double.NaN), 0);
    }
}