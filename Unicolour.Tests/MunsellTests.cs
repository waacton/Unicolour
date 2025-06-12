using System;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;

namespace Wacton.Unicolour.Tests;

public class MunsellTests
{
    [Test]
    public void X()
    {
        // first item of ASTM D1535-14 table 2:
        // 2.5R 9/6 = (0.3665, 0.3183, 76.70)
        var munsell = new Munsell(2.5, 9, 6, Munsell.Letter.R);
        var xyy = munsell.ToXyy();
        Console.WriteLine(xyy);
    }
}