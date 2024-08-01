using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownYiqTests
{
    private const double Tolerance = 0.0000005; 
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Yiq>(red, new(0.299, 0.595081, 0.211008), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Yiq>(green, new(0.587, -0.273875, -0.522286), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Yiq>(blue, new(0.114, -0.321205, 0.311277), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Yiq>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Yiq>(white, new(1.0, 0.0, 0.0), Tolerance);
    }
}