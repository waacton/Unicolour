namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

/*
 * should closely match Chrome emulation
 * considered matching Firefox except:
 * - it uses sRGB space for Protanopia/Deuteranopia/Tritanopia
 * - I don't know how it arrives at Achromatopsia values
 */
public class VisionDeficiencyTests
{
    [TestCase(ColourLimit.Red, 109, 95, 0)]
    [TestCase(ColourLimit.Yellow, 255, 244, 0)]
    [TestCase(ColourLimit.Green, 255, 229, 0)]
    [TestCase(ColourLimit.Cyan, 237, 242, 255)]
    [TestCase(ColourLimit.Blue, 0, 88, 255)]
    [TestCase(ColourLimit.Magenta, 0, 128, 255)]
    [TestCase(ColourLimit.Black, 0, 0, 0)]
    [TestCase(ColourLimit.White, 255, 255, 255)]
    public void Protanopia(ColourLimit colourLimit, double expectedR, double expectedG, double expectedB)
    {
        var unicolour = ColourLimits.Rgb[colourLimit];
        var simulatedColour = unicolour.SimulateProtanopia();
        var simulatedRgb = simulatedColour.Rgb.Byte255.ConstrainedTriplet;
        TestUtils.AssertTriplet(simulatedRgb, new(expectedR, expectedG, expectedB), 1);
    }
    
    [TestCase(ColourLimit.Red, 163, 144, 0)]
    [TestCase(ColourLimit.Yellow, 255, 250, 50)]
    [TestCase(ColourLimit.Green, 239, 214, 59)]
    [TestCase(ColourLimit.Cyan, 208, 221, 255)]
    [TestCase(ColourLimit.Blue, 0, 61, 251)]
    [TestCase(ColourLimit.Magenta, 104, 155, 250)]
    [TestCase(ColourLimit.Black, 0, 0, 0)]
    [TestCase(ColourLimit.White, 255, 255, 255)]
    public void Deuteranopia(ColourLimit colourLimit, double expectedR, double expectedG, double expectedB)
    {
        var unicolour = ColourLimits.Rgb[colourLimit];
        var simulatedColour = unicolour.SimulateDeuteranopia();
        var simulatedRgb = simulatedColour.Rgb.Byte255.ConstrainedTriplet;
        TestUtils.AssertTriplet(simulatedRgb, new(expectedR, expectedG, expectedB), 1);
    }
    
    [TestCase(ColourLimit.Red, 255, 0, 13)]
    [TestCase(ColourLimit.Yellow, 255, 238, 217)]
    [TestCase(ColourLimit.Green, 0, 247, 216)]
    [TestCase(ColourLimit.Cyan, 0, 255, 255)]
    [TestCase(ColourLimit.Blue, 0, 108, 150)]
    [TestCase(ColourLimit.Magenta, 255, 75, 151)]
    [TestCase(ColourLimit.Black, 0, 0, 0)]
    [TestCase(ColourLimit.White, 255, 255, 255)]
    public void Tritanopia(ColourLimit colourLimit, double expectedR, double expectedG, double expectedB)
    {
        var unicolour = ColourLimits.Rgb[colourLimit];
        var simulatedColour = unicolour.SimulateTritanopia();
        var simulatedRgb = simulatedColour.Rgb.Byte255.ConstrainedTriplet;
        TestUtils.AssertTriplet(simulatedRgb, new(expectedR, expectedG, expectedB), 2);
    }
    
    [TestCase(ColourLimit.Red, 127, 127, 127)]
    [TestCase(ColourLimit.Yellow, 247, 247, 247)]
    [TestCase(ColourLimit.Green, 220, 220, 220)]
    [TestCase(ColourLimit.Cyan, 230, 230, 230)]
    [TestCase(ColourLimit.Blue, 75, 75, 75)]
    [TestCase(ColourLimit.Magenta, 146, 146, 146)]
    [TestCase(ColourLimit.Black, 0, 0, 0)]
    [TestCase(ColourLimit.White, 255, 255, 255)]
    public void Achromatopsia(ColourLimit colourLimit, double expectedR, double expectedG, double expectedB)
    {
        var unicolour = ColourLimits.Rgb[colourLimit];
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