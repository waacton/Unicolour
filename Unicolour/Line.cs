using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

internal record LineSegment(Chromaticity Start, Chromaticity End)
{
    internal LineSegment((double x, double y) start, (double x, double y) end) : this(new Chromaticity(start.x, start.y), new Chromaticity(end.x, end.y)) { }
    
    internal Chromaticity Start { get; } = Start;
    internal Chromaticity End { get; } = End;
    internal Line Line { get; } = new(GetSlopeAndIntercept(Start, End));
    internal double Length { get; } = Distance(Start, End);
    
    internal static double Distance(Chromaticity start, Chromaticity end)
    {
        return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
    }

    internal static (double radius, double angle) Polar(Chromaticity start, Chromaticity end)
    {
        var radius = Distance(start, end);
        var angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
        return (radius, ToDegrees(angle).Modulo(360));
    }

    private static (double slope, double intercept) GetSlopeAndIntercept(Chromaticity point1, Chromaticity point2)
    {
        var (x1, y1) = point1;
        var (x2, y2) = point2;
        var sameX = x1 == x2;
        var sameY = y1 == y2;
        
        if (sameX && sameY) return (double.NaN, double.NaN); // no line between points in the same location
        if (sameX) return (double.PositiveInfinity, x1); // vertical line between points
        if (sameY) return (0, y1); // horizontal line between points
        
        var slope = (y2 - y1) / (x2 - x1);
        var intercept = y1 - slope * x1;
        return (slope, intercept);
    }

    public override string ToString() => $"{Start} \u27f6 {End}";
}

internal record Line(double Slope, double Intercept)
{
    internal Line((double slope, double intercept) tuple) : this(tuple.slope, tuple.intercept) { }
    
    internal double Slope { get; } = Slope;
    internal double Intercept { get; } = Intercept;

    internal bool IsVertical => double.IsInfinity(Slope);

    internal double GetY(double x) => Slope * x + Intercept;
        
    internal Chromaticity GetIntersect(Line other)
    {
        double x, y;      
        if (IsVertical)
        {
            x = Intercept;
            y = other.GetY(x);
        }
        else if (other.IsVertical)
        {
            x = other.Intercept;
            y = GetY(x);
        }
        else
        {
            x = (other.Intercept - Intercept) / (Slope - other.Slope);
            y = GetY(x);
        }

        return new(x, y);
    }
        
    public override string ToString() => $"y = {Slope}x + {Intercept}";
}