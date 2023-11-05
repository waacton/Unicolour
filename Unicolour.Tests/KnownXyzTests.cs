namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownXyzTests
{
    private const double Tolerance = 0.0000005;

    [TestCase(Illuminant.D65, 0.412456, 0.212673, 0.019334)]
    [TestCase(Illuminant.D50, 0.436075, 0.222504, 0.013932)]
    public void Red(Illuminant illuminant, double x, double y, double z)
    {
        var red = ColourLimits.Rgb[ColourLimit.Red].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Xyz>(red, new(x, y, z), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.357576, 0.715152, 0.119192)]
    [TestCase(Illuminant.D50, 0.385065, 0.716879, 0.097105)]
    public void Green(Illuminant illuminant, double x, double y, double z)
    {
        var green = ColourLimits.Rgb[ColourLimit.Green].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Xyz>(green, new(x, y, z), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.180437, 0.072175, 0.950304)]
    [TestCase(Illuminant.D50, 0.143080, 0.060617, 0.714173)]
    public void Blue(Illuminant illuminant, double x, double y, double z)
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Xyz>(blue, new(x, y, z), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.0, 0.0, 0.0)]
    [TestCase(Illuminant.D50, 0.0, 0.0, 0.0)]
    public void Black(Illuminant illuminant, double x, double y, double z)
    {
        var black = ColourLimits.Rgb[ColourLimit.Black].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Xyz>(black, new(x, y, z), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.950470, 1.000000, 1.088830)]
    [TestCase(Illuminant.D50, 0.964220, 1.000000, 0.825210)]
    public void White(Illuminant illuminant, double x, double y, double z)
    {
        var white = ColourLimits.Rgb[ColourLimit.White].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        TestUtils.AssertTriplet<Xyz>(white, new(x, y, z), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.312727, 0.329023)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496)]
    [TestCase(Illuminant.E, 0.333333, 0.333333)]
    public void BlackChromaticity(Illuminant illuminant, double x, double y)
    {
        var unicolour = new Unicolour(ColourSpace.Xyz, ConfigUtils.GetConfigWithStandardRgb(illuminant), 0.0, 0.0, 0.0);
        TestUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, 0.0), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.333333, 0.333333)]
    [TestCase(Illuminant.D50, 0.333333, 0.333333)]
    [TestCase(Illuminant.E, 0.333333, 0.333333)]
    public void GreyChromaticity(Illuminant illuminant, double x, double y)
    {
        var unicolour = new Unicolour(ColourSpace.Xyz, ConfigUtils.GetConfigWithStandardRgb(illuminant), 0.5, 0.5, 0.5);
        TestUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, 0.5), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.333333, 0.333333)]
    [TestCase(Illuminant.D50, 0.333333, 0.333333)]
    [TestCase(Illuminant.E, 0.333333, 0.333333)]
    public void WhiteChromaticity(Illuminant illuminant, double x, double y)
    {
        var unicolour = new Unicolour(ColourSpace.Xyz, ConfigUtils.GetConfigWithStandardRgb(illuminant), 1, 1, 1);
        TestUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, 1.0), Tolerance);
    }
}