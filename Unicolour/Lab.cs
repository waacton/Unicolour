namespace Wacton.Unicolour;

public class Lab
{
    public double L { get; }
    public double A { get; }
    public double B { get; }

    public Lab(double l, double a, double b)
    {
        L = l;
        A = a;
        B = b;
    }

    public override string ToString()
    {
        var prefixA = A > 0 ? "+" : string.Empty;
        var prefixB = B > 0 ? "+" : string.Empty;
        return $"{Math.Round(L, 2)} {prefixA}{Math.Round(A, 2)} {prefixB}{Math.Round(B, 2)}";
    }
}