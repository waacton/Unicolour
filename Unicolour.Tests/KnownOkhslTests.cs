using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Tests;

// matches values produced by https://github.com/bottosson/bottosson.github.io/blob/master/misc/colorpicker/colorconversion.js
public class KnownOkhslTests
{
    private const double Tolerance = 0.0000005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Okhsl(0.0812052366453962 * 360, 1.0000000001433997, 0.5680846525040862), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Okhsl(0.3958203857994721 * 360, 0.9999999700728788, 0.8445289645307816), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Okhsl(0.7334778351057084 * 360, 0.9999999948631134, 0.3665653394260194), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Okhsl(0, 0, 0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertColour(white, new Okhsl(0, 0, 0.9999999923961898), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertColour(grey, new Okhsl(0, 0, 0.5337598228073358), Tolerance);
    }
    
    [Test]
    public void Achromatic()
    {
        var grey = StandardRgb.Grey;
        Assert.That(grey.Okhsl.ToString().Contains(NoHue));
    }
    
    private static readonly TestCaseData[] OklabData =
    [
        new(new Oklab(0.5, -0.5, 0.0), new Okhsl(0.5 * 360, 3.377334888701067, 0.42114056260896976)),
        new(new Oklab(0.5, +0.5, 0.0), new Okhsl(0.0 * 360, 0.5241848002880121, 0.42114056260896976)),
        new(new Oklab(0.5, 0.0, -0.5), new Okhsl(0.75 * 360, 1.0959445594370765, 0.42114056260896976)),
        new(new Oklab(0.5, 0.0, +0.5), new Okhsl(0.25 * 360, -0.19759454578641367, 0.42114056260896976)),
        new(new Oklab(0.0, 0.0, 0.0), new Okhsl(0.0, 0.0, 0.0)), // Toe(0.0)
        new(new Oklab(0.5, 0.0, 0.0), new Okhsl(0.0, 0.0, 0.42114056260896976)), // Toe(0.5)
        new(new Oklab(1.0, 0.0, 0.0), new Okhsl(0.0, 0.0, 1.0))
    ];

    [TestCaseSource(nameof(OklabData))]
    public void FromOklab(Oklab oklab, Okhsl expected)
    {
        var colour = new Unicolour(ColourSpace.Oklab, oklab.Tuple);
        TestUtils.AssertColour(colour, expected, Tolerance);
    }
}