﻿namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

/*
 * expected colour values for these tests based on calculations from
 * http://www.brucelindbloom.com/index.html?ColorCalculator.html
 */
public static class RgbConfigurationTests
{
    private const double Tolerance = 0.01;
    
    // https://en.wikipedia.org/wiki/Adobe_RGB_color_space
    private static readonly Chromaticity AdobeChromaticityR = new(0.64, 0.33);
    private static readonly Chromaticity AdobeChromaticityG = new(0.21, 0.71);
    private static readonly Chromaticity AdobeChromaticityB = new(0.15, 0.06);

    // https://en.wikipedia.org/wiki/Wide-gamut_RGB_color_space
    private static readonly Chromaticity WideGamutChromaticityR = new(0.7347, 0.2653);
    private static readonly Chromaticity WideGamutChromaticityG = new(0.1152, 0.8264);
    private static readonly Chromaticity WideGamutChromaticityB = new(0.1566, 0.0177);

    private const double Gamma = 2.19921875;
    
    [Test]
    public static void XyzD65ToStandardRgbD65()
    {
        // https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
        var expectedMatrixA = new[,]
        {
            {3.2406, -1.5372, -0.4986},
            {-0.9689, 1.8758, 0.0415},
            {0.0557, -0.2040, 1.0570}
        };

        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrixB = new[,]
        {
            {3.2404542, -1.5371385, -0.4985314},
            {-0.9692660, 1.8760108, 0.0415560},
            {0.0556434, -0.2040259,  1.0572252}
        };
        
        // testing default config values; other tests explicitly construct configs
        var xyzToRgbMatrix = Matrices.RgbToXyzMatrix(RgbConfiguration.StandardRgb, XyzConfiguration.D65).Inverse();
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrixA).Within(0.0005));
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrixB).Within(0.0000001));
        
        var unicolourXyz = Unicolour.FromXyz(Configuration.Default, 0.200757, 0.119618, 0.506757);
        var unicolourXyzNoConfig = Unicolour.FromXyz(0.200757, 0.119618, 0.506757);
        var unicolourLab = Unicolour.FromLab(Configuration.Default, 41.1553, 51.4108, -56.4485);
        var unicolourLabNoConfig = Unicolour.FromLab(41.1553, 51.4108, -56.4485);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourXyzNoConfig.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLabNoConfig.Rgb.Triplet, expectedRgb, Tolerance);
    }

    [Test]
    public static void XyzD50ToStandardRgbD65()
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
            {3.1338561, -1.6168667, -0.4906146},
            {-0.9787684, 1.9161415, 0.0334540},
            { 0.0719453, -0.2289914, 1.4052427}
        };
        
        var xyzToRgbMatrix = Matrices.RgbToXyzMatrix(config.Rgb, config.Xyz).Inverse();
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrix).Within(0.0000001));

        var unicolourXyz = Unicolour.FromXyz(config, 0.187691, 0.115771, 0.381093);
        var unicolourLab = Unicolour.FromLab(config, 40.5359, 46.0847, -57.1158);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }

    [Test]
    public static void XyzD65ToAdobeRgbD65()
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
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }
    
    [Test]
    public static void XyzD50ToAdobeRgbD65()
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
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }

    [Test]
    public static void XyzD65ToWideGamutRgbD50()
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
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }

    [Test]
    public static void XyzD50ToWideGamutRgbD50()
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
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }

    // TODO: update following tests when Unicolour configuration conversion is implemented
    // these tests currently convert manually via XYZ space

    private static readonly Dictionary<ColourTriplet, ColourTriplet> StandardRgbToDisplayP3Lookup = new()
    {
        { new(1.0, 0.0, 0.0), new(0.9175, 0.2003, 0.1386) },
        { new(0.0, 1.0, 0.0), new(0.4587, 0.9853, 0.2983) },
        { new(0.0, 0.0, 1.0), new(0.0000, 0.0000, 0.9596) },
        { new(1.0930, -0.5435, -0.2538), new(1.0, 0.0, 0.0) },
        { new(-2.9057, 1.0183, -1.0162), new(0.0, 1.0, 0.0) },
        { new(0.00, 0.00, 1.04), new(0.0, 0.0, 1.0) }
    };
    
    private static readonly Dictionary<ColourTriplet, ColourTriplet> StandardRgbToRec2020Lookup = new()
    {
        { new(1.0, 0.0, 0.0), new(0.7920, 0.2310, 0.0738) },
        { new(0.0, 1.0, 0.0), new(0.5675, 0.9593, 0.2690) },
        { new(0.0, 0.0, 1.0), new(0.1683, 0.0511, 0.9468) },
        { new(1.2482, -1.6094, -0.2346), new(1.0, 0.0, 0.0) },
        { new(-7.5910, 1.0563, -1.2998), new(0.0, 1.0, 0.0) },
        { new(-0.9409, -0.1079, 1.0505), new(0.0, 0.0, 1.0) }
    };

    [Test]
    public static void StandardRgbToDisplayP3()
    {
        var standardRgbConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
        var displayP3Config = new Configuration(RgbConfiguration.DisplayP3, XyzConfiguration.D65);

        foreach (var (standardRgbTriplet, displayP3Triplet) in StandardRgbToDisplayP3Lookup)
        {
            var standardRgb = Unicolour.FromRgb(standardRgbConfig, standardRgbTriplet.Tuple);
            var xyz = standardRgb.Xyz;
            var displayP3 = Unicolour.FromXyz(displayP3Config, xyz.Triplet.Tuple);
            AssertUtils.AssertColourTriplet(displayP3.Rgb.Triplet, displayP3Triplet, Tolerance);
        }
    }
    
    [Test]
    public static void DisplayP3ToStandardRgb()
    {
        var standardRgbConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
        var displayP3Config = new Configuration(RgbConfiguration.DisplayP3, XyzConfiguration.D65);
        
        foreach (var (standardRgbTriplet, displayP3Triplet) in StandardRgbToDisplayP3Lookup)
        {
            var displayP3 = Unicolour.FromRgb(displayP3Config, displayP3Triplet.Tuple);
            var xyz = displayP3.Xyz;
            var standardRgb = Unicolour.FromXyz(standardRgbConfig, xyz.Triplet.Tuple);
            AssertUtils.AssertColourTriplet(standardRgb.Rgb.Triplet, standardRgbTriplet, Tolerance);
        }
    }

    [Test]
    public static void StandardRgbToRec2020()
    {
        var standardRgbConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
        var rec2020Config = new Configuration(RgbConfiguration.Rec2020, XyzConfiguration.D65);

        foreach (var (standardRgbTriplet, rec2020Triplet) in StandardRgbToRec2020Lookup)
        {
            var standardRgb = Unicolour.FromRgb(standardRgbConfig, standardRgbTriplet.Tuple);
            var xyz = standardRgb.Xyz;
            var rec2020 = Unicolour.FromXyz(rec2020Config, xyz.Triplet.Tuple);
            AssertUtils.AssertColourTriplet(rec2020.Rgb.Triplet, rec2020Triplet, Tolerance);
        }
    }
    
    [Test]
    public static void Rec2020ToStandardRgb()
    {
        var standardRgbConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
        var rec2020Config = new Configuration(RgbConfiguration.Rec2020, XyzConfiguration.D65);
        
        foreach (var (standardRgbTriplet, rec2020Triplet) in StandardRgbToRec2020Lookup)
        {
            var rec2020 = Unicolour.FromRgb(rec2020Config, rec2020Triplet.Tuple);
            var xyz = rec2020.Xyz;
            var standardRgb = Unicolour.FromXyz(standardRgbConfig, xyz.Triplet.Tuple);
            AssertUtils.AssertColourTriplet(standardRgb.Rgb.Triplet, standardRgbTriplet, Tolerance);
        }
    }
}