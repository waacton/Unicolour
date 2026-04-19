namespace Wacton.Unicolour;

public record WhitePoint
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public ColourTriplet Triplet => new(X, Y, Z);
    public Chromaticity Chromaticity { get; }

    internal static WhitePoint FromAstm(double x, double y, double z) => new(x / 100, y / 100, z / 100);
    
    public WhitePoint(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;

        // usually when converting Xyz -> Xyy, the fallback value is the contextual white point
        // (e.g. X, Y, Z = 0 which implies black, convert to D65 white with 0 luminance)
        // however, this IS the white point definition, and a white point of X, Y, Z = 0 does not make sense - so use NaN as fallback
        var fallback = new Chromaticity(double.NaN, double.NaN);
        (Chromaticity, _) = Xyy.FromXyz(X, Y, Z, fallback);
    }
    
    public WhitePoint(double x, double y)
    {
        Chromaticity = new(x, y);
        (X, Y, Z) = Xyy.ToXyz(Chromaticity, 1.0);
    }
    
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