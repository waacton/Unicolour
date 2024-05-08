namespace Wacton.Unicolour;

public record RgbLinear : ColourRepresentation
{
    protected override int? HueIndex => null;
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
    internal RgbLinear(ColourTriplet triplet, ColourHeritage heritage) : this(triplet.First, triplet.Second, triplet.Third, heritage) {}
    internal RgbLinear(double r, double g, double b, ColourHeritage heritage) : base(r, g, b, heritage) {}
    
    protected override string FirstString => $"{R:F2}";
    protected override string SecondString => $"{G:F2}";
    protected override string ThirdString => $"{B:F2}";
    public override string ToString() => base.ToString();
    
    /*
     * RGB Linear is a transform of XYZ
     * Forward: https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
     * Reverse: https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
     */
    
    internal static RgbLinear FromXyz(Xyz xyz, RgbConfiguration rgbConfig, XyzConfiguration xyzConfig)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var rgbToXyzMatrix = Adaptation.WhitePoint(rgbConfig.RgbToXyzMatrix, rgbConfig.WhitePoint, xyzConfig.WhitePoint);
        var rgbLinearMatrix = rgbToXyzMatrix.Inverse().Multiply(xyzMatrix);
        return new RgbLinear(rgbLinearMatrix.ToTriplet(), ColourHeritage.From(xyz));
    }
    
    internal static Xyz ToXyz(RgbLinear rgbLinear, RgbConfiguration rgbConfig, XyzConfiguration xyzConfig)
    {
        var rgbLinearMatrix = Matrix.FromTriplet(rgbLinear.Triplet);
        var rgbToXyzMatrix = Adaptation.WhitePoint(rgbConfig.RgbToXyzMatrix, rgbConfig.WhitePoint, xyzConfig.WhitePoint);
        var xyzMatrix = rgbToXyzMatrix.Multiply(rgbLinearMatrix);
        return new Xyz(xyzMatrix.ToTriplet(), ColourHeritage.From(rgbLinear));
    }
}