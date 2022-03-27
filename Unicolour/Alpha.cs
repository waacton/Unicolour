namespace Wacton.Unicolour;

public record Alpha(double A)
{
    public double A { get; } = A;
    public int A255 => (int) Math.Round(A * 255);
    public string Hex => $"{A255:X2}";

    public override string ToString() => $"{A}";
}