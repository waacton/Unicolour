using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using Range = Wacton.Unicolour.Tests.Utils.Range;

namespace Wacton.Unicolour.Tests;

using Range = Range;

/*
 * this base class handles all tests specific to
 * colour spaces where the hue component is at index 0
 */

[TestFixtureSource(nameof(TestData))]
public class MixHueFirstComponentTests : MixHueAgnosticTests
{
    private static readonly List<TestFixtureData> TestData =
    [
        new(ColourSpace.Hsb, new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Hsl, new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Hwb, new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Hsi, new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Wxy, new Range(360, 700), new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Hsluv, new Range(0, 100), new Range(0, 100)),
        new(ColourSpace.Hpluv, new Range(0, 100), new Range(0, 100)),
        new(ColourSpace.Tsl, new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Okhsv, new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Okhsl, new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Okhwb, new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Hct, new Range(0, 120), new Range(0, 100))
    ];

    private readonly Func<double, double> mapFromDegree;
    private readonly Func<double, double> mapToDegree;
    
    public MixHueFirstComponentTests(ColourSpace colourSpace, Range second, Range third) 
        : this(colourSpace, new Range(0, 360), second, third)
    {
    }
    
    public MixHueFirstComponentTests(ColourSpace colourSpace, Range first, Range second, Range third) 
        : base(colourSpace, first, second, third)
    {
        mapFromDegree = colourSpace == ColourSpace.Wxy
            ? degree => Wxy.DegreeToWavelength(degree, XyzConfiguration.D65)
            : degree => degree;
        
        mapToDegree = colourSpace == ColourSpace.Wxy
            ? degree => Wxy.WavelengthToDegree(degree, XyzConfiguration.D65)
            : degree => degree;
    }

    [Test]
    public void EquidistantViaZero()
    {
        var colour1 = new Unicolour(ColourSpace, mapFromDegree(0), Second.At(0.0), Third.At(0.0), 0.0);
        var colour2 = new Unicolour(ColourSpace, mapFromDegree(340), Second.At(1.0), Third.At(0.5), 0.2);
        var mixed1 = colour1.Mix(colour2, ColourSpace, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, premultiplyAlpha: false);
        
        AssertMix(mixed1, (350, Second.At(0.5), Third.At(0.25), 0.1));
        AssertMix(mixed2, (350, Second.At(0.5), Third.At(0.25), 0.1));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var colour1 = new Unicolour(ColourSpace, mapFromDegree(300), Second.At(1.0), Third.At(0.5));
        var colour2 = new Unicolour(ColourSpace, mapFromDegree(60), Second.At(0.0), Third.At(0.0), 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.75, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.75, premultiplyAlpha: false);

        AssertMix(mixed1, (30, Second.At(0.25), Third.At(0.125), 0.625));
        AssertMix(mixed2, (330, Second.At(0.75), Third.At(0.375), 0.875));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var colour1 = new Unicolour(ColourSpace, mapFromDegree(300), Second.At(1.0), Third.At(0.5));
        var colour2 = new Unicolour(ColourSpace, mapFromDegree(60), Second.At(0.0), Third.At(0.0), 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.25, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.25, premultiplyAlpha: false);
        
        AssertMix(mixed1, (330, Second.At(0.75), Third.At(0.375), 0.875));
        AssertMix(mixed2, (30, Second.At(0.25), Third.At(0.125), 0.625));
    }
    
