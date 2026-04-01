using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownHsiTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Hsi(0, 1.0, 1 / 3.0), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Hsi(120, 1.0, 1 / 3.0), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Hsi(240, 1.0, 1 / 3.0), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Hsi(0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertColour(white, new Hsi(0, 0.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Achromatic()
    {
        var grey = StandardRgb.Grey;
        Assert.That(grey.Hsi.ToString().Contains(NoHue));
    }
}