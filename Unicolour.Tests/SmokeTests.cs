namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class SmokeTests
{
    private static readonly List<TestCaseData> ColourSpaceTestCases = new()
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
        new TestCaseData(ColourSpace.Hsi, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Hsi, 360, 1, 1, 1),
        new TestCaseData(ColourSpace.Hsi, 180, 0.4, 0.6, 0.5),
        new TestCaseData(ColourSpace.Xyz, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Xyz, 1, 1, 1, 1),
        new TestCaseData(ColourSpace.Xyz, 0.4, 0.5, 0.6, 0.5),
        new TestCaseData(ColourSpace.Xyy, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Xyy, 1, 1, 1, 1),
        new TestCaseData(ColourSpace.Xyy, 0.4, 0.5, 0.6, 0.5),
        new TestCaseData(ColourSpace.Wxy, 360, 0, 0, 0),
        new TestCaseData(ColourSpace.Wxy, 700, 1, 1, 1),
        new TestCaseData(ColourSpace.Wxy, -530, 0.5, 0.5, 0.5),
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
        new TestCaseData(ColourSpace.Ypbpr, 0, -0.5, -0.5, 0),
        new TestCaseData(ColourSpace.Ypbpr, 1, 0.5, 0.5, 1),
        new TestCaseData(ColourSpace.Ypbpr, 0.5, -0.01, 0.01, 0.5),
        new TestCaseData(ColourSpace.Ycbcr, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Ycbcr, 255, 255, 255, 1),
        new TestCaseData(ColourSpace.Ycbcr, 127, 128, 129, 0.5),
        new TestCaseData(ColourSpace.Ycgco, 0, -0.5, -0.5, 0),
        new TestCaseData(ColourSpace.Ycgco, 1, 0.5, 0.5, 1),
        new TestCaseData(ColourSpace.Ycgco, 0.5, -0.01, 0.01, 0.5),
        new TestCaseData(ColourSpace.Yuv, 0, -0.436, -0.614, 0),
        new TestCaseData(ColourSpace.Yuv, 1, 0.436, 0.614, 1),
        new TestCaseData(ColourSpace.Yuv, 0.5, -0.01, 0.01, 0.5),
        new TestCaseData(ColourSpace.Yiq, 0, -0.595, -0.522, 0),
        new TestCaseData(ColourSpace.Yiq, 1, 0.595, 0.522, 1),
        new TestCaseData(ColourSpace.Yiq, 0.5, -0.01, 0.01, 0.5),
        new TestCaseData(ColourSpace.Ydbdr, 0, -1.333, -1.333, 0),
        new TestCaseData(ColourSpace.Ydbdr, 1, 1.333, 1.333, 1),
        new TestCaseData(ColourSpace.Ydbdr, 0.5, -0.01, 0.01, 0.5),
        new TestCaseData(ColourSpace.Tsl, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Tsl, 360, 1, 1, 1),
        new TestCaseData(ColourSpace.Tsl, 180, 0.4, 0.6, 0.5),
        new TestCaseData(ColourSpace.Xyb, -0.03, 0, -0.4, 0),
        new TestCaseData(ColourSpace.Xyb, 0.03, 1, 0.4, 1),
        new TestCaseData(ColourSpace.Xyb, 0, 0.5, 0, 0.5),
        new TestCaseData(ColourSpace.Ipt, 0, -0.75, -0.75, 0),
        new TestCaseData(ColourSpace.Ipt, 1, 0.75, 0.75, 1),
        new TestCaseData(ColourSpace.Ipt, 0.5, -0.01, 0.01, 0.5),
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
        new TestCaseData(ColourSpace.Okhsv, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Okhsv, 360, 1, 1, 1),
        new TestCaseData(ColourSpace.Okhsv, 180, 0.4, 0.6, 0.5),
        new TestCaseData(ColourSpace.Okhsl, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Okhsl, 360, 1, 1, 1),
        new TestCaseData(ColourSpace.Okhsl, 180, 0.4, 0.6, 0.5),
        new TestCaseData(ColourSpace.Okhwb, 0, 0, 0, 0),
        new TestCaseData(ColourSpace.Okhwb, 360, 1, 1, 1),
        new TestCaseData(ColourSpace.Okhwb, 180, 0.4, 0.6, 0.5),
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
    
    [TestCaseSource(nameof(ColourSpaceTestCases))]
    public void UnicolourNoAlpha(ColourSpace colourSpace, double first, double second, double third, double alpha)
    {
        var tuple = (first, second, third);
        var expected = new Unicolour(colourSpace, first, second, third);
        AssertNoError(expected, new Unicolour(colourSpace, tuple));
        AssertNoError(expected, new Unicolour(colourSpace, first, second, third));
        AssertNoError(expected, new Unicolour(Configuration.Default, colourSpace, tuple));
        AssertNoError(expected, new Unicolour(Configuration.Default, colourSpace, first, second, third));
    }
    
    [TestCaseSource(nameof(ColourSpaceTestCases))]
    public void UnicolourWithAlpha(ColourSpace colourSpace, double first, double second, double third, double alpha)
    {
        var tuple = (first, second, third);
        var alphaTuple = (first, second, third, alpha);
        var expected = new Unicolour(colourSpace, first, second, third, alpha);
        AssertNoError(expected, new Unicolour(colourSpace, tuple, alpha));
        AssertNoError(expected, new Unicolour(colourSpace, alphaTuple));
        AssertNoError(expected, new Unicolour(colourSpace, first, second, third, alpha));
        AssertNoError(expected, new Unicolour(Configuration.Default, colourSpace, tuple, alpha));
        AssertNoError(expected, new Unicolour(Configuration.Default, colourSpace, alphaTuple));
        AssertNoError(expected, new Unicolour(Configuration.Default, colourSpace, first, second, third, alpha));
    }
    
    private static readonly List<string> HexDigit6Values = new() { "000000", "FFFFFF", "798081", "#000000", "#FFFFFF", "#798081" };
    private static readonly List<string> HexDigit8Values = new() { "00000000", "FFFFFFFF", "79808180", "#00000000", "#FFFFFFFF", "#79808180" };
    private static readonly List<double> AlphaOverrideValues = new() { 0.0, 0.5, 1.0 };
    
    [Test]
    public void HexDigit6(
        [ValueSource(nameof(HexDigit6Values))] string hex)
    {
        var expected = new Unicolour(hex);
        AssertNoError(expected, new Unicolour(hex));
        AssertNoError(expected, new Unicolour(Configuration.Default, hex));
    }
    
    [Test]
    public void HexDigit8(
        [ValueSource(nameof(HexDigit8Values))] string hex)
    {
        var expected = new Unicolour(hex);
        AssertNoError(expected, new Unicolour(hex));
        AssertNoError(expected, new Unicolour(Configuration.Default, hex));
    }
    
    [Test]
    public void HexDigit6WithAlphaOverride(
        [ValueSource(nameof(HexDigit6Values))] string hex,
        [ValueSource(nameof(AlphaOverrideValues))] double alphaOverride)
    {
        var expected = new Unicolour(hex, alphaOverride);
        AssertNoError(expected, new Unicolour(hex, alphaOverride));
        AssertNoError(expected, new Unicolour(Configuration.Default, hex, alphaOverride));
    }
    
    [Test]
    public void HexDigit8WithAlphaOverride(
        [ValueSource(nameof(HexDigit8Values))] string hex,
        [ValueSource(nameof(AlphaOverrideValues))] double alphaOverride)
    {
        var expected = new Unicolour(hex, alphaOverride);
        AssertNoError(expected, new Unicolour(hex, alphaOverride));
        AssertNoError(expected, new Unicolour(Configuration.Default, hex, alphaOverride));
    }
    
    private static readonly List<double> ChromaticityValues = new() { 0.0, 0.25, 0.4, 0.5, 0.6, 0.75, 1.0 };
    private static readonly List<double> LuminanceValues = new() { 1.0, 0.5, 0.0 };
    
    [Test]
    public void Chromaticity(
        [ValueSource(nameof(ChromaticityValues))] double x,
        [ValueSource(nameof(ChromaticityValues))] double y)
    {
        var chromaticity = new Chromaticity(x, y);
        var expected = new Unicolour(chromaticity);
        AssertNoError(expected, new Unicolour(chromaticity));
        AssertNoError(expected, new Unicolour(Configuration.Default, chromaticity));
    }
    
    [Test]
    public void ChromaticityWithLuminance(
        [ValueSource(nameof(ChromaticityValues))] double x,
        [ValueSource(nameof(ChromaticityValues))] double y,
        [ValueSource(nameof(LuminanceValues))] double luminance)
    {
        var chromaticity = new Chromaticity(x, y);
        var expected = new Unicolour(chromaticity, luminance);
        AssertNoError(expected, new Unicolour(chromaticity, luminance));
        AssertNoError(expected, new Unicolour(Configuration.Default, chromaticity, luminance));
    }


    private static readonly List<double> CctValues = new() { 400, 500, 1000, 6504, 20000, 25000, 1e9 };
    private static readonly List<double> DuvValues = new() { -0.05, 0, 0.05 };
    private static readonly List<Locus> LocusValues = new() { Locus.Blackbody, Locus.Daylight };
    
    [Test]
    public void TemperatureOnlyCct(
        [ValueSource(nameof(CctValues))] double cct)
    {
        var expected = new Unicolour(cct);
        AssertNoError(expected, new Unicolour(cct));
        AssertNoError(expected, new Unicolour(Configuration.Default, cct));
    }
    
    [Test, Combinatorial]
    public void TemperatureWithDuv(
        [ValueSource(nameof(CctValues))] double cct, 
        [ValueSource(nameof(DuvValues))] double duv)
    {
        var temperature = new Temperature(cct, duv);
        var expected = new Unicolour(temperature);
        AssertNoError(expected, new Unicolour(temperature));
        AssertNoError(expected, new Unicolour(Configuration.Default, temperature));
    }
    
    [Test, Combinatorial]
    public void TemperatureWithLocus(
        [ValueSource(nameof(CctValues))] double cct,
        [ValueSource(nameof(LocusValues))] Locus locus)
    {
        var expected = new Unicolour(cct, locus);
        AssertNoError(expected, new Unicolour(cct, locus));
        AssertNoError(expected, new Unicolour(Configuration.Default, cct, locus));
    }
    
    [Test, Combinatorial]
    public void TemperatureWithLuminance(
        [ValueSource(nameof(CctValues))] double cct,
        [ValueSource(nameof(LuminanceValues))] double luminance)
    {
        var expected = new Unicolour(cct, luminance: luminance);
        AssertNoError(expected, new Unicolour(cct, luminance: luminance));
        AssertNoError(expected, new Unicolour(cct, Locus.Blackbody, luminance));
        AssertNoError(expected, new Unicolour(Configuration.Default, cct, Locus.Blackbody, luminance));
        AssertNoError(expected, new Unicolour(cct, 0.0, luminance));
        AssertNoError(expected, new Unicolour(Configuration.Default, cct, 0.0, luminance));
    }
    
    [Test, Combinatorial]
    public void TemperatureWithDuvAndLuminance(
        [ValueSource(nameof(CctValues))] double cct,
        [ValueSource(nameof(DuvValues))] double duv,
        [ValueSource(nameof(LuminanceValues))] double luminance)
    {
        var temperature = new Temperature(cct, duv);
        var expected = new Unicolour(temperature, luminance);
        AssertNoError(expected, new Unicolour(temperature, luminance));
        AssertNoError(expected, new Unicolour(Configuration.Default, temperature, luminance));
    }

    [Test, Combinatorial]
    public void TemperatureWithLocusAndLuminance(
        [ValueSource(nameof(CctValues))] double cct,
        [ValueSource(nameof(LocusValues))] Locus locus,
        [ValueSource(nameof(LuminanceValues))] double luminance)
    {
        var expected = new Unicolour(cct, locus, luminance);
        AssertNoError(expected, new Unicolour(cct, locus, luminance));
        AssertNoError(expected, new Unicolour(Configuration.Default, cct, locus, luminance));
    }

    [Test]
    public void SpectralPowerDistribution()
    {
        var expected = new Unicolour(Spd.D65);
        AssertNoError(expected, new Unicolour(Spd.D65));
        AssertNoError(expected, new Unicolour(Configuration.Default, Spd.D65));
    }

    private static void AssertNoError(Unicolour expected, Unicolour unicolour)
    {
        TestUtils.AssertNoPropertyError(unicolour);
        Assert.That(unicolour, Is.EqualTo(expected));
    }
}

