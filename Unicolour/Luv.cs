namespace Wacton.Unicolour;

public record Luv
{
    public double L { get; }
    public double U { get; }
    public double V { get; }
    public ColourTriplet Triplet => new(L, U, V);
    
    internal bool IsMonochrome => ConvertedFromMonochrome || U.Equals(0.0) && V.Equals(0.0);
    internal bool ConvertedFromMonochrome { get; }
    
    public Luv(double l, double u, double v) : this(l, u, v, false) {}
    internal Luv(double l, double u, double v, bool convertedFromMonochrome)
    {
        L = l;
        U = u;
        V = v;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }

    public override string ToString()
    {
        var prefixU = U > 0 ? "+" : string.Empty;
        var prefixV = V > 0 ? "+" : string.Empty;
        return $"{Math.Round(L, 2)}% {prefixU}{Math.Round(U, 2)} {prefixV}{Math.Round(V, 2)}";
    }
}