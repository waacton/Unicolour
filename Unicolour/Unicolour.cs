namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    private Rgb? rgb;
    private RgbLinear? rgbLinear;
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
    private Hct? hct;
    
    internal readonly ColourRepresentation InitialRepresentation;
    internal readonly ColourSpace InitialColourSpace;
    public Rgb Rgb => Get<Rgb>(ColourSpace.Rgb);
    public RgbLinear RgbLinear => Get<RgbLinear>(ColourSpace.RgbLinear);
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
    public Hct Hct => Get<Hct>(ColourSpace.Hct);
    public Alpha Alpha { get; }
    public Configuration Config { get; }

    public string Hex => !IsInDisplayGamut ? "-" : Rgb.Byte255.ConstrainedHex;
    public bool IsInDisplayGamut => Rgb.IsInGamut;
    public double RelativeLuminance => RgbLinear.RelativeLuminance;
    public string Description => string.Join(" ", ColourDescription.Get(Hsl));
    public Temperature Temperature => Temperature.Get(Xyz);
    
    private Unicolour(Configuration config, ColourRepresentation initialRepresentation, Alpha alpha)
    {
        Config = config;
        Alpha = alpha;
        InitialRepresentation = initialRepresentation;
        InitialColourSpace = GetSpace(InitialRepresentation);
        SetBackingField(InitialColourSpace);
    }

    public double Contrast(Unicolour other) => Comparison.Contrast(this, other);
    public double DeltaE76(Unicolour sample) => Comparison.DeltaE76(this, sample);
    public double DeltaE94(Unicolour sample, bool isForTextiles = false) => Comparison.DeltaE94(this, sample, isForTextiles);
    public double DeltaE00(Unicolour sample) => Comparison.DeltaE00(this, sample);
    public double DeltaEItp(Unicolour sample) => Comparison.DeltaEItp(this, sample);
    public double DeltaEz(Unicolour sample) => Comparison.DeltaEz(this, sample);
    public double DeltaEHyab(Unicolour sample) => Comparison.DeltaEHyab(this, sample);
    public double DeltaEOk(Unicolour sample) => Comparison.DeltaEOk(this, sample);
    public double DeltaECam02(Unicolour sample) => Comparison.DeltaECam02(this, sample);
    public double DeltaECam16(Unicolour sample) => Comparison.DeltaECam16(this, sample);
    
    public Unicolour MixRgb(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Rgb, this, other, amount);
    public Unicolour MixRgbLinear(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.RgbLinear, this, other, amount);
    public Unicolour MixHsb(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Hsb, this, other, amount);
    public Unicolour MixHsl(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Hsl, this, other, amount);
    public Unicolour MixHwb(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Hwb, this, other, amount);
    public Unicolour MixXyz(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Xyz, this, other, amount);
    public Unicolour MixXyy(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Xyy, this, other, amount);
    public Unicolour MixLab(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Lab, this, other, amount);
    public Unicolour MixLchab(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Lchab, this, other, amount);
    public Unicolour MixLuv(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Luv, this, other, amount);
    public Unicolour MixLchuv(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Lchuv, this, other, amount);
    public Unicolour MixHsluv(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Hsluv, this, other, amount);
    public Unicolour MixHpluv(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Hpluv, this, other, amount);
    public Unicolour MixIctcp(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Ictcp, this, other, amount);
    public Unicolour MixJzazbz(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Jzazbz, this, other, amount);
    public Unicolour MixJzczhz(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Jzczhz, this, other, amount);
    public Unicolour MixOklab(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Oklab, this, other, amount);
    public Unicolour MixOklch(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Oklch, this, other, amount);
    public Unicolour MixCam02(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Cam02, this, other, amount);
    public Unicolour MixCam16(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Cam16, this, other, amount);
    public Unicolour MixHct(Unicolour other, double amount = 0.5) => Interpolation.Mix(ColourSpace.Hct, this, other, amount);
    
    public Unicolour SimulateProtanopia() => VisionDeficiency.SimulateProtanopia(this);
    public Unicolour SimulateDeuteranopia() => VisionDeficiency.SimulateDeuteranopia(this);
    public Unicolour SimulateTritanopia() => VisionDeficiency.SimulateTritanopia(this);
    public Unicolour SimulateAchromatopsia() => VisionDeficiency.SimulateAchromatopsia(this);

    public Unicolour MapToGamut() => GamutMapping.ToRgbGamut(this);
    
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