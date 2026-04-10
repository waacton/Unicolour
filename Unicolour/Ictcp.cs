namespace Wacton.Unicolour;

public record Ictcp : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double I => First;
    public double Ct => Second;
    public double Cp => Third;
    
    protected override bool IsAchromatic => Ct == 0.0 && Cp == 0.0;
    
    public Ictcp(double i, double ct, double cp) : this(i, ct, cp, Limitation.None) {}
    public Ictcp(double i) : this(i, 0, 0, Limitation.Achromatic) {}
    internal Ictcp(ColourTriplet triplet, Limitation limitation) : this(triplet.First, triplet.Second, triplet.Third, limitation) {}
    internal Ictcp(double i, double ct, double cp, Limitation limitation) : base(i, ct, cp, limitation) {}

    protected override string String => $"{I:F2} {Ct:+0.00;-0.00;0.00} {Cp:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * ICTCP is a transform of XYZ 
     * Forward: https://professional.dolby.com/siteassets/pdfs/dolby-vision-measuring-perceptual-color-volume-v7.1.pdf
     * Reverse: not specified in the above paper; implementation unit tested to confirm roundtrip conversion
     * -------
     * currently only support PQ transfer function, not HLG (https://en.wikipedia.org/wiki/Hybrid_log%E2%80%93gamma)
     */
    
    private static readonly WhitePoint D65WhitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);

    private static readonly Matrix M1 = new(new[,]
    {
        { 0.3593, 0.6976, -0.0359 },
        { -0.1921, 1.1005, 0.0754 },
        { 0.0071, 0.0748, 0.8433 }
    });

    private static readonly Matrix M2 = new Matrix(new double[,]
    {
        { 2048, 2048, 0 },
        { 6610, -13613, 7003 },
        { 17933, -17390, -543 }
    }).Scale(1 / 4096.0);
    
    internal static Ictcp FromXyz(Xyz xyz, ChromaticAdaptor chromaticAdaptor, DynamicRange dynamicRange)
    {
        var d65Xyz = chromaticAdaptor.AdaptTo(xyz, D65WhitePoint);
        var d65Matrix = Matrix.From(d65Xyz);
        var lmsMatrix = M1.Multiply(d65Matrix);
        var lmsPrimeMatrix = lmsMatrix.Select(value => Pq.Smpte.InverseEotf(value, dynamicRange.WhiteLuminance));
        var ictcpMatrix = M2.Multiply(lmsPrimeMatrix);
        return new Ictcp(ictcpMatrix.ToTriplet(), xyz.Limitation);
    }
    
    internal static Xyz ToXyz(Ictcp ictcp, ChromaticAdaptor chromaticAdaptor, DynamicRange dynamicRange)
    {
        var ictcpMatrix = Matrix.From(ictcp);
        var lmsPrimeMatrix = M2.Inverse().Multiply(ictcpMatrix);
        var lmsMatrix = lmsPrimeMatrix.Select(value => Pq.Smpte.Eotf(value, dynamicRange.WhiteLuminance));
        var d65Matrix = M1.Inverse().Multiply(lmsMatrix);
        var d65Xyz = new Xyz(d65Matrix.ToTriplet(), D65WhitePoint, ictcp.Limitation);
        return chromaticAdaptor.AdaptFrom(d65Xyz);
    }
}