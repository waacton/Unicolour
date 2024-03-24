namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownLabTests
{
    private const double Tolerance = 0.00005;

    [TestCase(nameof(Illuminant.D65), 53.2408, 80.0925, 67.2032)]
    [TestCase(nameof(Illuminant.D50), 54.2917, 80.8125, 69.8851)]
    public void Red(string illuminantName, double l, double a, double b)
    {
        var red = StandardRgb.Red.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lab>(red, new(l, a, b), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 87.7347, -86.1827, 83.1793)]
    [TestCase(nameof(Illuminant.D50), 87.8181, -79.2873, 80.9902)]
    public void Green(string illuminantName, double l, double a, double b)
    {
        var green = StandardRgb.Green.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lab>(green, new(l, a, b), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 32.2970, 79.1875, -107.8602)]
    [TestCase(nameof(Illuminant.D50), 29.5676, 68.2986, -112.0294)]
    public void Blue(string illuminantName, double l, double a, double b)
    {
        var blue = StandardRgb.Blue.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lab>(blue, new(l, a, b), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.0, 0.0, 0.0)]
    [TestCase(nameof(Illuminant.D50), 0.0, 0.0, 0.0)]
    public void Black(string illuminantName, double l, double a, double b)
    {
        var black = StandardRgb.Black.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lab>(black, new(l, a, b), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 100.0000, 0.0, 0.0)]
    [TestCase(nameof(Illuminant.D50), 100.0000, 0.0, 0.0)]
    public void White(string illuminantName, double l, double a, double b)
    {
        var white = StandardRgb.White.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lab>(white, new(l, a, b), Tolerance);
    }
}