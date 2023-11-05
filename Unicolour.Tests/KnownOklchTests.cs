namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownOklchTests
{
    private const double Tolerance = 0.0005;
    
    [Test]
    public void Red()
    {
        var red = ColourLimits.Rgb[ColourLimit.Red];
        TestUtils.AssertTriplet<Oklch>(red, new(0.62796, 0.25768, 29.23), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = ColourLimits.Rgb[ColourLimit.Green];
        TestUtils.AssertTriplet<Oklch>(green, new(0.86644, 0.29483, 142.50), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue];
        TestUtils.AssertTriplet<Oklch>(blue, new(0.45201, 0.31321, 264.05), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = ColourLimits.Rgb[ColourLimit.Black];
        TestUtils.AssertTriplet<Oklch>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = ColourLimits.Rgb[ColourLimit.White];
        Assert.That(white.Oklch.L, Is.EqualTo(1.0).Within(Tolerance));
        Assert.That(white.Oklch.C, Is.EqualTo(0.0).Within(Tolerance));
    }
}