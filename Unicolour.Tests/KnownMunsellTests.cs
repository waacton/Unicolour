using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownMunsellTests
{
    private static MunsellTestData[] XyyData = Experimental.MunsellCache.Nodes.Value.Select(node => new MunsellTestData(node)).ToArray();
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
            var y = MunsellFuncs.GetLuminance(v);
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
        var munsellFromDegrees = new Munsell(Munsell.MunsellHue.ToDegrees(data.HueNumber, data.HueLetter), data.Value, data.Chroma);
        Assert.That(munsellFromDegrees, Is.EqualTo(munsell));

        // the Y value in the raw data is relative to MgO, and is not the correct luminance value
        // scale Ymgo to get the intended expected luminance Y for CIE illuminant C 
        var expectedLuminance = data.LuminanceMgo / 100 * MgoScale;
        
        var actual = MunsellFuncs.ToXyy(munsell);
        Assert.That(actual.Chromaticity.X, Is.EqualTo(data.X).Within(1e-16));
        Assert.That(actual.Chromaticity.Y, Is.EqualTo(data.Y).Within(1e-16));
        Assert.That(actual.Luminance, Is.EqualTo(expectedLuminance).Within(0.00025));
    }
    
    [TestCaseSource(nameof(RgbData))]
    public void KnownRgb(MunsellTestData data)
    {
        var munsell = new Munsell(data.HueNumber, data.HueLetter, data.Value, data.Chroma);
        var xyy = MunsellFuncs.ToXyy(munsell);

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
        var actual = MunsellFuncs.ToXyy(munsell);
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
        var y = MunsellFuncs.GetLuminance(value);
        Assert.That(y, Is.EqualTo(expectedLuminance / 100.0).Within(5e-16));
    }

    [TestCaseSource(nameof(LuminanceTestData))]
    public void LuminanceToValue(double expectedValue, double luminance)
    {
        var y = luminance / 100.0;
        var values = Enumerable.Range(0, 4).Select(i => MunsellFuncs.GetValue(y, iterationDepth: i)).ToList();
        
        Assert.That(values[0], Is.EqualTo(expectedValue).Within(MunsellFuncs.IterationDepthError[0]));
        Assert.That(values[1], Is.EqualTo(expectedValue).Within(MunsellFuncs.IterationDepthError[1]));
        Assert.That(values[2], Is.EqualTo(expectedValue).Within(MunsellFuncs.IterationDepthError[2]));
        Assert.That(values[3], Is.EqualTo(expectedValue).Within(MunsellFuncs.IterationDepthError[3]));
        
        Assert.That(MunsellFuncs.IterationDepthError[0], Is.EqualTo(0.0035));
        Assert.That(MunsellFuncs.IterationDepthError[1], Is.EqualTo(0.000005));
        Assert.That(MunsellFuncs.IterationDepthError[2], Is.EqualTo(0.00000000005));
        Assert.That(MunsellFuncs.IterationDepthError[3], Is.EqualTo(0.000000000000005));
    }
    
    // TODO: move to roundtrip tests when available
    [TestCaseSource(nameof(RandomLuminanceTestData))]
    public void LuminanceToValueRandom(double expectedValue, double luminance)
    {
        var v = MunsellFuncs.GetValue(luminance);
        Assert.That(v, Is.EqualTo(expectedValue).Within(MunsellFuncs.IterationDepthError[3]));
    }

    [Test]
    public void InsideDataset()
    {
        /*
         * closest node to (0.4, 0.4) at V6 is 2.5Y 6/4 (0.3840, 0.3867) and is located within quadrilateral formed by
         * 2.5Y 6/4 (0.3840, 0.3867) · 2.5Y 6/6 (0.4203, 0.4176) · 10YR 6/6 (0.4240, 0.4030) · 10YR 6/4 (0.3861, 0.3767)
         * nearest boundary intersect is (0.4, 0.4003), opposite intersect is (0.4, 0.3863) from vertical line through point
         * near (0.3840, 0.3867) to (0.4, 0.4003) is 44.08% to (0.4203, 0.4176) · 44.08% of 2.5Y /4 to 2.5Y /6 = 2.5Y /4.88
         * far  (0.4240, 0.4030) to (0.4, 0.3863) is 63.32% to (0.3861, 0.3767) · 63.32% of 10YR /6 to 10YR /4 = 10YR /4.73
         * between (0.4, 0.4003) to (0.4, 0.4) is 2.29% to (0.4, 0.3863) · 2.29% of 2.5Y /4.88 to 10YR /4.73 = 2.44Y /4.88
         */
        var xyyV6 = new Xyy(0.4, 0.4, MunsellFuncs.GetLuminance(6));
        var expectedV6 = new Munsell(2.44, "Y", 6, 4.88);
        var actualV6 = Munsell.FromXyy(xyyV6);
        Assert.That(actualV6.ToString(), Is.EqualTo(expectedV6.ToString()));
        
        /*
         * closest node to (0.4, 0.4) at V7 is 2.5Y 7/6 (0.4073, 0.4073) and is located within quadrilateral formed by
         * 2.5Y 7/6 (0.4073, 0.4073) · 2.5Y 7/4 (0.3761, 0.3800) · 10YR 7/4 (0.3778, 0.3719) · 10YR 7/6 (0.4102, 0.3960) 
         * nearest boundary intersect is (0.4, 0.4009), opposite intersect is (0.4, 0.3884) from vertical line through point
         * near (0.4073, 0.4073) to (0.4, 0.4009) is 23.40% to (0.3761, 0.3800) · 23.40% of 2.5Y /6 to 2.5Y /4 = 2.5Y /5.53
         * far  (0.3778, 0.3719) to (0.4, 0.3884) is 68.52% to (0.4102, 0.3960) · 63.32% of 10YR /4 to 10YR /6 = 10YR /5.37
         * between (0.4, 0.4009) to (0.4, 0.4) is 7.30% to (0.4, 0.3884) · 7.30% of 2.5Y /5.53 to 10YR /5.37 = 2.32Y /5.52
         */
        var xyyV7 = new Xyy(0.4, 0.4, MunsellFuncs.GetLuminance(7));
        var expectedV7 = new Munsell(2.32, "Y", 7, 5.52);
        var actualV7 = Munsell.FromXyy(xyyV7);
        Assert.That(actualV7.ToString(), Is.EqualTo(expectedV7.ToString()));
        
        /*
         * when V is between nodes, use result of lower and upper V and interpolate
         * target V = (6.25 - 6) / (7 - 6) = 25% from 2.44Y /4.88 to 2.32Y /5.52 = 2.41Y 6.25/5.04
         */
        var xyy = new Xyy(0.4, 0.4, MunsellFuncs.GetLuminance(6.25));
        var expected = new Munsell(2.41, "Y", 6.25, 5.04);
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
         * both of these points reside in the corner of the boundary formed by
         * 10YR 6/6 (0.4240, 0.4030) · 10YR 6/4 (0.3861, 0.3767) · 2.5Y 6/4 (0.3840, 0.3867) · 2.5Y 6/6 (0.4203, 0.4176)
         * for one, the nearest intersect is along the horizontal, and will interpolate along that axis
         * for the other, the nearest intersect is along the vertical, and will interpolate along that axis
         * but since they are close together the result is expected to be similar
         */
        var fromHorizontalInterpolation = Munsell.FromXyy(new Xyy(0.4228, 0.4033, MunsellFuncs.GetLuminance(6)));
        var fromVerticalInterpolation = Munsell.FromXyy(new Xyy(0.4228, 0.4032, MunsellFuncs.GetLuminance(6)));
        TestUtils.AssertTriplet(fromVerticalInterpolation.Triplet, fromHorizontalInterpolation.Triplet, [0.5, 0, 0.0005]);
    }


    /*
      not showing signs of initially much beyond chroma limit
      ViaXyy((296.99122278711053, 7.1458862344028375, 23.753411792350175))
      ViaXyy((323.68983215449, 9.248789828312518, 24.491452043176217))
      ViaXyy((342.0394935653883, 9.329025361760642, 25.859770450315057))
      ViaXyy((342.676682413887, 8.86670264104983, 17.600438424189612))
      ViaXyy((99.27903834847132, 0.6756384858878572, 18.217854208673895))
      ViaXyy((99.29021501653409, 0.7114380838955181, 3.393813318850121))
      ViaXyy((215.76870092982298, 9.386059990398635, 24.346793792313516))
      ViaXyy((90.15400657427764, 8.504026262273152, 25.48630965416118))
      ViaXyy((242.6383075733755, 2.2328441372792684, 24.231759645463015))
      ViaXyy((249.84998185176948, 6.850177746172708, 24.885076446944645))
      ViaXyy((306.06862703675625, 7.450675395678025, 25.661328740880105))
      
      does not converge
      ViaXyy((10.357846197671957, 9.267729934046438, 12.320776414129435))
      ViaXyy((11.740166736842784, 4.402800534264263, 2.4638227277392852))
      ViaXyy((14.940456541401916, 4.2218144824049295, 6.123591786188552))
      ViaXyy((21.793297613763357, 2.640517833386938, 19.100890877843067))
      ViaXyy((261.03621832725435, 0.6098862579241404, 12.34181986003048))
      ViaXyy((39.5611419937967, 2.0198983883140853, 17.51239891286274))
      ViaXyy((9.832657990067215, 5.72194569986478, 3.0789138344934406))

      results in negative xy, which means black LCH and no initial start (gets stuck at white point)
      (237.53961331646494, 0.9382942803771765, 18.571708212242463); // negative xy coordinates
      
      no chroma data at all for 10Y 0.2/ so needs to fall back to 0 chroma (white point)
      (107.01845505928627, 0.35467330379196027, 11.757722436978339)
      
      no chroma data at all for these either
      ViaXyy((102.02004375365254, 0.2544340832926528, 19.199055759042295))
      ViaXyy((120.80436018871674, 0.15141985879204345, 9.295290360348211))
      ViaXyy((185.660339017603, 0.05265165828158613, 17.09847455867324))
      ViaXyy((21.623650354786474, 0.10198991388955747, 2.558440708292178))
      ViaXyy((310.7389843153769, 0.17841275958321634, 9.710527097398455))
      ViaXyy((60.02764606853851, 0.015378879617444774, 18.644192832214145))
    */
    [Test]
    public void Test()
    {
        // var original = new Munsell(4.2, "YR", 8.1, 5.3);
        // var original = new Munsell(6.66, "R", 6.66, 6.66);
        // var original = new Munsell(4.2, "G", 5.5, 99);
        // var original = new Munsell(116.60697248709873, 0.22737082192960334, 2.1740859211633805);
        var original = new Munsell(4.098412460750023, 3.574724518514384, 10.196462054672057);
        var originalBounds = original.Bounds;

        var xyy = MunsellFuncs.ToXyy(original);
        var roundtrip = MunsellFuncs.FromXyy(xyy);
        var roundtripBounds = roundtrip.Bounds;

        if (original.C < 0.5)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [0.75, 5e-15, 0.5]);
            return;
        }

        // Console.WriteLine($"original ... C {original.C:F2} is above max chroma by {originalBounds.MaxChromaScale}x ({string.Join(", ", originalBounds.UpperChromaLimits)})");
        // Console.WriteLine($"roundtrip .. C {roundtrip.C:F2} is above max chroma by {roundtripBounds.MaxChromaScale}x ({string.Join(", ", roundtripBounds.UpperChromaLimits)})");
        if (originalBounds.ChromaLimitScale > 2.5 || roundtripBounds.ChromaLimitScale > 2.5)
        {
            // roundtrip is almost never this inaccurate even when chroma is not within range
            // but certain rare values deviate a lot, and these coincide with chromas well outside the available data
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [7.5, 5e-15, 15]);
            return;
        }
            
        if (originalBounds.ChromaLimitScale > 1.5 || roundtripBounds.ChromaLimitScale > 1.5)
        {
            // roundtrip is almost never this inaccurate even when chroma is not within range
            // but certain rare values deviate a lot, and these coincide with chromas well outside the available data
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [1.25, 5e-15, 7]);
            return;
        }
        
        if (originalBounds.ChromaLimitScale > 1 || roundtripBounds.ChromaLimitScale > 1)
        {
            // roundtrip is almost never this inaccurate even when chroma is not within range
            // but certain rare values deviate a lot, and these coincide with chromas well outside the available data
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [1.25, 5e-15, 1.25]);
            return;
        }
        
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [0.1, 5e-15, 0.1]);
    }

    [Test]
    public void Test2()
    {
        // var xyy = new Xyy(0.52, 0.27, 11.71 / 100.0);
        // var xyy = new Xyy(0.33677419510472395, 0.1708935237882816, 0.001797989184534332);
        // var xyy = new Xyy(0.7858633707744189, 0.007105953622768335, 0.274929467457103);
        // var xyy = new Xyy(0.9338835363255807, 0.3152111547768083, 0.6212223067431817);
        var xyy = new Xyy(0.45874396736163936, 0.8456399614207708, 0.2075337930224812);
        var munsell = MunsellFuncs.FromXyy(xyy);
        var round = MunsellFuncs.ToXyy(munsell);
        Console.WriteLine(xyy);
        Console.WriteLine(munsell);
        Console.WriteLine(round);
    }
}