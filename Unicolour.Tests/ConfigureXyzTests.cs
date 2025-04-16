using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * expected colour values for these tests based on calculations from
 * http://www.brucelindbloom.com/index.html?ColorCalculator.html
 */
public class ConfigureXyzTests
{
    private const double XyzTolerance = 0.001;
    private const double LabTolerance = 0.05;
    private const double LuvTolerance = 0.1;
    
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
    public void StandardRgbD65ToXyzD65()
    {
        // https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
        var expectedMatrixA = new[,]
        {
            { 0.4124, 0.3576, 0.1805 },
            { 0.2126, 0.7152, 0.0722 },
            { 0.0193, 0.1192, 0.9505 }
        };

        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrixB = new[,]
        {
            { 0.4124564, 0.3575761, 0.1804375 },
            { 0.2126729, 0.7151522, 0.0721750 },
            { 0.0193339, 0.1191920, 0.9503041 }
        };
        
        // testing default config values; other tests explicitly construct configs
        var rgbToXyzMatrix = RgbConfiguration.StandardRgb.RgbToXyzMatrix;
        Assert.That(rgbToXyzMatrix.Data, Is.EqualTo(expectedMatrixA).Within(0.0005));
        Assert.That(rgbToXyzMatrix.Data, Is.EqualTo(expectedMatrixB).Within(0.0000001));
        
        var colour = new Unicolour(Configuration.Default, ColourSpace.Rgb, 0.5, 0.25, 0.75);
        var colourNoConfig = new Unicolour(ColourSpace.Rgb, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.200757, 0.119618, 0.506757);
        var expectedLab = new ColourTriplet(41.1553, 51.4108, -56.4485);
        var expectedLuv = new ColourTriplet(41.1553, 16.3709, -86.7190);
        TestUtils.AssertTriplet<Xyz>(colour, expectedXyz, XyzTolerance);
        TestUtils.AssertTriplet<Lab>(colour, expectedLab, LabTolerance);
        TestUtils.AssertTriplet<Luv>(colour, expectedLuv, LuvTolerance);
        TestUtils.AssertTriplet<Xyz>(colourNoConfig, expectedXyz, XyzTolerance);
        TestUtils.AssertTriplet<Lab>(colourNoConfig, expectedLab, LabTolerance);
        TestUtils.AssertTriplet<Luv>(colourNoConfig, expectedLuv, LuvTolerance);
    }

