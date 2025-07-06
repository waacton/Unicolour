using static Wacton.Unicolour.Experimental.Munsell;

namespace Wacton.Unicolour.Experimental;

internal static class MunsellUtils
{
    internal static (double x, double y) GetXy(Munsell munsell)
    {
        // TODO: handle grey (null value (implies null chroma) or 0 chroma)
        var hueDegrees = munsell.HueDegrees;
        var value = munsell.Value;
        var chroma = munsell.Chroma;
        return GetXy(value, chroma, hueDegrees);
    }
    
    private static (double x, double y) GetXy(double v, double c, double h)
    {
        // TODO: what if V is out of range? extrapolate? (see chroma)
        var lowerNodeV = NodeValues.Last(nodeV => nodeV <= v);
        var upperNodeV = NodeValues.First(nodeV => nodeV >= v);

        if (lowerNodeV == upperNodeV)
        {
            return GetXyForValue(lowerNodeV, c, h);
        }
        
        var lower = GetXyForValue(lowerNodeV, c, h);
        var upper = GetXyForValue(upperNodeV, c, h);
        
        // TODO: ensure works when null chroma
        var distance = upperNodeV - lowerNodeV == 0 ? 0 : (v - lowerNodeV) / (upperNodeV - lowerNodeV);
        var x = Interpolation.Interpolate(lower.x, upper.x, distance);
        var y = Interpolation.Interpolate(lower.y, upper.y, distance);
        return (x, y);
    }
    
    private static (double x, double y) GetXyForValue(double nodeV, double c, double h)
    {
        var scaled = h / DegreesPerHueNumber; // maps 0-360 to 0-40 (10 letter bands with 4 numbers per band)
        var lowerH = Math.Floor(scaled) * DegreesPerHueNumber;
        var upperH = Math.Ceiling(scaled) * DegreesPerHueNumber;

        var lowerNodeH = FromDegrees(lowerH);
        var upperNodeH = FromDegrees(upperH);
        
        if (lowerNodeH == upperNodeH)
        {
            return GetXyForValueAndHue(nodeV, c, lowerNodeH);
        }
                
        var lower = GetXyForValueAndHue(nodeV, c, lowerNodeH);
        var upper = GetXyForValueAndHue(nodeV, c, upperNodeH);
        
        var distance = upperH - lowerH == 0 ? 0 : (h - lowerH) / (upperH - lowerH);
        var x = Interpolation.Interpolate(lower.x, upper.x, distance);
        var y = Interpolation.Interpolate(lower.y, upper.y, distance);
        return (x, y);
    }

