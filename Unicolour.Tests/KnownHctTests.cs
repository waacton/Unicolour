namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownHctTests
{
    private static readonly ColourTriplet HctWhiteTriplet = new(209.492, 2.869, 100.000);
    
    [Test]
    public void Red()
    {
        var red = ColourLimits.Rgb[ColourLimit.Red];
        AssertUtils.AssertTriplet<Hct>(red, new(27.408, 113.358, 53.241), 0.005);
    }
    
    [Test]
    public void Green()
    {
        var green = ColourLimits.Rgb[ColourLimit.Green];
        AssertUtils.AssertTriplet<Hct>(green, new(142.139, 108.410, 87.737), 0.005);
    }
    
    [Test]
    public void Blue()
    {
        var blue = ColourLimits.Rgb[ColourLimit.Blue];
        AssertUtils.AssertTriplet<Hct>(blue, new(282.788, 87.230, 32.302), 0.005);
    }
    
    [Test]
    public void Black()
    {
        var black = ColourLimits.Rgb[ColourLimit.Black];
        AssertUtils.AssertTriplet<Hct>(black, new(0.000, 0.000, 0.000), 0.005);
    }
    
    [Test]
    public void White()
    {
        var white = ColourLimits.Rgb[ColourLimit.White];
        AssertUtils.AssertTriplet<Hct>(white, HctWhiteTriplet, 0.005);
    }
    
    [Test]
    public void WhiteD65ToHct()
    {
        var whiteD65 = Unicolour.FromRgb(new Configuration(xyzConfiguration: XyzConfiguration.D65), 1.0, 1.0, 1.0);
        AssertUtils.AssertTriplet<Xyz>(whiteD65, new(0.9505, 1.0000, 1.0888), 0.0001); // XYZ values for D65 white
        AssertUtils.AssertTriplet<Hct>(whiteD65, HctWhiteTriplet, 0.005); // HCT values for D65 white (no adaptation needed)
    }
    
    [Test]
    public void WhiteD65FromHct()
    {
        var whiteD65 = Unicolour.FromHct(new Configuration(xyzConfiguration: XyzConfiguration.D65), 209.492, 2.869, 100.000);
        AssertUtils.AssertTriplet<Xyz>(whiteD65, new(0.9505, 1.0000, 1.0888), 0.0001); // XYZ values for D65 white (no adaptation needed)
    }
    
    [Test]
    public void WhiteD50ToHct()
    {
        var whiteD50 = Unicolour.FromRgb(new Configuration(xyzConfiguration: XyzConfiguration.D50), 1.0, 1.0, 1.0);
        AssertUtils.AssertTriplet<Xyz>(whiteD50, new(0.9642, 1.0000, 0.8252), 0.0001); // XYZ values for D50 white
        AssertUtils.AssertTriplet<Hct>(whiteD50, HctWhiteTriplet, 0.005); // HCT values same as D65 white due to adaptation
    }
    
    [Test]
    public void WhiteD50FromHct()
    {
        var whiteD50 = Unicolour.FromHct(new Configuration(xyzConfiguration: XyzConfiguration.D50), 209.492, 2.869, 100.000);
        AssertUtils.AssertTriplet<Xyz>(whiteD50, new(0.9642, 1.0000, 0.8252), 0.0001); // XYZ values for D50 white due to adaptation
    }
    
    [Test]
    public void WhiteEqualEnergyToHct()
    {
        var equalEnergyConfig = new XyzConfiguration(WhitePoint.From(Illuminant.E));
        var whiteE = Unicolour.FromRgb(new Configuration(xyzConfiguration: equalEnergyConfig), 1.0, 1.0, 1.0);
        AssertUtils.AssertTriplet<Xyz>(whiteE, new(1.0000, 1.0000, 1.0000), 0.0001); // XYZ values for E white
        AssertUtils.AssertTriplet<Hct>(whiteE, HctWhiteTriplet, 0.005); // HCT values same as D65 white due to adaptation
    }
    
    [Test]
    public void WhiteEqualEnergyFromHct()
    {
        var equalEnergyConfig = new XyzConfiguration(WhitePoint.From(Illuminant.E));
        var whiteE = Unicolour.FromHct(new Configuration(xyzConfiguration: equalEnergyConfig), 209.492, 2.869, 100.000);
        AssertUtils.AssertTriplet<Xyz>(whiteE, new(1.0000, 1.0000, 1.0000), 0.0001); // XYZ values for E white due to adaptation
    }
}