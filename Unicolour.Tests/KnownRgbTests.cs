using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownRgbTests
{
    private const double Tolerance = 0.00000000001;
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.640, 0.330)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.680, 0.320)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.708, 0.292)]
    public void Red(ConfigUtils.RgbType rgbType, double expectedX, double expectedY)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var red = new Unicolour(config, ColourSpace.Rgb, 1, 0, 0);
        Assert.That(red.Xyy.Chromaticity.X, Is.EqualTo(expectedX).Within(Tolerance));
        Assert.That(red.Xyy.Chromaticity.Y, Is.EqualTo(expectedY).Within(Tolerance));
    }
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.300, 0.600)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.265, 0.690)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.170, 0.797)]
    public void Green(ConfigUtils.RgbType rgbType, double expectedX, double expectedY)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var green = new Unicolour(config, ColourSpace.Rgb, 0, 1, 0);
        Assert.That(green.Xyy.Chromaticity.X, Is.EqualTo(expectedX).Within(Tolerance));
        Assert.That(green.Xyy.Chromaticity.Y, Is.EqualTo(expectedY).Within(Tolerance));
    }
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.150, 0.060)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.150, 0.060)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.131, 0.046)]
    public void Blue(ConfigUtils.RgbType rgbType, double expectedX, double expectedY)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var blue = new Unicolour(config, ColourSpace.Rgb, 0, 0, 1);
        Assert.That(blue.Xyy.Chromaticity.X, Is.EqualTo(expectedX).Within(Tolerance));
        Assert.That(blue.Xyy.Chromaticity.Y, Is.EqualTo(expectedY).Within(Tolerance));
    }
    
    [TestCase(ConfigUtils.RgbType.Standard, 0.3127, 0.3290)]
    [TestCase(ConfigUtils.RgbType.DisplayP3, 0.3127, 0.3290)]
    [TestCase(ConfigUtils.RgbType.Rec2020, 0.3127, 0.3290)]
    public void Black(ConfigUtils.RgbType rgbType, double expectedX, double expectedY)
    {
        var config = ConfigUtils.GetConfigWithXyzD65(rgbType);
        var black = new Unicolour(config, ColourSpace.Rgb, 0, 0, 0);
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
        var white = new Unicolour(config, ColourSpace.Rgb, 1, 1, 1);
        Assert.That(white.Xyy.Chromaticity.X, Is.EqualTo(expectedX).Within(0.00005));
        Assert.That(white.Xyy.Chromaticity.Y, Is.EqualTo(expectedY).Within(0.00005));
        Assert.That(white.Xyy.Luminance, Is.EqualTo(1).Within(Tolerance));
    }
}