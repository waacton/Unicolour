namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

/*
 * expected colour values for these tests based on calculations from
 * http://www.brucelindbloom.com/index.html?ColorCalculator.html
 */
public class ConfigureRgbTests
{
    private const double Tolerance = 0.001;
    
    // https://en.wikipedia.org/wiki/Adobe_RGB_color_space
    private static readonly Chromaticity AdobeChromaticityR = new(0.64, 0.33);
    private static readonly Chromaticity AdobeChromaticityG = new(0.21, 0.71);
    private static readonly Chromaticity AdobeChromaticityB = new(0.15, 0.06);

    // https://en.wikipedia.org/wiki/Wide-gamut_RGB_color_space
    private static readonly Chromaticity WideGamutChromaticityR = new(0.7347, 0.2653);
    private static readonly Chromaticity WideGamutChromaticityG = new(0.1152, 0.8264);
    private static readonly Chromaticity WideGamutChromaticityB = new(0.1566, 0.0177);

    private const double Gamma = 2.19921875;
    
    private static readonly Configuration StandardRgbConfig = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
    private static readonly Configuration DisplayP3Config = new(RgbConfiguration.DisplayP3, XyzConfiguration.D65);
    private static readonly Configuration Rec2020Config = new(RgbConfiguration.Rec2020, XyzConfiguration.D65);
    
    private static readonly List<TestCaseData> StandardRgbToDisplayP3Lookup = new()
    {
        new TestCaseData(new ColourTriplet(1, 0, 0), new ColourTriplet(0.9175, 0.2003, 0.1386)),
        new TestCaseData(new ColourTriplet(0, 1, 0), new ColourTriplet(0.4584, 0.9853, 0.2983)),
        new TestCaseData(new ColourTriplet(0, 0, 1), new ColourTriplet(0.0000, 0.0000, 0.9596)),
        new TestCaseData(new ColourTriplet(1.0931, -0.2267, -0.1501), new ColourTriplet(1, 0, 0)), 
        new TestCaseData(new ColourTriplet(-0.5116, 1.0183, -0.3107), new ColourTriplet(0, 1, 0)),
        new TestCaseData(new ColourTriplet(0.0000, 0.0000, 1.0420), new ColourTriplet(0, 0, 1))
    };
    
    private static readonly List<TestCaseData> StandardRgbToRec2020Lookup = new()
    {
        new TestCaseData(new ColourTriplet(1, 0, 0), new ColourTriplet(0.7920, 0.2310, 0.0738)),
        new TestCaseData(new ColourTriplet(0, 1, 0), new ColourTriplet(0.5675, 0.9593, 0.2690)),
        new TestCaseData(new ColourTriplet(0, 0, 1), new ColourTriplet(0.1684, 0.0511, 0.9468)),
        new TestCaseData(new ColourTriplet(1.2482, -0.3879, -0.1435), new ColourTriplet(1, 0, 0)), 
        new TestCaseData(new ColourTriplet(-0.7904, 1.0563, -0.3502), new ColourTriplet(0, 1, 0)),
        new TestCaseData(new ColourTriplet(-0.2992, -0.0886, 1.0505), new ColourTriplet(0, 0, 1))
    };
    
