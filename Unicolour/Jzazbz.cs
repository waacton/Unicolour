namespace Wacton.Unicolour;

public record Jzazbz
{
    public double J { get; }
    public double A { get; }
    public double B { get; }
    public ColourTriplet Triplet => new(J, A, B);
    
    // TODO:
    internal bool IsMonochrome => ConvertedFromMonochrome || A.Equals(0.0) && B.Equals(0.0);
    internal bool ConvertedFromMonochrome { get; }
    
    public Jzazbz(double j, double a, double b) : this(j, a, b, false) {}
    internal Jzazbz(double j, double a, double b, bool convertedFromMonochrome)
    {
        J = j;
        A = a;
        B = b;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }

    // TODO:
    public override string ToString()
    {
        string Prefix(double value) => value > 0 ? "+" : string.Empty;
        return $"{Math.Round(J, 2)}% {Prefix(A)}{Math.Round(A, 2)} {Prefix(B)}{Math.Round(B, 2)}";
    }
}