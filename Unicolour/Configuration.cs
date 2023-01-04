namespace Wacton.Unicolour;

public class Configuration
{
    public Chromaticity ChromaticityR { get; }
    public Chromaticity ChromaticityG { get; }
    public Chromaticity ChromaticityB { get; }
    public WhitePoint RgbWhitePoint { get; }
    public WhitePoint XyzWhitePoint { get; }
    public Chromaticity ChromaticityWhite { get; }
    public Func<double, double> Compand { get; }
    public Func<double, double> InverseCompand { get; }
    
    // default is sRGB model (defined by these chromaticities, illuminant/observer, and sRGB linear correction)
    // and will transform into D65-based XYZ colour space
    public static readonly Configuration Default = new(
        Chromaticity.StandardRgbR,
        Chromaticity.StandardRgbG, 
        Chromaticity.StandardRgbB,
        Companding.StandardRgb,
        Companding.InverseStandardRgb, 
        WhitePoint.From(Illuminant.D65), 
        WhitePoint.From(Illuminant.D65));

    public Configuration(Chromaticity chromaticityR, Chromaticity chromaticityG, Chromaticity chromaticityB,
        Func<double, double> compand, Func<double, double> inverseCompand, 
        WhitePoint rgbWhitePoint, WhitePoint xyzWhitePoint)
    {
        ChromaticityR = chromaticityR;
        ChromaticityG = chromaticityG;
        ChromaticityB = chromaticityB;
        Compand = compand;
        InverseCompand = inverseCompand;
        RgbWhitePoint = rgbWhitePoint;
        XyzWhitePoint = xyzWhitePoint;
        
        var x = XyzWhitePoint.X / 100.0;
        var y = XyzWhitePoint.Y / 100.0;
        var z = XyzWhitePoint.Z / 100.0;
        var normalisation = x + y + z;
        ChromaticityWhite = new(x / normalisation, y / normalisation);
    }

    public override string ToString() => $"RGB {RgbWhitePoint} {ChromaticityR} {ChromaticityG} {ChromaticityB} <-> XYZ {XyzWhitePoint} ";
}