using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownJzazbzTests
{
    private const double Tolerance = 0.0005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Jzazbz(0.13438, 0.11789, 0.11188), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Jzazbz(0.17681, -0.10904, 0.11899), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Jzazbz(0.09577, -0.04085, -0.18585), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Jzazbz(0.00000, 0.00000, 0.00000), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertColour(white, new Jzazbz(0.22207, -0.00016, -0.00012), Tolerance);
    }
}