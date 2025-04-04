﻿using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ConfigureIctcpTests
{
    private static readonly WhitePoint D65WhitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    private static readonly ColourTriplet XyzWhite = new(D65WhitePoint.X / 100.0, D65WhitePoint.Y / 100.0, D65WhitePoint.Z / 100.0);

    // Linear Rec2020 RGB as used in https://github.com/colour-science/colour#31224ictcp-colour-encoding
    private static readonly ColourTriplet TestLinearRgb = new(0.45620519, 0.03081071, 0.04091952);
    
    private static readonly Configuration Config100 = new(RgbConfiguration.Rec2020, XyzConfiguration.D65, dynamicRange: DynamicRange.Standard);
    private static readonly Configuration Config203 = new(RgbConfiguration.Rec2020, XyzConfiguration.D65, dynamicRange: DynamicRange.High);
    private static readonly Configuration Config1 = new(RgbConfiguration.Rec2020, XyzConfiguration.D65, dynamicRange: new(1, 100, 0.1)); // only white luminance matters for Ictcp

    [Test] // matches the behaviour of papers on Hung & Berns dataset (https://professional.dolby.com/siteassets/pdfs/ictcp_dolbywhitepaper_v071.pdf, figure 6)
    public void Rec2020RgbToIctcp100()
    {
        var red = new Unicolour(Config100, ColourSpace.Xyz, HungBerns.RedRef.Xyz.Tuple);
        var blue = new Unicolour(Config100, ColourSpace.Xyz, HungBerns.BlueRef.Xyz.Tuple);
        var white = new Unicolour(Config100, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config100, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Ictcp>(red, new(0.396807697, -0.135943598, 0.234295237), 0.000000001);
        TestUtils.AssertTriplet<Ictcp>(blue, new(0.323484554, 0.235351529, -0.130337248), 0.000000001);
        TestUtils.AssertTriplet<Ictcp>(white, new(0.50808, 0, 0), 0.0005); // InverseEOTF(100) ~= 0.50808
        TestUtils.AssertTriplet<Ictcp>(black, new(0, 0, 0), 0.0005);
    }
    
    [Test] // matches the behaviour of python-based "colour-science/colour" (https://github.com/colour-science/colour#31224ictcp-colour-encoding)  
    public void Rec2020RgbToIctcp1()
    {
        var colour = new Unicolour(Config1, ColourSpace.RgbLinear, TestLinearRgb.Tuple);
        TestUtils.AssertTriplet(colour.Ictcp.Triplet, new(0.07351364, 0.00475253, 0.09351596), 0.00001);
        
        var white = new Unicolour(Config1, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config1, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Ictcp>(white, new(0.14995, 0, 0), 0.0005); // InverseEOTF(1) ~= 0.14995
        TestUtils.AssertTriplet<Ictcp>(black, new(0, 0, 0), 0.0005);
    }

    [Test] // matches the behaviour of javascript-based "color.js" (https://github.com/LeaVerou/color.js / https://colorjs.io/apps/picker)  
    public void Rec2020RgbToIctcp203()
    {
        var colour = new Unicolour(Config203, ColourSpace.RgbLinear, TestLinearRgb.Tuple);
        TestUtils.AssertTriplet(colour.Ictcp.Triplet, new(0.39224991, -0.0001166, 0.28389029), 0.0001);
        
        var white = new Unicolour(Config203, ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(Config203, ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Ictcp>(white, new(0.58069, 0, 0), 0.0005); // InverseEOTF(203) ~= 0.5807
        TestUtils.AssertTriplet<Ictcp>(black, new(0, 0, 0), 0.0005);
    }
    
    [Test] // default config is HDR (203 white luminance), should be same as above
    public void Rec2020RgbToIctcpNoConfig()
    {
        var white = new Unicolour(ColourSpace.Xyz, XyzWhite.Tuple);
        var black = new Unicolour(ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Ictcp>(white, new(0.58069, 0, 0), 0.0005); // InverseEOTF(203) ~= 0.5807
        TestUtils.AssertTriplet<Ictcp>(black, new(0, 0, 0), 0.0005);
    }

    [Test]
    public void ConvertTestColour()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.RgbLinear, TestLinearRgb.Tuple);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertTriplet<Ictcp>(convertedTo1, new(0.07351364, 0.00475253, 0.09351596), 0.00001);
        TestUtils.AssertTriplet<Ictcp>(convertedTo203, new(0.39224991, -0.0001166, 0.28389029), 0.0001);
        TestUtils.AssertTriplet(convertedTo100.Ictcp.Triplet, initial100.Ictcp.Triplet, 0.0005);
    }
    
    [Test]
    public void ConvertWhite()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.Xyz, XyzWhite.Tuple);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertTriplet<Ictcp>(initial100, new(0.50808, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Ictcp>(convertedTo1, new(0.14995, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Ictcp>(convertedTo203, new(0.58069, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Ictcp>(convertedTo100, new(0.50808, 0, 0), 0.0005);
    }

    [Test]
    public void ConvertBlack()
    {
        var initial100 = new Unicolour(Config100, ColourSpace.Xyz, 0, 0, 0);
        var convertedTo1 = initial100.ConvertToConfiguration(Config1);
        var convertedTo203 = convertedTo1.ConvertToConfiguration(Config203);
        var convertedTo100 = convertedTo203.ConvertToConfiguration(Config100);
        
        TestUtils.AssertTriplet<Ictcp>(initial100, new(0, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Ictcp>(convertedTo1, new(0, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Ictcp>(convertedTo203, new(0, 0, 0), 0.0005);
        TestUtils.AssertTriplet<Ictcp>(convertedTo100, new(0, 0, 0), 0.0005);
    }
}