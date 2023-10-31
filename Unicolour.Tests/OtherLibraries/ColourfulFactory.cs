namespace Wacton.Unicolour.Tests.OtherLibraries;

using System;
using System.Collections.Generic;
using Colourful;
using Wacton.Unicolour.Tests.Utils;
using ColourfulRgb = Colourful.RGBColor;
using ColourfulRgbLinear = Colourful.LinearRGBColor;
using ColourfulXyz = Colourful.XYZColor;
using ColourfulXyy = Colourful.xyYColor;
using ColourfulLab = Colourful.LabColor;
using ColourfulLuv = Colourful.LuvColor;
using ColourfulLchab = Colourful.LChabColor;
using ColourfulLchuv = Colourful.LChuvColor;
using ColourfulIlluminants = Colourful.Illuminants;

/*
 * Colourful does not support HSB / HSL / HWB / HSLuv / HPLuv / ICtCp / Oklab / Oklch CAM02 / CAM16 / HCT
 * Colourful appears to behave oddly converting from LUV / LCHuv
 * Colourful handles XYZ -> JzAzBz / JzCzHz differently to Unicolour (related to https://github.com/nschloe/colorio/issues/41 and inconsistent ranges in the plots from the paper)
 * --- which results in wildly different Jz* values and makes comparing them pointless
 */
internal class ColourfulFactory : ITestColourFactory
{
    private static readonly Tolerances Tolerances = new()
    {
        Rgb = 0.0000000001, RgbLinear = 0.0000000001, Xyz = 0.0000000001, Xyy = 0.000000005, 
        Lab = 0.0000005, Lchab = 0.0000005, Luv = 0.000005, Lchuv = 0.000005
    };
    
    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var rgbLinearConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToXYZ(ColourfulIlluminants.D65).Build();
        var xyyConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToxyY(ColourfulIlluminants.D65).Build();
        var labConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLab(ColourfulIlluminants.D65).Build();
        var lchabConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLChab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLChuv(ColourfulIlluminants.D65).Build();

