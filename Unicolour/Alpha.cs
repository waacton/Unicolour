namespace Wacton.Unicolour;

public record Alpha
{
    public double A { get; }
    public int A255 => (int) Math.Round(A * 255);
    public string Hex => $"{A255:X2}";

    public Alpha(double a)
    {
        a.Guard(0.0, 1.0, "Alpha");
        A = a;
    }

    public override string ToString() => $"{A}";
}