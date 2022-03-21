namespace Wacton.Unicolour.Tests.Factories;

using Colourful;
using Wacton.Unicolour.Tests.Utils;
using ColourfulRgb = Colourful.RGBColor;
using ColourfulRgbLinear = Colourful.LinearRGBColor;
using ColourfulXyz = Colourful.XYZColor;
using ColourfulLab = Colourful.LabColor;
using ColourfulIlluminants = Colourful.Illuminants;

internal class ColourfulFactory : ITestColourFactory
{
    /*
     * Colourful doesn't support HSB / HSL
     */
    private static readonly Tolerances Tolerances = new() { Rgb = 0.00000000001, RgbLinear = 0.00000000001, Xyz = 0.00000000001, Lab = 0.0000005 };
    
    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var rgbLinearConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLinearRGB().Build();
        var xyzConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToXYZ(ColourfulIlluminants.D65).Build();
        var labConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLab(ColourfulIlluminants.D65).Build();

        var rgb = new ColourfulRgb(r, g, b);
        var rgbLinear = rgbLinearConverter.Convert(rgb);
        var xyz = xyzConverter.Convert(rgb);
        var lab = labConverter.Convert(rgb);
        return Create(name, rgb, rgbLinear, xyz, lab);
    }

    // Colourful does not support HSB
    public TestColour FromHsb(double h, double s, double b, string name)
    {
        return new TestColour();
    }

    // Colourful does not support HSL
    public TestColour FromHsl(double h, double s, double l, string name)
    {
        return new TestColour();
    }

    private static TestColour Create(string name, ColourfulRgb rgb, ColourfulRgbLinear rgbLinear, ColourfulXyz xyz, ColourfulLab lab)
    {
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.R, rgb.G, rgb.B),
            RgbLinear = new(rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Xyz = new(xyz.X, xyz.Y, xyz.Z),
            Lab = new(lab.L, lab.a, lab.b),
            Tolerances = Tolerances
        };
    }
}