    [Test]
    public void StandardRgbD65ToXyzD50()
    {
        var standardRgbConfig = new RgbConfiguration(
            RgbModels.StandardRgb.R,
            RgbModels.StandardRgb.G,
            RgbModels.StandardRgb.B,
            Illuminant.D65.GetWhitePoint(Observer.Degree2),
            RgbModels.StandardRgb.FromLinear,
            RgbModels.StandardRgb.ToLinear);
        var d50XyzConfig = new XyzConfiguration(Illuminant.D50, Observer.Degree2);
        var config = new Configuration(standardRgbConfig, d50XyzConfig);
        
        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrix = new[,]
        {
            { 0.4360747, 0.3850649, 0.1430804 },
            { 0.2225045, 0.7168786, 0.0606169 },
            { 0.0139322, 0.0971045, 0.7141733 }
        };
        
        var rgbToXyzMatrix = RgbConfiguration.StandardRgb.RgbToXyzMatrix;
        rgbToXyzMatrix = Adaptation.WhitePoint(rgbToXyzMatrix, standardRgbConfig.WhitePoint, d50XyzConfig.WhitePoint, d50XyzConfig.AdaptationMatrix);
        Assert.That(rgbToXyzMatrix.Data, Is.EqualTo(expectedMatrix).Within(0.0000001));

        var colour = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.187691, 0.115771, 0.381093);
        var expectedLab = new ColourTriplet(40.5359, 46.0847, -57.1158);
        var expectedLuv = new ColourTriplet(40.5359, 18.7523, -78.2057);
        TestUtils.AssertTriplet<Xyz>(colour, expectedXyz, XyzTolerance);
        TestUtils.AssertTriplet<Lab>(colour, expectedLab, LabTolerance);
        TestUtils.AssertTriplet<Luv>(colour, expectedLuv, LuvTolerance);
    }

    [Test]
    public void AdobeRgbD65ToXyzD65()
    {
        var adobeRgbConfig = new RgbConfiguration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB,
            Illuminant.D65.GetWhitePoint(Observer.Degree2),
            value => Math.Pow(value, 1 / Gamma),
            value => Math.Pow(value, Gamma));
        var d65XyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(adobeRgbConfig, d65XyzConfig);
        
        var colour = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.234243, 0.134410, 0.535559);
        var expectedLab = new ColourTriplet(43.4203, 57.3600, -55.4259);
        var expectedLuv = new ColourTriplet(43.4203, 25.4480, -87.3268);
        TestUtils.AssertTriplet<Xyz>(colour, expectedXyz, XyzTolerance);
        TestUtils.AssertTriplet<Lab>(colour, expectedLab, LabTolerance);
        TestUtils.AssertTriplet<Luv>(colour, expectedLuv, LuvTolerance);
    }

    [Test]
    public void AdobeRgbD65ToXyzD50()
    {
        var adobeRgbConfig = new RgbConfiguration(
            AdobeChromaticityR,
            AdobeChromaticityG,
            AdobeChromaticityB,
            Illuminant.D65.GetWhitePoint(Observer.Degree2),
            value => Math.Pow(value, 1 / Gamma),
            value => Math.Pow(value, Gamma));
        var d50XyzConfig = new XyzConfiguration(Illuminant.D50, Observer.Degree2);
        var config = new Configuration(adobeRgbConfig, d50XyzConfig);
        
        var colour = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.221673, 0.130920, 0.402670);
        var expectedLab = new ColourTriplet(42.9015, 52.4152, -55.9013);
        var expectedLuv = new ColourTriplet(42.9015, 29.0751, -78.5576);
        TestUtils.AssertTriplet<Xyz>(colour, expectedXyz, XyzTolerance);
        TestUtils.AssertTriplet<Lab>(colour, expectedLab, LabTolerance);
        TestUtils.AssertTriplet<Luv>(colour, expectedLuv, LuvTolerance);
    }

    [Test]
    public void WideGamutRgbD50ToXyzD65()
    {
        var wideGamutRgbConfig = new RgbConfiguration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            Illuminant.D50.GetWhitePoint(Observer.Degree2),
            value => Math.Pow(value, 1 / Gamma),
            value => Math.Pow(value, Gamma));
        var d65XyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(wideGamutRgbConfig, d65XyzConfig);
        
        var colour = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.251993, 0.102404, 0.550393);
        var expectedLab = new ColourTriplet(38.2704, 87.2838, -65.7493);
        var expectedLuv = new ColourTriplet(38.2704, 47.3837, -99.6819);
        TestUtils.AssertTriplet<Xyz>(colour, expectedXyz, XyzTolerance);
        TestUtils.AssertTriplet<Lab>(colour, expectedLab, LabTolerance);
        TestUtils.AssertTriplet<Luv>(colour, expectedLuv, LuvTolerance);
    }

    [Test]
    public void WideGamutRgbD50ToXyzD50()
    {
        var wideGamutRgbConfig = new RgbConfiguration(
            WideGamutChromaticityR,
            WideGamutChromaticityG,
            WideGamutChromaticityB,
            Illuminant.D50.GetWhitePoint(Observer.Degree2),
            value => Math.Pow(value, 1 / Gamma),
            value => Math.Pow(value, Gamma));
        var d50XyzConfig = new XyzConfiguration(Illuminant.D50, Observer.Degree2);
        var config = new Configuration(wideGamutRgbConfig, d50XyzConfig);
        
        var colour = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75);
        var expectedXyz = new ColourTriplet(0.238795, 0.099490, 0.413181);
        var expectedLab = new ColourTriplet(37.7508, 82.3084, -66.1402);
        var expectedLuv = new ColourTriplet(37.7508, 55.1488, -91.6044);
        TestUtils.AssertTriplet<Xyz>(colour, expectedXyz, XyzTolerance);
        TestUtils.AssertTriplet<Lab>(colour, expectedLab, LabTolerance);
        TestUtils.AssertTriplet<Luv>(colour, expectedLuv, LuvTolerance);
    }

    [TestCase(nameof(Illuminant.D65), 0.312727, 0.329023)]
    [TestCase(nameof(Illuminant.D50), 0.345669, 0.358496)]
    [TestCase(nameof(Illuminant.E), 0.333333, 0.333333)]
    public void WhiteChromaticity(string illuminantName, double expectedX, double expectedY)
    {
        var illuminant = TestUtils.Illuminants[illuminantName];
        var xyzConfig = new XyzConfiguration(illuminant, Observer.Degree2);
        var chromaticity = xyzConfig.WhiteChromaticity;
        Assert.That(Math.Round(chromaticity.X, 6), Is.EqualTo(Math.Round(expectedX, 6)));
        Assert.That(Math.Round(chromaticity.Y, 6), Is.EqualTo(Math.Round(expectedY, 6)));
    }
    
    [Test]
    public void ConvertWhite()
    {
        Configuration Config(Illuminant illuminant) => new(RgbConfiguration.StandardRgb, new XyzConfiguration(illuminant, Observer.Degree2));

        var initialA = new Unicolour(Config(Illuminant.A), ColourSpace.Rgb, 1, 1, 1);
        var convertedToC = initialA.ConvertToConfiguration(Config(Illuminant.C));
        var convertedToD50 = convertedToC.ConvertToConfiguration(Config(Illuminant.D50));
        var convertedToD55 = convertedToD50.ConvertToConfiguration(Config(Illuminant.D55));
        var convertedToD65 = convertedToD55.ConvertToConfiguration(Config(Illuminant.D65));
        var convertedToD75 = convertedToD65.ConvertToConfiguration(Config(Illuminant.D75));
        var convertedToE = convertedToD75.ConvertToConfiguration(Config(Illuminant.E));
        var convertedToF2 = convertedToE.ConvertToConfiguration(Config(Illuminant.F2));
        var convertedToF7 = convertedToF2.ConvertToConfiguration(Config(Illuminant.F7));
        var convertedToF11 = convertedToF7.ConvertToConfiguration(Config(Illuminant.F11));
        var convertedToA = convertedToF11.ConvertToConfiguration(Config(Illuminant.A));
        
        ColourTriplet WhitePointTriplet(Illuminant illuminant) => illuminant.GetWhitePoint(Observer.Degree2).AsXyzMatrix().ToTriplet();
        TestUtils.AssertTriplet<Xyz>(initialA, WhitePointTriplet(Illuminant.A), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToC, WhitePointTriplet(Illuminant.C), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToD50, WhitePointTriplet(Illuminant.D50), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToD55, WhitePointTriplet(Illuminant.D55), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToD65, WhitePointTriplet(Illuminant.D65), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToD75, WhitePointTriplet(Illuminant.D75), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToE, WhitePointTriplet(Illuminant.E), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToF2, WhitePointTriplet(Illuminant.F2), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToF7, WhitePointTriplet(Illuminant.F7), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToF11, WhitePointTriplet(Illuminant.F11), XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToA, WhitePointTriplet(Illuminant.A), XyzTolerance);
    }

    [Test]
    public void ConvertBlack()
    {
        Configuration Config(Illuminant illuminant) => new(RgbConfiguration.StandardRgb, new XyzConfiguration(illuminant, Observer.Degree2));

        var initialA = new Unicolour(Config(Illuminant.A), ColourSpace.Rgb, 0, 0, 0);
        var convertedToC = initialA.ConvertToConfiguration(Config(Illuminant.C));
        var convertedToD50 = convertedToC.ConvertToConfiguration(Config(Illuminant.D50));
        var convertedToD55 = convertedToD50.ConvertToConfiguration(Config(Illuminant.D55));
        var convertedToD65 = convertedToD55.ConvertToConfiguration(Config(Illuminant.D65));
        var convertedToD75 = convertedToD65.ConvertToConfiguration(Config(Illuminant.D75));
        var convertedToE = convertedToD75.ConvertToConfiguration(Config(Illuminant.E));
        var convertedToF2 = convertedToE.ConvertToConfiguration(Config(Illuminant.F2));
        var convertedToF7 = convertedToF2.ConvertToConfiguration(Config(Illuminant.F7));
        var convertedToF11 = convertedToF7.ConvertToConfiguration(Config(Illuminant.F11));
        var convertedToA = convertedToF11.ConvertToConfiguration(Config(Illuminant.A));

        ColourTriplet zeroes = new(0, 0, 0);
        TestUtils.AssertTriplet<Xyz>(initialA, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToC, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToD50, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToD55, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToD65, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToD75, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToE, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToF2, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToF7, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToF11, zeroes, XyzTolerance);
        TestUtils.AssertTriplet<Xyz>(convertedToA, zeroes, XyzTolerance);
    }
    
    [Test]
    public void RgbWhitePointRoundTrip([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant rgbIlluminant)
    {
        RgbConfiguration RgbConfig(WhitePoint whitePoint, RgbConfiguration baseConfig)
        {
            return new(baseConfig.ChromaticityR, baseConfig.ChromaticityG, baseConfig.ChromaticityB,
                whitePoint, baseConfig.FromLinear, baseConfig.ToLinear);
        }
        
        var initialRgbConfig = RgbConfig(XyzConfiguration.D65.WhitePoint, RgbConfiguration.StandardRgb);
        var initialRgb = new RgbLinear(1.0, 0.08, 0.58);
        var expectedXyz = RgbLinear.ToXyz(initialRgb, initialRgbConfig, XyzConfiguration.D65);
        
        var rgbConfig = RgbConfig(rgbIlluminant.GetWhitePoint(Observer.Degree2), RgbConfiguration.StandardRgb);
        var rgb = RgbLinear.FromXyz(expectedXyz, rgbConfig, XyzConfiguration.D65);
        var xyz = RgbLinear.ToXyz(rgb, rgbConfig, XyzConfiguration.D65);
        TestUtils.AssertTriplet(xyz.Triplet, expectedXyz.Triplet, 0.00000000001);
    }
    
    [Test]
    public void Cam02WhitePointRoundTrip([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant camIlluminant)
    {
        CamConfiguration CamConfig(WhitePoint whitePoint, CamConfiguration baseConfig)
        {
            return new(whitePoint, baseConfig.AdaptingLuminance, baseConfig.BackgroundLuminance, baseConfig.Surround);
        }
        
        var initialCamConfig = CamConfig(XyzConfiguration.D65.WhitePoint, CamConfiguration.StandardRgb);
        var initialCam = new Cam02(62.86, 40.81, -1.18, initialCamConfig);
        var expectedXyz = Cam02.ToXyz(initialCam, initialCamConfig, XyzConfiguration.D65);
        
        var camConfig = CamConfig(camIlluminant.GetWhitePoint(Observer.Degree2), CamConfiguration.StandardRgb);
        var cam = Cam02.FromXyz(expectedXyz, camConfig, XyzConfiguration.D65);
        var xyz = Cam02.ToXyz(cam, camConfig, XyzConfiguration.D65);
        TestUtils.AssertTriplet(xyz.Triplet, expectedXyz.Triplet, 0.00000000001);
    }
    
    [Test]
    public void Cam16WhitePointRoundTrip([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant camIlluminant)
    {
        CamConfiguration CamConfig(WhitePoint whitePoint, CamConfiguration baseConfig)
        {
            return new(whitePoint, baseConfig.AdaptingLuminance, baseConfig.BackgroundLuminance, baseConfig.Surround);
        }
        
        var initialCamConfig = CamConfig(XyzConfiguration.D65.WhitePoint, CamConfiguration.StandardRgb);
        var initialCam = new Cam16(62.47, 42.60, -1.36, initialCamConfig);
        var expectedXyz = Cam16.ToXyz(initialCam, initialCamConfig, XyzConfiguration.D65);
        
        var camConfig = CamConfig(camIlluminant.GetWhitePoint(Observer.Degree2), CamConfiguration.StandardRgb);
        var cam = Cam16.FromXyz(expectedXyz, camConfig, XyzConfiguration.D65);
        var xyz = Cam16.ToXyz(cam, camConfig, XyzConfiguration.D65);
        TestUtils.AssertTriplet(xyz.Triplet, expectedXyz.Triplet, 0.00000000001);
    }
    
    [Test]
    public void ChromaticAdaptationNoData() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new double[,] {},
        expectedAdaptation: new[,]
        {
            { double.NaN, double.NaN, double.NaN },
            { double.NaN, double.NaN, double.NaN },
            { double.NaN, double.NaN, double.NaN }
        }
    );

    [Test]
    public void ChromaticAdaptationMissingRow() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { double.NaN, double.NaN, double.NaN }
        }
    );
    
    [Test]
    public void ChromaticAdaptationMissingColumn() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0 },
            { 0.0, 2.0 },
            { 0.0, 0.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, double.NaN },
            { 0.0, 2.0, double.NaN },
            { 0.0, 0.0, double.NaN }
        }
    );
    
    [Test]
    public void ChromaticAdaptationExtraRow() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { 0.0, 0.0, 3.0 },
            { 9.0, 9.0, 9.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { 0.0, 0.0, 3.0 }
        }
    );
    
    [Test]
    public void ChromaticAdaptationExtraColumn() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0, 9.0 },
            { 0.0, 2.0, 0.0, 9.0 },
            { 0.0, 0.0, 3.0, 9.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { 0.0, 0.0, 3.0 }
        }
    );
    
    [Test]
    public void ChromaticAdaptationExtraRowAndColumn() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0, 9.0 },
            { 0.0, 2.0, 0.0, 9.0 },
            { 0.0, 0.0, 3.0, 9.0 },
            { 9.0, 9.0, 9.0, 9.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { 0.0, 0.0, 3.0 }
        }
    );

    private static void AssertInvalidChromaticAdaptation(double[,] invalidAdaptation, double[,] expectedAdaptation)
    {
        var observer = Observer.Degree2;
        var sourceIlluminant = Illuminant.D65;
        var targetIlluminant = Illuminant.D50;
        var xyzConfig = new XyzConfiguration(sourceIlluminant, observer, invalidAdaptation);
        Assert.That(xyzConfig.AdaptationMatrix.Data, Is.EqualTo(expectedAdaptation));

        var xyz = new Xyz(0.5, 0.5, 0.5);
        var sourceWhite = sourceIlluminant.GetWhitePoint(observer);
        var targetWhite = targetIlluminant.GetWhitePoint(observer);
        var adaptedXyz = Adaptation.WhitePoint(xyz, sourceWhite, targetWhite, xyzConfig.AdaptationMatrix);
        var expectedXyz = Adaptation.WhitePoint(xyz, sourceWhite, targetWhite, new Matrix(expectedAdaptation));
        TestUtils.AssertTriplet(adaptedXyz.Triplet, expectedXyz.Triplet, 0);
    }
}