using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ConfigureRgbTests
{
    // many of these test values come from sources that use slightly different white point values
    // which introduces differences during the XYZ chromatic adaptation to convert from one RGB config to another
    // as well as the RGB-to-XYZ matrix which is derived from the white point
    private const double Tolerance = 0.005;

    private static readonly Configuration DisplayP3 = new(RgbConfiguration.DisplayP3);
    private static readonly Configuration Rec2020 = new(RgbConfiguration.Rec2020);
    private static readonly Configuration Rec2100Pq = new(RgbConfiguration.Rec2100Pq);
    private static readonly Configuration Rec2100Hlg = new(RgbConfiguration.Rec2100Hlg);
    private static readonly Configuration A98 = new(RgbConfiguration.A98);
    private static readonly Configuration ProPhoto = new(RgbConfiguration.ProPhoto);
    private static readonly Configuration Aces20651 = new(RgbConfiguration.Aces20651);
    private static readonly Configuration Acescg = new(RgbConfiguration.Acescg);
    private static readonly Configuration Acescct = new(RgbConfiguration.Acescct);
    private static readonly Configuration Acescc = new(RgbConfiguration.Acescc);
    private static readonly Configuration Rec709 = new(RgbConfiguration.Rec709);
    private static readonly Configuration Pal625 = new(RgbConfiguration.Pal625);
    private static readonly Configuration Secam625 = new(RgbConfiguration.Secam625);
    private static readonly Configuration Ntsc = new(RgbConfiguration.Ntsc);
    private static readonly Configuration NtscSmpteC = new(RgbConfiguration.NtscSmpteC);
    
    /*
     * for generating test data http://www.brucelindbloom.com/ColorCalculator.html is a good place to start, especially for older RGB configs
     * no reliable reference data for Rec. 601 (625-line or 525-line), xvYCC, PAL/PAL-M/SECAM with Rec. 470 gamma (2.8), NTSC-525 with Rec. 2020 gamma
     * but they are at least covered by roundtrip tests
     */

    private static readonly TestCaseData[] ToDisplayP3Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.917488, 0.200287, 0.138561)),
        new((0.0, 1.0, 0.0), (0.458402, 0.985265, 0.298295)),
        new((0.0, 0.0, 1.0), (0.000000, 0.000000, 0.959588))
    ];
    
    private static readonly TestCaseData[] FromDisplayP3Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.093066, -0.226742, -0.150135)),
        new((0.0, 1.0, 0.0), (-0.511605, 1.018266, -0.310675)),
        new((0.0, 0.0, 1.0), (0.000000, 0.000000, 1.042022))
    ];
    
    [TestCaseSource(nameof(ToDisplayP3Data))]
    public void ToDisplayP3((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, DisplayP3, expected);
    
    [TestCaseSource(nameof(FromDisplayP3Data))]
    public void FromDisplayP3((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(DisplayP3, triplet, Configuration.Default, expected);

    private static readonly TestCaseData[] ToRec2020Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.791977, 0.230976, 0.073761)),
        new((0.0, 1.0, 0.0), (0.567542, 0.959279, 0.268969)),
        new((0.0, 0.0, 1.0), (0.168369, 0.051130, 0.946784))
    ];
    
    private static readonly TestCaseData[] FromRec2020Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.248220, -0.387908, -0.143514)),
        new((0.0, 1.0, 0.0), (-0.790375, 1.056302, -0.350164)),
        new((0.0, 0.0, 1.0), (-0.299213, -0.088640, 1.050489))
    ];
    
    [TestCaseSource(nameof(ToRec2020Data))]
    public void ToRec2020((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Rec2020, expected);
    
    [TestCaseSource(nameof(FromRec2020Data))]
    public void FromRec2020((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Rec2020, triplet, Configuration.Default, expected);
    
    // HDR space; test data comes from source using 203 white luminance, same as Unicolour default configuration (using DynamicRange.High)
    private static readonly TestCaseData[] ToRec2100PqData =
    [
        new((1.0, 1.0, 1.0), (0.580689, 0.580689, 0.580689)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.532546, 0.327023, 0.220069)),
        new((0.0, 1.0, 0.0), (0.468230, 0.571939, 0.347333)),
        new((0.0, 0.0, 1.0), (0.289648, 0.196811, 0.569194))
    ];
    
    // HDR space; test data comes from source using 203 white luminance, same as Unicolour default configuration (using DynamicRange.High)
    private static readonly TestCaseData[] FromRec2100PqData =
    [
        new((1.0, 1.0, 1.0), (5.296339, 5.296339, 5.296339)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (6.555399, -2.191586, -0.951936)),
        new((0.0, 1.0, 0.0), (-4.233044, 5.581925, -2.000135)),
        new((0.0, 0.0, 1.0), (-1.741695, -0.673595, 5.552439))
    ];
    
    [TestCaseSource(nameof(ToRec2100PqData))]
    public void ToRec2100Pq((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Rec2100Pq, expected);
    
    [TestCaseSource(nameof(FromRec2100PqData))]
    public void FromRec2100Pq((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Rec2100Pq, triplet, Configuration.Default, expected);
    
    // HDR space; test data comes from source using 203 white luminance, same as Unicolour default configuration (using DynamicRange.High)
    private static readonly TestCaseData[] ToRec2100HlgData =
    [
        new((1.0, 1.0, 1.0), (0.749991, 0.749991, 0.749991)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.655864, 0.234354, 0.114143)),
        new((0.0, 1.0, 0.0), (0.511362, 0.733444, 0.264494)),
        new((0.0, 0.0, 1.0), (0.185546, 0.095033, 0.728209))
    ];
    
    // HDR space; test data comes from source using 203 white luminance, same as Unicolour default configuration (using DynamicRange.High)
    private static readonly TestCaseData[] FromRec2100HlgData =
    [
        new((1.0, 1.0, 1.0), (1.779852, 1.779852, 1.779852)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (2.211555, -0.715303, -0.290255)),
        new((0.0, 1.0, 0.0), (-1.415272, 1.877773, -0.649659)),
        new((0.0, 0.0, 1.0), (-0.561046, -0.194818, 1.867663))
    ];
    
    [TestCaseSource(nameof(ToRec2100HlgData))]
    public void ToRec2100Hlg((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Rec2100Hlg, expected);
    
    [TestCaseSource(nameof(FromRec2100HlgData))]
    public void FromRec2100Hlg((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Rec2100Hlg, triplet, Configuration.Default, expected);

    private static readonly TestCaseData[] ToA98Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.858659, 0.000000, 0.000000)),
        new((0.0, 1.0, 0.0), (0.565053, 1.000000, 0.234567)),
        new((0.0, 0.0, 1.0), (-0.000000, -0.000000, 0.981071))
    ];
    
    private static readonly TestCaseData[] FromA98Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.158157, 0.000000, 0.000000)),
        new((0.0, 1.0, 0.0), (-0.663895, 1.000000, -0.229188)),
        new((0.0, 0.0, 1.0), (-0.000000, -0.000000, 1.018643))
    ];
    
    [TestCaseSource(nameof(ToA98Data))]
    public void ToA98((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, A98, expected);
    
    [TestCaseSource(nameof(FromA98Data))]
    public void FromA98((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(A98, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToProPhotoData =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.702299, 0.275734, 0.103574)),
        new((0.0, 1.0, 0.0), (0.540208, 0.927593, 0.304585)),
        new((0.0, 0.0, 1.0), (0.336222, 0.137634, 0.922854))
    ];
    
    private static readonly TestCaseData[] FromProPhotoData =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.363204, -0.515649, -0.090208)),
        new((0.0, 1.0, 0.0), (-0.868935, 1.095714, -0.427925)),
        new((0.0, 0.0, 1.0), (-0.589774, -0.037691, 1.068050))
    ];
    
    [TestCaseSource(nameof(ToProPhotoData))]
    public void ToProPhoto((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, ProPhoto, expected);
    
    [TestCaseSource(nameof(FromProPhotoData))]
    public void FromProPhoto((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(ProPhoto, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToAces20651Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.439633, 0.089776, 0.017541)),
        new((0.0, 1.0, 0.0), (0.382989, 0.813439, 0.111547)),
        new((0.0, 0.0, 1.0), (0.177378, 0.096784, 0.870912))
    ];
    
    private static readonly TestCaseData[] FromAces20651Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.496040, -0.562465, -0.130266)),
        new((0.0, 1.0, 0.0), (-1.05681, 1.148870, -0.427516)),
        new((0.0, 0.0, 1.0), (-0.655763, -0.342786, 1.070660))
    ];
    
    [TestCaseSource(nameof(ToAces20651Data))]
    public void ToAces20651((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Aces20651, expected);
    
    [TestCaseSource(nameof(FromAces20651Data))]
    public void FromAces20651((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Aces20651, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToAcescgData =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.613097, 0.070194, 0.020616)),
        new((0.0, 1.0, 0.0), (0.339523, 0.916354, 0.109570)),
        new((0.0, 0.0, 1.0), (0.047379, 0.013452, 0.869815))
    ];
    
    private static readonly TestCaseData[] FromAcescgData =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.262680, -0.396252, -0.168031)),
        new((0.0, 1.0, 0.0), (-0.810508, 1.059530, -0.394388)),
        new((0.0, 0.0, 1.0), (-0.319483, -0.103336, 1.064460))
    ];
    
    [TestCaseSource(nameof(ToAcescgData))]
    public void ToAcescg((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Acescg, expected);
    
    [TestCaseSource(nameof(FromAcescgData))]
    public void FromAcescg((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Acescg, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToAcescctData =
    [
        new((1.0, 1.0, 1.0), (0.554794, 0.554794, 0.554794)),
        new((0.0, 0.0, 0.0), (0.072906, 0.072906, 0.072906)),
        new((1.0, 0.0, 0.0), (0.514508, 0.336044, 0.235153)),
        new((0.0, 1.0, 0.0), (0.465844, 0.547601, 0.372712)),
        new((0.0, 0.0, 1.0), (0.303676, 0.200000, 0.543309))
    ];
    
    private static readonly TestCaseData[] FromAcescctData =
    [
        new((1.0, 1.0, 1.0), (9.981908, 9.981908, 9.981908)),
        new((0.0, 0.0, 0.0), (-0.077805, -0.077805, -0.077805)),
        new((1.0, 0.0, 0.0), (12.481015, -4.238535, -2.068013)),
        new((0.0, 1.0, 0.0), (-8.179428, 10.548242, -4.220806)),
        new((0.0, 0.0, 1.0), (-3.508300, -1.453216, 10.595220)),
    ];
    
    [TestCaseSource(nameof(ToAcescctData))]
    public void ToAcescct((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Acescct, expected);
    
    [TestCaseSource(nameof(FromAcescctData))]
    public void FromAcescct((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Acescct, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToAcesccData =
    [
        new((1.0, 1.0, 1.0), (0.554795, 0.554795, 0.554795)),
        new((0.0, 0.0, 0.0), (-0.358447, -0.358447, -0.358447)),
        new((1.0, 0.0, 0.0), (0.514508, 0.336044, 0.235153)),
        new((0.0, 1.0, 0.0), (0.465844, 0.547601, 0.372712)),
        new((0.0, 0.0, 1.0), (0.303676, 0.200000, 0.543309))
    ];
    
    private static readonly TestCaseData[] FromAcesccData =
    [
        new((1.0, 1.0, 1.0), (9.981908, 9.981908, 9.981908)),
        new((0.0, 0.0, 0.0), (0.015320, 0.015320, 0.015320)),
        new((1.0, 0.0, 0.0), (12.480937, -4.237970, -2.066642)),
        new((0.0, 1.0, 0.0), (-8.179103, 10.548223, -4.220239)),
        new((0.0, 0.0, 1.0), (-3.507598, -1.451031, 10.595198))
    ];
    
    [TestCaseSource(nameof(ToAcesccData))]
    public void ToAcescc((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Acescc, expected);
    
    [TestCaseSource(nameof(FromAcesccData))]
    public void FromAcescc((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Acescc, triplet, Configuration.Default, expected);
    
    // same primaries as sRGB
    private static readonly TestCaseData[] ToRec709Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.000000, 0.000000, 0.000000)),
        new((0.0, 1.0, 0.0), (0.000000, 1.000000, 0.000000)),
        new((0.0, 0.0, 1.0), (0.000000, 0.000000, 1.000000))
    ];
    
    // same primaries as sRGB
    private static readonly TestCaseData[] FromRec709Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.000000, 0.000000, 0.000000)),
        new((0.0, 1.0, 0.0), (0.000000, 1.000000, 0.000000)),
        new((0.0, 0.0, 1.0), (0.000000, 0.000000, 1.000000))
    ];
    
    [TestCaseSource(nameof(ToRec709Data))]
    public void ToRec709((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Rec709, expected);
    
    [TestCaseSource(nameof(FromRec709Data))]
    public void FromRec709((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Rec709, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToPal625Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.980603, 0.000000, 0.000000)),
        new((0.0, 1.0, 0.0), (0.237158, 1.000000, -0.13361)),
        new((0.0, 0.0, 1.0), (0.000000, 0.000000, 1.005408))
    ];
    
    private static readonly TestCaseData[] FromPal625Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.019114, 0.000000, 0.000000)),
        new((0.0, 1.0, 0.0), (-0.232190, 1.000000, 0.110885)),
        new((0.0, 0.0, 1.0), (0.000000, 0.000000, 0.994799))
    ];
    
    [TestCaseSource(nameof(ToPal625Data))]
    public void ToPal625((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Pal625, expected);
    
    [TestCaseSource(nameof(FromPal625Data))]
    public void FromPal625((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Pal625, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToSecam625Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.980603, 0.000000, 0.000000)),
        new((0.0, 1.0, 0.0), (0.237158, 1.000000, -0.13361)),
        new((0.0, 0.0, 1.0), (0.000000, 0.000000, 1.005408))
    ];
    
    private static readonly TestCaseData[] FromSecam625Data =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.019114, 0.000000, 0.000000)),
        new((0.0, 1.0, 0.0), (-0.232190, 1.000000, 0.110885)),
        new((0.0, 0.0, 1.0), (0.000000, 0.000000, 0.994799))
    ];
    
    [TestCaseSource(nameof(ToSecam625Data))]
    public void ToSecam625((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Secam625, expected);
    
    [TestCaseSource(nameof(FromSecam625Data))]
    public void FromSecam625((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Secam625, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToNtscData =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.838363, 0.154919, 0.160730)),
        new((0.0, 1.0, 0.0), (0.568205, 1.023253, 0.257641)),
        new((0.0, 0.0, 1.0), (0.212579, -0.295449, 0.968245))
    ];
    
    private static readonly TestCaseData[] FromNtscData =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.189250, -0.172290, -0.180047)),
        new((0.0, 1.0, 0.0), (-0.667681, 0.979572, -0.232316)),
        new((0.0, 0.0, 1.0), (-0.318069, 0.295358, 1.030718))
    ];
    
    [TestCaseSource(nameof(ToNtscData))]
    public void ToNtsc((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, Ntsc, expected);
    
    [TestCaseSource(nameof(FromNtscData))]
    public void FromNtsc((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Ntsc, triplet, Configuration.Default, expected);
    
    private static readonly TestCaseData[] ToNtscSmpteCData =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (1.029199, -0.167539, 0.054093)),
        new((0.0, 1.0, 0.0), (-0.268431, 1.016368, 0.085004)),
        new((0.0, 0.0, 1.0), (-0.123139, -0.155762, 0.997247))
    ];
    
    private static readonly TestCaseData[] FromNtscSmpteCData =
    [
        new((1.0, 1.0, 1.0), (1.000000, 1.000000, 1.000000)),
        new((0.0, 0.0, 0.0), (0.000000, 0.000000, 0.000000)),
        new((1.0, 0.0, 0.0), (0.972945, 0.141795, -0.020960)),
        new((0.0, 1.0, 0.0), (0.248235, 0.984811, -0.054685)),
        new((0.0, 0.0, 1.0), (0.101597, 0.135451, 1.002629)),
    ];
    
    [TestCaseSource(nameof(ToNtscSmpteCData))]
    public void ToNtscSmpteC((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(Configuration.Default, triplet, NtscSmpteC, expected);
    
    [TestCaseSource(nameof(FromNtscSmpteCData))]
    public void FromNtscSmpteC((double r, double g, double b) triplet, (double r, double g, double b) expected) => AssertConvertConfig(NtscSmpteC, triplet, Configuration.Default, expected);
    
    [Test]
    public void NotNumberFromStandardRgb([ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig)
        => AssertConvertConfig(Configuration.Default, (double.NaN, double.NaN, double.NaN), new(rgbConfig), (double.NaN, double.NaN, double.NaN));
    
    [Test]
    public void NotNumberToStandardRgb([ValueSource(typeof(TestUtils), nameof(TestUtils.NonDefaultRgbConfigs))] RgbConfiguration rgbConfig)
        => AssertConvertConfig(new(rgbConfig), (double.NaN, double.NaN, double.NaN), Configuration.Default, (double.NaN, double.NaN, double.NaN));
    
    private static void AssertConvertConfig(Configuration sourceConfig, (double r, double g, double b) triplet, Configuration targetConfig, (double r, double g, double b) expected)
    {
        var source = new Unicolour(sourceConfig, ColourSpace.Rgb, triplet);
        var target = source.ConvertToConfiguration(targetConfig);
        TestUtils.AssertTriplet<Rgb>(target, new(expected.r, expected.g, expected.b), Tolerance);
    }
    
    // example of an RGB model that is not predefined in Unicolour
    // https://en.wikipedia.org/wiki/Wide-gamut_RGB_color_space
    private static readonly RgbConfiguration WideGamutRgbConfig = new(
        new(0.7347, 0.2653),
        new(0.1152, 0.8264),
        new(0.1566, 0.0177),
        Illuminant.D50.GetWhitePoint(Observer.Degree2),
        value => Math.Pow(value, 1 / 2.19921875),
        value => Math.Pow(value, 2.19921875)
    );
    
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
        var xyzToRgbMatrix = RgbConfiguration.StandardRgb.RgbToXyzMatrix.Inverse();
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrixA).Within(0.0005));
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrixB).Within(0.0000001));
        
        var colourXyz = new Unicolour(Configuration.Default, ColourSpace.Xyz, 0.200757, 0.119618, 0.506757);
        var colourXyzNoConfig = new Unicolour(ColourSpace.Xyz, 0.200757, 0.119618, 0.506757);
        var colourLab = new Unicolour(Configuration.Default, ColourSpace.Lab, 41.1553, 51.4108, -56.4485);
        var colourLabNoConfig = new Unicolour(ColourSpace.Lab, 41.1553, 51.4108, -56.4485);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        TestUtils.AssertTriplet<Rgb>(colourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(colourXyzNoConfig, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(colourLab, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(colourLabNoConfig, expectedRgb, Tolerance);
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
        
        var rgbToXyzMatrix = RgbConfiguration.StandardRgb.RgbToXyzMatrix;
        rgbToXyzMatrix = Adaptation.WhitePoint(rgbToXyzMatrix, standardRgbConfig.WhitePoint, d50XyzConfig.WhitePoint, d50XyzConfig.AdaptationMatrix);
        var xyzToRgbMatrix = rgbToXyzMatrix.Inverse();
        Assert.That(xyzToRgbMatrix.Data, Is.EqualTo(expectedMatrix).Within(0.0000001));

        var colourXyz = new Unicolour(config, ColourSpace.Xyz, 0.187691, 0.115771, 0.381093);
        var colourLab = new Unicolour(config, ColourSpace.Lab, 40.5359, 46.0847, -57.1158);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        TestUtils.AssertTriplet<Rgb>(colourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(colourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD65ToAcesRgbD60()
    {
        var d65XyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(RgbConfiguration.Acescg, d65XyzConfig);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        var colourXyz = new Unicolour(config, ColourSpace.Xyz, 0.485665, 0.345912, 0.817454);
        var colourLab = new Unicolour(config, ColourSpace.Lab, 65.4291, 48.7467, -41.3660);
        TestUtils.AssertTriplet<Rgb>(colourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(colourLab, expectedRgb, Tolerance);
    }
    
    [Test]
    public void XyzD50ToAcesRgbD60()
    {
        var d50XyzConfig = new XyzConfiguration(Illuminant.D50, Observer.Degree2);
        var config = new Configuration(RgbConfiguration.Acescg, d50XyzConfig);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        var colourXyz = new Unicolour(config, ColourSpace.Xyz, 0.475850, 0.343035, 0.615342);
        var colourLab = new Unicolour(config, ColourSpace.Lab, 65.2028, 45.1028, -41.3650);
        TestUtils.AssertTriplet<Rgb>(colourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(colourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD65ToWideGamutRgbD50()
    {
        var d65XyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(WideGamutRgbConfig, d65XyzConfig);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        var colourXyz = new Unicolour(config, ColourSpace.Xyz, 0.251993, 0.102404, 0.550393);
        var colourLab = new Unicolour(config, ColourSpace.Lab, 38.2704, 87.2838, -65.7493);
        TestUtils.AssertTriplet<Rgb>(colourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(colourLab, expectedRgb, Tolerance);
    }

    [Test]
    public void XyzD50ToWideGamutRgbD50()
    {
        var d50XyzConfig = new XyzConfiguration(Illuminant.D50, Observer.Degree2);
        var config = new Configuration(WideGamutRgbConfig, d50XyzConfig);
        var expectedRgb = new ColourTriplet(0.5, 0.25, 0.75);
        var colourXyz = new Unicolour(config, ColourSpace.Xyz, 0.238795, 0.099490, 0.413181);
        var colourLab = new Unicolour(config, ColourSpace.Lab, 37.7508, 82.3084, -66.1402);
        TestUtils.AssertTriplet<Rgb>(colourXyz, expectedRgb, Tolerance);
        TestUtils.AssertTriplet<Rgb>(colourLab, expectedRgb, Tolerance);
    }
    
    [Test]
    public void XyzWhitePointRoundTrip([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant xyzIlluminant)
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