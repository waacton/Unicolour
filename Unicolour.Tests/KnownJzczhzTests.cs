namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownJzczhzTests
{
    private const double Tolerance = 0.0005;
    private static readonly Configuration Jzazbz203Config = new(jzazbzScalar: 203);
    
    [Test]
    public void Red()
    {
        var red = ColourLimits.Rgb[ColourLimit.Red].ConvertToConfiguration(Jzazbz203Config);
        TestUtils.AssertTriplet<Jzczhz>(red, new(0.13438, 0.16252, 43.502), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = ColourLimits.Rgb[ColourLimit.Green].ConvertToConfiguration(Jzazbz203Config);
        TestUtils.AssertTriplet<Jzczhz>(green, new(0.17681, 0.1614, 132.5), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue].ConvertToConfiguration(Jzazbz203Config);
        TestUtils.AssertTriplet<Jzczhz>(blue, new(0.09577, 0.19029, 257.61), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = ColourLimits.Rgb[ColourLimit.Black].ConvertToConfiguration(Jzazbz203Config);
        Assert.That(black.Jzczhz.J, Is.EqualTo(0.0).Within(Tolerance));
        Assert.That(black.Jzczhz.C, Is.EqualTo(0.0).Within(Tolerance));
    }
    
    [Test]
    public void White()
    {
        var white = ColourLimits.Rgb[ColourLimit.White].ConvertToConfiguration(Jzazbz203Config);
        Assert.That(white.Jzczhz.J, Is.EqualTo(0.22207).Within(Tolerance));
        Assert.That(white.Jzczhz.C, Is.EqualTo(0.0002).Within(Tolerance));
    }
}