namespace Wacton.Unicolour;

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