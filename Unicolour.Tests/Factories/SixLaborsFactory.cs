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
using SixLaborsLuv = SixLabors.ImageSharp.ColorSpaces.CieLuv;
using SixLaborsIlluminants = SixLabors.ImageSharp.ColorSpaces.Illuminants;

/*
 * SixLabors doesn't do a great job of converting to or from LAB / LUV
 * SixLabors doesn't handle very small RGB -> HSB / HSL
 * SixLabors produces unexpected results for XYZ -> HSB / HSL due to clamping RGB during conversion
 */
internal class SixLaborsFactory : ITestColourFactory
{
    private static readonly Tolerances Tolerances = new() { Rgb = 0.001, RgbLinear = 0.005, Hsb = 0.000005, Hsl = 0.000005, Xyz = 0.005, Lab = 0.1, Luv = 0.2 };

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
        return FromRgb(rgb, name, Tolerances);
    }

    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var rgb = new SixLaborsRgb((float) r, (float) g, (float) b, RgbWorkingSpaces.SRgb);
        return FromRgb(rgb, name, Tolerances with {Hsb = 0.05, Hsl = 0.05});
    }

    private static TestColour FromRgb(SixLaborsRgb rgb, string name, Tolerances tolerances)
    {
        var rgbLinear = Converter.ToLinearRgb(rgb);
        var hsb = Converter.ToHsv(rgb);
        var hsl = Converter.ToHsl(rgb);
        var xyz = Converter.ToCieXyz(rgb);
        var lab = Converter.ToCieLab(rgb);
        var luv = Converter.ToCieLuv(rgb);
        return Create(name, rgb, rgbLinear, hsb, hsl, xyz, lab, luv, tolerances);
    }

    public TestColour FromHsb(double h, double s, double b, string name)
    {
        var hsb = new SixLaborsHsb((float) h, (float) s, (float) b);
        var rgb = Converter.ToRgb(hsb);
        var rgbLinear = Converter.ToLinearRgb(hsb);
        var hsl = Converter.ToHsl(hsb);
        var xyz = Converter.ToCieXyz(hsb);
        var lab = Converter.ToCieLab(hsb);
        var luv = Converter.ToCieLuv(hsb);
        return Create(name, rgb, rgbLinear, hsb, hsl, xyz, lab, luv, Tolerances with {Hsl = 0.00005, Lab = 0.125});
    }

    public TestColour FromHsl(double h, double s, double l, string name)
    {
        var hsl = new SixLaborsHsl((float) h, (float) s, (float) l);
        var rgb = Converter.ToRgb(hsl);
        var rgbLinear = Converter.ToLinearRgb(hsl);
        var hsb = Converter.ToHsv(hsl);
        var xyz = Converter.ToCieXyz(hsl);
        var lab = Converter.ToCieLab(hsl);
        var luv = Converter.ToCieLuv(hsl);
        return Create(name, rgb, rgbLinear, hsb, hsl, xyz, lab, luv, Tolerances with {Rgb = 0.05, Hsb = 0.00005, Lab = 0.175});
    }

    // SixLabors XYZ -> HSB / HSL uses a clamped version of RGB during conversion
    // which significantly affects the result when transforming to the hue-based cylindrical model
    // for this reason, not comparing hue-based values here
    public TestColour FromXyz(double x, double y, double z, string name)
    {
        var xyz = new SixLaborsXyz((float) x, (float) y, (float) z);
        var rgb = Converter.ToRgb(xyz);
        var rgbLinear = Converter.ToLinearRgb(xyz);
        var lab = Converter.ToCieLab(xyz);
        var luv = Converter.ToCieLuv(xyz);
        return CreateWithoutHue(name, rgb, rgbLinear, xyz, lab, luv, Tolerances);
    }

    // SixLabors doesn't do a good job of converting from LAB / LUV even when specifying D65 illuminant
    // potentially due to clamping XYZ values during conversion (e.g. LAB -> XYZ -> RGB)
    public TestColour FromLab(double l, double a, double b, string name) => throw new NotImplementedException();
    public TestColour FromLuv(double l, double u, double v, string name) => throw new NotImplementedException();
    
    private static TestColour Create(string name, 
        SixLaborsRgb rgb, SixLaborsRgbLinear rgbLinear, SixLaborsHsb hsb, SixLaborsHsl hsl,
        SixLaborsXyz xyz, SixLaborsLab lab, SixLaborsLuv luv,
        Tolerances tolerances)
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
            Luv = new(luv.L, luv.U, luv.V),
            Tolerances = tolerances,
            ExcludeFromHueBasedTestReasons = hueExclusions
        };
    }
    
    private static TestColour CreateWithoutHue(string name, 
        SixLaborsRgb rgb, SixLaborsRgbLinear rgbLinear,
        SixLaborsXyz xyz, SixLaborsLab lab, SixLaborsLuv luv,
        Tolerances tolerances)
    {
        var hueExclusions = new List<string>();
        if (HasLowChroma(rgb)) hueExclusions.Add("SixLabors converts via RGB and does not handle low RGB chroma");
        
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.R, rgb.G, rgb.B),
            RgbLinear = new(rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Xyz = new(xyz.X, xyz.Y, xyz.Z),
            Lab = new(lab.L, lab.A, lab.B),
            Luv = new(luv.L, luv.U, luv.V),
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