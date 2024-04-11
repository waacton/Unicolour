namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

/*
 * CMY / CMYK is not integrated into Unicolour
 * and tests will need to change if calibrated CMYK using ICC profiles are implemented
 */

public class KnownCmykTests
{
    private const double Tolerance = 0.00000000001;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        AssertCmy(red, new[] { 0.0, 1.0, 1.0 });
        AssertCmyk(red, new[] { 0.0, 1.0, 1.0, 0.0 });
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        AssertCmy(green, new[] { 1.0, 0.0, 1.0 });
        AssertCmyk(green, new[] { 1.0, 0.0, 1.0, 0.0 });
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        AssertCmy(blue, new[] { 1.0, 1.0, 0.0 });
        AssertCmyk(blue, new[] { 1.0, 1.0, 0.0, 0.0 });
    }
    
    [Test]
    public void Cyan()
    {
        var cyan = StandardRgb.Cyan;
        AssertCmy(cyan, new[] { 1.0, 0.0, 0.0 });
        AssertCmyk(cyan, new[] { 1.0, 0.0, 0.0, 0.0 });
    }
    
    [Test]
    public void Magenta()
    {
        var magenta = StandardRgb.Magenta;
        AssertCmy(magenta, new[] { 0.0, 1.0, 0.0 });
        AssertCmyk(magenta, new[] { 0.0, 1.0, 0.0, 0.0 });
    }
    
    [Test]
    public void Yellow()
    {
        var yellow = StandardRgb.Yellow;
        AssertCmy(yellow, new[] { 0.0, 0.0, 1.0 });
        AssertCmyk(yellow, new[] { 0.0, 0.0, 1.0, 0.0 });
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        AssertCmy(black, new[] { 1.0, 1.0, 1.0 });
        AssertCmyk(black, new[] { 0.0, 0.0, 0.0, 1.0 });
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        AssertCmy(white, new[] { 0.0, 0.0, 0.0 });
        AssertCmyk(white, new[] { 0.0, 0.0, 0.0, 0.0 });
    }
    
    [Test]
    public void Grey()
    {
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);
        AssertCmy(grey, new[] { 0.5, 0.5, 0.5 });
        AssertCmyk(grey, new[] { 0.0, 0.0, 0.0, 0.5 });
    }
    
    [Test]
    public void Firebrick()
    {
        var firebrick = new Unicolour("#B22222");
        AssertCmy(firebrick, new[] { 0.30, 0.87, 0.87 }, 0.01);
        AssertCmyk(firebrick, new[] { 0.0, 0.81, 0.81, 0.30 }, 0.01);
    }

    private static void AssertCmy(Unicolour original, double[] expected, double tolerance = Tolerance)
    {
        var actual = Cmy.FromUnicolour(original);
        Assert.That(actual[0], Is.EqualTo(expected[0]).Within(tolerance));
        Assert.That(actual[1], Is.EqualTo(expected[1]).Within(tolerance));
        Assert.That(actual[2], Is.EqualTo(expected[2]).Within(tolerance));

        var unicolour = Cmy.ToUnicolour(actual);
        TestUtils.AssertTriplet<Rgb>(unicolour, original.Rgb.Triplet, tolerance);
    }
    
    private static void AssertCmyk(Unicolour original, double[] expected, double tolerance = Tolerance)
    {
        var actual = Cmyk.FromUnicolour(original);
        Assert.That(actual[0], Is.EqualTo(expected[0]).Within(tolerance));
        Assert.That(actual[1], Is.EqualTo(expected[1]).Within(tolerance));
        Assert.That(actual[2], Is.EqualTo(expected[2]).Within(tolerance));
        Assert.That(actual[3], Is.EqualTo(expected[3]).Within(tolerance));
        
        var unicolour = Cmyk.ToUnicolour(actual);
        TestUtils.AssertTriplet<Rgb>(unicolour, original.Rgb.Triplet, tolerance);
    }
}