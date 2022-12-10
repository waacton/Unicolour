namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    private readonly ColourSpace initialColourSpace;
    private Rgb? rgb;
    private Hsb? hsb;
    private Hsl? hsl;
    private Xyz? xyz;
    private Lab? lab;
    private Lchab? lchab;
    private Luv? luv;
    private Lchuv? lchuv;
    private Hsluv? hsluv;
    private Hpluv? hpluv;
    private Jzazbz? jzazbz;
    private Jzczhz? jzczhz;
    private Oklab? oklab;
    private Oklch? oklch;
    
    public Rgb Rgb => Get(ColourSpace.Rgb, () => rgb)!;
    public Hsb Hsb => Get(ColourSpace.Hsb, () => hsb)!;
    public Hsl Hsl => Get(ColourSpace.Hsl, () => hsl)!;
    public Xyz Xyz => Get(ColourSpace.Xyz, () => xyz)!;
    public Lab Lab => Get(ColourSpace.Lab, () => lab)!;
    public Lchab Lchab => Get(ColourSpace.Lchab, () => lchab)!;
    public Luv Luv => Get(ColourSpace.Luv, () => luv)!;
    public Lchuv Lchuv => Get(ColourSpace.Lchuv, () => lchuv)!;
    public Hsluv Hsluv => Get(ColourSpace.Hsluv, () => hsluv)!;
    public Hpluv Hpluv => Get(ColourSpace.Hpluv, () => hpluv)!;
    public Jzazbz Jzazbz => Get(ColourSpace.Jzazbz, () => jzazbz)!;
    public Jzczhz Jzczhz => Get(ColourSpace.Jzczhz, () => jzczhz)!;
    public Oklab Oklab => Get(ColourSpace.Oklab, () => oklab)!;
    public Oklch Oklch => Get(ColourSpace.Oklch, () => oklch)!;
    public Alpha Alpha { get; }
    public Configuration Config { get; }

    public string Hex => IsDisplayable ? this.Hex() : "-";
    public bool IsDisplayable => this.IsDisplayable();
    public double RelativeLuminance => this.RelativeLuminance();
    public string Description => string.Join(" ", this.Description());
    
    private Unicolour(Configuration config, ColourRepresentation initialColourRepresentation, Alpha alpha)
    {
        Config = config;
        Alpha = alpha;
        SetInitialRepresentation(initialColourRepresentation);
        initialColourSpace = initialColourRepresentation.ColourSpace;
    }

    public override string ToString()
    {
        var parts = new List<string> { $"from {initialColourSpace} {InitialRepresentation()}, alpha {Alpha.A}" };
        if (Description != ColourDescription.NotApplicable.ToString())
        {
            parts.Add(Description);
        }

        return string.Join(" · ", parts);
    }

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
        return InitialRepresentation().Equals(other.InitialRepresentation());
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (InitialRepresentation().GetHashCode() * 397) ^ Alpha.GetHashCode();
        }
    }
}