    [TestCase(HueSpan.Shorter, 0)]
    [TestCase(HueSpan.Longer, 180)]
    [TestCase(HueSpan.Increasing, 0)]
    [TestCase(HueSpan.Decreasing, 0)]
    public void Span0(HueSpan hueSpan, double expected)
    {
        var colour1 = new Unicolour(ColourSpace, mapFromDegree(0), Second.At(0.5), Third.At(0.5), 0.5);
        var colour2 = new Unicolour(ColourSpace, mapFromDegree(0), Second.At(0.5), Third.At(0.5), 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        
        AssertMix(mixed1, (expected, Second.At(0.5), Third.At(0.5), 0.5));
        AssertMix(mixed2, (expected, Second.At(0.5), Third.At(0.5), 0.5));
    }
    
    [TestCase(HueSpan.Shorter, 180, 180)]
    [TestCase(HueSpan.Longer, 0, 0)]
    [TestCase(HueSpan.Increasing, 180, 0)]
    [TestCase(HueSpan.Decreasing, 0, 180)]
    public void Span120(HueSpan hueSpan, double expectedForward, double expectedBackward)
    {
        var colour1 = new Unicolour(ColourSpace, mapFromDegree(120), Second.At(0.5), Third.At(0.5), 0.5);
        var colour2 = new Unicolour(ColourSpace, mapFromDegree(240), Second.At(0.5), Third.At(0.5), 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        
        AssertMix(mixed1, (expectedForward, Second.At(0.5), Third.At(0.5), 0.5));
        AssertMix(mixed2, (expectedBackward, Second.At(0.5), Third.At(0.5), 0.5));
    }
    
    [TestCase(HueSpan.Shorter, 0, 0)]
    [TestCase(HueSpan.Longer, 180, 180)]
    [TestCase(HueSpan.Increasing, 180, 0)]
    [TestCase(HueSpan.Decreasing, 0, 180)]
    public void Span360(HueSpan hueSpan, double expectedForward, double expectedBackward)
    {
        var colour1 = new Unicolour(ColourSpace, mapFromDegree(0), Second.At(0.5), Third.At(0.5), 0.5);
        var colour2 = new Unicolour(ColourSpace, mapFromDegree(360), Second.At(0.5), Third.At(0.5), 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        
        AssertMix(mixed1, (expectedForward, Second.At(0.5), Third.At(0.5), 0.5));
        AssertMix(mixed2, (expectedBackward, Second.At(0.5), Third.At(0.5), 0.5));
    }
    
    [TestCase(HueSpan.Shorter, 135, 135)]
    [TestCase(HueSpan.Longer, 315, 315)]
    [TestCase(HueSpan.Increasing, 135, 315)]
    [TestCase(HueSpan.Decreasing, 315, 135)]
    public void SpanUneven(HueSpan hueSpan, double expectedForward, double expectedBackward)
    {
        var colour1 = new Unicolour(ColourSpace, mapFromDegree(90), Second.At(0.5), Third.At(0.5), 0.5);
        var colour2 = new Unicolour(ColourSpace, mapFromDegree(180), Second.At(0.5), Third.At(0.5), 0.5);
        var mixed1 = colour1.Mix(colour2, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        var mixed2 = colour2.Mix(colour1, ColourSpace, 0.5, hueSpan, premultiplyAlpha: false);
        
        AssertMix(mixed1, (expectedForward, Second.At(0.5), Third.At(0.5), 0.5));
        AssertMix(mixed2, (expectedBackward, Second.At(0.5), Third.At(0.5), 0.5));
    }
    
    public static readonly List<TestCaseData> PremultipliedAlphaTestData =
    [
        new(new AlphaTriplet(new(90, 1.0, 0.5, 0), 0.25), new AlphaTriplet(new(270, 0.5, 1.0, 0), 0.75), 0.00, new AlphaTriplet(new(90, 1.000, 0.500, 0), 0.250)),
        new(new AlphaTriplet(new(90, 1.0, 0.5, 0), 0.25), new AlphaTriplet(new(270, 0.5, 1.0, 0), 0.75), 0.25, new AlphaTriplet(new(135, 0.750, 0.750, 0), 0.375)),
        new(new AlphaTriplet(new(90, 1.0, 0.5, 0), 0.25), new AlphaTriplet(new(270, 0.5, 1.0, 0), 0.75), 0.50, new AlphaTriplet(new(180, 0.625, 0.875, 0), 0.500)),
        new(new AlphaTriplet(new(90, 1.0, 0.5, 0), 0.25), new AlphaTriplet(new(270, 0.5, 1.0, 0), 0.75), 0.75, new AlphaTriplet(new(225, 0.550, 0.950, 0), 0.625)),
        new(new AlphaTriplet(new(90, 1.0, 0.5, 0), 0.25), new AlphaTriplet(new(270, 0.5, 1.0, 0), 0.75), 1.00, new AlphaTriplet(new(270, 0.500, 1.000, 0), 0.750))
    ];
    
    [TestCaseSource(nameof(PremultipliedAlphaTestData))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        start = start with { Triplet = start.Triplet.WithDegreeMap(mapFromDegree) };
        end = end with { Triplet = end.Triplet.WithDegreeMap(mapFromDegree) };
        
        var colour1 = new Unicolour(ColourSpace, start.Triplet.Tuple, start.Alpha);
        var colour2 = new Unicolour(ColourSpace, end.Triplet.Tuple, end.Alpha);
        var mixed = colour1.Mix(colour2, ColourSpace, amount, HueSpan.Increasing, premultiplyAlpha: true);
        AssertMix(mixed, expected.Tuple);
    }
    
    // this extra step is needed to handle the unusual WXY "hue"
    private new void AssertMix(Unicolour colour, (double first, double second, double third, double alpha) expected)
    {
        var tripletWithDegree = colour.GetRepresentation(ColourSpace).Triplet.WithDegreeMap(mapToDegree).WithHueModulo();
        TestUtils.AssertMixed(tripletWithDegree, colour.Alpha.A, expected);
    }
}