namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class RgbConfigurationTests
{
    /*
     * expected colour values for these tests based on calculations from
     * http://www.brucelindbloom.com/index.html?ColorCalculator.html
     */
    private const double Tolerance = 0.01;
    
    private static readonly Chromaticity AdobeChromaticityR = new(0.64, 0.33);
    private static readonly Chromaticity AdobeChromaticityG = new(0.21, 0.71);
    private static readonly Chromaticity AdobeChromaticityB = new(0.15, 0.06);

    private static readonly Chromaticity WideGamutChromaticityR = new(0.7347, 0.2653);
    private static readonly Chromaticity WideGamutChromaticityG = new(0.1152, 0.8264);
    private static readonly Chromaticity WideGamutChromaticityB = new(0.1566, 0.0177);

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
        
        var xyzToRgbMatrix = Matrices.RgbToXyzMatrix(Configuration.Default).Inverse();
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
            {3.1338561, -1.6168667, -0.4906146},
            {-0.9787684, 1.9161415, 0.0334540},
            { 0.0719453, -0.2289914, 1.4052427}
        };
        
        var xyzToRgbMatrix = Matrices.RgbToXyzMatrix(configuration).Inverse();
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrix).Within(0.0000001));

        var unicolourXyz = Unicolour.FromXyz(configuration, 0.187691, 0.115771, 0.381093);
        var unicolourLab = Unicolour.FromLab(configuration, 40.5359, 46.0847, -57.1158);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }

    [Test]
    public static void XyzD65ToAdobeRgbD65()
    {
        var configuration = new Configuration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB,
            value => Companding.Gamma(value, 2.19921875),
            value => Companding.InverseGamma(value, 2.19921875), 
            WhitePoint.From(Illuminant.D65, Observer.Standard2), 
            WhitePoint.From(Illuminant.D65, Observer.Standard2));
        
        var unicolourXyz = Unicolour.FromXyz(configuration, 0.234243, 0.134410, 0.535559);
        var unicolourLab = Unicolour.FromLab(configuration, 43.4203, 57.3600, -55.4259);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }
    
    [Test]
    public static void XyzD50ToAdobeRgbD65()
    {
        var configuration = new Configuration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB,
            value => Companding.Gamma(value, 2.19921875),
            value => Companding.InverseGamma(value, 2.19921875), 
            WhitePoint.From(Illuminant.D65, Observer.Standard2), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2));
        
        var unicolourXyz = Unicolour.FromXyz(configuration, 0.221673, 0.130920, 0.402670);
        var unicolourLab = Unicolour.FromLab(configuration, 42.9015, 52.4152, -55.9013);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }

    [Test]
    public static void XyzD65ToWideGamutRgbD50()
    {
        var configuration = new Configuration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            value => Companding.Gamma(value, 2.19921875),
            value => Companding.InverseGamma(value, 2.19921875), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2), 
            WhitePoint.From(Illuminant.D65, Observer.Standard2));
        
        var unicolourXyz = Unicolour.FromXyz(configuration, 0.251993, 0.102404, 0.550393);
        var unicolourLab = Unicolour.FromLab(configuration, 38.2704, 87.2838, -65.7493);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }

    [Test]
    public static void XyzD50ToWideGamutRgbD50()
    {
        var configuration = new Configuration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            value => Companding.Gamma(value, 2.19921875),
            value => Companding.InverseGamma(value, 2.19921875), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2), 
            WhitePoint.From(Illuminant.D50, Observer.Standard2));
        
        var unicolourXyz = Unicolour.FromXyz(configuration, 0.238795, 0.099490, 0.413181);
        var unicolourLab = Unicolour.FromLab(configuration, 37.7508, 82.3084, -66.1402);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        AssertUtils.AssertColourTriplet(unicolourXyz.Rgb.Triplet, expectedRgb, Tolerance);
        AssertUtils.AssertColourTriplet(unicolourLab.Rgb.Triplet, expectedRgb, Tolerance);
    }
}