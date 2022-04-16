namespace Wacton.Unicolour.Tests.Factories;

using System;
using Colourful;
using Wacton.Unicolour.Tests.Utils;
using ColourfulRgb = Colourful.RGBColor;
using ColourfulRgbLinear = Colourful.LinearRGBColor;
using ColourfulXyz = Colourful.XYZColor;
using ColourfulLab = Colourful.LabColor;
using ColourfulLuv = Colourful.LuvColor;
using ColourfulIlluminants = Colourful.Illuminants;

/*
 * Colourful doesn't support HSB / HSL
 */
internal class ColourfulFactory : ITestColourFactory
{
    private static readonly Tolerances Tolerances = new() { Rgb = 0.00000000001, RgbLinear = 0.00000000001, Xyz = 0.00000000001, Lab = 0.0000005, Luv = 0.0000005 };
    
    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var rgbLinearConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToXYZ(ColourfulIlluminants.D65).Build();
        var labConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLuv(ColourfulIlluminants.D65).Build();

        var rgb = new ColourfulRgb(r, g, b);
        var rgbLinear = rgbLinearConverter.Convert(rgb);
        var xyz = xyzConverter.Convert(rgb);
        var lab = labConverter.Convert(rgb);
        var luv = luvConverter.Convert(rgb);
        return Create(name, rgb, rgbLinear, xyz, lab, luv);
    }

    // Colourful does not support HSB or HSL
    public TestColour FromHsb(double h, double s, double b, string name) => throw new NotImplementedException();
    public TestColour FromHsl(double h, double s, double l, string name) => throw new NotImplementedException();

    public TestColour FromXyz(double x, double y, double z, string name)
    {
        var rgbConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var labConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();

        var xyz = new ColourfulXyz(x, y, z);
        var rgb = rgbConverter.Convert(xyz);
        var rgbLinear = rgbLinearConverter.Convert(xyz);
        var lab = labConverter.Convert(xyz);
        var luv = luvConverter.Convert(xyz);
        return Create(name, rgb, rgbLinear, xyz, lab, luv);
    }

    public TestColour FromLab(double l, double a, double b, string name)
    {
        var rgbConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToXYZ(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();

        var lab = new ColourfulLab(l, a, b);
        var rgb = rgbConverter.Convert(lab);
        var rgbLinear = rgbLinearConverter.Convert(lab);
        var xyz = xyzConverter.Convert(lab);
        var luv = luvConverter.Convert(lab);
        return Create(name, rgb, rgbLinear, xyz, lab, luv);
    }

    // Colourful LUV appears to not convert correctly
    public TestColour FromLuv(double l, double u, double v, string name) => throw new NotImplementedException();

    private static TestColour Create(string name, 
        ColourfulRgb rgb, ColourfulRgbLinear rgbLinear, 
        ColourfulXyz xyz, ColourfulLab lab, ColourfulLuv luv)
    {
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.R, rgb.G, rgb.B),
            RgbLinear = new(rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Xyz = new(xyz.X, xyz.Y, xyz.Z),
            Lab = new(lab.L, lab.a, lab.b),
            Luv = new(luv.L, luv.u, luv.v),
            Tolerances = Tolerances,
            IsRgbConstrained = false,
            IsRgbLinearConstrained = false
        };
    }
}