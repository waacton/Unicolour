namespace Wacton.Unicolour;

public record WhitePoint(double X, double Y, double Z)
{
    public double X { get; } = X;
    public double Y { get; } = Y;
    public double Z { get; } = Z;
    internal Matrix AsXyzMatrix() => Matrix.FromTriplet(X, Y, Z).Select(x => x / 100.0);

    public static WhitePoint FromXyz(Xyz xyz) => new(xyz.X * 100, xyz.Y * 100, xyz.Z * 100);
    
    public override string ToString() => $"({X}, {Y}, {Z})";
}