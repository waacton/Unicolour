namespace Wacton.Unicolour;

internal record Line(double Slope, double Intercept)
{
    internal double Slope { get; } = Slope;
    internal double Intercept { get; } = Intercept;

    internal static Line FromPoints((double x, double y) point1, (double x, double y) point2)
    {
        var (x1, y1) = point1;
        var (x2, y2) = point2;
        var sameX = x1 == x2;
        var sameY = y1 == y2;
        
        if (sameX && sameY) return new Line(double.NaN, double.NaN); // no line between points in the same location
        if (sameX) return new Line(double.PositiveInfinity, x1); // vertical line between points
        if (sameY) return new Line(0, y1); // horizontal line between points
        
        var slope = (y2 - y1) / (x2 - x1);
        var intercept = y1 - slope * x1;
        return new Line(slope, intercept);
    }

    internal bool IsVertical => double.IsInfinity(Slope);

    internal double GetY(double x) => Slope * x + Intercept;
        
    internal (double x, double y) GetIntersect(Line other)
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