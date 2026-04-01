using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownOkhwbTests
{
    private const double Tolerance = 0.0000005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Okhwb(0.0812052366453962 * 360, 0.000478031, 0.000000000), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Okhwb(0.3958203857994721 * 360, 0.000000279, 0.000000012), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Okhwb(0.7334778351057084 * 360, 0.000008909, 0.000000035), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Okhwb(0, 0, 1), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertColour(white, new Okhwb(0, 0.999999924, -0.000000027), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertColour(grey, new Okhwb(0, 0.533759786, 0.466240158), Tolerance);
    }
    
    [Test]
    public void Achromatic()
    {
        var grey = StandardRgb.Grey;
        Assert.That(grey.Okhwb.ToString().Contains(NoHue));
    }
}