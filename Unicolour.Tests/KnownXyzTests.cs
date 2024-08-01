using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownXyzTests
{
    private const double Tolerance = 0.0000005;

    [TestCase(nameof(Illuminant.D65), 0.412456, 0.212673, 0.019334)]
    [TestCase(nameof(Illuminant.D50), 0.436075, 0.222504, 0.013932)]
    public void Red(string illuminantName, double x, double y, double z)
    {
        var red = StandardRgb.Red.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Xyz>(red, new(x, y, z), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.357576, 0.715152, 0.119192)]
    [TestCase(nameof(Illuminant.D50), 0.385065, 0.716879, 0.097105)]
    public void Green(string illuminantName, double x, double y, double z)
    {
        var green = StandardRgb.Green.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Xyz>(green, new(x, y, z), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.180437, 0.072175, 0.950304)]
    [TestCase(nameof(Illuminant.D50), 0.143080, 0.060617, 0.714173)]
    public void Blue(string illuminantName, double x, double y, double z)
    {
        var blue = StandardRgb.Blue.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Xyz>(blue, new(x, y, z), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.0, 0.0, 0.0)]
    [TestCase(nameof(Illuminant.D50), 0.0, 0.0, 0.0)]
    public void Black(string illuminantName, double x, double y, double z)
    {
        var black = StandardRgb.Black.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Xyz>(black, new(x, y, z), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.950470, 1.000000, 1.088830)]
    [TestCase(nameof(Illuminant.D50), 0.964220, 1.000000, 0.825210)]
    public void White(string illuminantName, double x, double y, double z)
    {
        var white = StandardRgb.White.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        TestUtils.AssertTriplet<Xyz>(white, new(x, y, z), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.312727, 0.329023)]
    [TestCase(nameof(Illuminant.D50), 0.345669, 0.358496)]
    [TestCase(nameof(Illuminant.E), 0.333333, 0.333333)]
    public void BlackChromaticity(string illuminantName, double x, double y)
    {
        var unicolour = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), ColourSpace.Xyz, 0.0, 0.0, 0.0);
        TestUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, 0.0), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.333333, 0.333333)]
    [TestCase(nameof(Illuminant.D50), 0.333333, 0.333333)]
    [TestCase(nameof(Illuminant.E), 0.333333, 0.333333)]
    public void GreyChromaticity(string illuminantName, double x, double y)
    {
        var unicolour = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), ColourSpace.Xyz, 0.5, 0.5, 0.5);
        TestUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, 0.5), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65), 0.333333, 0.333333)]
    [TestCase(nameof(Illuminant.D50), 0.333333, 0.333333)]
    [TestCase(nameof(Illuminant.E), 0.333333, 0.333333)]
    public void WhiteChromaticity(string illuminantName, double x, double y)
    {
        var unicolour = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), ColourSpace.Xyz, 1, 1, 1);
        TestUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, 1.0), Tolerance);
    }
}