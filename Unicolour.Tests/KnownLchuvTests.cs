namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownLchuvTests
{
    private const double Tolerance = 0.00005;

    [TestCase(Illuminant.D65, 53.2408, 179.0414, 12.1740)]
    [TestCase(Illuminant.D50, 54.2917, 176.9569, 8.4352)]
    public void Red(Illuminant illuminant, double l, double c, double h)
    {
        var red = ColourLimits.Rgb[ColourLimit.Red].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchuv>(red, new(l, c, h), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 87.7347, 135.7804, 127.7236)]
    [TestCase(Illuminant.D50, 87.8181, 121.7606, 134.2323)]
    public void Green(Illuminant illuminant, double l, double c, double h)
    {
        var green = ColourLimits.Rgb[ColourLimit.Green].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchuv>(green, new(l, c, h), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 32.2970, 130.6812, 265.8727)]
    [TestCase(Illuminant.D50, 29.5676, 122.5132, 264.5953)]
    public void Blue(Illuminant illuminant, double l, double c, double h)
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchuv>(blue, new(l, c, h), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.0, 0.0, 0.0)]
    [TestCase(Illuminant.D50, 0.0, 0.0, 0.0)]
    public void Black(Illuminant illuminant, double l, double c, double h)
    {
        var black = ColourLimits.Rgb[ColourLimit.Black].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchuv>(black, new(l, c, h), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 100.0000, 0.0)]
    [TestCase(Illuminant.D50, 100.0000, 0.0)]
    public void White(Illuminant illuminant, double l, double c)
    {
        var white = ColourLimits.Rgb[ColourLimit.White].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        Assert.That(white.Lchuv.L, Is.EqualTo(l).Within(Tolerance));
        Assert.That(white.Lchuv.C, Is.EqualTo(c).Within(Tolerance));
    }
}