namespace Wacton.Unicolour;

using static Utils;

internal static class Conversion
{
    // https://en.wikipedia.org/wiki/HSL_and_HSV#From_RGB
    public static Hsb RgbToHsb(Rgb rgb)
    {
        var (r, g, b) = rgb.ConstrainedTriplet;
        var components = new[] {r, g, b};
        var xMax = components.Max();
        var xMin = components.Min();
        var chroma = xMax - xMin;

        double hue;
        if (chroma == 0.0) hue = 0;
        else if (xMax == r) hue = 60 * (0 + (g - b) / chroma);
        else if (xMax == g) hue = 60 * (2 + (b - r) / chroma);
        else if (xMax == b) hue = 60 * (4 + (r - g) / chroma);
        else hue = double.NaN;
        var brightness = xMax;
        var saturation = brightness == 0 ? 0 : chroma / brightness;
        return new Hsb(hue.Modulo(360.0), saturation, brightness, ColourMode.FromRepresentation(rgb));
    }

    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB
    public static Rgb HsbToRgb(Hsb hsb, Configuration config)
    {
        var (hue, saturation, brightness) = hsb.ConstrainedTriplet;
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
        return new Rgb(red, green, blue, config, ColourMode.FromRepresentation(hsb));
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_HSL
    public static Hsl HsbToHsl(Hsb hsb)
    {
        var (hue, hsbSaturation, brightness) = hsb.ConstrainedTriplet;
        var lightness = brightness * (1 - hsbSaturation / 2);
        var saturation = lightness is > 0.0 and < 1.0
            ? (brightness - lightness) / Math.Min(lightness, 1 - lightness)
            : 0;

        return new Hsl(hue, saturation, lightness, ColourMode.FromRepresentation(hsb));
    }
    
    // https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_HSV
    public static Hsb HslToHsb(Hsl hsl)
    {
        var (hue, hslSaturation, lightness) = hsl.ConstrainedTriplet;
        var brightness = lightness + hslSaturation * Math.Min(lightness, 1 - lightness);
        var saturation = brightness > 0.0
            ? 2 * (1 - lightness / brightness)
            : 0;

        return new Hsb(hue, saturation, brightness, ColourMode.FromRepresentation(hsl));
    }
    
    // https://en.wikipedia.org/wiki/HWB_color_model#Conversion
    public static Hwb HsbToHwb(Hsb hsb)
    {
        var (hue, s, b) = hsb.ConstrainedTriplet;
        var whiteness = (1 - s) * b;
        var blackness = 1 - b;
        return new Hwb(hue, whiteness, blackness, ColourMode.FromRepresentation(hsb));
    }
    
    // https://en.wikipedia.org/wiki/HWB_color_model#Conversion
    public static Hsb HwbToHsb(Hwb hwb)
    {
        var (hue, w, b) = hwb.ConstrainedTriplet;

        double brightness;
        double saturation;
        if (hwb.IsGreyscale)
        {
            brightness = w / (w + b);
            saturation = 0;
        }
        else
        {
            brightness = 1 - b;
            saturation = brightness == 0.0 ? 0 : 1 - w / brightness;
        }
        
        return new Hsb(hue, saturation, brightness, ColourMode.FromRepresentation(hwb));
    }

    // https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
    public static Xyz RgbToXyz(Rgb rgb, Configuration config)
    {
        var rgbLinearMatrix = Matrix.FromTriplet(rgb.Linear.Triplet);
        var transformationMatrix = Matrices.RgbToXyzMatrix(config);
        var xyzMatrix = transformationMatrix.Multiply(rgbLinearMatrix);
        return new Xyz(xyzMatrix.ToTriplet(), ColourMode.FromRepresentation(rgb));
    }
    
    // https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
    public static Rgb XyzToRgb(Xyz xyz, Configuration config)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var transformationMatrix = Matrices.RgbToXyzMatrix(config).Inverse();
        var rgbLinearMatrix = transformationMatrix.Multiply(xyzMatrix);
        var rgbMatrix = rgbLinearMatrix.Select(config.Compand);
        return new Rgb(rgbMatrix.ToTriplet(), config, ColourMode.FromRepresentation(xyz));
    }
    
