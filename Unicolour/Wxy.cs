using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Wxy : ColourTriplet
{
    public double W => this[0];
    public double X => this[1];
    public double Y => this[2];
    
    public Wxy(double w, double x, double y) : this(w, x, y, Limitation.None) {}
    public Wxy(double y) : this(SpectralBoundary.MinWavelength, 0, y, Limitation.Achromatic) {}
    internal Wxy(double w, double x, double y, Limitation limitation) : base(w, x, y, 0, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{W:F1}nm {X * 100:F1}% {Y:F4}" : $"{NoHue}nm {X * 100:F1}% {Y:F4}";
    public override string ToString() => base.ToString();
    
    /*
     * WXY is a transform of XYY 
     * Forward: n/a - my own concoction
     * Reverse: n/a - my own concoction
     */
    
    internal static Wxy FromXyy(Xyy xyy, SpectralBoundary spectralBoundary)
    {
        var chromaticity = xyy.Chromaticity;
        var y = xyy.Luminance;
        
        var (w, x) = xyy.IsNaN 
            ? (double.NaN, double.NaN) 
            : spectralBoundary.GetWavelengthAndPurity(chromaticity) ?? (SpectralBoundary.MinWavelength, 0);

        return new Wxy(w, x, y, xyy.Limitation);
    }
    
    internal static Xyy ToXyy(Wxy wxy, SpectralBoundary spectralBoundary)
    {
        var (wavelength, purity, luminance) = wxy;
        
        var (x, y) = wxy.IsNaN 
            ? new(double.NaN, double.NaN) 
            : spectralBoundary.GetChromaticity(wavelength, purity);
        
        return new Xyy(x, y, luminance, spectralBoundary.WhitePoint, wxy.Limitation);
    }
}