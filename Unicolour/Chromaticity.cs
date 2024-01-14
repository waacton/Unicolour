namespace Wacton.Unicolour;

public record Chromaticity(double X, double Y)
{
    public double X { get; } = X;
    public double Y { get; } = Y;
    public (double x, double y) Xy => (X, Y);

    // https://en.wikipedia.org/wiki/CIE_1960_color_space
    public double U { get; } = 4 * X / (12 * Y - 2 * X + 3);
    public double V { get; } = 6 * Y / (12 * Y - 2 * X + 3);
    public (double u, double v) Uv => (U, V);
    
    public static Chromaticity FromUv(double u, double v)
    {
        var x = 3 * u / (2 * u - 8 * v + 4);
        var y = 2 * v / (2 * u - 8 * v + 4);
        return new Chromaticity(x, y);
    }

    public WhitePoint ToWhitePoint(double luminance = 1.0)
    {
        var xyz = Xyy.ToXyz(new(X, Y, luminance));
        return WhitePoint.FromXyz(xyz);
    }
    
    public override string ToString() => double.IsNaN(X) || double.IsNaN(Y) ? "-" : $"({X:F4}, {Y:F4})";
}