namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using static Cam;

public class KnownCam16Tests
{
    private const double ModelTolerance = 0.00000000001;
    private const double UcsTolerance = 0.000000005; // different to model because RED test UCS values are rounded

    [Test] // matching values from https://github.com/colour-science/colour#34colour-appearance-models---colourappearance & https://github.com/colour-science/colour#3127cam16-lcd-cam16-scd-and-cam16-ucs-colourspaces---li-et-al-2017
    public void Red()
    {
        var xyz = new Xyz(0.20654008, 0.12197225, 0.05136952);
        var camConfig = new CamConfiguration(new WhitePoint(95.05, 100.00, 108.88), 318.31, 20, Surround.Average);

        var expectedModel = new Model(
            J: 33.880368498111686,
            C: 69.444353357408033,
            H: 19.510887327451748,
            M: 72.18638534116765,
            S: 64.03612114840314,
            Q: 176.03752758512178);
        
        var expectedUcs = new Ucs(
            J: 46.55542238,
            A: 40.22460974,
            B: 14.25288392);

        const string expectedHueComposition = "1B99R";
        AssertCam16(xyz, camConfig, expectedModel, expectedUcs, expectedHueComposition);
    }
    
    [Test] // matching values from https://observablehq.com/@jrus/cam16
    public void Blue()
    {
        var xyz = new Xyz(0.23446234045762356, 0.23897966766938545, 0.6049634765734733);
        var camConfig = new CamConfiguration(WhitePoint.From(Illuminant.D65), 40, 20, Surround.Average);
        
        var expectedModel = new Model(
            J: 45.54426472036036,
            C: 45.070010482937676,
            H: 259.225345298129,
            M: 39.41306078701033,
            S: 54.44320314132591,
            Q: 132.96974182692048);

        var expectedUcs = new Ucs(
            J: 58.70842551410342,
            A: -5.256862406480357,
            B: -27.623820831847528);

        const string expectedHueComposition = "89B11R";
        AssertCam16(xyz, camConfig, expectedModel, expectedUcs, expectedHueComposition);
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
        var camConfig = new CamConfiguration(WhitePoint.From(Illuminant.D65), CamConfiguration.LuxToLuminance(lux), background, surround);
        var (x, y, z) = camConfig.WhitePoint.AsXyzMatrix().ToTriplet();
        var xyz = new Xyz(x, y, z);
        var xyzConfig = new XyzConfiguration(camConfig.WhitePoint);
        var cam16 = Cam16.FromXyz(xyz, camConfig, xyzConfig);
        
        AssertModelDouble(cam16.Model.J, 100);
        AssertModelDouble(cam16.Ucs.J, 100);
        AssertModelDouble(cam16.J, 100);
        AssertString(cam16.Model.Hc, "34G66B");
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
        var camConfig = new CamConfiguration(WhitePoint.From(Illuminant.D65), CamConfiguration.LuxToLuminance(lux), background, surround);
        var xyz = new Xyz(0, 0, 0);
        var expectedModel = new Model(0, 0, 0, 0, 0, 0);
        var expectedUcs = new Ucs(0, 0, 0);
        const string expectedHueComposition = "20B80R";
        AssertCam16(xyz, camConfig, expectedModel, expectedUcs, expectedHueComposition);
    }
    
    private static void AssertCam16(Xyz xyz, CamConfiguration camConfig, Model expectedModel, Ucs expectedUcs, string expectedHueComposition)
    {
        var xyzConfig = new XyzConfiguration(camConfig.WhitePoint);
        var cam16 = Cam16.FromXyz(xyz, camConfig, xyzConfig);
        AssertModel(cam16.Model, expectedModel, expectedHueComposition);
        AssertUcs(cam16, expectedUcs);
    }
    
    private static void AssertModel(Model actual, Model expected, string expectedHueComposition)
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

        AssertString(expected.HueComposition, expectedHueComposition);
        AssertString(actual.HueComposition, expected.HueComposition);
        AssertString(actual.Hc, expected.HueComposition);
        AssertString(actual.HueComposition, expected.Hc);
    }
    
    private static void AssertUcs(Cam16 actual, Ucs expected)
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
    private static void AssertString(string actual, string expected) => Assert.That(actual, Is.EqualTo(expected));
}