        var rgb = new ColourfulRgb(r, g, b);
        var rgbLinear = rgbLinearConverter.Convert(rgb);
        var xyz = xyzConverter.Convert(rgb);
        var xyy = xyyConverter.Convert(rgb);
        var lab = labConverter.Convert(rgb);
        var lchab = lchabConverter.Convert(rgb);
        var luv = luvConverter.Convert(rgb);
        var lchuv = lchuvConverter.Convert(rgb);
        return Create(name, rgb, rgbLinear, xyz, xyy, lab, lchab, luv, lchuv, Tolerances);
    }

    // Colourful does not support HSB or HSL
    public TestColour FromHsb(double h, double s, double b, string name) => throw new NotImplementedException();
    public TestColour FromHsl(double h, double s, double l, string name) => throw new NotImplementedException();

    public TestColour FromXyz(double x, double y, double z, string name)
    {
        var rgbConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyyConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToxyY(ColourfulIlluminants.D65).Build();
        var labConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLab(ColourfulIlluminants.D65).Build();
        var lchabConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLChab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromXYZ(ColourfulIlluminants.D65).ToLChuv(ColourfulIlluminants.D65).Build();

        var xyz = new ColourfulXyz(x, y, z);
        var rgb = rgbConverter.Convert(xyz);
        var rgbLinear = rgbLinearConverter.Convert(xyz);
        var xyy = xyyConverter.Convert(xyz);
        var lab = labConverter.Convert(xyz);
        var lchab = lchabConverter.Convert(xyz);
        var luv = luvConverter.Convert(xyz);
        var lchuv = lchuvConverter.Convert(xyz);
        return Create(name, rgb, rgbLinear, xyz, xyy, lab, lchab, luv, lchuv, Tolerances);
    }
    
    public TestColour FromXyy(double x, double y, double upperY, string name)
    {
        var rgbConverter = new ConverterBuilder().FromxyY(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromxyY(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromxyY(ColourfulIlluminants.D65).ToXYZ(ColourfulIlluminants.D65).Build();
        var labConverter = new ConverterBuilder().FromxyY(ColourfulIlluminants.D65).ToLab(ColourfulIlluminants.D65).Build();
        var lchabConverter = new ConverterBuilder().FromxyY(ColourfulIlluminants.D65).ToLChab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromxyY(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromxyY(ColourfulIlluminants.D65).ToLChuv(ColourfulIlluminants.D65).Build();

        var xyy = new ColourfulXyy(x, y, upperY);
        var rgb = rgbConverter.Convert(xyy);
        var rgbLinear = rgbLinearConverter.Convert(xyy);
        var xyz = xyzConverter.Convert(xyy);
        var lab = labConverter.Convert(xyy);
        var lchab = lchabConverter.Convert(xyy);
        var luv = luvConverter.Convert(xyy);
        var lchuv = lchuvConverter.Convert(xyy);
        return Create(name, rgb, rgbLinear, xyz, xyy, lab, lchab, luv, lchuv, Tolerances);
    }

    public TestColour FromLab(double l, double a, double b, string name)
    {
        var rgbConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToXYZ(ColourfulIlluminants.D65).Build();
        var xyyConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToxyY(ColourfulIlluminants.D65).Build();
        var lchabConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLChab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromLab(ColourfulIlluminants.D65).ToLChuv(ColourfulIlluminants.D65).Build();

        var lab = new ColourfulLab(l, a, b);
        var rgb = rgbConverter.Convert(lab);
        var rgbLinear = rgbLinearConverter.Convert(lab);
        var xyz = xyzConverter.Convert(lab);
        var xyy = xyyConverter.Convert(lab);
        var lchab = lchabConverter.Convert(lab);
        var luv = luvConverter.Convert(lab);
        var lchuv = lchuvConverter.Convert(lab);
        return Create(name, rgb, rgbLinear, xyz, xyy, lab, lchab, luv, lchuv, Tolerances with {Luv = 0.005, Lchuv = 0.005});
    }
    
    public TestColour FromLchab(double l, double c, double h, string name)
    {
        var rgbConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToRGB(RGBWorkingSpaces.sRGB).Build();
        var rgbLinearConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToLinearRGB(RGBWorkingSpaces.sRGB).Build();
        var xyzConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToXYZ(ColourfulIlluminants.D65).Build();
        var xyyConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToxyY(ColourfulIlluminants.D65).Build();
        var labConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToLab(ColourfulIlluminants.D65).Build();
        var luvConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToLuv(ColourfulIlluminants.D65).Build();
        var lchuvConverter = new ConverterBuilder().FromLChab(ColourfulIlluminants.D65).ToLChuv(ColourfulIlluminants.D65).Build();

        var lchab = new ColourfulLchab(l, c, h);
        var rgb = rgbConverter.Convert(lchab);
        var rgbLinear = rgbLinearConverter.Convert(lchab);
        var xyz = xyzConverter.Convert(lchab);
        var xyy = xyyConverter.Convert(lchab);
        var lab = labConverter.Convert(lchab);
        var luv = luvConverter.Convert(lchab);
        var lchuv = lchuvConverter.Convert(lchab);
        return Create(name, rgb, rgbLinear, xyz, xyy, lab, lchab, luv, lchuv, Tolerances with {Luv = 0.005, Lchuv = 0.005});
    }

    // Colourful LUV / LCHuv appears to not convert correctly
    // potentially due to clamping negative XYZ values? (which doesn't happen with LAB / LCHab) 
    public TestColour FromLuv(double l, double u, double v, string name) => throw new NotImplementedException();
    public TestColour FromLchuv(double l, double c, double h, string name) => throw new NotImplementedException();

    private static TestColour Create(string name, 
        ColourfulRgb rgb, ColourfulRgbLinear rgbLinear, 
        ColourfulXyz xyz, ColourfulXyy xyy, ColourfulLab lab, ColourfulLchab lchab, ColourfulLuv luv, ColourfulLchuv lchuv,
        Tolerances tolerances)
    {
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.R, rgb.G, rgb.B),
            RgbLinear = new(rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Xyz = new(xyz.X, xyz.Y, xyz.Z),
            Xyy = new(xyy.x, xyy.y, xyy.Luminance),
            Lab = new(lab.L, lab.a, lab.b),
            Lchab = new(lchab.L, lchab.C, lchab.h),
            Luv = new(luv.L, luv.u, luv.v),
            Lchuv = new(lchuv.L, lchuv.C, lchuv.h),
            Tolerances = tolerances,
            IsRgbConstrained = false,
            IsRgbLinearConstrained = false,
            ExcludeFromLchTestReasons = LchExclusions(rgb),
            ExcludeFromXyyTestReasons = XyyExclusions(xyy),
            ExcludeFromAllTestReasons = AllExclusions(rgbLinear)
        };
    }

    private static List<string> LchExclusions(ColourfulRgb rgb)
    {
        var exclusions = new List<string>();
        if (IsGreyscale(rgb)) exclusions.Add("Colourful calculates hue differently value when RGB is greyscale");
        return exclusions;
    }
    
    private static List<string> XyyExclusions(ColourfulXyy xyy)
    {
        var exclusions = new List<string>();
        if (HasNoChromaticityY(xyy)) exclusions.Add("Colourful sets all values to 0 when xyY has no y-chromaticity");
        return exclusions;
    }
    
    private static List<string> AllExclusions(ColourfulRgbLinear rgbLinear)
    {
        var exclusions = new List<string>();
        if (HasNegativeLinear(rgbLinear)) exclusions.Add("Colourful does not handle linear companding when negative");
        return exclusions;
    }

    private static bool IsGreyscale(ColourfulRgb rgb) => rgb.R == rgb.G && rgb.G == rgb.B;
    private static bool HasNoChromaticityY(ColourfulXyy xyy) => xyy.y <= 0.0;
    private static bool HasNegativeLinear(ColourfulRgbLinear rgbLinear) => rgbLinear.R < 0 || rgbLinear.G < 0 || rgbLinear.B < 0;
}