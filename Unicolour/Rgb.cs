namespace Wacton.Unicolour;

public record Rgb : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double R => First;
    public double G => Second;
    public double B => Third;
    
    protected override bool IsAchromatic => R == G && G == B;
    
    public Rgb Clipped => new(R.Clamp(0.0, 1.0), G.Clamp(0.0, 1.0), B.Clamp(0.0, 1.0), Limitation);
    
    // for almost all cases, doing this check in linear RGB will return the same result
    // but handling it here feels most natural as it is the intended "display" space
    // and isn't concerned about questionable custom to-linear functions (e.g. where RGB <= 1.0 but RGB-Linear > 1.0)
    internal bool IsInGamut => !IsNaN && Triplet == Clipped.Triplet;
    public Rgb255 Byte255 => new(To255(R), To255(G), To255(B), Limitation);

    public Rgb(double r, double g, double b) : this(r, g, b, Limitation.None) {}
    public Rgb(double grey) : this(grey, grey, grey, Limitation.Achromatic) {}
    internal Rgb(double r, double g, double b, Limitation limitation) : base(r, g, b, limitation) {}
    
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
        return new Rgb(r, g, b, rgbLinear.Limitation);
    }
    
    internal static RgbLinear ToRgbLinear(Rgb rgb, RgbConfiguration rgbConfig, DynamicRange dynamicRange)
    {
        var r = rgbConfig.ToLinear(rgb.R, dynamicRange);
        var g = rgbConfig.ToLinear(rgb.G, dynamicRange);
        var b = rgbConfig.ToLinear(rgb.B, dynamicRange);
        return new RgbLinear(r, g, b, rgb.Limitation);
    }
}