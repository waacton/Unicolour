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
        var lowerNodeC = NodeChromas.Last(nodeC => nodeC <= c);
        var upperNodeC = NodeChromas.First(nodeC => nodeC >= c);
        
        if (lowerNodeC == upperNodeC)
        {
            return GetXyForValueAndChroma(nodeV, lowerNodeC, h);
        }
        
        var lower = GetXyForValueAndChroma(nodeV, lowerNodeC, h);
        var upper = GetXyForValueAndChroma(nodeV, upperNodeC, h);
        
        // TODO: ensure works when null chroma
        var distance = upperNodeC - lowerNodeC == 0 ? 0 : (c - lowerNodeC) / (upperNodeC - lowerNodeC);
        var x = Interpolation.Interpolate(lower.x, upper.x, distance);
        var y = Interpolation.Interpolate(lower.y, upper.y, distance);
        return (x, y);
    }

    private static (double x, double y) GetXyForValueAndChroma(double nodeV, double nodeC, double h)
    {
        var scaled = h / DegreesPerHueNumber; // maps 0-360 to 0-40 (10 letter bands with 4 numbers per band)
        var lowerH = Math.Floor(scaled) * DegreesPerHueNumber;
        var upperH = Math.Ceiling(scaled) * DegreesPerHueNumber;

        var lowerNodeH = FromDegrees(lowerH);
        var upperNodeH = FromDegrees(upperH);
        
        if (lowerNodeH == upperNodeH)
        {
            var exact = Nodes.Value.Single(x => x.IsMatch(lowerNodeH.number, lowerNodeH.letter, nodeV, nodeC));
            return exact.Point;
        }
        
        var lower = Nodes.Value.Single(x => x.IsMatch(lowerNodeH.number, lowerNodeH.letter, nodeV, nodeC));
        var upper = Nodes.Value.Single(x => x.IsMatch(upperNodeH.number, upperNodeH.letter, nodeV, nodeC));
        
        var distance = upperH - lowerH == 0 ? 0 : (h - lowerH) / (upperH - lowerH);
        var x = Interpolation.Interpolate(lower.X, upper.X, distance);
        var y = Interpolation.Interpolate(lower.Y, upper.Y, distance);
        return (x, y);
    }

    internal static (double h, double c) GetHueAndChroma(Chromaticity chromaticity, double v)
    {
        var lowerNodeV = NodeValues.Last(item => item <= v);
        var upperNodeV = NodeValues.First(item => item >= v);
        
        var (lowerH, lowerC) = GetHueAndChromaForValue(chromaticity.Xy, lowerNodeV);
        var (upperH, upperC) = GetHueAndChromaForValue(chromaticity.Xy, upperNodeV);

        var distance = v - lowerNodeV;
        var h = Interpolation.Interpolate(lowerH, upperH, distance);
        var c = Interpolation.Interpolate(lowerC, upperC, distance);
        return (h, c);
    }
    
    internal static (double h, double c) GetHueAndChromaForValue((double x, double y) targetPoint, double nodeV)
    {
        var nodes = Nodes.Value.Where(data => data.Value == nodeV).ToArray();
        var downLefts = nodes.Where(data => data.X <= targetPoint.x && data.Y <= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var downRights = nodes.Where(data => data.X >= targetPoint.x && data.Y <= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var upLefts = nodes.Where(data => data.X <= targetPoint.x && data.Y >= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        var upRights = nodes.Where(data => data.X >= targetPoint.x && data.Y >= targetPoint.y).OrderBy(data => GetDistance(targetPoint, data.Point)).ToArray();
        
        var upperHorizontal = GetBoundary(downLefts, downRights);
        var lowerHorizontal = GetBoundary(upLefts, upRights);
        var leftVertical = GetBoundary(downLefts, upLefts);
        var rightVertical = GetBoundary(downRights, upRights);
        
        var horizontalThroughPoint = Line.FromPoints(targetPoint, (targetPoint.x + 1, targetPoint.y));
        var verticalThroughPoint = Line.FromPoints(targetPoint, (targetPoint.x, targetPoint.y + 1));
        
        bool useHorizontals;
        if (lowerHorizontal != null && upperHorizontal != null)
        {
            if (leftVertical == null || rightVertical == null)
            {
                useHorizontals = true;
            }
            else
            {
                var lowerHorizontalIntersect = lowerHorizontal.Line.GetIntersect(verticalThroughPoint);
                var upperHorizontalIntersect = upperHorizontal.Line.GetIntersect(verticalThroughPoint);
                var leftVerticalIntersect = leftVertical.Line.GetIntersect(horizontalThroughPoint);
                var rightVerticalIntersect = rightVertical.Line.GetIntersect(horizontalThroughPoint);
                
                var distanceToLowerHorizontal = GetDistance(lowerHorizontalIntersect, targetPoint);
                var distanceToUpperHorizontal = GetDistance(upperHorizontalIntersect, targetPoint);
                var distanceToLeftVertical = GetDistance(leftVerticalIntersect, targetPoint);
                var distanceToRightVertical = GetDistance(rightVerticalIntersect, targetPoint);

                useHorizontals = (distanceToLowerHorizontal < distanceToLeftVertical && distanceToLowerHorizontal < distanceToRightVertical)
                                 || (distanceToUpperHorizontal < distanceToLeftVertical && distanceToUpperHorizontal < distanceToRightVertical);
            }
        }
        else
        {
            useHorizontals = false;
        }

        return useHorizontals
            ? GetHueAndChromaFromBoundaries(lowerHorizontal!, upperHorizontal!, targetPoint, verticalThroughPoint)
            : GetHueAndChromaFromBoundaries(leftVertical!, rightVertical!, targetPoint, horizontalThroughPoint);
    }
    
    private static Boundary? GetBoundary(Node[] directionA, Node[] directionB)
    {
        var closestA = directionA.FirstOrDefault();
        var backupA = directionA.Skip(1).FirstOrDefault();
        var closestB = directionB.FirstOrDefault();
        var backupB = directionB.Skip(1).FirstOrDefault();
        
        if (closestA != null && closestB != null)
        {
            return new Boundary(closestA, closestB, IsExtrapolation: false);
        }

        if (closestA == null && closestB != null && backupB != null)
        {
            return new Boundary(closestB, backupB, IsExtrapolation: true);
        }

        if (closestB == null && closestA != null && backupA != null)
        {
            return new Boundary(closestA, backupA, IsExtrapolation: true);
        }

        return null;
    }

    private static (double h, double c) GetHueAndChromaFromBoundaries(Boundary boundaryA, Boundary boundaryB, (double x, double y) point, Line lineThroughPoint)
    {
        var (intersectA, startH, startC) = GetBoundaryIntersect(boundaryA, lineThroughPoint);
        var (intersectB, endH, endC) = GetBoundaryIntersect(boundaryB, lineThroughPoint);

        if (intersectA == intersectB)
        {
            return (startH, startC);
        }

        var distanceBetweenIntersects = GetDistance(intersectA, intersectB);
        var distanceToPoint = GetDistance(intersectA, point);
        var interpolationDistance = distanceToPoint / distanceBetweenIntersects;
        var c = Interpolation.Interpolate(startC, endC, interpolationDistance);
        var h = Interpolation.Interpolate(startH, endH, interpolationDistance);
        return (h, c);
    }
    
    private static ((double x, double y) intersect, double h, double c) GetBoundaryIntersect(Boundary boundary, Line throughTarget)
    {
        if (boundary.IsSingularity)
        {
            return (boundary.Start.Point, boundary.Start.HueDegrees, boundary.Start.Chroma);
        }

        var intersect = boundary.Line.GetIntersect(throughTarget);
        var boundaryTotalLength = GetDistance(boundary.Start.Point, boundary.End.Point);
        var boundaryToIntersectLength = GetDistance(boundary.Start.Point, intersect);
        var distance = boundaryToIntersectLength / boundaryTotalLength;
        var h = Interpolation.Interpolate(boundary.Start.HueDegrees, boundary.End.HueDegrees, distance);
        var c = Interpolation.Interpolate(boundary.Start.Chroma, boundary.End.Chroma, distance);
        return (intersect, h, c);
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
    
    private record Boundary(Node Start, Node End, bool IsExtrapolation)
    {
        internal readonly Node Start = Start;
        internal readonly Node End = End;
        internal readonly Line Line = Line.FromPoints(Start.Point, End.Point);
        internal readonly bool IsExtrapolation = IsExtrapolation;
        internal readonly bool IsSingularity = Start == End;
        public override string ToString() => $"{Start.Point} --> {End.Point}{(IsExtrapolation ? " (extrapolation)" : string.Empty)}";
    }
}