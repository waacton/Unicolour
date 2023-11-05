namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownLchabTests
{
    private const double Tolerance = 0.00005;

    [TestCase(Illuminant.D65, 53.2408, 104.5518, 39.9990)]
    [TestCase(Illuminant.D50, 54.2917, 106.8390, 40.8526)]
    public void Red(Illuminant illuminant, double l, double c, double h)
    {
        var red = ColourLimits.Rgb[ColourLimit.Red].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchab>(red, new(l, c, h), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 87.7347, 119.7759, 136.0160)]
    [TestCase(Illuminant.D50, 87.8181, 113.3397, 134.3912)]
    public void Green(Illuminant illuminant, double l, double c, double h)
    {
        var green = ColourLimits.Rgb[ColourLimit.Green].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchab>(green, new(l, c, h), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 32.2970, 133.8076, 306.2849)]
    [TestCase(Illuminant.D50, 29.5676, 131.2070, 301.3685)]
    public void Blue(Illuminant illuminant, double l, double c, double h)
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchab>(blue, new(l, c, h), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.0, 0.0, 0.0)]
    [TestCase(Illuminant.D50, 0.0, 0.0, 0.0)]
    public void Black(Illuminant illuminant, double l, double c, double h)
    {
        var black = ColourLimits.Rgb[ColourLimit.Black].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchab>(black, new(l, c, h), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 100.0000, 0.0, 270.0)]
    [TestCase(Illuminant.D50, 100.0000, 0.0, 0.0)]
    public void White(Illuminant illuminant, double l, double c, double h)
    {
        var white = ColourLimits.Rgb[ColourLimit.White].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Lchab>(white, new(l, c, h), Tolerance);
    }
}