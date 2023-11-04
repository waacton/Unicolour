namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownLuvTests
{
    private const double Tolerance = 0.00005;

    [TestCase(Illuminant.D65, 53.2408, 175.0151, 37.7564)]
    [TestCase(Illuminant.D50, 54.2917, 175.0426, 25.9580)]
    public void Red(Illuminant illuminant, double l, double u, double v)
    {
        var red = ColourLimits.Rgb[ColourLimit.Red].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Luv>(red, new(l, u, v), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 87.7347, -83.0776, 107.3985)]
    [TestCase(Illuminant.D50, 87.8181, -84.9365, 87.2436)]
    public void Green(Illuminant illuminant, double l, double u, double v)
    {
        var green = ColourLimits.Rgb[ColourLimit.Green].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Luv>(green, new(l, u, v), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 32.2970, -9.4054, -130.3423)]
    [TestCase(Illuminant.D50, 29.5676, -11.5396, -121.9686)]
    public void Blue(Illuminant illuminant, double l, double u, double v)
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Luv>(blue, new(l, u, v), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.0, 0.0, 0.0)]
    [TestCase(Illuminant.D50, 0.0, 0.0, 0.0)]
    public void Black(Illuminant illuminant, double l, double u, double v)
    {
        var black = ColourLimits.Rgb[ColourLimit.Black].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Luv>(black, new(l, u, v), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 100.0000, 0.0, 0.0)]
    [TestCase(Illuminant.D50, 100.0000, 0.0, 0.0)]
    public void White(Illuminant illuminant, double l, double u, double v)
    {
        var white = ColourLimits.Rgb[ColourLimit.White].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Luv>(white, new(l, u, v), Tolerance);
    }
}