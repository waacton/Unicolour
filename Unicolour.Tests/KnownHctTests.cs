using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownHctTests
{
    private static readonly ColourTriplet HctWhiteTriplet = new(209.492, 2.869, 100.000);
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Hct>(red, new(27.408, 113.358, 53.241), 0.005);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Hct>(green, new(142.139, 108.410, 87.737), 0.005);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Hct>(blue, new(282.788, 87.230, 32.302), 0.005);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Hct>(black, new(0.000, 0.000, 0.000), 0.005);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Hct>(white, HctWhiteTriplet, 0.005);
    }
    
    [Test]
    public void WhiteD65ToHct()
    {
        var whiteD65 = new Unicolour(new Configuration(xyzConfiguration: XyzConfiguration.D65), ColourSpace.Rgb, 1.0, 1.0, 1.0);
        TestUtils.AssertTriplet<Xyz>(whiteD65, new(0.9505, 1.0000, 1.0888), 0.0001); // XYZ values for D65 white
        TestUtils.AssertTriplet<Hct>(whiteD65, HctWhiteTriplet, 0.005); // HCT values for D65 white (no adaptation needed)
    }
    
    [Test]
    public void WhiteD65FromHct()
    {
        var whiteD65 = new Unicolour(new Configuration(xyzConfiguration: XyzConfiguration.D65), ColourSpace.Hct, 209.492, 2.869, 100.000);
        TestUtils.AssertTriplet<Xyz>(whiteD65, new(0.9505, 1.0000, 1.0888), 0.0001); // XYZ values for D65 white (no adaptation needed)
    }
    
    [Test]
    public void WhiteD50ToHct()
    {
        var whiteD50 = new Unicolour(new Configuration(xyzConfiguration: XyzConfiguration.D50), ColourSpace.Rgb, 1.0, 1.0, 1.0);
        TestUtils.AssertTriplet<Xyz>(whiteD50, new(0.9642, 1.0000, 0.8252), 0.0001); // XYZ values for D50 white
        TestUtils.AssertTriplet<Hct>(whiteD50, HctWhiteTriplet, 0.005); // HCT values same as D65 white due to adaptation
    }
    
    [Test]
    public void WhiteD50FromHct()
    {
        var whiteD50 = new Unicolour(new Configuration(xyzConfiguration: XyzConfiguration.D50), ColourSpace.Hct, 209.492, 2.869, 100.000);
        TestUtils.AssertTriplet<Xyz>(whiteD50, new(0.9642, 1.0000, 0.8252), 0.0001); // XYZ values for D50 white due to adaptation
    }
    
    [Test]
    public void WhiteEqualEnergyToHct()
    {
        var equalEnergyConfig = new XyzConfiguration(Illuminant.E, Observer.Degree2);
        var whiteE = new Unicolour(new Configuration(xyzConfiguration: equalEnergyConfig), ColourSpace.Rgb, 1.0, 1.0, 1.0);
        TestUtils.AssertTriplet<Xyz>(whiteE, new(1.0000, 1.0000, 1.0000), 0.0001); // XYZ values for E white
        TestUtils.AssertTriplet<Hct>(whiteE, HctWhiteTriplet, 0.005); // HCT values same as D65 white due to adaptation
    }
    
    [Test]
    public void WhiteEqualEnergyFromHct()
    {
        var equalEnergyConfig = new XyzConfiguration(Illuminant.E, Observer.Degree2);
        var whiteE = new Unicolour(new Configuration(xyzConfiguration: equalEnergyConfig), ColourSpace.Hct, 209.492, 2.869, 100.000);
        TestUtils.AssertTriplet<Xyz>(whiteE, new(1.0000, 1.0000, 1.0000), 0.0001); // XYZ values for E white due to adaptation
    }
}