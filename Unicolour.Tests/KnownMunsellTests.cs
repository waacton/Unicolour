using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownMunsellTests
{
    private static MunsellTestData[] XyyData = Experimental.Munsell.Nodes.Value.Select(node => new MunsellTestData(node)).ToArray();
    private static MunsellTestData[] RgbData = XyyData.Where(data => data.HasRgbMgoData).ToArray();
    
    // TODO: test extreme values? are values beyond 0 - 10 supported? clamped?
    private static readonly TestCaseData[] LuminanceTestData =
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
        new(10, 100),
        new(0.5, 0.5673028559375) // doesn't appear in the dataset but is reasonable
    ];

    private static TestCaseData[] RandomLuminanceTestData = new TestCaseData[1000];

    static KnownMunsellTests()
    {
        for (var i = 0; i < RandomLuminanceTestData.Length; i++)
        {
            var v = TestUtils.RandomDouble(0, 10);
            var y = Munsell.GetLuminance(v);
            RandomLuminanceTestData[i] = new TestCaseData(v, y);
        }
    }
    
    // raw Munsell luminance data is relative to reference white of smoked magnesium oxide (MgO)
    // CIE Y for illuminant C is ~0.975x MgO Y
    private const double MgoScale = 0.975;
    
    [TestCaseSource(nameof(XyyData))]
    public void KnownXyy(MunsellTestData data)
    {
        var munsell = new Munsell(data.HueNumber, data.HueLetter, data.Value, data.Chroma);
        var munsellFromDegrees = new Munsell(MunsellUtils.ToDegrees(data.HueNumber, data.HueLetter), data.Value, data.Chroma);
        Assert.That(munsellFromDegrees, Is.EqualTo(munsell));

        // the Y value in the raw data is relative to MgO, and is not the correct luminance value
        // scale Ymgo to get the intended expected luminance Y for CIE illuminant C 
        var expectedLuminance = data.LuminanceMgo / 100 * MgoScale;
        
        var actual = Munsell.ToXyy(munsell);
        Assert.That(actual.Chromaticity.X, Is.EqualTo(data.X).Within(1e-16));
        Assert.That(actual.Chromaticity.Y, Is.EqualTo(data.Y).Within(1e-16));
        Assert.That(actual.Luminance, Is.EqualTo(expectedLuminance).Within(0.00025));
    }
    
    [TestCaseSource(nameof(RgbData))]
    public void KnownRgb(MunsellTestData data)
    {
        var munsell = new Munsell(data.HueNumber, data.HueLetter, data.Value, data.Chroma);
        var xyy = Munsell.ToXyy(munsell);

        /*
         * to the best of my knowledge, the sRGB data for "real" colours provided by RIT aren't quite right
         * but can be matched with error < 0.01 RGB (2.55 in Byte255 format) when:
         * 1) incorrectly using raw Y MgO as the luminance instead of correcting to Y for illuminant C
         *    - this results in an exact match for X_C, Y_C, Z_C in the dataset
         *    - without this, error increases to < 0.0114 (2.9 in Byte255 format)
         * 2) questionably using XYZ values rounded to 4 decimal places
         *    - without this but using Y MgO, error reduces to < 0.01033 (~2.63 in Byte255 format)
         * 3) using CIECAT02 chromatic adaptation transform as mentioned in the data notes
         */
        xyy = new Xyy(xyy.Chromaticity.X, xyy.Chromaticity.Y, xyy.Luminance / MgoScale);
        var xyzC = Xyy.ToXyz(xyy);
        xyzC = new Xyz(Math.Round(xyzC.X, 4), Math.Round(xyzC.Y, 4), Math.Round(xyzC.Z, 4));
        var xyzD65 = Adaptation.WhitePoint(xyzC, Illuminant.C.GetWhitePoint(Observer.Degree2), Illuminant.D65.GetWhitePoint(Observer.Degree2), Cam02.MCAT02);
        xyzD65 = new Xyz(Math.Round(xyzD65.X, 4), Math.Round(xyzD65.Y, 4), Math.Round(xyzD65.Z, 4));
        var colour = new Unicolour(ColourSpace.Xyz, xyzD65.Tuple);
        
        var expectedRgb = data.RgbMgo!.Value;
        var actualRgb = colour.Rgb.ConstrainedTriplet;
        TestUtils.AssertTriplet(actualRgb, new(expectedRgb.r, expectedRgb.g, expectedRgb.b), 0.01); // tolerance of 2.55 in byte255 format
        
        // TODO: use actual unicolour Munsell -> RGB, show error < 0.0114
    }
    
    [Test]
    public void LuminanceMgoCorrection()
    {
        // first entry of Table 2 in ASTM https://doi.org/10.1520/D1535-14R18
        var munsell = new Munsell(2.5, "R", 9, 6);
        var expected = new Xyy(0.3665, 0.3183, 76.70 / 100.0);
        var actual = Munsell.ToXyy(munsell);
        Assert.That(actual.Luminance, Is.EqualTo(expected.Luminance).Within(0.00005));
        
        // same node from raw data contains different value for Y, because it is relative to MgO white
        var expectedMgo = new Xyy(0.3665, 0.3183, 78.66 / 100.0);
        var rawMgo = XyyData.Single(data => data.Notation == munsell.ToString());
        var actualMgo = new Xyy(rawMgo.X, rawMgo.Y, rawMgo.LuminanceMgo / 100.0);
        Assert.That(actualMgo, Is.EqualTo(expectedMgo));
        Assert.That(actualMgo.Luminance, Is.GreaterThan(expected.Luminance));
        Assert.That(actualMgo.Luminance * MgoScale, Is.EqualTo(expected.Luminance).Within(0.0001));
    }
    
    [TestCaseSource(nameof(LuminanceTestData))]
    public void ValueToLuminance(double value, double expectedLuminance)
    {
        var y = Munsell.GetLuminance(value);
        Assert.That(y, Is.EqualTo(expectedLuminance / 100.0).Within(5e-16));
    }

    [TestCaseSource(nameof(LuminanceTestData))]
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
    
    // TODO: move to roundtrip tests when available
    [TestCaseSource(nameof(RandomLuminanceTestData))]
    public void LuminanceToValueRandom(double expectedValue, double luminance)
    {
        var v = Munsell.GetValue(luminance, iterationDepth: 3);
        Assert.That(v, Is.EqualTo(expectedValue).Within(Munsell.IterationDepthError[3]));
    }

    // TODO: rework with correct bounding points
    [Test]
    public void InsideDataset()
    {
        /*
         * closest known xy coordinates that form a boundary around (0.4, 0.4) at 6V are:
         * [up-left]   (0.3745, 0.4004) 7.5Y 6/4 ... (0.4240, 0.4030)  10YR 6/6 [up-right]
         * [down-left] (0.3840, 0.3867) 2.5Y 6/4 ... (0.4242, 0.3876) 7.5YR 6/6 [down-right]
         * x = 0.4 vertical intersects upper@(0.4, 0.4017) lower@(0.4, 0.3871)
         * y = 0.4 horizontal intersects left@(0.3748, 0.4) right@(0.424, 0.4)
         * target-xy is closest to upper intersect, so will interpolate using upper and lower "horizontals"
         * upper length (0.3745, 0.4004) --> (0.4240, 0.4030) = 0.0496; to intersect (0.3745, 0.4004) --> (0.4, 0.4017) = 0.0255
         * upper munsell = 0.0255 / 0.0496 = 51.52% from 7.5Y 6/4 --> 10YR 6/6 = 3.64Y 6/5.03
         * lower length (0.3840, 0.3867) --> (0.4242, 0.3876) = 0.0402; to intersect (0.3840, 0.3867) --> (0.4, 0.3871) = 0.0160
         * lower munsell = 0.0160 / 0.0402 = 39.80% from 2.5Y 6/4 --> 7.5YR 6/6 = 0.51Y 6/4.80
         * vertical length between intersects (0.4, 0.4017) --> (0.4, 0.3871) = 0.0147; to target-xy (0.4, 0.4017) --> (0.4, 0.4) = 0.0017
         * target munsell = 0.0017 / 0.0147 = 11.85% from 3.64Y 6/5.03 --> 0.51Y 6/4.80 = 3.27Y 6/5.00
         */
        var xyyV6 = new Xyy(0.4, 0.4, Munsell.GetLuminance(6));
        var expectedV6 = new Munsell(3.27, "Y", 6, 5.00);
        Assert.That(Interpolation.Interpolate(3.64, 0.51, 0.1185), Is.EqualTo(expectedV6.Hue.number).Within(0.005));
        Assert.That(Interpolation.Interpolate(5.03, 4.80, 0.1185), Is.EqualTo(expectedV6.Chroma).Within(0.005));
        var actualV6 = Munsell.FromXyy(xyyV6);
        Assert.That(actualV6.ToString(), Is.EqualTo(expectedV6.ToString()));
        
        /*
         * closest known xy coordinates that form a boundary around (0.4, 0.4) at 7V are:
         * [up-left]   (0.3943, 0.4264) 7.5Y 7/6 ... (0.4073, 0.4073) 2.5Y 7/6 [up-right]
         * [down-left] (0.3718, 0.3885)   5Y 7/4 ... (0.4102, 0.3960) 10YR 7/6 [down-right]
         * x = 0.4 vertical intersects upper@(0.4, 0.4180) lower@(0.4, 0.3940)
         * y = 0.4 horizontal intersects left@(0.3786, 0.4) right@(0.4092, 0.4)
         * target-xy is closest to lower intersect, so will interpolate using upper and lower "horizontals"
         * upper length (0.3943, 0.4264) --> (0.4073, 0.4073) = 0.0231; to intersect (0.3943, 0.4264) --> (0.4, 0.4180) = 0.0101
         * upper munsell = 0.0101 / 0.0231 = 43.85% from 7.5Y 7/6 --> 2.5Y 7/6 = 5.31Y 7/6.00
         * lower length (0.3718, 0.3885) --> (0.4102, 0.3960) = 0.0391; to intersect (0.3718, 0.3885) --> (0.4, 0.3940) = 0.0287
         * lower munsell = 0.0287 / 0.0391 = 73.44% from 5Y 7/4 --> 10YR 7/6 = 1.33Y 6/5.47
         * vertical length between intersects (0.4, 0.4180) --> (0.4, 0.3940) = 0.0240; to target-xy (0.4, 0.4180) --> (0.4, 0.4) = 0.0180
         * target munsell = 0.0180 / 0.0240 = 75.05% from 5.31Y 7/6.00 --> 1.33Y 6/5.47 = 2.32Y 7/5.60
         */
        var xyyV7 = new Xyy(0.4, 0.4, Munsell.GetLuminance(7));
        var expectedV7 = new Munsell(2.32, "Y", 7, 5.60);
        Assert.That(Interpolation.Interpolate(5.31, 1.33, 0.7505), Is.EqualTo(expectedV7.Hue.number).Within(0.005));
        Assert.That(Interpolation.Interpolate(6.00, 5.47, 0.7505), Is.EqualTo(expectedV7.Chroma).Within(0.005));
        var actualV7 = Munsell.FromXyy(xyyV7);
        Assert.That(actualV7.ToString(), Is.EqualTo(expectedV7.ToString()));
        
        /*
         * when V is between nodes, use result of lower and upper V and interpolate
         * target V = (6.25 - 6) / (7 - 6) = 25% from 3.27Y 6/5.00 --> 2.32Y 7/5.60 = 3.03Y 6.25/5.15
         */
        var xyy = new Xyy(0.4, 0.4, Munsell.GetLuminance(6.25));
        var expected = new Munsell(3.03, "Y", 6.25, 5.15);
        Assert.That(Interpolation.Interpolate(3.27, 2.32, 0.25), Is.EqualTo(expected.Hue.number).Within(0.005));
        Assert.That(Interpolation.Interpolate(5.00, 5.60, 0.25), Is.EqualTo(expected.Chroma).Within(0.005));
        var actual = Munsell.FromXyy(xyy);
        Assert.That(actual.ToString(), Is.EqualTo(expected.ToString()));
    }
    
    // TODO: test data point that isn't bounded
    
    // TODO: test greyscale, once implemented
    
    // TODO: test extreme values like NaN, infinity
    
    // TODO: get specific examples from ASTM to test against
    
    [Test]
    public void InsideDatasetBothAxes()
    {
        /*
         * both of these points reside in the lower-right corner of the boundary formed by
         * [up-left]   (0.4140, 0.4305)   5Y 6/6 ... (0.4240, 0.4030)  10YR 6/6 [up-right]
         * [down-left] (0.3840, 0.3867) 2.5Y 6/4 ... (0.4242, 0.3876) 7.5YR 6/6 [down-right]
         * one is closer to the lower boundary intersect, so will interpolate using upper and lower "horizontals"
         * the other is closer to the right boundary intersect, so will interpolate using left and right "verticals"
         * but since they are close together we expect the result to be similar
         */
        var xyyNearLower = new Xyy(0.4150, 0.3964, Munsell.GetLuminance(6));
        var xyyNearRight = new Xyy(0.4150, 0.3965, Munsell.GetLuminance(6));
        var fromHorizontals = Munsell.FromXyy(xyyNearLower);
        var fromVerticals = Munsell.FromXyy(xyyNearRight);
        Assert.That(fromHorizontals.HueDegrees, Is.EqualTo(fromVerticals.HueDegrees).Within(0.5)); // 0 - 360
        Assert.That(fromHorizontals.Value, Is.EqualTo(fromVerticals.Value));
        Assert.That(fromHorizontals.Chroma, Is.EqualTo(fromVerticals.Chroma).Within(0.075)); // 0 - 10
    }
    
    [Test]
    public void NotBounded()
    {
        var xyy = new Xyy(0.4, 0.4, 0.4);
        var munsell = Munsell.FromXyy(xyy);
        var xyyRoundtrip = Munsell.ToXyy(munsell);
        var munsellRoundtrip = Munsell.FromXyy(xyyRoundtrip);
    }

    [Test]
    public void Test()
    {
        var xyy = new Xyy(0.24, 0.41, Munsell.GetLuminance(5));
        var munsell = Munsell.FromXyy(xyy);
        var xyyRoundtrip = Munsell.ToXyy(munsell);
        Console.WriteLine($"{xyy} --> {munsell} --> {xyyRoundtrip}");
    }
}