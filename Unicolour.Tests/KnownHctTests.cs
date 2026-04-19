using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownHctTests
{
    private static readonly Hct HctWhiteTriplet = new(209.492, 2.869, 100.000);
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Hct(27.408, 113.358, 53.241), 0.005);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Hct(142.139, 108.410, 87.737), 0.005);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Hct(282.788, 87.230, 32.302), 0.005);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Hct(0.000, 0.000, 0.000), 0.005);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertColour(white, HctWhiteTriplet, 0.005);
    }
    
    [Test]
    public void Achromatic()
    {
        var grey = StandardRgb.Grey;
        Assert.That(grey.Hct.ToString().Contains(NoHue));
    }
    
    [Test]
    public void WhiteD65ToHct()
    {
        var whiteD65 = new Unicolour(TestUtils.D65Config, ColourSpace.Rgb, 1.0, 1.0, 1.0);
        TestUtils.AssertColour(whiteD65, new Xyz(0.9505, 1.0000, 1.0888, whiteD65.Configuration.Xyz.WhitePoint), 0.0001); // XYZ values for D65 white
        TestUtils.AssertColour(whiteD65, HctWhiteTriplet, 0.005); // HCT values for D65 white (no adaptation needed)
    }
    
    [Test]
    public void WhiteD65FromHct()
    {
        var whiteD65 = new Unicolour(TestUtils.D65Config, ColourSpace.Hct, 209.492, 2.869, 100.000);
        TestUtils.AssertColour(whiteD65, new Xyz(0.9505, 1.0000, 1.0888, whiteD65.Configuration.Xyz.WhitePoint), 0.0001); // XYZ values for D65 white (no adaptation needed)
    }
    
    [Test]
    public void WhiteD50ToHct()
    {
        var whiteD50 = new Unicolour(TestUtils.D50Config, ColourSpace.Rgb, 1.0, 1.0, 1.0);
        TestUtils.AssertColour(whiteD50, new Xyz(0.9642, 1.0000, 0.8252, whiteD50.Configuration.Xyz.WhitePoint), 0.0001); // XYZ values for D50 white
        TestUtils.AssertColour(whiteD50, HctWhiteTriplet, 0.005); // HCT values same as D65 white due to adaptation
    }
    
    [Test]
    public void WhiteD50FromHct()
    {
        var whiteD50 = new Unicolour(TestUtils.D50Config, ColourSpace.Hct, 209.492, 2.869, 100.000);
        TestUtils.AssertColour(whiteD50, new Xyz(0.9642, 1.0000, 0.8252, whiteD50.Configuration.Xyz.WhitePoint), 0.0001); // XYZ values for D50 white due to adaptation
    }
    
    [Test]
    public void WhiteEqualEnergyToHct()
    {
        var whiteE = new Unicolour(TestUtils.EqualEnergyConfig, ColourSpace.Rgb, 1.0, 1.0, 1.0);
        TestUtils.AssertColour(whiteE, new Xyz(1.0000, 1.0000, 1.0000, whiteE.Configuration.Xyz.WhitePoint), 0.0001); // XYZ values for E white
        TestUtils.AssertColour(whiteE, HctWhiteTriplet, 0.005); // HCT values same as D65 white due to adaptation
    }
    
    [Test]
    public void WhiteEqualEnergyFromHct()
    {
        var whiteE = new Unicolour(TestUtils.EqualEnergyConfig, ColourSpace.Hct, 209.492, 2.869, 100.000);
        TestUtils.AssertColour(whiteE, new Xyz(1.0000, 1.0000, 1.0000, whiteE.Configuration.Xyz.WhitePoint), 0.0001); // XYZ values for E white due to adaptation
    }
}