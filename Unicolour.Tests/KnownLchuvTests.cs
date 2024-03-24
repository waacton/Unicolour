namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownLchuvTests
{
    private const double Tolerance = 0.00005;

    [TestCase(nameof(Illuminant.D65), 53.2408, 179.0414, 12.1740)]
    [TestCase(nameof(Illuminant.D50), 54.2917, 176.9569, 8.4352)]
    public void Red(string illuminantName, double l, double c, double h)
    {
        var red = StandardRgb.Red.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchuv>(red, new(l, c, h), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 87.7347, 135.7804, 127.7236)]
    [TestCase(nameof(Illuminant.D50), 87.8181, 121.7606, 134.2323)]
    public void Green(string illuminantName, double l, double c, double h)
    {
        var green = StandardRgb.Green.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchuv>(green, new(l, c, h), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 32.2970, 130.6812, 265.8727)]
    [TestCase(nameof(Illuminant.D50), 29.5676, 122.5132, 264.5953)]
    public void Blue(string illuminantName, double l, double c, double h)
    {
        var blue = StandardRgb.Blue.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchuv>(blue, new(l, c, h), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.0, 0.0, 0.0)]
    [TestCase(nameof(Illuminant.D50), 0.0, 0.0, 0.0)]
    public void Black(string illuminantName, double l, double c, double h)
    {
        var black = StandardRgb.Black.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchuv>(black, new(l, c, h), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 100.0000, 0.0)]
    [TestCase(nameof(Illuminant.D50), 100.0000, 0.0)]
    public void White(string illuminantName, double l, double c)
    {
        var white = StandardRgb.White.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        Assert.That(white.Lchuv.L, Is.EqualTo(l).Within(Tolerance));
        Assert.That(white.Lchuv.C, Is.EqualTo(c).Within(Tolerance));
    }
}