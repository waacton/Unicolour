namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public static class Cam16Tests
{
    [Test] // matching values from https://github.com/colour-science/colour#34colour-appearance-models---colourappearance
    public static void Red()
    {
        var xyz = new Xyz(0.20654008, 0.12197225, 0.05136952);
        var cam16Config = new Cam16Configuration(new WhitePoint(95.05, 100.00, 108.88), 318.31, 20, Surround.Average);

        var expectedModel = new Cam16Model(
            J: 33.880368498111686,
            C: 69.444353357408033,
            H: 19.510887327451748,
            M: 72.18638534116765,
            S: 64.03612114840314,
            Q: 176.03752758512178);
        
        var expectedUcs = new Cam16Ucs(
            J: 46.555422384892175,
            A: 40.22460973831151,
            B: 14.252883919504304);

        var expectedHueComposition = "1B99R";

        AssertCam16(xyz, cam16Config, expectedModel, expectedUcs, expectedHueComposition);
    }
    
    [Test] // matching values from https://observablehq.com/@jrus/cam16
    public static void Blue()
    {
        var xyz = new Xyz(0.23446234045762356, 0.23897966766938545, 0.6049634765734733);
        var cam16Config = new Cam16Configuration(WhitePoint.From(Illuminant.D65), 40, 20, Surround.Average);
        
        var expectedModel = new Cam16Model(
            J: 45.54426472036036,
            C: 45.070010482937676,
            H: 259.225345298129,
            M: 39.41306078701033,
            S: 54.44320314132591,
            Q: 132.96974182692048);

        var expectedUcs = new Cam16Ucs(
            J: 58.70842551410342,
            A: -5.256862406480357,
            B: -27.623820831847528);

        var expectedHueComposition = "89B11R";

        AssertCam16(xyz, cam16Config, expectedModel, expectedUcs, expectedHueComposition);
    }
    
    [Test, Combinatorial]
    public static void White(
        [Values(1, 10, 100, 1000, 10000)] double lux, 
        [Values(1, 5, 20, 50, 100)] double background,
        [Values(Surround.Dark, Surround.Dim, Surround.Average)] Surround surround)
    {
        // when XYZ input is the same as CAM16 white point, i.e. fully white, and the white point is D65
        // J = 100 (full lightness), H ~= 209.5 & Hc = "34G66B" (D65 'white' in CAM16 has a slight blueness) 
        // regardless of the viewing conditions
        var cam16Config = new Cam16Configuration(WhitePoint.From(Illuminant.D65), Cam16Configuration.LuxToLuminance(lux), background, surround);
        var (x, y, z) = cam16Config.WhitePoint.AsXyzMatrix().ToTriplet();
        var xyz = new Xyz(x, y, z);
        var xyzConfig = new XyzConfiguration(cam16Config.WhitePoint);
        var cam16 = Cam16.FromXyz(xyz, cam16Config, xyzConfig);
        
        AssertCam16Value(cam16.Model.J, 100);
        AssertCam16Value(cam16.Ucs.J, 100);
        AssertCam16Value(cam16.J, 100);
        AssertCam16String(cam16.Model.Hc, "34G66B");
        Assert.That(cam16.Model.H, Is.EqualTo(209.5).Within(0.5));
    }
    
    [Test]
    public static void Black(
        [Values(1, 10, 100, 1000, 10000)] double lux, 
        [Values(1, 5, 20, 50, 100)] double background,
        [Values(Surround.Dark, Surround.Dim, Surround.Average)] Surround surround)
    {
        // when XYZ input is all zero, i.e. fully black
        // all CAM16 properties = 0
        // regardless of the viewing conditions
        var cam16Config = new Cam16Configuration(WhitePoint.From(Illuminant.D65), Cam16Configuration.LuxToLuminance(lux), background, surround);
        var xyz = new Xyz(0, 0, 0);
        var expectedModel = new Cam16Model(0, 0, 0, 0, 0, 0);
        var expectedUcs = new Cam16Ucs(0, 0, 0);
        var expectedHueComposition = "20B80R";
        AssertCam16(xyz, cam16Config, expectedModel, expectedUcs, expectedHueComposition);
    }
    
    private static void AssertCam16(Xyz xyz, Cam16Configuration cam16Config, Cam16Model expectedModel, Cam16Ucs expectedUcs, string expectedHueComposition)
    {
        var xyzConfig = new XyzConfiguration(cam16Config.WhitePoint);
        var cam16 = Cam16.FromXyz(xyz, cam16Config, xyzConfig);
        AssertModel(cam16.Model, expectedModel, expectedHueComposition);
        AssertUcs(cam16, expectedUcs);
    }
    
    private static void AssertModel(Cam16Model actual, Cam16Model expected, string expectedHueComposition)
    {
        AssertCam16Value(actual.Lightness, expected.Lightness);
        AssertCam16Value(actual.Chroma, expected.Chroma);
        AssertCam16Value(actual.HueAngle, expected.HueAngle);
        AssertCam16Value(actual.Colourfulness, expected.Colourfulness);
        AssertCam16Value(actual.Saturation, expected.Saturation);
        AssertCam16Value(actual.Brightness, expected.Brightness);
        
        AssertCam16Value(actual.J, expected.Lightness);
        AssertCam16Value(actual.C, expected.Chroma);
        AssertCam16Value(actual.H, expected.HueAngle);
        AssertCam16Value(actual.M, expected.Colourfulness);
        AssertCam16Value(actual.S, expected.Saturation);
        AssertCam16Value(actual.Q, expected.Brightness);

        AssertCam16Value(actual.Lightness, expected.J);
        AssertCam16Value(actual.Chroma, expected.C);
        AssertCam16Value(actual.HueAngle, expected.H);
        AssertCam16Value(actual.Colourfulness, expected.M);
        AssertCam16Value(actual.Saturation, expected.S);
        AssertCam16Value(actual.Brightness, expected.Q);

        AssertCam16String(expected.HueComposition, expectedHueComposition);
        AssertCam16String(actual.HueComposition, expected.HueComposition);
        AssertCam16String(actual.Hc, expected.HueComposition);
        AssertCam16String(actual.HueComposition, expected.Hc);
    }
    
    private static void AssertUcs(Cam16 actual, Cam16Ucs expected)
    {
        AssertCam16Value(actual.Ucs.J, expected.J);
        AssertCam16Value(actual.Ucs.A, expected.A);
        AssertCam16Value(actual.Ucs.B, expected.B);
        
        AssertCam16Value(actual.J, expected.J);
        AssertCam16Value(actual.A, expected.A);
        AssertCam16Value(actual.B, expected.B);
    }

    private static void AssertCam16Value(double actual, double expected) => Assert.That(actual, Is.EqualTo(expected).Within(0.00000000001));
    private static void AssertCam16String(string actual, string expected) => Assert.That(actual, Is.EqualTo(expected));
}