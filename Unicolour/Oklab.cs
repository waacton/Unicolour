namespace Wacton.Unicolour;

using static Utils;

public record Oklab : ColourRepresentation
{
    protected override int? HueIndex => null;
    public double L => First;
    public double A => Second;
    public double B => Third;
    internal override bool IsGreyscale => A.Equals(0.0) && B.Equals(0.0);

    public Oklab(double l, double a, double b) : this(l, a, b, ColourMode.Unset) {}
    internal Oklab(ColourTriplet triplet, ColourMode colourMode) : this(triplet.First, triplet.Second, triplet.Third, colourMode) {}
    internal Oklab(double l, double a, double b, ColourMode colourMode) : base(l, a, b, colourMode) {}

    protected override string FirstString => $"{L:F2}";
    protected override string SecondString => $"{A:+0.00;-0.00;0.00}";
    protected override string ThirdString => $"{B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * OKLAB is a transform of XYZ 
     * Forward: https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab
     * Reverse: https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab
     */
    
    internal static Oklab FromXyz(Xyz xyz, XyzConfiguration xyzConfig)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var d65Matrix = Matrices.AdaptForWhitePoint(xyzMatrix, xyzConfig.WhitePoint, WhitePoint.From(Illuminant.D65));
        var lmsMatrix = Matrices.OklabM1.Multiply(d65Matrix);
        var lmsNonLinearMatrix = lmsMatrix.Scalar(CubeRoot);
        var labMatrix = Matrices.OklabM2.Multiply(lmsNonLinearMatrix);
        return new Oklab(labMatrix.ToTriplet(), ColourMode.FromRepresentation(xyz));
    }
    
    internal static Xyz ToXyz(Oklab oklab, XyzConfiguration xyzConfig)
    {
        var labMatrix = Matrix.FromTriplet(oklab.Triplet);
        var lmsNonLinearMatrix = Matrices.OklabM2.Inverse().Multiply(labMatrix);
        var lmsMatrix = lmsNonLinearMatrix.Scalar(x => Math.Pow(x, 3));
        var d65Matrix = Matrices.OklabM1.Inverse().Multiply(lmsMatrix);
        var xyzMatrix = Matrices.AdaptForWhitePoint(d65Matrix, WhitePoint.From(Illuminant.D65), xyzConfig.WhitePoint);
        return new Xyz(xyzMatrix.ToTriplet(), ColourMode.FromRepresentation(oklab));
    }
}