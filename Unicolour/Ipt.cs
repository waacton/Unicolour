namespace Wacton.Unicolour;

public record Ipt : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double I => First;
    public double P => Second;
    public double T => Third;
    
    protected override bool IsAchromatic => P == 0.0 && T == 0.0;
    
    public Ipt(double i, double p, double t) : this(i, p, t, Limitation.None) {}
    internal Ipt(ColourTriplet triplet, Limitation limitation) : this(triplet.First, triplet.Second, triplet.Third, limitation) {}
    internal Ipt(double i, double p, double t, Limitation limitation) : base(i, p, t, limitation) {}
    
    protected override string String => $"{I:F2} {P:+0.00;-0.00;0.00} {T:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * IPT is a transform of XYZ 
     * Forward: https://repository.rit.edu/theses/2858/
     * Reverse: https://repository.rit.edu/theses/2858/
     */
    
    private static readonly WhitePoint D65WhitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);

    private static readonly Matrix M1 = new(new[,]
    {
        { 0.4002, 0.7075, -0.0807 },
        { -0.2280, 1.1500, 0.0612 },
        { 0.0000, 0.0000, 0.9184 }
    });
    
    private static readonly Matrix M2 = new(new[,]
    {
        { 0.4000, 0.4000, 0.2000 },
        { 4.4550, -4.8510, 0.3960 },
        { 0.8056, 0.3572, -1.1628 }
    });
    
    internal static Ipt FromXyz(Xyz xyz, ChromaticAdaptor chromaticAdaptor)
    {
        var d65Xyz = chromaticAdaptor.AdaptTo(xyz, D65WhitePoint);
        var d65Matrix = Matrix.From(d65Xyz);
        var lmsMatrix = M1.Multiply(d65Matrix);
        var lmsPrimeMatrix = lmsMatrix.Select(x => x >= 0 ? Math.Pow(x, 0.43) : -Math.Pow(-x, 0.43));
        var iptMatrix = M2.Multiply(lmsPrimeMatrix);
        return new Ipt(iptMatrix.ToTriplet(), xyz.Limitation);
    }
    
    internal static Xyz ToXyz(Ipt ipt, ChromaticAdaptor chromaticAdaptor)
    {
        var iptMatrix = Matrix.From(ipt);
        var lmsPrimeMatrix = M2.Inverse().Multiply(iptMatrix);
        var lmsMatrix = lmsPrimeMatrix.Select(x => x >= 0 ? Math.Pow(x, 1 / 0.43) : -Math.Pow(-x, 1 / 0.43));
        var d65Matrix = M1.Inverse().Multiply(lmsMatrix);
        var d65Xyz = new Xyz(d65Matrix.ToTriplet(), D65WhitePoint, ipt.Limitation);
        return chromaticAdaptor.AdaptFrom(d65Xyz);
    }
}