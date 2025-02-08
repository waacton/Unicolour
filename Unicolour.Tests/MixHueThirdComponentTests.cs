using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * this base class handles all tests specific to
 * colour spaces where the hue component is at index 2
 */

[TestFixtureSource(nameof(TestData))]
public class MixHueThirdComponentTests : MixHueAgnosticTests
{
    private static readonly List<TestFixtureData> TestData =
    [
        new(ColourSpace.Lchab, new Range(0, 100), new Range(0, 100)),
        new(ColourSpace.Lchuv, new Range(0, 100), new Range(0, 100)),
        new(ColourSpace.Jzczhz, new Range(0, 0.16), new Range(0, 0.16)),
        new(ColourSpace.Oklch, new Range(0, 1), new Range(0, 0.5))
    ];

    public MixHueThirdComponentTests(ColourSpace colourSpace, Range first, Range second) 
        : base(colourSpace, first, second, new Range(0, 360))
    {
    }

    [Test]
    public void EquidistantViaZero()
    {
        var colour1 = new Unicolour(ColourSpace, First.At(0.0), Second.At(0.0), 0, 0.0);
        var colour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(1.0), 340, 0.2);
        var mixed1 = colour1.Mix(colour2, ColourSpace, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.25), Second.At(0.5), 350, 0.1));
        AssertMix(mixed2, (First.At(0.25), Second.At(0.5), 350, 0.1));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var colour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(1.0), 300);
        var colour2 = new Unicolour(ColourSpace, First.At(0.0), Second.At(0.0), 60, 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.75, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.75, premultiplyAlpha: false);

        AssertMix(mixed1, (First.At(0.125), Second.At(0.25), 30, 0.625));
        AssertMix(mixed2, (First.At(0.375), Second.At(0.75), 330, 0.875));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var colour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(1.0), 300);
        var colour2 = new Unicolour(ColourSpace, First.At(0.0), Second.At(0.0), 60, 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.25, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.25, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.375), Second.At(0.75), 330, 0.875));
        AssertMix(mixed2, (First.At(0.125), Second.At(0.25), 30, 0.625));
    }
    
    [TestCase(HueSpan.Shorter, 0)]
    [TestCase(HueSpan.Longer, 180)]
    [TestCase(HueSpan.Increasing, 0)]
    [TestCase(HueSpan.Decreasing, 0)]
    public void Span0(HueSpan hueSpan, double expected)
    {
        var colour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.5), 0, 0.5);
        var colour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.5), 0, 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.5), Second.At(0.5), expected, 0.5));
        AssertMix(mixed2, (First.At(0.5), Second.At(0.5), expected, 0.5));
    }
    
    [TestCase(HueSpan.Shorter, 180, 180)]
    [TestCase(HueSpan.Longer, 0, 0)]
    [TestCase(HueSpan.Increasing, 180, 0)]
    [TestCase(HueSpan.Decreasing, 0, 180)]
    public void Span120(HueSpan hueSpan, double expectedForward, double expectedBackward)
    {
        var colour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.5), 120, 0.5);
        var colour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.5), 240, 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.5), Second.At(0.5), expectedForward, 0.5));
        AssertMix(mixed2, (First.At(0.5), Second.At(0.5), expectedBackward, 0.5));
    }
    
    [TestCase(HueSpan.Shorter, 0, 0)]
    [TestCase(HueSpan.Longer, 180, 180)]
    [TestCase(HueSpan.Increasing, 180, 0)]
    [TestCase(HueSpan.Decreasing, 0, 180)]
    public void Span360(HueSpan hueSpan, double expectedForward, double expectedBackward)
    {
        var colour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.5), 0, 0.5);
        var colour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.5), 360, 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.5), Second.At(0.5), expectedForward, 0.5));
        AssertMix(mixed2, (First.At(0.5), Second.At(0.5), expectedBackward, 0.5));
    }
    
    [TestCase(HueSpan.Shorter, 135, 135)]
    [TestCase(HueSpan.Longer, 315, 315)]
    [TestCase(HueSpan.Increasing, 135, 315)]
    [TestCase(HueSpan.Decreasing, 315, 135)]
    public void SpanUneven(HueSpan hueSpan, double expectedForward, double expectedBackward)
    {
        var colour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.5), 90, 0.5);
        var colour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.5), 180, 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.5), Second.At(0.5), expectedForward, 0.5));
        AssertMix(mixed2, (First.At(0.5), Second.At(0.5), expectedBackward, 0.5));
    }
    
    public static readonly List<TestCaseData> PremultipliedAlphaTestData =
    [
        new(new AlphaTriplet(new(0.5, 1.0, 90, 2), 0.25), new AlphaTriplet(new(1.0, 0.5, 270, 2), 0.75), 0.00, new AlphaTriplet(new(0.500, 1.000, 90, 2), 0.250)),
        new(new AlphaTriplet(new(0.5, 1.0, 90, 2), 0.25), new AlphaTriplet(new(1.0, 0.5, 270, 2), 0.75), 0.25, new AlphaTriplet(new(0.750, 0.750, 135, 2), 0.375)),
        new(new AlphaTriplet(new(0.5, 1.0, 90, 2), 0.25), new AlphaTriplet(new(1.0, 0.5, 270, 2), 0.75), 0.50, new AlphaTriplet(new(0.875, 0.625, 180, 2), 0.500)),
        new(new AlphaTriplet(new(0.5, 1.0, 90, 2), 0.25), new AlphaTriplet(new(1.0, 0.5, 270, 2), 0.75), 0.75, new AlphaTriplet(new(0.950, 0.550, 225, 2), 0.625)),
        new(new AlphaTriplet(new(0.5, 1.0, 90, 2), 0.25), new AlphaTriplet(new(1.0, 0.5, 270, 2), 0.75), 1.00, new AlphaTriplet(new(1.000, 0.500, 270, 2), 0.750))
    ];
    
    [TestCaseSource(nameof(PremultipliedAlphaTestData))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var colour1 = new Unicolour(ColourSpace, start.Triplet.Tuple, start.Alpha);
        var colour2 = new Unicolour(ColourSpace, end.Triplet.Tuple, end.Alpha);
        var mixed = colour1.Mix(colour2, ColourSpace, amount, premultiplyAlpha: true);
        AssertMix(mixed, expected.Tuple);
    }
}