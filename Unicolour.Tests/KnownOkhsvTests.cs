using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// matches values produced by https://github.com/bottosson/bottosson.github.io/blob/master/misc/colorpicker/colorconversion.js
public class KnownOkhsvTests
{
    private const double Tolerance = 0.0000005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Okhsv>(red, new(0.0812052366453962 * 360, 0.9995219692256989, 1.0000000001685625), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Okhsv>(green, new(0.3958203857994721 * 360, 0.9999997210415695, 0.9999999884428648), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Okhsv>(blue, new(0.7334778351057084 * 360, 0.9999910912349018, 0.9999999646150918), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Okhsv>(black, new(0, 0, 0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Okhsv>(white, new(0.2496543419330623 * 360, 1.0347523928230576e-7, 1.000000027003774), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertTriplet<Okhsv>(grey, new(0.24965434119047608 * 360, 1.0347523926099617e-7, 0.5337598416065157), Tolerance);
    }
    
    private static readonly List<TestCaseData> OklabData =
    [
        new(new ColourTriplet(0.5, -0.5, 0.0), new ColourTriplet(0.5 * 360, -1.4104445419929168, 0.631481023598059)),
        new(new ColourTriplet(0.5, +0.5, 0.0), new ColourTriplet(0.0 * 360, 1.7822882983359132, 1.049263654851725)),
        new(new ColourTriplet(0.5, 0.0, -0.5), new ColourTriplet(0.75 * 360, 1.1627893841865418, 1.4956630881154662)),
        new(new ColourTriplet(0.5, 0.0, +0.5), new ColourTriplet(0.25 * 360, -2.4740585901001264, 0.7862268119428452)),
        new(new ColourTriplet(0.0, 0.0, 0.0), new ColourTriplet(0.0, 0.0, 0.0)), // Toe(0.0)
        new(new ColourTriplet(0.5, 0.0, 0.0), new ColourTriplet(0.0, 0.0, 0.42114056260896976)), // Toe(0.5)
        new(new ColourTriplet(1.0, 0.0, 0.0), new ColourTriplet(0.0, 0.0, 1.0))
    ];

    [TestCaseSource(nameof(OklabData))]
    public void FromOklab(ColourTriplet oklab, ColourTriplet expected)
    {
        var colour = new Unicolour(ColourSpace.Oklab, oklab.Tuple);
        TestUtils.AssertTriplet<Okhsv>(colour, expected, Tolerance);
    }
}