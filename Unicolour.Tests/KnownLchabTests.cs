using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownLchabTests
{
    private const double Tolerance = 0.00005;

    [TestCase(nameof(Illuminant.D65), 53.2408, 104.5518, 39.9990)]
    [TestCase(nameof(Illuminant.D50), 54.2917, 106.8390, 40.8526)]
    public void Red(string illuminantName, double l, double c, double h)
    {
        var red = StandardRgb.Red.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchab>(red, new(l, c, h), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 87.7347, 119.7759, 136.0160)]
    [TestCase(nameof(Illuminant.D50), 87.8181, 113.3397, 134.3912)]
    public void Green(string illuminantName, double l, double c, double h)
    {
        var green = StandardRgb.Green.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchab>(green, new(l, c, h), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 32.2970, 133.8076, 306.2849)]
    [TestCase(nameof(Illuminant.D50), 29.5676, 131.2070, 301.3685)]
    public void Blue(string illuminantName, double l, double c, double h)
    {
        var blue = StandardRgb.Blue.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchab>(blue, new(l, c, h), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.0, 0.0, 0.0)]
    [TestCase(nameof(Illuminant.D50), 0.0, 0.0, 0.0)]
    public void Black(string illuminantName, double l, double c, double h)
    {
        var black = StandardRgb.Black.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchab>(black, new(l, c, h), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 100.0000, 0.0, 270.0)]
    [TestCase(nameof(Illuminant.D50), 100.0000, 0.0, 0.0)]
    public void White(string illuminantName, double l, double c, double h)
    {
        var white = StandardRgb.White.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Lchab>(white, new(l, c, h), Tolerance);
    }
}