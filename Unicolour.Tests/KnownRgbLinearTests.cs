namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownRgbLinearTests
{
    private const double Tolerance = 0.0005;
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.Standard, 0.5, 0.735)]
    [TestCase(ConfigUtils.RgbType.Standard, 1.0, 1.0)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.5, 0.735)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 1.0, 1.0)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.5, 0.705)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 1.0, 1.0)]
    public void Red(ConfigUtils.RgbType rgbType, double linear, double nonlinear)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var red = Unicolour.FromRgbLinear(config, linear, 0, 0);
        AssertUtils.AssertTriplet(red.Rgb.Triplet, new(nonlinear, 0, 0), Tolerance);
    }
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.Standard, 0.5, 0.735)]
    [TestCase(ConfigUtils.RgbType.Standard, 1.0, 1.0)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.5, 0.735)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 1.0, 1.0)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.5, 0.705)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 1.0, 1.0)]
    public void Green(ConfigUtils.RgbType rgbType, double linear, double nonlinear)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var green = Unicolour.FromRgbLinear(config, 0, linear, 0);
        AssertUtils.AssertTriplet(green.Rgb.Triplet, new(0, nonlinear, 0), Tolerance);
    }
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.Standard, 0.5, 0.735)]
    [TestCase(ConfigUtils.RgbType.Standard, 1.0, 1.0)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.5, 0.735)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 1.0, 1.0)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.0, 0.0)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.5, 0.705)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 1.0, 1.0)]
    public void Blue(ConfigUtils.RgbType rgbType, double linear, double nonlinear)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var blue = Unicolour.FromRgbLinear(config, 0, 0, linear);
        AssertUtils.AssertTriplet(blue.Rgb.Triplet, new(0, 0, nonlinear), Tolerance);
    }
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.3127, 0.3290)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.3127, 0.3290)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.3127, 0.3290)]
    public void Black(ConfigUtils.RgbType rgbType, double expectedX, double expectedY)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var black = Unicolour.FromRgbLinear(config, 0, 0, 0);
        Assert.That(black.Xyy.Chromaticity.X, Is.EqualTo(expectedX).Within(0.00005));
        Assert.That(black.Xyy.Chromaticity.Y, Is.EqualTo(expectedY).Within(0.00005));
        Assert.That(black.Xyy.Luminance, Is.EqualTo(0).Within(Tolerance));
    }
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.3127, 0.3290)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.3127, 0.3290)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.3127, 0.3290)]
    public void White(ConfigUtils.RgbType rgbType, double expectedX, double expectedY)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var white = Unicolour.FromRgbLinear(config, 1, 1, 1);
        Assert.That(white.Xyy.Chromaticity.X, Is.EqualTo(expectedX).Within(0.00005));
        Assert.That(white.Xyy.Chromaticity.Y, Is.EqualTo(expectedY).Within(0.00005));
        Assert.That(white.Xyy.Luminance, Is.EqualTo(1).Within(Tolerance));
    }
}