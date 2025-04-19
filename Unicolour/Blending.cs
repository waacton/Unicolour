namespace Wacton.Unicolour;

/*
 * assumes source-over compositing
 * ----------
 * while CSS presumably blends in sRGB, Unicolour does not enforce that restriction
 * instead blending is performed in the RGB space of the source colour
 * and non-separable blend modes reuse the luma coefficients from YbrConfig
 * ----------
 * in same way as TSL conversion, CSS luma calculation is based on Rec. 601 coefficients
 * despite them being colorimetrically unrelated to sRGB (sRGB uses Rec. 709 primaries)
 * so this repurposes the luma coefficients defined in YbrConfig, and allows user configuration, but is not obvious
 * if this became problematic, could be refactored to extract a LumaConfig from YbrConfig
 * (at the cost of making YbrConfig more brittle - YbrConfig.Jpeg would need LumaConfig.Rec601 to behave as expected)
 */
internal static class Blending
{
    // https://www.w3.org/TR/compositing-1/#generalformula
    internal static Unicolour Blend(Unicolour source, Unicolour backdrop, BlendMode blendMode)
    {
        var config = source.Configuration;
        backdrop = backdrop.ConvertToConfiguration(config);

        var cs = source.Rgb.ConstrainedTriplet;
        var cb = backdrop.Rgb.ConstrainedTriplet;
        var @as = source.Alpha.ConstrainedA;
        var ab = backdrop.Alpha.ConstrainedA;

        var blend = BlendColour(cb.ToArray(), cs.ToArray(), blendMode, config.Ybr);
        var co = CompositeAlpha(cb, ab, cs, @as, blend);
        
        // CSS spec implies that 'co' is premultipied and 'Co' is the actual colour component
        // but Co here is clearly premultiplied
        var ao = @as + ab * (1 - @as);
        var (ro, go, bo) = co.WithUnpremultipliedAlpha(ao);
        return new Unicolour(config, ColourSpace.Rgb, ro, go, bo, ao);
    }

    private static ColourTriplet BlendColour(double[] cb, double[] cs, BlendMode blendMode, YbrConfiguration ybrConfig)
    {
        var blend = blendMode switch
        {
            BlendMode.Normal => Separable(cb, cs, Normal),
            BlendMode.Multiply => Separable(cb, cs, Multiply),
            BlendMode.Screen => Separable(cb, cs, Screen),
            BlendMode.Overlay => Separable(cb, cs, Overlay),
            BlendMode.Darken => Separable(cb, cs, Darken),
            BlendMode.Lighten => Separable(cb, cs, Lighten),
            BlendMode.ColourDodge => Separable(cb, cs, ColourDodge),
            BlendMode.ColourBurn => Separable(cb, cs, ColourBurn),
            BlendMode.HardLight => Separable(cb, cs, HardLight),
            BlendMode.SoftLight => Separable(cb, cs, SoftLight),
            BlendMode.Difference => Separable(cb, cs, Difference),
            BlendMode.Exclusion => Separable(cb, cs, Exclusion),
            BlendMode.Hue => Hue(cb, cs, ybrConfig),
            BlendMode.Saturation => Saturation(cb, cs, ybrConfig),
            BlendMode.Colour => Colour(cb, cs, ybrConfig),
            BlendMode.Luminosity => Luminosity(cb, cs, ybrConfig),
            _ => throw new ArgumentOutOfRangeException(nameof(blendMode), blendMode, null)
        };

        return new(blend[0], blend[1], blend[2]);
    }

    private static ColourTriplet CompositeAlpha(ColourTriplet cb, double ab, ColourTriplet cs, double @as, ColourTriplet blend)
    {
        // Cs = (1 - αb) x Cs + αb x B(Cb, Cs)
        cs = cs.WithPremultipliedAlpha(1 - ab);
        blend = blend.WithPremultipliedAlpha(ab);
        cs = new(cs.First + blend.First, cs.Second + blend.Second, cs.Third + blend.Third);
        
        // Porter Duff source-over compositing; could provide other compositing operators in future
        const double fa = 1;
        var fb = 1 - @as;
        
        // Co = αs x Fa x Cs + αb x Fb x Cb
        cs = cs.WithPremultipliedAlpha(@as * fa);
        cb = cb.WithPremultipliedAlpha(ab * fb);
        return new ColourTriplet(cs.First + cb.First, cs.Second + cb.Second, cs.Third + cb.Third);
    }

