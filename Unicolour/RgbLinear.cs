namespace Wacton.Unicolour;

public record RgbLinear : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double R => First;
    public double G => Second;
    public double B => Third;
    
    protected override bool IsAchromatic => R == G && G == B;
    
    public RgbLinear(double r, double g, double b) : this(r, g, b, Limitation.None) {}
    public RgbLinear(double grey) : this(grey, grey, grey, Limitation.Achromatic) {}
    internal RgbLinear(double r, double g, double b, Limitation limitation) : base(r, g, b, limitation) {}
    
    protected override string String => $"{R:F2} {G:F2} {B:F2}";
    public override string ToString() => base.ToString();
    
    /*
     * RGB Linear is a transform of XYZ
     * Forward: https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
     * Reverse: https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
     */
    
    internal static RgbLinear FromXyz(Xyz xyz, RgbConfiguration rgbConfig, ChromaticAdaptor chromaticAdaptor)
    {
        // note that http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html has precalculated XYZ to RGB matrices
        // which is the equivalent of CAT(RgbToXyz, RgbWhite, XyzWhite).Inverse()
        // but this code multiplies matrices in a different order; mathematically the same, just those matrices won't be seen here
        var adaptedXyz = chromaticAdaptor.AdaptTo(xyz, rgbConfig.WhitePoint);
        var adaptedXyzMatrix = Matrix.From(adaptedXyz);
        var rgbMatrix = rgbConfig.RgbToXyzMatrix.Inverse().Multiply(adaptedXyzMatrix);
        var (r, g, b) = rgbMatrix.ToTriplet();
        return new RgbLinear(r, g, b, xyz.Limitation);
    }
    
    internal static Xyz ToXyz(RgbLinear rgbLinear, RgbConfiguration rgbConfig, ChromaticAdaptor chromaticAdaptor)
    {
        // note that http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html has precalculated RGB to XYZ matrices
        // which is the equivalent of CAT(RgbToXyz, RgbWhite, XyzWhite)
        // but this code multiplies matrices in a different order; mathematically the same, just those matrices won't be seen here
        var rgbLinearMatrix = Matrix.From(rgbLinear);
        var adaptedXyzMatrix = rgbConfig.RgbToXyzMatrix.Multiply(rgbLinearMatrix);
        var adaptedXyz = new Xyz(adaptedXyzMatrix.ToTriplet(), rgbConfig.WhitePoint, rgbLinear.Limitation);
        return chromaticAdaptor.AdaptFrom(adaptedXyz);
    }
}