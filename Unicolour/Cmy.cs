namespace Wacton.Unicolour;

/*
 * ⚠️ this is even less useful than the naive CMYK conversion ⚠️
 * included for the sake of completeness
 * and will not be integrated into Unicolour
 */

internal static class Cmy
{
    internal static double[] FromUnicolour(Unicolour colour) => FromRgb(colour.Rgb);
    internal static double[] FromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb.ConstrainedTuple;
        var c = 1 - r;
        var m = 1 - g;
        var y = 1 - b;
        return new[] { c, m, y };
    }

    internal static Unicolour ToUnicolour(double[] cmy) => new(ColourSpace.Rgb, ToRgb(cmy).Tuple);
    internal static Rgb ToRgb(double[] cmy)
    {
        var (c, m, y) = (cmy[0], cmy[1], cmy[2]);
        var r = 1 - c;
        var g = 1 - m;
        var b = 1 - y;
        return new Rgb(r, g, b);
    }
}