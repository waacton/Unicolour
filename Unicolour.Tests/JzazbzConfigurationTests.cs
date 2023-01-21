namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;

public static class JzazbzConfigurationTests
{
    // TODO: extract this after v2 changes
    private static readonly WhitePoint D65WhitePoint = WhitePoint.From(Illuminant.D65);
    private static readonly ColourTriplet XyzWhite = new(D65WhitePoint.X / 100.0, D65WhitePoint.Y / 100.0, D65WhitePoint.Z / 100.0);

    // XYZ as used in https://github.com/colour-science/colour#31220jzazbz-colourspace
    private static readonly ColourTriplet TestXyz = new(0.20654008, 0.12197225, 0.05136952);

    [Test] // matches the behaviour of papers on Hung & Berns dataset (https://www.researchgate.net/figure/The-Hung-Berns-data-plotted-in-six-different-color-spaces-a-CIELAB-b-CIELUV-c_fig2_317811721)
    public static void XyzD65ToJzazbz100()
    {
        var config = new Configuration(
            Chromaticity.StandardRgbR, Chromaticity.StandardRgbG, Chromaticity.StandardRgbB, 
            Companding.StandardRgb, Companding.InverseStandardRgb,
            D65WhitePoint, 
            D65WhitePoint,
            100, 100);

        var red = Unicolour.FromXyz(config, HungBerns.RedRef.Xyz.Triplet.Tuple);
        var blue = Unicolour.FromXyz(config, HungBerns.BlueRef.Xyz.Triplet.Tuple);
        var white = Unicolour.FromXyz(config, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(config, 0, 0, 0);
        AssertUtils.AssertColourTriplet(red.Jzazbz.Triplet, new(0.113977241, 0.092841591, 0.102192454), 0.000000001);
        AssertUtils.AssertColourTriplet(blue.Jzazbz.Triplet, new(0.077776685, -0.028264073, -0.149837552), 0.000000001);
        AssertUtils.AssertColourTriplet(white.Jzazbz.Triplet, new(0.16717, 0, 0), 0.0005);
        AssertUtils.AssertColourTriplet(black.Jzazbz.Triplet, new(0, 0, 0), 0.0005);
        
        var redNoConfig = Unicolour.FromXyz(HungBerns.RedRef.Xyz.Triplet.Tuple);
        var blueNoConfig = Unicolour.FromXyz(HungBerns.BlueRef.Xyz.Triplet.Tuple);
        var whiteNoConfig = Unicolour.FromXyz(XyzWhite.Tuple);
        var blackNoConfig = Unicolour.FromXyz(0, 0, 0);
        AssertUtils.AssertColourTriplet(redNoConfig.Jzazbz.Triplet, new(0.113977241, 0.092841591, 0.102192454), 0.000000001);
        AssertUtils.AssertColourTriplet(blueNoConfig.Jzazbz.Triplet, new(0.077776685, -0.028264073, -0.149837552), 0.000000001);
        AssertUtils.AssertColourTriplet(whiteNoConfig.Jzazbz.Triplet, new(0.16717, 0, 0), 0.0005);
        AssertUtils.AssertColourTriplet(blackNoConfig.Jzazbz.Triplet, new(0, 0, 0), 0.0005);
    }
    
    [Test] // matches the behaviour of python-based "colour-science/colour" (https://github.com/colour-science/colour#31220jzazbz-colourspace)  
    public static void XyzD65ToJzazbz1()
    {
        var config = new Configuration(
            Chromaticity.StandardRgbR, Chromaticity.StandardRgbG, Chromaticity.StandardRgbB, 
            Companding.StandardRgb, Companding.InverseStandardRgb,
            D65WhitePoint, 
            D65WhitePoint,
            100, 1);
        
        var unicolour = Unicolour.FromXyz(config, TestXyz.Tuple);
        AssertUtils.AssertColourTriplet(unicolour.Jzazbz.Triplet, new(0.00535048, 0.00924302, 0.00526007), 0.00001);

        var white = Unicolour.FromXyz(config, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(config, 0, 0, 0);
        AssertUtils.AssertColourTriplet(white.Jzazbz.Triplet, new(0.01758, 0, 0), 0.0005);
        AssertUtils.AssertColourTriplet(black.Jzazbz.Triplet, new(0, 0, 0), 0.0005);
    }

    [Test] // matches the behaviour of javascript-based "color.js" (https://github.com/LeaVerou/color.js / https://colorjs.io/apps/picker)  
    public static void XyzD65ToJzazbz203()
    {
        var config = new Configuration(
            Chromaticity.StandardRgbR, Chromaticity.StandardRgbG, Chromaticity.StandardRgbB, 
            Companding.StandardRgb, Companding.InverseStandardRgb,
            D65WhitePoint, 
            D65WhitePoint,
            100, 203);
        
        var unicolour = Unicolour.FromXyz(config, TestXyz.Tuple);
        AssertUtils.AssertColourTriplet(unicolour.Jzazbz.Triplet, new(0.10287841, 0.08613415, 0.05873694), 0.0001);
        
        var white = Unicolour.FromXyz(config, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(config, 0, 0, 0);
        AssertUtils.AssertColourTriplet(white.Jzazbz.Triplet, new(0.22207, 0, 0), 0.0005);
        AssertUtils.AssertColourTriplet(black.Jzazbz.Triplet, new(0, 0, 0), 0.0005);
    }
}