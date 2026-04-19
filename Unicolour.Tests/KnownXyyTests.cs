using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownXyyTests
{
    private const double Tolerance = 0.0000005;
    
    [TestCase(nameof(Illuminant.D65), 0.640000, 0.330000, 0.212673)]
    [TestCase(nameof(Illuminant.D50), 0.648427, 0.330856, 0.222504)]
    public void Red(string illuminantName, double x, double y, double expectedZ)
    {
        var red = StandardRgb.Red.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet(red.Xyy.Triplet, new(x, y, expectedZ), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.300000, 0.600000, 0.715152)]
    [TestCase(nameof(Illuminant.D50), 0.321142, 0.597873, 0.716879)]
    public void Green(string illuminantName, double x, double y, double expectedZ)
    {
        var green = StandardRgb.Green.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet(green.Xyy.Triplet, new(x, y, expectedZ), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.150000, 0.060000, 0.072175)]
    [TestCase(nameof(Illuminant.D50), 0.155883, 0.066041, 0.060617)]
    public void Blue(string illuminantName, double x, double y, double expectedZ)
    {
        var blue = StandardRgb.Blue.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet(blue.Xyy.Triplet, new(x, y, expectedZ), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.312727, 0.329023, 0.000000)]
    [TestCase(nameof(Illuminant.D50), 0.345669, 0.358496, 0.000000)]
    [TestCase(nameof(Illuminant.E), 0.333333, 0.333333, 0.000000)]
    public void Black(string illuminantName, double x, double y, double luminance)
    {
        var colour = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), ColourSpace.Rgb, 0, 0, 0);
        TestUtils.AssertTriplet(colour.Xyy.Triplet, new(x, y, luminance), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.312727, 0.329023, 0.000001)]
    [TestCase(nameof(Illuminant.D50), 0.345669, 0.358496, 0.000001)]
    [TestCase(nameof(Illuminant.E), 0.333333, 0.333333, 0.000001)]
    public void NearBlack(string illuminantName, double x, double y, double luminance)
    {
        var colour = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), ColourSpace.Rgb, 0.00001, 0.00001, 0.00001);
        TestUtils.AssertTriplet(colour.Xyy.Triplet, new(x, y, luminance), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.312727, 0.329023, 0.214041)]
    [TestCase(nameof(Illuminant.D50), 0.345669, 0.358496, 0.214041)]
    [TestCase(nameof(Illuminant.E), 0.333333, 0.333333, 0.214041)]
    public void Grey(string illuminantName, double x, double y, double luminance)
    {
        var colour = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), ColourSpace.Rgb, 0.5, 0.5, 0.5);
        TestUtils.AssertTriplet(colour.Xyy.Triplet, new(x, y, luminance), Tolerance);
    }

    [TestCase(nameof(Illuminant.D65), 0.312727, 0.329023, 1.000000)]
    [TestCase(nameof(Illuminant.D50), 0.345669, 0.358496, 1.000000)]
    [TestCase(nameof(Illuminant.E), 0.333333, 0.333333, 1.000000)]
    public void White(string illuminantName, double x, double y, double luminance)
    {
        var colour = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), ColourSpace.Rgb, 1, 1, 1);
        TestUtils.AssertTriplet(colour.Xyy.Triplet, new(x, y, luminance), Tolerance);
    }
    
    [TestCase(0.5, 0.00000000001, 50000000000, 49999999999)]
    [TestCase(0.5, -0.00000000001, -50000000000, -50000000001)]
    [TestCase(0.5, 0, double.NaN, double.NaN)]
    public void ChromaticityY(double chromaticityX, double chromaticityY, double expectedX, double expectedZ)
    {
        var (x, y, z) = new Unicolour(ColourSpace.Xyy, chromaticityX, chromaticityY, 1).Xyz;
        Assert.That(x, Is.EqualTo(expectedX));
        Assert.That(y, Is.EqualTo(1));
        Assert.That(z, Is.EqualTo(expectedZ));
    }
}