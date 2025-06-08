using System;
using System.Linq;
using MathNet.Numerics.Statistics;
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
    private static readonly Configuration D50Config = new(RgbConfiguration.ProPhoto, XyzConfiguration.D50);
    
    [TestCase(nameof(StandardRgb.Red), 109, 95, 0)]
    [TestCase(nameof(StandardRgb.Yellow), 255, 244, 0)]
    [TestCase(nameof(StandardRgb.Green), 255, 229, 0)]
    [TestCase(nameof(StandardRgb.Cyan), 237, 242, 255)]
    [TestCase(nameof(StandardRgb.Blue), 0, 88, 255)]
    [TestCase(nameof(StandardRgb.Magenta), 0, 128, 255)]
    [TestCase(nameof(StandardRgb.Black), 0, 0, 0)]
    [TestCase(nameof(StandardRgb.White), 255, 255, 255)]
    public void Protanopia(string colourName, double expectedR, double expectedG, double expectedB)
    {
        var colour = StandardRgb.Lookup[colourName];
        var simulatedColour = colour.Simulate(Cvd.Protanopia);
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
    public void Deuteranopia(string colourName, double expectedR, double expectedG, double expectedB)
    {
        var colour = StandardRgb.Lookup[colourName];
        var simulatedColour = colour.Simulate(Cvd.Deuteranopia);
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
    public void Tritanopia(string colourName, double expectedR, double expectedG, double expectedB)
    {
        var colour = StandardRgb.Lookup[colourName];
        var simulatedColour = colour.Simulate(Cvd.Tritanopia);
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
    public void NotNumber([Values(Cvd.Protanopia, Cvd.Protanomaly, Cvd.Deuteranopia, Cvd.Deuteranomaly, Cvd.Tritanopia, Cvd.Tritanomaly, Cvd.BlueConeMonochromacy, Cvd.Achromatopsia)] Cvd cvd)
    {
        var colour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = colour.Simulate(cvd);
        Assert.That(simulatedColour.Rgb.IsNaN);
    }
    
    [Test]
    public void DifferentConfig(
        [Values(Cvd.Protanopia, Cvd.Protanomaly, Cvd.Deuteranopia, Cvd.Deuteranomaly, Cvd.Tritanopia, Cvd.Tritanomaly, Cvd.BlueConeMonochromacy, Cvd.Achromatopsia)] Cvd cvd,
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
    public void AnomalyNoSeverity(
        [Values(Cvd.Protanomaly, Cvd.Deuteranomaly, Cvd.Tritanomaly)] Cvd cvd,
        [Values(0, -0.0000000001, double.MinValue, double.NegativeInfinity, double.NaN)] double severity)
    {
        var colour = new Unicolour(Configuration.Default, ColourSpace.Rgb, 1.0, 0.5, 0.25);
        var simulated = colour.Simulate(cvd, severity);
        Assert.That(simulated, Is.EqualTo(colour));
    }
    
    [Test]
    public void AnomalyFullSeverity(
        [Values(Cvd.Protanomaly, Cvd.Deuteranomaly, Cvd.Tritanomaly)] Cvd cvd,
        [Values(1, 1.0000000001, double.MaxValue, double.PositiveInfinity)] double severity)
    {
        var colour = new Unicolour(Configuration.Default, ColourSpace.Rgb, 1.0, 0.5, 0.25);
        var maxSeverity = colour.Simulate(cvd, severity: 1.0);
        var simulated = colour.Simulate(cvd, severity);
        Assert.That(simulated, Is.EqualTo(maxSeverity));
    }
    
    private static readonly TestCaseData[] HalfSeverityData =
    [
        new(Cvd.Protanomaly, new RgbLinear(0.5965189636385253, 0.2770292552467638, 0.04102103854212891)),
        new(Cvd.Deuteranomaly, new RgbLinear(0.6696817431717564, 0.35087728428294257, 0.045446085927284545)),
        new(Cvd.Tritanomaly, new RgbLinear(1.0208082020235654, 0.2014643698722337, 0.09751100343519398))
    ];
    
    [TestCaseSource(nameof(HalfSeverityData))]
    public void AnomalyHalfSeverity(Cvd cvd, RgbLinear expected)
    {
        var colour = new Unicolour(Configuration.Default, ColourSpace.Rgb, 1.0, 0.5, 0.25);
        var halfSeverity = colour.Simulate(cvd, severity: 0.5);
        Assert.That(halfSeverity.RgbLinear, Is.EqualTo(expected));
    }

    /*
     * there is no real data to validate against
     * ----------
     * the `BlueConeMonochromacy` test just confirms that a blue colour generates a more luminous colour than non-blue colours
     * (since the blue cone is the only one detecting light)
     * which is quite unusual, e.g. sRGB yellow light is far more luminous than sRGB blue light
     * ----------
     * the `LmsSimulation` is checking that the methodology used at https://ixora.io/projects/colorblindness/color-blindness-simulation-research/
     * results in values somewhat similar to the existing protanopia, deuteranopia, tritanopia
     * and assumption is that blue cone monochromacy is correctly derived from same methodology, and the output is therefore sensible
     * (some colours produce notable differences but the similarities are clear when viewed across the spectrum)
     */
    
    [TestCase(nameof(StandardRgb.Red))]
    [TestCase(nameof(StandardRgb.Yellow))]
    [TestCase(nameof(StandardRgb.Green))]
    public void BlueConeMonochromacy(string rgbName)
    {
        var blueCvd = StandardRgb.Blue.Simulate(Cvd.BlueConeMonochromacy);
        var otherCvd = StandardRgb.Lookup[rgbName].Simulate(Cvd.BlueConeMonochromacy);
        Assert.That(blueCvd.RelativeLuminance, Is.GreaterThan(otherCvd.RelativeLuminance));
    }
    
    [Test]
    public void LmsSimulation([Values(Cvd.Protanopia, Cvd.Deuteranopia, Cvd.Tritanopia)] Cvd cvd)
    {
        Unicolour[] colours =
        [
            StandardRgb.Red, StandardRgb.Green, StandardRgb.Blue,
            StandardRgb.Cyan, StandardRgb.Magenta, StandardRgb.Yellow,
            StandardRgb.Black, StandardRgb.White, StandardRgb.Grey
        ];
        
        var lmsSimulationMatrix = cvd switch
        {
            Cvd.Protanopia => VisionDeficiency.ProtanopiaLmsSim,
            Cvd.Deuteranopia => VisionDeficiency.DeuteranopiaLmsSim,
            Cvd.Tritanopia => VisionDeficiency.TritanopiaLmsSim,
            _ => throw new ArgumentOutOfRangeException(nameof(cvd), cvd, null)
        };
    
        var lmsSimulations = colours.Select(x => VisionDeficiency.ApplySimulation(x, lmsSimulationMatrix)).ToArray();
        var expected = colours.Select(x => x.Simulate(cvd)).ToArray();
        var differences = lmsSimulations.Zip(expected, (x, y) => x.Difference(y, DeltaE.Ciede2000)).ToArray();
        
        Assert.That(differences.Average(), Is.LessThan(10));
        Assert.That(differences.Median(), Is.LessThan(10));
    }
}