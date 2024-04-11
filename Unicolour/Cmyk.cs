namespace Wacton.Unicolour;

/*
 * ⚠️ this is the naive CMYK conversion ⚠️
 * CMYK is a device-dependent colour space based on ICC profiles
 * but this conversion does not take that into account
 * and is unlikely to be integrated into Unicolour until ICC profiles are supported (if ever)
 */

internal static class Cmyk
{
    internal static double[] FromUnicolour(Unicolour unicolour) => FromRgb(unicolour.Rgb);
    internal static double[] FromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb.ConstrainedTriplet.Tuple;
        var black = 1 - new[] { r, g, b }.Max();
        var c = black >= 1.0 ? 0 : (1 - r - black) / (1 - black);
        var m = black >= 1.0 ? 0 : (1 - g - black) / (1 - black);
        var y = black >= 1.0 ? 0 : (1 - b - black) / (1 - black);
        return new[] { c, m, y, black };
    }
    
    internal static Unicolour ToUnicolour(double[] cmyk) => new(ColourSpace.Rgb, ToRgb(cmyk).Triplet.Tuple);
    internal static Rgb ToRgb(double[] cmyk)
    {
        var (c, m, y, black) = (cmyk[0], cmyk[1], cmyk[2], cmyk[3]);
        var r = 1 - Math.Min(1, c * (1 - black) + black);
        var g = 1 - Math.Min(1, m * (1 - black) + black);
        var b = 1 - Math.Min(1, y * (1 - black) + black);
        return new Rgb(r, g, b);
    }
}

/*
 * ⚠️ this is even less useful than the naive CMYK conversion ⚠️
 * included for the sake of completeness
 * but is likely to be removed if calibrated CMYK conversion with ICC profiles is ever supported
 */

internal static class Cmy
{
    internal static double[] FromUnicolour(Unicolour unicolour) => FromRgb(unicolour.Rgb);
    internal static double[] FromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb.ConstrainedTriplet.Tuple;
        var c = 1 - r;
        var m = 1 - g;
        var y = 1 - b;
        return new[] { c, m, y };
    }

    internal static Unicolour ToUnicolour(double[] cmy) => new(ColourSpace.Rgb, ToRgb(cmy).Triplet.Tuple);
    internal static Rgb ToRgb(double[] cmy)
    {
        var (c, m, y) = (cmy[0], cmy[1], cmy[2]);
        var r = 1 - c;
        var g = 1 - m;
        var b = 1 - y;
        return new Rgb(r, g, b);
    }
}