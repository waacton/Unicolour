using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownJzczhzTests
{
    private const double Tolerance = 0.0005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Jzczhz>(red, new(0.13438, 0.16252, 43.502), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Jzczhz>(green, new(0.17681, 0.1614, 132.5), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Jzczhz>(blue, new(0.09577, 0.19029, 257.61), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        Assert.That(black.Jzczhz.J, Is.EqualTo(0.0).Within(Tolerance));
        Assert.That(black.Jzczhz.C, Is.EqualTo(0.0).Within(Tolerance));
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        Assert.That(white.Jzczhz.J, Is.EqualTo(0.22207).Within(Tolerance));
        Assert.That(white.Jzczhz.C, Is.EqualTo(0.0002).Within(Tolerance));
    }
}