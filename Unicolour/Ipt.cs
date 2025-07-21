namespace Wacton.Unicolour;

public record Ipt : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double I => First;
    public double P => Second;
    public double T => Third;
    
    // no clear lightness upper-bound
    internal override bool IsGreyscale => I <= 0.0 || (P.Equals(0.0) && T.Equals(0.0));
    
    public Ipt(double i, double p, double t) : this(i, p, t, ColourHeritage.None) {}
    internal Ipt(ColourTriplet triplet, ColourHeritage heritage) : this(triplet.First, triplet.Second, triplet.Third, heritage) {}
    internal Ipt(double i, double p, double t, ColourHeritage heritage) : base(i, p, t, heritage) {}

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
    
    internal static Ipt FromXyz(Xyz xyz, XyzConfiguration xyzConfig)
    {
        var xyzMatrix = Matrix.From(xyz);
        var d65Matrix = Adaptation.WhitePoint(xyzMatrix, xyzConfig.WhitePoint, D65WhitePoint, xyzConfig.AdaptationMatrix);
        var lmsMatrix = M1.Multiply(d65Matrix);
        var lmsPrimeMatrix = lmsMatrix.Select(x => x >= 0 ? Math.Pow(x, 0.43) : -Math.Pow(-x, 0.43));
        var iptMatrix = M2.Multiply(lmsPrimeMatrix);
        return new Ipt(iptMatrix.ToTriplet(), ColourHeritage.From(xyz));
    }

    internal static Xyz ToXyz(Ipt ictcp, XyzConfiguration xyzConfig)
    {
        var iptMatrix = Matrix.From(ictcp);
        var lmsPrimeMatrix = M2.Inverse().Multiply(iptMatrix);
        var lmsMatrix = lmsPrimeMatrix.Select(x => x >= 0 ? Math.Pow(x, 1 / 0.43) : -Math.Pow(-x, 1 / 0.43));
        var d65Matrix = M1.Inverse().Multiply(lmsMatrix);
        var xyzMatrix = Adaptation.WhitePoint(d65Matrix, D65WhitePoint, xyzConfig.WhitePoint, xyzConfig.AdaptationMatrix);
        return new Xyz(xyzMatrix.ToTriplet(), ColourHeritage.From(ictcp));
    }
}