namespace Wacton.Unicolour;

public class RgbConfiguration
{
    public static readonly RgbConfiguration StandardRgb = RgbModels.StandardRgb.RgbConfiguration;
    public static readonly RgbConfiguration DisplayP3 = RgbModels.DisplayP3.RgbConfiguration;
    public static readonly RgbConfiguration Rec2020 = RgbModels.Rec2020.RgbConfiguration;
    public static readonly RgbConfiguration Rec2100Pq = RgbModels.Rec2100Pq.RgbConfiguration;
    public static readonly RgbConfiguration Rec2100Hlg = RgbModels.Rec2100Hlg.RgbConfiguration;
    public static readonly RgbConfiguration A98 = RgbModels.A98.RgbConfiguration;
    public static readonly RgbConfiguration ProPhoto = RgbModels.ProPhoto.RgbConfiguration;
    public static readonly RgbConfiguration Aces20651 = RgbModels.Aces20651.RgbConfiguration;
    public static readonly RgbConfiguration Acescg = RgbModels.Acescg.RgbConfiguration;
    public static readonly RgbConfiguration Acescct = RgbModels.Acescct.RgbConfiguration;
    public static readonly RgbConfiguration Acescc = RgbModels.Acescc.RgbConfiguration;
    public static readonly RgbConfiguration Rec601Line625 = RgbModels.Rec601Line625.RgbConfiguration;
    public static readonly RgbConfiguration Rec601Line525 = RgbModels.Rec601Line525.RgbConfiguration;
    public static readonly RgbConfiguration Rec709 = RgbModels.Rec709.RgbConfiguration;
    public static readonly RgbConfiguration XvYcc = RgbModels.XvYcc.RgbConfiguration;
    public static readonly RgbConfiguration Pal = RgbModels.Pal.RgbConfiguration;
    public static readonly RgbConfiguration PalM = RgbModels.PalM.RgbConfiguration;
    public static readonly RgbConfiguration Pal625 = RgbModels.Pal625.RgbConfiguration;
    public static readonly RgbConfiguration Pal525 = RgbModels.Pal525.RgbConfiguration;
    public static readonly RgbConfiguration Ntsc = RgbModels.Ntsc.RgbConfiguration;
    public static readonly RgbConfiguration NtscSmpteC = RgbModels.NtscSmpteC.RgbConfiguration;
    public static readonly RgbConfiguration Ntsc525 = RgbModels.Ntsc525.RgbConfiguration;
    public static readonly RgbConfiguration Secam = RgbModels.Secam.RgbConfiguration;
    public static readonly RgbConfiguration Secam625 = RgbModels.Secam625.RgbConfiguration;
    
    public Chromaticity ChromaticityR { get; }
    public Chromaticity ChromaticityG { get; }
    public Chromaticity ChromaticityB { get; }
    public WhitePoint WhitePoint { get; }
    public Func<double, DynamicRange, double> FromLinear { get; }
    public Func<double, DynamicRange, double> ToLinear { get; }
    
    private readonly Lazy<Matrix> rgbToXyzMatrix;
    internal Matrix RgbToXyzMatrix => rgbToXyzMatrix.Value;
    
    public string Name { get; }

    public RgbConfiguration(
        Chromaticity chromaticityR, 
        Chromaticity chromaticityG, 
        Chromaticity chromaticityB,
        WhitePoint whitePoint,
        Func<double, double> fromLinear, 
        Func<double, double> toLinear,
        string name = Utils.Unnamed) 
        : this(chromaticityR, chromaticityG, chromaticityB, whitePoint, (x, _) => fromLinear(x), (x, _) => toLinear(x), name)
    {
    }
    
    internal RgbConfiguration(
        Chromaticity chromaticityR, 
        Chromaticity chromaticityG, 
        Chromaticity chromaticityB,
        WhitePoint whitePoint,
        Func<double, DynamicRange, double> fromLinear, 
        Func<double, DynamicRange, double> toLinear,
        string name = Utils.Unnamed)
    {
        ChromaticityR = chromaticityR;
        ChromaticityG = chromaticityG;
        ChromaticityB = chromaticityB;
        WhitePoint = whitePoint;
        FromLinear = fromLinear;
        ToLinear = toLinear;
        rgbToXyzMatrix = new Lazy<Matrix>(GetRgbToXyzMatrix);
        Name = name;
    }
    
    // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
    private Matrix GetRgbToXyzMatrix()
    {
        var cr = ChromaticityR;
        var cg = ChromaticityG;
        var cb = ChromaticityB;

        double X(Chromaticity c) => c.X / c.Y;
        double Y(Chromaticity c) => 1;
        double Z(Chromaticity c) => (1 - c.X - c.Y) / c.Y;

        var (xr, yr, zr) = (X(cr), Y(cr), Z(cr));
        var (xg, yg, zg) = (X(cg), Y(cg), Z(cg));
        var (xb, yb, zb) = (X(cb), Y(cb), Z(cb));

        var fromPrimaries = new Matrix(new[,]
        {
            { xr, xg, xb },
            { yr, yg, yb },
            { zr, zg, zb }
        });
        
        var xyzWhite = WhitePoint.AsXyzMatrix();
        var (sr, sg, sb) = fromPrimaries.Inverse().Multiply(xyzWhite).ToTriplet();

        return new Matrix(new[,]
        {
            { sr * xr, sg * xg, sb * xb },
            { sr * yr, sg * yg, sb * yb },
            { sr * zr, sg * zg, sb * zb }
        });
    }

    public override string ToString() => $"{Name} · R {ChromaticityR} · G {ChromaticityG} · B {ChromaticityB} · white point {WhitePoint}";
}