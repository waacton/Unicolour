namespace Wacton.Unicolour;

public partial class Unicolour
{
    private Unicolour(Configuration config, Rgb rgb, Alpha alpha) : this(config, alpha, ColourSpace.Rgb) => this.rgb = rgb;
    private Unicolour(Configuration config, Hsb hsb, Alpha alpha) : this(config, alpha, ColourSpace.Hsb) => this.hsb = hsb;
    private Unicolour(Configuration config, Hsl hsl, Alpha alpha) : this(config, alpha, ColourSpace.Hsl) => this.hsl = hsl;
    private Unicolour(Configuration config, Xyz xyz, Alpha alpha) : this(config, alpha, ColourSpace.Xyz) => this.xyz = xyz;
    private Unicolour(Configuration config, Lab lab, Alpha alpha) : this(config, alpha, ColourSpace.Lab) => this.lab = lab;
    
    public static Unicolour FromHex(string hex) => FromHex(Configuration.Default, hex);
    public static Unicolour FromHex(Configuration config, string hex)
    {
        var (r255, g255, b255, a255) = Utils.ParseColourHex(hex);
        return FromRgb255(config, r255, g255, b255, a255);
    }
    
    public static Unicolour FromRgb255(int r255, int g255, int b255, int a255 = 255) => FromRgb255(Configuration.Default, r255, g255, b255, a255);
    public static Unicolour FromRgb255(Configuration config, int r255, int g255, int b255, int a255 = 255) => FromRgb(config, r255/255.0, g255/255.0, b255/255.0, a255/255.0);
    
    public static Unicolour FromRgb(double r, double g, double b, double a = 1.0) => FromRgb(Configuration.Default, r, g, b, a);
    public static Unicolour FromRgb(Configuration config, double r, double g, double b, double a = 1.0) => new(config, new Rgb(r, g, b, config), new Alpha(a));
    
    public static Unicolour FromHsb(double h, double s, double b, double a = 1.0) => FromHsb(Configuration.Default, h, s, b, a);
    public static Unicolour FromHsb(Configuration config, double h, double s, double b, double a = 1.0) => new(config, new Hsb(h, s, b), new Alpha(a));
    
    public static Unicolour FromHsl(double h, double s, double l, double a = 1.0) => FromHsl(Configuration.Default, h, s, l, a);
    public static Unicolour FromHsl(Configuration config, double h, double s, double l, double a = 1.0) => new(config, new Hsl(h, s, l), new Alpha(a));
    
    public static Unicolour FromXyz(double x, double y, double z, double a = 1.0) => FromXyz(Configuration.Default, x, y, z, a);
    public static Unicolour FromXyz(Configuration config, double x, double y, double z, double a = 1.0) => new(config, new Xyz(x, y, z), new Alpha(a));
    
    public static Unicolour FromLab(double l, double a, double b, double alpha = 1.0) => FromLab(Configuration.Default, l, a, b, alpha);
    public static Unicolour FromLab(Configuration config, double l, double a, double b, double alpha = 1.0) => new(config, new Lab(l, a, b), new Alpha(alpha));
}