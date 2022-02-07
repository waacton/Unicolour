namespace Wacton.Unicolour.Tests.Utils;

using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using Wacton.Unicolour.Tests.Lookups;
using SixLaborsRgb = SixLabors.ImageSharp.ColorSpaces.Rgb;
using SixLaborsRgbLinear = SixLabors.ImageSharp.ColorSpaces.LinearRgb;
using SixLaborsHsb = SixLabors.ImageSharp.ColorSpaces.Hsv;
using SixLaborsXyz = SixLabors.ImageSharp.ColorSpaces.CieXyz;
using SixLaborsLab = SixLabors.ImageSharp.ColorSpaces.CieLab;
using SixLaborsIlluminants = SixLabors.ImageSharp.ColorSpaces.Illuminants;

internal static class SixLaborsUtils
{
    public static TestColour FromRgb255(int r255, int g255, int b255) => FromRgb255(r255, g255, b255, $"{r255:000} {g255:000} {b255:000}");
    private static TestColour FromRgb255(int r255, int g255, int b255, string name)
    {
        var r = r255 / 255.0;
        var g = g255 / 255.0;
        var b = b255 / 255.0;
        return FromRgb(r, g, b, name);
    }
    
    public static TestColour FromRgb(double r, double g, double b) => FromRgb(r, g, b, $"{r:F2} {g:F2} {b:F2}");
    private static TestColour FromRgb(double r, double g, double b, string name)
    {
        var converter = new ColorSpaceConverter(new ColorSpaceConverterOptions
        {
            TargetLabWhitePoint = SixLaborsIlluminants.D65
        });

        var rgb = new SixLaborsRgb((float) r, (float) g, (float) b, RgbWorkingSpaces.SRgb);
        var rgbLinear =  converter.ToLinearRgb(rgb);
        var hsb =  converter.ToHsv(rgb);
        var xyz =  converter.ToCieXyz(rgb);
        var lab =  converter.ToCieLab(rgb);

        return new TestColour
        {
            Name = name,
            Rgb = (rgb.R, rgb.G, rgb.B),
            RgbLinear = (rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Hsb = (hsb.H, hsb.S, hsb.V),
            Xyz = (xyz.X, xyz.Y, xyz.Z),
            Lab = (lab.L, lab.A, lab.B)
        };
    }

    public static TestColour FromHsb(double h, double s, double b) => FromHsb(h, s, b, $"{h:F2} {s:F2} {b:F2}");
    public static TestColour FromHsb(double h, double s, double b, string name)
    {
        var converter = new ColorSpaceConverter(new ColorSpaceConverterOptions
        {
            TargetLabWhitePoint = SixLaborsIlluminants.D65
        });

        var hsb = new SixLaborsHsb((float) h, (float) s, (float) b);
        var rgb =  converter.ToRgb(hsb);
        var rgbLinear =  converter.ToLinearRgb(hsb);
        var xyz =  converter.ToCieXyz(hsb);
        var lab =  converter.ToCieLab(hsb);

        return new TestColour
        {
            Name = name,
            Rgb = (rgb.R, rgb.G, rgb.B),
            RgbLinear = (rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Xyz = (xyz.X, xyz.Y, xyz.Z),
            Lab = (lab.L, lab.A, lab.B)
        };
    }
}