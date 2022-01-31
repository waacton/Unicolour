namespace Wacton.Unicolour.Tests.Utils;

using System;
using Wacton.Unicolour.Tests.Lookups;
using ColorMineRgb = ColorMine.ColorSpaces.Rgb;
using ColorMineHsb = ColorMine.ColorSpaces.Hsb;
using ColorMineXyz = ColorMine.ColorSpaces.Xyz;
using ColorMineLab = ColorMine.ColorSpaces.Lab;

internal static class ColorMineUtils
{
    public static TestColour FromRgb(double r, double g, double b) => FromRgb(r, g, b, $"{r:F2} {g:F2} {b:F2}");
    private static TestColour FromRgb(double r, double g, double b, string name)
    {
        var r255 = (int)Math.Round(r * 255.0);
        var g255 = (int)Math.Round(g * 255.0);
        var b255 = (int)Math.Round(b * 255.0);
        return FromRgb255(r255, g255, b255, name);
    }

    public static TestColour FromRgb255(int r255, int g255, int b255) => FromRgb255(r255, g255, b255, $"{r255:000} {g255:000} {b255:000}");
    private static TestColour FromRgb255(int r255, int g255, int b255, string name)
    {
        var rgb = new ColorMineRgb { R = r255, G = g255, B = b255 };
        var hsb = rgb.To<ColorMineHsb>();
        var xyz = rgb.To<ColorMineXyz>();
        var lab = rgb.To<ColorMineLab>();
        
        return new TestColour
        {
            Name = name,
            Rgb = (rgb.R / 255.0, rgb.G / 255.0, rgb.B / 255.0),
            Hsb = (hsb.H, hsb.S, hsb.B),
            Xyz = (xyz.X / 100.0, xyz.Y / 100.0, xyz.Z / 100.0),
            Lab = (lab.L, lab.A, lab.B)
        };
    }
    
    public static TestColour FromHsb(double h, double s, double b) => FromHsb(h, s, b, $"{h:F2} {s:F2} {b:F2}");
    public static TestColour FromHsb(double h, double s, double b, string name)
    {
        var hsb = new ColorMineHsb {H = h, S = s, B = b};
        var rgb = hsb.To<ColorMineRgb>();
        var xyz = hsb.To<ColorMineXyz>();
        var lab = hsb.To<ColorMineLab>();
        
        return new TestColour
        {
            Name = name,
            Rgb = (rgb.R / 255.0, rgb.G / 255.0, rgb.B / 255.0),
            Hsb = (hsb.H, hsb.S, hsb.B),
            Xyz = (xyz.X / 100.0, xyz.Y / 100.0, xyz.Z / 100.0),
            Lab = (lab.L, lab.A, lab.B)
        };
    }
}