namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Lookups;

public static class ConfigurationTests
{
    /*
     * expected colour values for these tests based on calculations from
     * http://www.brucelindbloom.com/index.html?ColorCalculator.html
     */
    
    private static readonly Chromaticity AdobeChromaticityR = new(0.64, 0.33);
    private static readonly Chromaticity AdobeChromaticityG = new(0.21, 0.71);
    private static readonly Chromaticity AdobeChromaticityB = new(0.15, 0.06);

    private static readonly Chromaticity WideGamutChromaticityR = new(0.7347, 0.2653);
    private static readonly Chromaticity WideGamutChromaticityG = new(0.1152, 0.8264);
    private static readonly Chromaticity WideGamutChromaticityB = new(0.1566, 0.0177);

    [Test]
    public static void StandardRgbD65ToXyzD65()
    {
        var rgbToXyzMatrix = Configuration.Default.RgbToXyzMatrix;
        
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
        
        var unicolourNoConfig = Unicolour.FromRgb(0.5, 0.25, 0.75);
        var unicolourWithConfig = Unicolour.FromRgb(Configuration.Default, 0.5, 0.25, 0.75);
        var expectedColour = new TestColour
        {
            Xyz = (0.200757, 0.119618, 0.506757),
            Lab = (41.1553, 51.4108, -56.4485),
            // Luv = (41.1553, 16.3709, -86.7190)
        };
        
        Assert.That(rgbToXyzMatrix.Data, Is.EqualTo(expectedMatrixA).Within(0.0005));
        Assert.That(rgbToXyzMatrix.Data, Is.EqualTo(expectedMatrixB).Within(0.0000001));
        AssertColour(unicolourNoConfig, expectedColour);
        AssertColour(unicolourWithConfig, expectedColour);
    }
    
    [Test]
    public static void StandardRgbD65ToXyzD50()
    {
        var configuration = new Configuration(
            Chromaticity.StandardRgbR,
            Chromaticity.StandardRgbG,
            Chromaticity.StandardRgbB,
            Illuminant.D65,
            Illuminant.D50,
            Observer.Standard2,
            Companding.InverseStandardRgb);
        
        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrix = new[,]
        {
            {0.4360747, 0.3850649, 0.1430804},
            {0.2225045, 0.7168786, 0.0606169},
            {0.0139322, 0.0971045, 0.7141733}
        };
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedColour = new TestColour
        {
            Xyz = (0.187691, 0.115771, 0.381093),
            Lab = (40.5359, 46.0847, -57.1158),
            // Luv = (40.5359, 18.7523, -78.2057)
        };
        
        Assert.That(configuration.RgbToXyzMatrix.Data, Is.EqualTo(expectedMatrix).Within(0.0000001));
        AssertColour(unicolour, expectedColour);
    }
    
    [Test]
    public static void AdobeRgbD65ToXyzD65()
    {
        var configuration = new Configuration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB,
            Illuminant.D65,
            Illuminant.D65,
            Observer.Standard2,
            value => Companding.InverseGamma(value, 2.19921875));
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedColour = new TestColour
        {
            Xyz = (0.234243, 0.134410, 0.535559),
            Lab = (43.4203, 57.3600, -55.4259),
            // Luv = (43.4203, 25.4480, -87.3268)
        };
        
        AssertColour(unicolour, expectedColour);
    }
    
    [Test]
    public static void AdobeRgbD65ToXyzD50()
    {
        var configuration = new Configuration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB,
            Illuminant.D65,
            Illuminant.D50,
            Observer.Standard2,
            value => Companding.InverseGamma(value, 2.19921875));
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedColour = new TestColour
        {
            Xyz = (0.221673, 0.130920, 0.402670),
            Lab = (42.9015, 52.4152, -55.9013),
            // Luv = (42.9015, 29.0751, -78.5576)
        };
        
        AssertColour(unicolour, expectedColour);
    }
    
    [Test]
    public static void WideGamutRgbD50ToXyzD65()
    {
        var configuration = new Configuration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            Illuminant.D50,
            Illuminant.D65,
            Observer.Standard2,
            value => Companding.InverseGamma(value, 2.19921875));
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedColour = new TestColour
        {
            Xyz = (0.251993, 0.102404, 0.550393),
            Lab = (38.2704, 87.2838, -65.7493),
            // Luv = (38.2704, 47.3837, -99.6819)
        };
        
        AssertColour(unicolour, expectedColour);
    }
    
    [Test]
    public static void WideGamutRgbD50ToXyzD50()
    {
        var configuration = new Configuration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            Illuminant.D50,
            Illuminant.D50,
            Observer.Standard2,
            value => Companding.InverseGamma(value, 2.19921875));
        
        var unicolour = Unicolour.FromRgb(configuration, 0.5, 0.25, 0.75);
        var expectedColour = new TestColour
        {
            Xyz = (0.238795, 0.099490, 0.413181),
            Lab = (37.7508, 82.3084, -66.1402),
            // Luv = (37.7508, 55.1488, -91.6044)
        };
        
        AssertColour(unicolour, expectedColour);
    }

    private static void AssertColour(Unicolour unicolour, TestColour expected)
    {
        Assert.That(unicolour.Xyz.Tuple, Is.EqualTo(expected.Xyz).Within(0.001));
        Assert.That(unicolour.Lab.Tuple, Is.EqualTo(expected.Lab).Within(0.05));
    }
}