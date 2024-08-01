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
    public void Protanopia(string colourName, double expectedR, double expectedG, double expectedB)
    {
        var unicolour = StandardRgb.Lookup[colourName];
        var simulatedColour = unicolour.SimulateProtanopia();
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
        var unicolour = StandardRgb.Lookup[colourName];
        var simulatedColour = unicolour.SimulateDeuteranopia();
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
        var unicolour = StandardRgb.Lookup[colourName];
        var simulatedColour = unicolour.SimulateTritanopia();
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
        var unicolour = StandardRgb.Lookup[colourName];
        var simulatedColour = unicolour.SimulateAchromatopsia();
        var simulatedRgb = simulatedColour.Rgb.Byte255.ConstrainedTriplet;
        TestUtils.AssertTriplet(simulatedRgb, new(expectedR, expectedG, expectedB), 1);
    }

    [Test]
    public void ProtanopiaNotNumber()
    {
        var unicolour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = unicolour.SimulateProtanopia();
        Assert.That(simulatedColour.Rgb.IsNaN);
    }
    
    [Test]
    public void DeuteranopiaNotNumber()
    {
        var unicolour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = unicolour.SimulateDeuteranopia();
        Assert.That(simulatedColour.Rgb.IsNaN);
    }
    
    [Test]
    public void TritanopiaNotNumber()
    {
        var unicolour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = unicolour.SimulateTritanopia();
        Assert.That(simulatedColour.Rgb.IsNaN);
    }
    
    [Test]
    public void AchromatopsiaNotNumber()
    {
        var unicolour = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var simulatedColour = unicolour.SimulateAchromatopsia();
        Assert.That(simulatedColour.Rgb.IsNaN);
    }
}