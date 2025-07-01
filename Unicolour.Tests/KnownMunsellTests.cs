using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownMunsellTests
{
    private static MunsellTestData[] XyyData = Experimental.Munsell.Nodes.Value.Select(MunsellTestData.FromNode).ToArray();
    
    [TestCaseSource(nameof(XyyData))]
    public void KnownXyy(MunsellTestData data)
    {
        var munsell = new Munsell(data.HueNumber, data.HueLetter, data.Value, data.Chroma);
        var munsellFromDegrees = new Munsell(MunsellUtils.ToDegrees(data.HueNumber, data.HueLetter), data.Value, data.Chroma);

        Assert.That(munsellFromDegrees, Is.EqualTo(munsell));
        
        var actual = Munsell.ToXyy(munsell);
        Assert.That(actual.Chromaticity.X, Is.EqualTo(data.X).Within(1e-16));
        Assert.That(actual.Chromaticity.Y, Is.EqualTo(data.Y).Within(1e-16));
        Assert.That(actual.Luminance, Is.EqualTo(data.LuminanceMagnesiumOxide / 100 * 0.975).Within(0.00025));
    }
    
    /*
     * expected xyY is taken directly from Table 2 of ASTM https://doi.org/10.1520/D1535-14R18
     * expected RGB is taken from https://www.andrewwerth.com/color/
     */ 
    private static readonly TestCaseData[] RgbData =
    [
        new TestCaseData(new Munsell(5, "R", 5, 18), new ColourTriplet(0.5918, 0.3038, 19.27 / 100.0), new ColourTriplet(243, 8, 61)).SetName("Red"),
        new TestCaseData(new Munsell(5, "YR", 7, 12), new ColourTriplet(0.5007, 0.4081, 41.99 / 100.0), new ColourTriplet(252, 150, 51)).SetName("Yellow-Red"),
        new TestCaseData(new Munsell(5, "Y", 8, 10), new ColourTriplet(0.4376, 0.4601, 57.62 / 100.0), new ColourTriplet(232, 201, 53)).SetName("Yellow"),
        new TestCaseData(new Munsell(5, "GY", 9, 12), new ColourTriplet(0.3911, 0.5082, 76.70 / 100.0), new ColourTriplet(207, 245, 41)).SetName("Green-Yellow"),
    ];
    
    [TestCaseSource(nameof(RgbData))]
    public void Rgb(Munsell munsell, ColourTriplet expectedXyy, ColourTriplet expectedRgb255)
    {
        var xyy = Munsell.ToXyy(munsell);
        TestUtils.AssertTriplet(xyy.Triplet, expectedXyy, 0.00005);

        var xyyRelativeToYmgo = new Xyy(xyy.Chromaticity.X, xyy.Chromaticity.Y, xyy.Luminance / 0.975);
        var colour = ToUnicolour(xyy);
        TestUtils.AssertTriplet<Rgb255>(colour, expectedRgb255, 3); // equivalent to ~0.0125 tolerance of RGB in 0-1 range
    }

    [Test]
    public void FromMunsell2()
    {
        var munsell = new Munsell(4.2, "YR", 8.1, 5.3); // 0.38736945,  0.35751656,  0.59362
        Console.WriteLine(munsell);

        var xyy = Experimental.Munsell.ToXyy(munsell);
        Console.WriteLine(xyy);

        var munsellRoundtrip = Experimental.Munsell.FromXyy(xyy);
        Console.WriteLine(munsellRoundtrip);
    }
    
    [Test]
    public void FromXyy1()
    {
        var xyy = new Xyy(0.500, 0.454, 0.4602); // 10YR 7.2/13.5
        Console.WriteLine(xyy);

        var munsell = Experimental.Munsell.FromXyy(xyy);
        Console.WriteLine(munsell);

        var xyyRoundtrip = Experimental.Munsell.ToXyy(munsell);
        Console.WriteLine(xyyRoundtrip);
        
        var munsellRoundtrip = Experimental.Munsell.FromXyy(xyyRoundtrip);
        Console.WriteLine(munsellRoundtrip);
    }

    [Test]
    public void FromXyy2()
    {
        var xyy = new Xyy(0.2437, 0.3240, 0.2198); //  5.6 BG 5.30 / 5.3
        Console.WriteLine(xyy);
        
        var munsell = Experimental.Munsell.FromXyy(xyy);
        Console.WriteLine(munsell);

        var xyyRoundtrip = Experimental.Munsell.ToXyy(munsell);
        Console.WriteLine(xyyRoundtrip);
    }
    
    [Test]
    public void FromXyy3()
    {
        var xyy = new Xyy(0.365, 0.347, 0.576196); // somewhere between 2.5YR 8/3.5 ; 2.5YR 8/4.0 ; 5.0YR 8/3.5 ; 5.0YR 8/4.0
        Console.WriteLine(xyy);
        
        var munsell = Experimental.Munsell.FromXyy(xyy);
        Console.WriteLine(munsell);

        var xyyRoundtrip = Experimental.Munsell.ToXyy(munsell);
        Console.WriteLine(xyyRoundtrip);
    }

    private static Unicolour ToUnicolour(Xyy xyyC)
    {
        var xyz = Xyy.ToXyz(xyyC); // ugh, srgb excel file uses raw Y from the data as illuminant C, but it's actually Ymgo...
        var xyzD65 = Adaptation.WhitePoint(xyz, Illuminant.C.GetWhitePoint(Observer.Degree2), Illuminant.D65.GetWhitePoint(Observer.Degree2), new Matrix(Adaptation.Bradford));
        return new Unicolour(ColourSpace.Xyz, xyzD65.Tuple);
    }
}