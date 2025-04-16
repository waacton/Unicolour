using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class RgbGamutTests
{
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(0.5, 0.5, 0.5)]
    [TestCase(1.0, 1.0, 1.0)]
    [TestCase(double.Epsilon, double.Epsilon, double.Epsilon)]
    public void InRgbGamut(double r, double g, double b)
    {
        var colour = new Unicolour(ColourSpace.Rgb, r, g, b);
        Assert.That(colour.IsInRgbGamut, Is.True);
    }
    
    [TestCase(-0.00001, 0.0, 0.0)]
    [TestCase(0.0, -0.00001, 0.0)]
    [TestCase(0.0, 0.0, -0.00001)]
    [TestCase(1.00001, 1.0, 1.0)]
    [TestCase(1.0, 1.00001, 1.0)]
    [TestCase(1.0, 1.0, 1.00001)]
    [TestCase(double.MaxValue, 0.5, 0.5)]
    [TestCase(0.5, double.MaxValue, 0.5)]
    [TestCase(0.5, 0.5, double.MaxValue)]
    [TestCase(double.MinValue, 0.5, 0.5)]
    [TestCase(0.5, double.MinValue, 0.5)]
    [TestCase(0.5, 0.5, double.MinValue)]
    [TestCase(double.PositiveInfinity, 0.5, 0.5)]
    [TestCase(0.5, double.PositiveInfinity, 0.5)]
    [TestCase(0.5, 0.5, double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity, 0.5, 0.5)]
    [TestCase(0.5, double.NegativeInfinity, 0.5)]
    [TestCase(0.5, 0.5, double.NegativeInfinity)]
    [TestCase(double.NaN, 0.5, 0.5)]
    [TestCase(0.5, double.NaN, 0.5)]
    [TestCase(0.5, 0.5, double.NaN)]
    public void OutRgbGamut(double r, double g, double b)
    {
        var colour = new Unicolour(ColourSpace.Rgb, r, g, b);
        Assert.That(colour.IsInRgbGamut, Is.False);
    }
}