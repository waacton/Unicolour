namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownHpluvTests
{
    private const double Tolerance = 0.075;

    [Test]
    public void Red()
    {
        var red = ColourLimits.Rgb[ColourLimit.Red];
        TestUtils.AssertTriplet<Hpluv>(red, new(12.177, 426.75, 53.237), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = ColourLimits.Rgb[ColourLimit.Green];
        TestUtils.AssertTriplet<Hpluv>(green, new(127.72, 490.15, 87.736), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue];
        TestUtils.AssertTriplet<Hpluv>(blue, new(265.87, 513.41, 32.301), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = ColourLimits.Rgb[ColourLimit.Black];
        TestUtils.AssertTriplet<Hpluv>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = ColourLimits.Rgb[ColourLimit.White];
        TestUtils.AssertTriplet<Hpluv>(white, new(180.0, 0.0, 100.0), Tolerance);
    }
}