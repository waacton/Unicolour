using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

internal record Segment(Chromaticity Start, Chromaticity End)
{
    internal Segment((double x, double y) start, (double x, double y) end) : this(new Chromaticity(start.x, start.y), new Chromaticity(end.x, end.y)) { }
    
    internal Chromaticity Start { get; } = Start;
    internal Chromaticity End { get; } = End;
    internal Line Line { get; } = new(GetSlopeAndIntercept(Start, End));
    internal double Length { get; } = Distance(Start, End);
    
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