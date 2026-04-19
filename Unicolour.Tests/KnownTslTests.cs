using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownTslTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Tsl(0.573791808825217 * 360, 1.0, 0.299), Tolerance);
    }
    
    [Test]
    public void Orange()
    {
        var orange = new Unicolour(ColourSpace.Rgb, 1, 0.5, 0);
        TestUtils.AssertColour(orange, new Tsl(0.5 * 360, 0.447213595499958, 0.5925), Tolerance);
    }
    
    [Test]
    public void Yellow()
    {
        var yellow = StandardRgb.Yellow;
        TestUtils.AssertColour(yellow, new Tsl(0.375 * 360, 0.316227766016838, 0.886), Tolerance);
    }
    
    [Test]
    public void Chartreuse()
    {
        var chartreuse = new Unicolour(ColourSpace.Rgb, 0.5, 1, 0);
        TestUtils.AssertColour(chartreuse, new Tsl(0.25 * 360, 0.447213595499958, 0.7365), Tolerance);
    }

    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Tsl(0.176208191174783 * 360, 1.0, 0.587), Tolerance);
    }
    
    [Test]
    public void Mint()
    {
        var mint = new Unicolour(ColourSpace.Rgb, 0, 1, 0.5);
        TestUtils.AssertColour(mint, new Tsl(0.125 * 360, 0.632455532033676, 0.644), Tolerance);
    }
    
    [Test]
    public void Cyan()
    {
        var cyan = StandardRgb.Cyan;
        TestUtils.AssertColour(cyan, new Tsl(0.073791808825217 * 360, 0.5, 0.701), Tolerance);
    }
    
    [Test]
    public void Azure()
    {
        var azure = new Unicolour(ColourSpace.Rgb, 0, 0.5, 1);
        TestUtils.AssertColour(azure, new Tsl(0 * 360, 0.447213595499958, 0.4075), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Tsl(0.875 * 360, 0.632455532033676, 0.114), Tolerance);
    }
    
    [Test]
    public void Violet()
    {
        var violet = new Unicolour(ColourSpace.Rgb, 0.5, 0, 1);
        TestUtils.AssertColour(violet, new Tsl(0.75 * 360, 0.447213595499958, 0.2635), Tolerance);
    }
    
    [Test]
    public void Magenta()
    {
        var magenta = StandardRgb.Magenta;
        TestUtils.AssertColour(magenta, new Tsl(0.676208191174783 * 360, 0.5, 0.413), Tolerance);
    }
    
    [Test]
    public void Rose()
    {
        var rose = new Unicolour(ColourSpace.Rgb, 1, 0, 0.5);
        TestUtils.AssertColour(rose, new Tsl(0.625 * 360, 0.632455532033676, 0.356), Tolerance);
    }

    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Tsl(0.0 * 360, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertColour(grey, new Tsl(0.0 * 360, 0.0, 0.5), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertColour(white, new Tsl(0.0 * 360, 0.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Achromatic()
    {
        var grey = StandardRgb.Grey;
        Assert.That(grey.Tsl.ToString().Contains(NoHue));
    }
}