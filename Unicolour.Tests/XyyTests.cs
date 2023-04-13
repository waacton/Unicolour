namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// XYY has special handling for black, since otherwise it would result in a divide-by-zero
// some implementations would set all values to zero
// but I think it's more intuitive to set chromaticity to the same as white, and set only luminance to 0
public static class XyyTests
{
    private const double Tolerance = 0.000001;

    [TestCase(Illuminant.D65, 0.312727, 0.329023, 1.000000)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496, 1.000000)]
    [TestCase(Illuminant.E, 0.333333, 0.333333, 1.000000)]
    public static void White(Illuminant illuminant, double expectedX, double expectedY, double expectedLuminance)
    {
        var unicolour = Unicolour.FromRgb(GetConfig(illuminant), 1, 1, 1);
        AssertUtils.AssertTriplet(unicolour.Xyy.Triplet, new(expectedX, expectedY, expectedLuminance), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.312727, 0.329023, 0.214041)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496, 0.214041)]
    [TestCase(Illuminant.E, 0.333333, 0.333333, 0.214041)]
    public static void Grey(Illuminant illuminant, double expectedX, double expectedY, double expectedLuminance)
    {
        var unicolour = Unicolour.FromRgb(GetConfig(illuminant), 0.5, 0.5, 0.5);
        AssertUtils.AssertTriplet(unicolour.Xyy.Triplet, new(expectedX, expectedY, expectedLuminance), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.312727, 0.329023, 0.000001)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496, 0.000001)]
    [TestCase(Illuminant.E, 0.333333, 0.333333, 0.000001)]
    public static void NearBlack(Illuminant illuminant, double expectedX, double expectedY, double expectedLuminance)
    {
        var unicolour = Unicolour.FromRgb(GetConfig(illuminant), 0.00001, 0.00001, 0.00001);
        AssertUtils.AssertTriplet(unicolour.Xyy.Triplet, new(expectedX, expectedY, expectedLuminance), Tolerance);
    }
    
    [TestCase(Illuminant.D65, 0.312727, 0.329023, 0.000000)]
    [TestCase(Illuminant.D50, 0.345669, 0.358496, 0.000000)]
    [TestCase(Illuminant.E, 0.333333, 0.333333, 0.000000)]
    public static void Black(Illuminant illuminant, double expectedX, double expectedY, double expectedLuminance)
    {
        var unicolour = Unicolour.FromRgb(GetConfig(illuminant), 0, 0, 0);
        AssertUtils.AssertTriplet(unicolour.Xyy.Triplet, new(expectedX, expectedY, expectedLuminance), Tolerance);
    }

    [TestCase(-0.00000000001)]
    [TestCase(0)]
    [TestCase(0.00000000001)]
    public static void ChromaticityY(double chromaticityY)
    {
        var unicolour = Unicolour.FromXyy(0.5, chromaticityY, 1);
        var xyz = unicolour.Xyz;
        
        var useZero = chromaticityY <= 0;
        Assert.That(xyz.X, useZero ? Is.EqualTo(0) : Is.GreaterThan(0));
        Assert.That(xyz.Y, useZero ? Is.EqualTo(0) : Is.GreaterThan(0));
        Assert.That(xyz.Z, useZero ? Is.EqualTo(0) : Is.GreaterThan(0));
    }

    private static Configuration GetConfig(Illuminant illuminant)
    {
        var xyzConfig = new XyzConfiguration(WhitePoint.From(illuminant));
        return new Configuration(RgbConfiguration.StandardRgb, xyzConfig);
    }
}