namespace Wacton.Unicolour;

using static Utils;

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

    public override string ToString() => $"{Math.Round(L, 2)} {Signed(Math.Round(A, 2))} {Signed(Math.Round(B, 2))}";
}