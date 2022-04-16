namespace Wacton.Unicolour;

public record Luv(double L, double U, double V)
{
    public double L { get; } = L;
    public double U { get; } = U;
    public double V { get; } = V;
    public ColourTriplet Triplet => new(L, U, V);

    public override string ToString()
    {
        var prefixU = U > 0 ? "+" : string.Empty;
        var prefixV = V > 0 ? "+" : string.Empty;
        return $"{Math.Round(L, 2)} {prefixU}{Math.Round(U, 2)} {prefixV}{Math.Round(V, 2)}";
    }
}