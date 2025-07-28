namespace Wacton.Unicolour;

public record Wxy : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double W => First;
    public double X => Second;
    public double Y => Third;
    
    public double DominantWavelength => W;
    public double ExcitationPurity => X;
    public double Luminance => Y;
    
    internal override bool IsGreyscale => ExcitationPurity <= 0.0 || Luminance <= 0.0;
    
    public Wxy(double w, double x, double y) : this(w, x, y, ColourHeritage.None) {}
    internal Wxy(double w, double x, double y, ColourHeritage heritage) : base(w, x, y, heritage) { }
    
    protected override string String => UseAsHued ? $"{W:F1}nm {X * 100:F1}% {Y:F4}%" : $"—nm {X * 100:F1}% {Y:F4}%";
    public override string ToString() => base.ToString();
    
    /*
     * WXY is a transform of XYY 
     * Forward: n/a - my own concoction
     * Reverse: n/a - my own concoction
     */
    
    internal static Wxy FromXyy(Xyy xyy, XyzConfiguration xyzConfig)
    {
        var chromaticity = xyy.Chromaticity;
        var luminance = xyy.Luminance;
        
        var intersects = xyy.UseAsNaN || xyy.UseAsGreyscale
            ? null
            : xyzConfig.SpectralBoundary.FindIntersects(chromaticity);

        var w = intersects?.DominantWavelength ?? SpectralBoundary.MinWavelength;
        var x = intersects?.ExcitationPurity ?? 0;
        var y = luminance;
        return new Wxy(w, x, y, ColourHeritage.From(xyy));
    }
    
    internal static Xyy ToXyy(Wxy wxy, XyzConfiguration xyzConfig)
    {
        var (wavelength, purity, luminance) = wxy;
        
        Chromaticity chromaticity;
        if (wxy.UseAsNaN)
        {
            chromaticity = new(double.NaN, double.NaN);
        }
        else if (wxy.UseAsGreyscale)
        {
            chromaticity = xyzConfig.WhiteChromaticity;
        }
        else
        {
            chromaticity = xyzConfig.SpectralBoundary.GetChromaticity(wavelength, purity);
        }

        return new Xyy(chromaticity.X, chromaticity.Y, luminance, ColourHeritage.From(wxy));
    }
    
    internal static double WavelengthToDegree(double wavelength, XyzConfiguration xyzConfig)
    {
        if (double.IsNaN(wavelength)) return double.NaN;

        double degree;
        if (wavelength >= 0)
        {
            var (min, max) = (SpectralBoundary.MinWavelength, SpectralBoundary.MaxWavelength);
            var clamped = wavelength.Clamp(min, max);
            var normalised = (clamped - min) / (max - min);
            degree = normalised * 180;
        }
        else
        {
            var (min, max) = (xyzConfig.SpectralBoundary.MinNegativeWavelength, xyzConfig.SpectralBoundary.MaxNegativeWavelength);
            var clamped = wavelength.Clamp(min, max);
            var normalised = 1 - (clamped - min) / (max - min);
            degree = 180 + normalised * 180;
        }

        return degree;
    } 
    
    internal static double DegreeToWavelength(double degree, XyzConfiguration xyzConfig)
    {
        if (double.IsNaN(degree)) return double.NaN;

        double wavelength;
        if (degree <= 180)
        {
            var (min, max) = (SpectralBoundary.MinWavelength, SpectralBoundary.MaxWavelength);
            var normalised = degree / 180.0;
            wavelength = normalised * (max - min) + min;
        }
        else
        {
            var (min, max) = (xyzConfig.SpectralBoundary.MinNegativeWavelength, xyzConfig.SpectralBoundary.MaxNegativeWavelength);
            var normalised = 1 - (degree - 180) / 180.0;
            wavelength = normalised * (max - min) + min;
        }

        return wavelength;
    }
}