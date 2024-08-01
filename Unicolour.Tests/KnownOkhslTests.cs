using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// matches values produced by https://github.com/bottosson/bottosson.github.io/blob/master/misc/colorpicker/colorconversion.js
public class KnownOkhslTests
{
    private const double Tolerance = 0.0000005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Okhsl>(red, new(0.0812052366453962 * 360, 1.0000000001433997, 0.5680846525040862), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Okhsl>(green, new(0.3958203857994721 * 360, 0.9999999700728788, 0.8445289645307816), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Okhsl>(blue, new(0.7334778351057084 * 360, 0.9999999948631134, 0.3665653394260194), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Okhsl>(black, new(0, 0, 0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Okhsl>(white, new(0.2496543419330623 * 360, 0.5582831888483675, 0.9999999923961898), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertTriplet<Okhsl>(grey, new(0.24965434119047608 * 360, 1.1616558204531687e-7, 0.5337598228073358), Tolerance);
    }
    
    private static readonly List<TestCaseData> OklabData =
    [
        new TestCaseData(new ColourTriplet(0.5, -0.5, 0.0), new ColourTriplet(0.5 * 360, 3.377334888701067, 0.42114056260896976)),
        new TestCaseData(new ColourTriplet(0.5, +0.5, 0.0), new ColourTriplet(0.0 * 360, 0.5241848002880121, 0.42114056260896976)),
        new TestCaseData(new ColourTriplet(0.5, 0.0, -0.5), new ColourTriplet(0.75 * 360, 1.0959445594370765, 0.42114056260896976)),
        new TestCaseData(new ColourTriplet(0.5, 0.0, +0.5), new ColourTriplet(0.25 * 360, -0.19759454578641367, 0.42114056260896976)),
        new TestCaseData(new ColourTriplet(0.0, 0.0, 0.0), new ColourTriplet(0.0, 0.0, 0.0)), // Toe(0.0)
        new TestCaseData(new ColourTriplet(0.5, 0.0, 0.0), new ColourTriplet(0.0, 0.0, 0.42114056260896976)), // Toe(0.5)
        new TestCaseData(new ColourTriplet(1.0, 0.0, 0.0), new ColourTriplet(0.0, 0.0, 1.0))
    ];

    [TestCaseSource(nameof(OklabData))]
    public void FromOklab(ColourTriplet oklab, ColourTriplet expected)
    {
        var colour = new Unicolour(ColourSpace.Oklab, oklab.Tuple);
        TestUtils.AssertTriplet<Okhsl>(colour, expected, Tolerance);
    }
}