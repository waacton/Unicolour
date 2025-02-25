using Wacton.Unicolour.Icc;

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
    private readonly Lazy<Wxy> wxy;
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
    private readonly Lazy<Tsl> tsl;
    private readonly Lazy<Xyb> xyb;
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
    private readonly Lazy<Channels> icc;
    private readonly Lazy<Temperature> temperature;
    private readonly Lazy<string> source;
    
    public Alpha Alpha { get; }
    public Configuration Configuration { get; }
    internal readonly ColourSpace SourceColourSpace;
    internal readonly ColourRepresentation SourceRepresentation;

    public Rgb Rgb => rgb.Value;
    public RgbLinear RgbLinear => rgbLinear.Value;
    public Hsb Hsb => hsb.Value;
    public Hsl Hsl => hsl.Value;
    public Hwb Hwb => hwb.Value;
    public Hsi Hsi => hsi.Value;
    public Xyz Xyz => xyz.Value;
    public Xyy Xyy => xyy.Value;
    public Wxy Wxy => wxy.Value;
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
    public Tsl Tsl => tsl.Value;
    public Xyb Xyb => xyb.Value;
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
    public Channels Icc => icc.Value;

    public string Hex => isUnseen ? UnseenName : !IsInRgbGamut ? "-" : Rgb.Byte255.ConstrainedHex;
    public bool IsInRgbGamut => Rgb.IsInGamut;
    public string Description => isUnseen ? UnseenDescription : string.Join(" ", ColourDescription.Get(Hsl));
    public Chromaticity Chromaticity => Xyy.UseAsNaN ? new Chromaticity(double.NaN, double.NaN) : Xyy.Chromaticity;
    public bool IsImaginary => Configuration.Xyz.SpectralBoundary.IsOutside(Chromaticity);
    public double RelativeLuminance => Xyz.UseAsNaN ? double.NaN : Xyz.Y; // will meet https://www.w3.org/TR/WCAG21/#dfn-relative-luminance when sRGB (middle row of RGB -> XYZ matrix)
    public Temperature Temperature => temperature.Value;
    public double DominantWavelength => Wxy.UseAsNaN || Wxy.UseAsGreyscale ? double.NaN : Wxy.DominantWavelength;
    public double ExcitationPurity => Wxy.UseAsNaN || Wxy.UseAsGreyscale ? double.NaN : Wxy.ExcitationPurity;

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
        
        Configuration = config;
        Alpha = new Alpha(alpha);
        SourceColourSpace = colourSpace;
        SourceRepresentation = CreateRepresentation(colourSpace, first, second, third, config, heritage);

        rgb = new Lazy<Rgb>(EvaluateRgb);
        rgbLinear = new Lazy<RgbLinear>(EvaluateRgbLinear);
        hsb = new Lazy<Hsb>(EvaluateHsb);
        hsl = new Lazy<Hsl>(EvaluateHsl);
        hwb = new Lazy<Hwb>(EvaluateHwb);
        hsi = new Lazy<Hsi>(EvaluateHsi);
        xyz = new Lazy<Xyz>(EvaluateXyz);
        xyy = new Lazy<Xyy>(EvaluateXyy);
        wxy = new Lazy<Wxy>(EvaluateWxy);
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
        tsl = new Lazy<Tsl>(EvaluateTsl);
        xyb = new Lazy<Xyb>(EvaluateXyb);
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
        
        // the following are overriden by the derived constructors
        // that enable Unicolour to be constructed from entities other than colour spaces
        icc = new Lazy<Channels>(() =>
            Configuration.Icc.HasSupportedProfile
                ? Channels.FromXyz(Xyz, Configuration.Icc, Configuration.Xyz)
                : Channels.UncalibratedFromRgb(Rgb));
        
        temperature = new Lazy<Temperature>(() => Temperature.FromChromaticity(Chromaticity, Configuration.Xyz.Planckian));
        source = new Lazy<string>(() => $"{SourceColourSpace} {SourceRepresentation}");
    }

    public double Contrast(Unicolour other) => Comparison.Contrast(this, other);
    public double Difference(Unicolour reference, DeltaE deltaE) => Comparison.Difference(this, reference, deltaE);

    public Unicolour Mix(Unicolour other, ColourSpace colourSpace, double amount = 0.5, HueSpan hueSpan = HueSpan.Shorter, bool premultiplyAlpha = true)
    {
        return Interpolation.Mix(this, other, colourSpace, amount, hueSpan, premultiplyAlpha);
    }
    
    public IEnumerable<Unicolour> Palette(Unicolour other, ColourSpace colourSpace, int count, HueSpan hueSpan = HueSpan.Shorter, bool premultiplyAlpha = true)
    {
        return Interpolation.Palette(this, other, colourSpace, count, hueSpan, premultiplyAlpha);
    }
    
    // TODO: explore if this is worthwhile
    // public Unicolour MixChannels(Unicolour other, double amount = 0.5, bool premultiplyAlpha = true)
    // {
    //     return Interpolation.MixChannels(this, other, amount, premultiplyAlpha);
    // }

    public Unicolour Simulate(Cvd cvd) => VisionDeficiency.Simulate(cvd, this);

    public Unicolour MapToRgbGamut(GamutMap gamutMap = GamutMap.OklchChromaReduction) => GamutMapping.ToRgbGamut(this, gamutMap);
    
    public Unicolour ConvertToConfiguration(Configuration config)
    {
        var adapted = Adaptation.WhitePoint(Xyz, Configuration.Xyz.WhitePoint, config.Xyz.WhitePoint, Configuration.Xyz.AdaptationMatrix);
        return new Unicolour(config, ColourSpace.Xyz, adapted.Tuple, Alpha.A);
    }
    
    public override string ToString()
    {
        var parts = new List<string> { $"from {source.Value}", $"alpha {Alpha}" };
        if (Description != ColourDescription.NotApplicable.ToString())
        {
            parts.Add(Description);
        }

        return string.Join(" · ", parts);
    }

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
        return obj.GetType() == GetType() && Equals((Unicolour) obj);
    }

    private bool ColourSpaceEquals(Unicolour other)
    {
        return SourceRepresentation.Equals(other.SourceRepresentation);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (SourceRepresentation.GetHashCode() * 397) ^ Alpha.GetHashCode();
        }
    }
}