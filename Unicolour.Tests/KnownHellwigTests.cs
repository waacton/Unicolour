using NUnit.Framework;
using Wacton.Unicolour.Hellwig;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownHellwigTests
{
    private const double ModelTolerance = 0.00000000001;
    private const double UcsTolerance = 0.000000005; // different to model because RED test UCS values are rounded
    
    // [Test] TODO: determine and enable
    // public void Red()
    // {
    //     var whitePoint = new WhitePoint(0.9505, 1.0000, 1.0888);
    //     var xyz = new Xyz(0.20654008, 0.12197225, 0.05136952, whitePoint);
    //     var camConfig = new CamConfiguration(whitePoint, 318.31, 20, Surround.Average);
    //
    //     var expectedModel = new Model(
    //         j: 33.880368498111686,
    //         c: 69.444353357408033,
    //         h: 19.510887327451748,
    //         m: 72.18638534116765,
    //         s: 64.03612114840314,
    //         q: 176.03752758512178);
    //     
    //     var expectedUcs = new Ucs(
    //         j: 46.55542238,
    //         a: 40.22460974,
    //         b: 14.25288392);
    //
    //     AssertHellwig(xyz, camConfig, expectedModel, expectedUcs, "1B99R");
    // }
    //
    // [Test]
    // public void Blue()
    // {
    //     var whitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    //     var xyz = new Xyz(0.23446234045762356, 0.23897966766938545, 0.6049634765734733, whitePoint);
    //     var camConfig = new CamConfiguration(whitePoint, 40, 20, Surround.Average);
    //     
    //     var expectedModel = new Model(
    //         j: 45.54426472036036,
    //         c: 45.070010482937676,
    //         h: 259.225345298129,
    //         m: 39.41306078701033,
    //         s: 54.44320314132591,
    //         q: 132.96974182692048);
    //
    //     var expectedUcs = new Ucs(
    //         j: 58.70842551410342,
    //         a: -5.256862406480357,
    //         b: -27.623820831847528);
    //
    //     AssertHellwig(xyz, camConfig, expectedModel, expectedUcs, "89B11R");
    // }
    
    // when XYZ input is the same as CAM white point, i.e. fully white, and the white point is D65
    // J = 100 (full lightness), H ~= 209.5 & Hc = "34G66B" (D65 'white' in Hellwig has a slight blueness) 
    // regardless of the viewing conditions
    // [Test, Combinatorial]
    // public void White(
    //     [Values(1, 10, 100, 1000, 10000)] double lux, 
    //     [Values(1, 5, 20, 50, 100)] double background,
    //     [Values(Surround.Dark, Surround.Dim, Surround.Average)] Surround surround)
    // {
    //     var whitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    //     var camConfig = new CamConfiguration(whitePoint, CamConfiguration.LuxToLuminance(lux), background, surround);
    //     var (x, y, z) = whitePoint;
    //     var chromaticAdaptor = new ChromaticAdaptor(whitePoint, ChromaticAdaptation.Bradford);
    //     var xyz = new Xyz(x, y, z, whitePoint);
    //     var Hellwig = Cam.FromXyz(xyz, camConfig.WhitePoint, camConfig.ViewHellwig, chromaticAdaptor);
    //     var HellwigUcs = Ucs.FromCam(Hellwig);
    //     
    //     AssertModel(Hellwig.Model.J, 100);
    //     AssertModel(HellwigUcs.J, 100);
    //     Assert.That(Hellwig.Model.Hc, Is.EqualTo("34G66B"));
    //     Assert.That(Hellwig.Model.H, Is.EqualTo(209.5).Within(0.5));
    // }
    //
    // // when XYZ input is all zero, i.e. fully black
    // // all CAM properties = 0
    // // regardless of the viewing conditions
    // [Test]
    // public void Black(
    //     [Values(1, 10, 100, 1000, 10000)] double lux, 
    //     [Values(1, 5, 20, 50, 100)] double background,
    //     [Values(Surround.Dark, Surround.Dim, Surround.Average)] Surround surround)
    // {
    //     var whitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    //     var camConfig = new CamConfiguration(whitePoint, CamConfiguration.LuxToLuminance(lux), background, surround);
    //     var xyz = new Xyz(0, 0, 0, whitePoint);
    //     var expectedModel = new Model(0, 0, 0, 0, 0, 0);
    //     var expectedUcs = new Ucs(0, 0, 0);
    //     AssertHellwig(xyz, camConfig, expectedModel, expectedUcs, "20B80R");
    // }
    
    [Test]
    public void NegativeReflectJ()
    {
        var positive = Cam.NegativeReflectJ(q: 50, CamConfiguration.StandardRgb.ViewHellwig);
        var negative = Cam.NegativeReflectJ(q: -50, CamConfiguration.StandardRgb.ViewHellwig);
        Assert.That(positive, Is.EqualTo(-negative));
    }
    
    [Test]
    public void NegativeReflectC()
    {
        var positive = Cam.NegativeReflectC(m: 50, CamConfiguration.StandardRgb.ViewHellwig);
        var negative = Cam.NegativeReflectC(m: -50, CamConfiguration.StandardRgb.ViewHellwig);
        Assert.That(negative, Is.EqualTo(-positive));
    }
    
    [Test]
    public void NegativeReflectQ()
    {
        var positive = Cam.NegativeReflectQ(j: 50, CamConfiguration.StandardRgb.ViewHellwig);
        var negative = Cam.NegativeReflectQ(j: -50, CamConfiguration.StandardRgb.ViewHellwig);
        Assert.That(positive, Is.EqualTo(-negative));
    }

    [Test]
    public void NegativeReflectM1()
    {
        var positive = Cam.NegativeReflectM(c: 50, CamConfiguration.StandardRgb.ViewHellwig);
        var negative = Cam.NegativeReflectM(c: -50, CamConfiguration.StandardRgb.ViewHellwig);
        Assert.That(negative, Is.EqualTo(-positive));
    }
    
    [Test]
    public void NegativeReflectM2()
    {
        var positive = Cam.NegativeReflectM(s: 50, q: 50);
        var negative = Cam.NegativeReflectM(s: -50, q: 50);
        Assert.That(negative, Is.EqualTo(-positive));
        
        var doubleNegative = Cam.NegativeReflectM(s: -50, q: -50);
        Assert.That(doubleNegative, Is.EqualTo(positive));
    }
    
    [Test]
    public void NegativeReflectS()
    {
        var positive = Cam.NegativeReflectS(m: 50, q: 50);
        var negative = Cam.NegativeReflectS(m: 50, q: -50);
        Assert.That(negative, Is.EqualTo(-positive));
        
        var doubleNegative = Cam.NegativeReflectS(m: -50, q: -50);
        Assert.That(doubleNegative, Is.EqualTo(positive));
    }
    
    private static void AssertHellwig(Xyz xyz, CamConfiguration camConfig, Model expectedModel, Ucs expectedUcs, string expectedHueComposition)
    {
        var chromaticAdaptor = new ChromaticAdaptor(xyz.WhitePoint, ChromaticAdaptation.Bradford);
        var Hellwig = Cam.FromXyz(xyz, camConfig.WhitePoint, camConfig.ViewHellwig, chromaticAdaptor);
        var HellwigUcs = Ucs.FromCam(Hellwig);
        
        Assert.That(expectedModel.HueComposition, Is.EqualTo(expectedHueComposition));
        
        AssertModel(Hellwig.Model.Lightness, expectedModel.Lightness);
        AssertModel(Hellwig.Model.Chroma, expectedModel.Chroma);
        AssertModel(Hellwig.Model.HueAngle, expectedModel.HueAngle);
        AssertModel(Hellwig.Model.Colourfulness, expectedModel.Colourfulness);
        AssertModel(Hellwig.Model.Saturation, expectedModel.Saturation);
        AssertModel(Hellwig.Model.Brightness, expectedModel.Brightness);
        Assert.That(Hellwig.Model.HueComposition, Is.EqualTo(expectedModel.HueComposition));
        
        AssertModel(Hellwig.Model.J, expectedModel.Lightness);
        AssertModel(Hellwig.Model.C, expectedModel.Chroma);
        AssertModel(Hellwig.Model.H, expectedModel.HueAngle);
        AssertModel(Hellwig.Model.M, expectedModel.Colourfulness);
        AssertModel(Hellwig.Model.S, expectedModel.Saturation);
        AssertModel(Hellwig.Model.Q, expectedModel.Brightness);
        Assert.That(Hellwig.Model.Hc, Is.EqualTo(expectedModel.HueComposition));
        
        AssertModel(Hellwig.Model.Lightness, expectedModel.J);
        AssertModel(Hellwig.Model.Chroma, expectedModel.C);
        AssertModel(Hellwig.Model.HueAngle, expectedModel.H);
        AssertModel(Hellwig.Model.Colourfulness, expectedModel.M);
        AssertModel(Hellwig.Model.Saturation, expectedModel.S);
        AssertModel(Hellwig.Model.Brightness, expectedModel.Q);
        Assert.That(Hellwig.Model.HueComposition, Is.EqualTo(expectedModel.Hc));
        
        TestUtils.AssertTriplet(Hellwig.Jch, new Jch(expectedModel.J, expectedModel.C, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(Hellwig.Jmh, new Jmh(expectedModel.J, expectedModel.M, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(Hellwig.Jsh, new Jsh(expectedModel.J, expectedModel.S, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(Hellwig.Qch, new Qch(expectedModel.Q, expectedModel.C, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(Hellwig.Qmh, new Qmh(expectedModel.Q, expectedModel.M, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(Hellwig.Qsh, new Qsh(expectedModel.Q, expectedModel.S, expectedModel.H), ModelTolerance);
        TestUtils.AssertTriplet(HellwigUcs, expectedUcs, UcsTolerance);
    }
    
    private static void AssertModel(double actual, double expected) => Assert.That(actual, Is.EqualTo(expected).Within(ModelTolerance));
}