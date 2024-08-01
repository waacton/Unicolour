using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownXybTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Xyb>(red, new(0.028100083161277323, 0.4881882010413151, -0.01652922538774071), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Xyb>(green, new(-0.015386116472573375, 0.714781372724691, -0.2777046155146864), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Xyb>(blue, new(0.0, 0.27812819734781813, 0.3880116647837879), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Xyb>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Xyb>(white, new(0.0, 0.8453085619621622, 2.220446049250313e-16), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertTriplet<Xyb>(grey, new(0.0, 0.4457393607565907, 0.0), Tolerance);
    }
}