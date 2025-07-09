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
        // TODO: attempt to interpolate to chromas (and values) beyond the dataset?
        var maxChroma = MaxChroma(nodeV, nodeH);
        if (maxChroma == 0)
        {
            // TODO: at least true for 10/Y 0.2 somehow?
            throw new InvalidOperationException($"No max chroma for {nodeH.number}/{nodeH.letter} {nodeV}; this shouldn't be possible");
        }
        
        var useMaxChroma = c > maxChroma;
        if (useMaxChroma)
        {
            var whitePointC = Illuminant.C.GetWhitePoint(Observer.Degree2).ToChromaticity();

            // var lowerNodeC = NodeChromas.Last(nodeC => nodeC < maxChroma);
            var lowerNodeC = NodeChromas.First();
            var upperNodeC = maxChroma;
            
            if (lowerNodeC == upperNodeC)
            {
                var exact = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, lowerNodeC));
                return exact.Point;
            }
            
            var lower = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, lowerNodeC));
            // (double X, double Y) lower = whitePointC.Xy;
            // lowerNodeC = 0; // no chroma at whitepoint

            var upper = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, upperNodeC));
            var distance = (c - lowerNodeC) / (upperNodeC - lowerNodeC);
            var x = Interpolation.Interpolate(lower.X, upper.X, distance);
            var y = Interpolation.Interpolate(lower.Y, upper.Y, distance);
            return (x, y);
        }
        else
        {
            var lowerNodeC = NodeChromas.Last(nodeC => nodeC <= c);
            var upperNodeC = NodeChromas.First(nodeC => nodeC >= c);
            
            if (lowerNodeC == upperNodeC)
            {
                var exact = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, lowerNodeC));
                return exact.Point;
            }
            
            var lower = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, lowerNodeC));
            var upper = Nodes.Value.Single(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, upperNodeC));
            
            // TODO: ensure works when null chroma
            var distance = upperNodeC - lowerNodeC == 0 ? 0 : (c - lowerNodeC) / (upperNodeC - lowerNodeC);
            var x = Interpolation.Interpolate(lower.X, upper.X, distance);
            var y = Interpolation.Interpolate(lower.Y, upper.Y, distance);
            return (x, y);
        }
    }

    private static int MaxChroma(double nodeV, (double number, string letter) nodeH)
    {
        for (var i = NodeChromas.Length - 1; i >= 0; i--)
        {
            var nodeC = NodeChromas[i];
            var result = Nodes.Value.SingleOrDefault(x => x.IsMatch(nodeH.number, nodeH.letter, nodeV, nodeC));
            if (result != null) return nodeC;
        }

        // TODO: what does it mean if we've ended up here?!
        return 0;
    }

    internal static (double h, double c) GetHueAndChroma(Chromaticity chromaticity, double v)
    {
        // TODO: handle out of range, interpolate from closest two Vs
        var lowerNodeV = NodeValues.Last(item => item <= v);
        var upperNodeV = NodeValues.First(item => item >= v);
        
        var (lowerH, lowerC) = EstimateHueAndChromaForValue(chromaticity.Xy, lowerNodeV);
        var (upperH, upperC) = EstimateHueAndChromaForValue(chromaticity.Xy, upperNodeV);

        var distance = v - lowerNodeV;
        var h = Boundary.InterpolateHue(lowerH, upperH, distance);
        var c = Interpolation.Interpolate(lowerC, upperC, distance);
        return (h, c);
    }

    private static (double h, double c) EstimateHueAndChromaForValue((double x, double y) targetPoint, double nodeV)
    {
        var boundary = GetBoundary(targetPoint, nodeV);
        if (boundary != null)
        {
            return boundary.EstimateHueAndChroma();
        }
        
        // TODO: is something like this worth keeping?
        //       finds (x, y) between target and white point within dataset, so (h, c) can be obtained
        //       and use those points and their (h, c) to extrapolate to the point outside the dataset
        //       makes sense, but would need to allow extrapolation the other way too (Munsell -> XYY) which isn't obvious
        //       (see `GetXyForValueAndHue()` where needing to find a max chroma for the hue, and similar will be needed for value
        
        var currentPoint = targetPoint;
        Boundary? closerBoundary = null;
        while (closerBoundary == null)
        {
            currentPoint = MoveTowardsWhite(currentPoint);
            closerBoundary = GetBoundary(currentPoint, nodeV);
        }
        
        // TODO: is it at all possible for this point, one step from first point found within dataset, to somehow be outside the dataset?
        // TODO: apparently yes with XYY(0.44085535496654615, 0.39631413842136687, 0.7940058511128927)

        currentPoint = MoveTowardsWhite(currentPoint);
        Boundary fartherBoundary = GetBoundary(currentPoint, nodeV)!;

        var closer = closerBoundary.EstimateHueAndChroma();
        var farther = fartherBoundary.EstimateHueAndChroma();
        
        var length = GetDistance(fartherBoundary.Point, closerBoundary.Point);
        var extrapolationLength = GetDistance(fartherBoundary.Point, targetPoint);
        var distance = extrapolationLength / length;
        var h = Boundary.InterpolateHue(farther.h, closer.h, distance);
        var c = Interpolation.Interpolate(farther.c, closer.c, distance);
        return (h, c);
    }

    private static (double x, double y) MoveTowardsWhite((double x, double y) start)
    {
        const double step = 0.01;
        var white = Illuminant.C.GetWhitePoint(Observer.Degree2).ToChromaticity();
        var xc = start.x - step * ((start.x - white.X) / GetDistance(start, white.Xy));
        var line = Line.FromPoints(start, white.Xy);
        var y = line.GetY(xc);
        return (xc, y);
    }
    
    private static Boundary? GetBoundary((double x, double y) targetPoint, double nodeV)
    {
        var nodes = Nodes.Value.Where(data => data.Value == nodeV).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var closest = nodes.First();
        
        // TODO: what happens when there are no bounding quads?
        //       fall back to algorithm that moves closer to whitepoint?
        // e.g. the closest node if 5G /8
        // there are 4 possible bounding quadrilaterals:
        // - 5G /8 · 5G /10 · 7.5G /10 · 7.5G /8
        // - 5G /8 · 5G /10 · 2.5G /10 · 2.5G /8
        // - 5G /8 · 5G  /6 · 7.5G  /6 · 7.5G /8
        // - 5G /8 · 5G  /6 · 2.5G  /6 · 2.5G /8

        var upHue = FromDegrees(closest.HueDegrees + DegreesPerHueNumber);
        var downHue = FromDegrees(closest.HueDegrees - DegreesPerHueNumber);
        var upChroma = NodeChromas.First(c => c > closest.Chroma);
        var downChroma = NodeChromas.Last(c => c < closest.Chroma);

        // TODO: refactor; make sure nodes exist before trying to create quads
        var quadNodes = new[]
        {
            new[]
            {
                closest,
                Nodes.Value.Single(data => data.IsMatch(closest.HueNumber, closest.HueLetter, nodeV, upChroma)),
                Nodes.Value.Single(data => data.IsMatch(upHue.number, upHue.letter, nodeV, upChroma)),
                Nodes.Value.Single(data => data.IsMatch(upHue.number, upHue.letter, nodeV, closest.Chroma))
            },
            new[]
            {
                closest,
                Nodes.Value.Single(data => data.IsMatch(closest.HueNumber, closest.HueLetter, nodeV, upChroma)),
                Nodes.Value.Single(data => data.IsMatch(downHue.number, downHue.letter, nodeV, upChroma)),
                Nodes.Value.Single(data => data.IsMatch(downHue.number, downHue.letter, nodeV, closest.Chroma))
            },
            new[]
            {
                closest,
                Nodes.Value.Single(data => data.IsMatch(closest.HueNumber, closest.HueLetter, nodeV, downChroma)),
                Nodes.Value.Single(data => data.IsMatch(upHue.number, upHue.letter, nodeV, downChroma)),
                Nodes.Value.Single(data => data.IsMatch(upHue.number, upHue.letter, nodeV, closest.Chroma))
            },
            new[]
            {
                closest,
                Nodes.Value.Single(data => data.IsMatch(closest.HueNumber, closest.HueLetter, nodeV, downChroma)),
                Nodes.Value.Single(data => data.IsMatch(downHue.number, downHue.letter, nodeV, downChroma)),
                Nodes.Value.Single(data => data.IsMatch(downHue.number, downHue.letter, nodeV, closest.Chroma))
            }
        };
        
        var quadrilaterals = quadNodes.Select(n => new Quadrilateral(n[0].Point, n[1].Point, n[2].Point, n[3].Point)).ToArray();
        var quadrilateralsContainingPoint = quadrilaterals.Where(x => x.Contains(targetPoint)).ToArray();
        var matchingQuad = quadrilateralsContainingPoint.Single();
        var index = Array.IndexOf(quadrilaterals, matchingQuad);
        var boundingQuad = quadNodes[index];

        // var nodes = Nodes.Value.Where(data => data.Value == nodeV).ToArray();
        var upLefts = nodes.Where(data => data.X <= targetPoint.x && data.Y >= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var upRights = nodes.Where(data => data.X >= targetPoint.x && data.Y >= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var downLefts = nodes.Where(data => data.X <= targetPoint.x && data.Y <= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var downRights = nodes.Where(data => data.X >= targetPoint.x && data.Y <= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        
        var upLeft = upLefts.FirstOrDefault();
        var upRight = upRights.FirstOrDefault();
        var downLeft = downLefts.FirstOrDefault();
        var downRight = downRights.FirstOrDefault();

        upLeft = boundingQuad[0];
        upRight = boundingQuad[1];
        downRight = boundingQuad[2];
        downLeft = boundingQuad[3];
        
        return new Boundary(targetPoint, boundingQuad[0], boundingQuad[1], boundingQuad[2], boundingQuad[3]);
    }

    private record Triangle((double x, double y) Point1, (double x, double y) Point2, (double x, double y) Point3)
    {
        internal (double x, double y) Point1 { get; } = Point1;
        internal (double x, double y) Point2 { get; } = Point2;
        internal (double x, double y) Point3 { get; } = Point3;
        internal readonly double Area = 0.5 * Math.Abs(Point1.x * (Point2.y - Point3.y) + Point2.x * (Point3.y - Point1.y) + Point3.x * (Point1.y - Point2.y));
    }
    
    private record Quadrilateral((double x, double y) Point1, (double x, double y) Point2, (double x, double y) Point3, (double x, double y) Point4)
    {
        internal (double x, double y) Point1 { get; } = Point1;
        internal (double x, double y) Point2 { get; } = Point2;
        internal (double x, double y) Point3 { get; } = Point3;
        internal (double x, double y) Point4 { get; } = Point4;
        internal readonly Triangle Triangle1 = new(Point1, Point2, Point3);
        internal readonly Triangle Triangle2 = new(Point1, Point3, Point4);
        internal double Area => Triangle1.Area + Triangle2.Area;
        
        internal Triangle[] GetTriangles((double x, double y) targetPoint)
        {
            return new Triangle[]
            {
                new(Point1, Point2, targetPoint),
                new(Point2, Point3, targetPoint),
                new(Point3, Point4, targetPoint),
                new(Point4, Point1, targetPoint)
            };
        }

        internal bool Contains((double x, double y) targetPoint)
        {
            var triangles = new Triangle[]
            {
                new(Point1, Point2, targetPoint),
                new(Point2, Point3, targetPoint),
                new(Point3, Point4, targetPoint),
                new(Point4, Point1, targetPoint)
            };

            var areaDifference = Area - triangles.Sum(x => x.Area);
            return areaDifference.IsEffectivelyZero();
        }
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
    
    // TODO: these corners are no longer specific to those directions
    //       should probably take the quadrilateral instead
    private record Boundary((double x, double y) Point, Node NodeA, Node NodeB, Node NodeC, Node NodeD) 
    {
        internal (double x, double y) Point { get; } = Point;
        internal Node NodeA { get; } = NodeA;
        internal Node NodeB { get; } = NodeB;
        internal Node NodeC { get; } = NodeC;
        internal Node NodeD { get; } = NodeD;

        private Segment SegmentAB { get; } = new(NodeA, NodeB, IsExtrapolation: false);
        private Segment SegmentBC { get; } = new(NodeB, NodeC, IsExtrapolation: false);
        private Segment SegmentCD { get; } = new(NodeC, NodeD, IsExtrapolation: false);
        private Segment SegmentDA { get; } = new(NodeD, NodeA, IsExtrapolation: false);
        
        private readonly Line Horizontal = Line.FromPoints(Point, (Point.x + 1, Point.y));
        private readonly Line Vertical = Line.FromPoints(Point, (Point.x, Point.y + 1));

        internal (double h, double c) EstimateHueAndChroma()
        {
            (double h, double c) start;
            (double h, double c) end;
            double distance;
            
            // TODO: account for segments being singularities
            //       e.g. up-left and up-right might be the same point! (if target point lies DIRECTLY below with exact same double)
            //       and the result will be divide-by-zero
            
            // closest point is A
            // find a line through target that intersects closest to point A
            var horizontalIntersectAB = SegmentAB.Line.GetIntersect(Horizontal);
            var verticalIntersectAB = SegmentAB.Line.GetIntersect(Vertical);
            var horizontalIntersectDA = SegmentDA.Line.GetIntersect(Horizontal);
            var verticalIntersectDA = SegmentDA.Line.GetIntersect(Vertical);

            var horizontalIntersectABToA = GetDistance(horizontalIntersectAB, Point);
            var verticalIntersectABToA = GetDistance(verticalIntersectAB, Point);
            var horizontalIntersectDAToA = GetDistance(horizontalIntersectDA, Point);
            var verticalIntersectDAToA = GetDistance(verticalIntersectDA, Point);
            var min = new [] { horizontalIntersectABToA, verticalIntersectABToA, horizontalIntersectDAToA, verticalIntersectDAToA }.Min();

            Segment nearSegment = null!;
            Segment farSegment = null!;
            Line throughLine = null!;

            if (min == horizontalIntersectABToA)
            {
                nearSegment = SegmentAB;
                farSegment = SegmentCD;
                throughLine = Horizontal;
            }
            else if (min == verticalIntersectABToA)
            {
                nearSegment = SegmentAB;
                farSegment = SegmentCD;
                throughLine = Vertical;
            }
            else if (min == horizontalIntersectDAToA)
            {
                nearSegment = SegmentDA;
                farSegment = SegmentBC;
                throughLine = Horizontal;
            }
            else if (min == verticalIntersectDAToA)
            {
                nearSegment = SegmentDA;
                farSegment = SegmentBC;
                throughLine = Vertical;
            }
            else
            {
                throw new InvalidOperationException("Somehow min intersect distance not found");
            }
            
            var nearIntersect = nearSegment.Line.GetIntersect(throughLine);
            var farIntersect = farSegment.Line.GetIntersect(throughLine);
            start = EstimateHueAndChroma(nearSegment, nearIntersect); 
            end = EstimateHueAndChroma(farSegment, farIntersect); 
            distance = GetDistance(nearIntersect, Point) / GetDistance(nearIntersect, farIntersect);
            var h = InterpolateHue(start.h, end.h, distance);
            var c = Interpolation.Interpolate(start.c, end.c, distance);
            return (h, c);
        }

        internal static (double h, double c) EstimateHueAndChroma(Segment segment, (double x, double y) intersect)
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