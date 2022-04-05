namespace Wacton.Unicolour;

public record Oklab(double L, double A, double B)
{
    public double L { get; } = L;
    public double A { get; } = A;
    public double B { get; } = B;
    public ColourTriplet Triplet => new(L, A, B);

    public override string ToString() => $"{Math.Round(L, 2)} {Math.Round(A, 2)} {Math.Round(B, 2)}";
}