    private static (double x, double y) GetXyForValueAndHue(double nodeV, double c, (double number, string letter) nodeH)
    {
        var whitePointC = Illuminant.C.GetWhitePoint(Observer.Degree2).ToChromaticity();
        
        // TODO: attempt to interpolate to chromas (and values) beyond the dataset?
        var maxChroma = MaxChroma(nodeV, nodeH);
        var useMaxChroma = c > maxChroma;
        
        var lowerNodeC = useMaxChroma ? NodeChromas.Last(nodeC => nodeC < maxChroma) : NodeChromas.Last(nodeC => nodeC <= c);
        var upperNodeC = useMaxChroma ? maxChroma : NodeChromas.First(nodeC => nodeC >= c);
        
        // var lowerNodeC = NodeChromas.Last(nodeC => nodeC <= c);
        // var upperNodeC = NodeChromas.First(nodeC => nodeC >= c);
        
        if (lowerNodeC == upperNodeC)
        {
            var exact = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, lowerNodeC));
            return exact.Point;
        }

        if (useMaxChroma)
        {
            var lower = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, lowerNodeC));
            var upper = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, upperNodeC));

            var distance = (c - lowerNodeC) / (upperNodeC - lowerNodeC);
            var x = Interpolation.Interpolate(lower.X, upper.X, distance);
            var y = Interpolation.Interpolate(lower.Y, upper.Y, distance);
            return (x, y);
        }
        else
        {
            var lower = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, lowerNodeC));
            var upper = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, upperNodeC));
            
            // TODO: ensure works when null chroma
            var distance = upperNodeC - lowerNodeC == 0 ? 0 : (c - lowerNodeC) / (upperNodeC - lowerNodeC);
            var x = Interpolation.Interpolate(lower.X, upper.X, distance);
            var y = Interpolation.Interpolate(lower.Y, upper.Y, distance);
            return (x, y);
        }
    }

    private static double MaxChroma(double nodeV, (double number, string letter) nodeH)
    {
        for (var i = NodeChromas.Length - 1; i > 0; i--)
        {
            var nodeC = NodeChromas[i];
            var result = Nodes.Value.SingleOrDefault(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, nodeC));
            if (result != null) return nodeC;
        }

        return 0;
    }

    internal static (double h, double c) GetHueAndChroma(Chromaticity chromaticity, double v)
    {
        // TODO: handle out of range, interpolate from closest two Vs
        var lowerNodeV = NodeValues.Last(item => item <= v);
        var upperNodeV = NodeValues.First(item => item >= v);
        
        var (lowerH, lowerC) = GetHueAndChromaForValue(chromaticity.Xy, lowerNodeV);
        var (upperH, upperC) = GetHueAndChromaForValue(chromaticity.Xy, upperNodeV);

        var distance = v - lowerNodeV;
        var h = Boundary.InterpolateHue(lowerH, upperH, distance);
        var c = Interpolation.Interpolate(lowerC, upperC, distance);
        return (h, c);
    }

    private static (double h, double c) GetHueAndChromaForValue((double x, double y) targetPoint, double nodeV)
    {
        var boundary = GetBoundary(targetPoint, nodeV);
        if (boundary != null) return boundary.GetHueAndChroma();
        
        // TODO: is something like this worth keeping?
        //       finds (x, y) between target and white point within dataset, so (h, c) can be obtained
        //       and use those points and their (h, c) to extrapolate to the point outside the dataset
        //       makes sense, but would need to allow extrapolation the other way too (Munsell -> XYY) which isn't obvious
        //       (see `GetXyForValueAndHue()` where needing to find a max chroma for the hue, and similar will be needed for value
        var whitePointC = Illuminant.C.GetWhitePoint(Observer.Degree2).ToChromaticity();
        
        // line between target point and white point
        var line = Line.FromPoints(targetPoint, whitePointC.Xy);

        var (x, y) = targetPoint;
        const double step = 0.01;
        Boundary? closerBoundary = null;
        while (closerBoundary == null)
        {
            x -= step;
            y = line.GetY(x);
            closerBoundary = GetBoundary((x, y), nodeV);
        }
        
        x -= step;
        y = line.GetY(x);
        Boundary? fartherBoundary = GetBoundary((x, y), nodeV);

        var closer = closerBoundary.GetHueAndChroma();
        var farther = fartherBoundary!.GetHueAndChroma();
        
        var length = GetDistance(fartherBoundary.Point, closerBoundary.Point);
        var extrapolationLength = GetDistance(fartherBoundary.Point, targetPoint);
        var distance = extrapolationLength / length;
        var h = Boundary.InterpolateHue(farther.h, closer.h, distance);
        var c = Interpolation.Interpolate(farther.c, closer.c, distance);
        return (h, c);
    }
    
    private static Boundary? GetBoundary((double x, double y) targetPoint, double nodeV)
    {
        var nodes = Nodes.Value.Where(data => data.Value == nodeV).ToArray();
        var upLefts = nodes.Where(data => data.X <= targetPoint.x && data.Y >= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var upRights = nodes.Where(data => data.X >= targetPoint.x && data.Y >= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var downLefts = nodes.Where(data => data.X <= targetPoint.x && data.Y <= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var downRights = nodes.Where(data => data.X >= targetPoint.x && data.Y <= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        
        var upLeft = upLefts.FirstOrDefault();
        var upRight = upRights.FirstOrDefault();
        var downLeft = downLefts.FirstOrDefault();
        var downRight = downRights.FirstOrDefault();

        if (upLeft == null || upRight == null || downLeft == null || downRight == null) return null;
        return new Boundary(targetPoint, upLeft, upRight, downLeft, downRight);
    }
    
    internal static double ToDegrees(double hueNumber, string hueLetter)
    {
        var bandIndex = Array.IndexOf(NodeHueLetters, hueLetter);

        var minDegrees = bandIndex * DegreesPerHueLetter;
        var maxDegrees = (bandIndex + 1) * DegreesPerHueLetter;
        var distance = hueNumber / 10.0; // maps 0 - 10 to 0 - 1
        return Interpolation.Interpolate(minDegrees, maxDegrees, distance);
    }

    internal static (double number, string letter) FromDegrees(double degrees)
    {
        var bandLocation = degrees.Modulo(360) / DegreesPerHueLetter;
        var bandIndex = (int)Math.Truncate(bandLocation);
        var hueLetter = NodeHueLetters[bandIndex];
        var hueNumber = (bandLocation - bandIndex) * 10;
        if (hueNumber != 0) return (hueNumber, hueLetter);
        
        bandIndex = bandIndex == 0 ? NodeHueLetters.Length - 1 : bandIndex - 1;
        hueLetter = NodeHueLetters[bandIndex];
        hueNumber = 10;
        return (hueNumber, hueLetter);
    }
    
    private static double GetDistance((double x, double y) point1, (double x, double y) point2)
    {
        return Math.Sqrt(Math.Pow(point2.x - point1.x, 2) + Math.Pow(point2.y - point1.y, 2));
    }
    
    private record Boundary((double x, double y) Point, Node UpLeft, Node UpRight, Node DownLeft, Node DownRight) 
    {
        internal (double x, double y) Point { get; } = Point;
        internal Node UpLeft { get; } = UpLeft;
        internal Node UpRight { get; } = UpRight;
        internal Node DownLeft { get; } = DownLeft;
        internal Node DownRight { get; } = DownRight;

        private Segment Upper { get; } = new(UpLeft, UpRight, IsExtrapolation: false);
        private Segment Lower { get; } = new(DownLeft, DownRight, IsExtrapolation: false);
        private Segment Left { get; } = new(UpLeft, DownLeft, IsExtrapolation: false);
        private Segment Right { get; } = new(UpRight, DownRight, IsExtrapolation: false);
        
        private readonly Line Horizontal = Line.FromPoints(Point, (Point.x + 1, Point.y));
        private readonly Line Vertical = Line.FromPoints(Point, (Point.x, Point.y + 1));

        private (double x, double y) UpperIntersect => Upper.Line.GetIntersect(Vertical);
        private (double x, double y) LowerIntersect => Lower.Line.GetIntersect(Vertical);
        private (double x, double y) LeftIntersect => Left.Line.GetIntersect(Horizontal);
        private (double x, double y) RightIntersect => Right.Line.GetIntersect(Horizontal);
        
        private double UpperIntersectToPointDistance => GetDistance(UpperIntersect, Point);
        private double LowerIntersectToPointDistance => GetDistance(LowerIntersect, Point);
        private double LeftIntersectToPointDistance => GetDistance(LeftIntersect, Point);
        private double RightIntersectToPointDistance => GetDistance(RightIntersect, Point);

        private bool InterpolateVertically => 
            (UpperIntersectToPointDistance < LeftIntersectToPointDistance && UpperIntersectToPointDistance < RightIntersectToPointDistance) 
            || (LowerIntersectToPointDistance < LeftIntersectToPointDistance && LowerIntersectToPointDistance < RightIntersectToPointDistance);

        internal (double h, double c) GetHueAndChroma()
        {
            (double h, double c) start;
            (double h, double c) end;
            double distance;
            
            // TODO: account for segments being singularities
            //       e.g. up-left and up-right might be the same point! (if target point lies DIRECTLY below with exact same double)
            //       and the result will be divide-by-zero

            if (InterpolateVertically)
            {
                start = GetHueAndChroma(Upper, UpperIntersect); 
                end = GetHueAndChroma(Lower, LowerIntersect); 
                distance = GetDistance(UpperIntersect, Point) / GetDistance(UpperIntersect, LowerIntersect);
            }
            else
            {
                start = GetHueAndChroma(Left, LeftIntersect); 
                end = GetHueAndChroma(Right, RightIntersect); 
                distance = GetDistance(LeftIntersect, Point) / GetDistance(LeftIntersect, RightIntersect);
            }

            var h = InterpolateHue(start.h, end.h, distance);
            var c = Interpolation.Interpolate(start.c, end.c, distance);
            return (h, c);
        }

        internal static (double h, double c) GetHueAndChroma(Segment segment, (double x, double y) intersect)
        {
            var distance = GetDistance(segment.Start.Point, intersect) / GetDistance(segment.Start.Point, segment.End.Point);
            var h = InterpolateHue(segment.Start.HueDegrees, segment.End.HueDegrees, distance);
            var c = Interpolation.Interpolate(segment.Start.Chroma, segment.End.Chroma, distance);
            return (h, c);
        }

        internal static double InterpolateHue(double start, double end, double distance)
        {
            var adjustedHues = Interpolation.AdjustHues(start, end, HueSpan.Shorter);
            return Interpolation.Interpolate(adjustedHues.start, adjustedHues.end, distance).Modulo(360);
        }
    }
    
    private record Segment(Node Start, Node End, bool IsExtrapolation)
    {
        internal readonly Node Start = Start;
        internal readonly Node End = End;
        internal readonly Line Line = Line.FromPoints(Start.Point, End.Point);
        internal readonly bool IsExtrapolation = IsExtrapolation;
        internal readonly bool IsSingularity = Start == End;
        public override string ToString() => $"{Start.Point} --> {End.Point}{(IsExtrapolation ? " (extrapolation)" : string.Empty)}";
    }
}