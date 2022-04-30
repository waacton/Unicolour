namespace Wacton.Unicolour;

public partial class Unicolour
{
    private Unicolour(Configuration config, Rgb rgb, Alpha alpha) : this(config, alpha, ColourSpace.Rgb) => this.rgb = rgb;
    private Unicolour(Configuration config, Hsb hsb, Alpha alpha) : this(config, alpha, ColourSpace.Hsb) => this.hsb = hsb;
    private Unicolour(Configuration config, Hsl hsl, Alpha alpha) : this(config, alpha, ColourSpace.Hsl) => this.hsl = hsl;
    private Unicolour(Configuration config, Xyz xyz, Alpha alpha) : this(config, alpha, ColourSpace.Xyz) => this.xyz = xyz;
    private Unicolour(Configuration config, Lab lab, Alpha alpha) : this(config, alpha, ColourSpace.Lab) => this.lab = lab;
    private Unicolour(Configuration config, Lchab lchab, Alpha alpha) : this(config, alpha, ColourSpace.Lchab) => this.lchab = lchab;
    private Unicolour(Configuration config, Luv luv, Alpha alpha) : this(config, alpha, ColourSpace.Luv) => this.luv = luv;
    private Unicolour(Configuration config, Lchuv lchuv, Alpha alpha) : this(config, alpha, ColourSpace.Lchuv) => this.lchuv = lchuv;
    private Unicolour(Configuration config, Oklab oklab, Alpha alpha) : this(config, alpha, ColourSpace.Oklab) => this.oklab = oklab;
    private Unicolour(Configuration config, Oklch oklch, Alpha alpha) : this(config, alpha, ColourSpace.Oklch) => this.oklch = oklch;
    
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
    
    public static Unicolour FromHsb((double h, double s, double b) tuple, double alpha = 1.0) => FromHsb(Configuration.Default, tuple.h, tuple.s, tuple.b, alpha);
    public static Unicolour FromHsb(Configuration config, (double h, double s, double b) tuple, double alpha = 1.0) => FromHsb(config, tuple.h, tuple.s, tuple.b, alpha);
    public static Unicolour FromHsb(double h, double s, double b, double alpha = 1.0) => FromHsb(Configuration.Default, h, s, b, alpha);
    public static Unicolour FromHsb(Configuration config, double h, double s, double b, double alpha = 1.0) => new(config, new Hsb(h, s, b), new Alpha(alpha));
    
    public static Unicolour FromHsl((double h, double s, double l) tuple, double alpha = 1.0) => FromHsl(Configuration.Default, tuple.h, tuple.s, tuple.l, alpha);
    public static Unicolour FromHsl(Configuration config, (double h, double s, double l) tuple, double alpha = 1.0) => FromHsl(config, tuple.h, tuple.s, tuple.l, alpha);
    public static Unicolour FromHsl(double h, double s, double l, double alpha = 1.0) => FromHsl(Configuration.Default, h, s, l, alpha);
    public static Unicolour FromHsl(Configuration config, double h, double s, double l, double alpha = 1.0) => new(config, new Hsl(h, s, l), new Alpha(alpha));
    
    public static Unicolour FromXyz((double x, double y, double z) tuple, double alpha = 1.0) => FromXyz(Configuration.Default, tuple.x, tuple.y, tuple.z, alpha);
    public static Unicolour FromXyz(Configuration config, (double x, double y, double z) tuple, double alpha = 1.0) => FromXyz(config, tuple.x, tuple.y, tuple.z, alpha);
    public static Unicolour FromXyz(double x, double y, double z, double alpha = 1.0) => FromXyz(Configuration.Default, x, y, z, alpha);
    public static Unicolour FromXyz(Configuration config, double x, double y, double z, double alpha = 1.0) => new(config, new Xyz(x, y, z), new Alpha(alpha));
    
    public static Unicolour FromLab((double l, double a, double b) tuple, double alpha = 1.0) => FromLab(Configuration.Default, tuple.l, tuple.a, tuple.b, alpha);
    public static Unicolour FromLab(Configuration config, (double l, double a, double b) tuple, double alpha = 1.0) => FromLab(config, tuple.l, tuple.a, tuple.b, alpha);
    public static Unicolour FromLab(double l, double a, double b, double alpha = 1.0) => FromLab(Configuration.Default, l, a, b, alpha);
    public static Unicolour FromLab(Configuration config, double l, double a, double b, double alpha = 1.0) => new(config, new Lab(l, a, b), new Alpha(alpha));
    
    public static Unicolour FromLchab((double l, double c, double h) tuple, double alpha = 1.0) => FromLchab(Configuration.Default, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromLchab(Configuration config, (double l, double c, double h) tuple, double alpha = 1.0) => FromLchab(config, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromLchab(double l, double c, double h, double alpha = 1.0) => FromLchab(Configuration.Default, l, c, h, alpha);
    public static Unicolour FromLchab(Configuration config, double l, double c, double h, double alpha = 1.0) => new(config, new Lchab(l, c, h), new Alpha(alpha));
    
    public static Unicolour FromLuv((double l, double u, double v) tuple, double alpha = 1.0) => FromLuv(Configuration.Default, tuple.l, tuple.u, tuple.v, alpha);
    public static Unicolour FromLuv(Configuration config, (double l, double u, double v) tuple, double alpha = 1.0) => FromLuv(config, tuple.l, tuple.u, tuple.v, alpha);
    public static Unicolour FromLuv(double l, double u, double v, double alpha = 1.0) => FromLuv(Configuration.Default, l, u, v, alpha);
    public static Unicolour FromLuv(Configuration config, double l, double u, double v, double alpha = 1.0) => new(config, new Luv(l, u, v), new Alpha(alpha));
    
    public static Unicolour FromLchuv((double l, double c, double h) tuple, double alpha = 1.0) => FromLchuv(Configuration.Default, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromLchuv(Configuration config, (double l, double c, double h) tuple, double alpha = 1.0) => FromLchuv(config, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromLchuv(double l, double c, double h, double alpha = 1.0) => FromLchuv(Configuration.Default, l, c, h, alpha);
    public static Unicolour FromLchuv(Configuration config, double l, double c, double h, double alpha = 1.0) => new(config, new Lchuv(l, c, h), new Alpha(alpha));
    
    public static Unicolour FromOklab((double l, double a, double b) tuple, double alpha = 1.0) => FromOklab(Configuration.Default, tuple.l, tuple.a, tuple.b, alpha);
    public static Unicolour FromOklab(Configuration config, (double l, double a, double b) tuple, double alpha = 1.0) => FromOklab(config, tuple.l, tuple.a, tuple.b, alpha);
    public static Unicolour FromOklab(double l, double a, double b, double alpha = 1.0) => FromOklab(Configuration.Default, l, a, b, alpha);
    public static Unicolour FromOklab(Configuration config, double l, double a, double b, double alpha = 1.0) => new(config, new Oklab(l, a, b), new Alpha(alpha));
    
    public static Unicolour FromOklch((double l, double c, double h) tuple, double alpha = 1.0) => FromOklch(Configuration.Default, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromOklch(Configuration config, (double l, double c, double h) tuple, double alpha = 1.0) => FromOklch(config, tuple.l, tuple.c, tuple.h, alpha);
    public static Unicolour FromOklch(double l, double c, double h, double alpha = 1.0) => FromOklch(Configuration.Default, l, c, h, alpha);
    public static Unicolour FromOklch(Configuration config, double l, double c, double h, double alpha = 1.0) => new(config, new Oklch(l, c, h), new Alpha(alpha));
}