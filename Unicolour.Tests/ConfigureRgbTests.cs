namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ConfigureRgbTests
{
    private const double Tolerance = 0.005;
    
    private static readonly Configuration StandardRgbConfig = new(RgbConfiguration.StandardRgb);
    private static readonly Configuration DisplayP3Config = new(RgbConfiguration.DisplayP3);
    private static readonly Configuration Rec2020Config = new(RgbConfiguration.Rec2020);
    private static readonly Configuration A98Config = new(RgbConfiguration.A98);
    private static readonly Configuration ProPhotoConfig = new(RgbConfiguration.ProPhoto);

    // https://en.wikipedia.org/wiki/Academy_Color_Encoding_System
    private static readonly RgbConfiguration AcescgRgbConfig = new(
        new(0.713, 0.293),
        new(0.165, 0.830),
        new(0.128, 0.044), 
        new Chromaticity(0.32168, 0.33767).ToWhitePoint(1.0), // ACES not based on an illuminant
        value => value, // ACEScg is linear, no companding
        value => value
    );
    
    // https://en.wikipedia.org/wiki/Wide-gamut_RGB_color_space
    private static readonly RgbConfiguration WideGamutRgbConfig = new(
        new(0.7347, 0.2653),
        new(0.1152, 0.8264),
        new(0.1566, 0.0177), 
        Illuminant.D50.GetWhitePoint(Observer.Degree2),
        value => Companding.Gamma(value, 2.19921875),
        value => Companding.InverseGamma(value, 2.19921875)
    );
    
    private static readonly List<TestCaseData> StandardRgbLookup = new()
    {
        new TestCaseData((1.0, 0.0, 0.0), DisplayP3Config, (0.917488, 0.200287, 0.138561)).SetName("sRGB (Red) <-> Display-P3"),
        new TestCaseData((0.0, 1.0, 0.0), DisplayP3Config, (0.458402, 0.985265, 0.298295)).SetName("sRGB (Green) <-> Display-P3"),
        new TestCaseData((0.0, 0.0, 1.0), DisplayP3Config, (0.000000, 0.000000, 0.959588)).SetName("sRGB (Blue) <-> Display-P3"),
        new TestCaseData((1.093066, -0.226742, -0.150135), DisplayP3Config, (1.0, 0.0, 0.0)).SetName("sRGB <-> Display-P3 (Red)"), 
        new TestCaseData((-0.511605, 1.018266, -0.310675), DisplayP3Config, (0.0, 1.0, 0.0)).SetName("sRGB <-> Display-P3 (Green)"),
        new TestCaseData((0.000000, 0.000000, 1.042022), DisplayP3Config, (0.0, 0.0, 1.0)).SetName("sRGB <-> Display-P3 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), DisplayP3Config, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) <-> Display-P3 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), Rec2020Config, (0.791977, 0.230976, 0.073761)).SetName("sRGB (Red) <-> Rec. 2020"),
        new TestCaseData((0.0, 1.0, 0.0), Rec2020Config, (0.567542, 0.959279, 0.268969)).SetName("sRGB (Green) <-> Rec. 2020"),
        new TestCaseData((0.0, 0.0, 1.0), Rec2020Config, (0.168369, 0.051130, 0.946784)).SetName("sRGB (Blue) <-> Rec. 2020"),
        new TestCaseData((1.248220, -0.387908, -0.143514), Rec2020Config, (1.0, 0.0, 0.0)).SetName("sRGB <-> Rec. 2020 (Red)"), 
        new TestCaseData((-0.790375, 1.056302, -0.350164), Rec2020Config, (0.0, 1.0, 0.0)).SetName("sRGB <-> Rec. 2020 (Green)"),
        new TestCaseData((-0.299213, -0.088640, 1.050489), Rec2020Config, (0.0, 0.0, 1.0)).SetName("sRGB <-> Rec. 2020 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), Rec2020Config, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) <-> Rec. 2020 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), A98Config, (0.858659, 0.000000, 0.000000)).SetName("sRGB (Red) <-> A98"),
        new TestCaseData((0.0, 1.0, 0.0), A98Config, (0.565053, 1.000000, 0.234567)).SetName("sRGB (Green) <-> A98"),
        new TestCaseData((0.0, 0.0, 1.0), A98Config, (-0.000000, -0.000000, 0.981071)).SetName("sRGB (Blue) <-> A98"),
        new TestCaseData((1.158157, 0.000000, 0.000000), A98Config, (1.0, 0.0, 0.0)).SetName("sRGB <-> A98 (Red)"),
        new TestCaseData((-0.663895, 1.000000, -0.229188), A98Config, (0.0, 1.0, 0.0)).SetName("sRGB <-> A98 (Green)"),
        new TestCaseData((-0.000000, -0.000000, 1.018643), A98Config, (0.0, 0.0, 1.0)).SetName("sRGB <-> A98 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), A98Config, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) <-> A98 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), ProPhotoConfig, (0.702299, 0.275734, 0.103574)).SetName("sRGB (Red) <-> ProPhoto"),
        new TestCaseData((0.0, 1.0, 0.0), ProPhotoConfig, (0.540208, 0.927593, 0.304585)).SetName("sRGB (Green) <-> ProPhoto"),
        new TestCaseData((0.0, 0.0, 1.0), ProPhotoConfig, (0.336222, 0.137634, 0.922854)).SetName("sRGB (Blue) <-> ProPhoto"),
        new TestCaseData((1.363204, -0.515649, -0.090208), ProPhotoConfig, (1.0, 0.0, 0.0)).SetName("sRGB <-> ProPhoto (Red)"), 
        new TestCaseData((-0.868935, 1.095714, -0.427925), ProPhotoConfig, (0.0, 1.0, 0.0)).SetName("sRGB <-> ProPhoto (Green)"),
        new TestCaseData((-0.589774, -0.037691, 1.068050), ProPhotoConfig, (0.0, 0.0, 1.0)).SetName("sRGB <-> ProPhoto (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), ProPhotoConfig, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) <-> ProPhoto (NaN)")
    };
    
    [TestCaseSource(nameof(StandardRgbLookup))]
    public void StandardRgbToOtherModel((double r, double g, double b) standardTriplet, Configuration otherConfig, (double r, double g, double b) otherTriplet)
    {
        var standard = new Unicolour(StandardRgbConfig, ColourSpace.Rgb, standardTriplet);
        var other = standard.ConvertToConfiguration(otherConfig);
        TestUtils.AssertTriplet<Rgb>(other, new(otherTriplet.r, otherTriplet.g, otherTriplet.b), Tolerance);
    }
    
    [TestCaseSource(nameof(StandardRgbLookup))]
    public void OtherModelToStandardRgb((double r, double g, double b) standardTriplet, Configuration otherConfig, (double r, double g, double b) otherTriplet)
    {
        var other = new Unicolour(otherConfig, ColourSpace.Rgb, otherTriplet);
        var standard = other.ConvertToConfiguration(StandardRgbConfig);
        TestUtils.AssertTriplet<Rgb>(standard, new(standardTriplet.r, standardTriplet.g, standardTriplet.b), Tolerance);
    }
    
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
        var xyzToRgbLinearMatrix = RgbLinear.RgbLinearToXyzMatrix(RgbConfiguration.StandardRgb, XyzConfiguration.D65).Inverse();
        Assert.That(xyzToRgbLinearMatrix.Data, Is.EqualTo(expectedMatrixA).Within(0.0005));
        Assert.That(xyzToRgbLinearMatrix.Data, Is.EqualTo(expectedMatrixB).Within(0.0000001));
        
        var unicolourXyz = new Unicolour(Configuration.Default, ColourSpace.Xyz, 0.200757, 0.119618, 0.506757);
        var unicolourXyzNoConfig = new Unicolour(ColourSpace.Xyz, 0.200757, 0.119618, 0.506757);
        var unicolourLab = new Unicolour(Configuration.Default, ColourSpace.Lab, 41.1553, 51.4108, -56.4485);
        var unicolourLabNoConfig = new Unicolour(ColourSpace.Lab, 41.1553, 51.4108, -56.4485);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        TestUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(unicolourXyzNoConfig, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(unicolourLabNoConfig, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD50ToStandardRgbD65()
    {
        var standardRgbConfig = new RgbConfiguration(
            RgbModels.StandardRgb.R,
            RgbModels.StandardRgb.G,
            RgbModels.StandardRgb.B,
            RgbModels.StandardRgb.WhitePoint,
            RgbModels.StandardRgb.FromLinear,
            RgbModels.StandardRgb.ToLinear);
        var d50XyzConfig = new XyzConfiguration(Illuminant.D50, Observer.Degree2);
        var config = new Configuration(standardRgbConfig, d50XyzConfig);

        // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        var expectedMatrix = new[,]
        {
            { 3.1338561, -1.6168667, -0.4906146 },
            { -0.9787684, 1.9161415, 0.0334540 },
            { 0.0719453, -0.2289914, 1.4052427 }
        };
        
        var xyzToRgbLinearMatrix = RgbLinear.RgbLinearToXyzMatrix(config.Rgb, config.Xyz).Inverse();
        Assert.That(xyzToRgbLinearMatrix.Data, Is.EqualTo(expectedMatrix).Within(0.0000001));

        var unicolourXyz = new Unicolour(config, ColourSpace.Xyz, 0.187691, 0.115771, 0.381093);
        var unicolourLab = new Unicolour(config, ColourSpace.Lab, 40.5359, 46.0847, -57.1158);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        TestUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD65ToAcesRgbD60()
    {
        var d65XyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(AcescgRgbConfig, d65XyzConfig);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        var unicolourXyz = new Unicolour(config, ColourSpace.Xyz, 0.485665, 0.345912, 0.817454);
        var unicolourLab = new Unicolour(config, ColourSpace.Lab, 65.4291, 48.7467, -41.3660);
        TestUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }
    
    [Test]
    public void XyzD50ToAcesRgbD60()
    {
        var d50XyzConfig = new XyzConfiguration(Illuminant.D50, Observer.Degree2);
        var config = new Configuration(AcescgRgbConfig, d50XyzConfig);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        var unicolourXyz = new Unicolour(config, ColourSpace.Xyz, 0.475850, 0.343035, 0.615342);
        var unicolourLab = new Unicolour(config, ColourSpace.Lab, 65.2028, 45.1028, -41.3650);
        TestUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD65ToWideGamutRgbD50()
    {
        var d65XyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(WideGamutRgbConfig, d65XyzConfig);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        var unicolourXyz = new Unicolour(config, ColourSpace.Xyz, 0.251993, 0.102404, 0.550393);
        var unicolourLab = new Unicolour(config, ColourSpace.Lab, 38.2704, 87.2838, -65.7493);
        TestUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD50ToWideGamutRgbD50()
    {
        var d50XyzConfig = new XyzConfiguration(Illuminant.D50, Observer.Degree2);
        var config = new Configuration(WideGamutRgbConfig, d50XyzConfig);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        var unicolourXyz = new Unicolour(config, ColourSpace.Xyz, 0.238795, 0.099490, 0.413181);
        var unicolourLab = new Unicolour(config, ColourSpace.Lab, 37.7508, 82.3084, -66.1402);
        TestUtils.AssertTriplet<Rgb>(unicolourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(unicolourLab, expectedRgb, Tolerance);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllIlluminantsTestCases))]
    public void XyzWhitePointRoundTrip(Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(RgbConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974);
        var expectedRgb = RgbLinear.FromXyz(initialXyz, RgbConfiguration.StandardRgb, initialXyzConfig);

        var xyzConfig = new XyzConfiguration(xyzIlluminant, Observer.Degree2);
        var xyz = RgbLinear.ToXyz(expectedRgb, RgbConfiguration.StandardRgb, xyzConfig);
        var rgb = RgbLinear.FromXyz(xyz, RgbConfiguration.StandardRgb, xyzConfig);
        TestUtils.AssertTriplet(rgb.Triplet, expectedRgb.Triplet, 0.00000000001);
    }
}