    // https://en.wikipedia.org/wiki/CIE_1931_color_space#CIE_xy_chromaticity_diagram_and_the_CIE_xyY_color_space
    public static Xyy XyzToXyy(Xyz xyz, Configuration config)
    {
        var (x, y, z) = xyz.Triplet;
        var normalisation = x + y + z;
        var isBlack = normalisation == 0.0;
        
        var chromaticityX = isBlack ? config.ChromaticityWhite.X : x / normalisation;
        var chromaticityY = isBlack ? config.ChromaticityWhite.Y : y / normalisation;
        var luminance = isBlack ? 0 : y;
        return new Xyy(chromaticityX, chromaticityY, luminance, ColourMode.FromRepresentation(xyz));
    }
    
    // https://en.wikipedia.org/wiki/CIE_1931_color_space#CIE_xy_chromaticity_diagram_and_the_CIE_xyY_color_space
    public static Xyz XyyToXyz(Xyy xyy)
    {
        var chromaticity = xyy.ConstrainedChromaticity;
        var luminance = xyy.ConstrainedLuminance;

        var useZero = chromaticity.Y <= 0;
        var factor = luminance / chromaticity.Y;
        var x = useZero ? 0 : factor * chromaticity.X;
        var y = useZero ? 0 : luminance;
        var z = useZero ? 0 : factor * (1 - chromaticity.X - chromaticity.Y);
        return new Xyz(x, y, z, ColourMode.FromRepresentation(xyy));
    }
    
    // https://en.wikipedia.org/wiki/CIELAB_color_space#From_CIEXYZ_to_CIELAB
    public static Lab XyzToLab(Xyz xyz, Configuration config)
    {
        var (x, y, z) = xyz.Triplet;
        var referenceWhite = config.XyzWhitePoint;
        var xRatio = x * 100 / referenceWhite.X;
        var yRatio = y * 100 / referenceWhite.Y;
        var zRatio = z * 100 / referenceWhite.Z;
        
        var delta = 6.0 / 29.0;
        double F(double t) => t > Math.Pow(delta, 3) ? CubeRoot(t) : t * (1 / 3.0) * Math.Pow(delta, -2) + 4.0 / 29.0;
        var l = 116 * F(yRatio) - 16;
        var a = 500 * (F(xRatio) - F(yRatio));
        var b = 200 * (F(yRatio) - F(zRatio));
        return new Lab(l, a, b, ColourMode.FromRepresentation(xyz));
    }

    // https://en.wikipedia.org/wiki/CIELAB_color_space#From_CIELAB_to_CIEXYZ
    public static Xyz LabToXyz(Lab lab, Configuration config)
    {
        var (l, a, b) = lab.Triplet;
        var referenceWhite = config.XyzWhitePoint;
        var delta = 6.0 / 29.0;
        double F(double t) => t > delta ? Math.Pow(t, 3.0) : 3 * Math.Pow(delta, 2) * (t - 4.0 / 29.0);
        var x = referenceWhite.X / 100.0 * F((l + 16) / 116.0 + a / 500.0);
        var y = referenceWhite.Y / 100.0 * F((l + 16) / 116.0);
        var z = referenceWhite.Z / 100.0 * F((l + 16) / 116.0 - b / 200.0);
        return new Xyz(x, y, z, ColourMode.FromRepresentation(lab));
    }
    
    public static Lchab LabToLchab(Lab lab)
    {
        var (l, c, h) = ToChromaHueTriplet(lab.L, lab.A, lab.B);
        return new Lchab(l, c, h, ColourMode.FromRepresentation(lab));
    }
    
    public static Lab LchabToLab(Lchab lchab)
    {
        var (l, a, b) = FromChromaHueTriplet(lchab.ConstrainedTriplet);
        return new Lab(l, a, b, ColourMode.FromRepresentation(lchab));
    }
    
    // https://en.wikipedia.org/wiki/CIELUV#The_forward_transformation
    public static Luv XyzToLuv(Xyz xyz, Configuration config)
    {
        var (x, y, z) = xyz.Triplet;
        var (xRef, yRef, zRef) = config.XyzWhitePoint;

        double U(double xu, double yu, double zu) => 4 * xu / (xu + 15 * yu + 3 * zu);
        double V(double xv, double yv, double zv) => 9 * yv / (xv + 15 * yv + 3 * zv);
        var uPrime = U(x * 100, y * 100, z * 100);
        var uPrimeRef = U(xRef, yRef, zRef);
        var vPrime = V(x * 100, y * 100, z * 100);
        var vPrimeRef = V(xRef, yRef, zRef);
        
        var yRatio = y * 100 / yRef;
        var l = yRatio > Math.Pow(6.0 / 29.0, 3) ? 116 * CubeRoot(yRatio) - 16 : Math.Pow(29 / 3.0, 3) * yRatio;
        var u = 13 * l * (uPrime - uPrimeRef);
        var v = 13 * l * (vPrime - vPrimeRef);
        
        double ZeroNaN(double value) => double.IsNaN(value) ? 0.0 : value;
        return new Luv(ZeroNaN(l), ZeroNaN(u), ZeroNaN(v), ColourMode.FromRepresentation(xyz));
    }
    
