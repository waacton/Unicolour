namespace Wacton.Unicolour;

public record Xyz : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double X => First;
    public double Y => Second;
    public double Z => Third;

    internal WhitePoint WhitePoint { get; }
    protected override bool IsAchromatic
    {
        get
        {
            if (Y == 0.0)
            {
                return X == 0.0 && Z == 0.0;
            }
            
            var yFactor = WhitePoint.Y / Y;
            var scaledX = X * yFactor;
            var scaledZ = Z * yFactor;
            return scaledX == WhitePoint.X && scaledZ == WhitePoint.Z;
        }
    }

    public Xyz(double x, double y, double z, WhitePoint whitePoint) : this(x, y, z, whitePoint, Limitation.None) {}
    public Xyz(double luminance, WhitePoint whitePoint) : this(whitePoint.X * luminance, luminance, whitePoint.Z * luminance, whitePoint, Limitation.Achromatic) {}
    internal Xyz(ColourTriplet triplet, WhitePoint whitePoint, Limitation limitation) : this(triplet.First, triplet.Second, triplet.Third, whitePoint, limitation) {}
    internal Xyz(double x, double y, double z, WhitePoint whitePoint, Limitation limitation) : base(x, y, z, limitation)
    {
        WhitePoint = whitePoint;
    }
    
    protected override string String => $"{X:F4} {Y:F4} {Z:F4}";
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
     * ----------
     * SPD white point calculation uses 360 nm - 780 nm range
     * following ASTM standard practice https://doi.org/10.1520/E0308-18 (7.1.2) for 1 nm or 5 nm intervals [TODO: check if different in E308-22]
     * including approximation of integration using summation and limiting to range 360 - 780 nm
     * (10 nm and 20 nm intervals require different procedures, see 7.3.2.1, 7.3.3.1 - using tables and specific interpolation)
     * intervals other than 1 nm or 5 nm will work but the SPD will report as not valid
     * ----------
     * note that SPD coefficients can cover a wider range than 360 nm - 780 nm
     * e.g.
     * - Illuminant D starts from 300 nm; < 360 nm values are ignored
     * - Illuminant F starts from 380 nm; 360 - 380 nm values are treated as 0
     */
    
    internal const int StartWavelength = 360;
    private const int EndWavelength = 780;
    internal const int WavelengthCount = EndWavelength - StartWavelength + 1;
    private static readonly int[] TargetWavelengths = Enumerable.Range(StartWavelength, WavelengthCount).ToArray();

    internal static Xyz FromSpd(Spd spd, Observer observer, WhitePoint whitePoint)
    {
        var (x, y, z) = FromSpd(spd, observer);
        return new Xyz(x, y, z, whitePoint);
    }
    
    // this is separated out to allow Spd -> Xyz conversion without white point context
    // only intended for use to define actual white points (which themselves are the context!)
    internal static (double x, double y, double z) FromSpd(Spd spd, Observer observer)
    {
        var (wavelengths, delta) = GetWavelengths(spd);
        var absoluteX = GetAbsolute(spd, observer.ColourMatchX, wavelengths, delta);
        var absoluteY = GetAbsolute(spd, observer.ColourMatchY, wavelengths, delta);
        var absoluteZ = GetAbsolute(spd, observer.ColourMatchZ, wavelengths, delta);
        var x = absoluteX / absoluteY;
        var y = absoluteY / absoluteY;
        var z = absoluteZ / absoluteY;
        return (x, y, z);
    }
    
    internal static Xyz FromSpd(Spd spd, Observer observer, SpectralCoefficients reflectance, WhitePoint whitePoint)
    {
        var (wavelengths, delta) = GetWavelengths(spd);
        var absoluteX = GetAbsoluteUsingReflectance(spd, observer.ColourMatchX, reflectance, wavelengths, delta);
        var absoluteY = GetAbsoluteUsingReflectance(spd, observer.ColourMatchY, reflectance, wavelengths, delta);
        var absoluteZ = GetAbsoluteUsingReflectance(spd, observer.ColourMatchZ, reflectance, wavelengths, delta);
        var perfectReflectanceY = GetAbsolute(spd, observer.ColourMatchY, wavelengths, delta);
        var x = absoluteX / perfectReflectanceY;
        var y = absoluteY / perfectReflectanceY;
        var z = absoluteZ / perfectReflectanceY;
        return new Xyz(x, y, z, whitePoint);
    }

    private static (int[] wavelengths, int delta) GetWavelengths(Spd spd)
    {
        // interval of zero indicates SPD of a single wavelength
        // this is handled by all assuming all other wavelengths were implicitly measured with zero power
        var interval = spd.Interval;
        return interval == 0 
            ? (TargetWavelengths, 1)
            : (TargetWavelengths.Where(x => x % interval == 0).ToArray(), interval);
    }
    
    private static double GetAbsolute(Spd spd, Func<int, double> cmf, int[] wavelengths, int delta)
    {
        return GetAbsolute(Power, cmf, wavelengths, delta);
        double Power(int wavelength) => GetPower(spd, wavelength);
    }
    
    internal static double GetAbsolute(Func<int, double> power, Func<int, double> cmf, int[] wavelengths, int delta)
    {
        return GetAbsoluteUsingReflectance(power, cmf, PerfectReflectance, wavelengths, delta);
        double PerfectReflectance(int wavelength) => 1.0;
    }
    
    private static double GetAbsoluteUsingReflectance(Spd spd, Func<int, double> cmf, SpectralCoefficients reflectance, int[] wavelengths, int delta)
    {
        return GetAbsoluteUsingReflectance(Power, cmf, Reflectance, wavelengths, delta);
        double Power(int wavelength) => GetPower(spd, wavelength);
        double Reflectance(int wavelength) => GetReflectance(reflectance, wavelength);
    }
    
    private static double GetAbsoluteUsingReflectance(Func<int, double> power, Func<int, double> cmf, Func<int, double> reflectance, int[] wavelengths, int delta)
    {
        return wavelengths.Sum(wavelength => power(wavelength) * cmf(wavelength) * reflectance(wavelength) * delta);
    }
    
    private static double GetPower(Spd spd, int wavelength) => spd.Get(wavelength, MissingWavelength.Zero);
    private static double GetReflectance(SpectralCoefficients reflectance, int wavelength) => reflectance.Get(wavelength, MissingWavelength.Interpolate);
    
    // only for potential debugging or diagnostics
    // until there is an "official" HCT -> XYZ reverse transform
    internal HctToXyzSearchResult? HctToXyzSearchResult;
}