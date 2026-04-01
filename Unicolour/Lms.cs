namespace Wacton.Unicolour;

public record Lms : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double L => First;
    public double M => Second;
    public double S => Third;
    
    protected override bool IsAchromatic => L == M && M == S;
    
    public Lms(double l, double m, double s) : this(l, m, s, Limitation.None) {}
    internal Lms(ColourTriplet triplet, Limitation limitation) : this(triplet.First, triplet.Second, triplet.Third, limitation) {}
    internal Lms(double l, double m, double s, Limitation limitation) : base(l, m, s, limitation) {}

    protected override string String => $"{L:F4} {M:F4} {S:F4}";
    public override string ToString() => base.ToString();
    
    /*
     * LMS is a transform of XYZ 
     * Forward: https://en.wikipedia.org/wiki/LMS_color_space#XYZ_to_LMS
     * Reverse: https://en.wikipedia.org/wiki/LMS_color_space#XYZ_to_LMS
     *
     * this is a normalised space where black = (0, 0, 0), white = (1, 1, 1), etc. regardless of XYZ illuminant
     * determining physiological cone responses (i.e. D65 white does trigger equal L, M, S responses) requires spectral data and LMS CMFs
     * but spectral data is almost never present whereas XYZ values always are
     * see also: https://doi.org/10.1002/col.22879 - transforming XYZ CMFs (e.g. x̄) to LMS CMFs (e.g. l-bar)
     */
    
    // aka VonKries from http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
    internal static readonly Matrix HuntPointerEstevez = new(new[,]
    {
        { +0.4002400, +0.7076000, -0.0808100 },
        { -0.2263000, +1.1653200, +0.0457000 },
        { +0.0000000, +0.0000000, +0.9182200 }
    });
    
    // this is the white point that HPE is defined relative to
    // the LMS values themselves are not tied to a specific white point
    // and the XYZ white point ultimately has no impact - black is always (0, 0, 0), white is always (1, 1, 1), etc.
    private static readonly WhitePoint D65WhitePoint = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    
    internal static Lms FromXyz(Xyz xyz, ChromaticAdaptor chromaticAdaptor)
    {
        var d65Xyz = chromaticAdaptor.AdaptTo(xyz, D65WhitePoint);
        var d65Matrix = Matrix.From(d65Xyz);
        var lmsMatrix = HuntPointerEstevez.Multiply(d65Matrix);
        return new Lms(lmsMatrix.ToTriplet(), xyz.Limitation);
    }

    internal static Xyz ToXyz(Lms lms, ChromaticAdaptor chromaticAdaptor)
    {
        var lmsMatrix = Matrix.From(lms);
        var d65Matrix = HuntPointerEstevez.Inverse().Multiply(lmsMatrix);
        var d65Xyz = new Xyz(d65Matrix.ToTriplet(), D65WhitePoint, lms.Limitation);
        return chromaticAdaptor.AdaptFrom(d65Xyz);
    }
}