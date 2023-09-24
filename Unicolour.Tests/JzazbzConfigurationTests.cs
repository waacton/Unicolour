namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;

public static class JzazbzConfigurationTests
{
    private static readonly WhitePoint D65WhitePoint = WhitePoint.From(Illuminant.D65);
    private static readonly ColourTriplet XyzWhite = new(D65WhitePoint.X / 100.0, D65WhitePoint.Y / 100.0, D65WhitePoint.Z / 100.0);

    // XYZ as used in https://github.com/colour-science/colour#31212jzazbz-colourspace
    private static readonly ColourTriplet TestXyz = new(0.20654008, 0.12197225, 0.05136952);
    
    private static readonly Configuration Config100 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, jzazbzScalar: 100);
    private static readonly Configuration Config1 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, jzazbzScalar: 1);
    private static readonly Configuration Config203 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65, jzazbzScalar: 203);

    [Test] // matches the behaviour of papers on Hung & Berns dataset (https://www.researchgate.net/figure/The-Hung-Berns-data-plotted-in-six-different-color-spaces-a-CIELAB-b-CIELUV-c_fig2_317811721)
    public static void XyzD65ToJzazbz100()
    {
        var red = Unicolour.FromXyz(Config100, HungBerns.RedRef.Xyz.Triplet.Tuple);
        var blue = Unicolour.FromXyz(Config100, HungBerns.BlueRef.Xyz.Triplet.Tuple);
        var white = Unicolour.FromXyz(Config100, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(Config100, 0, 0, 0);
        AssertUtils.AssertTriplet<Jzazbz>(red, new(0.113977241, 0.092841591, 0.102192454), 0.000000001);
        AssertUtils.AssertTriplet<Jzazbz>(blue, new(0.077776685, -0.028264073, -0.149837552), 0.000000001);
        AssertUtils.AssertTriplet<Jzazbz>(white, new(0.16717, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(black, new(0, 0, 0), 0.0005);
        
        var redNoConfig = Unicolour.FromXyz(HungBerns.RedRef.Xyz.Triplet.Tuple);
        var blueNoConfig = Unicolour.FromXyz(HungBerns.BlueRef.Xyz.Triplet.Tuple);
        var whiteNoConfig = Unicolour.FromXyz(XyzWhite.Tuple);
        var blackNoConfig = Unicolour.FromXyz(0, 0, 0);
        AssertUtils.AssertTriplet<Jzazbz>(redNoConfig, new(0.113977241, 0.092841591, 0.102192454), 0.000000001);
        AssertUtils.AssertTriplet<Jzazbz>(blueNoConfig, new(0.077776685, -0.028264073, -0.149837552), 0.000000001);
        AssertUtils.AssertTriplet<Jzazbz>(whiteNoConfig, new(0.16717, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(blackNoConfig, new(0, 0, 0), 0.0005);
    }
    
    [Test] // matches the behaviour of python-based "colour-science/colour" (https://github.com/colour-science/colour#31212jzazbz-colourspace)  
    public static void XyzD65ToJzazbz1()
    {
        var unicolour = Unicolour.FromXyz(Config1, TestXyz.Tuple);
        AssertUtils.AssertTriplet<Jzazbz>(unicolour, new(0.00535048, 0.00924302, 0.00526007), 0.00001);

        var white = Unicolour.FromXyz(Config1, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(Config1, 0, 0, 0);
        AssertUtils.AssertTriplet<Jzazbz>(white, new(0.01758, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(black, new(0, 0, 0), 0.0005);
    }

    [Test] // matches the behaviour of javascript-based "color.js" (https://github.com/LeaVerou/color.js / https://colorjs.io/apps/picker)  
    public static void XyzD65ToJzazbz203()
    {
        var unicolour = Unicolour.FromXyz(Config203, TestXyz.Tuple);
        AssertUtils.AssertTriplet<Jzazbz>(unicolour, new(0.10287841, 0.08613415, 0.05873694), 0.0001);
        
        var white = Unicolour.FromXyz(Config203, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(Config203, 0, 0, 0);
        AssertUtils.AssertTriplet<Jzazbz>(white, new(0.22207, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(black, new(0, 0, 0), 0.0005);
    }
    
    [Test]
    public static void ConvertTestColour()
    {
        var initial100 = Unicolour.FromXyz(Config100, TestXyz.Tuple);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        AssertUtils.AssertTriplet<Jzazbz>(convertedTo1, new(0.00535048, 0.00924302, 0.00526007), 0.00001);
        AssertUtils.AssertTriplet<Jzazbz>(convertedTo203, new(0.10287841, 0.08613415, 0.05873694), 0.0001);
        AssertUtils.AssertTriplet(convertedTo100.Jzazbz.Triplet, initial100.Jzazbz.Triplet, 0.0005);
    }
    
    [Test]
    public static void ConvertWhite()
    {
        var initial100 = Unicolour.FromXyz(Config100, XyzWhite.Tuple);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        AssertUtils.AssertTriplet<Jzazbz>(initial100, new(0.16717, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(convertedTo1, new(0.01758, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(convertedTo203, new(0.22207, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(convertedTo100, new(0.16717, 0, 0), 0.0005);
    }

    [Test]
    public static void ConvertBlack()
    {
        var initial100 = Unicolour.FromXyz(Config100, 0, 0, 0);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        AssertUtils.AssertTriplet<Jzazbz>(initial100, new(0, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(convertedTo1, new(0, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(convertedTo203, new(0, 0, 0), 0.0005);
        AssertUtils.AssertTriplet<Jzazbz>(convertedTo100, new(0, 0, 0), 0.0005);
    }
}