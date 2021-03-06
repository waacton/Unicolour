namespace Wacton.Unicolour.Tests.Factories;

using System;
using System.Collections.Generic;
using Colourful;
using Wacton.Unicolour.Tests.Utils;
using ColourfulRgb = Colourful.RGBColor;
using ColourfulRgbLinear = Colourful.LinearRGBColor;
using ColourfulXyz = Colourful.XYZColor;
using ColourfulLab = Colourful.LabColor;
using ColourfulLuv = Colourful.LuvColor;
using ColourfulLchab = Colourful.LChabColor;
using ColourfulLchuv = Colourful.LChuvColor;
using ColourfulIlluminants = Colourful.Illuminants;

/*
 * Colourful doesn't support HSB / HSL
 * Colourful appears to behave oddly converting from LUV / LCHuv
 */
internal class ColourfulFactory : ITestColourFactory
{
    private static readonly Tolerances Tolerances = new()
        {Rgb = 0.00000000001, RgbLinear = 0.00000000001, Xyz = 0.00000000001, Lab = 0.0000005, Lchab = 0.0000005, Luv = 0.000005, Lchuv = 0.000005};
    
    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var rgbLinearConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToXYZ(ColourfulIlluminants.D65).Build();
        var labConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLab(ColourfulIlluminants.D65).Build();
        var lchabConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLChab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLChuv(ColourfulIlluminants.D65).Build();

        var rgb = new ColourfulRgb(r, g, b);
        var rgbLinear = rgbLinearConverter.Convert(rgb);
        var xyz = xyzConverter.Convert(rgb);
        var lab = labConverter.Convert(rgb);
        var lchab = lchabConverter.Convert(rgb);
        var luv = luvConverter.Convert(rgb);
        var lchuv = lchuvConverter.Convert(rgb);
        return Create(name, rgb, rgbLinear, xyz, lab, lchab, luv, lchuv, Tolerances);
    }

    // Colourful does not support HSB or HSL
    public TestColour FromHsb(double h, double s, double b, string name) => throw new NotImplementedException();
    public TestColour FromHsl(double h, double s, double l, string name) => throw new NotImplementedException();

    public TestColour FromXyz(double x, double y, double z, string name)
    {
        var rgbConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var labConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLab(ColourfulIlluminants.D65).Build();
        var lchabConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLChab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLChuv(ColourfulIlluminants.D65).Build();

        var xyz = new ColourfulXyz(x, y, z);
        var rgb = rgbConverter.Convert(xyz);
        var rgbLinear = rgbLinearConverter.Convert(xyz);
        var lab = labConverter.Convert(xyz);
        var lchab = lchabConverter.Convert(xyz);
        var luv = luvConverter.Convert(xyz);
        var lchuv = lchuvConverter.Convert(xyz);
        return Create(name, rgb, rgbLinear, xyz, lab, lchab, luv, lchuv, Tolerances);
    }

    public TestColour FromLab(double l, double a, double b, string name)
    {
        var rgbConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToXYZ(ColourfulIlluminants.D65).Build();
        var lchabConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLChab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLChuv(ColourfulIlluminants.D65).Build();

        var lab = new ColourfulLab(l, a, b);
        var rgb = rgbConverter.Convert(lab);
        var rgbLinear = rgbLinearConverter.Convert(lab);
        var xyz = xyzConverter.Convert(lab);
        var lchab = lchabConverter.Convert(lab);
        var luv = luvConverter.Convert(lab);
        var lchuv = lchuvConverter.Convert(lab);
        return Create(name, rgb, rgbLinear, xyz, lab, lchab, luv, lchuv, Tolerances with {Luv = 0.005, Lchuv = 0.005});
    }
    
    public TestColour FromLchab(double l, double c, double h, string name)
    {
        var rgbConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToXYZ(ColourfulIlluminants.D65).Build();
        var labConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToLab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToLChuv(ColourfulIlluminants.D65).Build();

        var lchab = new ColourfulLchab(l, c, h);
        var rgb = rgbConverter.Convert(lchab);
        var rgbLinear = rgbLinearConverter.Convert(lchab);
        var xyz = xyzConverter.Convert(lchab);
        var lab = labConverter.Convert(lchab);
        var luv = luvConverter.Convert(lchab);
        var lchuv = lchuvConverter.Convert(lchab);
        return Create(name, rgb, rgbLinear, xyz, lab, lchab, luv, lchuv, Tolerances with {Luv = 0.005, Lchuv = 0.005});
    }

    // Colourful LUV / LCHuv appears to not convert correctly
    // potentially due to clamping negative XYZ values? (which doesn't happen with LAB / LCHab) 
    public TestColour FromLuv(double l, double u, double v, string name) => throw new NotImplementedException();
    public TestColour FromLchuv(double l, double c, double h, string name) => throw new NotImplementedException();

    private static TestColour Create(string name, 
        ColourfulRgb rgb, ColourfulRgbLinear rgbLinear, 
        ColourfulXyz xyz, ColourfulLab lab, ColourfulLchab lchab, ColourfulLuv luv, ColourfulLchuv lchuv,
        Tolerances tolerances)
    {
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.R, rgb.G, rgb.B),
            RgbLinear = new(rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Xyz = new(xyz.X, xyz.Y, xyz.Z),
            Lab = new(lab.L, lab.a, lab.b),
            Lchab = new(lchab.L, lchab.C, lchab.h),
            Luv = new(luv.L, luv.u, luv.v),
            Lchuv = new(lchuv.L, lchuv.C, lchuv.h),
            Tolerances = tolerances,
            IsRgbConstrained = false,
            IsRgbLinearConstrained = false,
            ExcludeFromLchTestReasons = LchExclusions(rgb)
        };
    }

    private static List<string> LchExclusions(ColourfulRgb rgb)
    {
        var exclusions = new List<string>();
        if (IsMonochrome(rgb)) exclusions.Add("Colourful calculates hue differently value when RGB is monochrome");
        return exclusions;
    }

    private static bool IsMonochrome(ColourfulRgb rgb) => rgb.R == rgb.G && rgb.G == rgb.B;
}