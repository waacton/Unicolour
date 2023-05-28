namespace Wacton.Unicolour.Tests.Factories;

using System;
using System.Collections.Generic;
using Wacton.Unicolour.Tests.Utils;
using ColorMineRgb = ColorMine.ColorSpaces.Rgb;
using ColorMineHsb = ColorMine.ColorSpaces.Hsb;
using ColorMineHsl = ColorMine.ColorSpaces.Hsl;
using ColorMineXyz = ColorMine.ColorSpaces.Xyz;
using ColorMineXyy = ColorMine.ColorSpaces.Yxy;
using ColorMineLab = ColorMine.ColorSpaces.Lab;
using ColorMineLchab = ColorMine.ColorSpaces.Lch;
using ColorMineLuv = ColorMine.ColorSpaces.Luv;

/*
 * ColorMine does not support HWB / LCHuv / HSLuv / HPLuv / CAM16 / ICtCp / Jzazbz / Jzczhz / Oklab / Oklch
 * ColorMine does not expose linear RGB
 * ColorMine does a bad job of converting to HSL
 * ColorMine does a terrible job of converting from XYZ / LAB / LCHab / LUV
 */
internal class ColorMineFactory : ITestColourFactory
{
    private static readonly Tolerances Tolerances = new()
        {Rgb = 0.0005, Hsb = 0.00005, Hsl = 0.0125, Xyz = 0.0005, Xyy = 0.0005, Lab = 0.05, Lchab = 0.05, Luv = 0.05};

    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var r255 = (int)Math.Round(r * 255.0);
        var g255 = (int)Math.Round(g * 255.0);
        var b255 = (int)Math.Round(b * 255.0);
        return FromRgb255(r255, g255, b255, name);
    }
    
    public TestColour FromRgb255(int r255, int g255, int b255, string name)
    {
        var rgb = new ColorMineRgb { R = r255, G = g255, B = b255 };
        var hsb = rgb.To<ColorMineHsb>();
        var hsl = rgb.To<ColorMineHsl>();
        var xyz = rgb.To<ColorMineXyz>();
        var xyy = rgb.To<ColorMineXyy>();
        var lab = rgb.To<ColorMineLab>();
        var lchab = rgb.To<ColorMineLchab>();
        var luv = rgb.To<ColorMineLuv>();
        return Create(name, rgb, hsb, hsl, xyz, xyy, lab, lchab, luv, Tolerances);
    }

    public TestColour FromHsb(double h, double s, double b, string name)
    {
        var hsb = new ColorMineHsb {H = h, S = s, B = b};
        var rgb = hsb.To<ColorMineRgb>();
        var hsl = hsb.To<ColorMineHsl>();
        var xyz = hsb.To<ColorMineXyz>();
        var xyy = hsb.To<ColorMineXyy>();
        var lab = hsb.To<ColorMineLab>();
        var lchab = hsb.To<ColorMineLchab>();
        var luv = hsb.To<ColorMineLuv>();
        return Create(name, rgb, hsb, hsl, xyz, xyy, lab, lchab, luv, Tolerances);
    }

    // ColorMine HSL for some reason uses 0-100 despite HSB using 0-1
    public TestColour FromHsl(double h, double s, double l, string name)
    {
        var hsl = new ColorMineHsl {H = h, S = s * 100, L = l * 100};
        var rgb = hsl.To<ColorMineRgb>();
        var hsb = hsl.To<ColorMineHsb>();
        var xyz = hsl.To<ColorMineXyz>();
        var xyy = hsl.To<ColorMineXyy>();
        var lab = hsl.To<ColorMineLab>();
        var lchab = hsl.To<ColorMineLchab>();
        var luv = hsl.To<ColorMineLuv>();
        return Create(name, rgb, hsb, hsl, xyz, xyy, lab, lchab, luv, Tolerances);
    }

    // ColorMine from XYZ, xyY, LAB, LCHab & LUV is so bad for most conversions, it's not worth testing
    // ColorMine doesn't support LCHuv
    public TestColour FromXyz(double x, double y, double z, string name) => throw new NotImplementedException();
    public TestColour FromXyy(double x, double y, double upperY, string name) => throw new NotImplementedException();
    public TestColour FromLab(double l, double a, double b, string name) => throw new NotImplementedException();
    public TestColour FromLchab(double l, double c, double h, string name) => throw new NotImplementedException();
    public TestColour FromLuv(double l, double u, double v, string name) => throw new NotImplementedException();
    public TestColour FromLchuv(double l, double c, double h, string name) => throw new NotImplementedException();

    private static TestColour Create(string name, 
        ColorMineRgb rgb, ColorMineHsb hsb, ColorMineHsl hsl, 
        ColorMineXyz xyz, ColorMineXyy xyy, ColorMineLab lab, ColorMineLchab lchab, ColorMineLuv luv,
        Tolerances tolerances)
    {
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.R / 255.0, rgb.G / 255.0, rgb.B / 255.0),
            Hsb = new(hsb.H, hsb.S, hsb.B),
            Hsl = new(hsl.H, hsl.S / 100.0, hsl.L / 100.0),
            Xyz = new(xyz.X / 100.0, xyz.Y / 100.0, xyz.Z / 100.0),
            Xyy = new(xyy.X, xyy.Y2, xyy.Y1 / 100.0), // ColorMine is Yxy, hence Y1 is luminance and Y2 is y-chromaticity
            Lab = new(lab.L, lab.A, lab.B),
            Lchab = new(lchab.L, lchab.C, lchab.H),
            Luv = new(luv.L, luv.U, luv.V),
            Tolerances = tolerances,
            ExcludeFromHsxTestReasons = HsxExclusions(rgb, hsb, hsl),
            ExcludeFromLchTestReasons = LchExclusions(rgb, hsb, hsl),
            ExcludeFromXyyTestReasons = XyyExclusions(xyy)
        };
    }
    
    private static List<string> HsxExclusions(ColorMineRgb rgb, ColorMineHsb hsb, ColorMineHsl hsl)
    {
        var exclusions = new List<string>();
        if (HasInconsistentHue(hsb, hsl)) exclusions.Add("ColorMine converts via RGB and loses hue due to rounding to greyscale");
        if (HasRgbTruncationError(rgb)) exclusions.Add("ColorMine converts via RGB and has RGB truncation errors");
        if (HasLostSaturation(hsb, hsl)) exclusions.Add("ColorMine loses saturation due to rounding");
        return exclusions;
    }
    
    private static List<string> LchExclusions(ColorMineRgb rgb, ColorMineHsb hsb, ColorMineHsl hsl)
    {
        var exclusions = new List<string>();
        if (IsGreyscale(rgb)) exclusions.Add("ColorMine calculates hue differently value when RGB is greyscale");
        if (HasInconsistentHue(hsb, hsl)) exclusions.Add("ColorMine converts via RGB and loses hue due to rounding to greyscale");
        return exclusions;
    }
    
    private static List<string> XyyExclusions(ColorMineXyy xyy)
    {
        var exclusions = new List<string>();
        if (HasNoChromaticityY(xyy)) exclusions.Add("ColorMine sets all values to 0 when xyY has no y-chromaticity");
        return exclusions;
    }
    
    private static bool HasInconsistentHue(ColorMineHsb hsb, ColorMineHsl hsl) => Math.Abs(hsb.H - hsl.H) > 0.01;
    private static bool HasRgbTruncationError(ColorMineRgb rgb) => HasTruncationError(rgb.R) || HasTruncationError(rgb.G) || HasTruncationError(rgb.B);
    private static bool HasLostSaturation(ColorMineHsb hsl, ColorMineHsl hsb) => hsl.S > 0.0 && hsb.S == 0.0 || hsb.S > 0.0 && hsl.S == 0.0;
    private static bool IsGreyscale(ColorMineRgb rgb) => rgb.R == rgb.G && rgb.G == rgb.B;
    private static bool HasNoChromaticityY(ColorMineXyy xyy) => xyy.Y2 <= 0.0;

    private static bool HasTruncationError(double value)
    {
        // this is what ColorMine does to RGB values
        var truncated = (int) value; 
        
        // even if the value has been "barely" truncated
        // truncation to zero totally changes the outcome
        if (value > 0 && truncated == 0)
        {
            return true;
        }

        // somewhat arbitrary as to how much truncation is too much
        // but generally, any early truncation will introduce errors
        return Math.Abs(truncated - value) > 0.1; 
    }
}