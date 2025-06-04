using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * should closely match Chrome emulation
 * considered matching Firefox except:
 * - it uses sRGB space for Protanopia/Deuteranopia/Tritanopia
 * - I don't know how it arrives at Achromatopsia values
 */
public class VisionDeficiencyTests
{
    [TestCase(nameof(StandardRgb.Red), 109, 95, 0)]
    [TestCase(nameof(StandardRgb.Yellow), 255, 244, 0)]
    [TestCase(nameof(StandardRgb.Green), 255, 229, 0)]
    [TestCase(nameof(StandardRgb.Cyan), 237, 242, 255)]
    [TestCase(nameof(StandardRgb.Blue), 0, 88, 255)]
    [TestCase(nameof(StandardRgb.Magenta), 0, 128, 255)]
    [TestCase(nameof(StandardRgb.Black), 0, 0, 0)]
    [TestCase(nameof(StandardRgb.White), 255, 255, 255)]
    public void Protan(string colourName, double expectedR, double expectedG, double expectedB)
    {
        var colour = StandardRgb.Lookup[colourName];
        var simulatedColour = colour.Simulate(Cvd.Protan);
        var simulatedRgb = simulatedColour.Rgb.Byte255.ConstrainedTriplet;
        TestUtils.AssertTriplet(simulatedRgb, new(expectedR, expectedG, expectedB), 1);
    }
    
    [TestCase(nameof(StandardRgb.Red), 163, 144, 0)]
    [TestCase(nameof(StandardRgb.Yellow), 255, 250, 50)]
    [TestCase(nameof(StandardRgb.Green), 239, 214, 59)]
    [TestCase(nameof(StandardRgb.Cyan), 208, 221, 255)]
    [TestCase(nameof(StandardRgb.Blue), 0, 61, 251)]
    [TestCase(nameof(StandardRgb.Magenta), 104, 155, 250)]
    [TestCase(nameof(StandardRgb.Black), 0, 0, 0)]
    [TestCase(nameof(StandardRgb.White), 255, 255, 255)]
    public void Deutan(string colourName, double expectedR, double expectedG, double expectedB)
    {
        var colour = StandardRgb.Lookup[colourName];
        var simulatedColour = colour.Simulate(Cvd.Deutan);
        var simulatedRgb = simulatedColour.Rgb.Byte255.ConstrainedTriplet;
        TestUtils.AssertTriplet(simulatedRgb, new(expectedR, expectedG, expectedB), 1);
    }
    
    [TestCase(nameof(StandardRgb.Red), 255, 0, 13)]
    [TestCase(nameof(StandardRgb.Yellow), 255, 238, 217)]
    [TestCase(nameof(StandardRgb.Green), 0, 247, 216)]
    [TestCase(nameof(StandardRgb.Cyan), 0, 255, 255)]
    [TestCase(nameof(StandardRgb.Blue), 0, 108, 150)]
    [TestCase(nameof(StandardRgb.Magenta), 255, 75, 151)]
    [TestCase(nameof(StandardRgb.Black), 0, 0, 0)]
    [TestCase(nameof(StandardRgb.White), 255, 255, 255)]
    public void Tritan(string colourName, double expectedR, double expectedG, double expectedB)
    {
        var colour = StandardRgb.Lookup[colourName];
        var simulatedColour = colour.Simulate(Cvd.Tritan);
        var simulatedRgb = simulatedColour.Rgb.Byte255.ConstrainedTriplet;
        TestUtils.AssertTriplet(simulatedRgb, new(expectedR, expectedG, expectedB), 2);
    }
    
