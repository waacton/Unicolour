namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

/*
 * this base class handles all tests specific to
 * colour spaces where there is no hue component
 */

[TestFixtureSource(nameof(TestData))]
public class MixHueNoComponentTests : MixHueAgnosticTests
{
    private static readonly List<TestFixtureData> TestData = new()
    {
        new(ColourSpace.Rgb, new Range(0, 1), new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.RgbLinear, new Range(0, 1), new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Xyz, new Range(0, 1), new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Xyy, new Range(0, 1), new Range(0, 1), new Range(0, 1)),
        new(ColourSpace.Lab, new Range(0, 100), new Range(-128, 128), new Range(-128, 128)),
        new(ColourSpace.Luv, new Range(0, 100), new Range(-100, 100), new Range(-100, 100)),
        new(ColourSpace.Ictcp, new Range(0, 1), new Range(-0.5, 0.5), new Range(-0.5, 0.5)),
        new(ColourSpace.Jzazbz, new Range(0, 0.16), new Range(-0.1, 0.1), new Range(-0.1, 0.1)),
        new(ColourSpace.Oklab, new Range(0, 1), new Range(-0.5, 0.5), new Range(-0.5, 0.5)),
        new(ColourSpace.Cam02, new Range(0, 100), new Range(-50, 50), new Range(-50, 50)),
        new(ColourSpace.Cam16, new Range(0, 100), new Range(-50, 50), new Range(-50, 50))
    };

    public MixHueNoComponentTests(ColourSpace colourSpace, Range first, Range second, Range third) 
        : base(colourSpace, first, second, third)
    {
    }

    public static readonly List<TestCaseData> PremultipliedAlphaTestData = new()
    {
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 0.00, new AlphaTriplet(new(0.500, 1.000, 0.500), 0.250)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 0.25, new AlphaTriplet(new(0.750, 0.750, 0.750), 0.375)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 0.50, new AlphaTriplet(new(0.875, 0.625, 0.875), 0.500)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 0.75, new AlphaTriplet(new(0.950, 0.550, 0.950), 0.625)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 1.00, new AlphaTriplet(new(1.000, 0.500, 1.000), 0.750))
    };
    
    [TestCaseSource(nameof(PremultipliedAlphaTestData))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var unicolour1 = new Unicolour(ColourSpace, start.Triplet.Tuple, start.Alpha);
        var unicolour2 = new Unicolour(ColourSpace, end.Triplet.Tuple, end.Alpha);
        var mixed = unicolour1.Mix(unicolour2, ColourSpace, amount, premultiplyAlpha: true);
        AssertMix(mixed, expected.Tuple);
    }
}