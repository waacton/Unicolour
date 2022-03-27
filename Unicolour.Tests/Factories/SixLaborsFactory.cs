namespace Wacton.Unicolour.Tests.Factories;

using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using Wacton.Unicolour.Tests.Utils;
using SixLaborsRgb = SixLabors.ImageSharp.ColorSpaces.Rgb;
using SixLaborsRgbLinear = SixLabors.ImageSharp.ColorSpaces.LinearRgb;
using SixLaborsHsb = SixLabors.ImageSharp.ColorSpaces.Hsv;
using SixLaborsHsl = SixLabors.ImageSharp.ColorSpaces.Hsl;
using SixLaborsXyz = SixLabors.ImageSharp.ColorSpaces.CieXyz;
using SixLaborsLab = SixLabors.ImageSharp.ColorSpaces.CieLab;
using SixLaborsIlluminants = SixLabors.ImageSharp.ColorSpaces.Illuminants;

internal class SixLaborsFactory : ITestColourFactory
{
    /*
     * SixLabors doesn't seem to do a great job of converting to LAB
     * and does a poor job of converting from decimal RGB -> HSB / HSL
     */
    private static readonly Tolerances BaseTolerances = new() { Rgb = 0.001, RgbLinear = 0.005, Hsb = 0.000005, Hsl = 0.000005, Xyz = 0.005, Lab = 0.1 };
    private static readonly Tolerances FromRgbTolerances = BaseTolerances with { Hsb = 0.05, Hsl = 0.05 };
    private static readonly Tolerances FromHsbTolerances = BaseTolerances with { Hsl = 0.00005, Lab = 0.125 };
    private static readonly Tolerances FromHslTolerances = BaseTolerances with { Rgb = 0.05, Hsb = 0.00005, Lab = 0.175 };

    private static readonly ColorSpaceConverter Converter = new(new ColorSpaceConverterOptions
    {
        TargetLabWhitePoint = SixLaborsIlluminants.D65
    });
    
    public TestColour FromRgb255(int r255, int g255, int b255, string name)
    {
        var r = r255 / 255.0;
        var g = g255 / 255.0;
        var b = b255 / 255.0;
        var rgb = new SixLaborsRgb((float) r, (float) g, (float) b, RgbWorkingSpaces.SRgb);
        return FromRgb(rgb, name, BaseTolerances);
    }

    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var rgb = new SixLaborsRgb((float) r, (float) g, (float) b, RgbWorkingSpaces.SRgb);
        return FromRgb(rgb, name, FromRgbTolerances);
    }

    private static TestColour FromRgb(SixLaborsRgb rgb, string name, Tolerances tolerances)
    {
        var rgbLinear = Converter.ToLinearRgb(rgb);
        var hsb = Converter.ToHsv(rgb);
        var hsl = Converter.ToHsl(rgb);
        var xyz = Converter.ToCieXyz(rgb);
        var lab = Converter.ToCieLab(rgb);
        return Create(name, rgb, rgbLinear, hsb, hsl, xyz, lab, tolerances);
    }

    public TestColour FromHsb(double h, double s, double b, string name)
    {
        var hsb = new SixLaborsHsb((float) h, (float) s, (float) b);
        var rgb = Converter.ToRgb(hsb);
        var rgbLinear = Converter.ToLinearRgb(hsb);
        var hsl = Converter.ToHsl(hsb);
        var xyz = Converter.ToCieXyz(hsb);
        var lab = Converter.ToCieLab(hsb);
        return Create(name, rgb, rgbLinear, hsb, hsl, xyz, lab, FromHsbTolerances);
    }

    public TestColour FromHsl(double h, double s, double l, string name)
    {
        var hsl = new SixLaborsHsl((float) h, (float) s, (float) l);
        var rgb = Converter.ToRgb(hsl);
        var rgbLinear = Converter.ToLinearRgb(hsl);
        var hsb = Converter.ToHsv(hsl);
        var xyz = Converter.ToCieXyz(hsl);
        var lab = Converter.ToCieLab(hsl);
        return Create(name, rgb, rgbLinear, hsb, hsl, xyz, lab, FromHslTolerances);
    }

    private static TestColour Create(string name, SixLaborsRgb rgb, SixLaborsRgbLinear rgbLinear, SixLaborsHsb hsb, SixLaborsHsl hsl, SixLaborsXyz xyz, SixLaborsLab lab, Tolerances tolerances)
    {
        var hueExclusions = new List<string>();
        if (HasInconsistentHue(hsb, hsl)) hueExclusions.Add("SixLabors converts via RGB and loses hue when greyscale");
        if (HasLowChroma(rgb)) hueExclusions.Add("SixLabors converts via RGB and does not handle low RGB chroma");
        
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.R, rgb.G, rgb.B),
            RgbLinear = new(rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Hsb = new(hsb.H, hsb.S, hsb.V),
            Hsl = new(hsl.H, hsl.S, hsl.L),
            Xyz = new(xyz.X, xyz.Y, xyz.Z),
            Lab = new(lab.L, lab.A, lab.B),
            Tolerances = tolerances,
            ExcludeFromHueBasedTestReasons = hueExclusions
        };
    }
    
    private static bool HasInconsistentHue(SixLaborsHsb hsb, SixLaborsHsl hsl) => Math.Abs(hsb.H - hsl.H) > 0.01;

    private static bool HasLowChroma(SixLaborsRgb rgb)
    {
        // SixLabors can end up with extreme values (e.g. 0 or multiple of 60 hue) if the chroma is small
        // (potentially due to using floats instead of doubles)
        // which causes significant deviation from Unicolour calculations with very small values
        var components = new[] {rgb.R, rgb.G, rgb.B};
        var chroma = components.Max() - components.Min();
        return chroma < 0.01;
    }
}