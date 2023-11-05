namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownHsbTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = ColourLimits.Rgb[ColourLimit.Red];
        TestUtils.AssertTriplet<Hsb>(red, new(0, 1.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = ColourLimits.Rgb[ColourLimit.Green];
        TestUtils.AssertTriplet<Hsb>(green, new(120, 1.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue];
        TestUtils.AssertTriplet<Hsb>(blue, new(240, 1.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = ColourLimits.Rgb[ColourLimit.Black];
        TestUtils.AssertTriplet<Hsb>(black, new(0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = ColourLimits.Rgb[ColourLimit.White];
        TestUtils.AssertTriplet<Hsb>(white, new(0, 0.0, 1.0), Tolerance);
    }
}