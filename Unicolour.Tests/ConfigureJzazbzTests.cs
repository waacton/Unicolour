using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ConfigureJzazbzTests
{
    private static readonly WhitePoint D65WhitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    private static readonly ColourTriplet XyzWhite = new(D65WhitePoint.X, D65WhitePoint.Y, D65WhitePoint.Z);

    // XYZ as used in https://github.com/colour-science/colour#31212jzazbz-colourspace
    private static readonly ColourTriplet TestXyz = new(0.20654008, 0.12197225, 0.05136952);
    
    private static readonly Configuration Config100 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, dynamicRange: DynamicRange.Standard);
    private static readonly Configuration Config203 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, dynamicRange: DynamicRange.High);
    private static readonly Configuration Config1 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, dynamicRange: new(1, 100, 0.1)); // only white luminance matters for Jzazbz

    [Test] // matches the behaviour of papers on Hung & Berns dataset (https://www.researchgate.net/figure/The-Hung-Berns-data-plotted-in-six-different-color-spaces-a-CIELAB-b-CIELUV-c_fig2_317811721)
    public void XyzD65ToJzazbz100()
    {
        var red = new Unicolour(Config100, ColourSpace.Xyz, HungBerns.RedRef.Xyz.Tuple);
        var blue = new Unicolour(Config100, ColourSpace.Xyz, HungBerns.BlueRef.Xyz.Tuple);
        var white = new Unicolour(Config100, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config100, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertColour(red, new Jzazbz(0.113977241, 0.092841591, 0.102192454), 0.000000001);
        TestUtils.AssertColour(blue, new Jzazbz(0.077776685, -0.028264073, -0.149837552), 0.000000001);
        TestUtils.AssertColour(white, new Jzazbz(0.16717, 0, 0), 0.0005);
        TestUtils.AssertColour(black, new Jzazbz(0, 0, 0), 0.0005);
    }
    
    [Test] // matches the behaviour of python-based "colour-science/colour" (https://github.com/colour-science/colour#31212jzazbz-colourspace)  
    public void XyzD65ToJzazbz1()
    {
        var colour = new Unicolour(Config1, ColourSpace.Xyz, TestXyz.Tuple);
        TestUtils.AssertColour(colour, new Jzazbz(0.00535048, 0.00924302, 0.00526007), 0.00001);

        var white = new Unicolour(Config1, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config1, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertColour(white, new Jzazbz(0.01758, 0, 0), 0.0005);
        TestUtils.AssertColour(black, new Jzazbz(0, 0, 0), 0.0005);
    }

    [Test] // matches the behaviour of javascript-based "color.js" (https://github.com/LeaVerou/color.js / https://colorjs.io/apps/picker)  
    public void XyzD65ToJzazbz203()
    {
        var colour = new Unicolour(Config203, ColourSpace.Xyz, TestXyz.Tuple);
        TestUtils.AssertColour(colour, new Jzazbz(0.10287841, 0.08613415, 0.05873694), 0.0001);
        
        var white = new Unicolour(Config203, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config203, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertColour(white, new Jzazbz(0.22207, 0, 0), 0.0005);
        TestUtils.AssertColour(black, new Jzazbz(0, 0, 0), 0.0005);
    }
    
    [Test] // default config is HDR (203 white luminance), should be same as above
    public void XyzD65ToJzazbzNoConfig()
    {
        var colour = new Unicolour(ColourSpace.Xyz, TestXyz.Tuple);
        TestUtils.AssertColour(colour, new Jzazbz(0.10287841, 0.08613415, 0.05873694), 0.0001);
        
        var white = new Unicolour(ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertColour(white, new Jzazbz(0.22207, 0, 0), 0.0005);
        TestUtils.AssertColour(black, new Jzazbz(0, 0, 0), 0.0005);
    }
    
    [Test]
    public void ConvertTestColour()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.Xyz, TestXyz.Tuple);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertColour(convertedTo1, new Jzazbz(0.00535048, 0.00924302, 0.00526007), 0.00001);
        TestUtils.AssertColour(convertedTo203, new Jzazbz(0.10287841, 0.08613415, 0.05873694), 0.0001);
        TestUtils.AssertTriplet(convertedTo100.Jzazbz.Triplet, initial100.Jzazbz.Triplet, 0.0005);
    }
    
    [Test]
    public void ConvertWhite()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.Xyz, XyzWhite.Tuple);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertColour(initial100, new Jzazbz(0.16717, 0, 0), 0.0005);
        TestUtils.AssertColour(convertedTo1, new Jzazbz(0.01758, 0, 0), 0.0005);
        TestUtils.AssertColour(convertedTo203, new Jzazbz(0.22207, 0, 0), 0.0005);
        TestUtils.AssertColour(convertedTo100, new Jzazbz(0.16717, 0, 0), 0.0005);
    }

    [Test]
    public void ConvertBlack()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.Xyz, 0, 0, 0);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertColour(initial100, new Jzazbz(0, 0, 0), 0.0005);
        TestUtils.AssertColour(convertedTo1, new Jzazbz(0, 0, 0), 0.0005);
        TestUtils.AssertColour(convertedTo203, new Jzazbz(0, 0, 0), 0.0005);
        TestUtils.AssertColour(convertedTo100, new Jzazbz(0, 0, 0), 0.0005);
    }
}