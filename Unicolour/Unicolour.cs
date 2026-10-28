using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour;

public partial class Unicolour : IEquatable<Unicolour>
{
    public Configuration Configuration { get; }
    internal readonly ColourSpace SourceColourSpace;
    internal readonly ColourRepresentation SourceRepresentation;
    
    public Rgb Rgb => field ??= EvaluateRgb();
    public Rgb255 Rgb255 => Rgb.Byte255;
    public RgbLinear RgbLinear => field ??= EvaluateRgbLinear();
    public Hsb Hsb => field ??= EvaluateHsb();
    public Hsl Hsl => field ??= EvaluateHsl();
    public Hwb Hwb => field ??= EvaluateHwb();
    public Hsi Hsi => field ??= EvaluateHsi();
    public Xyz Xyz => field ??= EvaluateXyz();
    public Xyy Xyy => field ??= EvaluateXyy();
    public Wxy Wxy => field ??= EvaluateWxy();
    public Lab Lab => field ??= EvaluateLab();
    public Lchab Lchab => field ??= EvaluateLchab();
    public Luv Luv => field ??= EvaluateLuv();
    public Lchuv Lchuv => field ??= EvaluateLchuv();
    public Hsluv Hsluv => field ??= EvaluateHsluv();
    public Hpluv Hpluv => field ??= EvaluateHpluv();
    public Ypbpr Ypbpr => field ??= EvaluateYpbpr();
    public Ycbcr Ycbcr => field ??= EvaluateYcbcr();
    public Ycgco Ycgco => field ??= EvaluateYcgco();
    public Yuv Yuv => field ??= EvaluateYuv();
    public Yiq Yiq => field ??= EvaluateYiq();
    public Ydbdr Ydbdr => field ??= EvaluateYdbdr();
    public Tsl Tsl => field ??= EvaluateTsl();
    public Xyb Xyb => field ??= EvaluateXyb();
    public Lms Lms => field ??= EvaluateLms();
    public Ipt Ipt => field ??= EvaluateIpt();
    public Ictcp Ictcp => field ??= EvaluateIctcp();
    public Jzazbz Jzazbz => field ??= EvaluateJzazbz();
    public Jzczhz Jzczhz => field ??= EvaluateJzczhz();
    public Oklab Oklab => field ??= EvaluateOklab();
    public Oklch Oklch => field ??= EvaluateOklch();
    public Okhsv Okhsv => field ??= EvaluateOkhsv();
    public Okhsl Okhsl => field ??= EvaluateOkhsl();
    public Okhwb Okhwb => field ??= EvaluateOkhwb();
    public Oklrab Oklrab => field ??= EvaluateOklrab();
    public Oklrch Oklrch => field ??= EvaluateOklrch();
    public Cam02 Cam02 => field ??= EvaluateCam02();
    public Cam02Jch Cam02Jch => Cam02.Jch;
    public Cam02Jmh Cam02Jmh => Cam02.Jmh;
    public Cam02Jsh Cam02Jsh => Cam02.Jsh;
    public Cam02Qch Cam02Qch => Cam02.Qch;
    public Cam02Qmh Cam02Qmh => Cam02.Qmh;
    public Cam02Qsh Cam02Qsh => Cam02.Qsh;
    public Cam02Ucs Cam02Ucs => field ??= EvaluateCam02Ucs();
    public Cam16 Cam16 => field ??= EvaluateCam16();
    public Hct Hct => field ??= EvaluateHct();
    public Munsell Munsell => field ??= EvaluateMunsell();
    
    public Channels Icc => field ??= Configuration.Icc.HasSupportedProfile
            ? Channels.FromXyz(Xyz, Configuration.Xyz.ChromaticAdaptor, Configuration.Icc)
            : Channels.UncalibratedFromRgb(Rgb);
    
