namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownXyyTests
{
    private const double Tolerance = 0.0000005;
    
    [TestCase(Illuminant.D65, 0.640000, 0.330000, 0.212673)]
    [TestCase(Illuminant.D50, 0.648427, 0.330856, 0.222504)]
    public void Red(Illuminant illuminant, double x, double y, double expectedZ)
    {
        var red = ColourLimits.Rgb[ColourLimit.Red].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Xyy>(red, new(x, y, expectedZ), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.300000, 0.600000, 0.715152)]
    [TestCase(Illuminant.D50, 0.321142, 0.597873, 0.716879)]
    public void Green(Illuminant illuminant, double x, double y, double expectedZ)
    {
        var green = ColourLimits.Rgb[ColourLimit.Green].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Xyy>(green, new(x, y, expectedZ), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.150000, 0.060000, 0.072175)]
    [TestCase(Illuminant.D50, 0.155883, 0.066041, 0.060617)]
    public void Blue(Illuminant illuminant, double x, double y, double expectedZ)
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue].ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminant));
        AssertUtils.AssertTriplet<Xyy>(blue, new(x, y, expectedZ), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.312727, 0.329023, 0.000000)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496, 0.000000)]
    [TestCase(Illuminant.E, 0.333333, 0.333333, 0.000000)]
    public void Black(Illuminant illuminant, double x, double y, double luminance)
    {
        var unicolour = Unicolour.FromRgb(ConfigUtils.GetConfigWithStandardRgb(illuminant), 0, 0, 0);
        AssertUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, luminance), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.312727, 0.329023, 0.000001)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496, 0.000001)]
    [TestCase(Illuminant.E, 0.333333, 0.333333, 0.000001)]
    public void NearBlack(Illuminant illuminant, double x, double y, double luminance)
    {
        var unicolour = Unicolour.FromRgb(ConfigUtils.GetConfigWithStandardRgb(illuminant), 0.00001, 0.00001, 0.00001);
        AssertUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, luminance), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.312727, 0.329023, 0.214041)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496, 0.214041)]
    [TestCase(Illuminant.E, 0.333333, 0.333333, 0.214041)]
    public void Grey(Illuminant illuminant, double x, double y, double luminance)
    {
        var unicolour = Unicolour.FromRgb(ConfigUtils.GetConfigWithStandardRgb(illuminant), 0.5, 0.5, 0.5);
        AssertUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, luminance), Tolerance);
    }

    [TestCase(Illuminant.D65, 0.312727, 0.329023, 1.000000)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496, 1.000000)]
    [TestCase(Illuminant.E, 0.333333, 0.333333, 1.000000)]
    public void White(Illuminant illuminant, double x, double y, double luminance)
    {
        var unicolour = Unicolour.FromRgb(ConfigUtils.GetConfigWithStandardRgb(illuminant), 1, 1, 1);
        AssertUtils.AssertTriplet(unicolour.Xyy.Triplet, new(x, y, luminance), Tolerance);
    }
    
    [TestCase(-0.00000000001)]
    [TestCase(0)]
    [TestCase(0.00000000001)]
    public void ChromaticityY(double chromaticityY)
    {
        var unicolour = Unicolour.FromXyy(0.5, chromaticityY, 1);
        var xyz = unicolour.Xyz;
        
        var useZero = chromaticityY <= 0;
        Assert.That(xyz.X, useZero ? Is.EqualTo(0) : Is.GreaterThan(0));
        Assert.That(xyz.Y, useZero ? Is.EqualTo(0) : Is.GreaterThan(0));
        Assert.That(xyz.Z, useZero ? Is.EqualTo(0) : Is.GreaterThan(0));
    }
}