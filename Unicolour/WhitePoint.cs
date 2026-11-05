namespace Wacton.Unicolour;

public record WhitePoint : ColourTriplet
{
    public double X => this[0];
    public double Y => this[1];
    public double Z => this[2];
    public Chromaticity Chromaticity { get; } = null!;

    // white point by definition is white...
    protected override bool HasAchromaticValues => true;
    
    internal static WhitePoint FromAstm(double x, double y, double z) => new(x / 100, y / 100, z / 100);
    
    public WhitePoint(double x, double y, double z) : this(new ColourTriplet(x, y, z))
    {
        // usually when converting Xyz -> Xyy, the fallback value is the contextual white point
        // (e.g. X, Y, Z = 0 which implies black, convert to D65 white with 0 luminance)
        // however, this IS the white point definition, and a white point of X, Y, Z = 0 does not make sense - so use NaN as fallback
        var fallback = new Chromaticity(double.NaN, double.NaN);
        (Chromaticity, _) = Xyy.FromXyz(X, Y, Z, fallback);
    }
    
    public WhitePoint(double x, double y) : this(Xyy.ToXyz(new Chromaticity(x, y), 1.0))
    {
        Chromaticity = new(x, y);
    }

    private WhitePoint(ColourTriplet xyz) : base(xyz.First, xyz.Second, xyz.Third) {}
    
    public void Deconstruct(out double x, out double y, out double z)
    {
        (x, y, z) = (X, Y, Z);
    }
    
    public void Deconstruct(out double x, out double y)
    {
        (x, y) = (Chromaticity.X, Chromaticity.Y);
    }

    public override string ToString() => $"({X:F4}, {Y:F4}, {Z:F4}) · {Chromaticity}";
}