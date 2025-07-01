using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownMunsellTests
{
    private static MunsellTestData[] KnownData = Experimental.Munsell.Nodes.Value.Select(MunsellTestData.FromNode).ToArray();
    
    [TestCaseSource(nameof(KnownData))]
    public void KnownMunsell(MunsellTestData data)
    {
        var munsell = new Munsell(data.HueNumber, data.HueLetter, data.Value, data.Chroma);
        var munsellFromDegrees = new Munsell(MunsellUtils.ToDegrees(data.HueNumber, data.HueLetter), data.Value, data.Chroma);

        Assert.That(munsellFromDegrees, Is.EqualTo(munsell));
        
        var actual = Experimental.Munsell.ToXyy(munsell);
        Assert.That(actual.Chromaticity.X, Is.EqualTo(data.X).Within(1e-16));
        Assert.That(actual.Chromaticity.Y, Is.EqualTo(data.Y).Within(1e-16));
        Assert.That(actual.Luminance, Is.EqualTo(data.LuminanceMagnesiumOxide / 100 * 0.975).Within(0.00025));
    }

    [Test]
    public void ToD65()
    {
        var munsell = new Munsell(10, "RP", 1, 2); // 0.3629, 0.271, 1.21
        var xyy = Munsell.ToXyy(munsell);
        var xyz = Xyy.ToXyz(new Xyy(xyy.Chromaticity.X, xyy.Chromaticity.Y, 0.0121)); // ugh, srgb excel file uses raw Y from the data as illuminant C, but it's actually Ymgo...
        var xyzD65 = Adaptation.WhitePoint(xyz, Illuminant.C.GetWhitePoint(Observer.Degree2), Illuminant.D65.GetWhitePoint(Observer.Degree2), new Matrix(Adaptation.Bradford));
        var uni = new Unicolour(ColourSpace.Xyz, xyzD65.Tuple);
        var rgb = uni.Rgb.Byte255;
    }
    
    [Test]
    public void FromMunsell1()
    {
        // first item of ASTM D1535-14 table 2:
        // 2.5R 9/6 = (0.3665, 0.3183, 76.70)
        var munsell = new Munsell(2.5, "R", 9, 6);
        Console.WriteLine(munsell);

        var xyy = Experimental.Munsell.ToXyy(munsell);
        Console.WriteLine(xyy);
        
        var munsellRoundtrip = Experimental.Munsell.FromXyy(xyy);
        Console.WriteLine(munsellRoundtrip);
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
}