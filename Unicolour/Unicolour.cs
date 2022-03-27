namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    private readonly ColourSpace initialSpace;
    private Rgb? rgb;
    private Hsb? hsb;
    private Hsl? hsl;
    private Xyz? xyz;
    private Lab? lab;

    public Rgb Rgb => Get(() => rgb, ColourSpace.Rgb)!;
    public Hsb Hsb => Get(() => hsb, ColourSpace.Hsb)!;
    public Hsl Hsl => Get(() => hsl, ColourSpace.Hsl)!;
    public Xyz Xyz => Get(() => xyz, ColourSpace.Xyz)!;
    public Lab Lab => Get(() => lab, ColourSpace.Lab)!;
    public Alpha Alpha { get; }
    public Configuration Config { get; }

    public double Luminance => this.Luminance();

    private Unicolour(Configuration config, Alpha alpha, ColourSpace colourSpace)
    {
        SetupConversions();
        Alpha = alpha;
        Config = config;
        initialSpace = colourSpace;
    }

    public override string ToString() => $"RGB:[{Rgb}] Hex:{Rgb.Hex} HSB:[{Hsb}] A:{Alpha.A}";

    // ----- the following is based on auto-generated code -----

    public bool Equals(Unicolour? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ColourSpaceEquals(other) && Alpha.Equals(other.Alpha);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Unicolour) obj);
    }
    
    private bool ColourSpaceEquals(Unicolour other)
    {
        return initialSpace switch
        {
            ColourSpace.Rgb => Rgb.Equals(other.Rgb),
            ColourSpace.Hsb => Hsb.Equals(other.Hsb),
            ColourSpace.Hsl => Hsl.Equals(other.Hsl),
            ColourSpace.Xyz => Xyz.Equals(other.Xyz),
            ColourSpace.Lab => Lab.Equals(other.Lab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var colourSpaceHashCode = initialSpace switch
            {
                ColourSpace.Rgb => Rgb.GetHashCode() * 397,
                ColourSpace.Hsb => Hsb.GetHashCode() * 397,
                ColourSpace.Hsl => Hsl.GetHashCode() * 397,
                ColourSpace.Xyz => Xyz.GetHashCode() * 397,
                ColourSpace.Lab => Lab.GetHashCode() * 397,
                _ => throw new ArgumentOutOfRangeException()
            };

            return colourSpaceHashCode ^ Alpha.GetHashCode();
        }
    }
}