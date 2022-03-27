namespace Wacton.Unicolour;

internal static class Conversion
{
    // https://en.wikipedia.org/wiki/HSL_and_HSV#From_RGB
    public static Hsb RgbToHsb(Rgb rgb)
    {
        var r = rgb.R;
        var g = rgb.G;
        var b = rgb.B;

        var components = new[] {r, g, b};
        var xMax = components.Max();
        var xMin = components.Min();
        var chroma = xMax - xMin;

        double hue;
        if (chroma == 0.0) hue = 0;
        else if (xMax == r) hue = 60 * (0 + ((g - b) / chroma));
        else if (xMax == g) hue = 60 * (2 + ((b - r) / chroma));
        else if (xMax == b) hue = 60 * (4 + ((r - g) / chroma));
        else throw new InvalidOperationException();
        hue = hue < 0 ? 360 + hue : hue;
        var brightness = xMax;
        var saturation = brightness == 0 ? 0 : chroma / brightness;
        return new Hsb(hue, saturation, brightness, false);
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#From_RGB
    public static Hsl RgbToHsl(Rgb rgb)
    {
        var r = rgb.R;
        var g = rgb.G;
        var b = rgb.B;

        var components = new[] {r, g, b};
        var xMax = components.Max();
        var xMin = components.Min();
        var chroma = xMax - xMin;
        var lightness = (xMax + xMin) / 2.0;

        double hue;
        if (chroma == 0.0) hue = 0;
        else if (xMax == r) hue = 60 * (0 + ((g - b) / chroma));
        else if (xMax == g) hue = 60 * (2 + ((b - r) / chroma));
        else if (xMax == b) hue = 60 * (4 + ((r - g) / chroma));
        else throw new InvalidOperationException();
        hue = hue < 0 ? 360 + hue : hue;
        var saturation = lightness > 0.0 && lightness < 1.0
            ? (xMax - lightness) / Math.Min(lightness, 1 - lightness)
            : 0.0;
        
        return new Hsl(hue, saturation, lightness, false);
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB
    public static Rgb HsbToRgb(Hsb hsb, Configuration config)
    {
        var hue = hsb.H;
        var saturation = hsb.S;
        var brightness = hsb.B;
        
        var chroma = brightness * saturation;
        var h = hue / 60;
        var x = chroma * (1 - Math.Abs(((h % 2) - 1)));

        var (r, g, b) = h switch
        {
            < 1 => (chroma, x, 0.0),
            < 2 => (x, chroma, 0.0),
            < 3 => (0.0, chroma, x),
            < 4 => (0.0, x, chroma),
            < 5 => (x, 0.0, chroma),
            < 6 => (chroma, 0.0, x),
            _ => (0.0, 0.0, 0.0)
        };

        var m = brightness - chroma;
        var (red, green, blue) = (r + m, g + m, b + m);
        return new Rgb(red, green, blue, config);
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_HSL
    public static Hsl HsbToHsl(Hsb hsb)
    {
        var hue = hsb.H;
        var lightness = hsb.B * (1 - hsb.S / 2);
        var saturation = lightness is > 0.0 and < 1.0
            ? (hsb.B - lightness) / Math.Min(lightness, 1 - lightness)
            : 0;

        return new Hsl(hue, saturation, lightness, hsb.HasHue);
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_HSV
    public static Hsb HslToHsb(Hsl hsl)
    {
        var hue = hsl.H;
        var lightness = hsl.L;
        var brightness = lightness + hsl.S * Math.Min(lightness, 1 - lightness);
        var saturation = brightness > 0.0
            ? 2 * (1 - lightness / brightness)
            : 0;

        return new Hsb(hue, saturation, brightness);
    }

    // https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
    public static Xyz RgbToXyz(Rgb rgb, Configuration config)
    {
        var rgbLinearMatrix = new Matrix(new[,]
        {
            {rgb.RLinear},
            {rgb.GLinear},
            {rgb.BLinear}
        });

        var transformationMatrix = config.RgbToXyzMatrix;
        var xyzMatrix = transformationMatrix.Multiply(rgbLinearMatrix);

        var x = xyzMatrix[0, 0];
        var y = xyzMatrix[1, 0];
        var z = xyzMatrix[2, 0];

        return new Xyz(x, y, z);
    }
    
    // https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
    public static Rgb XyzToRgb(Xyz xyz, Configuration config)
    {
        var xyzMatrix = new Matrix(new[,]
        {
            {xyz.X},
            {xyz.Y},
            {xyz.Z}
        });

        var transformationMatrix = config.XyzToRgbMatrix;
        var rgbLinearMatrix = transformationMatrix.Multiply(xyzMatrix);

        var rLinear = rgbLinearMatrix[0, 0];
        var gLinear = rgbLinearMatrix[1, 0];
        var bLinear = rgbLinearMatrix[2, 0];

        var r = config.Compand(rLinear);
        var g = config.Compand(gLinear);
        var b = config.Compand(bLinear);

        return new Rgb(r, g, b, config);
    }
    
    // https://en.wikipedia.org/wiki/CIELAB_color_space#From_CIEXYZ_to_CIELAB
    public static Lab XyzToLab(Xyz xyz, Configuration config)
    {
        var x = xyz.X;
        var y = xyz.Y;
        var z = xyz.Z;

        var referenceWhite = config.XyzWhitePoint;
        var xRatio = x * 100 / referenceWhite.X;
        var yRatio = y * 100 / referenceWhite.Y;
        var zRatio = z * 100 / referenceWhite.Z;
        
        var delta = 6.0 / 29.0;
        double F(double t) => t > Math.Pow(delta, 3) ? Math.Pow(t, 1 / 3.0) : t * (1 / 3.0) * Math.Pow(delta, -2) + 4.0 / 29.0;
        var l = 116 * F(yRatio) - 16;
        var a = 500 * (F(xRatio) - F(yRatio));
        var b = 200 * (F(yRatio) - F(zRatio));

        return new Lab(l, a, b);
    }
    
    // https://en.wikipedia.org/wiki/CIELAB_color_space#From_CIELAB_to_CIEXYZ
    public static Xyz LabToXyz(Lab lab, Configuration config)
    {
        var l = lab.L;
        var a = lab.A;
        var b = lab.B;
        
        var referenceWhite = config.XyzWhitePoint;
        var delta = 6.0 / 29.0;
        double F(double t) => t > delta ? Math.Pow(t, 3.0) : 3 * Math.Pow(delta, 2) * (t - 4.0 / 29.0);
        var x = referenceWhite.X / 100.0 * F((l + 16) / 116.0 + a / 500.0);
        var y = referenceWhite.Y / 100.0 * F((l + 16) / 116.0);
        var z = referenceWhite.Z / 100.0 * F((l + 16) / 116.0 - b / 200.0);

        return new Xyz(x, y, z);
    }
}