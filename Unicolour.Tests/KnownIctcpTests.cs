using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownIctcpTests
{
    private const double Tolerance = 0.0005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Ictcp>(red, new(0.42785, -0.11574, 0.2788), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Ictcp>(green, new(0.53975, -0.28121, -0.04946), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Ictcp>(blue, new(0.35607, 0.26914, -0.16143), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Ictcp>(black, new(0.00000, 0.00000, 0.00000), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Ictcp>(white, new(0.58069, 0.00000, 0.00000), Tolerance);
    }
}