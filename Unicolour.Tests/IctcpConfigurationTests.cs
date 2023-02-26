namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;

public static class IctcpConfigurationTests
{
    private static readonly WhitePoint D65WhitePoint = WhitePoint.From(Illuminant.D65);
    private static readonly ColourTriplet XyzWhite = new(D65WhitePoint.X / 100.0, D65WhitePoint.Y / 100.0, D65WhitePoint.Z / 100.0);

    // Linear Rec2020 RGB as used in https://github.com/colour-science/colour#31223ictcp-colour-encoding
    private static double Gamma(double e) => Companding.Rec2020.FromLinear(e);
    private static readonly ColourTriplet TestLinearRgb = new(0.45620519, 0.03081071, 0.04091952);
    private static readonly ColourTriplet TestRgb = new(Gamma(TestLinearRgb.First), Gamma(TestLinearRgb.Second), Gamma(TestLinearRgb.Third));

    [Test] // matches the behaviour of papers on Hung & Berns dataset (https://professional.dolby.com/siteassets/pdfs/ictcp_dolbywhitepaper_v071.pdf, figure 6)
    public static void Rec2020RgbToIctcp100()
    {
        var config = new Configuration(RgbConfiguration.Rec2020, XyzConfiguration.D65, ictcpScalar: 100, jzazbzScalar: 100);

        var red = Unicolour.FromXyz(config, HungBerns.RedRef.Xyz.Triplet.Tuple);
        var blue = Unicolour.FromXyz(config, HungBerns.BlueRef.Xyz.Triplet.Tuple);
        var white = Unicolour.FromXyz(config, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(config, 0, 0, 0);
        AssertUtils.AssertColourTriplet(red.Ictcp.Triplet, new(0.396807697, -0.135943598, 0.234295237), 0.000000001);
        AssertUtils.AssertColourTriplet(blue.Ictcp.Triplet, new(0.323484554, 0.235351529, -0.130337248), 0.000000001);
        AssertUtils.AssertColourTriplet(white.Ictcp.Triplet, new(0.50808, 0, 0), 0.0005); // InverseEOTF(100) ~= 0.50808
        AssertUtils.AssertColourTriplet(black.Ictcp.Triplet, new(0, 0, 0), 0.0005);
        
        var redNoConfig = Unicolour.FromXyz(HungBerns.RedRef.Xyz.Triplet.Tuple);
        var blueNoConfig = Unicolour.FromXyz(HungBerns.BlueRef.Xyz.Triplet.Tuple);
        var whiteNoConfig = Unicolour.FromXyz(XyzWhite.Tuple);
        var blackNoConfig = Unicolour.FromXyz(0, 0, 0);
        AssertUtils.AssertColourTriplet(redNoConfig.Ictcp.Triplet, new(0.396807697, -0.135943598, 0.234295237), 0.000000001);
        AssertUtils.AssertColourTriplet(blueNoConfig.Ictcp.Triplet, new(0.323484554, 0.235351529, -0.130337248), 0.000000001);
        AssertUtils.AssertColourTriplet(whiteNoConfig.Ictcp.Triplet, new(0.50808, 0, 0), 0.0005); // InverseEOTF(100) ~= 0.50808
        AssertUtils.AssertColourTriplet(blackNoConfig.Ictcp.Triplet, new(0, 0, 0), 0.0005);
    }
    
    [Test] // matches the behaviour of python-based "colour-science/colour" (https://github.com/colour-science/colour#31223ictcp-colour-encoding)  
    public static void Rec2020RgbToIctcp1()
    {
        var config = new Configuration(RgbConfiguration.Rec2020, XyzConfiguration.D65, ictcpScalar: 1, jzazbzScalar: 100);

        var unicolour = Unicolour.FromRgb(config, TestRgb.Tuple);
        AssertUtils.AssertColourTriplet(unicolour.Rgb.Linear.Triplet, TestLinearRgb, 0.00000000001);
        AssertUtils.AssertColourTriplet(unicolour.Ictcp.Triplet, new(0.07351364, 0.00475253, 0.09351596), 0.00001);
        
        var white = Unicolour.FromXyz(config, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(config, 0, 0, 0);
        AssertUtils.AssertColourTriplet(white.Ictcp.Triplet, new(0.14995, 0, 0), 0.0005); // InverseEOTF(1) ~= 0.14995
        AssertUtils.AssertColourTriplet(black.Ictcp.Triplet, new(0, 0, 0), 0.0005);
    }

    [Test] // matches the behaviour of javascript-based "color.js" (https://github.com/LeaVerou/color.js / https://colorjs.io/apps/picker)  
    public static void Rec2020RgbToIctcp203()
    {
        var config = new Configuration(RgbConfiguration.Rec2020, XyzConfiguration.D65, ictcpScalar: 203, jzazbzScalar: 100);

        var unicolour = Unicolour.FromRgb(config, TestRgb.Tuple);
        AssertUtils.AssertColourTriplet(unicolour.Rgb.Linear.Triplet, TestLinearRgb, 0.00000000001);
        AssertUtils.AssertColourTriplet(unicolour.Ictcp.Triplet, new(0.39224991, -0.0001166, 0.28389029), 0.0001);
        
        var white = Unicolour.FromXyz(config, XyzWhite.Tuple);
        var black = Unicolour.FromXyz(config, 0, 0, 0);
        AssertUtils.AssertColourTriplet(white.Ictcp.Triplet, new(0.58069, 0, 0), 0.0005); // InverseEOTF(203) ~= 0.5807
        AssertUtils.AssertColourTriplet(black.Ictcp.Triplet, new(0, 0, 0), 0.0005);
    }
}