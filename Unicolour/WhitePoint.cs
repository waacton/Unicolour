namespace Wacton.Unicolour;

public record WhitePoint(double X, double Y, double Z)
{
    public double X { get; } = X;
    public double Y { get; } = Y;
    public double Z { get; } = Z;

    public static WhitePoint FromXyz(Xyz xyz) => new(xyz.X * 100, xyz.Y * 100, xyz.Z * 100);
    
    internal Matrix AsXyzMatrix() => Matrix.From(X, Y, Z).Select(x => x / 100.0);

    public Chromaticity ToChromaticity()
    {
        var x = X / 100.0;
        var y = Y / 100.0;
        var z = Z / 100.0;
        var normalisation = x + y + z;
        return new(x / normalisation, y / normalisation);
    }
    
    public override string ToString() => $"({X}, {Y}, {Z})";
}