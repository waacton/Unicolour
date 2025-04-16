using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownOklrchTests
{
    private const double Tolerance = 0.00005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Oklrch>(red, new(0.56808, 0.25768, 29.23), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Oklrch>(green, new(0.84453, 0.29483, 142.50), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Oklrch>(blue, new(0.36657, 0.31321, 264.05), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Oklrch>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        Assert.That(white.Oklrch.L, Is.EqualTo(1.0).Within(Tolerance));
        Assert.That(white.Oklrch.C, Is.EqualTo(0.0).Within(Tolerance));
    }
}