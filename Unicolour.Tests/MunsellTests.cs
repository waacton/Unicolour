using System;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;

namespace Wacton.Unicolour.Tests;

public class MunsellTests
{
    [Test]
    public void ToXyy()
    {
        // first item of ASTM D1535-14 table 2:
        // 2.5R 9/6 = (0.3665, 0.3183, 76.70)
        var munsell = new Munsell(2.5, 9, 6, "R");
        var xyy = munsell.ToXyy();
        Console.WriteLine(xyy);

        var munsell2 = new Munsell(7.5, 5, 10, "G"); // Y = 19.27, x = 0.2200, and y = 0.4082
        var xyy2 = munsell2.ToXyy();
        Console.WriteLine(xyy2);
        var xyz = Xyy.ToXyz(xyy2);

        var d65 = Adaptation.WhitePoint(xyz, 
            Illuminant.C.GetWhitePoint(Observer.Degree2),
            Illuminant.D65.GetWhitePoint(Observer.Degree2), 
            new Matrix(Adaptation.XyzScaling));
    }

    [Test]
    public void FromXyy()
    {
        var xyy = new Xyy(0.2437, 0.3240, 21.98); //  5.6 BG 5.30 / 5.3
        var munsell = Munsell.FromXyy(xyy);
    }
}