    // https://en.wikipedia.org/wiki/CIELUV#The_reverse_transformation
    public static Xyz LuvToXyz(Luv luv, Configuration config)
    {
        var (l, u, v) = luv.Triplet;
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
        return new Xyz(ZeroNaN(x), ZeroNaN(y), ZeroNaN(z), ColourMode.FromRepresentation(luv));
    }

    public static Lchuv LuvToLchuv(Luv luv)
    {
        var (l, c, h) = ToChromaHueTriplet(luv.L, luv.U, luv.V);
        return new Lchuv(l, c, h, ColourMode.FromRepresentation(luv));
    }
    
    public static Luv LchuvToLuv(Lchuv lchuv)
    {
        var (l, u, v) = FromChromaHueTriplet(lchuv.ConstrainedTriplet);
        return new Luv(l, u, v, ColourMode.FromRepresentation(lchuv));
    }
    
    // https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L363
    public static Hsluv LchuvToHsluv(Lchuv lchuv)
    {
        var (lchLightness, chroma, hue) = lchuv.ConstrainedTriplet;
        double saturation;
        double lightness;

        switch (lchLightness)
        {
            case > 99.9999999:
                saturation = 0.0;
                lightness = 100.0;
                break;
            case < 0.00000001:
                saturation = 0.0;
                lightness = 0.0;
                break;
            default:
            {
                var maxChroma = Lines.CalculateMaxChroma(lchLightness, hue);
                saturation = chroma / maxChroma * 100;
                lightness = lchLightness;
                break;
            }
        }
        
        return new Hsluv(hue, saturation, lightness, ColourMode.FromRepresentation(lchuv));
    }
    
    // https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L346
    public static Lchuv HsluvToLchuv(Hsluv hsluv)
    {
        var hue = hsluv.ConstrainedH;
        var saturation = hsluv.S;
        var hslLightness = hsluv.L;
        double lightness;
        double chroma;

        switch (hslLightness)
        {
            case > 99.9999999:
                lightness = 100.0;
                chroma = 0.0;
                break;
            case < 0.00000001:
                lightness = 0.0;
                chroma = 0.0;
                break;
            default:
            {
                var maxChroma = Lines.CalculateMaxChroma(hslLightness, hue);
                chroma = maxChroma / 100 * saturation;
                lightness = hslLightness;
                break;
            }
        }
        
        return new Lchuv(lightness, chroma, hue, ColourMode.FromRepresentation(hsluv));
    }
    
    // https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L397
    public static Hpluv LchuvToHpluv(Lchuv lchuv)
    {
        var (lchLightness, chroma, hue) = lchuv.ConstrainedTriplet;
        double saturation;
        double lightness;

        switch (lchLightness)
        {
            case > 99.9999999:
                saturation = 0.0;
                lightness = 100.0;
                break;
            case < 0.00000001:
                saturation = 0.0;
                lightness = 0.0;
                break;
            default:
            {
                var maxChroma = Lines.CalculateMaxChroma(lchLightness);
                saturation = chroma / maxChroma * 100;
                lightness = lchLightness;
                break;
            }
        }
        
        return new Hpluv(hue, saturation, lightness, ColourMode.FromRepresentation(lchuv));
    }
    
    // https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L380
    public static Lchuv HpluvToLchuv(Hpluv hpluv)
    {
        var hue = hpluv.ConstrainedH;
        var saturation = hpluv.S;
        var hslLightness = hpluv.L;
        double lightness;
        double chroma;

        switch (hslLightness)
        {
            case > 99.9999999:
                lightness = 100.0;
                chroma = 0.0;
                break;
            case < 0.00000001:
                lightness = 0.0;
                chroma = 0.0;
                break;
            default:
            {
                var maxChroma = Lines.CalculateMaxChroma(hslLightness);
                chroma = maxChroma / 100 * saturation;
                lightness = hslLightness;
                break;
            }
        }
        
        return new Lchuv(lightness, chroma, hue, ColourMode.FromRepresentation(hpluv));
    }

