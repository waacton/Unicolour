namespace Wacton.Unicolour;

public record Alpha(double A)
{
    public double A { get; } = A;
    public int A255 => double.IsNaN(A) ? 0 : (int)Math.Round(A * 255).Clamp(0, 255);
    public string Hex => $"{A255:X2}";
    
    public override string ToString() => $"{A}";
}