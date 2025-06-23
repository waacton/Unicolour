using System;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;

namespace Wacton.Unicolour.Tests;

public class MunsellTests
{
    [Test]
    public void FromMunsell1()
    {
        // first item of ASTM D1535-14 table 2:
        // 2.5R 9/6 = (0.3665, 0.3183, 76.70)
        var munsell = new Munsell(2.5, 9, 6, "R");
        Console.WriteLine(munsell);

        var xyy = Munsell.ToXyy(munsell);
        Console.WriteLine(xyy);
        
        var munsellRoundtrip = Munsell.FromXyy(xyy);
        Console.WriteLine(munsellRoundtrip);
    }

    [Test]
    public void FromMunsell2()
    {
        var munsell = new Munsell(4.2, 8.1, 5.3, "YR"); // 0.38736945,  0.35751656,  0.59362
        Console.WriteLine(munsell);

        var xyy = Munsell.ToXyy(munsell);
        Console.WriteLine(xyy);

        var munsellRoundtrip = Munsell.FromXyy(xyy);
        Console.WriteLine(munsellRoundtrip);
    }
    
    [Test]
    public void FromXyy1()
    {
        var xyy = new Xyy(0.500, 0.454, 0.4602); // 10YR 7.2/13.5
        Console.WriteLine(xyy);

        var munsell = Munsell.FromXyy(xyy);
        Console.WriteLine(munsell);

        var xyyRoundtrip = Munsell.ToXyy(munsell);
        Console.WriteLine(xyyRoundtrip);
    }

    [Test]
    public void FromXyy2()
    {
        var xyy = new Xyy(0.2437, 0.3240, 0.2198); //  5.6 BG 5.30 / 5.3
        Console.WriteLine(xyy);
        
        var munsell = Munsell.FromXyy(xyy);
        Console.WriteLine(munsell);

        var xyyRoundtrip = Munsell.ToXyy(munsell);
        Console.WriteLine(xyyRoundtrip);
    }
}