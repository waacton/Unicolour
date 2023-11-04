namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownLabTests
{
    private const double Tolerance = 0.00005;

    [TestCase(Illuminant.D65, 53.2408, 80.0925, 67.2032)]
    [TestCase(Illuminant.D50, 54.2917, 80.8125, 69.8851)]
    public void Red(Illuminant illuminant, double l, double a, double b)
    {
        var red = ColourLimits.Rgb[ColourLimit.Red].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Lab>(red, new(l, a, b), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 87.7347, -86.1827, 83.1793)]
    [TestCase(Illuminant.D50, 87.8181, -79.2873, 80.9902)]
    public void Green(Illuminant illuminant, double l, double a, double b)
    {
        var green = ColourLimits.Rgb[ColourLimit.Green].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Lab>(green, new(l, a, b), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 32.2970, 79.1875, -107.8602)]
    [TestCase(Illuminant.D50, 29.5676, 68.2986, -112.0294)]
    public void Blue(Illuminant illuminant, double l, double a, double b)
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Lab>(blue, new(l, a, b), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.0, 0.0, 0.0)]
    [TestCase(Illuminant.D50, 0.0, 0.0, 0.0)]
    public void Black(Illuminant illuminant, double l, double a, double b)
    {
        var black = ColourLimits.Rgb[ColourLimit.Black].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Lab>(black, new(l, a, b), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 100.0000, 0.0, 0.0)]
    [TestCase(Illuminant.D50, 100.0000, 0.0, 0.0)]
    public void White(Illuminant illuminant, double l, double a, double b)
    {
        var white = ColourLimits.Rgb[ColourLimit.White].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Lab>(white, new(l, a, b), Tolerance);
    }
}