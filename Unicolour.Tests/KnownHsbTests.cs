using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownHsbTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Hsb(0, 1.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Hsb(120, 1.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Hsb(240, 1.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Hsb(0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertColour(white, new Hsb(0, 0.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Achromatic()
    {
        var grey = StandardRgb.Grey;
        Assert.That(grey.Hsb.ToString().Contains(NoHue));
    }
}