using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Cam;

namespace Wacton.Unicolour.Tests;

public class KnownCam16Tests
{
    private const double ModelTolerance = 0.00000000001;
    private const double UcsTolerance = 0.000000005; // different to model because RED test UCS values are rounded

    [Test] // matching values from https://github.com/colour-science/colour#34colour-appearance-models---colourappearance & https://github.com/colour-science/colour#3127cam16-lcd-cam16-scd-and-cam16-ucs-colourspaces---li-et-al-2017
    public void Red()
    {
        var whitePoint = new WhitePoint(0.9505, 1.0000, 1.0888);
        var xyz = new Xyz(0.20654008, 0.12197225, 0.05136952, whitePoint);
        var camConfig = new CamConfiguration(whitePoint, 318.31, 20, Surround.Average);

        var expectedModel = new Model(
            j: 33.880368498111686,
            c: 69.444353357408033,
            h: 19.510887327451748,
            m: 72.18638534116765,
            s: 64.03612114840314,
            q: 176.03752758512178);
        
        var expectedUcs = new Cam16Ucs(
            j: 46.55542238,
            a: 40.22460974,
            b: 14.25288392);

        AssertCam16(xyz, camConfig, expectedModel, expectedUcs, "1B99R");
    }
    
    [Test] // matching values from https://observablehq.com/@jrus/cam16
    public void Blue()
    {
        var whitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
        var xyz = new Xyz(0.23446234045762356, 0.23897966766938545, 0.6049634765734733, whitePoint);
        var camConfig = new CamConfiguration(whitePoint, 40, 20, Surround.Average);
        
        var expectedModel = new Model(
            j: 45.54426472036036,
            c: 45.070010482937676,
            h: 259.225345298129,
            m: 39.41306078701033,
            s: 54.44320314132591,
            q: 132.96974182692048);

        var expectedUcs = new Cam16Ucs(
            j: 58.70842551410342,
            a: -5.256862406480357,
            b: -27.623820831847528);

        AssertCam16(xyz, camConfig, expectedModel, expectedUcs, "89B11R");
    }
    
    // when XYZ input is the same as CAM white point, i.e. fully white, and the white point is D65
    // J = 100 (full lightness), H ~= 209.5 & Hc = "34G66B" (D65 'white' in CAM16 has a slight blueness) 
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
        var cam16 = Cam16.FromXyz(xyz, camConfig.WhitePoint, camConfig.View16, chromaticAdaptor);
        var cam16Ucs = Cam16Ucs.FromCam16(cam16);
        
