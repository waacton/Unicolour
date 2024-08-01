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
        var xyz = new Xyz(0.1931, 0.2393, 0.1014);
        var camConfig = new CamConfiguration(new WhitePoint(98.88, 90, 32.03), 200, 18, Surround.Average);

        var expectedModel = new Model(
            J: 48.0314,
            C: 38.7789,
            H: 191.0452,
            M: 38.7789,
            S: 46.0177,
            Q: 183.1240);
        
        var expectedUcs = new Ucs(
            J: 61.1077,
            A: -27.2696,
            B: -5.3230);

        AssertCam02(xyz, camConfig, expectedModel, expectedUcs);
    }
    
    [Test] // matching values from https://github.com/igd-geo/pcolor/blob/master/de.fhg.igd.pcolor.test/src/de/fhg/igd/pcolor/test/CAMWorkedExample.java#L76
    public void Cyan20La()
    {
        var xyz = new Xyz(0.1931, 0.2393, 0.1014);
        var camConfig = new CamConfiguration(new WhitePoint(98.88, 90, 32.03), 20, 18, Surround.Average);
        
        var expectedModel = new Model(
            J: 47.6856,
            C: 36.0527,
            H: 185.3445,
            M: 29.7580,
            S: 51.1275,
            Q: 113.8401);
        
        var expectedUcs = new Ucs(
            J: 60.7779,
            A: -22.6157,
            B: -2.1157);

        AssertCam02(xyz, camConfig, expectedModel, expectedUcs);
    }
    
    [Test] // matching values from https://github.com/colour-science/colour#34colour-appearance-models---colourappearance & https://github.com/colour-science/colour#3126cam02-lcd-cam02-scd-and-cam02-ucs-colourspaces---luo-cui-and-li-2006
    public void Red()
    {
        var xyz = new Xyz(0.20654008, 0.12197225, 0.05136952);
        var camConfig = new CamConfiguration(new WhitePoint(95.05, 100.00, 108.88), 318.31, 20, Surround.Average);

        var expectedModel = new Model(
            J: 34.434525727858997,
            C: 67.365010921125943,
            H: 22.279164147957065,
            M: 70.024939419291414,
            S: 62.81485585332716,
            Q: 177.47124941102123);
        
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
        var camConfig = new CamConfiguration(Illuminant.D65.GetWhitePoint(Observer.Degree2), CamConfiguration.LuxToLuminance(lux), background, surround);
        var (x, y, z) = camConfig.WhitePoint.AsXyzMatrix().ToTriplet();
        var xyz = new Xyz(x, y, z);
        var xyzConfig = new XyzConfiguration(camConfig.WhitePoint);
        var cam02 = Cam02.FromXyz(xyz, camConfig, xyzConfig);
        
        AssertModelDouble(cam02.Model.J, 100);
        AssertModelDouble(cam02.Ucs.J, 100);
        AssertModelDouble(cam02.J, 100);
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
        var camConfig = new CamConfiguration(Illuminant.D65.GetWhitePoint(Observer.Degree2), CamConfiguration.LuxToLuminance(lux), background, surround);
        var xyz = new Xyz(0, 0, 0);
        var expectedModel = new Model(0, 0, 0, 0, 0, 0);
        var expectedUcs = new Ucs(0, 0, 0);
        AssertCam02(xyz, camConfig, expectedModel, expectedUcs);
    }
    
    private static void AssertCam02(Xyz xyz, CamConfiguration camConfig, Model expectedModel, Ucs expectedUcs)
    {
        var xyzConfig = new XyzConfiguration(camConfig.WhitePoint);
        var cam02 = Cam02.FromXyz(xyz, camConfig, xyzConfig);
        AssertModel(cam02.Model, expectedModel);
        AssertUcs(cam02, expectedUcs);
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
    
    private static void AssertUcs(Cam02 actual, Ucs expected)
    {
        AssertUcsDouble(actual.Ucs.J, expected.J);
        AssertUcsDouble(actual.Ucs.A, expected.A);
        AssertUcsDouble(actual.Ucs.B, expected.B);
        
        AssertUcsDouble(actual.J, expected.J);
        AssertUcsDouble(actual.A, expected.A);
        AssertUcsDouble(actual.B, expected.B);
    }

    private static void AssertModelDouble(double actual, double expected) => Assert.That(actual, Is.EqualTo(expected).Within(ModelTolerance));
    private static void AssertUcsDouble(double actual, double expected) => Assert.That(actual, Is.EqualTo(expected).Within(UcsTolerance));
}