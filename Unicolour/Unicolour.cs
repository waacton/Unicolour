namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    private readonly Lazy<Rgb> rgb = null!;
    private readonly Lazy<RgbLinear> rgbLinear = null!;
    private readonly Lazy<Hsb> hsb = null!;
    private readonly Lazy<Hsl> hsl = null!;
    private readonly Lazy<Hwb> hwb = null!;
    private readonly Lazy<Xyz> xyz = null!;
    private readonly Lazy<Xyy> xyy = null!;
    private readonly Lazy<Lab> lab = null!;
    private readonly Lazy<Lchab> lchab = null!;
    private readonly Lazy<Luv> luv = null!;
    private readonly Lazy<Lchuv> lchuv = null!;
    private readonly Lazy<Hsluv> hsluv = null!;
    private readonly Lazy<Hpluv> hpluv = null!;
    private readonly Lazy<Ictcp> ictcp = null!;
    private readonly Lazy<Jzazbz> jzazbz = null!;
    private readonly Lazy<Jzczhz> jzczhz = null!;
    private readonly Lazy<Oklab> oklab = null!;
    private readonly Lazy<Oklch> oklch = null!;
    private readonly Lazy<Cam02> cam02 = null!;
    private readonly Lazy<Cam16> cam16 = null!;
    private readonly Lazy<Hct> hct = null!;
    private readonly Lazy<Temperature> temperature = null!;
    private readonly Lazy<Spectral.Intersects?> spectralIntersects = null!;
    
    internal readonly ColourRepresentation InitialRepresentation;
    internal readonly ColourSpace InitialColourSpace;
    
    public Rgb Rgb => rgb.Value;
    public RgbLinear RgbLinear => rgbLinear.Value;
    public Hsb Hsb => hsb.Value;
    public Hsl Hsl => hsl.Value;
    public Hwb Hwb => hwb.Value;
    public Xyz Xyz => xyz.Value;
    public Xyy Xyy => xyy.Value;
    public Lab Lab => lab.Value;
    public Lchab Lchab => lchab.Value;
    public Luv Luv => luv.Value;
    public Lchuv Lchuv => lchuv.Value;
    public Hsluv Hsluv => hsluv.Value;
    public Hpluv Hpluv => hpluv.Value;
    public Ictcp Ictcp => ictcp.Value;
    public Jzazbz Jzazbz => jzazbz.Value;
    public Jzczhz Jzczhz => jzczhz.Value;
    public Oklab Oklab => oklab.Value;
    public Oklch Oklch => oklch.Value;
    public Cam02 Cam02 => cam02.Value;
    public Cam16 Cam16 => cam16.Value;
    public Hct Hct => hct.Value;
    public Alpha Alpha { get; }
    public Configuration Config { get; }

    public string Hex => isUnseen ? UnseenName : !IsInDisplayGamut ? "-" : Rgb.Byte255.ConstrainedHex;
    public Chromaticity Chromaticity => Xyy.UseAsNaN ? new Chromaticity(double.NaN, double.NaN) : Xyy.Chromaticity;
    public bool IsInDisplayGamut => Rgb.IsInGamut;
    public double RelativeLuminance => RgbLinear.RelativeLuminance;
    public string Description => isUnseen ? UnseenDescription : string.Join(" ", ColourDescription.Get(Hsl));
    public double DominantWavelength => spectralIntersects.Value?.DominantWavelength() ?? double.NaN;
    public double ExcitationPurity => spectralIntersects.Value?.ExcitationPurity() ?? double.NaN;
    public bool IsImaginary => spectralIntersects.Value?.IsImaginary() ?? !Xyy.UseAsGreyscale;
    public Temperature Temperature => temperature.Value;
    
    internal Unicolour(Configuration config, ColourHeritage heritage,
        ColourSpace colourSpace, double first, double second, double third, double alpha = 1.0)
    {
        if (colourSpace == ColourSpace.Rgb255)
        {
            colourSpace = ColourSpace.Rgb;
            first /= 255.0;
            second /= 255.0;
            third /= 255.0;
        }
        
        Config = config;
        Alpha = new Alpha(alpha);
        InitialRepresentation = CreateRepresentation(colourSpace, first, second, third, config, heritage);
        InitialColourSpace = colourSpace;
        
        rgb = new Lazy<Rgb>(EvaluateRgb);
        rgbLinear = new Lazy<RgbLinear>(EvaluateRgbLinear);
        hsb = new Lazy<Hsb>(EvaluateHsb);
        hsl = new Lazy<Hsl>(EvaluateHsl);
        hwb = new Lazy<Hwb>(EvaluateHwb);
        xyz = new Lazy<Xyz>(EvaluateXyz);
        xyy = new Lazy<Xyy>(EvaluateXyy);
        lab = new Lazy<Lab>(EvaluateLab);
        lchab = new Lazy<Lchab>(EvaluateLchab);
        luv = new Lazy<Luv>(EvaluateLuv);
        lchuv = new Lazy<Lchuv>(EvaluateLchuv);
        hsluv = new Lazy<Hsluv>(EvaluateHsluv);
        hpluv = new Lazy<Hpluv>(EvaluateHpluv);
        ictcp = new Lazy<Ictcp>(EvaluateIctcp);
        jzazbz = new Lazy<Jzazbz>(EvaluateJzazbz);
        jzczhz = new Lazy<Jzczhz>(EvaluateJzczhz);
        oklab = new Lazy<Oklab>(EvaluateOklab);
        oklch = new Lazy<Oklch>(EvaluateOklch);
        cam02 = new Lazy<Cam02>(EvaluateCam02);
        cam16 = new Lazy<Cam16>(EvaluateCam16);
        hct = new Lazy<Hct>(EvaluateHct);

        spectralIntersects = new Lazy<Spectral.Intersects?>(() =>
            Xyy.UseAsNaN || Xyy.UseAsGreyscale
                ? null
                : Config.Xyz.Spectral.FindBoundaryIntersects(Chromaticity));
        
        // this will get overridden when called by the derived constructor that takes temperature as a parameter 
        temperature = new Lazy<Temperature>(() => Temperature.FromChromaticity(Chromaticity, Config.Xyz.Planckian));
    }

    public double Contrast(Unicolour other) => Comparison.Contrast(this, other);
    public double Difference(Unicolour reference, DeltaE deltaE) => Comparison.Difference(this, reference, deltaE);

    public Unicolour Mix(Unicolour other, ColourSpace colourSpace, double amount = 0.5, HueSpan hueSpan = HueSpan.Shorter, bool premultiplyAlpha = true)
    {
        return Interpolation.Mix(this, other, colourSpace, amount, hueSpan, premultiplyAlpha);
    }
    
    public Unicolour SimulateProtanopia() => VisionDeficiency.SimulateProtanopia(this);
    public Unicolour SimulateDeuteranopia() => VisionDeficiency.SimulateDeuteranopia(this);
    public Unicolour SimulateTritanopia() => VisionDeficiency.SimulateTritanopia(this);
    public Unicolour SimulateAchromatopsia() => VisionDeficiency.SimulateAchromatopsia(this);

    public Unicolour MapToGamut() => GamutMapping.ToRgbGamut(this);
    
    public Unicolour ConvertToConfiguration(Configuration newConfig)
    {
        var xyzMatrix = Matrix.FromTriplet(Xyz.Triplet);
        var adaptedMatrix = Adaptation.WhitePoint(xyzMatrix, Config.Xyz.WhitePoint, newConfig.Xyz.WhitePoint);
        return new Unicolour(newConfig, ColourSpace.Xyz, adaptedMatrix.ToTriplet().Tuple, Alpha.A);
    }
    
    public override string ToString()
    {
        var parts = new List<string> { $"from {InitialColourSpace} {InitialRepresentation} alpha {Alpha}" };
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