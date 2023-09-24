namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    private Rgb? rgb;
    private Hsb? hsb;
    private Hsl? hsl;
    private Hwb? hwb;
    private Xyz? xyz;
    private Xyy? xyy;
    private Lab? lab;
    private Lchab? lchab;
    private Luv? luv;
    private Lchuv? lchuv;
    private Hsluv? hsluv;
    private Hpluv? hpluv;
    private Ictcp? ictcp;
    private Jzazbz? jzazbz;
    private Jzczhz? jzczhz;
    private Oklab? oklab;
    private Oklch? oklch;
    private Cam02? cam02;
    private Cam16? cam16;
    
    internal readonly ColourRepresentation InitialRepresentation;
    internal readonly ColourSpace InitialColourSpace;
    public Rgb Rgb => Get<Rgb>(ColourSpace.Rgb);
    public Hsb Hsb => Get<Hsb>(ColourSpace.Hsb);
    public Hsl Hsl => Get<Hsl>(ColourSpace.Hsl);
    public Hwb Hwb => Get<Hwb>(ColourSpace.Hwb);
    public Xyz Xyz => Get<Xyz>(ColourSpace.Xyz);
    public Xyy Xyy => Get<Xyy>(ColourSpace.Xyy);
    public Lab Lab => Get<Lab>(ColourSpace.Lab);
    public Lchab Lchab => Get<Lchab>(ColourSpace.Lchab);
    public Luv Luv => Get<Luv>(ColourSpace.Luv);
    public Lchuv Lchuv => Get<Lchuv>(ColourSpace.Lchuv);
    public Hsluv Hsluv => Get<Hsluv>(ColourSpace.Hsluv);
    public Hpluv Hpluv => Get<Hpluv>(ColourSpace.Hpluv);
    public Ictcp Ictcp => Get<Ictcp>(ColourSpace.Ictcp);
    public Jzazbz Jzazbz => Get<Jzazbz>(ColourSpace.Jzazbz);
    public Jzczhz Jzczhz => Get<Jzczhz>(ColourSpace.Jzczhz);
    public Oklab Oklab => Get<Oklab>(ColourSpace.Oklab);
    public Oklch Oklch => Get<Oklch>(ColourSpace.Oklch);
    public Cam02 Cam02 => Get<Cam02>(ColourSpace.Cam02);
    public Cam16 Cam16 => Get<Cam16>(ColourSpace.Cam16);
    public Alpha Alpha { get; }
    public Configuration Config { get; }

    public string Hex => this.GetHex();
    public bool IsDisplayable => this.GetIsDisplayable();
    public double RelativeLuminance => this.GetRelativeLuminance();
    public string Description => string.Join(" ", this.GetDescriptions());
    public Temperature Temperature => this.GetTemperature();
    
    private Unicolour(Configuration config, ColourRepresentation initialRepresentation, Alpha alpha)
    {
        Config = config;
        Alpha = alpha;
        InitialRepresentation = initialRepresentation;
        InitialColourSpace = GetSpace(InitialRepresentation);
        SetBackingField(InitialColourSpace);
    }

    public Unicolour ConvertToConfiguration(Configuration newConfig)
    {
        var xyzMatrix = Matrix.FromTriplet(Xyz.Triplet);
        var adaptedMatrix = Adaptation.WhitePoint(xyzMatrix, Config.Xyz.WhitePoint, newConfig.Xyz.WhitePoint);
        return FromXyz(newConfig, adaptedMatrix.ToTriplet().Tuple, Alpha.A);
    }
    
    public override string ToString()
    {
        var parts = new List<string> { $"from {InitialColourSpace} {InitialRepresentation}, alpha {Alpha.A}" };
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
        return InitialRepresentation.Equals(other.InitialRepresentation);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (InitialRepresentation.GetHashCode() * 397) ^ Alpha.GetHashCode();
        }
    }
}