namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class XyzConfigurationTests
{
    /*
     * expected colour values for these tests based on calculations from
     * http://www.brucelindbloom.com/index.html?ColorCalculator.html
     */
    private const double XyzTolerance = 0.001;
    private const double LabTolerance = 0.05;
    private const double LuvTolerance = 0.1;
    
    private static readonly Chromaticity AdobeChromaticityR = new(0.64, 0.33);
    private static readonly Chromaticity AdobeChromaticityG = new(0.21, 0.71);
    private static readonly Chromaticity AdobeChromaticityB = new(0.15, 0.06);

    private static readonly Chromaticity WideGamutChromaticityR = new(0.7347, 0.2653);
    private static readonly Chromaticity WideGamutChromaticityG = new(0.1152, 0.8264);
    private static readonly Chromaticity WideGamutChromaticityB = new(0.1566, 0.0177);

    [Test]
    public static void StandardRgbD65ToXyzD65()
    {
        // https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
        var expectedMatrixA = new[,]
        {
            {0.4124, 0.3576, 0.1805},
            {0.2126, 0.7152, 0.0722},
            {0.0193, 0.1192, 0.9505}
        };

        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrixB = new[,]
        {
            {0.4124564, 0.3575761, 0.1804375},
            {0.2126729, 0.7151522, 0.0721750},
            {0.0193339, 0.1191920, 0.9503041}
        };
        
        var rgbToXyzMatrix = Matrices.RgbToXyzMatrix(Configuration.Default);
        Assert.That(rgbToXyzMatrix.Data, Is.EqualTo(expectedMatrixA).Within(0.0005));
        Assert.That(rgbToXyzMatrix.Data, Is.EqualTo(expectedMatrixB).Within(0.0000001));
        
        var unicolour = Unicolour.FromRgb(Configuration.Default, 0.5, 0.25, 0.75);
        var unicolourNoConfig = Unicolour.FromRgb(0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.200757, 0.119618, 0.506757);
        var expectedLab = new ColourTriplet(41.1553, 51.4108, -56.4485);
        var expectedLuv = new ColourTriplet(41.1553, 16.3709, -86.7190);
        AssertUtils.AssertColourTriplet(unicolour.Xyz.Triplet, expectedXyz, XyzTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Lab.Triplet, expectedLab, LabTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Luv.Triplet, expectedLuv, LuvTolerance);
        AssertUtils.AssertColourTriplet(unicolourNoConfig.Xyz.Triplet, expectedXyz, XyzTolerance);
        AssertUtils.AssertColourTriplet(unicolourNoConfig.Lab.Triplet, expectedLab, LabTolerance);
        AssertUtils.AssertColourTriplet(unicolourNoConfig.Luv.Triplet, expectedLuv, LuvTolerance);
    }

    [Test]
    public static void StandardRgbD65ToXyzD50()
    {
        var configuration = new Configuration(
            Chromaticity.StandardRgbR,
            Chromaticity.StandardRgbG,
            Chromaticity.StandardRgbB,
            Companding.StandardRgb, 
            Companding.InverseStandardRgb, 
            WhitePoint.From(Illuminant.D65, Observer.Standard2), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2));
        
        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrix = new[,]
        {
            {0.4360747, 0.3850649, 0.1430804},
            {0.2225045, 0.7168786, 0.0606169},
            {0.0139322, 0.0971045, 0.7141733}
        };
        
        var rgbToXyzMatrix = Matrices.RgbToXyzMatrix(configuration);
        Assert.That(rgbToXyzMatrix.Data, Is.EqualTo(expectedMatrix).Within(0.0000001));

        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.187691, 0.115771, 0.381093);
        var expectedLab = new ColourTriplet(40.5359, 46.0847, -57.1158);
        var expectedLuv = new ColourTriplet(40.5359, 18.7523, -78.2057);
        AssertUtils.AssertColourTriplet(unicolour.Xyz.Triplet, expectedXyz, XyzTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Lab.Triplet, expectedLab, LabTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Luv.Triplet, expectedLuv, LuvTolerance);
    }

    [Test]
    public static void AdobeRgbD65ToXyzD65()
    {
        var configuration = new Configuration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB,
            value => Companding.Gamma(value, 2.19921875),
            value => Companding.InverseGamma(value, 2.19921875), 
            WhitePoint.From(Illuminant.D65, Observer.Standard2), 
            WhitePoint.From(Illuminant.D65, Observer.Standard2));
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.234243, 0.134410, 0.535559);
        var expectedLab = new ColourTriplet(43.4203, 57.3600, -55.4259);
        var expectedLuv = new ColourTriplet(43.4203, 25.4480, -87.3268);
        AssertUtils.AssertColourTriplet(unicolour.Xyz.Triplet, expectedXyz, XyzTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Lab.Triplet, expectedLab, LabTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Luv.Triplet, expectedLuv, LuvTolerance);
    }

    [Test]
    public static void AdobeRgbD65ToXyzD50()
    {
        var configuration = new Configuration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB,
            value => Companding.Gamma(value, 2.19921875),
            value => Companding.InverseGamma(value, 2.19921875), 
            WhitePoint.From(Illuminant.D65, Observer.Standard2), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2));
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.221673, 0.130920, 0.402670);
        var expectedLab = new ColourTriplet(42.9015, 52.4152, -55.9013);
        var expectedLuv = new ColourTriplet(42.9015, 29.0751, -78.5576);
        AssertUtils.AssertColourTriplet(unicolour.Xyz.Triplet, expectedXyz, XyzTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Lab.Triplet, expectedLab, LabTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Luv.Triplet, expectedLuv, LuvTolerance);
    }

    [Test]
    public static void WideGamutRgbD50ToXyzD65()
    {
        var configuration = new Configuration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            value => Companding.Gamma(value, 2.19921875),
            value => Companding.InverseGamma(value, 2.19921875), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2), 
            WhitePoint.From(Illuminant.D65, Observer.Standard2));
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.251993, 0.102404, 0.550393);
        var expectedLab = new ColourTriplet(38.2704, 87.2838, -65.7493);
        var expectedLuv = new ColourTriplet(38.2704, 47.3837, -99.6819);
        AssertUtils.AssertColourTriplet(unicolour.Xyz.Triplet, expectedXyz, XyzTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Lab.Triplet, expectedLab, LabTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Luv.Triplet, expectedLuv, LuvTolerance);
    }

    [Test]
    public static void WideGamutRgbD50ToXyzD50()
    {
        var configuration = new Configuration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            value => Companding.Gamma(value, 2.19921875),
            value => Companding.InverseGamma(value, 2.19921875), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2));
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.238795, 0.099490, 0.413181);
        var expectedLab = new ColourTriplet(37.7508, 82.3084, -66.1402);
        var expectedLuv = new ColourTriplet(37.7508, 55.1488, -91.6044);
        AssertUtils.AssertColourTriplet(unicolour.Xyz.Triplet, expectedXyz, XyzTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Lab.Triplet, expectedLab, LabTolerance);
        AssertUtils.AssertColourTriplet(unicolour.Luv.Triplet, expectedLuv, LuvTolerance);
    }

    [TestCase(Illuminant.D65, 0.312727, 0.329023)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496)]
    [TestCase(Illuminant.E, 0.333333, 0.333333)]
    public static void WhiteChromaticity(Illuminant illuminant, double expectedX, double expectedY)
    {
        var configuration = new Configuration(
            Chromaticity.StandardRgbR,
            Chromaticity.StandardRgbG,
            Chromaticity.StandardRgbB,
            Companding.StandardRgb, 
            Companding.InverseStandardRgb, 
            WhitePoint.From(Illuminant.D65, Observer.Standard2), 
            WhitePoint.From(illuminant, Observer.Standard2));

        var chromaticity = configuration.ChromaticityWhite;
        Assert.That(Math.Round(chromaticity.X, 6), Is.EqualTo(Math.Round(expectedX, 6)));
        Assert.That(Math.Round(chromaticity.Y, 6), Is.EqualTo(Math.Round(expectedY, 6)));
    }
}