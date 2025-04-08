using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ContrastTests
{
    [TestCase(nameof(StandardRgb.Black), nameof(StandardRgb.White), 21)]
    [TestCase(nameof(StandardRgb.Red), nameof(StandardRgb.Green), 2.91)]
    [TestCase(nameof(StandardRgb.Green), nameof(StandardRgb.Blue), 6.26)]
    [TestCase(nameof(StandardRgb.Blue), nameof(StandardRgb.Red), 2.15)]
    public void KnownContrast(string colour1Name, string colour2Name, double expected)
    {
        var colour1 = StandardRgb.Lookup[colour1Name];
        var colour2 = StandardRgb.Lookup[colour2Name];
        AssertKnownContrast(colour1, colour2, expected);
    }

    [Test]
    public void RandomColourContrast()
    {
        var random = RandomColours.UnicolourFrom(ColourSpace.Rgb);
        AssertKnownContrast(random, random, 1);
    }
    
    [Test]
    public void DifferentConfigs()
    {
        var redD65 = new Unicolour(TestUtils.D65Config, ColourSpace.Rgb, 1, 0, 0);
        var redD50 = new Unicolour(TestUtils.D50Config, ColourSpace.Rgb, 1, 0, 0);
        AssertKnownContrast(redD65, redD50, 1);
    }
    
    [Test]
    public void NaNContrast()
    {
        var notNumber = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var grey = new Unicolour(ColourSpace.Rgb, 0.5, 0.5, 0.5);
        
        AssertKnownContrast(notNumber, grey, double.NaN);
        AssertKnownContrast(grey, notNumber, double.NaN);
        AssertKnownContrast(notNumber, notNumber, double.NaN);
    }

    [Test]
    public void BeyondMinRgbLuminance()
    {
        var black = StandardRgb.Black;
        var beyondMinRgb = new Unicolour(ColourSpace.Rgb, -0.25, -0.5, -0.75);
        Assert.That(beyondMinRgb.RelativeLuminance, Is.LessThan(black.RelativeLuminance));
        AssertRelativeLuminance(beyondMinRgb);
    }
    
    [Test]
    public void BeyondMaxRgbLuminance()
    {
        var white = StandardRgb.White;
        var beyondMaxRgb = new Unicolour(ColourSpace.Rgb, 1.25, 1.5, 1.75);
        Assert.That(beyondMaxRgb.RelativeLuminance, Is.GreaterThan(white.RelativeLuminance));
        AssertRelativeLuminance(beyondMaxRgb);
    }
    
    [Test]
    public void InGamutLuminance()
    {
        var standardRgb = new Unicolour(new Configuration(RgbConfiguration.StandardRgb), ColourSpace.Rgb, 1, 1, 0);
        var displayP3 = standardRgb.ConvertToConfiguration(new Configuration(RgbConfiguration.DisplayP3));
        Assert.That(displayP3.RelativeLuminance, Is.EqualTo(standardRgb.RelativeLuminance));
    }
    
    [Test]
    public void OutOfGamutLuminance()
    {
        var displayP3 = new Unicolour(new Configuration(RgbConfiguration.DisplayP3), ColourSpace.Rgb, 1, 1, 0);
        var standardRgb = displayP3.ConvertToConfiguration(new Configuration(RgbConfiguration.StandardRgb));
        Assert.That(standardRgb.RelativeLuminance, Is.EqualTo(displayP3.RelativeLuminance));
    }
    
    private static void AssertKnownContrast(Unicolour colour1, Unicolour colour2, double expectedContrast)
    {
        AssertRelativeLuminance(colour1);
        AssertRelativeLuminance(colour2);
        
        var delta1 = colour1.Contrast(colour2);
        var delta2 = colour2.Contrast(colour1);
        Assert.That(delta1, Is.EqualTo(expectedContrast).Within(0.005));
        Assert.That(delta1, Is.EqualTo(delta2));
    }

    private static void AssertRelativeLuminance(Unicolour colour)
    {
        // WCAG relative luminance is defined according to sRGB https://www.w3.org/TR/WCAG21/#dfn-relative-luminance
        // which should match XYZ.Y under D65
        // so need to ensure colour is using the correct configuration for the formula to work 
        if (colour.Configuration != Configuration.Default)
        {
            colour = colour.ConvertToConfiguration(Configuration.Default);
        }

        var (r, g, b) = colour.RgbLinear;
        var expected = 0.2126 * r + 0.7152 * g + 0.0722 * b;
        Assert.That(colour.RelativeLuminance, Is.EqualTo(expected).Within(0.0005));
    }
}