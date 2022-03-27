namespace Wacton.Unicolour;

public record Xyz(double X, double Y, double Z)
{
    public double X { get; } = X;
    public double Y { get; } = Y;
    public double Z { get; } = Z;
    public ColourTuple Tuple => new(X, Y, Z);

    public override string ToString() => $"{Math.Round(X, 2)} {Math.Round(Y, 2)} {Math.Round(Z, 2)}";
}