using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Wxy : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double W => First;
    public double X => Second;
    public double Y => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Wxy(double w, double x, double y) : this(w, x, y, Limitation.None) {}
    internal Wxy(double w, double x, double y, Limitation limitation) : base(w, x, y, limitation) { }

    protected override string String => Limitation != Limitation.Achromatic ? $"{W:F1}nm {X * 100:F1}% {Y:F4}%" : $"{NoHue}nm {X * 100:F1}% {Y:F4}%";
    public override string ToString() => base.ToString();
    
    /*
     * WXY is a transform of XYY 
     * Forward: n/a - my own concoction
     * Reverse: n/a - my own concoction
     */
    
    internal static Wxy FromXyy(Xyy xyy, SpectralBoundary spectralBoundary)
    {
        var chromaticity = xyy.Chromaticity;
        var luminance = xyy.Luminance;
        
        var result = xyy.Limitation is Limitation.NaN or Limitation.Achromatic
            ? null
            : spectralBoundary.GetWavelengthAndPurity(chromaticity);

        var w = result?.wavelength ?? SpectralBoundary.MinWavelength;
        var x = result?.purity ?? 0;
        var y = luminance;
        return new Wxy(w, x, y, xyy.Limitation);
    }
    
    internal static Xyy ToXyy(Wxy wxy, SpectralBoundary spectralBoundary)
    {
        var (wavelength, purity, luminance) = wxy;
        
        var chromaticity = wxy.Limitation switch
        {
            Limitation.NaN => new Chromaticity(double.NaN, double.NaN),
            Limitation.Achromatic => spectralBoundary.WhitePoint.Chromaticity,
            _ => spectralBoundary.GetChromaticity(wavelength, purity)
        };

        return new Xyy(chromaticity.X, chromaticity.Y, luminance, spectralBoundary.WhitePoint, wxy.Limitation);
    }
    
    internal static double WavelengthToDegree(double wavelength, SpectralBoundary spectralBoundary)
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
            var (min, max) = (spectralBoundary.MinNegativeWavelength, spectralBoundary.MaxNegativeWavelength);
            var clamped = wavelength.Clamp(min, max);
            var normalised = 1 - (clamped - min) / (max - min);
            degree = 180 + normalised * 180;
        }

        return degree;
    } 
    
    internal static double DegreeToWavelength(double degree, SpectralBoundary spectralBoundary)
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
            var (min, max) = (spectralBoundary.MinNegativeWavelength, spectralBoundary.MaxNegativeWavelength);
            var normalised = 1 - (degree - 180) / 180.0;
            wavelength = normalised * (max - min) + min;
        }

        return wavelength;
    }
}