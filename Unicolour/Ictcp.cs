namespace Wacton.Unicolour;

public record Ictcp : ColourRepresentation
{
    protected override int? HueIndex => null;
    public double I => First;
    public double Ct => Second;
    public double Cp => Third;
    internal override bool IsGreyscale => Ct.Equals(0.0) && Cp.Equals(0.0);
    
    public Ictcp(double i, double ct, double cp) : this(i, ct, cp, ColourMode.Unset) {}
    internal Ictcp(ColourTriplet triplet, ColourMode colourMode) : this(triplet.First, triplet.Second, triplet.Third, colourMode) {}
    internal Ictcp(double i, double ct, double cp, ColourMode colourMode) : base(i, ct, cp, colourMode) {}

    protected override string FirstString => $"{I:F2}";
    protected override string SecondString => $"{Ct:+0.00;-0.00;0.00}";
    protected override string ThirdString => $"{Cp:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * ICTCP is a transform of XYZ 
     * Forward: https://professional.dolby.com/siteassets/pdfs/dolby-vision-measuring-perceptual-color-volume-v7.1.pdf
     * Reverse: not specified in the above paper; implementation unit tested to confirm roundtrip conversion
     * -------
     * currently only support PQ transfer function, not HLG (https://en.wikipedia.org/wiki/Hybrid_log%E2%80%93gamma)
     */
    
    internal static Ictcp FromXyz(Xyz xyz, XyzConfiguration xyzConfig, double ictcpScalar)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var d65Matrix = Matrices.AdaptForWhitePoint(xyzMatrix, xyzConfig.WhitePoint, WhitePoint.From(Illuminant.D65));
        var d65ScaledMatrix = d65Matrix.Scalar(x => x * ictcpScalar);
        var lmsMatrix = Matrices.IctcpM1.Multiply(d65ScaledMatrix);
        var lmsPrimeMatrix = lmsMatrix.Scalar(Pq.Smpte.InverseEotf);
        var ictcpMatrix = Matrices.IctcpM2.Multiply(lmsPrimeMatrix);
        return new Ictcp(ictcpMatrix.ToTriplet(), ColourMode.FromRepresentation(xyz));
    }

    internal static Xyz ToXyz(Ictcp ictcp, XyzConfiguration xyzConfig, double ictcpScalar)
    {
        var ictcpMatrix = Matrix.FromTriplet(ictcp.Triplet);
        var lmsPrimeMatrix = Matrices.IctcpM2.Inverse().Multiply(ictcpMatrix);
        var lmsMatrix = lmsPrimeMatrix.Scalar(Pq.Smpte.Eotf);
        var d65ScaledMatrix = lmsMatrix.Scalar(x => x / ictcpScalar);
        var d65Matrix = Matrices.IctcpM1.Inverse().Multiply(d65ScaledMatrix);
        var xyzMatrix = Matrices.AdaptForWhitePoint(d65Matrix, WhitePoint.From(Illuminant.D65), xyzConfig.WhitePoint);
        return new Xyz(xyzMatrix.ToTriplet(), ColourMode.FromRepresentation(ictcp));
    }
}