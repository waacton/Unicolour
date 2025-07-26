using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    private Rgb? rgb;
    private RgbLinear? rgbLinear;
    private Hsb? hsb;
    private Hsl? hsl;
    private Hwb? hwb;
    private Hsi? hsi;
    private Xyz? xyz;
    private Xyy? xyy;
    private Wxy? wxy;
    private Lab? lab;
    private Lchab? lchab;
    private Luv? luv;
    private Lchuv? lchuv;
    private Hsluv? hsluv;
    private Hpluv? hpluv;
    private Ypbpr? ypbpr;
    private Ycbcr? ycbcr;
    private Ycgco? ycgco;
    private Yuv? yuv;
    private Yiq? yiq;
    private Ydbdr? ydbdr;
    private Tsl? tsl;
    private Xyb? xyb;
    private Lms? lms;
    private Ipt? ipt;
    private Ictcp? ictcp;
    private Jzazbz? jzazbz;
    private Jzczhz? jzczhz;
    private Oklab? oklab;
    private Oklch? oklch;
    private Okhsv? okhsv;
    private Okhsl? okhsl;
    private Okhwb? okhwb;
    private Oklrab? oklrab;
    private Oklrch? oklrch;
    private Cam02? cam02;
    private Cam16? cam16;
    private Hct? hct;
    private Munsell? munsell;
    private Channels? icc;
    
    private bool? isInPointerGamut;
    private Temperature? temperature;
    private bool? isImaginary;
    private string? description;
    private string? source;
    
    public Configuration Configuration { get; }
    internal readonly ColourSpace SourceColourSpace;
    internal readonly ColourRepresentation SourceRepresentation;

    public Rgb Rgb => Get(ref rgb, EvaluateRgb);
    public RgbLinear RgbLinear => Get(ref rgbLinear, EvaluateRgbLinear);
    public Hsb Hsb => Get(ref hsb, EvaluateHsb);
    public Hsl Hsl => Get(ref hsl, EvaluateHsl);
    public Hwb Hwb => Get(ref hwb, EvaluateHwb);
    public Hsi Hsi => Get(ref hsi, EvaluateHsi);
    public Xyz Xyz => Get(ref xyz, EvaluateXyz);
    public Xyy Xyy => Get(ref xyy, EvaluateXyy);
    public Wxy Wxy => Get(ref wxy, EvaluateWxy);
    public Lab Lab => Get(ref lab, EvaluateLab);
    public Lchab Lchab => Get(ref lchab, EvaluateLchab);
    public Luv Luv => Get(ref luv, EvaluateLuv);
    public Lchuv Lchuv => Get(ref lchuv, EvaluateLchuv);
    public Hsluv Hsluv => Get(ref hsluv, EvaluateHsluv);
    public Hpluv Hpluv => Get(ref hpluv, EvaluateHpluv);
    public Ypbpr Ypbpr => Get(ref ypbpr, EvaluateYpbpr);
    public Ycbcr Ycbcr => Get(ref ycbcr, EvaluateYcbcr);
    public Ycgco Ycgco => Get(ref ycgco, EvaluateYcgco);
    public Yuv Yuv => Get(ref yuv, EvaluateYuv);
    public Yiq Yiq => Get(ref yiq, EvaluateYiq);
    public Ydbdr Ydbdr => Get(ref ydbdr, EvaluateYdbdr);
    public Tsl Tsl => Get(ref tsl, EvaluateTsl);
    public Xyb Xyb => Get(ref xyb, EvaluateXyb);
    public Lms Lms => Get(ref lms, EvaluateLms);
    public Ipt Ipt => Get(ref ipt, EvaluateIpt);
    public Ictcp Ictcp => Get(ref ictcp, EvaluateIctcp);
    public Jzazbz Jzazbz => Get(ref jzazbz, EvaluateJzazbz);
    public Jzczhz Jzczhz => Get(ref jzczhz, EvaluateJzczhz);
    public Oklab Oklab => Get(ref oklab, EvaluateOklab);
    public Oklch Oklch => Get(ref oklch, EvaluateOklch);
    public Okhsv Okhsv => Get(ref okhsv, EvaluateOkhsv);
    public Okhsl Okhsl => Get(ref okhsl, EvaluateOkhsl);
    public Okhwb Okhwb => Get(ref okhwb, EvaluateOkhwb);
    public Oklrab Oklrab => Get(ref oklrab, EvaluateOklrab);
    public Oklrch Oklrch => Get(ref oklrch, EvaluateOklrch);
    public Cam02 Cam02 => Get(ref cam02, EvaluateCam02);
    public Cam16 Cam16 => Get(ref cam16, EvaluateCam16);
    public Hct Hct => Get(ref hct, EvaluateHct);
    public Munsell Munsell => Get(ref munsell, EvaluateMunsell);

    public Channels Icc => Get(ref icc, () => Configuration.Icc.HasSupportedProfile
            ? Channels.FromXyz(Xyz, Configuration.Icc, Configuration.Xyz)
            : Channels.UncalibratedFromRgb(Rgb));
    
    public string Hex => isUnseen ? UnseenName : !IsInRgbGamut ? "-" : Rgb.Byte255.ConstrainedHex;
    public bool IsInRgbGamut => Rgb.IsInGamut;
    public bool IsInPointerGamut => Get(ref isInPointerGamut, () => PointerGamut.IsInGamut(this));
    public double RelativeLuminance => Xyz.UseAsNaN ? double.NaN : Xyz.Y; // will meet https://www.w3.org/TR/WCAG21/#dfn-relative-luminance when sRGB (middle row of RGB -> XYZ matrix)
    public Chromaticity Chromaticity => Xyy.UseAsNaN ? new Chromaticity(double.NaN, double.NaN) : Xyy.Chromaticity;
    public Temperature Temperature => Get(ref temperature, () => Temperature.FromChromaticity(Chromaticity, Configuration.Xyz.Planckian));
    public double DominantWavelength => Wxy.UseAsNaN || Wxy.UseAsGreyscale ? double.NaN : Wxy.DominantWavelength;
    public double ExcitationPurity => Wxy.UseAsNaN || Wxy.UseAsGreyscale ? double.NaN : Wxy.ExcitationPurity;
    public bool IsImaginary => Get(ref isImaginary, () => Configuration.Xyz.SpectralBoundary.IsOutside(Chromaticity));
    public string Description => Get(ref description, () => isUnseen ? UnseenDescription : string.Join(" ", ColourDescription.Get(Hsl)));
    public Alpha Alpha { get; }
    private string Source => Get(ref source, () => $"{SourceColourSpace} {SourceRepresentation}");
    
    private static T Get<T>(ref T? backingField, Func<T> evaluate) where T : class
    {
        backingField ??= evaluate();
        return backingField;
    }
    
    private static bool Get(ref bool? backingField, Func<bool> evaluate)
    {
        backingField ??= evaluate();
        return (bool)backingField;
    }
    
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
        SourceColourSpace = colourSpace;
        SourceRepresentation = CreateRepresentation(colourSpace, first, second, third, config, heritage);
        Alpha = new Alpha(alpha);
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

    public Unicolour Blend(Unicolour backdrop, BlendMode blendMode) => Blending.Blend(this, backdrop, blendMode);
    
    public Unicolour Simulate(Cvd cvd, double severity = 1.0) => VisionDeficiency.Simulate(cvd, severity, this);
    
    public Unicolour MapToRgbGamut(GamutMap gamutMap = GamutMap.OklchChromaReduction) => GamutMapping.ToRgbGamut(this, gamutMap);

    public Unicolour MapToPointerGamut()
    {
        // need to preserver the result so downstream usage doesn't perform in-gamut check
        // since rounding errors during chromatic adaptation to C/2° will frequently result in false
        var mapped = GamutMapping.ToPointerGamut(this);
        mapped.isInPointerGamut = !mapped.SourceRepresentation.UseAsNaN; 
        return mapped;
    } 
    
    public Unicolour ConvertToConfiguration(Configuration config)
    {
        if (config == Configuration) return Clone();
        var heritage = ColourHeritage.From(SourceRepresentation);
        var (x, y, z) = Adaptation.WhitePoint(Xyz, Configuration.Xyz.WhitePoint, config.Xyz.WhitePoint, Configuration.Xyz.AdaptationMatrix);
        return new Unicolour(config, heritage, ColourSpace.Xyz, x, y, z, Alpha.A);
    }
    
    internal Unicolour Clone()
    {
        var (first, second, third) = SourceRepresentation.Triplet;
        var heritage = SourceRepresentation.Heritage;
        var alpha = Alpha.A;
        return new Unicolour(Configuration, heritage, SourceColourSpace, first, second, third, alpha)
        {
            isInPointerGamut = isInPointerGamut
        };
    }
    
    public override string ToString()
    {
        var parts = new List<string> { $"from {Source}", $"alpha {Alpha}" };
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