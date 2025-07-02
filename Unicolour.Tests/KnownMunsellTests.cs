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
    
    
    // public void Rgb(Munsell munsell, ColourTriplet expectedXyy, ColourTriplet expectedRgb255)
    // {
    //     var xyy = Munsell.ToXyy(munsell);
    //     
    //     var xyyRelativeToYmgo = new Xyy(xyy.Chromaticity.X, xyy.Chromaticity.Y, xyy.Luminance / 0.975);
    //     TestUtils.AssertTriplet(xyyRelativeToYmgo.Triplet, expectedXyy, 0.00005);
    //
    //     var colour = ToUnicolour(xyyRelativeToYmgo, false);
    //     TestUtils.AssertTriplet<Rgb255>(colour, expectedRgb255, 3); // equivalent to ~0.0125 tolerance of RGB in 0-1 range
    // }
    //
    // [Test]
    // public void FromMunsell2()
    // {
    //     var munsell = new Munsell(4.2, "YR", 8.1, 5.3); // 0.38736945,  0.35751656,  0.59362
    //     Console.WriteLine(munsell);
    //
    //     var xyy = Experimental.Munsell.ToXyy(munsell);
    //     Console.WriteLine(xyy);
    //
    //     var munsellRoundtrip = Experimental.Munsell.FromXyy(xyy);
    //     Console.WriteLine(munsellRoundtrip);
    // }
    //
    // [Test]
    // public void FromXyy1()
    // {
    //     var xyy = new Xyy(0.500, 0.454, 0.4602); // 10YR 7.2/13.5
    //     Console.WriteLine(xyy);
    //
    //     var munsell = Experimental.Munsell.FromXyy(xyy);
    //     Console.WriteLine(munsell);
    //
    //     var xyyRoundtrip = Experimental.Munsell.ToXyy(munsell);
    //     Console.WriteLine(xyyRoundtrip);
    //     
    //     var munsellRoundtrip = Experimental.Munsell.FromXyy(xyyRoundtrip);
    //     Console.WriteLine(munsellRoundtrip);
    // }
    //
    // [Test]
    // public void FromXyy2()
    // {
    //     var xyy = new Xyy(0.2437, 0.3240, 0.2198); //  5.6 BG 5.30 / 5.3
    //     Console.WriteLine(xyy);
    //     
    //     var munsell = Experimental.Munsell.FromXyy(xyy);
    //     Console.WriteLine(munsell);
    //
    //     var xyyRoundtrip = Experimental.Munsell.ToXyy(munsell);
    //     Console.WriteLine(xyyRoundtrip);
    // }
    //
    // [Test]
    // public void FromXyy3()
    // {
    //     var xyy = new Xyy(0.365, 0.347, 0.576196); // somewhere between 2.5YR 8/3.5 ; 2.5YR 8/4.0 ; 5.0YR 8/3.5 ; 5.0YR 8/4.0
    //     Console.WriteLine(xyy);
    //     
    //     var munsell = Experimental.Munsell.FromXyy(xyy);
    //     Console.WriteLine(munsell);
    //
    //     var xyyRoundtrip = Experimental.Munsell.ToXyy(munsell);
    //     Console.WriteLine(xyyRoundtrip);
    // }
}