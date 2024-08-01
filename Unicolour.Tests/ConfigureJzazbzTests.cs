using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ConfigureJzazbzTests
{
    private static readonly WhitePoint D65WhitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    private static readonly ColourTriplet XyzWhite = new(D65WhitePoint.X / 100.0, D65WhitePoint.Y / 100.0, D65WhitePoint.Z / 100.0);

    // XYZ as used in https://github.com/colour-science/colour#31212jzazbz-colourspace
    private static readonly ColourTriplet TestXyz = new(0.20654008, 0.12197225, 0.05136952);
    
    private static readonly Configuration Config100 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, jzazbzScalar: 100);
    private static readonly Configuration Config1 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, jzazbzScalar: 1);
    private static readonly Configuration Config203 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, jzazbzScalar: 203);

    [Test] // matches the behaviour of papers on Hung & Berns dataset (https://www.researchgate.net/figure/The-Hung-Berns-data-plotted-in-six-different-color-spaces-a-CIELAB-b-CIELUV-c_fig2_317811721)
    public void XyzD65ToJzazbz100()
    {
        var red = new Unicolour(Config100, ColourSpace.Xyz, HungBerns.RedRef.Xyz.Triplet.Tuple);
        var blue = new Unicolour(Config100, ColourSpace.Xyz, HungBerns.BlueRef.Xyz.Triplet.Tuple);
        var white = new Unicolour(Config100, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config100, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Jzazbz>(red, new(0.113977241, 0.092841591, 0.102192454), 0.000000001);
        TestUtils.AssertTriplet<Jzazbz>(blue, new(0.077776685, -0.028264073, -0.149837552), 0.000000001);
        TestUtils.AssertTriplet<Jzazbz>(white, new(0.16717, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(black, new(0, 0, 0), 0.0005);
        
        var redNoConfig = new Unicolour(ColourSpace.Xyz, HungBerns.RedRef.Xyz.Triplet.Tuple);
        var blueNoConfig = new Unicolour(ColourSpace.Xyz, HungBerns.BlueRef.Xyz.Triplet.Tuple);
        var whiteNoConfig = new Unicolour(ColourSpace.Xyz, XyzWhite.Tuple);
        var blackNoConfig = new Unicolour(ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Jzazbz>(redNoConfig, new(0.113977241, 0.092841591, 0.102192454), 0.000000001);
        TestUtils.AssertTriplet<Jzazbz>(blueNoConfig, new(0.077776685, -0.028264073, -0.149837552), 0.000000001);
        TestUtils.AssertTriplet<Jzazbz>(whiteNoConfig, new(0.16717, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(blackNoConfig, new(0, 0, 0), 0.0005);
    }
    
    [Test] // matches the behaviour of python-based "colour-science/colour" (https://github.com/colour-science/colour#31212jzazbz-colourspace)  
    public void XyzD65ToJzazbz1()
    {
        var unicolour = new Unicolour(Config1, ColourSpace.Xyz, TestXyz.Tuple);
        TestUtils.AssertTriplet<Jzazbz>(unicolour, new(0.00535048, 0.00924302, 0.00526007), 0.00001);

        var white = new Unicolour(Config1, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config1, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Jzazbz>(white, new(0.01758, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(black, new(0, 0, 0), 0.0005);
    }

    [Test] // matches the behaviour of javascript-based "color.js" (https://github.com/LeaVerou/color.js / https://colorjs.io/apps/picker)  
    public void XyzD65ToJzazbz203()
    {
        var unicolour = new Unicolour(Config203, ColourSpace.Xyz, TestXyz.Tuple);
        TestUtils.AssertTriplet<Jzazbz>(unicolour, new(0.10287841, 0.08613415, 0.05873694), 0.0001);
        
        var white = new Unicolour(Config203, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config203, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Jzazbz>(white, new(0.22207, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(black, new(0, 0, 0), 0.0005);
    }
    
    [Test]
    public void ConvertTestColour()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.Xyz, TestXyz.Tuple);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertTriplet<Jzazbz>(convertedTo1, new(0.00535048, 0.00924302, 0.00526007), 0.00001);
        TestUtils.AssertTriplet<Jzazbz>(convertedTo203, new(0.10287841, 0.08613415, 0.05873694), 0.0001);
        TestUtils.AssertTriplet(convertedTo100.Jzazbz.Triplet, initial100.Jzazbz.Triplet, 0.0005);
    }
    
    [Test]
    public void ConvertWhite()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.Xyz, XyzWhite.Tuple);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertTriplet<Jzazbz>(initial100, new(0.16717, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(convertedTo1, new(0.01758, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(convertedTo203, new(0.22207, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(convertedTo100, new(0.16717, 0, 0), 0.0005);
    }

    [Test]
    public void ConvertBlack()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.Xyz, 0, 0, 0);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertTriplet<Jzazbz>(initial100, new(0, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(convertedTo1, new(0, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(convertedTo203, new(0, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Jzazbz>(convertedTo100, new(0, 0, 0), 0.0005);
    }
}