namespace Wacton.Unicolour;

internal static class Converter
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
        return new Hsb(hue, saturation, brightness);
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB
    public static Rgb HsbToRgb(Hsb hsb)
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
        return new Rgb(red, green, blue);
    }

    // https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
    public static Xyz RgbToXyz(Rgb rgb)
    {
        var rLinear = rgb.RLinear;
        var gLinear = rgb.GLinear;
        var bLinear = rgb.BLinear;

        var x = (0.4124 * rLinear) + (0.3576 * gLinear) + (0.1805 * bLinear);
        var y = (0.2126 * rLinear) + (0.7152 * gLinear) + (0.0722 * bLinear);
        var z = (0.0193 * rLinear) + (0.1192 * gLinear) + (0.9505 * bLinear);

        return new Xyz(x, y, z);
    }
    
    // https://en.wikipedia.org/wiki/CIELAB_color_space#From_CIEXYZ_to_CIELAB
    public static Lab XyzToLab(Xyz xyz)
    {
        var x = xyz.X;
        var y = xyz.Y;
        var z = xyz.Z;
        
        // Standard Illuminant D65 using standard 2° observer (https://en.wikipedia.org/wiki/Illuminant_D65) - reference white = Y = 100
        const double xn = 95.047;
        const double yn = 100.0;
        const double zn = 108.883;
            
        // XYZ is 0-1, conversion to LAB expects 0-100
        var xRatio = (x * 100) / xn;
        var yRatio = (y * 100) / yn;
        var zRatio = (z * 100) / zn;

        /*
         * --- delta = 6 / 29
         * --- 0.008856 = delta ^ 3
         * --- t * 7.787037 = t / 0.128419 = t / 3delta^2 [1 / 0.128419 = 7.787037]
         */
        double F(double t) => t > 0.008856 ? Math.Pow(t, 1 / 3.0) : (t * 7.787037) + (4.0 / 29.0);
        var l = 116 * F(yRatio) - 16;
        var a = 500 * (F(xRatio) - F(yRatio));
        var b = 200 * (F(yRatio) - F(zRatio));

        return new Lab(l, a, b);
    }
}