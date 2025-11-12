using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Hue;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownMunsellTests
{
    private static readonly XyzConfiguration XyzConfig = TestUtils.CConfig.Xyz;
    private static readonly Chromaticity WhiteChromaticity = XyzConfig.WhiteChromaticity;
    
    private static readonly TestCaseData[] AstmData =
    [
        new(new Xyy(0.2395, 0.2905, 59.53 / 100), new Munsell(3.9, "B", 8.11, 6.6)),
        new(new Xyy(0.2437, 0.3240, 21.98 / 100), new Munsell(5.6, "BG", 5.30, 5.3)),
        new(new Xyy(0.4183, 0.3790, 72.22 / 100), new Munsell(5.4, "YR", 8.78, 7.6)),
        new(new Xyy(0.4690, 0.4953, 50.30 / 100), new Munsell(5.6, "Y", 7.56, 13.7)),
        new(new Xyy(0.5000, 0.4540, 46.02 / 100), new Munsell(10, "YR", 7.2, 13.5))
    ];

    [TestCaseSource(nameof(AstmData))]
    public void Astm(Xyy xyy, Munsell expected)
    {
        // ASTM approach is to convert graphically, less accurate than the implemented algorithm
        var munsell = Munsell.FromXyy(xyy, XyzConfig);
        TestUtils.AssertTriplet(munsell.Triplet, expected.Triplet, [0.5, 0.1, 0.1]);
    }
    
    // raw Munsell luminance data is relative to reference white of smoked magnesium oxide (MgO)
    // CIE Y for illuminant C is ~0.975x MgO Y
    private const double MgoScale = 0.975;
    
    private static MunsellTestData[] XyyData = Munsell.Node.LookupCache.Values.Select(node => new MunsellTestData(node)).ToArray();
    [TestCaseSource(nameof(XyyData))]
    public void KnownXyy(MunsellTestData data)
    {
        var munsell = new Munsell(data.HueNumber, data.HueLetter, data.Value, data.Chroma);
        var munsellFromDegrees = new Munsell(FromMunsell(data.HueNumber, data.HueLetter), data.Value, data.Chroma);
        Assert.That(munsellFromDegrees, Is.EqualTo(munsell));

        // the Y value in the raw data is relative to MgO, and is not the correct luminance value
        // scale Ymgo to get the intended expected luminance Y for CIE illuminant C 
        var expectedLuminance = data.LuminanceMgo / 100 * MgoScale;
        
        var actual = Munsell.ToXyy(munsell, XyzConfig);
        Assert.That(actual.Chromaticity.X, Is.EqualTo(data.X).Within(1e-16));
        Assert.That(actual.Chromaticity.Y, Is.EqualTo(data.Y).Within(1e-16));
        Assert.That(actual.Luminance, Is.EqualTo(expectedLuminance).Within(0.00025));
    }
    
    private static readonly Configuration RgbDataConfig = new(xyzConfig: new XyzConfiguration(Illuminant.D65, Observer.Degree2, Cam02.MCAT02.Data));
    private static MunsellTestData[] RgbData = XyyData.Where(data => data.HasRgbMgoData).ToArray();
    
    [TestCaseSource(nameof(RgbData))]
    public void KnownRgb(MunsellTestData data)
    {
        var expectedRgb = data.RgbMgo!.Value;
        var munsell = new Munsell(data.HueNumber, data.HueLetter, data.Value, data.Chroma);
        var xyy = Munsell.ToXyy(munsell, XyzConfig);

        /*
         * to the best of my knowledge, the sRGB data for "real" colours provided by RIT aren't quite right
         * but can be matched with error < 0.01035 RGB (2.63925 in Byte255 format) when:
         * 1) using CIECAT02 chromatic adaptation transform as mentioned in the data notes
         * 2) incorrectly using raw Y MgO as the luminance instead of correcting to Y for illuminant C
         *    - this results in an exact match for X_C, Y_C, Z_C in the dataset
         *    - without this, error increases to < 0.0112 (2.856 in Byte255 format)
         */
        var colourIncorrectY = new Unicolour(RgbDataConfig, ColourSpace.Munsell, munsell.Tuple);
        var actualRgbIncorrectY = colourIncorrectY.Rgb.ConstrainedTriplet;
        TestUtils.AssertTriplet(actualRgbIncorrectY, new(expectedRgb.r, expectedRgb.g, expectedRgb.b), 0.0112);
        
        xyy = new Xyy(xyy.Chromaticity.X, xyy.Chromaticity.Y, xyy.Luminance / MgoScale);
        var xyzC = Xyy.ToXyz(xyy);
        var xyzD65 = Adaptation.WhitePoint(xyzC, Illuminant.C.GetWhitePoint(Observer.Degree2), Illuminant.D65.GetWhitePoint(Observer.Degree2), Cam02.MCAT02);
        var colour = new Unicolour(ColourSpace.Xyz, xyzD65.Tuple);
        var actualRgb = colour.Rgb.ConstrainedTriplet;
        TestUtils.AssertTriplet(actualRgb, new(expectedRgb.r, expectedRgb.g, expectedRgb.b), 0.01035); // tolerance of 2.63925 in byte255 format
    }
    
    // expecting similar results to https://www.andrewwerth.com/color/
    // though unclear what CAT was used (CIECAT02 gives closest match), if MgO Y was accounted for, was rounding used, is their code correct etc.
    // so naturally some tolerance
    private static readonly TestCaseData[] StandardRgbData =
    [
        new(new Munsell(5, "R", 5, 18), new Rgb255(243, 8, 61)),
        new(new Munsell(2.5, "YR", 6, 14), new Rgb255(237, 113, 10)),
        new(new Munsell(10, "Y", 9, 12), new Rgb255(242, 236, 2)),
        new(new Munsell(5, "GY", 9, 12), new Rgb255(207, 245, 41)),
        new(new Munsell(2.5, "G", 6, 10), new Rgb255(20, 171, 99)),
        new(new Munsell(2.5, "BG", 7, 8), new Rgb255(54, 195, 171)),
        new(new Munsell(7.5, "B", 6, 8), new Rgb255(45, 160, 198)),
        new(new Munsell(7.5, "PB", 3, 24), new Rgb255(59, 13, 215)),
        new(new Munsell(5, "P", 5, 26), new Rgb255(199, 3, 249)),
        new(new Munsell(10, "RP", 6, 14), new Rgb255(246, 97, 130)),
        new(new Munsell(9), new Rgb255(229, 229, 229)),
        new(new Munsell(5), new Rgb255(122, 122, 122)),
        new(new Munsell(1), new Rgb255(28, 28, 28))
    ];
    
    [TestCaseSource(nameof(StandardRgbData))]
    public void StandardRgbFromMunsell(Munsell munsell, Rgb255 expectedRgb)
    {
        var colour = new Unicolour(RgbDataConfig, ColourSpace.Munsell, munsell.Tuple);
        TestUtils.AssertTriplet<Rgb255>(colour, expectedRgb.Triplet, 2);
    } 
    
    [TestCaseSource(nameof(StandardRgbData))]
    public void StandardRgbToMunsell(Munsell expectedMunsell, Rgb255 rgb)
    {
        var colour = new Unicolour(RgbDataConfig, ColourSpace.Rgb255, rgb.Tuple);
        TestUtils.AssertTriplet<Munsell>(colour, expectedMunsell.Triplet, 0.25);
    }
    
    [Test]
    public void LuminanceMgoCorrection()
    {
        // first entry of Table 2 in ASTM https://doi.org/10.1520/D1535-14R18
        var munsell = new Munsell(2.5, "R", 9, 6);
        var expected = new Xyy(0.3665, 0.3183, 76.70 / 100.0);
        var actual = Munsell.ToXyy(munsell, XyzConfig);
        Assert.That(actual.Luminance, Is.EqualTo(expected.Luminance).Within(0.00005));
        
        // same node from raw data contains different value for Y, because it is relative to MgO white
        var expectedMgo = new Xyy(0.3665, 0.3183, 78.66 / 100.0);
        var rawMgo = XyyData.Single(data => data.Notation == munsell.ToString());
        var actualMgo = new Xyy(rawMgo.X, rawMgo.Y, rawMgo.LuminanceMgo / 100.0);
        Assert.That(actualMgo, Is.EqualTo(expectedMgo));
        Assert.That(actualMgo.Luminance, Is.GreaterThan(expected.Luminance));
        Assert.That(actualMgo.Luminance * MgoScale, Is.EqualTo(expected.Luminance).Within(0.0001));
    }
    
    private static readonly TestCaseData[] ValueToLuminanceData =
    [
        new(0, 0),
        new(0.2, 0.2311024478048),
        new(0.4, 0.4549364801536),
        new(0.6, 0.6815705093664),
        new(0.8, 0.9203492913152),
        new(1, 1.17992539),
        new(2, 3.04811648),
        new(3, 6.39117777),
        new(4, 11.70075136),
        new(5, 19.27184375),
        new(6, 29.30115264),
        new(7, 41.98539373),
        new(8, 57.61962752),
        new(9, 76.69558611),
        new(10, 100)
    ];

    [TestCaseSource(nameof(ValueToLuminanceData))]
    public void ValueToLuminance(double value, double expectedLuminance)
    {
        var y = Munsell.GetLuminance(value);
        Assert.That(y, Is.EqualTo(expectedLuminance / 100.0).Within(5e-16));
    }

    [TestCaseSource(nameof(ValueToLuminanceData))]
    public void LuminanceToValue(double expectedValue, double luminance)
    {
        var y = luminance / 100.0;
        var values = Enumerable.Range(0, 4).Select(i => Munsell.GetValue(y, iterationDepth: i)).ToList();
        
        Assert.That(values[0], Is.EqualTo(expectedValue).Within(Munsell.IterationDepthError[0]));
        Assert.That(values[1], Is.EqualTo(expectedValue).Within(Munsell.IterationDepthError[1]));
        Assert.That(values[2], Is.EqualTo(expectedValue).Within(Munsell.IterationDepthError[2]));
        Assert.That(values[3], Is.EqualTo(expectedValue).Within(Munsell.IterationDepthError[3]));
        
        Assert.That(Munsell.IterationDepthError[0], Is.EqualTo(0.0035));
        Assert.That(Munsell.IterationDepthError[1], Is.EqualTo(0.000005));
        Assert.That(Munsell.IterationDepthError[2], Is.EqualTo(0.00000000005));
        Assert.That(Munsell.IterationDepthError[3], Is.EqualTo(0.000000000000005));
    }

    private static TestCaseData[] RandomValueToLuminanceData = GetRandomValueToLuminanceData();
    private static TestCaseData[] GetRandomValueToLuminanceData()
    {
        var data = new TestCaseData[1000];
        for (var i = 0; i < data.Length; i++)
        {
            var v = TestUtils.RandomDouble(0, 10);
            var y = Munsell.GetLuminance(v);
            data[i] = new TestCaseData(v, y);
        }

        return data;
    }
    
    [TestCaseSource(nameof(RandomValueToLuminanceData))]
    public void LuminanceToValueRandom(double expectedValue, double luminance)
    {
        var v = Munsell.GetValue(luminance);
        Assert.That(v, Is.EqualTo(expectedValue).Within(Munsell.IterationDepthError[3]));
    }
    
    private static readonly TestCaseData[] LuminanceData =
    [
        new(0, new ColourTriplet(0, 0, 0)),
        new(-0.5, new ColourTriplet(0, 0, 0)),
        new(double.PositiveInfinity, new ColourTriplet(double.NaN, double.NaN, double.NaN)),
        new(double.NaN, new ColourTriplet(double.NaN, double.NaN, double.NaN))
    ];

    [TestCaseSource(nameof(LuminanceData))]
    public void Luminance(double luminance, ColourTriplet expected)
    {
        var xyy = new Xyy(0.4, 0.4, luminance);
        var munsell = Munsell.FromXyy(xyy, XyzConfig);
        TestUtils.AssertTriplet(munsell.Triplet, expected, 0);
    }
    
    [TestCase(double.PositiveInfinity, double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity, double.NegativeInfinity)]
    [TestCase(double.PositiveInfinity, double.NegativeInfinity)]
    [TestCase(double.NegativeInfinity, double.PositiveInfinity)]
    [TestCase(double.NaN, double.NaN)]
    public void Chromaticity(double x, double y)
    {
        var xyy = new Xyy(x, y, 0.5);
        var munsell = Munsell.FromXyy(xyy, XyzConfig);
        var expected = new ColourTriplet(double.NaN, Munsell.GetValue(0.5), double.NaN);
        TestUtils.AssertTriplet(munsell.Triplet, expected, 0);
    }
    
    [TestCase(double.NaN, "R")]
    [TestCase(2.5, "X")]
    [TestCase(7.5, "💩")]
    public void HueFromNotation(double hueNumber, string hueLetter)
    {
        var munsell = new Munsell(hueNumber, hueLetter, 5, 10);
        var xyy = Munsell.ToXyy(munsell, XyzConfig);
        var expected = new ColourTriplet(WhiteChromaticity.X, WhiteChromaticity.Y, Munsell.GetLuminance(5));
        TestUtils.AssertTriplet(xyy.Triplet, expected, 0);
    }
    
    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity)]
    public void HueFromDegrees(double hueDegrees)
    {
        var munsell = new Munsell(hueDegrees, 5, 10);
        var xyy = Munsell.ToXyy(munsell, XyzConfig);
        var expected = new ColourTriplet(WhiteChromaticity.X, WhiteChromaticity.Y, Munsell.GetLuminance(5));
        TestUtils.AssertTriplet(xyy.Triplet, expected, 0);
        Assert.That(munsell.HueNotation, Is.EqualTo((double.NaN, string.Empty)));
    }
    
    [TestCase(15, 10)]
    [TestCase(-5, 0)]
    [TestCase(double.PositiveInfinity, 10)]
    [TestCase(double.NegativeInfinity, 0)]
    [TestCase(double.Epsilon, double.Epsilon)]
    public void HueNumber(double hue, double expected)
    {
        var munsell = new Munsell(hue, "R", 4, 8);
        TestUtils.AssertTriplet(munsell.Triplet, new Munsell(expected, "R", 4, 8).Triplet, 0);
    }

    [TestCase("r", "R")]
    [TestCase("yr", "YR")]
    [TestCase("Yr", "YR")]
    [TestCase("yR", "YR")]
    [TestCase(" pb ", "PB")]
    [TestCase("\t\ng\n\t", "G")]
    public void HueLetter(string hue, string expected)
    {
        var munsell = new Munsell(3, hue, 6, 9);
        TestUtils.AssertTriplet(munsell.Triplet, new Munsell(3, expected, 6, 9).Triplet, 0);
    }
    
    private static readonly TestCaseData[] ValueData =
    [
        new(0, new ColourTriplet(WhiteChromaticity.X, WhiteChromaticity.Y, 0)),
        new(-5, new ColourTriplet(WhiteChromaticity.X, WhiteChromaticity.Y, 0)),
        new(11, new ColourTriplet(WhiteChromaticity.X, WhiteChromaticity.Y, Munsell.GetLuminance(11))),
        new(double.PositiveInfinity, new ColourTriplet(WhiteChromaticity.X, WhiteChromaticity.Y, double.NaN)), // no luminance without V
        new(double.NaN, new ColourTriplet(double.NaN, double.NaN, double.NaN)) // no luminance without V
    ];

    [TestCaseSource(nameof(ValueData))]
    public void Value(double value, ColourTriplet expected)
    {
        var munsell = new Munsell(10, "G", value, 5);
        var xyy = Munsell.ToXyy(munsell, XyzConfig);
        TestUtils.AssertTriplet(xyy.Triplet, expected, 0);
    }

    [Test]
    public void ValueOnly([Values(-5, -0.0000000001, 0, 0.0000000001, 5, 9, 10, 11)] double value)
    {
        var munsell = new Munsell(10, "G", value, 0);
        var xyy = Munsell.ToXyy(munsell, XyzConfig);

        var munsellValueOnly = new Munsell(value);
        var xyyValueOnly = Munsell.ToXyy(munsellValueOnly, XyzConfig);
        
        var expected = new ColourTriplet(WhiteChromaticity.X, WhiteChromaticity.Y, Munsell.GetLuminance(value));
        TestUtils.AssertTriplet(xyy.Triplet, expected, 0);
        TestUtils.AssertTriplet(xyyValueOnly.Triplet, expected, 0);
    }
    
    private static readonly TestCaseData[] ChromaData =
    [
        new(0, WhiteChromaticity),
        new(-5, WhiteChromaticity),
        new(double.PositiveInfinity, new Chromaticity(double.PositiveInfinity, double.NegativeInfinity)),
        new(double.NaN, new Chromaticity(double.NaN, double.NaN))
    ];

    [TestCaseSource(nameof(ChromaData))]
    public void Chroma(double chroma, Chromaticity expected)
    {
        var munsell = new Munsell(2.5, "R", 5, chroma);
        var xyy = Munsell.ToXyy(munsell, XyzConfig);
        TestUtils.AssertTriplet(xyy.Triplet, new ColourTriplet(expected.X, expected.Y, Munsell.GetLuminance(5)), 0);
    }
    
    [Test] // no delta from exact polar angle match could result in divide-by-zero
    public void ConvergeHueToExactPolarAngle()
    {
        var munsell = new Munsell(10, "G", 4, 6);
        var xyy = Munsell.ToXyy(munsell, XyzConfig);
        var polar = Polar(WhiteChromaticity, xyy.Chromaticity);
        Assert.DoesNotThrow(() => Munsell.ModifyHue(munsell, polar.angle));
    }

    [Test] // no delta from exact polar radius match could result in divide-by-zero
    public void ConvergeChromaToExactPolarRadius()
    {
        var munsell = new Munsell(10, "G", 4, 6);
        var xyy = Munsell.ToXyy(munsell, XyzConfig);
        var polar = Polar(WhiteChromaticity, xyy.Chromaticity);
        Assert.DoesNotThrow(() => Munsell.ModifyChroma(munsell, polar.radius));
    }
        
    [Test] // cannot converge, exits early instead of being stuck in loop
    public void ConvergeHueTooManyIterations() 
    {
        var munsell = new Munsell(10, "G", 4, 6);
        Munsell.ModifyHue(munsell, double.NaN);
        Assert.Pass();
    }
    
    [Test] // cannot converge, exits early instead of being stuck in loop
    public void ConvergeChromaTooManyIterations()
    {
        var munsell = new Munsell(10, "G", 4, 6);
        Munsell.ModifyChroma(munsell, double.NaN);
        Assert.Pass();
    }
    
    // very low luminance will converge to white point because there is no chroma data; no delta could result in divide-by-zero
    [TestCase(0)] 
    [TestCase(0.0000000000000000001)] 
    [TestCase(double.Epsilon)] 
    [TestCase(-double.Epsilon)] 
    public void ConvergeToWhitePoint(double luminance)
    {
        var xyy = new Xyy(0.4, 0.4, luminance);
        var munsell = Munsell.FromXyy(xyy, XyzConfig);
        Assert.That(munsell.IsGreyscale);
    }

    [Test]
    public void ConfigurationFromMunsell()
    {
        var configC = new Configuration(xyzConfig: new(Illuminant.C, Observer.Degree2));
        var colourC = new Unicolour(configC, ColourSpace.Munsell, FromMunsell(2.5, "R"), 9, 6);
        var colourD65 = new Unicolour(ColourSpace.Munsell, FromMunsell(2.5, "R"), 9, 6);
        
        TestUtils.AssertTriplet<Xyy>(colourC, new(0.3665, 0.3183, 0.7670), 0.00005);
        TestUtils.AssertTriplet<Xyy>(colourD65, new(0.3700, 0.3293, 0.7658), 0.00005);
    }
    
    [Test]
    public void ConfigurationToMunsell()
    {
        var configC = new Configuration(xyzConfig: new(Illuminant.C, Observer.Degree2));
        var colourC = new Unicolour(configC, ColourSpace.Xyy, 0.3665, 0.3183, 0.7670);
        var colourD65 = new Unicolour(ColourSpace.Xyy, 0.3700, 0.3293, 0.7658);
        
        TestUtils.AssertTriplet<Munsell>(colourC, new(FromMunsell(2.5, "R"), 9, 6), 0.0005);
        TestUtils.AssertTriplet<Munsell>(colourD65, new(FromMunsell(2.5, "R"), 9, 6), 0.005);
    }

    [TestCase(160, 200, 159, false)]
    [TestCase(160, 200, 160, true)]
    [TestCase(160, 200, 180, true)]
    [TestCase(160, 200, 200, true)]
    [TestCase(160, 200, 201, false)]
    [TestCase(340, 20, 0, true)]
    [TestCase(340, 20, 20, true)]
    [TestCase(340, 20, 21, false)]
    [TestCase(340, 20, 340, true)]
    [TestCase(340, 20, 339, false)]
    [TestCase(340, 20, 360, true)]
    public void IsBetween(double start, double end, double h, bool expected)
    {
        var isBetween = Hue.IsBetween(h, ToMunsell(end), ToMunsell(start));
        Assert.That(isBetween, Is.EqualTo(expected));
    }
    
    private static readonly TestCaseData[] RadialData = GetRadialData();
    [TestCaseSource(nameof(RadialData))]
    public void RadialInterpolation(int v, int c, (double h1, string h2) h, bool isRadial)
    {
        Assert.That(Munsell.IsOnRadialInterpolationHueSegment(v, c, FromMunsell(h.h1, h.h2)), Is.EqualTo(isRadial));
    }

    private static TestCaseData[] GetRadialData()
    {
        var data = new List<(int v, int c, (double h1, string h2) h, bool isRadial)>();
        data.Add(new(0, 5, ToMunsell(0), false));
        
        data.Add(new(1, 1, ToMunsell(0), false));
        data.AddRange(Get(1, 2, (10, "Y"), (5, "YR")));
        data.AddRange(Get(1, 2, (10, "Y"), (5, "YR")));
        data.AddRange(Get(1, 2, (5, "P"), (10, "BG")));
        data.AddRange(Get(1, 4, (7.5, "Y"), (2.5, "YR")));
        data.AddRange(Get(1, 4, (10, "PB"), (7.5, "BG")));
        data.AddRange(Get(1, 6, (10, "PB"), (5, "BG")));
        data.AddRange(Get(1, 8, (7.5, "PB"), (7.5, "B")));
        data.AddRange(Get(1, 10, (7.5, "PB"), (2.5, "PB")));
        
        data.Add(new(2, 1, ToMunsell(0), false));
        data.AddRange(Get(2, 2, (7.5, "Y"), (5, "YR")));
        data.AddRange(Get(2, 2, (10, "PB"), (7.5, "PB")));
        data.AddRange(Get(2, 4, (10, "Y"), (2.5, "YR")));
        data.AddRange(Get(2, 4, (10, "PB"), (2.5, "B")));
        data.AddRange(Get(2, 6, (2.5, "Y"), (7.5, "R")));
        data.AddRange(Get(2, 6, (10, "PB"), (2.5, "B")));
        data.AddRange(Get(2, 8, (5, "YR"), (7.5, "R")));
        data.AddRange(Get(2, 8, (10, "PB"), (10, "BG")));
        data.AddRange(Get(2, 10, (7.5, "PB"), (5, "B")));
        
        data.Add(new(3, 1, ToMunsell(0), false));
        data.AddRange(Get(3, 2, (7.5, "GY"), (10, "R")));
        data.AddRange(Get(3, 2, (5, "P"), (5, "B")));
        data.AddRange(Get(3, 4, (7.5, "GY"), (5, "R")));
        data.AddRange(Get(3, 4, (2.5, "PB"), (5, "BG")));
        data.AddRange(Get(3, 6, (7.5, "GY"), (7.5, "R")));
        data.AddRange(Get(3, 6, (2.5, "P"), (7.5, "BG")));
        data.AddRange(Get(3, 8, (7.5, "GY"), (7.5, "R")));
        data.AddRange(Get(3, 8, (2.5, "P"), (7.5, "BG")));
        data.AddRange(Get(3, 10, (7.5, "GY"), (7.5, "R")));
        data.AddRange(Get(3, 10, (2.5, "P"), (7.5, "BG")));
        data.AddRange(Get(3, 12, (2.5, "G"), (7.5, "R")));
        data.AddRange(Get(3, 12, (10, "PB"), (7.5, "BG")));
        
        data.Add(new(4, 1, ToMunsell(0), false));
        data.AddRange(Get(4, 2, (2.5, "G"), (7.5, "R")));
        data.AddRange(Get(4, 2, (5, "P"), (7.5, "BG")));
        data.AddRange(Get(4, 4, (2.5, "G"), (7.5, "R")));
        data.AddRange(Get(4, 4, (5, "P"), (7.5, "BG")));
        data.AddRange(Get(4, 6, (10, "GY"), (7.5, "R")));
        data.AddRange(Get(4, 6, (2.5, "P"), (7.5, "BG")));
        data.AddRange(Get(4, 8, (10, "GY"), (7.5, "R")));
        data.AddRange(Get(4, 8, (2.5, "P"), (7.5, "BG")));
        data.AddRange(Get(4, 10, (10, "GY"), (7.5, "R")));
        data.AddRange(Get(4, 10, (10, "PB"), (7.5, "BG")));
        
        data.Add(new(5, 1, ToMunsell(0), false));
        data.AddRange(Get(5, 2, (7.5, "GY"), (5, "R")));
        data.AddRange(Get(5, 2, (5, "P"), (5, "BG")));
        data.AddRange(Get(5, 4, (2.5, "G"), (2.5, "R")));
        data.AddRange(Get(5, 4, (5, "P"), (5, "BG")));
        data.AddRange(Get(5, 6, (2.5, "G"), (2.5, "R")));
        data.AddRange(Get(5, 6, (5, "P"), (5, "BG")));
        data.AddRange(Get(5, 8, (2.5, "G"), (2.5, "R")));
        data.AddRange(Get(5, 8, (5, "P"), (5, "BG")));
        data.AddRange(Get(5, 10, (2.5, "G"), (2.5, "R")));
        data.AddRange(Get(5, 10, (2.5, "P"), (5, "BG")));
        
        data.Add(new(6, 1, ToMunsell(0), false));
        data.AddRange(Get(6, 2, (7.5, "GY"), (5, "R")));
        data.AddRange(Get(6, 2, (7.5, "P"), (5, "BG")));
        data.AddRange(Get(6, 4, (7.5, "GY"), (5, "R")));
        data.AddRange(Get(6, 4, (7.5, "P"), (5, "BG")));
        data.AddRange(Get(6, 6, (2.5, "G"), (5, "R")));
        data.AddRange(Get(6, 6, (7.5, "P"), (7.5, "BG")));
        data.AddRange(Get(6, 8, (2.5, "G"), (5, "R")));
        data.AddRange(Get(6, 8, (5, "P"), (10, "BG")));
        data.AddRange(Get(6, 10, (2.5, "G"), (5, "R")));
        data.AddRange(Get(6, 10, (5, "P"), (10, "BG")));
        data.AddRange(Get(6, 12, (2.5, "G"), (5, "R")));
        data.AddRange(Get(6, 12, (2.5, "P"), (10, "BG")));
        data.AddRange(Get(6, 14, (2.5, "G"), (5, "R")));
        data.AddRange(Get(6, 14, (2.5, "P"), (10, "BG")));
        data.AddRange(Get(6, 16, (2.5, "G"), (5, "R")));
        data.AddRange(Get(6, 16, (10, "PB"), (10, "BG")));
        
        data.Add(new(7, 1, ToMunsell(0), false));
        data.AddRange(Get(7, 2, (2.5, "G"), (5, "R")));
        data.AddRange(Get(7, 2, (5, "P"), (10, "BG")));
        data.AddRange(Get(7, 4, (2.5, "G"), (5, "R")));
        data.AddRange(Get(7, 4, (5, "P"), (10, "BG")));
        data.AddRange(Get(7, 6, (2.5, "G"), (5, "R")));
        data.AddRange(Get(7, 6, (5, "P"), (10, "BG")));
        data.AddRange(Get(7, 8, (2.5, "G"), (5, "R")));
        data.AddRange(Get(7, 8, (2.5, "P"), (10, "BG")));
        data.AddRange(Get(7, 10, (5, "Y"), (5, "R")));
        data.AddRange(Get(7, 10, (2.5, "G"), (10, "Y")));
        data.AddRange(Get(7, 10, (2.5, "P"), (10, "BG")));
        data.AddRange(Get(7, 12, (7.5, "Y"), (7.5, "R")));
        data.AddRange(Get(7, 12, (2.5, "G"), (10, "Y")));
        data.AddRange(Get(7, 12, (2.5, "P"), (10, "PB")));
        data.AddRange(Get(7, 14, (5, "YR"), (7.5, "R")));
        data.AddRange(Get(7, 14, (10, "GY"), (2.5, "GY")));
        data.AddRange(Get(7, 14, (2.5, "P"), (10, "PB")));
        
        data.Add(new(8, 1, ToMunsell(0), false));
        data.AddRange(Get(8, 2, (10, "GY"), (5, "R")));
        data.AddRange(Get(8, 2, (5, "P"), (10, "BG")));
        data.AddRange(Get(8, 4, (10, "GY"), (5, "R")));
        data.AddRange(Get(8, 4, (5, "P"), (10, "BG")));
        data.AddRange(Get(8, 6, (10, "GY"), (5, "R")));
        data.AddRange(Get(8, 6, (5, "P"), (10, "BG")));
        data.AddRange(Get(8, 8, (10, "GY"), (5, "R")));
        data.AddRange(Get(8, 8, (5, "P"), (10, "BG")));
        data.AddRange(Get(8, 10, (10, "GY"), (5, "R")));
        data.AddRange(Get(8, 10, (5, "P"), (10, "BG")));
        data.AddRange(Get(8, 12, (10, "GY"), (5, "R")));
        data.AddRange(Get(8, 12, (5, "P"), (10, "BG")));
        data.AddRange(Get(8, 14, (5, "YR"), (5, "R")));
        data.AddRange(Get(8, 14, (10, "GY"), (2.5, "GY")));
        data.AddRange(Get(8, 14, (5, "P"), (10, "BG")));
        
        data.Add(new(9, 1, ToMunsell(0), false));
        data.AddRange(Get(9, 2, (10, "GY"), (5, "R")));
        data.AddRange(Get(9, 2, (10, "PB"), (5, "BG")));
        data.AddRange(Get(9, 4, (10, "GY"), (5, "R")));
        data.AddRange(Get(9, 4, (10, "PB"), (5, "R")));
        data.AddRange(Get(9, 6, (2.5, "G"), (5, "R")));
        data.AddRange(Get(9, 8, (2.5, "G"), (5, "R")));
        data.AddRange(Get(9, 10, (2.5, "G"), (5, "R")));
        data.AddRange(Get(9, 12, (2.5, "G"), (5, "R")));
        data.AddRange(Get(9, 14, (2.5, "G"), (5, "R")));
        data.AddRange(Get(9, 16, (2.5, "G"), (5, "GY")));
        
        data.Add(new(10, 1, ToMunsell(0), false));
        return data.Distinct().Select(x => new TestCaseData(x.v, x.c, x.h, x.isRadial)).ToArray();

        List<(int v, int c, (double h1, string h2) h, bool isRadial)> Get(int v, int c, (double h1, string h2) end, (double h1, string h2) start)
        {
            const double forward = Munsell.DegreesPerHueNumber / 4.0;
            const double none = 0;
            const double backward = -Munsell.DegreesPerHueNumber / 4.0;

            var endDegrees = FromMunsell(end.h1, end.h2);
            var startDegrees = FromMunsell(start.h1, start.h2);
            
            return
            [
                (v, c, Offset(endDegrees, forward), false),
                (v, c, Offset(endDegrees, none), true),
                (v, c, Offset(endDegrees, backward), true),
                (v, c, Offset(startDegrees, forward), true),
                (v, c, Offset(startDegrees, none), true),
                (v, c, Offset(startDegrees, backward), false)
            ];
            
            (double h1, string h2) Offset(double hue, double degrees) => ToMunsell((hue + degrees).Modulo(360));
        }
    }
}