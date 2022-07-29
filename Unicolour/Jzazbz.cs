namespace Wacton.Unicolour;

using static Utils;

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

    public override string ToString() => $"{Math.Round(J, 5)} {Signed(Math.Round(A, 5))} {Signed(Math.Round(B, 5))}";
}