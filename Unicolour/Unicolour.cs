namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    private readonly Lazy<Rgb> rgb;
    private readonly Lazy<RgbLinear> rgbLinear;
    private readonly Lazy<Hsb> hsb;
    private readonly Lazy<Hsl> hsl;
    private readonly Lazy<Hwb> hwb;
    private readonly Lazy<Hsi> hsi;
    private readonly Lazy<Xyz> xyz;
    private readonly Lazy<Xyy> xyy;
    private readonly Lazy<Lab> lab;
    private readonly Lazy<Lchab> lchab;
    private readonly Lazy<Luv> luv;
    private readonly Lazy<Lchuv> lchuv;
    private readonly Lazy<Hsluv> hsluv;
    private readonly Lazy<Hpluv> hpluv;
    private readonly Lazy<Ypbpr> ypbpr;
    private readonly Lazy<Ycbcr> ycbcr;
    private readonly Lazy<Ycgco> ycgco;
    private readonly Lazy<Yuv> yuv;
    private readonly Lazy<Yiq> yiq;
    private readonly Lazy<Ydbdr> ydbdr;
    private readonly Lazy<Ipt> ipt;
    private readonly Lazy<Ictcp> ictcp;
    private readonly Lazy<Jzazbz> jzazbz;
    private readonly Lazy<Jzczhz> jzczhz;
    private readonly Lazy<Oklab> oklab;
    private readonly Lazy<Oklch> oklch;
    private readonly Lazy<Okhsv> okhsv;
    private readonly Lazy<Okhsl> okhsl;
    private readonly Lazy<Okhwb> okhwb;
    private readonly Lazy<Cam02> cam02;
    private readonly Lazy<Cam16> cam16;
    private readonly Lazy<Hct> hct;
    private readonly Lazy<Temperature> temperature;
    private readonly Lazy<Spectral.Intersects?> spectralIntersects;
    
    internal readonly ColourRepresentation InitialRepresentation;
    internal readonly ColourSpace InitialColourSpace;
    
    public Rgb Rgb => rgb.Value;
    public RgbLinear RgbLinear => rgbLinear.Value;
    public Hsb Hsb => hsb.Value;
    public Hsl Hsl => hsl.Value;
    public Hwb Hwb => hwb.Value;
    public Hsi Hsi => hsi.Value;
    public Xyz Xyz => xyz.Value;
    public Xyy Xyy => xyy.Value;
    public Lab Lab => lab.Value;
    public Lchab Lchab => lchab.Value;
    public Luv Luv => luv.Value;
    public Lchuv Lchuv => lchuv.Value;
    public Hsluv Hsluv => hsluv.Value;
    public Hpluv Hpluv => hpluv.Value;
    public Ypbpr Ypbpr => ypbpr.Value;
    public Ycbcr Ycbcr => ycbcr.Value;
    public Ycgco Ycgco => ycgco.Value;
    public Yuv Yuv => yuv.Value;
    public Yiq Yiq => yiq.Value;
    public Ydbdr Ydbdr => ydbdr.Value;
    public Ipt Ipt => ipt.Value;
    public Ictcp Ictcp => ictcp.Value;
    public Jzazbz Jzazbz => jzazbz.Value;
    public Jzczhz Jzczhz => jzczhz.Value;
    public Oklab Oklab => oklab.Value;
    public Oklch Oklch => oklch.Value;
    public Okhsv Okhsv => okhsv.Value;
    public Okhsl Okhsl => okhsl.Value;
    public Okhwb Okhwb => okhwb.Value;
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
        hsi = new Lazy<Hsi>(EvaluateHsi);
        xyz = new Lazy<Xyz>(EvaluateXyz);
        xyy = new Lazy<Xyy>(EvaluateXyy);
        lab = new Lazy<Lab>(EvaluateLab);
        lchab = new Lazy<Lchab>(EvaluateLchab);
        luv = new Lazy<Luv>(EvaluateLuv);
        lchuv = new Lazy<Lchuv>(EvaluateLchuv);
        hsluv = new Lazy<Hsluv>(EvaluateHsluv);
        hpluv = new Lazy<Hpluv>(EvaluateHpluv);
        ypbpr = new Lazy<Ypbpr>(EvaluateYpbpr); 
        ycbcr = new Lazy<Ycbcr>(EvaluateYcbcr); 
        ycgco = new Lazy<Ycgco>(EvaluateYcgco); 
        yuv = new Lazy<Yuv>(EvaluateYuv); 
        yiq = new Lazy<Yiq>(EvaluateYiq); 
        ydbdr = new Lazy<Ydbdr>(EvaluateYdbdr); 
        ipt = new Lazy<Ipt>(EvaluateIpt);
        ictcp = new Lazy<Ictcp>(EvaluateIctcp);
        jzazbz = new Lazy<Jzazbz>(EvaluateJzazbz);
        jzczhz = new Lazy<Jzczhz>(EvaluateJzczhz);
        oklab = new Lazy<Oklab>(EvaluateOklab);
        oklch = new Lazy<Oklch>(EvaluateOklch);
        okhsv = new Lazy<Okhsv>(EvaluateOkhsv);
        okhsl = new Lazy<Okhsl>(EvaluateOkhsl);
        okhwb = new Lazy<Okhwb>(EvaluateOkhwb);
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