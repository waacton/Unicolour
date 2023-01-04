namespace Wacton.Unicolour;

public record Chromaticity(double X, double Y)
{
    public static readonly Chromaticity StandardRgbR = new(0.6400, 0.3300);
    public static readonly Chromaticity StandardRgbG = new(0.3000, 0.6000);
    public static readonly Chromaticity StandardRgbB = new(0.1500, 0.0600);
    
    public double X { get; } = X;
    public double Y { get; } = Y;
    
    public override string ToString() => $"({X:F4}, {Y:F4})";
}