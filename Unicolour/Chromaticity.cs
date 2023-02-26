namespace Wacton.Unicolour;

public record Chromaticity(double X, double Y)
{
    public double X { get; } = X;
    public double Y { get; } = Y;
    public override string ToString() => $"({X:F4}, {Y:F4})";

    public static class StandardRgb
    {
        public static readonly Chromaticity R = new(0.6400, 0.3300);
        public static readonly Chromaticity G = new(0.3000, 0.6000);
        public static readonly Chromaticity B = new(0.1500, 0.0600);
    }
    
    public static class DisplayP3
    {
        public static readonly Chromaticity R = new(0.680, 0.320);
        public static readonly Chromaticity G = new(0.265, 0.690);
        public static readonly Chromaticity B = new(0.150, 0.060);
    }
    
    public static class Rec2020
    {
        public static readonly Chromaticity R = new(0.708, 0.292);
        public static readonly Chromaticity G = new(0.170, 0.797);
        public static readonly Chromaticity B = new(0.131, 0.046);
    }
}