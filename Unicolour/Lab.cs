namespace Wacton.Unicolour;

public record Lab
{
    public double L { get; }
    public double A { get; }
    public double B { get; }
    public ColourTriplet Triplet => new(L, A, B);
    
    internal bool IsMonochrome => ConvertedFromMonochrome || A.Equals(0.0) && B.Equals(0.0);
    internal bool ConvertedFromMonochrome { get; }
    
    public Lab(double l, double a, double b) : this(l, a, b, false) {}
    internal Lab(double l, double a, double b, bool convertedFromMonochrome)
    {
        L = l;
        A = a;
        B = b;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }

    public override string ToString()
    {
        var prefixA = A > 0 ? "+" : string.Empty;
        var prefixB = B > 0 ? "+" : string.Empty;
        return $"{Math.Round(L, 2)}% {prefixA}{Math.Round(A, 2)} {prefixB}{Math.Round(B, 2)}";
    }
}