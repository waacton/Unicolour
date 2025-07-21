namespace Wacton.Unicolour;

public record RgbLinear : ColourRepresentation
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

    public RgbLinear(double r, double g, double b) : this(r, g, b, ColourHeritage.None) {}
    internal RgbLinear(double r, double g, double b, ColourHeritage heritage) : base(r, g, b, heritage) {}
    
    protected override string String => $"{R:F2} {G:F2} {B:F2}";
    public override string ToString() => base.ToString();
    
    /*
     * RGB Linear is a transform of XYZ
     * Forward: https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
     * Reverse: https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
     */
    
    internal static RgbLinear FromXyz(Xyz xyz, RgbConfiguration rgbConfig, XyzConfiguration xyzConfig)
    {
        var xyzMatrix = Matrix.From(xyz);
        var rgbToXyzMatrix = Adaptation.WhitePoint(rgbConfig.RgbToXyzMatrix, rgbConfig.WhitePoint, xyzConfig.WhitePoint, xyzConfig.AdaptationMatrix);
        var (r, g, b) = rgbToXyzMatrix.Inverse().Multiply(xyzMatrix).ToTriplet();
        return new RgbLinear(r, g, b, ColourHeritage.From(xyz));
    }
    
    internal static Xyz ToXyz(RgbLinear rgbLinear, RgbConfiguration rgbConfig, XyzConfiguration xyzConfig)
    {
        var rgbLinearMatrix = Matrix.From(rgbLinear);
        var rgbToXyzMatrix = Adaptation.WhitePoint(rgbConfig.RgbToXyzMatrix, rgbConfig.WhitePoint, xyzConfig.WhitePoint, xyzConfig.AdaptationMatrix);
        var (x, y, z) = rgbToXyzMatrix.Multiply(rgbLinearMatrix).ToTriplet();
        return new Xyz(x, y, z, ColourHeritage.From(rgbLinear));
    }
}