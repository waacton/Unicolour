namespace Wacton.Unicolour;

public class Unicolour : IEquatable<Unicolour>
{
    private readonly ColourSpace initialSpace;
    private Rgb? rgb;
    private Hsb? hsb;
    private Hsl? hsl;
    private Xyz? xyz;
    private Lab? lab;

    public Rgb Rgb => rgb ??= Conversion.HsbToRgb(Hsb, Config);
    public Hsb Hsb => hsb ??= rgb != null ? Conversion.RgbToHsb(Rgb) : Conversion.HslToHsb(Hsl);
    public Hsl Hsl => hsl ??= Conversion.HsbToHsl(Hsb);
    public Xyz Xyz => xyz ??= Conversion.RgbToXyz(Rgb, Config);
    public Lab Lab => lab ??= Conversion.XyzToLab(Xyz, Config);
    public Alpha Alpha { get; }
    public Configuration Config { get; }

    public double Luminance => this.Luminance();

    private Unicolour(Configuration config, Rgb rgb, Alpha alpha)
    {
        this.rgb = rgb;
        Alpha = alpha;
        Config = config;
        initialSpace = ColourSpace.Rgb;
    }
    
    private Unicolour(Configuration config, Hsb hsb, Alpha alpha)
    {
        this.hsb = hsb;
        Alpha = alpha;
        Config = config;
        initialSpace = ColourSpace.Hsb;
    }
    
    private Unicolour(Configuration config, Hsl hsl, Alpha alpha)
    {
        this.hsl = hsl;
        Alpha = alpha;
        Config = config;
        initialSpace = ColourSpace.Hsl;
    }

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

    public override string ToString() => $"RGB:[{Rgb}] Hex:{Rgb.Hex} HSB:[{Hsb}] A:{Alpha.A}";

    // ----- the following is based on auto-generated code -----
    
    public bool Equals(Unicolour? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ColourSpaceEquals(other) && Alpha.Equals(other.Alpha);
    }

    private bool ColourSpaceEquals(Unicolour other)
    {
        switch (initialSpace)
        {
            case ColourSpace.Rgb: return Rgb.Equals(other.Rgb);
            case ColourSpace.Hsb: return Hsb.Equals(other.Hsb);
            case ColourSpace.Hsl: return Hsl.Equals(other.Hsl);
            case ColourSpace.Xyz:
            case ColourSpace.Lab:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Unicolour) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int colourSpaceHashCode;
            switch (initialSpace)
            {
                case ColourSpace.Rgb: 
                    colourSpaceHashCode = Rgb.GetHashCode() * 397;
                    break;
                case ColourSpace.Hsb: 
                    colourSpaceHashCode = Hsb.GetHashCode() * 397;
                    break;
                case ColourSpace.Hsl: 
                    colourSpaceHashCode = Hsl.GetHashCode() * 397;
                    break;
                case ColourSpace.Xyz:
                case ColourSpace.Lab:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return colourSpaceHashCode ^ Alpha.GetHashCode();
        }
    }
    
    private enum ColourSpace { Rgb, Hsb, Hsl, Xyz, Lab }
}