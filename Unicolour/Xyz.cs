namespace Wacton.Unicolour;

public record Xyz
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public ColourTuple Tuple => new(X, Y, Z);

    public Xyz(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString() => $"{Math.Round(X, 2)} {Math.Round(Y, 2)} {Math.Round(Z, 2)}";
}