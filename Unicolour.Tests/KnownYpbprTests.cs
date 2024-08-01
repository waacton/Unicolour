using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownYpbprTests
{
    private const double Tolerance = 0.0000005; 
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Ypbpr>(red, new(0.299, -0.168736, 0.5), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Ypbpr>(green, new(0.587, -0.331264, -0.418688), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Ypbpr>(blue, new(0.114, 0.5, -0.081312), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Ypbpr>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Ypbpr>(white, new(1.0, 0.0, 0.0), Tolerance);
    }
}