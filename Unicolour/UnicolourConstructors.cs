namespace Wacton.Unicolour;

public partial class Unicolour
{
    public static Unicolour FromHex(string hex) => FromHex(Configuration.Default, hex);
    public static Unicolour FromHex(Configuration config, string hex)
    {
        var (r255, g255, b255, a255) = Utils.ParseColourHex(hex);
        return FromRgb255(config, r255, g255, b255, a255);
    }

    public static Unicolour FromRgb255((int r255, int g255, int b255) tuple, int alpha255 = 255) => FromRgb255(Configuration.Default, tuple.r255, tuple.g255, tuple.b255, alpha255);
    public static Unicolour FromRgb255(Configuration config, (int r255, int g255, int b255) tuple, int alpha255 = 255) => FromRgb255(config, tuple.r255, tuple.g255, tuple.b255, alpha255);
    public static Unicolour FromRgb255(int r255, int g255, int b255, int alpha255 = 255) => FromRgb255(Configuration.Default, r255, g255, b255, alpha255);
    public static Unicolour FromRgb255(Configuration config, int r255, int g255, int b255, int alpha255 = 255) => FromRgb(config, r255/255.0, g255/255.0, b255/255.0, alpha255/255.0);
    
    public static Unicolour FromRgb((double r, double g, double b) tuple, double alpha = 1.0) => FromRgb(Configuration.Default, tuple.r, tuple.g, tuple.b, alpha);
    public static Unicolour FromRgb(Configuration config, (double r, double g, double b) tuple, double alpha = 1.0) => FromRgb(config, tuple.r, tuple.g, tuple.b, alpha);
    public static Unicolour FromRgb(double r, double g, double b, double alpha = 1.0) => FromRgb(Configuration.Default, r, g, b, alpha);
    public static Unicolour FromRgb(Configuration config, double r, double g, double b, double alpha = 1.0) => new(config, new Rgb(r, g, b, config), new Alpha(alpha));
    internal static Unicolour FromRgb(Configuration config, ColourMode colourMode, double r, double g, double b, double alpha = 1.0) => new(config, new Rgb(r, g, b, config, colourMode), new Alpha(alpha));
    
    public static Unicolour FromHsb((double h, double s, double b) tuple, double alpha = 1.0) => FromHsb(Configuration.Default, tuple.h, tuple.s, tuple.b, alpha);
    public static Unicolour FromHsb(Configuration config, (double h, double s, double b) tuple, double alpha = 1.0) => FromHsb(config, tuple.h, tuple.s, tuple.b, alpha);
    public static Unicolour FromHsb(double h, double s, double b, double alpha = 1.0) => FromHsb(Configuration.Default, h, s, b, alpha);
    public static Unicolour FromHsb(Configuration config, double h, double s, double b, double alpha = 1.0) => new(config, new Hsb(h, s, b), new Alpha(alpha));
    internal static Unicolour FromHsb(Configuration config, ColourMode colourMode, double h, double s, double b, double alpha = 1.0) => new(config, new Hsb(h, s, b, colourMode), new Alpha(alpha));

    public static Unicolour FromHsl((double h, double s, double l) tuple, double alpha = 1.0) => FromHsl(Configuration.Default, tuple.h, tuple.s, tuple.l, alpha);
    public static Unicolour FromHsl(Configuration config, (double h, double s, double l) tuple, double alpha = 1.0) => FromHsl(config, tuple.h, tuple.s, tuple.l, alpha);
    public static Unicolour FromHsl(double h, double s, double l, double alpha = 1.0) => FromHsl(Configuration.Default, h, s, l, alpha);
    public static Unicolour FromHsl(Configuration config, double h, double s, double l, double alpha = 1.0) => new(config, new Hsl(h, s, l), new Alpha(alpha));
    internal static Unicolour FromHsl(Configuration config, ColourMode colourMode, double h, double s, double l, double alpha = 1.0) => new(config, new Hsl(h, s, l, colourMode), new Alpha(alpha));

    public static Unicolour FromXyz((double x, double y, double z) tuple, double alpha = 1.0) => FromXyz(Configuration.Default, tuple.x, tuple.y, tuple.z, alpha);
    public static Unicolour FromXyz(Configuration config, (double x, double y, double z) tuple, double alpha = 1.0) => FromXyz(config, tuple.x, tuple.y, tuple.z, alpha);
    public static Unicolour FromXyz(double x, double y, double z, double alpha = 1.0) => FromXyz(Configuration.Default, x, y, z, alpha);
    public static Unicolour FromXyz(Configuration config, double x, double y, double z, double alpha = 1.0) => new(config, new Xyz(x, y, z), new Alpha(alpha));
    internal static Unicolour FromXyz(Configuration config, ColourMode colourMode, double x, double y, double z, double alpha = 1.0) => new(config, new Xyz(x, y, z, colourMode), new Alpha(alpha));

