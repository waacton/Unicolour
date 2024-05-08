namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownTslTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Tsl>(red, new(0.573791808825217 * 360, 1.0, 0.299), Tolerance);
    }
    
    [Test]
    public void Orange()
    {
        var orange = new Unicolour(ColourSpace.Rgb, 1, 0.5, 0);
        TestUtils.AssertTriplet<Tsl>(orange, new(0.5 * 360, 0.447213595499958, 0.5925), Tolerance);
    }
    
    [Test]
    public void Yellow()
    {
        var yellow = StandardRgb.Yellow;
        TestUtils.AssertTriplet<Tsl>(yellow, new(0.375 * 360, 0.316227766016838, 0.886), Tolerance);
    }
    
    [Test]
    public void Chartreuse()
    {
        var chartreuse = new Unicolour(ColourSpace.Rgb, 0.5, 1, 0);
        TestUtils.AssertTriplet<Tsl>(chartreuse, new(0.25 * 360, 0.447213595499958, 0.7365), Tolerance);
    }

    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Tsl>(green, new(0.176208191174783 * 360, 1.0, 0.587), Tolerance);
    }
    
    [Test]
    public void Mint()
    {
        var mint = new Unicolour(ColourSpace.Rgb, 0, 1, 0.5);
        TestUtils.AssertTriplet<Tsl>(mint, new(0.125 * 360, 0.632455532033676, 0.644), Tolerance);
    }
    
    [Test]
    public void Cyan()
    {
        var cyan = StandardRgb.Cyan;
        TestUtils.AssertTriplet<Tsl>(cyan, new(0.073791808825217 * 360, 0.5, 0.701), Tolerance);
    }
    
    [Test]
    public void Azure()
    {
        var azure = new Unicolour(ColourSpace.Rgb, 0, 0.5, 1);
        TestUtils.AssertTriplet<Tsl>(azure, new(0 * 360, 0.447213595499958, 0.4075), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Tsl>(blue, new(0.875 * 360, 0.632455532033676, 0.114), Tolerance);
    }
    
    [Test]
    public void Violet()
    {
        var violet = new Unicolour(ColourSpace.Rgb, 0.5, 0, 1);
        TestUtils.AssertTriplet<Tsl>(violet, new(0.75 * 360, 0.447213595499958, 0.2635), Tolerance);
    }
    
    [Test]
    public void Magenta()
    {
        var magenta = StandardRgb.Magenta;
        TestUtils.AssertTriplet<Tsl>(magenta, new(0.676208191174783 * 360, 0.5, 0.413), Tolerance);
    }
    
    [Test]
    public void Rose()
    {
        var rose = new Unicolour(ColourSpace.Rgb, 1, 0, 0.5);
        TestUtils.AssertTriplet<Tsl>(rose, new(0.625 * 360, 0.632455532033676, 0.356), Tolerance);
    }

    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Tsl>(black, new(0.0 * 360, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertTriplet<Tsl>(grey, new(0.0 * 360, 0.0, 0.5), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Tsl>(white, new(0.0 * 360, 0.0, 1.0), Tolerance);
    }
}