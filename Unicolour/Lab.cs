namespace Wacton.Unicolour;

public class Lab
{
    public double L { get; }
    public double A { get; }
    public double B { get; }

    public Lab(double l, double a, double b)
    {
        l.Check(0.0, 100.0, "Lightness");
        a.Check(-128.0, 128.0, "A (green/red)");
        b.Check(-128.0, 128.0, "B (blue/yellow)");

        L = l;
        A = a;
        B = b;
    }

    public override string ToString() => $"{L} {(A > 0 ? "+" : string.Empty)}{A} {(B > 0 ? "+" : string.Empty)}{B}";
}