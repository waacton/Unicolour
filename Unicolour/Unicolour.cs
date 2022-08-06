namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    private readonly ColourSpace initialSpace;
    private Rgb? rgb;
    private Hsb? hsb;
    private Hsl? hsl;
    private Xyz? xyz;
    private Lab? lab;
    private Lchab? lchab;
    private Luv? luv;
    private Lchuv? lchuv;
    private Jzazbz? jzazbz;
    private Jzczhz? jzczhz;
    private Oklab? oklab;
    private Oklch? oklch;

    public Rgb Rgb => Get(() => rgb, ColourSpace.Rgb)!;
    public Hsb Hsb => Get(() => hsb, ColourSpace.Hsb)!;
    public Hsl Hsl => Get(() => hsl, ColourSpace.Hsl)!;
    public Xyz Xyz => Get(() => xyz, ColourSpace.Xyz)!;
    public Lab Lab => Get(() => lab, ColourSpace.Lab)!;
    public Lchab Lchab => Get(() => lchab, ColourSpace.Lchab)!;
    public Luv Luv => Get(() => luv, ColourSpace.Luv)!;
    public Lchuv Lchuv => Get(() => lchuv, ColourSpace.Lchuv)!;
    public Jzazbz Jzazbz => Get(() => jzazbz, ColourSpace.Jzazbz)!;
    public Jzczhz Jzczhz => Get(() => jzczhz, ColourSpace.Jzczhz)!;
    public Oklab Oklab => Get(() => oklab, ColourSpace.Oklab)!;
    public Oklch Oklch => Get(() => oklch, ColourSpace.Oklch)!;
    public Alpha Alpha { get; }
    public Configuration Config { get; }

    public string Hex => CanBeDisplayed ? this.GetHex() : "-";
    public bool CanBeDisplayed => this.CanBeDisplayed();
    public double RelativeLuminance => this.RelativeLuminance();

    private Unicolour(Configuration config, Alpha alpha, ColourSpace colourSpace)
    {
        SetupConversions();
        Alpha = alpha;
        Config = config;
        initialSpace = colourSpace;
    }

    public override string ToString() => $"RGB:[{Rgb}] Hex:{Hex} XYZ:[{Xyz}] A:{Alpha.A}";

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
            ColourSpace.Lchab => Lchab.Equals(other.Lchab),
            ColourSpace.Luv => Luv.Equals(other.Luv),
            ColourSpace.Lchuv => Lchuv.Equals(other.Lchuv),
            ColourSpace.Jzazbz => Jzazbz.Equals(other.Jzazbz),
            ColourSpace.Jzczhz => Jzczhz.Equals(other.Jzczhz),
            ColourSpace.Oklab => Oklab.Equals(other.Oklab),
            ColourSpace.Oklch => Oklch.Equals(other.Oklch),
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
                ColourSpace.Lchab => Lchab.GetHashCode() * 397,
                ColourSpace.Luv => Luv.GetHashCode() * 397,
                ColourSpace.Lchuv => Lchuv.GetHashCode() * 397,
                ColourSpace.Jzazbz => Jzazbz.GetHashCode() * 397,
                ColourSpace.Jzczhz => Jzczhz.GetHashCode() * 397,
                ColourSpace.Oklab => Oklab.GetHashCode() * 397,
                ColourSpace.Oklch => Oklch.GetHashCode() * 397,
                _ => throw new ArgumentOutOfRangeException()
            };

            return colourSpaceHashCode ^ Alpha.GetHashCode();
        }
    }
}