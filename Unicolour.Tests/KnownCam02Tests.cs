using NUnit.Framework;
using Wacton.Unicolour.Cam02;
using Wacton.Unicolour.Tests.Utils;

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
            j: 61.1077,
            a: -27.2696,
            b: -5.3230);

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
            j: 60.7779,
            a: -22.6157,
            b: -2.1157);

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
            j: 47.16899898,
            a: 38.72623785,
            b: 15.8663383);

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
        var cam02 = Cam02.Cam.FromXyz(xyz, camConfig.WhitePoint, camConfig.View02, chromaticAdaptor);
        var cam02Ucs = Ucs.FromCam02(cam02);
        
        AssertModel(cam02.Model.J, 100);
        AssertModel(cam02Ucs.J, 100);
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
    
    [Test]
    public void NegativeReflectJ()
    {
        var positive = Cam02.Cam.NegativeReflectJ(q: 50, CamConfiguration.StandardRgb.View02);
        var negative = Cam02.Cam.NegativeReflectJ(q: -50, CamConfiguration.StandardRgb.View02);
        Assert.That(positive, Is.EqualTo(-negative));
    }
    
    [Test]
    public void NegativeReflectC1()
    {
        var positive = Cam02.Cam.NegativeReflectC(m: 50, CamConfiguration.StandardRgb.View02);
        var negative = Cam02.Cam.NegativeReflectC(m: -50, CamConfiguration.StandardRgb.View02);
        Assert.That(negative, Is.EqualTo(-positive));
    }

    [Test]
    public void NegativeReflectQ()
    {
        var positive = Cam02.Cam.NegativeReflectQ(j: 50, CamConfiguration.StandardRgb.View02);
        var negative = Cam02.Cam.NegativeReflectQ(j: -50, CamConfiguration.StandardRgb.View02);
        Assert.That(positive, Is.EqualTo(-negative));
    }
    
    [Test]
    public void NegativeReflectC2()
    {
        var positive = Cam02.Cam.NegativeReflectC(s: 50, q: 50, CamConfiguration.StandardRgb.View02);
        var negative = Cam02.Cam.NegativeReflectC(s: -50, q: 50, CamConfiguration.StandardRgb.View02);
        Assert.That(negative, Is.EqualTo(-positive));
        
        var doubleNegative = Cam02.Cam.NegativeReflectC(s: -50, q: -50, CamConfiguration.StandardRgb.View02);
        Assert.That(doubleNegative, Is.EqualTo(positive));
    }
    
    [Test]
    public void NegativeReflectM()
    {
        var positive = Cam02.Cam.NegativeReflectM(c: 50, CamConfiguration.StandardRgb.View02);
        var negative = Cam02.Cam.NegativeReflectM(c: -50, CamConfiguration.StandardRgb.View02);
        Assert.That(negative, Is.EqualTo(-positive));
    }
    
    [Test]
    public void NegativeReflectS()
    {
        var positive = Cam02.Cam.NegativeReflectS(m: 50, q: 50);
        var negative = Cam02.Cam.NegativeReflectS(m: 50, q: -50);
        Assert.That(negative, Is.EqualTo(-positive));
        
        var doubleNegative = Cam02.Cam.NegativeReflectS(m: -50, q: -50);
        Assert.That(doubleNegative, Is.EqualTo(positive));
    }
    
    private static void AssertCam02(Xyz xyz, CamConfiguration camConfig, Model expectedModel, Ucs expectedUcs)
    {
        var chromaticAdaptor = new ChromaticAdaptor(xyz.WhitePoint, ChromaticAdaptation.Bradford);
        var cam02 = Cam02.Cam.FromXyz(xyz, camConfig.WhitePoint, camConfig.View02, chromaticAdaptor);
        var cam02Ucs = Ucs.FromCam02(cam02);
        
        AssertModel(cam02.Model.Lightness, expectedModel.Lightness);
        AssertModel(cam02.Model.Chroma, expectedModel.Chroma);
        AssertModel(cam02.Model.HueAngle, expectedModel.HueAngle);
        AssertModel(cam02.Model.Colourfulness, expectedModel.Colourfulness);
        AssertModel(cam02.Model.Saturation, expectedModel.Saturation);
        AssertModel(cam02.Model.Brightness, expectedModel.Brightness);
        Assert.That(cam02.Model.HueComposition, Is.EqualTo(expectedModel.HueComposition));
        
        AssertModel(cam02.Model.J, expectedModel.Lightness);
        AssertModel(cam02.Model.C, expectedModel.Chroma);
        AssertModel(cam02.Model.H, expectedModel.HueAngle);
        AssertModel(cam02.Model.M, expectedModel.Colourfulness);
        AssertModel(cam02.Model.S, expectedModel.Saturation);
        AssertModel(cam02.Model.Q, expectedModel.Brightness);
        Assert.That(cam02.Model.Hc, Is.EqualTo(expectedModel.HueComposition));

        AssertModel(cam02.Model.Lightness, expectedModel.J);
        AssertModel(cam02.Model.Chroma, expectedModel.C);
        AssertModel(cam02.Model.HueAngle, expectedModel.H);
        AssertModel(cam02.Model.Colourfulness, expectedModel.M);
        AssertModel(cam02.Model.Saturation, expectedModel.S);
        AssertModel(cam02.Model.Brightness, expectedModel.Q);
        Assert.That(cam02.Model.HueComposition, Is.EqualTo(expectedModel.Hc));
        
        TestUtils.AssertTriplet(cam02.Jch, new Jch(expectedModel.J, expectedModel.C, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam02.Jmh, new Jmh(expectedModel.J, expectedModel.M, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam02.Jsh, new Jsh(expectedModel.J, expectedModel.S, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam02.Qch, new Qch(expectedModel.Q, expectedModel.C, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam02.Qmh, new Qmh(expectedModel.Q, expectedModel.M, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam02.Qsh, new Qsh(expectedModel.Q, expectedModel.S, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam02Ucs, expectedUcs, UcsTolerance);
    }
    
    private static void AssertModel(double actual, double expected) => Assert.That(actual, Is.EqualTo(expected).Within(ModelTolerance));
}