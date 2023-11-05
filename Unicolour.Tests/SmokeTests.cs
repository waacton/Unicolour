namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class SmokeTests
{
    [TestCase("000000")]
    [TestCase("FFFFFF")]
    [TestCase("798081")]
    [TestCase("#000000")]
    [TestCase("#FFFFFF")]
    [TestCase("#798081")]
    public void Hex(string hex)
    {
        var expected = new Unicolour(hex);
        AssertNoError(expected, new Unicolour(hex));
        AssertNoError(expected, new Unicolour(Configuration.Default, hex));
    }
    
    [TestCase("00000000")]
    [TestCase("FFFFFFFF")]
    [TestCase("79808180")]
    [TestCase("#00000000")]
    [TestCase("#FFFFFFFF")]
    [TestCase("#79808180")]
    public void HexAlpha(string hex)
    {
        var expected = new Unicolour(hex);
        AssertNoError(expected, new Unicolour(hex));
        AssertNoError(expected, new Unicolour(Configuration.Default, hex));
    }

    private static readonly List<TestCaseData> TestCases = new()
    {
        new TestCaseData(ColourSpace.Rgb255, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Rgb255, 255, 255, 255, 1),
        new TestCaseData(ColourSpace.Rgb255, 127, 128, 129, 0.5),
        new TestCaseData(ColourSpace.Rgb, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Rgb, 1, 1, 1, 1),
        new TestCaseData(ColourSpace.Rgb, 0.4, 0.5, 0.6, 0.5),
        new TestCaseData(ColourSpace.RgbLinear, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.RgbLinear, 1, 1, 1, 1),
        new TestCaseData(ColourSpace.RgbLinear, 0.4, 0.5, 0.6, 0.5),
        new TestCaseData(ColourSpace.Hsb, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Hsb, 360, 1, 1, 1),
        new TestCaseData(ColourSpace.Hsb, 180, 0.4, 0.6, 0.5),
        new TestCaseData(ColourSpace.Hsl, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Hsl, 360, 1, 1, 1),
        new TestCaseData(ColourSpace.Hsl, 180, 0.4, 0.6, 0.5),
        new TestCaseData(ColourSpace.Hwb, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Hwb, 360, 1, 1, 1),
        new TestCaseData(ColourSpace.Hwb, 180, 0.4, 0.6, 0.5),
        new TestCaseData(ColourSpace.Xyz, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Xyz, 1, 1, 1, 1),
        new TestCaseData(ColourSpace.Xyz, 0.4, 0.5, 0.6, 0.5),
        new TestCaseData(ColourSpace.Xyy, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Xyy, 1, 1, 1, 1),
        new TestCaseData(ColourSpace.Xyy, 0.4, 0.5, 0.6, 0.5),
        new TestCaseData(ColourSpace.Lab, 0, -128, -128, 0),
        new TestCaseData(ColourSpace.Lab, 100, 128, 128, 1),
        new TestCaseData(ColourSpace.Lab, 50, -1, 1, 0.5),
        new TestCaseData(ColourSpace.Lchab, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Lchab, 100, 230, 360, 1),
        new TestCaseData(ColourSpace.Lchab, 50, 115, 180, 0.5),
        new TestCaseData(ColourSpace.Luv, 0, -100, -100, 0),
        new TestCaseData(ColourSpace.Luv, 100, 100, 100, 1),
        new TestCaseData(ColourSpace.Luv, 50, -1, 1, 0.5),
        new TestCaseData(ColourSpace.Lchuv, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Lchuv, 100, 230, 360, 1),
        new TestCaseData(ColourSpace.Lchuv, 50, 115, 180, 0.5),
        new TestCaseData(ColourSpace.Hsluv, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Hsluv, 360, 100, 100, 1),
        new TestCaseData(ColourSpace.Hsluv, 180, 50, 50, 0.5),
        new TestCaseData(ColourSpace.Hpluv, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Hpluv, 360, 100, 100, 1),
        new TestCaseData(ColourSpace.Hpluv, 180, 50, 50, 0.5),
        new TestCaseData(ColourSpace.Ictcp, 0, -0.5, -0.5, 0),
        new TestCaseData(ColourSpace.Ictcp, 1, 0.5, 0.5, 1),
        new TestCaseData(ColourSpace.Ictcp, 0.5, -0.01, 0.01, 0.5),
        new TestCaseData(ColourSpace.Jzazbz, 0, -0.10, -0.16, 0),
        new TestCaseData(ColourSpace.Jzazbz, 0.17, 0.11, 0.12, 1),
        new TestCaseData(ColourSpace.Jzazbz, 0.085, -0.0001, 0.0001, 0.5),
        new TestCaseData(ColourSpace.Jzczhz, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Jzczhz, 0.17, 0.16, 360, 1),
        new TestCaseData(ColourSpace.Jzczhz, 0.085, 0.08, 180, 0.5),
        new TestCaseData(ColourSpace.Oklab, 0, -0.5, -0.5, 0),
        new TestCaseData(ColourSpace.Oklab, 1, 0.5, 0.5, 1),
        new TestCaseData(ColourSpace.Oklab, 0.5, -0.001, 0.001, 0.5),
        new TestCaseData(ColourSpace.Oklch, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Oklch, 1, 0.5, 360, 1),
        new TestCaseData(ColourSpace.Oklch, 0.5, 0.25, 180, 0.5),
        new TestCaseData(ColourSpace.Cam02, 0, -50, -50, 0),
        new TestCaseData(ColourSpace.Cam02, 100, 50, 50, 1),
        new TestCaseData(ColourSpace.Cam02, 50, -1, 1, 0.5),
        new TestCaseData(ColourSpace.Cam16, 0, -50, -50, 0),
        new TestCaseData(ColourSpace.Cam16, 100, 50, 50, 1),
        new TestCaseData(ColourSpace.Cam16, 50, -1, 1, 0.5),
        new TestCaseData(ColourSpace.Hct, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Hct, 360, 120, 100, 1),
        new TestCaseData(ColourSpace.Hct, 180, 60, 50, 0.5)
    };
    
    
    [TestCaseSource(nameof(TestCases))]
    public void Unicolour(ColourSpace colourSpace, double first, double second, double third, double alpha)
    {
        var tuple = (first, second, third);
        var expected = new Unicolour(colourSpace, first, second, third);
        AssertNoError(expected, new Unicolour(colourSpace, tuple));
        AssertNoError(expected, new Unicolour(colourSpace, first, second, third));
        AssertNoError(expected, new Unicolour(colourSpace, Configuration.Default, tuple));
        AssertNoError(expected, new Unicolour(colourSpace, Configuration.Default, first, second, third));
    }
    
    [TestCaseSource(nameof(TestCases))]
    public void UnicolourAlpha(ColourSpace colourSpace, double first, double second, double third, double alpha)
    {
        var tuple = (first, second, third);
        var alphaTuple = (first, second, third, alpha);
        var expected = new Unicolour(colourSpace, first, second, third, alpha);
        AssertNoError(expected, new Unicolour(colourSpace, tuple, alpha));
        AssertNoError(expected, new Unicolour(colourSpace, first, second, third, alpha));
        AssertNoError(expected, new Unicolour(colourSpace, Configuration.Default, tuple, alpha));
        AssertNoError(expected, new Unicolour(colourSpace, Configuration.Default, alphaTuple));
        AssertNoError(expected, new Unicolour(colourSpace, Configuration.Default, first, second, third, alpha));
    }

    private static void AssertNoError(Unicolour expected, Unicolour unicolour)
    {
        TestUtils.AssertNoPropertyError(unicolour);
        Assert.That(unicolour, Is.EqualTo(expected));
    }
}

