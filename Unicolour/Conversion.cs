namespace Wacton.Unicolour;

using static Wacton.Unicolour.Utils;

internal static class Conversion
{
    // https://en.wikipedia.org/wiki/HSL_and_HSV#From_RGB
    public static Hsb RgbToHsb(Rgb rgb)
    {
        var r = rgb.ConstrainedR;
        var g = rgb.ConstrainedG;
        var b = rgb.ConstrainedB;

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
        var brightness = xMax;
        var saturation = brightness == 0 ? 0 : chroma / brightness;
        return new Hsb(hue.Modulo(360.0), saturation, brightness, false, rgb.IsMonochrome);
    }

    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB
    public static Rgb HsbToRgb(Hsb hsb, Configuration config)
    {
        var hue = hsb.ConstrainedH;
        var saturation = hsb.ConstrainedS;
        var brightness = hsb.ConstrainedB;
        
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
        return new Rgb(red, green, blue, config, hsb.IsMonochrome);
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_HSL
    public static Hsl HsbToHsl(Hsb hsb)
    {
        var hue = hsb.ConstrainedH;
        var hsbSaturation = hsb.ConstrainedS;
        var brightness = hsb.ConstrainedB;
        
        var lightness = brightness * (1 - hsbSaturation / 2);
        var saturation = lightness is > 0.0 and < 1.0
            ? (brightness - lightness) / Math.Min(lightness, 1 - lightness)
            : 0;

        return new Hsl(hue, saturation, lightness, hsb.HasExplicitHue, hsb.IsMonochrome);
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_HSV
    public static Hsb HslToHsb(Hsl hsl)
    {
        var hue = hsl.ConstrainedH;
        var hslSaturation = hsl.ConstrainedS;
        var lightness = hsl.ConstrainedL;
        
        var brightness = lightness + hslSaturation * Math.Min(lightness, 1 - lightness);
        var saturation = brightness > 0.0
            ? 2 * (1 - lightness / brightness)
            : 0;

        return new Hsb(hue, saturation, brightness, hsl.HasExplicitHue, hsl.IsMonochrome);
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

        var transformationMatrix = Matrices.RgbToXyzMatrix(config);
        var xyzMatrix = transformationMatrix.Multiply(rgbLinearMatrix);

        var x = xyzMatrix[0, 0];
        var y = xyzMatrix[1, 0];
        var z = xyzMatrix[2, 0];

        return new Xyz(x, y, z, rgb.IsMonochrome);
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

        var transformationMatrix = Matrices.RgbToXyzMatrix(config).Inverse();
        var rgbLinearMatrix = transformationMatrix.Multiply(xyzMatrix);

        var rLinear = rgbLinearMatrix[0, 0];
        var gLinear = rgbLinearMatrix[1, 0];
        var bLinear = rgbLinearMatrix[2, 0];

        var r = config.Compand(rLinear);
        var g = config.Compand(gLinear);
        var b = config.Compand(bLinear);

        return new Rgb(r, g, b, config, xyz.ConvertedFromMonochrome);
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
        double F(double t) => t > Math.Pow(delta, 3) ? CubeRoot(t) : t * (1 / 3.0) * Math.Pow(delta, -2) + 4.0 / 29.0;
        var l = 116 * F(yRatio) - 16;
        var a = 500 * (F(xRatio) - F(yRatio));
        var b = 200 * (F(yRatio) - F(zRatio));

        return new Lab(l, a, b, xyz.ConvertedFromMonochrome);
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

        return new Xyz(x, y, z, lab.IsMonochrome);
    }
    
    public static Lchab LabToLchab(Lab lab)
    {
        var (l, c, h) = ToLchTriplet(lab.L, lab.A, lab.B);
        return new Lchab(l, c, h, false, lab.IsMonochrome);
    }
    
    public static Lab LchabToLab(Lchab lchab)
    {
        var (l, a, b) = FromLchTriplet(lchab.ConstrainedTriplet);
        return new Lab(l, a, b, lchab.IsMonochrome);
    }
    
    // https://en.wikipedia.org/wiki/CIELUV#The_forward_transformation
    public static Luv XyzToLuv(Xyz xyz, Configuration config)
    {
        double U(double x, double y, double z) => 4 * x / (x + 15 * y + 3 * z);
        double V(double x, double y, double z) => 9 * y / (x + 15 * y + 3 * z);
            
        var (xRef, yRef, zRef) = config.XyzWhitePoint;
        var uPrime = U(xyz.X * 100, xyz.Y * 100, xyz.Z * 100);
        var uPrimeRef = U(xRef, yRef, zRef);
        var vPrime = V(xyz.X * 100, xyz.Y * 100, xyz.Z * 100);
        var vPrimeRef = V(xRef, yRef, zRef);
        
        var yRatio = xyz.Y * 100 / yRef;
        var l = yRatio > Math.Pow(6.0 / 29.0, 3) ? 116 * CubeRoot(yRatio) - 16 : Math.Pow(29 / 3.0, 3) * yRatio;
        var u = 13 * l * (uPrime - uPrimeRef);
        var v = 13 * l * (vPrime - vPrimeRef);
        
        double ZeroNaN(double value) => double.IsNaN(value) ? 0.0 : value;
        return new Luv(ZeroNaN(l), ZeroNaN(u), ZeroNaN(v), xyz.ConvertedFromMonochrome);
    }
    
    // https://en.wikipedia.org/wiki/CIELUV#The_reverse_transformation
    public static Xyz LuvToXyz(Luv luv, Configuration config)
    {
        var l = luv.L;
        var u = luv.U;
        var v = luv.V;
        
        double U(double x, double y, double z) => 4 * x / (x + 15 * y + 3 * z);
        double V(double x, double y, double z) => 9 * y / (x + 15 * y + 3 * z);

        var (xRef, yRef, zRef) = config.XyzWhitePoint;
        var uPrimeRef = U(xRef, yRef, zRef);
        var uPrime = u / (13 * l) + uPrimeRef;
        var vPrimeRef = V(xRef, yRef, zRef);
        var vPrime = v / (13 * l) + vPrimeRef;

        var y = (l > 8 ? yRef * Math.Pow((l + 16) / 116.0, 3) : yRef * l * Math.Pow(3 / 29.0, 3)) / 100.0;
        var x = y * ((9 * uPrime) / (4 * vPrime));
        var z = y * ((12 - 3 * uPrime - 20 * vPrime) / (4 * vPrime));
        
        double ZeroNaN(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0.0 : value;
        return new Xyz(ZeroNaN(x), ZeroNaN(y), ZeroNaN(z), luv.IsMonochrome);
    }

    public static Lchuv LuvToLchuv(Luv luv)
    {
        var (l, c, h) = ToLchTriplet(luv.L, luv.U, luv.V);
        return new Lchuv(l, c, h, false, luv.IsMonochrome);
    }
    
    public static Luv LchuvToLuv(Lchuv lchuv)
    {
        var (l, u, v) = FromLchTriplet(lchuv.ConstrainedTriplet);
        return new Luv(l, u, v, lchuv.IsMonochrome);
    }

    // https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab
    public static Oklab XyzToOklab(Xyz xyz, Configuration config)
    {
        var xyzMatrix = new Matrix(new[,]
        {
            {xyz.X},
            {xyz.Y},
            {xyz.Z}
        });

        var adaptedXyz = Matrices.AdaptForWhitePoint(xyzMatrix, config.XyzWhitePoint, WhitePoint.From(Illuminant.D65));
        var lmsMatrix = Matrices.OklabM1.Multiply(adaptedXyz);
        var lmsNonLinearMatrix = new Matrix(new[,]
        {
            {CubeRoot(lmsMatrix[0, 0])},
            {CubeRoot(lmsMatrix[1, 0])},
            {CubeRoot(lmsMatrix[2, 0])}
        });
        
        var labMatrix = Matrices.OklabM2.Multiply(lmsNonLinearMatrix);
        
        var l = labMatrix[0, 0];
        var a = labMatrix[1, 0];
        var b = labMatrix[2, 0];
        return new Oklab(l, a, b, xyz.ConvertedFromMonochrome);
    }
    
    // https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab
    public static Xyz OklabToXyz(Oklab oklab, Configuration config)
    {
        var labMatrix = new Matrix(new[,]
        {
            {oklab.L},
            {oklab.A},
            {oklab.B}
        });

        var lmsNonLinearMatrix = Matrices.OklabM2.Inverse().Multiply(labMatrix);
        var lmsMatrix = new Matrix(new[,]
        {
            {Math.Pow(lmsNonLinearMatrix[0, 0], 3)},
            {Math.Pow(lmsNonLinearMatrix[1, 0], 3)},
            {Math.Pow(lmsNonLinearMatrix[2, 0], 3)}
        });

        var xyzMatrix = Matrices.OklabM1.Inverse().Multiply(lmsMatrix);
        var adaptedXyz = Matrices.AdaptForWhitePoint(xyzMatrix, WhitePoint.From(Illuminant.D65), config.XyzWhitePoint);
        
        var x = adaptedXyz[0, 0];
        var y = adaptedXyz[1, 0];
        var z = adaptedXyz[2, 0];
        return new Xyz(x, y, z, oklab.IsMonochrome);
    }
    
    public static Oklch OklabToOklch(Oklab oklab)
    {
        var (l, c, h) = ToLchTriplet(oklab.L, oklab.A, oklab.B);
        return new Oklch(l, c, h, false, oklab.IsMonochrome);
    }
    
    public static Oklab OklchToOklab(Oklch oklch)
    {
        var (l, a, b) = FromLchTriplet(oklch.ConstrainedTriplet);
        return new Oklab(l, a, b, oklch.IsMonochrome);
    }
    
    private static ColourTriplet ToLchTriplet(double lightness, double axis1, double axis2)
    {
        var chroma = Math.Sqrt(Math.Pow(axis1, 2) + Math.Pow(axis2, 2));
        var hue = ToDegrees(Math.Atan2(axis2, axis1));
        return new(lightness, chroma, hue.Modulo(360.0));
    }
    
    private static ColourTriplet FromLchTriplet(ColourTriplet lchTriplet)
    {
        var (l, c, h) = lchTriplet;
        var axis1 = c * Math.Cos(ToRadians(h));
        var axis2 = c * Math.Sin(ToRadians(h));
        return new ColourTriplet(l, axis1, axis2);
    }

    private static double ToDegrees(double radians) => radians * (180.0 / Math.PI);
    private static double ToRadians(double degrees) => degrees * (Math.PI / 180.0);
}