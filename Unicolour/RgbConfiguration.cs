namespace Wacton.Unicolour;

public class RgbConfiguration
{
    public Chromaticity ChromaticityR { get; }
    public Chromaticity ChromaticityG { get; }
    public Chromaticity ChromaticityB { get; }
    public WhitePoint WhitePoint { get; }
    public Func<double, double> CompandFromLinear { get; }
    public Func<double, double> InverseCompandToLinear { get; }
    
    public static readonly RgbConfiguration StandardRgb = new(
        Chromaticity.StandardRgb.R,
        Chromaticity.StandardRgb.G, 
        Chromaticity.StandardRgb.B, 
        WhitePoint.StandardRgb, 
        Companding.StandardRgb.FromLinear, 
        Companding.StandardRgb.ToLinear);
    
    public static readonly RgbConfiguration DisplayP3 = new(
        Chromaticity.DisplayP3.R,
        Chromaticity.DisplayP3.G, 
        Chromaticity.DisplayP3.B, 
        WhitePoint.DisplayP3, 
        Companding.DisplayP3.FromLinear, 
        Companding.DisplayP3.ToLinear);
    
    public static readonly RgbConfiguration Rec2020 = new(
        Chromaticity.Rec2020.R,
        Chromaticity.Rec2020.G, 
        Chromaticity.Rec2020.B, 
        WhitePoint.Rec2020, 
        Companding.Rec2020.FromLinear, 
        Companding.Rec2020.ToLinear);

    public RgbConfiguration(
        Chromaticity chromaticityR, 
        Chromaticity chromaticityG, 
        Chromaticity chromaticityB,
        WhitePoint whitePoint,
        Func<double, double> fromLinear, 
        Func<double, double> toLinear)
    {
        ChromaticityR = chromaticityR;
        ChromaticityG = chromaticityG;
        ChromaticityB = chromaticityB;
        WhitePoint = whitePoint;
        CompandFromLinear = fromLinear;
        InverseCompandToLinear = toLinear;
    }

    public override string ToString() => $"RGB {WhitePoint} {ChromaticityR} {ChromaticityG} {ChromaticityB}";
}