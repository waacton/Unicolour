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

    public override string ToString() => $"{L} {(A > 0 ? "+" : string.Empty)}{A} {(B > 0 ? "+" : string.Empty)}{B}";
}