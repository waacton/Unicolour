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
    
    // no reliable reference data for Rec. 601 (625-line or 525-line), xvYCC, PAL/PAL-M/SECAM with Rec. 470 gamma (2.8), NTSC-525 with Rec. 2020 gamma
    // but they are at least covered by roundtrip tests
    // TODO: white and black test values
    private static readonly TestCaseData[] StandardRgbLookup =
    [
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.DisplayP3, (0.917488, 0.200287, 0.138561)).SetName("sRGB (Red) ↔ Display-P3"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.DisplayP3, (0.458402, 0.985265, 0.298295)).SetName("sRGB (Green) ↔ Display-P3"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.DisplayP3, (0.000000, 0.000000, 0.959588)).SetName("sRGB (Blue) ↔ Display-P3"),
        new TestCaseData((1.093066, -0.226742, -0.150135), RgbConfiguration.DisplayP3, (1.0, 0.0, 0.0)).SetName("sRGB ↔ Display-P3 (Red)"), 
        new TestCaseData((-0.511605, 1.018266, -0.310675), RgbConfiguration.DisplayP3, (0.0, 1.0, 0.0)).SetName("sRGB ↔ Display-P3 (Green)"),
        new TestCaseData((0.000000, 0.000000, 1.042022), RgbConfiguration.DisplayP3, (0.0, 0.0, 1.0)).SetName("sRGB ↔ Display-P3 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.DisplayP3, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ Display-P3 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Rec2020, (0.791977, 0.230976, 0.073761)).SetName("sRGB (Red) ↔ Rec. 2020"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Rec2020, (0.567542, 0.959279, 0.268969)).SetName("sRGB (Green) ↔ Rec. 2020"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Rec2020, (0.168369, 0.051130, 0.946784)).SetName("sRGB (Blue) ↔ Rec. 2020"),
        new TestCaseData((1.248220, -0.387908, -0.143514), RgbConfiguration.Rec2020, (1.0, 0.0, 0.0)).SetName("sRGB ↔ Rec. 2020 (Red)"), 
        new TestCaseData((-0.790375, 1.056302, -0.350164), RgbConfiguration.Rec2020, (0.0, 1.0, 0.0)).SetName("sRGB ↔ Rec. 2020 (Green)"),
        new TestCaseData((-0.299213, -0.088640, 1.050489), RgbConfiguration.Rec2020, (0.0, 0.0, 1.0)).SetName("sRGB ↔ Rec. 2020 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Rec2020, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ Rec. 2020 (NaN)"),
        
        // HDR space; test data comes from source using 203 white luminance, same as Unicolour default configuration (using DynamicRange.High)
        new TestCaseData((1.0, 1.0, 1.0), RgbConfiguration.Rec2100Pq, (0.580689, 0.580689, 0.580689)).SetName("sRGB (White) ↔ Rec. 2100 PQ"),
        new TestCaseData((0.0, 0.0, 0.0), RgbConfiguration.Rec2100Pq, (0.000000, 0.000000, 0.000000)).SetName("sRGB (Black) ↔ Rec. 2100 PQ"),
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Rec2100Pq, (0.532546, 0.327023, 0.220069)).SetName("sRGB (Red) ↔ Rec. 2100 PQ"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Rec2100Pq, (0.468230, 0.571939, 0.347333)).SetName("sRGB (Green) ↔ Rec. 2100 PQ"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Rec2100Pq, (0.289648, 0.196811, 0.569194)).SetName("sRGB (Blue) ↔ Rec. 2100 PQ"),
        new TestCaseData((5.296339, 5.296339, 5.296339), RgbConfiguration.Rec2100Pq, (1.0, 1.0, 1.0)).SetName("sRGB ↔ Rec. 2100 PQ (White)"), 
        new TestCaseData((0.000000, 0.000000, 0.000000), RgbConfiguration.Rec2100Pq, (0.0, 0.0, 0.0)).SetName("sRGB ↔ Rec. 2100 PQ (Black)"), 
        new TestCaseData((6.555399, -2.191586, -0.951936), RgbConfiguration.Rec2100Pq, (1.0, 0.0, 0.0)).SetName("sRGB ↔ Rec. 2100 PQ (Red)"), 
        new TestCaseData((-4.233044, 5.581925, -2.000135), RgbConfiguration.Rec2100Pq, (0.0, 1.0, 0.0)).SetName("sRGB ↔ Rec. 2100 PQ (Green)"),
        new TestCaseData((-1.741695, -0.673595, 5.552439), RgbConfiguration.Rec2100Pq, (0.0, 0.0, 1.0)).SetName("sRGB ↔ Rec. 2100 PQ (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Rec2100Pq, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ Rec. 2100 PQ (NaN)"),
        
        // HDR space; test data comes from source using 203 white luminance, same as Unicolour default configuration (using DynamicRange.High)
        new TestCaseData((1.0, 1.0, 1.0), RgbConfiguration.Rec2100Hlg, (0.749991, 0.749991, 0.749991)).SetName("sRGB (White) ↔ Rec. 2100 HLG"),
        new TestCaseData((0.0, 0.0, 0.0), RgbConfiguration.Rec2100Hlg, (0.000000, 0.000000, 0.000000)).SetName("sRGB (Black) ↔ Rec. 2100 HLG"),
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Rec2100Hlg, (0.655864, 0.234354, 0.114143)).SetName("sRGB (Red) ↔ Rec. 2100 HLG"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Rec2100Hlg, (0.511362, 0.733444, 0.264494)).SetName("sRGB (Green) ↔ Rec. 2100 HLG"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Rec2100Hlg, (0.185546, 0.095033, 0.728209)).SetName("sRGB (Blue) ↔ Rec. 2100 HLG"),
        new TestCaseData((1.779852, 1.779852, 1.779852), RgbConfiguration.Rec2100Hlg, (1.0, 1.0, 1.0)).SetName("sRGB ↔ Rec. 2100 HLG (White)"), 
        new TestCaseData((0.000000, 0.000000, 0.000000), RgbConfiguration.Rec2100Hlg, (0.0, 0.0, 0.0)).SetName("sRGB ↔ Rec. 2100 HLG (Black)"), 
        new TestCaseData((2.211555, -0.715303, -0.290255), RgbConfiguration.Rec2100Hlg, (1.0, 0.0, 0.0)).SetName("sRGB ↔ Rec. 2100 HLG (Red)"), 
        new TestCaseData((-1.415272, 1.877773, -0.649659), RgbConfiguration.Rec2100Hlg, (0.0, 1.0, 0.0)).SetName("sRGB ↔ Rec. 2100 HLG (Green)"),
        new TestCaseData((-0.561046, -0.194818, 1.867663), RgbConfiguration.Rec2100Hlg, (0.0, 0.0, 1.0)).SetName("sRGB ↔ Rec. 2100 HLG (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Rec2100Hlg, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ Rec. 2100 HLG (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.A98, (0.858659, 0.000000, 0.000000)).SetName("sRGB (Red) ↔ A98"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.A98, (0.565053, 1.000000, 0.234567)).SetName("sRGB (Green) ↔ A98"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.A98, (-0.000000, -0.000000, 0.981071)).SetName("sRGB (Blue) ↔ A98"),
        new TestCaseData((1.158157, 0.000000, 0.000000), RgbConfiguration.A98, (1.0, 0.0, 0.0)).SetName("sRGB ↔ A98 (Red)"),
        new TestCaseData((-0.663895, 1.000000, -0.229188), RgbConfiguration.A98, (0.0, 1.0, 0.0)).SetName("sRGB ↔ A98 (Green)"),
        new TestCaseData((-0.000000, -0.000000, 1.018643), RgbConfiguration.A98, (0.0, 0.0, 1.0)).SetName("sRGB ↔ A98 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.A98, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ A98 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.ProPhoto, (0.702299, 0.275734, 0.103574)).SetName("sRGB (Red) ↔ ProPhoto"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.ProPhoto, (0.540208, 0.927593, 0.304585)).SetName("sRGB (Green) ↔ ProPhoto"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.ProPhoto, (0.336222, 0.137634, 0.922854)).SetName("sRGB (Blue) ↔ ProPhoto"),
        new TestCaseData((1.363204, -0.515649, -0.090208), RgbConfiguration.ProPhoto, (1.0, 0.0, 0.0)).SetName("sRGB ↔ ProPhoto (Red)"), 
        new TestCaseData((-0.868935, 1.095714, -0.427925), RgbConfiguration.ProPhoto, (0.0, 1.0, 0.0)).SetName("sRGB ↔ ProPhoto (Green)"),
        new TestCaseData((-0.589774, -0.037691, 1.068050), RgbConfiguration.ProPhoto, (0.0, 0.0, 1.0)).SetName("sRGB ↔ ProPhoto (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.ProPhoto, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ ProPhoto (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Aces20651, (0.43963, 0.08978, 0.01754)).SetName("sRGB (Red) ↔ ACES2065-1"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Aces20651, (0.38299, 0.81344, 0.11155)).SetName("sRGB (Green) ↔ ACES2065-1"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Aces20651, (0.17738, 0.09678, 0.87091)).SetName("sRGB (Blue) ↔ ACES2065-1"),
        new TestCaseData((1.49604, -0.56246, -0.13027), RgbConfiguration.Aces20651, (1.0, 0.0, 0.0)).SetName("sRGB ↔ ACES2065-1 (Red)"), 
        new TestCaseData((-1.05681, 1.14887, -0.42752), RgbConfiguration.Aces20651, (0.0, 1.0, 0.0)).SetName("sRGB ↔ ACES2065-1 (Green)"),
        new TestCaseData((-0.65576, -0.34279, 1.07066), RgbConfiguration.Aces20651, (0.0, 0.0, 1.0)).SetName("sRGB ↔ ACES2065-1 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Aces20651, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ ACES2065-1 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Acescg, (0.61310, 0.07019, 0.02062)).SetName("sRGB (Red) ↔ ACEScg"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Acescg, (0.33952, 0.91635, 0.10957)).SetName("sRGB (Green) ↔ ACEScg"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Acescg, (0.04738, 0.01345, 0.86981)).SetName("sRGB (Blue) ↔ ACEScg"),
        new TestCaseData((1.26268, -0.39625, -0.16803), RgbConfiguration.Acescg, (1.0, 0.0, 0.0)).SetName("sRGB ↔ ACEScg (Red)"), 
        new TestCaseData((-0.81051, 1.05953, -0.39439), RgbConfiguration.Acescg, (0.0, 1.0, 0.0)).SetName("sRGB ↔ ACEScg (Green)"),
        new TestCaseData((-0.31948, -0.10334, 1.06446), RgbConfiguration.Acescg, (0.0, 0.0, 1.0)).SetName("sRGB ↔ ACEScg (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Acescg, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ ACEScg (NaN)"),
            
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Acescct, (0.51451, 0.33604, 0.23515)).SetName("sRGB (Red) ↔ ACEScct"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Acescct, (0.46584, 0.54760, 0.37271)).SetName("sRGB (Green) ↔ ACEScct"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Acescct, (0.30368, 0.20000, 0.54331)).SetName("sRGB (Blue) ↔ ACEScct"),
        new TestCaseData((0.94593, -0.30309, -0.14919), RgbConfiguration.Acescct, (0.5, 0.0, 0.0)).SetName("sRGB ↔ ACEScct (Red)"), 
        new TestCaseData((-0.61043, 0.79024, -0.30175), RgbConfiguration.Acescct, (0.0, 0.5, 0.0)).SetName("sRGB ↔ ACEScct (Green)"),
        new TestCaseData((-0.24854, -0.11444, 0.79403), RgbConfiguration.Acescct, (0.0, 0.0, 0.5)).SetName("sRGB ↔ ACEScct (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Acescct, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ ACEScct (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Acescc, (0.51451, 0.33604, 0.23515)).SetName("sRGB (Red) ↔ ACEScc"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Acescc, (0.46584, 0.54760, 0.37271)).SetName("sRGB (Green) ↔ ACEScc"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Acescc, (0.30368, 0.20000, 0.54331)).SetName("sRGB (Blue) ↔ ACEScc"),
        new TestCaseData((0.94322, -0.28412, -0.10689), RgbConfiguration.Acescc, (0.5, 0.0, 0.0)).SetName("sRGB ↔ ACEScc (Red)"), 
        new TestCaseData((-0.59929, 0.78955, -0.28269), RgbConfiguration.Acescc, (0.0, 0.5, 0.0)).SetName("sRGB ↔ ACEScc (Green)"),
        new TestCaseData((-0.22522, -0.05314, 0.79329), RgbConfiguration.Acescc, (0.0, 0.0, 0.5)).SetName("sRGB ↔ ACEScc (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Acescc, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ ACEScc (NaN)"),
        
        // same chromaticity coordinates as sRGB
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Rec709, (1.0, 0.0, 0.0)).SetName("sRGB (Red) ↔ Rec. 709"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Rec709, (0.0, 1.0, 0.0)).SetName("sRGB (Green) ↔ Rec. 709"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Rec709, (0.0, 0.0, 1.0)).SetName("sRGB (Blue) ↔ Rec. 709"),
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Rec709, (1.0, 0.0, 0.0)).SetName("sRGB ↔ Rec. 709 (Red)"), 
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Rec709, (0.0, 1.0, 0.0)).SetName("sRGB ↔ Rec. 709 (Green)"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Rec709, (0.0, 0.0, 1.0)).SetName("sRGB ↔ Rec. 709 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Rec709, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ Rec. 709 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Pal625, (0.980603, 0.000000, 0.000000)).SetName("sRGB (Red) ↔ PAL 625"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Pal625, (0.237158, 1.000000, -0.133616)).SetName("sRGB (Green) ↔ PAL 625"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Pal625, (0.000000, 0.000000, 1.005408)).SetName("sRGB (Blue) ↔ PAL 625"),
        new TestCaseData((1.019114, 0.000000, 0.000000), RgbConfiguration.Pal625, (1.0, 0.0, 0.0)).SetName("sRGB ↔ PAL 625 (Red)"), 
        new TestCaseData((-0.232190, 1.000000, 0.110885), RgbConfiguration.Pal625, (0.0, 1.0, 0.0)).SetName("sRGB ↔ PAL 625 (Green)"),
        new TestCaseData((0.000000, 0.000000, 0.994799), RgbConfiguration.Pal625, (0.0, 0.0, 1.0)).SetName("sRGB ↔ PAL 625 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Pal625, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ PAL 625 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Secam625, (0.980603, 0.000000, 0.000000)).SetName("sRGB (Red) ↔ SECAM 625"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Secam625, (0.237158, 1.000000, -0.133616)).SetName("sRGB (Green) ↔ SECAM 625"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Secam625, (0.000000, 0.000000, 1.005408)).SetName("sRGB (Blue) ↔ SECAM 625"),
        new TestCaseData((1.019114, 0.000000, 0.000000), RgbConfiguration.Secam625, (1.0, 0.0, 0.0)).SetName("sRGB ↔ SECAM 625 (Red)"), 
        new TestCaseData((-0.232190, 1.000000, 0.110885), RgbConfiguration.Secam625, (0.0, 1.0, 0.0)).SetName("sRGB ↔ SECAM 625 (Green)"),
        new TestCaseData((0.000000, 0.000000, 0.994799), RgbConfiguration.Secam625, (0.0, 0.0, 1.0)).SetName("sRGB ↔ SECAM 625 (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Secam625, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ SECAM 625 (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.Ntsc, (0.838363, 0.154919, 0.160730)).SetName("sRGB (Red) ↔ NTSC"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.Ntsc, (0.568205, 1.023253, 0.257641)).SetName("sRGB (Green) ↔ NTSC"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.Ntsc, (0.212579, -0.295449, 0.968245)).SetName("sRGB (Blue) ↔ NTSC"),
        new TestCaseData((1.189250, -0.172290, -0.180047), RgbConfiguration.Ntsc, (1.0, 0.0, 0.0)).SetName("sRGB ↔ NTSC (Red)"), 
        new TestCaseData((-0.667681, 0.979572, -0.232316), RgbConfiguration.Ntsc, (0.0, 1.0, 0.0)).SetName("sRGB ↔ NTSC (Green)"),
        new TestCaseData((-0.318069, 0.295358, 1.030718), RgbConfiguration.Ntsc, (0.0, 0.0, 1.0)).SetName("sRGB ↔ NTSC (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.Ntsc, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ NTSC (NaN)"),
        
        new TestCaseData((1.0, 0.0, 0.0), RgbConfiguration.NtscSmpteC, (1.029199, -0.167539, 0.054093)).SetName("sRGB (Red) ↔ NTSC SMPTE-C"),
        new TestCaseData((0.0, 1.0, 0.0), RgbConfiguration.NtscSmpteC, (-0.268431, 1.016368, 0.085004)).SetName("sRGB (Green) ↔ NTSC SMPTE-C"),
        new TestCaseData((0.0, 0.0, 1.0), RgbConfiguration.NtscSmpteC, (-0.123139, -0.155762, 0.997247)).SetName("sRGB (Blue) ↔ NTSC SMPTE-C"),
        new TestCaseData((0.972945, 0.141795, -0.020960), RgbConfiguration.NtscSmpteC, (1.0, 0.0, 0.0)).SetName("sRGB ↔ NTSC SMPTE-C (Red)"), 
        new TestCaseData((0.248235, 0.984811, -0.054685), RgbConfiguration.NtscSmpteC, (0.0, 1.0, 0.0)).SetName("sRGB ↔ NTSC SMPTE-C (Green)"),
        new TestCaseData((0.101597, 0.135451, 1.002629), RgbConfiguration.NtscSmpteC, (0.0, 0.0, 1.0)).SetName("sRGB ↔ NTSC SMPTE-C (Blue)"),
        new TestCaseData((double.NaN, double.NaN, double.NaN), RgbConfiguration.NtscSmpteC, (double.NaN, double.NaN, double.NaN)).SetName("sRGB (NaN) ↔ NTSC SMPTE-C (NaN)")
    ];

    [TestCaseSource(nameof(StandardRgbLookup))]
    public void StandardRgbToOtherModel((double r, double g, double b) standardTriplet, RgbConfiguration rgbConfig, (double r, double g, double b) otherTriplet)
    {
        var standard = new Unicolour(Configuration.Default, ColourSpace.Rgb, standardTriplet);
        var otherConfig = new Configuration(rgbConfig);
        var other = standard.ConvertToConfiguration(otherConfig);
        
        // the enormous magnitude of sRGB when converted from HDR RGB spaces (e.g. white luminance of 203 out of a possible 10,000)
        // lead to the very small differences of chosen white point values becoming hugely exaggerated (see Illuminant.cs for more discussion) 
        var tolerance = rgbConfig == RgbConfiguration.Rec2100Pq || rgbConfig == RgbConfiguration.Rec2100Hlg ? 0.15 : Tolerance;
        TestUtils.AssertTriplet<Rgb>(other, new(otherTriplet.r, otherTriplet.g, otherTriplet.b), tolerance);
    }
    
    [TestCaseSource(nameof(StandardRgbLookup))]
    public void OtherModelToStandardRgb((double r, double g, double b) standardTriplet, RgbConfiguration rgbConfig, (double r, double g, double b) otherTriplet)
    {
        var otherConfig = new Configuration(rgbConfig);
        var other = new Unicolour(otherConfig, ColourSpace.Rgb, otherTriplet);
        var standard = other.ConvertToConfiguration(Configuration.Default);
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