    // https://professional.dolby.com/siteassets/pdfs/dolby-vision-measuring-perceptual-color-volume-v7.1.pdf
    // currently only PQ, not HLG (https://en.wikipedia.org/wiki/Hybrid_log%E2%80%93gamma)
    public static Ictcp XyzToIctcp(Xyz xyz, Configuration config)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var d65Matrix = Matrices.AdaptForWhitePoint(xyzMatrix, config.XyzWhitePoint, WhitePoint.From(Illuminant.D65));
        var d65ScaledMatrix = d65Matrix.Select(x => x * config.IctcpScalar);
        var lmsMatrix = Matrices.IctcpM1.Multiply(d65ScaledMatrix);
        var lmsPrimeMatrix = lmsMatrix.Select(Pq.Smpte.InverseEotf);
        var ictcpMatrix = Matrices.IctcpM2.Multiply(lmsPrimeMatrix);
        return new Ictcp(ictcpMatrix.ToTriplet(), ColourMode.FromRepresentation(xyz));
    }

    // this is not specified in the Dolby white paper, but unit tests confirm roundtrip conversion
    public static Xyz IctcpToXyz(Ictcp ictcp, Configuration config)
    {
        var ictcpMatrix = Matrix.FromTriplet(ictcp.Triplet);
        var lmsPrimeMatrix = Matrices.IctcpM2.Inverse().Multiply(ictcpMatrix);
        var lmsMatrix = lmsPrimeMatrix.Select(Pq.Smpte.Eotf);
        var d65ScaledMatrix = lmsMatrix.Select(x => x / config.IctcpScalar);
        var d65Matrix = Matrices.IctcpM1.Inverse().Multiply(d65ScaledMatrix);
        var xyzMatrix = Matrices.AdaptForWhitePoint(d65Matrix, WhitePoint.From(Illuminant.D65), config.XyzWhitePoint);
        return new Xyz(xyzMatrix.ToTriplet(), ColourMode.FromRepresentation(ictcp));
    }
    
    // https://opg.optica.org/oe/fulltext.cfm?uri=oe-25-13-15131&id=368272
    // https://opticapublishing.figshare.com/articles/software/JzAzBz_m/5016299
    public static Jzazbz XyzToJzazbz(Xyz xyz, Configuration config)
    {
        var b = 1.15;
        var g = 0.66;
        var d = -0.56;
        var d0 = 1.6295499532821566e-11;

        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var d65Matrix = Matrices.AdaptForWhitePoint(xyzMatrix, config.XyzWhitePoint, WhitePoint.From(Illuminant.D65));
        var d65ScaledMatrix = d65Matrix.Select(x => Math.Max(x * config.JzazbzScalar, 0));
        var x65 = d65ScaledMatrix[0, 0];
        var y65 = d65ScaledMatrix[1, 0];
        var z65 = d65ScaledMatrix[2, 0];
        
        var x65Prime = b * x65 - (b - 1) * z65;
        var y65Prime = g * y65 - (g - 1) * x65;
        var xyz65PrimeMatrix = Matrix.FromTriplet(new(x65Prime, y65Prime, z65));
        var lmsMatrix = Matrices.JzazbzM1.Multiply(xyz65PrimeMatrix);
        var lmsPrimeMatrix = lmsMatrix.Select(Pq.Jzazbz.InverseEotf);
        var izazbzMatrix = Matrices.JzazbzM2.Multiply(lmsPrimeMatrix);
        
        var iz = izazbzMatrix[0, 0];
        var az = izazbzMatrix[1, 0];
        var bz = izazbzMatrix[2, 0];
        var jz = (1 + d) * iz / (1 + d * iz) - d0;
        return new Jzazbz(jz, az, bz, ColourMode.FromRepresentation(xyz));
    }
    
    // https://opg.optica.org/oe/fulltext.cfm?uri=oe-25-13-15131&id=368272
    public static Xyz JzazbzToXyz(Jzazbz jzazbz, Configuration config)
    {
        var b = 1.15;
        var g = 0.66;
        var d = -0.56;
        var d0 = 1.6295499532821566e-11;
        
        var (jz, az, bz) = jzazbz.Triplet;
        var iz = (jz + d0) / (1 + d - d * (jz + d0));
        var izazbzMatrix = Matrix.FromTriplet(new(iz, az, bz));
        var lmsPrimeMatrix = Matrices.JzazbzM2.Inverse().Multiply(izazbzMatrix);
        var lmsMatrix = lmsPrimeMatrix.Select(Pq.Jzazbz.Eotf);
        var xyz65PrimeMatrix = Matrices.JzazbzM1.Inverse().Multiply(lmsMatrix);
        var x65Prime = xyz65PrimeMatrix[0, 0];
        var y65Prime = xyz65PrimeMatrix[1, 0];
        var z65Prime = xyz65PrimeMatrix[2, 0];
        
        var x65 = (x65Prime + (b - 1) * z65Prime) / b;
        var y65 = (y65Prime + (g - 1) * x65) / g;
        var z65 = z65Prime;
        var d65ScaledMatrix = Matrix.FromTriplet(new(x65, y65, z65));
        var d65Matrix = d65ScaledMatrix.Select(x => x / config.JzazbzScalar);
        var xyzMatrix = Matrices.AdaptForWhitePoint(d65Matrix, WhitePoint.From(Illuminant.D65), config.XyzWhitePoint);
        return new Xyz(xyzMatrix.ToTriplet(), ColourMode.FromRepresentation(jzazbz));
    }
    
    public static Jzczhz JzazbzToJzczhz(Jzazbz jzazbz)
    {
        var (jz, cz, hz) = ToChromaHueTriplet(jzazbz.J, jzazbz.A, jzazbz.B);
        return new Jzczhz(jz, cz, hz, ColourMode.FromRepresentation(jzazbz));
    }
    
    public static Jzazbz JzczhzToJzazbz(Jzczhz jzczhz)
    {
        var (jz, az, bz) = FromChromaHueTriplet(jzczhz.ConstrainedTriplet);
        return new Jzazbz(jz, az, bz, ColourMode.FromRepresentation(jzczhz));
    }

    // https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab
    public static Oklab XyzToOklab(Xyz xyz, Configuration config)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var d65Matrix = Matrices.AdaptForWhitePoint(xyzMatrix, config.XyzWhitePoint, WhitePoint.From(Illuminant.D65));
        var lmsMatrix = Matrices.OklabM1.Multiply(d65Matrix);
        var lmsNonLinearMatrix = lmsMatrix.Select(CubeRoot);
        var labMatrix = Matrices.OklabM2.Multiply(lmsNonLinearMatrix);
        return new Oklab(labMatrix.ToTriplet(), ColourMode.FromRepresentation(xyz));
    }
    
    // https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab
    public static Xyz OklabToXyz(Oklab oklab, Configuration config)
    {
        var labMatrix = Matrix.FromTriplet(oklab.Triplet);
        var lmsNonLinearMatrix = Matrices.OklabM2.Inverse().Multiply(labMatrix);
        var lmsMatrix = lmsNonLinearMatrix.Select(x => Math.Pow(x, 3));
        var d65Matrix = Matrices.OklabM1.Inverse().Multiply(lmsMatrix);
        var xyzMatrix = Matrices.AdaptForWhitePoint(d65Matrix, WhitePoint.From(Illuminant.D65), config.XyzWhitePoint);
        return new Xyz(xyzMatrix.ToTriplet(), ColourMode.FromRepresentation(oklab));
    }
    
    public static Oklch OklabToOklch(Oklab oklab)
    {
        var (l, c, h) = ToChromaHueTriplet(oklab.L, oklab.A, oklab.B);
        return new Oklch(l, c, h, ColourMode.FromRepresentation(oklab));
    }
    
    public static Oklab OklchToOklab(Oklch oklch)
    {
        var (l, a, b) = FromChromaHueTriplet(oklch.ConstrainedTriplet);
        return new Oklab(l, a, b, ColourMode.FromRepresentation(oklch));
    }
    
    private static ColourTriplet ToChromaHueTriplet(double lightness, double axis1, double axis2)
    {
        var chroma = Math.Sqrt(Math.Pow(axis1, 2) + Math.Pow(axis2, 2));
        var hue = ToDegrees(Math.Atan2(axis2, axis1));
        return new ColourTriplet(lightness, chroma, hue.Modulo(360.0));
    }
    
    private static ColourTriplet FromChromaHueTriplet(ColourTriplet lchTriplet)
    {
        var (l, c, h) = lchTriplet;
        var axis1 = c * Math.Cos(ToRadians(h));
        var axis2 = c * Math.Sin(ToRadians(h));
        return new ColourTriplet(l, axis1, axis2);
    }
}