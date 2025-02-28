using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class SmokeTests
{
    private static readonly TestCaseData[] ColourSpaceTestCases =
    [
        new(ColourSpace.Rgb255, 0, 0, 0, 0),
        new(ColourSpace.Rgb255, 255, 255, 255, 1),
        new(ColourSpace.Rgb255, 127, 128, 129, 0.5),
        new(ColourSpace.Rgb, 0, 0, 0, 0),
        new(ColourSpace.Rgb, 1, 1, 1, 1),
        new(ColourSpace.Rgb, 0.4, 0.5, 0.6, 0.5),
        new(ColourSpace.RgbLinear, 0, 0, 0, 0),
        new(ColourSpace.RgbLinear, 1, 1, 1, 1),
        new(ColourSpace.RgbLinear, 0.4, 0.5, 0.6, 0.5),
        new(ColourSpace.Hsb, 0, 0, 0, 0),
        new(ColourSpace.Hsb, 360, 1, 1, 1),
        new(ColourSpace.Hsb, 180, 0.4, 0.6, 0.5),
        new(ColourSpace.Hsl, 0, 0, 0, 0),
        new(ColourSpace.Hsl, 360, 1, 1, 1),
        new(ColourSpace.Hsl, 180, 0.4, 0.6, 0.5),
        new(ColourSpace.Hwb, 0, 0, 0, 0),
        new(ColourSpace.Hwb, 360, 1, 1, 1),
        new(ColourSpace.Hwb, 180, 0.4, 0.6, 0.5),
        new(ColourSpace.Hsi, 0, 0, 0, 0),
        new(ColourSpace.Hsi, 360, 1, 1, 1),
        new(ColourSpace.Hsi, 180, 0.4, 0.6, 0.5),
        new(ColourSpace.Xyz, 0, 0, 0, 0),
        new(ColourSpace.Xyz, 1, 1, 1, 1),
        new(ColourSpace.Xyz, 0.4, 0.5, 0.6, 0.5),
        new(ColourSpace.Xyy, 0, 0, 0, 0),
        new(ColourSpace.Xyy, 1, 1, 1, 1),
        new(ColourSpace.Xyy, 0.4, 0.5, 0.6, 0.5),
        new(ColourSpace.Wxy, 360, 0, 0, 0),
        new(ColourSpace.Wxy, 700, 1, 1, 1),
        new(ColourSpace.Wxy, -530, 0.5, 0.5, 0.5),
        new(ColourSpace.Lab, 0, -128, -128, 0),
        new(ColourSpace.Lab, 100, 128, 128, 1),
        new(ColourSpace.Lab, 50, -1, 1, 0.5),
        new(ColourSpace.Lchab, 0, 0, 0, 0),
        new(ColourSpace.Lchab, 100, 230, 360, 1),
        new(ColourSpace.Lchab, 50, 115, 180, 0.5),
        new(ColourSpace.Luv, 0, -100, -100, 0),
        new(ColourSpace.Luv, 100, 100, 100, 1),
        new(ColourSpace.Luv, 50, -1, 1, 0.5),
        new(ColourSpace.Lchuv, 0, 0, 0, 0),
        new(ColourSpace.Lchuv, 100, 230, 360, 1),
        new(ColourSpace.Lchuv, 50, 115, 180, 0.5),
        new(ColourSpace.Hsluv, 0, 0, 0, 0),
        new(ColourSpace.Hsluv, 360, 100, 100, 1),
        new(ColourSpace.Hsluv, 180, 50, 50, 0.5),
        new(ColourSpace.Hpluv, 0, 0, 0, 0),
        new(ColourSpace.Hpluv, 360, 100, 100, 1),
        new(ColourSpace.Hpluv, 180, 50, 50, 0.5),
        new(ColourSpace.Ypbpr, 0, -0.5, -0.5, 0),
        new(ColourSpace.Ypbpr, 1, 0.5, 0.5, 1),
        new(ColourSpace.Ypbpr, 0.5, -0.01, 0.01, 0.5),
        new(ColourSpace.Ycbcr, 0, 0, 0, 0),
        new(ColourSpace.Ycbcr, 255, 255, 255, 1),
        new(ColourSpace.Ycbcr, 127, 128, 129, 0.5),
        new(ColourSpace.Ycgco, 0, -0.5, -0.5, 0),
        new(ColourSpace.Ycgco, 1, 0.5, 0.5, 1),
        new(ColourSpace.Ycgco, 0.5, -0.01, 0.01, 0.5),
        new(ColourSpace.Yuv, 0, -0.436, -0.614, 0),
        new(ColourSpace.Yuv, 1, 0.436, 0.614, 1),
        new(ColourSpace.Yuv, 0.5, -0.01, 0.01, 0.5),
        new(ColourSpace.Yiq, 0, -0.595, -0.522, 0),
        new(ColourSpace.Yiq, 1, 0.595, 0.522, 1),
        new(ColourSpace.Yiq, 0.5, -0.01, 0.01, 0.5),
        new(ColourSpace.Ydbdr, 0, -1.333, -1.333, 0),
        new(ColourSpace.Ydbdr, 1, 1.333, 1.333, 1),
        new(ColourSpace.Ydbdr, 0.5, -0.01, 0.01, 0.5),
        new(ColourSpace.Tsl, 0, 0, 0, 0),
        new(ColourSpace.Tsl, 360, 1, 1, 1),
        new(ColourSpace.Tsl, 180, 0.4, 0.6, 0.5),
        new(ColourSpace.Xyb, -0.03, 0, -0.4, 0),
        new(ColourSpace.Xyb, 0.03, 1, 0.4, 1),
        new(ColourSpace.Xyb, 0, 0.5, 0, 0.5),
        new(ColourSpace.Ipt, 0, -0.75, -0.75, 0),
        new(ColourSpace.Ipt, 1, 0.75, 0.75, 1),
        new(ColourSpace.Ipt, 0.5, -0.01, 0.01, 0.5),
        new(ColourSpace.Ictcp, 0, -0.5, -0.5, 0),
        new(ColourSpace.Ictcp, 1, 0.5, 0.5, 1),
        new(ColourSpace.Ictcp, 0.5, -0.01, 0.01, 0.5),
        new(ColourSpace.Jzazbz, 0, -0.10, -0.16, 0),
        new(ColourSpace.Jzazbz, 0.17, 0.11, 0.12, 1),
        new(ColourSpace.Jzazbz, 0.085, -0.0001, 0.0001, 0.5),
        new(ColourSpace.Jzczhz, 0, 0, 0, 0),
        new(ColourSpace.Jzczhz, 0.17, 0.16, 360, 1),
        new(ColourSpace.Jzczhz, 0.085, 0.08, 180, 0.5),
        new(ColourSpace.Oklab, 0, -0.5, -0.5, 0),
        new(ColourSpace.Oklab, 1, 0.5, 0.5, 1),
        new(ColourSpace.Oklab, 0.5, -0.001, 0.001, 0.5),
        new(ColourSpace.Oklch, 0, 0, 0, 0),
        new(ColourSpace.Oklch, 1, 0.5, 360, 1),
        new(ColourSpace.Oklch, 0.5, 0.25, 180, 0.5),
        new(ColourSpace.Okhsv, 0, 0, 0, 0),
        new(ColourSpace.Okhsv, 360, 1, 1, 1),
        new(ColourSpace.Okhsv, 180, 0.4, 0.6, 0.5),
        new(ColourSpace.Okhsl, 0, 0, 0, 0),
        new(ColourSpace.Okhsl, 360, 1, 1, 1),
        new(ColourSpace.Okhsl, 180, 0.4, 0.6, 0.5),
        new(ColourSpace.Okhwb, 0, 0, 0, 0),
        new(ColourSpace.Okhwb, 360, 1, 1, 1),
        new(ColourSpace.Okhwb, 180, 0.4, 0.6, 0.5),
        new(ColourSpace.Cam02, 0, -50, -50, 0),
        new(ColourSpace.Cam02, 100, 50, 50, 1),
        new(ColourSpace.Cam02, 50, -1, 1, 0.5),
        new(ColourSpace.Cam16, 0, -50, -50, 0),
        new(ColourSpace.Cam16, 100, 50, 50, 1),
        new(ColourSpace.Cam16, 50, -1, 1, 0.5),
        new(ColourSpace.Hct, 0, 0, 0, 0),
        new(ColourSpace.Hct, 360, 120, 100, 1),
        new(ColourSpace.Hct, 180, 60, 50, 0.5)
    ];
    
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
    
    private static readonly List<string> HexDigit6Values = ["000000", "FFFFFF", "798081", "#000000", "#FFFFFF", "#798081"];
    private static readonly List<string> HexDigit8Values = ["00000000", "FFFFFFFF", "79808180", "#00000000", "#FFFFFFFF", "#79808180"];
    private static readonly List<double> AlphaOverrideValues = [0.0, 0.5, 1.0];
    
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
    
    private static readonly List<double> ChromaticityValues = [0.0, 0.25, 0.4, 0.5, 0.6, 0.75, 1.0];
    private static readonly List<double> LuminanceValues = [1.0, 0.5, 0.0];
    
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


    private static readonly List<double> CctValues = [400, 500, 1000, 6504, 20000, 25000, 1e9];
    private static readonly List<double> DuvValues = [-0.05, 0, 0.05];
    private static readonly List<Locus> LocusValues = [Locus.Blackbody, Locus.Daylight];
    
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

    private static readonly Spd MonochromaticSpd = new(start: 580, interval: 1, [1.0]);
    private static readonly Spd TwoNmSpd = new(start: 578, interval: 2, [0.25, 1.0, 0.75]);
    private static readonly Spd FiveNmSpd = new(start: 575, interval: 5, [0.25, 1.0, 0.75]);
    private static readonly Spd NoPowerOneNmSpd = new(start: 580, interval: 1, []);
    private static readonly Spd NoPowerZeroNmSpd = new(start: 580, interval: 0, []);
    private static readonly Spd[] SpdValues = [Spd.D65, MonochromaticSpd, TwoNmSpd, FiveNmSpd, NoPowerOneNmSpd, NoPowerZeroNmSpd];

    [Test]
    public void SpectralPowerDistribution(
        [ValueSource(nameof(SpdValues))] Spd spd)
    {
        var expected = new Unicolour(spd);
        AssertNoError(expected, new Unicolour(spd));
        AssertNoError(expected, new Unicolour(Configuration.Default, spd));
    }
    
    private static readonly Pigment[][] PigmentSingleConstantValues = [
        [new(360, 10, [0.1, 0.2, 0.3], "A1"), new(360, 10, [0.4, 0.5, 0.6], "B1"), new(360, 10, [0.7, 0.8, 0.9], "C1")],
        [new(570, 5, [0.5, 0.5, 0.5], "A2"), new(570, 5, [0.5, 0.5, 0.5], "B2"), new(570, 5, [0.5, 0.5, 0.5], "C2")],
        [new(780, 1, [0.9, 0.9, 0.9], "A3"), new(780, 1, [0.5, 0.5, 0.5], "B3"), new(780, 1, [0.1, 0.1, 0.1], "C3")]
    ];
    
    private static readonly Pigment[][] PigmentTwoConstantValues = [
        [new(360, 10, [0.1, 0.2, 0.3], [0.1, 0.1, 0.1], 0.04, 0.6, "A1"), new(360, 10, [0.4, 0.5, 0.6], [0.5, 0.5, 0.5], 0.04, 0.6, "B1"), new(360, 10, [0.7, 0.8, 0.9], [0.9, 0.9, 0.9], 0.04, 0.6, "C1")],
        [new(570, 5, [0.5, 0.5, 0.5], [1.0, 1.0, 1.0], null, null, "A2"), new(570, 5, [0.5, 0.5, 0.5], [0.5, 0.5, 0.5], null, null, "B2"), new(570, 5, [0.5, 0.5, 0.5], [0.1, 0.5, 1.0], null, null, "C2")],
        [new(780, 1, [0.9, 0.9, 0.9], [0.7, 0.8, 0.9], 0.6, 0.04, "A3"), new(780, 1, [0.5, 0.5, 0.5], [0.4, 0.5, 0.6], 0.6, 0.04, "B3"), new(780, 1, [0.1, 0.1, 0.1], [0.1, 0.2, 0.3], 0.6, 0.04, "C3")]
    ];
    
    private static readonly double[][] WeightValues = [
        [1 / 3.0, 1 / 3.0, 1 / 3.0],
        [0.5, 0.5, 0.5],
        [1, 0, 0],
        [0, 1, 0],
        [0, 0, 1],
        [0, 0.25, 0.75],
        [0.75, 0, 0.25],
        [0.25, 0.75, 0],
        [0.1, 0.3, 0.6],
        [0.6, 0.3, 0.1]
    ];

    [Test, Combinatorial]
    public void PigmentSingleConstant(
        [ValueSource(nameof(PigmentSingleConstantValues))] Pigment[] pigments,
        [ValueSource(nameof(WeightValues))] double[] weights)
    {
        var expected = new Unicolour(pigments, weights);
        AssertNoError(expected, new Unicolour(pigments, weights));
        AssertNoError(expected, new Unicolour(Configuration.Default, pigments, weights));
    }
    
    [Test, Combinatorial]
    public void PigmentTwoConstant(
        [ValueSource(nameof(PigmentTwoConstantValues))] Pigment[] pigments,
        [ValueSource(nameof(WeightValues))] double[] weights)
    {
        var expected = new Unicolour(pigments, weights);
        AssertNoError(expected, new Unicolour(pigments, weights));
        AssertNoError(expected, new Unicolour(Configuration.Default, pigments, weights));
    }
    
    private static readonly List<double[]> IccValues = [[0.0, 0.0, 0.0, 0.0], [1.0, 1.0, 1.0, 1.0], [0.5, 0.5, 0.5, 0.5]];
    
    [Test]
    public void IccChannelsNoAlpha(
        [ValueSource(nameof(IccValues))] double[] iccValues)
    {
        var profile = IccFile.Fogra39.GetProfile();
        var config = new Configuration(iccConfig: new IccConfiguration(profile));
        var expected = new Unicolour(config, new Channels(iccValues));
        AssertNoError(expected, new Unicolour(config, new Channels(iccValues)));
    }
    
    [Test, Sequential]
    public void IccChannelsWithAlpha(
        [ValueSource(nameof(IccValues))] double[] iccValues,
        [Values(0, 1, 0.5)] double alpha)
    {
        var profile = IccFile.Fogra39.GetProfile();
        var config = new Configuration(iccConfig: new IccConfiguration(profile));
        var expected = new Unicolour(config, new Channels(iccValues), alpha);
        AssertNoError(expected, new Unicolour(config, new Channels(iccValues), alpha));
    }
    
    [Test]
    public void IccChannelsUncalibratedNoAlpha(
        [ValueSource(nameof(IccValues))] double[] iccValues)
    {
        var expected = new Unicolour(new Channels(iccValues));
        AssertNoError(expected, new Unicolour(new Channels(iccValues)));
        AssertNoError(expected, new Unicolour(Configuration.Default, new Channels(iccValues)));
    }
    
    [Test, Sequential]
    public void IccChannelsUncalibratedWithAlpha(
        [ValueSource(nameof(IccValues))] double[] iccValues,
        [Values(0, 1, 0.5)] double alpha)
    {
        var expected = new Unicolour(new Channels(iccValues), alpha);
        AssertNoError(expected, new Unicolour(new Channels(iccValues), alpha));
        AssertNoError(expected, new Unicolour(Configuration.Default, new Channels(iccValues), alpha));
    }

    private static void AssertNoError(Unicolour expected, Unicolour colour)
    {
        TestUtils.AssertNoPropertyError(colour);
        Assert.That(colour, Is.EqualTo(expected));
    }
}