    [TestCase(nameof(StandardRgb.Red), 127, 127, 127)]
    [TestCase(nameof(StandardRgb.Yellow), 247, 247, 247)]
    [TestCase(nameof(StandardRgb.Green), 220, 220, 220)]
    [TestCase(nameof(StandardRgb.Cyan), 230, 230, 230)]
    [TestCase(nameof(StandardRgb.Blue), 75, 75, 75)]
    [TestCase(nameof(StandardRgb.Magenta), 146, 146, 146)]
    [TestCase(nameof(StandardRgb.Black), 0, 0, 0)]
    [TestCase(nameof(StandardRgb.White), 255, 255, 255)]
    public void Achromatopsia(string colourName, double expectedR, double expectedG, double expectedB)
    {
        var colour = StandardRgb.Lookup[colourName];
        var simulatedColour = colour.Simulate(Cvd.Achromatopsia);
        var simulatedRgb = simulatedColour.Rgb.Byte255.ConstrainedTriplet;
        TestUtils.AssertTriplet(simulatedRgb, new(expectedR, expectedG, expectedB), 1);
    }

    [Test]
    public void ProtanNotNumber()
    {
        var colour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = colour.Simulate(Cvd.Protan);
        Assert.That(simulatedColour.Rgb.IsNaN);
    }
    
    [Test]
    public void DeutanNotNumber()
    {
        var colour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = colour.Simulate(Cvd.Deutan);
        Assert.That(simulatedColour.Rgb.IsNaN);
    }
    
    [Test]
    public void TritanNotNumber()
    {
        var colour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = colour.Simulate(Cvd.Tritan);
        Assert.That(simulatedColour.Rgb.IsNaN);
    }
    
    [Test]
    public void AchromatopsiaNotNumber()
    {
        var colour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = colour.Simulate(Cvd.Achromatopsia);
        Assert.That(simulatedColour.Rgb.IsNaN);
    }

    private static readonly Configuration D50Config = new(RgbConfiguration.ProPhoto, XyzConfiguration.D50);
    
    [Test]
    public void DifferentConfig(
        [Values(Cvd.Protan, Cvd.Deutan, Cvd.Tritan, Cvd.Achromatopsia, Cvd.BlueConeMonochromacy)] Cvd cvd,
        [Values(0, 0.25, 0.5, 0.75, 1)] double severity)
    {
        var rgbD65 = new Unicolour(Configuration.Default, ColourSpace.Rgb, 1.0, 0.5, 0.25);
        var cvdD65 = rgbD65.Simulate(cvd, severity);
        var cvdD50FromD65 = cvdD65.ConvertToConfiguration(D50Config);

        var rgbD50 = rgbD65.ConvertToConfiguration(D50Config);
        var cvdD50 = rgbD50.Simulate(cvd, severity);
        var cvdD65FromD50 = cvdD50.ConvertToConfiguration(Configuration.Default);

        const double tolerance = 5e-15;
        TestUtils.AssertTriplet(cvdD65.RgbLinear.Triplet, cvdD65FromD50.RgbLinear.Triplet, tolerance);
        TestUtils.AssertTriplet(cvdD50.RgbLinear.Triplet, cvdD50FromD65.RgbLinear.Triplet, tolerance);
    }

    [Test]
    public void NoSeverity(
        [Values(Cvd.Protan, Cvd.Deutan, Cvd.Tritan)] Cvd cvd,
        [Values(0, -0.0000000001, double.MinValue, double.NegativeInfinity, double.NaN)] double severity)
    {
        var colour = new Unicolour(Configuration.Default, ColourSpace.Rgb, 1.0, 0.5, 0.25);
        var simulated = colour.Simulate(cvd, severity);
        Assert.That(simulated, Is.EqualTo(colour));
    }
    
    [Test]
    public void MaxSeverity(
        [Values(Cvd.Protan, Cvd.Deutan, Cvd.Tritan)] Cvd cvd,
        [Values(1, 1.0000000001, double.MaxValue, double.PositiveInfinity)] double severity)
    {
        var colour = new Unicolour(Configuration.Default, ColourSpace.Rgb, 1.0, 0.5, 0.25);
        var maxSeverity = colour.Simulate(cvd, severity: 1.0);
        var simulated = colour.Simulate(cvd, severity);
        Assert.That(simulated, Is.EqualTo(maxSeverity));
    }
}