    private static double[] Separable(double[] cb, double[] cs, Func<double, double, double> func) => cb.Zip(cs, func).ToArray();
    
    private static double Normal(double b, double s) => s;
    private static double Multiply(double b, double s) => b * s;
    private static double Screen(double b, double s) => b + s - b * s;
    private static double Overlay(double b, double s) => HardLight(s, b);
    private static double Darken(double b, double s) => Math.Min(b, s);
    private static double Lighten(double b, double s) => Math.Max(b, s);
    
    private static double ColourDodge(double b, double s) => b switch
    {
        0 => 0,
        1 => 1,
        _ => Math.Min(1, b / (1 - s))
    };
    
    private static double ColourBurn(double b, double s) => b switch
    {
        1 => 1,
        0 => 0,
        _ => 1 - Math.Min(1, (1 - b) / s)
    };

    private static double HardLight(double b, double s) => s switch
    {
        <= 0.5 => Multiply(b, 2 * s),
        _ => Screen(b, 2 * s - 1)
    };

    private static double SoftLight(double b, double s) => s switch
    {
        <= 0.5 => b - (1 - 2 * s) * b * (1 - b),
        _ => b + (2 * s - 1) * ((b <= 0.25 ? ((16 * b - 12) * b + 4) * b : Math.Sqrt(b)) - b)
    };

    private static double Difference(double b, double s) => Math.Abs(b - s);
    private static double Exclusion(double b, double s) => b + s - 2 * b * s;

    private static double[] Hue(double[] b, double[] s, YbrConfiguration ybrConfig)
    {
        return SetLuma(SetSaturation(s, GetSaturation(b)), GetLuma(b, ybrConfig), ybrConfig);
    }
    
    private static double[] Saturation(double[] b, double[] s, YbrConfiguration ybrConfig)
    {
        return SetLuma(SetSaturation(b, GetSaturation(s)), GetLuma(b, ybrConfig), ybrConfig);
    }
    
    private static double[] Colour(double[] b, double[] s, YbrConfiguration ybrConfig)
    {
        return SetLuma(s, GetLuma(b, ybrConfig), ybrConfig);
    }
    
    private static double[] Luminosity(double[] b, double[] s, YbrConfiguration ybrConfig)
    {
        return SetLuma(b, GetLuma(s, ybrConfig), ybrConfig);
    }

    private static double GetLuma(double[] rgb, YbrConfiguration ybrConfig)
    {
        return ybrConfig.Kr * rgb[0] + ybrConfig.Kg * rgb[1] + ybrConfig.Kb * rgb[2];
    }

    private static double[] SetLuma(double[] rgb, double l, YbrConfiguration ybrConfig)
    {
        var d = l - GetLuma(rgb, ybrConfig);
        return Clip(rgb.Select(c => c + d).ToArray(), ybrConfig);
    }
    
    private static double[] Clip(double[] rgb, YbrConfiguration ybrConfig)
    {
        var l = GetLuma(rgb, ybrConfig);
        var n = rgb.Min();
        var x = rgb.Max();

        if (n < 0)
        {
            return rgb.Select(c => l + (c - l) * l / (l - n)).ToArray();
        }

        if (x > 1)
        {
            return rgb.Select(c => l + (c - l) * (1 - l) / (x - l)).ToArray();
        }

        return rgb;
    }
    
    private static double GetSaturation(double[] rgb)
    {
        return rgb.Max() - rgb.Min();
    }
    
    private static double[] SetSaturation(double[] rgb, double s)
    {
        var ordered = rgb.Select((c, i) => (c, i)).OrderBy(c => c).ToArray();
        var (min, mid, max) = (ordered[0].i, ordered[1].i, ordered[2].i);

        var c = new double[3];
        if (rgb[max] > rgb[min])
        {
            c[mid] = (rgb[mid] - rgb[min]) * s / (rgb[max] - rgb[min]);
            c[max] = s;
        }
        else
        {
            c[mid] = 0;
            c[max] = 0;
        }

        c[min] = 0;
        return c;
    }
}