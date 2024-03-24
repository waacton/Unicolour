namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownLuvTests
{
    private const double Tolerance = 0.00005;

    [TestCase(nameof(Illuminant.D65), 53.2408, 175.0151, 37.7564)]
    [TestCase(nameof(Illuminant.D50), 54.2917, 175.0426, 25.9580)]
    public void Red(string illuminantName, double l, double u, double v)
    {
        var red = StandardRgb.Red.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Luv>(red, new(l, u, v), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 87.7347, -83.0776, 107.3985)]
    [TestCase(nameof(Illuminant.D50), 87.8181, -84.9365, 87.2436)]
    public void Green(string illuminantName, double l, double u, double v)
    {
        var green = StandardRgb.Green.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Luv>(green, new(l, u, v), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 32.2970, -9.4054, -130.3423)]
    [TestCase(nameof(Illuminant.D50), 29.5676, -11.5396, -121.9686)]
    public void Blue(string illuminantName, double l, double u, double v)
    {
        var blue = StandardRgb.Blue.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Luv>(blue, new(l, u, v), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.0, 0.0, 0.0)]
    [TestCase(nameof(Illuminant.D50), 0.0, 0.0, 0.0)]
    public void Black(string illuminantName, double l, double u, double v)
    {
        var black = StandardRgb.Black.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Luv>(black, new(l, u, v), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 100.0000, 0.0, 0.0)]
    [TestCase(nameof(Illuminant.D50), 100.0000, 0.0, 0.0)]
    public void White(string illuminantName, double l, double u, double v)
    {
        var white = StandardRgb.White.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Luv>(white, new(l, u, v), Tolerance);
    }
}