namespace Wacton.Unicolour;

public record Rgb : ColourRepresentation
{
    protected override int? HueIndex => null;
    public double R => First;
    public double G => Second;
    public double B => Third;
    public double ConstrainedR => ConstrainedFirst;
    public double ConstrainedG => ConstrainedSecond;
    public double ConstrainedB => ConstrainedThird;
    protected override double ConstrainedFirst => R.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => G.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => B.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => ConstrainedR.Equals(ConstrainedG) && ConstrainedG.Equals(ConstrainedB);

    // for almost all cases, doing this check in linear RGB will return the same result
    // but handling it here feels most natural as it is the intended "display" space
    // and isn't concerned about questionable custom inverse-companding-to-linear functions (e.g. where where RGB <= 1.0 but RGB-Linear > 1.0)
    internal bool IsDisplayable => !UseAsNaN && R is >= 0 and <= 1.0 && G is >= 0 and <= 1.0 && B is >= 0 and <= 1.0;
    public RgbLinear Linear { get; }
    public Rgb255 Byte255 { get; }

    public Rgb(double r, double g, double b, RgbConfiguration rgbConfig) : this(r, g, b, rgbConfig, ColourHeritage.None) {}
    internal Rgb(ColourTriplet triplet, RgbConfiguration rgbConfig, ColourHeritage heritage) : this(triplet.First, triplet.Second, triplet.Third, rgbConfig, heritage) {}
    internal Rgb(double r, double g, double b, RgbConfiguration rgbConfig, ColourHeritage heritage) : base(r, g, b, heritage)
    {
        double ToLinear(double value) => rgbConfig.InverseCompandToLinear(value);
        Linear = new RgbLinear(ToLinear(r), ToLinear(g), ToLinear(b), ColourHeritage.From(this));
        
        double To255(double value) => Math.Round(value * 255);
        Byte255 = new Rgb255(To255(r), To255(g), To255(b), ColourHeritage.From(this));
    }

    protected override string FirstString => $"{R:F2}";
    protected override string SecondString => $"{G:F2}";
    protected override string ThirdString => $"{B:F2}";
    public override string ToString() => base.ToString();
    
    /*
     * RGB is a transform of XYZ 
     * Forward: https://en.wikipedia.org/wiki/SRGB#From_CIE_XYZ_to_sRGB
     * Reverse: https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
     */

    internal static Rgb FromXyz(Xyz xyz, RgbConfiguration rgbConfig, XyzConfiguration xyzConfig)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var transformationMatrix = RgbToXyzMatrix(rgbConfig, xyzConfig).Inverse();
        var rgbLinearMatrix = transformationMatrix.Multiply(xyzMatrix);
        var rgbMatrix = rgbLinearMatrix.Select(rgbConfig.CompandFromLinear);
        return new Rgb(rgbMatrix.ToTriplet(), rgbConfig, ColourHeritage.From(xyz));
    }
    
    internal static Xyz ToXyz(Rgb rgb, RgbConfiguration rgbConfig, XyzConfiguration xyzConfig)
    {
        var rgbLinearMatrix = Matrix.FromTriplet(rgb.Linear.Triplet);
        var transformationMatrix = RgbToXyzMatrix(rgbConfig, xyzConfig);
        var xyzMatrix = transformationMatrix.Multiply(rgbLinearMatrix);
        return new Xyz(xyzMatrix.ToTriplet(), ColourHeritage.From(rgb));
    }
    
    // http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
    internal static Matrix RgbToXyzMatrix(RgbConfiguration rgbConfig, XyzConfiguration xyzConfig)
    {
        var cr = rgbConfig.ChromaticityR;
        var cg = rgbConfig.ChromaticityG;
        var cb = rgbConfig.ChromaticityB;

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
        
        var sourceWhite = rgbConfig.WhitePoint.AsXyzMatrix();
        var (sr, sg, sb) = fromPrimaries.Inverse().Multiply(sourceWhite).ToTriplet();

        var matrix = new Matrix(new[,]
        {
            { sr * xr, sg * xg, sb * xb },
            { sr * yr, sg * yg, sb * yb },
            { sr * zr, sg * zg, sb * zb }
        });

        var adaptedMatrix = Adaptation.WhitePoint(matrix, rgbConfig.WhitePoint, xyzConfig.WhitePoint);
        return adaptedMatrix;
    }
}