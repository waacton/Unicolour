using NUnit.Framework;
using static Wacton.Unicolour.Cam;

namespace Wacton.Unicolour.Tests;

public class KnownCam02Tests
{
    // generally high tolerances because the CYAN test data is rounded
    private const double ModelTolerance = 0.00005;
    private const double UcsTolerance = 0.0001;
    
    [Test] // matching values from https://github.com/igd-geo/pcolor/blob/master/de.fhg.igd.pcolor.test/src/de/fhg/igd/pcolor/test/CAMWorkedExample.java#L54
    public void Cyan200La()
    {
        var whitePoint = new WhitePoint(0.9888, 0.90, 0.3203);
        var xyz = new Xyz(0.1931, 0.2393, 0.1014, whitePoint);
        var camConfig = new CamConfiguration(whitePoint, 200, 18, Surround.Average);

        var expectedModel = new Model(
            j: 48.0314,
            c: 38.7789,
            h: 191.0452,
            m: 38.7789,
            s: 46.0177,
            q: 183.1240);
        
        var expectedUcs = new Ucs(
            J: 61.1077,
            A: -27.2696,
            B: -5.3230);

        AssertCam02(xyz, camConfig, expectedModel, expectedUcs);
    }
    
    [Test] // matching values from https://github.com/igd-geo/pcolor/blob/master/de.fhg.igd.pcolor.test/src/de/fhg/igd/pcolor/test/CAMWorkedExample.java#L76
    public void Cyan20La()
    {
        var whitePoint = new WhitePoint(0.9888, 0.90, 0.3203);
        var xyz = new Xyz(0.1931, 0.2393, 0.1014, whitePoint);
        var camConfig = new CamConfiguration(whitePoint, 20, 18, Surround.Average);
        
        var expectedModel = new Model(
            j: 47.6856,
            c: 36.0527,
            h: 185.3445,
            m: 29.7580,
            s: 51.1275,
            q: 113.8401);
        
        var expectedUcs = new Ucs(
            J: 60.7779,
            A: -22.6157,
            B: -2.1157);

        AssertCam02(xyz, camConfig, expectedModel, expectedUcs);
    }
    
    [Test] // matching values from https://github.com/colour-science/colour#34colour-appearance-models---colourappearance & https://github.com/colour-science/colour#3126cam02-lcd-cam02-scd-and-cam02-ucs-colourspaces---luo-cui-and-li-2006
    public void Red()
    {
        var whitePoint = new WhitePoint(0.9505, 1.0000, 1.0888);
        var xyz = new Xyz(0.20654008, 0.12197225, 0.05136952, whitePoint);
        var camConfig = new CamConfiguration(whitePoint, 318.31, 20, Surround.Average);

        var expectedModel = new Model(
            j: 34.434525727858997,
            c: 67.365010921125943,
            h: 22.279164147957065,
            m: 70.024939419291414,
            s: 62.81485585332716,
            q: 177.47124941102123);
        
        var expectedUcs = new Ucs(
            J: 47.16899898,
            A: 38.72623785,
            B: 15.8663383);

        AssertCam02(xyz, camConfig, expectedModel, expectedUcs);
    }
    
    // when XYZ input is the same as CAM white point, i.e. fully white, and the white point is D65
    // J = 100 (full lightness)
    // regardless of the viewing conditions
    [Test, Combinatorial]
    public void White(
        [Values(1, 10, 100, 1000, 10000)] double lux, 
        [Values(1, 5, 20, 50, 100)] double background,
        [Values(Surround.Dark, Surround.Dim, Surround.Average)] Surround surround)
    {
        var whitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
        var camConfig = new CamConfiguration(whitePoint, CamConfiguration.LuxToLuminance(lux), background, surround);
        var (x, y, z) = whitePoint;
        var chromaticAdaptor = new ChromaticAdaptor(whitePoint, ChromaticAdaptation.Bradford);
        var xyz = new Xyz(x, y, z, whitePoint);
        var cam02 = Cam02.FromXyz(xyz, camConfig.WhitePoint, camConfig.View02, chromaticAdaptor);
        var cam02Ucs = Cam02Ucs.FromCam02(cam02);
        
        AssertModelDouble(cam02.Model.J, 100);
        AssertModelDouble(cam02Ucs.J, 100);
    }
    
    // when XYZ input is all zero, i.e. fully black
    // all CAM properties = 0
    // regardless of the viewing conditions
    [Test]
    public void Black(
        [Values(1, 10, 100, 1000, 10000)] double lux, 
        [Values(1, 5, 20, 50, 100)] double background,
        [Values(Surround.Dark, Surround.Dim, Surround.Average)] Surround surround)
    {
        var whitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
        var camConfig = new CamConfiguration(whitePoint, CamConfiguration.LuxToLuminance(lux), background, surround);
        var xyz = new Xyz(0, 0, 0, whitePoint);
        var expectedModel = new Model(0, 0, 0, 0, 0, 0);
        var expectedUcs = new Ucs(0, 0, 0);
        AssertCam02(xyz, camConfig, expectedModel, expectedUcs);
    }
    
    private static void AssertCam02(Xyz xyz, CamConfiguration camConfig, Model expectedModel, Ucs expectedUcs)
    {
        var chromaticAdaptor = new ChromaticAdaptor(xyz.WhitePoint, ChromaticAdaptation.Bradford);
        var cam02 = Cam02.FromXyz(xyz, camConfig.WhitePoint, camConfig.View02, chromaticAdaptor);
        var cam02Ucs = Cam02Ucs.FromCam02(cam02);
        AssertModel(cam02.Model, expectedModel);
        AssertUcs(cam02Ucs, expectedUcs);
    }
    
    private static void AssertModel(Model actual, Model expected)
    {
        AssertModelDouble(actual.Lightness, expected.Lightness);
        AssertModelDouble(actual.Chroma, expected.Chroma);
        AssertModelDouble(actual.HueAngle, expected.HueAngle);
        AssertModelDouble(actual.Colourfulness, expected.Colourfulness);
        AssertModelDouble(actual.Saturation, expected.Saturation);
        AssertModelDouble(actual.Brightness, expected.Brightness);
        
        AssertModelDouble(actual.J, expected.Lightness);
        AssertModelDouble(actual.C, expected.Chroma);
        AssertModelDouble(actual.H, expected.HueAngle);
        AssertModelDouble(actual.M, expected.Colourfulness);
        AssertModelDouble(actual.S, expected.Saturation);
        AssertModelDouble(actual.Q, expected.Brightness);

        AssertModelDouble(actual.Lightness, expected.J);
        AssertModelDouble(actual.Chroma, expected.C);
        AssertModelDouble(actual.HueAngle, expected.H);
        AssertModelDouble(actual.Colourfulness, expected.M);
        AssertModelDouble(actual.Saturation, expected.S);
        AssertModelDouble(actual.Brightness, expected.Q);
    }
    
    private static void AssertUcs(Cam02Ucs actual, Ucs expected)
    {
        AssertUcsDouble(actual.J, expected.J);
        AssertUcsDouble(actual.A, expected.A);
        AssertUcsDouble(actual.B, expected.B);
    }

    private static void AssertModelDouble(double actual, double expected) => Assert.That(actual, Is.EqualTo(expected).Within(ModelTolerance));
    private static void AssertUcsDouble(double actual, double expected) => Assert.That(actual, Is.EqualTo(expected).Within(UcsTolerance));
}