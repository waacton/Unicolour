namespace Wacton.Unicolour;

public record Rgb : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double R => First;
    public double G => Second;
    public double B => Third;
    public double ConstrainedR => ConstrainedFirst;
    public double ConstrainedG => ConstrainedSecond;
    public double ConstrainedB => ConstrainedThird;
    protected override double ConstrainedFirst => R.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => G.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => B.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => ConstrainedR.Equals(ConstrainedG) && ConstrainedG.Equals(ConstrainedB);

    // for almost all cases, doing this check in linear RGB will return the same result
    // but handling it here feels most natural as it is the intended "display" space
    // and isn't concerned about questionable custom to-linear functions (e.g. where RGB <= 1.0 but RGB-Linear > 1.0)
    internal bool IsInGamut => !UseAsNaN && Triplet == ConstrainedTriplet;
    public Rgb255 Byte255 => new(To255(R), To255(G), To255(B), ColourHeritage.From(this));

    public Rgb(double r, double g, double b) : this(r, g, b, ColourHeritage.None) {}
    internal Rgb(double r, double g, double b, ColourHeritage heritage) : base(r, g, b, heritage) {}
    
    private static double To255(double value) => Math.Round(value * 255);
    
    protected override string String => $"{R:F2} {G:F2} {B:F2}";
    public override string ToString() => base.ToString();
    
    /*
     * RGB is a transform of RGB Linear
     * Forward: https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
     * Reverse: https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
     */
    
    internal static Rgb FromRgbLinear(RgbLinear rgbLinear, RgbConfiguration rgbConfig, DynamicRange dynamicRange)
    {
        var r = rgbConfig.FromLinear(rgbLinear.R, dynamicRange);
        var g = rgbConfig.FromLinear(rgbLinear.G, dynamicRange);
        var b = rgbConfig.FromLinear(rgbLinear.B, dynamicRange);
        return new Rgb(r, g, b, ColourHeritage.From(rgbLinear));
    }
    
    internal static RgbLinear ToRgbLinear(Rgb rgb, RgbConfiguration rgbConfig, DynamicRange dynamicRange)
    {
        var r = rgbConfig.ToLinear(rgb.R, dynamicRange);
        var g = rgbConfig.ToLinear(rgb.G, dynamicRange);
        var b = rgbConfig.ToLinear(rgb.B, dynamicRange);
        return new RgbLinear(r, g, b, ColourHeritage.From(rgb));
    }
}