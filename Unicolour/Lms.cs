namespace Wacton.Unicolour;

public record Lms : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double L => First;
    public double M => Second;
    public double S => Third;
    internal override bool IsGreyscale => L.Equals(M) && M.Equals(S);
    
    public Lms(double l, double m, double s) : this(l, m, s, ColourHeritage.None) {}
    internal Lms(ColourTriplet triplet, ColourHeritage heritage) : this(triplet.First, triplet.Second, triplet.Third, heritage) {}
    internal Lms(double l, double m, double s, ColourHeritage heritage) : base(l, m, s, heritage) {}

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
    
    internal static Lms FromXyz(Xyz xyz, XyzConfiguration xyzConfig)
    {
        var xyzMatrix = Matrix.From(xyz);
        var d65Matrix = Adaptation.WhitePoint(xyzMatrix, xyzConfig.WhitePoint, D65WhitePoint, xyzConfig.AdaptationMatrix);
        var lmsMatrix = HuntPointerEstevez.Multiply(d65Matrix);
        return new Lms(lmsMatrix.ToTriplet(), ColourHeritage.From(xyz));
    }

    internal static Xyz ToXyz(Lms lms, XyzConfiguration xyzConfig)
    {
        var lmsMatrix = Matrix.From(lms);
        var d65Matrix = HuntPointerEstevez.Inverse().Multiply(lmsMatrix);
        var xyzMatrix = Adaptation.WhitePoint(d65Matrix, D65WhitePoint, xyzConfig.WhitePoint, xyzConfig.AdaptationMatrix);
        return new Xyz(xyzMatrix.ToTriplet(), ColourHeritage.From(lms));
    }
}