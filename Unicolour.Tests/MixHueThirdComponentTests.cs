namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

/*
 * this base class handles all tests specific to
 * colour spaces where the hue component is at index 2
 */

[TestFixtureSource(nameof(TestData))]
public class MixHueThirdComponentTests : MixHueAgnosticTests
{
    private static readonly List<TestFixtureData> TestData = new()
    {
        new(ColourSpace.Lchab, new Range(0, 100), new Range(0, 100)),
        new(ColourSpace.Lchuv, new Range(0, 100), new Range(0, 100)),
        new(ColourSpace.Jzczhz, new Range(0, 0.16), new Range(0, 0.16)),
        new(ColourSpace.Oklch, new Range(0, 1), new Range(0, 0.5))
    };

    public MixHueThirdComponentTests(ColourSpace colourSpace, Range first, Range second) 
        : base(colourSpace, first, second, new Range(0, 360))
    {
    }

    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.0), Second.At(0.0), 0, 0.0);
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(1.0), 340, 0.2);
        var mixed1 = unicolour1.Mix(ColourSpace, unicolour2, 0.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace, unicolour1, 0.5, false);
        
        AssertMix(mixed1, (First.At(0.25), Second.At(0.5), 350, 0.1));
        AssertMix(mixed2, (First.At(0.25), Second.At(0.5), 350, 0.1));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(1.0), 300);
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.0), Second.At(0.0), 60, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace, unicolour2, 0.75, false);
        var mixed2 = unicolour2.Mix(ColourSpace, unicolour1, 0.75, false);

        AssertMix(mixed1, (First.At(0.125), Second.At(0.25), 30, 0.625));
        AssertMix(mixed2, (First.At(0.375), Second.At(0.75), 330, 0.875));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(1.0), 300);
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.0), Second.At(0.0), 60, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace, unicolour2, 0.25, false);
        var mixed2 = unicolour2.Mix(ColourSpace, unicolour1, 0.25, false);
        
        AssertMix(mixed1, (First.At(0.375), Second.At(0.75), 330, 0.875));
        AssertMix(mixed2, (First.At(0.125), Second.At(0.25), 30, 0.625));
    }
    
    public static readonly List<TestCaseData> PremultipliedAlphaTestData = new()
    {
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 0.00, new AlphaTriplet(new(0.500, 1.000, 90), 0.250)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 0.25, new AlphaTriplet(new(0.750, 0.750, 135), 0.375)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 0.50, new AlphaTriplet(new(0.875, 0.625, 180), 0.500)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 0.75, new AlphaTriplet(new(0.950, 0.550, 225), 0.625)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 1.00, new AlphaTriplet(new(1.000, 0.500, 270), 0.750))
    };
    
    [TestCaseSource(nameof(PremultipliedAlphaTestData))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var unicolour1 = new Unicolour(ColourSpace, start.Triplet.Tuple, start.Alpha);
        var unicolour2 = new Unicolour(ColourSpace, end.Triplet.Tuple, end.Alpha);
        var mixed = unicolour1.Mix(ColourSpace, unicolour2, amount, premultiplyAlpha: true);
        AssertMix(mixed, expected.Tuple);
    }
}