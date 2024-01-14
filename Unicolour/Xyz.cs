namespace Wacton.Unicolour;

public record Xyz : ColourRepresentation
{
    protected override int? HueIndex => null;
    public double X => First;
    public double Y => Second;
    public double Z => Third;
    
    // no clear luminance upper-bound; usually Y >= 1 is max luminance
    // but since custom white points can be provided, don't want to make the assumption
    internal override bool IsGreyscale => Y <= 0;

    public Xyz(double x, double y, double z) : this(x, y, z, ColourHeritage.None) {}
    internal Xyz(ColourTriplet triplet, ColourHeritage heritage) : this(triplet.First, triplet.Second, triplet.Third, heritage) {}
    internal Xyz(double x, double y, double z, ColourHeritage heritage) : base(x, y, z, heritage) {}

    protected override string FirstString => $"{X:F4}";
    protected override string SecondString => $"{Y:F4}";
    protected override string ThirdString => $"{Z:F4}";
    public override string ToString() => base.ToString();
    
    /*
     * XYZ is considered the root colour representation (in terms of Unicolour implementation)
     * so does not contain any transformations to/from another colour space
     * however, XYZ can be computed from a spectral power distribution
     * ----------
     * XYZ is a transform of SPD
     * Forward: https://en.wikipedia.org/wiki/CIE_1931_color_space#Computing_XYZ_from_spectral_data
     * Reverse: n/a
     *          - infinite SPD possibilities from single XYZ (metamerism: https://en.wikipedia.org/wiki/Metamerism_(color))
     *          - although potentially https://doi.org/10.1111/cgf.12676 calculates one good SPD (roundtrip conversion obviously not possible)
     */
    
    // TODO: check if different in E308-22
    // follows ASTM standard practice https://doi.org/10.1520/E0308-18
    // including integration approximation with summation and limiting to range 360 - 780 nm
    internal static Xyz FromSpd(Spd spd, Observer observer)
    {
        var delta = spd.WavelengthDelta;
        var wavelengths = Spd.ExpectedWavelengths(interval: delta);
        var xSum = wavelengths.Sum(wavelength => spd.SpectralPower(wavelength) * observer.ColourMatchX(wavelength) * delta);
        var ySum = wavelengths.Sum(wavelength => spd.SpectralPower(wavelength) * observer.ColourMatchY(wavelength) * delta);
        var zSum = wavelengths.Sum(wavelength => spd.SpectralPower(wavelength) * observer.ColourMatchZ(wavelength) * delta);

        var k = 100 / ySum;
        var x = xSum * k;
        var y = ySum * k;
        var z = zSum * k;
        
        return new Xyz(x / 100.0, y / 100.0, z / 100.0);
    }

    // only for potential debugging or diagnostics
    // until there is an "official" HCT -> XYZ reverse transform
    internal HctToXyzSearchResult? HctToXyzSearchResult;
}