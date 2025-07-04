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

    [Test]
    public void BetweenFourNodes()
    {
        var xyy = new Xyy(0.4, 0.4, Munsell.GetLuminance(6));
        var munsell = Munsell.FromXyy(xyy);

        /*
         * closest known xy coordinates that form a boundary around (0.4, 0.4) at 6V are:
         * [up-left] (0.3745, 0.4004) 7.5Y 6/4 ... (0.424, 0.403) 10YR 6/6 [up-right]
         * [down-left] (0.384, 0.3867) 2.5Y 6/4 ... (0.4242, 0.3876) 7.5YR 6/6 [down-right]
         * x = 0.4 vertical intersects upper@(0.4, 0.4017) lower@(0.4, 0.3871)
         * y = 0.4 horizontal intersects left@(0.3748, 0.4) right@(0.424, 0.4)
         * target-xy is closest to upper intersect, so will interpolate using upper and lower "horizontals"
         * upper length (0.3745, 0.4004) --> (0.424, 0.403) = 0.0496, to intersect (0.3745, 0.4004) --> (0.4, 0.4017) = 0.0255
         * upper munsell = 0.0255 / 0.0496 = 51.52% from 7.5Y 6/4 --> 10YR 6/6 = 3.64Y 6/5.0
         * lower length (0.384, 0.3867) --> (0.4242, 0.3876) = 0.0402, to intersect (0.384, 0.3867) --> (0.4, 0.3871) = 0.016
         * lower munsell = 0.016 / 0.0402 = 39.8% from 2.5Y 6/4 --> 7.5YR 6/6 = 0.51Y 6/4.8
         * vertical length between intersects (0.4, 0.4017) --> (0.4, 0.3871) = 0.0147, to target-xy (0.4, 0.4017) --> (0.4, 0.4) = 0.0017
         * target munsell = 0.0017 / 0.0147 = 11.8% from 3.64Y 6/5.0 --> 0.51Y 6/4.8 = 3.33Y 6/5
         */

        var expected = new Munsell(3.33, "Y", 6, 5);
        Assert.That(munsell.ToString(), Is.EqualTo(expected.ToString()));
        
        // TODO: to the same as above from luminance 5
        // TODO: then show an interpolated V e.g. 5.25, as the interpolated result of 5 and 6
    }
    
    // TODO: same as above test, but for 3 nodes, requiring extrapolation
    //       4 tests, one for each missing direction
    
    // TODO: same as above test, but for 2 nodes, requiring extrapolation
    //       4 tests, one for each pair of missing directions (e.g. negative infinity X, won't have upper-left or lower-left points)
    
    // TODO: test greyscale, once implemented
    
    // TODO: test extreme values like
    
    // TODO: get specific examples from ASTM to test against
}