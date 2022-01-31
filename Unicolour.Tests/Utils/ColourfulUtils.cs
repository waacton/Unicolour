namespace Wacton.Unicolour.Tests.Utils;

using Colourful;
using Wacton.Unicolour.Tests.Lookups;

internal static class ColourfulUtils
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
        var rgb = new RGBColor(r, g, b);

        var rgbLinearConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLinearRGB().Build();
        var rgbLinear = rgbLinearConverter.Convert(rgb);

        var xyzConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToXYZ(Illuminants.D65).Build();
        var xyz = xyzConverter.Convert(rgb);

        var labConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLab(Illuminants.D65).Build();
        var lab = labConverter.Convert(rgb);
        
        return new TestColour
        {
            Name = name,
            Rgb = (rgb.R, rgb.G, rgb.B),
            RgbLinear = (rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Xyz = (xyz.X, xyz.Y, xyz.Z),
            Lab = (lab.L, lab.a, lab.b)
        };
    }
}