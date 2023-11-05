namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownHslTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = ColourLimits.Rgb[ColourLimit.Red];
        TestUtils.AssertTriplet<Hsl>(red, new(0, 1.0, 0.5), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = ColourLimits.Rgb[ColourLimit.Green];
        TestUtils.AssertTriplet<Hsl>(green, new(120, 1.0, 0.5), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue];
        TestUtils.AssertTriplet<Hsl>(blue, new(240, 1.0, 0.5), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = ColourLimits.Rgb[ColourLimit.Black];
        TestUtils.AssertTriplet<Hsl>(black, new(0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = ColourLimits.Rgb[ColourLimit.White];
        TestUtils.AssertTriplet<Hsl>(white, new(0, 0.0, 1.0), Tolerance);
    }
}