    [Test]
    public void XyzD65ToStandardRgbD65()
    {
        // https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
        var expectedMatrixA = new[,]
        {
            { 3.2406, -1.5372, -0.4986 },
            { -0.9689, 1.8758, 0.0415 },
            { 0.0557, -0.2040, 1.0570 }
        };

        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrixB = new[,]
        {
            { 3.2404542, -1.5371385, -0.4985314 },
            { -0.9692660, 1.8760108, 0.0415560 },
            { 0.0556434, -0.2040259, 1.0572252 }
        };
        
        // testing default config values; other tests explicitly construct configs
        var xyzToRgbMatrix = Rgb.RgbToXyzMatrix(RgbConfiguration.StandardRgb, XyzConfiguration.D65).Inverse();
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrixA).Within(0.0005));
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrixB).Within(0.0000001));
        
        var unicolourXyz = Unicolour.FromXyz(Configuration.Default, 0.200757, 0.119618, 0.506757);
        var unicolourXyzNoConfig = Unicolour.FromXyz(0.200757, 0.119618, 0.506757);
        var unicolourLab = Unicolour.FromLab(Configuration.Default, 41.1553, 51.4108, -56.4485);
        var unicolourLabNoConfig = Unicolour.FromLab(41.1553, 51.4108, -56.4485);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        AssertUtils.AssertTriplet<Rgb>(unicolourXyzNoConfig, expectedRgb, Tolerance);
        AssertUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
        AssertUtils.AssertTriplet<Rgb>(unicolourLabNoConfig, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD50ToStandardRgbD65()
    {
        var standardRgbConfig = new RgbConfiguration(
            Chromaticity.StandardRgb.R,
            Chromaticity.StandardRgb.G,
            Chromaticity.StandardRgb.B,
            WhitePoint.From(Illuminant.D65, Observer.Standard2),
            Companding.StandardRgb.FromLinear,
            Companding.StandardRgb.ToLinear);
        var d50XyzConfig = new XyzConfiguration(WhitePoint.From(Illuminant.D50));
        var config = new Configuration(standardRgbConfig, d50XyzConfig);

        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrix = new[,]
        {
            { 3.1338561, -1.6168667, -0.4906146 },
            { -0.9787684, 1.9161415, 0.0334540 },
            { 0.0719453, -0.2289914, 1.4052427 }
        };
        
        var xyzToRgbMatrix = Rgb.RgbToXyzMatrix(config.Rgb, config.Xyz).Inverse();
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrix).Within(0.0000001));

        var unicolourXyz = Unicolour.FromXyz(config, 0.187691, 0.115771, 0.381093);
        var unicolourLab = Unicolour.FromLab(config, 40.5359, 46.0847, -57.1158);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        AssertUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD65ToAdobeRgbD65()
    {
        var adobeRgbConfig = new RgbConfiguration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB, 
            WhitePoint.From(Illuminant.D65, Observer.Standard2),
            value => Companding.Gamma(value, Gamma),
            value => Companding.InverseGamma(value, Gamma));
        var d65XyzConfig = new XyzConfiguration(WhitePoint.From(Illuminant.D65));
        var config = new Configuration(adobeRgbConfig, d65XyzConfig);

        var unicolourXyz = Unicolour.FromXyz(config, 0.234243, 0.134410, 0.535559);
        var unicolourLab = Unicolour.FromLab(config, 43.4203, 57.3600, -55.4259);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        AssertUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }
    
    [Test]
    public void XyzD50ToAdobeRgbD65()
    {
        var adobeRgbConfig = new RgbConfiguration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB, 
            WhitePoint.From(Illuminant.D65, Observer.Standard2),
            value => Companding.Gamma(value, Gamma),
            value => Companding.InverseGamma(value, Gamma));
        var d50XyzConfig = new XyzConfiguration(WhitePoint.From(Illuminant.D50));
        var config = new Configuration(adobeRgbConfig, d50XyzConfig);
        
        var unicolourXyz = Unicolour.FromXyz(config, 0.221673, 0.130920, 0.402670);
        var unicolourLab = Unicolour.FromLab(config, 42.9015, 52.4152, -55.9013);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        AssertUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD65ToWideGamutRgbD50()
    {
        var wideGamutRgbConfig = new RgbConfiguration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB, 
            WhitePoint.From(Illuminant.D50, Observer.Standard2),
            value => Companding.Gamma(value, Gamma),
            value => Companding.InverseGamma(value, Gamma));
        var d65XyzConfig = new XyzConfiguration(WhitePoint.From(Illuminant.D65));
        var config = new Configuration(wideGamutRgbConfig, d65XyzConfig);
        
        var unicolourXyz = Unicolour.FromXyz(config, 0.251993, 0.102404, 0.550393);
        var unicolourLab = Unicolour.FromLab(config, 38.2704, 87.2838, -65.7493);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        AssertUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD50ToWideGamutRgbD50()
    {
        var wideGamutRgbConfig = new RgbConfiguration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            WhitePoint.From(Illuminant.D50, Observer.Standard2),
            value => Companding.Gamma(value, Gamma),
            value => Companding.InverseGamma(value, Gamma));
        var d50XyzConfig = new XyzConfiguration(WhitePoint.From(Illuminant.D50));
        var config = new Configuration(wideGamutRgbConfig, d50XyzConfig);

        var unicolourXyz = Unicolour.FromXyz(config, 0.238795, 0.099490, 0.413181);
        var unicolourLab = Unicolour.FromLab(config, 37.7508, 82.3084, -66.1402);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        AssertUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }
    
    [TestCaseSource(nameof(StandardRgbToDisplayP3Lookup))]
    public void ConvertStandardRgbToDisplayP3(ColourTriplet standardRgbTriplet, ColourTriplet displayP3Triplet)
    {
        var standardRgb = Unicolour.FromRgb(StandardRgbConfig, standardRgbTriplet.Tuple);
        var displayP3 = standardRgb.ConvertToConfiguration(DisplayP3Config);
        AssertUtils.AssertTriplet<Rgb>(displayP3, displayP3Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(StandardRgbToDisplayP3Lookup))]
    public void ConvertDisplayP3ToStandardRgb(ColourTriplet standardRgbTriplet, ColourTriplet displayP3Triplet)
    {
        var displayP3 = Unicolour.FromRgb(DisplayP3Config, displayP3Triplet.Tuple);
        var standardRgb = displayP3.ConvertToConfiguration(StandardRgbConfig);
        AssertUtils.AssertTriplet<Rgb>(standardRgb, standardRgbTriplet, Tolerance);
    }
    
    [TestCaseSource(nameof(StandardRgbToRec2020Lookup))]
    public void ConvertStandardRgbToRec2020(ColourTriplet standardRgbTriplet, ColourTriplet rec2020Triplet)
    {
        var standardRgb = Unicolour.FromRgb(StandardRgbConfig, standardRgbTriplet.Tuple);
        var rec2020 = standardRgb.ConvertToConfiguration(Rec2020Config);
        AssertUtils.AssertTriplet<Rgb>(rec2020, rec2020Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(StandardRgbToRec2020Lookup))]
    public void ConvertRec2020ToStandardRgb(ColourTriplet standardRgbTriplet, ColourTriplet rec2020Triplet)
    {
        var rec2020 = Unicolour.FromRgb(Rec2020Config, rec2020Triplet.Tuple);
        var standardRgb = rec2020.ConvertToConfiguration(StandardRgbConfig);
        AssertUtils.AssertTriplet<Rgb>(standardRgb, standardRgbTriplet, Tolerance);
    }
    
    [TestCase(Illuminant.A)]
    [TestCase(Illuminant.C)]
    [TestCase(Illuminant.D50)]
    [TestCase(Illuminant.D55)]
    [TestCase(Illuminant.D65)]
    [TestCase(Illuminant.D75)]
    [TestCase(Illuminant.E)]
    [TestCase(Illuminant.F2)]
    [TestCase(Illuminant.F7)]
    [TestCase(Illuminant.F11)]
    public void XyzWhitePointRoundTrip(Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(RgbConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974);
        var expectedRgb = Rgb.FromXyz(initialXyz, RgbConfiguration.StandardRgb, initialXyzConfig);

        var xyzConfig = new XyzConfiguration(WhitePoint.From(xyzIlluminant));
        var xyz = Rgb.ToXyz(expectedRgb, RgbConfiguration.StandardRgb, xyzConfig);
        var rgb = Rgb.FromXyz(xyz, RgbConfiguration.StandardRgb, xyzConfig);
        AssertUtils.AssertTriplet(rgb.Triplet, expectedRgb.Triplet, 0.00000000001);
    }
}