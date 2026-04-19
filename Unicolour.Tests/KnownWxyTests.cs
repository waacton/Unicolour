using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownWxyTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Wxy(611.2792816164048, 0.9167808317011886, 0.21267285140562237), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Wxy(549.1325521262072, 0.7344486823607274, 0.7151521552878177), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Wxy(464.30730389823566, 0.9249258072942851, 0.07217499330655955), Tolerance);
    }
    
    [Test]
    public void Cyan()
    {
        var cyan = StandardRgb.Cyan;
        TestUtils.AssertColour(cyan, new Wxy(491.48970978428736, 0.3211884743401833, 0.7873271485943772), Tolerance);
    }
    
    [Test]
    public void Magenta()
    {
        var magenta = StandardRgb.Magenta;
        TestUtils.AssertColour(magenta, new Wxy(-549.1325521262073, 0.6872746354096638, 0.28484784471218194), Tolerance);
    }
    
    [Test]
    public void Yellow()
    {
        var yellow = StandardRgb.Yellow;
        TestUtils.AssertColour(yellow, new Wxy(570.4622811675213, 0.7920870446740987, 0.9278250066934401), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Wxy(360.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var (x, y) = Configuration.Default.Xyz.WhitePoint;
        var white = new Unicolour(ColourSpace.Xyy, x, y, 1.0);
        TestUtils.AssertColour(white, new Wxy(360.0, 0.0, 1.0), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertColour(grey, new Wxy(360.0, 0.0, 0.2140411404822325), Tolerance);
    }
    
    [Test]
    public void UltravioletPositive()
    {
        var violet = new Unicolour(ColourSpace.Wxy, SpectralBoundary.MinWavelength, 0.5, 0.5);
        var ultraviolet = new Unicolour(ColourSpace.Wxy, 300, 0.5, 0.5);
        TestUtils.AssertTriplet(ultraviolet.Xyy.Triplet, violet.Xyy.Triplet, Tolerance);
    }
    
    [Test]
    public void UltravioletNegative()
    {
        var violet = new Unicolour(ColourSpace.Wxy, SpectralBoundary.MinWavelength, 0.5, 0.5);
        var ultraviolet = new Unicolour(ColourSpace.Wxy, -600, 0.5, 0.5);
        TestUtils.AssertTriplet(ultraviolet.Xyy.Triplet, violet.Xyy.Triplet, Tolerance);
    }
    
    [Test]
    public void UltravioletPositiveComplementary()
    {
        var complementary = XyzConfiguration.D65.SpectralBoundary.MinNegativeWavelength;
        var violet = new Unicolour(ColourSpace.Wxy, complementary, 0.5, 0.5);
        var ultraviolet = new Unicolour(ColourSpace.Wxy, 300, 0.5, 0.5);
        TestUtils.AssertTriplet(ultraviolet.Xyy.Triplet, violet.Xyy.Triplet, Tolerance);
    }
    
    [Test]
    public void UltravioletNegativeComplementary()
    {
        var complementary = XyzConfiguration.D65.SpectralBoundary.MinNegativeWavelength;
        var violet = new Unicolour(ColourSpace.Wxy, complementary, 0.5, 0.5);
        var ultraviolet = new Unicolour(ColourSpace.Wxy, -600, 0.5, 0.5);
        TestUtils.AssertTriplet(ultraviolet.Xyy.Triplet, violet.Xyy.Triplet, Tolerance);
    }
    
    [Test]
    public void InfraredPositive()
    {
        var red = new Unicolour(ColourSpace.Wxy, SpectralBoundary.MaxWavelength, 0.5, 0.5);
        var infrared = new Unicolour(ColourSpace.Wxy, 750, 0.5, 0.5);
        TestUtils.AssertTriplet(infrared.Xyy.Triplet, red.Xyy.Triplet, Tolerance);
    }
    
    [Test]
    public void InfraredNegative()
    {
        var red = new Unicolour(ColourSpace.Wxy, SpectralBoundary.MaxWavelength, 0.5, 0.5);
        var infrared = new Unicolour(ColourSpace.Wxy, -450, 0.5, 0.5);
        TestUtils.AssertTriplet(infrared.Xyy.Triplet, red.Xyy.Triplet, Tolerance);
    }
    
    [Test]
    public void InfraredPositiveComplementary()
    {
        var complementary = XyzConfiguration.D65.SpectralBoundary.MaxNegativeWavelength;
        var red = new Unicolour(ColourSpace.Wxy, complementary, 0.5, 0.5);
        var infrared = new Unicolour(ColourSpace.Wxy, 750, 0.5, 0.5);
        TestUtils.AssertTriplet(infrared.Xyy.Triplet, red.Xyy.Triplet, Tolerance);
    }
    
    [Test]
    public void InfraredNegativeComplementary()
    {
        var complementary = XyzConfiguration.D65.SpectralBoundary.MaxNegativeWavelength;
        var red = new Unicolour(ColourSpace.Wxy, complementary, 0.5, 0.5);
        var infrared = new Unicolour(ColourSpace.Wxy, -450, 0.5, 0.5);
        TestUtils.AssertTriplet(infrared.Xyy.Triplet, red.Xyy.Triplet, Tolerance);
    }
    
    [Test]
    public void Achromatic()
    {
        var grey = StandardRgb.Grey;
        Assert.That(grey.Wxy.ToString().Contains(NoHue));
    }
}