        AssertModel(cam16.Model.J, 100);
        AssertModel(cam16Ucs.J, 100);
        Assert.That(cam16.Model.Hc, Is.EqualTo("34G66B"));
        Assert.That(cam16.Model.H, Is.EqualTo(209.5).Within(0.5));
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
        var expectedUcs = new Cam16Ucs(0, 0, 0);
        AssertCam16(xyz, camConfig, expectedModel, expectedUcs, "20B80R");
    }
    
    [Test]
    public void NegativeReflectJ()
    {
        var positive = Cam16.NegativeReflectJ(q: 50, CamConfiguration.StandardRgb.View16);
        var negative = Cam16.NegativeReflectJ(q: -50, CamConfiguration.StandardRgb.View16);
        Assert.That(positive, Is.EqualTo(-negative));
    }
    
    [Test]
    public void NegativeReflectC1()
    {
        var positive = Cam16.NegativeReflectC(m: 50, CamConfiguration.StandardRgb.View16);
        var negative = Cam16.NegativeReflectC(m: -50, CamConfiguration.StandardRgb.View16);
        Assert.That(negative, Is.EqualTo(-positive));
    }
    
    [Test]
    public void NegativeReflectQ()
    {
        var positive = Cam16.NegativeReflectQ(j: 50, CamConfiguration.StandardRgb.View16);
        var negative = Cam16.NegativeReflectQ(j: -50, CamConfiguration.StandardRgb.View16);
        Assert.That(positive, Is.EqualTo(-negative));
    }
    
    [Test]
    public void NegativeReflectC2()
    {
        var positive = Cam16.NegativeReflectC(s: 50, q: 50, CamConfiguration.StandardRgb.View16);
        var negative = Cam16.NegativeReflectC(s: -50, q: 50, CamConfiguration.StandardRgb.View16);
        Assert.That(negative, Is.EqualTo(-positive));
        
        var doubleNegative = Cam16.NegativeReflectC(s: -50, q: -50, CamConfiguration.StandardRgb.View16);
        Assert.That(doubleNegative, Is.EqualTo(positive));
    }
    
    [Test]
    public void NegativeReflectM()
    {
        var positive = Cam16.NegativeReflectM(c: 50, CamConfiguration.StandardRgb.View16);
        var negative = Cam16.NegativeReflectM(c: -50, CamConfiguration.StandardRgb.View16);
        Assert.That(negative, Is.EqualTo(-positive));
    }
    
    [Test]
    public void NegativeReflectS()
    {
        var positive = Cam16.NegativeReflectS(m: 50, q: 50);
        var negative = Cam16.NegativeReflectS(m: 50, q: -50);
        Assert.That(negative, Is.EqualTo(-positive));
        
        var doubleNegative = Cam16.NegativeReflectS(m: -50, q: -50);
        Assert.That(doubleNegative, Is.EqualTo(positive));
    }
    
    private static void AssertCam16(Xyz xyz, CamConfiguration camConfig, Model expectedModel, Cam16Ucs expectedUcs, string expectedHueComposition)
    {
        var chromaticAdaptor = new ChromaticAdaptor(xyz.WhitePoint, ChromaticAdaptation.Bradford);
        var cam16 = Cam16.FromXyz(xyz, camConfig.WhitePoint, camConfig.View16, chromaticAdaptor);
        var cam16Ucs = Cam16Ucs.FromCam16(cam16);
        
        Assert.That(expectedModel.HueComposition, Is.EqualTo(expectedHueComposition));
        
        AssertModel(cam16.Model.Lightness, expectedModel.Lightness);
        AssertModel(cam16.Model.Chroma, expectedModel.Chroma);
        AssertModel(cam16.Model.HueAngle, expectedModel.HueAngle);
        AssertModel(cam16.Model.Colourfulness, expectedModel.Colourfulness);
        AssertModel(cam16.Model.Saturation, expectedModel.Saturation);
        AssertModel(cam16.Model.Brightness, expectedModel.Brightness);
        Assert.That(cam16.Model.HueComposition, Is.EqualTo(expectedModel.HueComposition));
        
        AssertModel(cam16.Model.J, expectedModel.Lightness);
        AssertModel(cam16.Model.C, expectedModel.Chroma);
        AssertModel(cam16.Model.H, expectedModel.HueAngle);
        AssertModel(cam16.Model.M, expectedModel.Colourfulness);
        AssertModel(cam16.Model.S, expectedModel.Saturation);
        AssertModel(cam16.Model.Q, expectedModel.Brightness);
        Assert.That(cam16.Model.Hc, Is.EqualTo(expectedModel.HueComposition));
        
        AssertModel(cam16.Model.Lightness, expectedModel.J);
        AssertModel(cam16.Model.Chroma, expectedModel.C);
        AssertModel(cam16.Model.HueAngle, expectedModel.H);
        AssertModel(cam16.Model.Colourfulness, expectedModel.M);
        AssertModel(cam16.Model.Saturation, expectedModel.S);
        AssertModel(cam16.Model.Brightness, expectedModel.Q);
        Assert.That(cam16.Model.HueComposition, Is.EqualTo(expectedModel.Hc));
        
        TestUtils.AssertTriplet(cam16.Jch, new Cam16Jch(expectedModel.J, expectedModel.C, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam16.Jmh, new Cam16Jmh(expectedModel.J, expectedModel.M, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam16.Jsh, new Cam16Jsh(expectedModel.J, expectedModel.S, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam16.Qch, new Cam16Qch(expectedModel.Q, expectedModel.C, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam16.Qmh, new Cam16Qmh(expectedModel.Q, expectedModel.M, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam16.Qsh, new Cam16Qsh(expectedModel.Q, expectedModel.S, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(cam16Ucs, expectedUcs, UcsTolerance);
    }
    
    private static void AssertModel(double actual, double expected) => Assert.That(actual, Is.EqualTo(expected).Within(ModelTolerance));
}