    public string Hex => isUnseen ? UnseenName : !IsInRgbGamut ? "-" : Rgb.Byte255.Clipped.Hex;
    public double RelativeLuminance => Xyz.IsNaN ? double.NaN : Xyz.Y; // will meet https://www.w3.org/TR/WCAG21/#dfn-relative-luminance when sRGB (middle row of RGB -> XYZ matrix)
    public Chromaticity Chromaticity => Xyy.IsNaN ? new Chromaticity(double.NaN, double.NaN) : Xyy.Chromaticity;
    public Temperature Temperature => field ??= Temperature.FromChromaticity(Chromaticity, Configuration.Xyz.Planckian);
    public double DominantWavelength => Wxy.IsNaN ? double.NaN : Wxy.W;
    public double ExcitationPurity => Wxy.IsNaN ? double.NaN : Wxy.X;
    
    private bool? isInPointerGamut = null, isInMacAdamLimits = null, isImaginary = null; // want only the backing field to be nullable, `field` keyword does not support this
    public bool IsInRgbGamut => Rgb.IsInGamut;
    public bool IsInPointerGamut => isInPointerGamut ??= PointerGamut.IsInGamut(this);
    public bool IsInMacAdamLimits => isInMacAdamLimits ??= MacAdamLimits.IsInLimits(this);
    public bool IsImaginary => isImaginary ??= !Configuration.Xyz.SpectralBoundary.Contains(Chromaticity);
    
    public string Description => field ??=isUnseen ? UnseenDescription : string.Join(" ", ColourDescription.Get(Hsl));
    public Alpha Alpha { get; }
    private string Source => field ??= $"{SourceColourSpace} {SourceRepresentation}";
    
    internal Unicolour(Configuration config, ColourSpace colourSpace, double first, double second, double third, double alpha, Limitation limitation)
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
        SourceRepresentation = CreateRepresentation(colourSpace, first, second, third, config, limitation);
        Alpha = new Alpha(alpha);
    }
    
    public Unicolour(Configuration config, ColourSpace colourSpace, double grey, double alpha = 1.0)
    {
        if (colourSpace == ColourSpace.Rgb255)
        {
            colourSpace = ColourSpace.Rgb;
            grey /= 255.0;
        }
        
        Configuration = config;
        SourceColourSpace = colourSpace;
        SourceRepresentation = CreateRepresentation(colourSpace, grey, config);
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
        // need to preserve the result so downstream usage doesn't perform in-gamut check
        // since rounding errors during chromatic adaptation to C/2° will frequently result in false
        var mapped = GamutMapping.ToPointerGamut(this);
        mapped.isInPointerGamut = !mapped.SourceRepresentation.IsNaN; 
        return mapped;
    }

    public Unicolour MapToMacAdamLimits()
    {
        var mapped = GamutMapping.ToMacAdamLimits(this);
        mapped.isInMacAdamLimits = !mapped.SourceRepresentation.IsNaN; 
        return mapped;
    }
    
    public Unicolour ConvertToConfiguration(Configuration config)
    {
        if (config == Configuration)
        {
            return Clone();
        }
        
        var chromaticAdaptor = Configuration.Xyz.ChromaticAdaptor;
        var (x, y, z) = chromaticAdaptor.AdaptTo(Xyz, config.Xyz.WhitePoint);
        var limitation = SourceRepresentation.Limitation;
        return new Unicolour(config, ColourSpace.Xyz, x, y, z, Alpha.A, limitation);
    }
    
    internal Unicolour Clone()
    {
        var (first, second, third) = SourceRepresentation.Triplet;
        var alpha = Alpha.A;
        var limitation = SourceRepresentation.LimitationBaseline;
        return new Unicolour(Configuration, SourceColourSpace, first, second, third, alpha, limitation)
        {
            isInPointerGamut = isInPointerGamut,
            isInMacAdamLimits = isInMacAdamLimits
        };
    }
    
    public override string ToString()
    {
        List<string> parts = [$"from {Source}", $"alpha {Alpha}"];
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
        var spaceequals = ColourSpaceEquals(other);
        var alpaequals = Alpha.Equals(other.Alpha);
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
        var equal = SourceRepresentation.Equals(other.SourceRepresentation);
        return equal;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (SourceRepresentation.GetHashCode() * 397) ^ Alpha.GetHashCode();
        }
    }
}