    public static Unicolour FromLab((double l, double a, double b) tuple, double alpha = 1.0) => FromLab(Configuration.Default, tuple.l, tuple.a, tuple.b, alpha);
    public static Unicolour FromLab(Configuration config, (double l, double a, double b) tuple, double alpha = 1.0) => FromLab(config, tuple.l, tuple.a, tuple.b, alpha);
    public static Unicolour FromLab(double l, double a, double b, double alpha = 1.0) => FromLab(Configuration.Default, l, a, b, alpha);
    public static Unicolour FromLab(Configuration config, double l, double a, double b, double alpha = 1.0) => new(config, new Lab(l, a, b), new Alpha(alpha));
    internal static Unicolour FromLab(Configuration config, ColourMode colourMode, double l, double a, double b, double alpha = 1.0) => new(config, new Lab(l, a, b, colourMode), new Alpha(alpha));

    public static Unicolour FromLchab((double l, double c, double h) tuple, double alpha = 1.0) => FromLchab(Configuration.Default, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromLchab(Configuration config, (double l, double c, double h) tuple, double alpha = 1.0) => FromLchab(config, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromLchab(double l, double c, double h, double alpha = 1.0) => FromLchab(Configuration.Default, l, c, h, alpha);
    public static Unicolour FromLchab(Configuration config, double l, double c, double h, double alpha = 1.0) => new(config, new Lchab(l, c, h), new Alpha(alpha));
    internal static Unicolour FromLchab(Configuration config, ColourMode colourMode, double l, double c, double h, double alpha = 1.0) => new(config, new Lchab(l, c, h, colourMode), new Alpha(alpha));

    public static Unicolour FromLuv((double l, double u, double v) tuple, double alpha = 1.0) => FromLuv(Configuration.Default, tuple.l, tuple.u, tuple.v, alpha);
    public static Unicolour FromLuv(Configuration config, (double l, double u, double v) tuple, double alpha = 1.0) => FromLuv(config, tuple.l, tuple.u, tuple.v, alpha);
    public static Unicolour FromLuv(double l, double u, double v, double alpha = 1.0) => FromLuv(Configuration.Default, l, u, v, alpha);
    public static Unicolour FromLuv(Configuration config, double l, double u, double v, double alpha = 1.0) => new(config, new Luv(l, u, v), new Alpha(alpha));
    internal static Unicolour FromLuv(Configuration config, ColourMode colourMode, double l, double u, double v, double alpha = 1.0) => new(config, new Luv(l, u, v, colourMode), new Alpha(alpha));

    public static Unicolour FromLchuv((double l, double c, double h) tuple, double alpha = 1.0) => FromLchuv(Configuration.Default, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromLchuv(Configuration config, (double l, double c, double h) tuple, double alpha = 1.0) => FromLchuv(config, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromLchuv(double l, double c, double h, double alpha = 1.0) => FromLchuv(Configuration.Default, l, c, h, alpha);
    public static Unicolour FromLchuv(Configuration config, double l, double c, double h, double alpha = 1.0) => new(config, new Lchuv(l, c, h), new Alpha(alpha));
    internal static Unicolour FromLchuv(Configuration config, ColourMode colourMode, double l, double c, double h, double alpha = 1.0) => new(config, new Lchuv(l, c, h, colourMode), new Alpha(alpha));

    public static Unicolour FromHsluv((double h, double s, double l) tuple, double alpha = 1.0) => FromHsluv(Configuration.Default, tuple.h, tuple.s, tuple.l, alpha);
    public static Unicolour FromHsluv(Configuration config, (double h, double s, double l) tuple, double alpha = 1.0) => FromHsluv(config, tuple.h, tuple.s, tuple.l, alpha);
    public static Unicolour FromHsluv(double h, double s, double l, double alpha = 1.0) => FromHsluv(Configuration.Default, h, s, l, alpha);
    public static Unicolour FromHsluv(Configuration config, double h, double s, double l, double alpha = 1.0) => new(config, new Hsluv(h, s, l), new Alpha(alpha));
    internal static Unicolour FromHsluv(Configuration config, ColourMode colourMode, double h, double s, double l, double alpha = 1.0) => new(config, new Hsluv(h, s, l, colourMode), new Alpha(alpha));
    
    public static Unicolour FromHpluv((double h, double s, double l) tuple, double alpha = 1.0) => FromHpluv(Configuration.Default, tuple.h, tuple.s, tuple.l, alpha);
    public static Unicolour FromHpluv(Configuration config, (double h, double s, double l) tuple, double alpha = 1.0) => FromHpluv(config, tuple.h, tuple.s, tuple.l, alpha);
    public static Unicolour FromHpluv(double h, double s, double l, double alpha = 1.0) => FromHpluv(Configuration.Default, h, s, l, alpha);
    public static Unicolour FromHpluv(Configuration config, double h, double s, double l, double alpha = 1.0) => new(config, new Hpluv(h, s, l), new Alpha(alpha));
    internal static Unicolour FromHpluv(Configuration config, ColourMode colourMode, double h, double s, double l, double alpha = 1.0) => new(config, new Hpluv(h, s, l, colourMode), new Alpha(alpha));

    public static Unicolour FromJzazbz((double jz, double az, double bz) tuple, double alpha = 1.0) => FromJzazbz(Configuration.Default, tuple.jz, tuple.az, tuple.bz, alpha);
    public static Unicolour FromJzazbz(Configuration config, (double jz, double az, double bz) tuple, double alpha = 1.0) => FromJzazbz(config, tuple.jz, tuple.az, tuple.bz, alpha);
    public static Unicolour FromJzazbz(double jz, double az, double bz, double alpha = 1.0) => FromJzazbz(Configuration.Default, jz, az, bz, alpha);
    public static Unicolour FromJzazbz(Configuration config, double jz, double az, double bz, double alpha = 1.0) => new(config, new Jzazbz(jz, az, bz), new Alpha(alpha));
    internal static Unicolour FromJzazbz(Configuration config, ColourMode colourMode, double jz, double az, double bz, double alpha = 1.0) => new(config, new Jzazbz(jz, az, bz, colourMode), new Alpha(alpha));

    public static Unicolour FromJzczhz((double jz, double cz, double hz) tuple, double alpha = 1.0) => FromJzczhz(Configuration.Default, tuple.jz, tuple.cz, tuple.hz, alpha);
    public static Unicolour FromJzczhz(Configuration config, (double jz, double cz, double hz) tuple, double alpha = 1.0) => FromJzczhz(config, tuple.jz, tuple.cz, tuple.hz, alpha);
    public static Unicolour FromJzczhz(double jz, double cz, double hz, double alpha = 1.0) => FromJzczhz(Configuration.Default, jz, cz, hz, alpha);
    public static Unicolour FromJzczhz(Configuration config, double jz, double cz, double hz, double alpha = 1.0) => new(config, new Jzczhz(jz, cz, hz), new Alpha(alpha));
    internal static Unicolour FromJzczhz(Configuration config, ColourMode colourMode, double jz, double cz, double hz, double alpha = 1.0) => new(config, new Jzczhz(jz, cz, hz, colourMode), new Alpha(alpha));

    public static Unicolour FromOklab((double l, double a, double b) tuple, double alpha = 1.0) => FromOklab(Configuration.Default, tuple.l, tuple.a, tuple.b, alpha);
    public static Unicolour FromOklab(Configuration config, (double l, double a, double b) tuple, double alpha = 1.0) => FromOklab(config, tuple.l, tuple.a, tuple.b, alpha);
    public static Unicolour FromOklab(double l, double a, double b, double alpha = 1.0) => FromOklab(Configuration.Default, l, a, b, alpha);
    public static Unicolour FromOklab(Configuration config, double l, double a, double b, double alpha = 1.0) => new(config, new Oklab(l, a, b), new Alpha(alpha));
    internal static Unicolour FromOklab(Configuration config, ColourMode colourMode, double l, double a, double b, double alpha = 1.0) => new(config, new Oklab(l, a, b, colourMode), new Alpha(alpha));

    public static Unicolour FromOklch((double l, double c, double h) tuple, double alpha = 1.0) => FromOklch(Configuration.Default, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromOklch(Configuration config, (double l, double c, double h) tuple, double alpha = 1.0) => FromOklch(config, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromOklch(double l, double c, double h, double alpha = 1.0) => FromOklch(Configuration.Default, l, c, h, alpha);
    public static Unicolour FromOklch(Configuration config, double l, double c, double h, double alpha = 1.0) => new(config, new Oklch(l, c, h), new Alpha(alpha));
    internal static Unicolour FromOklch(Configuration config, ColourMode colourMode, double l, double c, double h, double alpha = 1.0) => new(config, new Oklch(l, c, h, colourMode), new Alpha(alpha));
}