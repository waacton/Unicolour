namespace Wacton.Unicolour;

public record Lab(double L, double A, double B)
{
    public double L { get; } = L;
    public double A { get; } = A;
    public double B { get; } = B;
    public ColourTuple Tuple => new(L, A, B);

    public override string ToString()
    {
        var prefixA = A > 0 ? "+" : string.Empty;
        var prefixB = B > 0 ? "+" : string.Empty;
        return $"{Math.Round(L, 2)} {prefixA}{Math.Round(A, 2)} {prefixB}{Math.Round(B, 2)}";
    }
}