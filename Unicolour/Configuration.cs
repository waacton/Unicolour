namespace Wacton.Unicolour;

public class Configuration
{
    public Chromaticity ChromaticityR { get; }
    public Chromaticity ChromaticityG { get; }
    public Chromaticity ChromaticityB { get; }
    public WhitePoint RgbWhitePoint { get; }
    public WhitePoint XyzWhitePoint { get; }
    public Func<double, double> InverseCompanding { get; }
    
    internal Matrix RgbToXyzMatrix { get; }
    
    // default is sRGB model (defined by these chromaticities, illuminant/observer, and sRGB linear correction)
    // and will transform into D65-based XYZ colour space
    public static readonly Configuration Default = new(
        Chromaticity.StandardRgbR,
        Chromaticity.StandardRgbG, 
        Chromaticity.StandardRgbB,
        Companding.InverseStandardRgb, 
        WhitePoint.From(Illuminant.D65), 
        WhitePoint.From(Illuminant.D65));

    public Configuration(Chromaticity chromaticityR, Chromaticity chromaticityG, Chromaticity chromaticityB,
        Func<double, double> inverseCompanding, WhitePoint rgbWhitePoint, WhitePoint xyzWhitePoint)
    {
        ChromaticityR = chromaticityR;
        ChromaticityG = chromaticityG;
        ChromaticityB = chromaticityB;
        InverseCompanding = inverseCompanding;
        RgbWhitePoint = rgbWhitePoint;
        XyzWhitePoint = xyzWhitePoint;
        RgbToXyzMatrix = Matrices.RgbToXyzMatrix(this);
    }

    public override string ToString() => $"RGB {RgbWhitePoint} {ChromaticityR} {ChromaticityG} {ChromaticityB} -> XYZ {XyzWhitePoint} ";
}

public record Chromaticity(double X, double Y)
{
    public static readonly Chromaticity StandardRgbR = new(0.6400, 0.3300);
    public static readonly Chromaticity StandardRgbG = new(0.3000, 0.6000);
    public static readonly Chromaticity StandardRgbB = new(0.1500, 0.0600);
    
    public double X { get; } = X;
    public double Y { get; } = Y;
    public override string ToString() => $"({X}, {Y})";
}