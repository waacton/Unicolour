using static Wacton.Unicolour.Experimental.Munsell;

namespace Wacton.Unicolour.Experimental;

internal static class MunsellUtils
{
    internal static (double x, double y) GetXy(Munsell munsell)
    {
        // TODO: handle grey (null value (implies null chroma) or 0 chroma)
        var hueDegrees = munsell.HueDegrees;
        var value = (double)munsell.Value;
        var chroma = (double)munsell.Chroma;
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

        Line? lowerHorizontal;
        Line? upperHorizontal;
        Line? leftVertical;
        Line? rightVertical;
        
        // TODO: this is unpleasant, try to find a way to improve
        //       considered using closest points as the basis of figuring which line segments to use
        //       but gets even messier in the cases where extrapolation is needed
        //       in cases where there is no point in one direction, 5 points are needed to make the 4 line segments instead of 4!
        //       potentially: extract 8 points: 4x nominal and 4x for extrapolation needs, and determine lines based on what is present?
        if (downLefts.Any() && downRights.Any() && upLefts.Any() && upRights.Any())
        {
            // TODO: when xy is a direct hit on a data point, first point in all directions will be THE SAME
            
            lowerHorizontal = GetLine(downLefts[0], downRights[0]);
            upperHorizontal = GetLine(upLefts[0], upRights[0]);
            leftVertical = GetLine(downLefts[0], upLefts[0]);
            rightVertical = GetLine(downRights[0], upRights[0]);
        }
        else if (!downLefts.Any() && downRights.Any() && upLefts.Any() && upRights.Any())
        {
            var upLeftForExtrapolation = upLefts[1];
            var downRightForExtrapolation = downRights[1];
                
            lowerHorizontal = GetLine(downRightForExtrapolation, downRights[0]);
            upperHorizontal = GetLine(upLefts[0], upRights[0]);
            leftVertical = GetLine(upLeftForExtrapolation, upLefts[0]);
            rightVertical = GetLine(downRights[0], upRights[0]);
        }
        else if (downLefts.Any() && !downRights.Any() && upLefts.Any() && upRights.Any())
        {
            var downLeftForExtrapolation = downLefts[1];
            var upRightForExtrapolation = upRights[1];
                
            lowerHorizontal = GetLine(downLefts[0], downLeftForExtrapolation);
            upperHorizontal = GetLine(upLefts[0], upRights[0]);
            leftVertical = GetLine(downLefts[0], upLefts[0]);
            rightVertical = GetLine(upRightForExtrapolation, upRights[0]);
        }
        else if (downLefts.Any() && downRights.Any() && !upLefts.Any() && upRights.Any())
        {
            var downLeftForExtrapolation = downLefts[1];
            var upRightForExtrapolation = upRights[1];
            
            lowerHorizontal = GetLine(downLefts[0], downRights[0]);
            upperHorizontal = GetLine(upRightForExtrapolation, upRights[0]);
            leftVertical = GetLine(downLefts[0], downLeftForExtrapolation);
            rightVertical = GetLine(downRights[0], upRights[0]);
        }
        else if (downLefts.Any() && downRights.Any() && upLefts.Any() && !upRights.Any())
        {
            var upLeftForExtrapolation = upLefts[1];
            var downRightForExtrapolation = downRights[1];
            
            lowerHorizontal = GetLine(downLefts[0], downRights[0]);
            upperHorizontal = GetLine(upLefts[0], upLeftForExtrapolation);
            leftVertical = GetLine(downLefts[0], upLefts[0]);
            rightVertical = GetLine(downRights[0], downRightForExtrapolation);
        }
        else if (!downLefts.Any() && !downRights.Any() && upLefts.Any() && upRights.Any())
        {
            var upLeftForExtrapolation = upLefts[1];
            var upRightForExtrapolation = upRights[1];

            lowerHorizontal = null;
            upperHorizontal = GetLine(upLefts[0], upRights[0]);
            leftVertical = GetLine(upLeftForExtrapolation, upLefts[0]);
            rightVertical = GetLine(upRightForExtrapolation, upRights[0]);
        }
        else if (downLefts.Any() && downRights.Any() && !upLefts.Any() && !upRights.Any())
        {
            var downLeftForExtrapolation = downLefts[1];
            var downRightForExtrapolation = downRights[1];

            lowerHorizontal = GetLine(downLefts[0], downRights[0]);
            upperHorizontal = null;
            leftVertical = GetLine(downLefts[0], downLeftForExtrapolation);
            rightVertical = GetLine(downRights[0], downRightForExtrapolation);
        }
        else if (!downLefts.Any() && downRights.Any() && !upLefts.Any() && upRights.Any())
        {
            var downRightForExtrapolation = downRights[1];
            var upRightForExtrapolation = upRights[1];

            lowerHorizontal = GetLine(downRightForExtrapolation, downRights[0]);
            upperHorizontal = GetLine(upRightForExtrapolation, upRights[0]);
            leftVertical = null;
            rightVertical = GetLine(downRights[0], upRights[0]);
        }
        else if (downLefts.Any() && !downRights.Any() && upLefts.Any() && !upRights.Any())
        {
            var downLeftForExtrapolation = downLefts[1];
            var upLeftForExtrapolation = upLefts[1];

            lowerHorizontal = GetLine(downLefts[0], downLeftForExtrapolation);
            upperHorizontal = GetLine(upLefts[0], upLeftForExtrapolation);
            leftVertical = GetLine(downLefts[0], upLefts[0]);
            rightVertical = null;
        }
        else
        {
            // I can't see how this scenario can be possible
            lowerHorizontal = null;
            upperHorizontal = null;
            leftVertical = null;
            rightVertical = null;
        }
        
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
                var lowerHorizontalIntersect = lowerHorizontal.GetIntersect(verticalThroughPoint);
                var upperHorizontalIntersect = upperHorizontal.GetIntersect(verticalThroughPoint);
                var leftVerticalIntersect = leftVertical.GetIntersect(horizontalThroughPoint);
                var rightVerticalIntersect = rightVertical.GetIntersect(horizontalThroughPoint);
                
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

        if (useHorizontals)
        {
            // TODO: horizontal start and end points might not be first in a direction, might be based on extrapolated in same direction
            // TODO: bundle up lines and corresponding points into data structures
            var downLeft = downLefts[0];
            var downRight = downRights[0];
            var upLeft = upLefts[0];
            var upRight = upRights[0];
            
            var lowerBoundary = new Segment(downLeft, downRight);
            var (lowerIntersect, lowerC, lowerH) = GetBoundaryIntersect(lowerBoundary, verticalThroughPoint);
            
            var upperBoundary = new Segment(upLeft, upRight);
            var (upperIntersect, upperC, upperH) = GetBoundaryIntersect(upperBoundary, verticalThroughPoint);

            double h;
            double c;
            if (lowerIntersect == upperIntersect)
            {
                c = lowerC;
                h = lowerH;
            }
            else
            {
                var totalIntersectDistance = GetDistance(lowerIntersect, upperIntersect);
                var distanceToTarget = GetDistance(lowerIntersect, targetPoint);
                var interpolationDistance = distanceToTarget / totalIntersectDistance;
                c = Interpolation.Interpolate(lowerC, upperC, interpolationDistance);
                h = Interpolation.Interpolate(lowerH, upperH, interpolationDistance);
            }

            return (h, c);
        }
        else
        {
            // TODO: vertical start and end points might not be first in a direction, might be based on extrapolated in same direction
            // TODO: bundle up lines and corresponding points into data structures
            var downLeft = downLefts[0];
            var downRight = downRights[0];
            var upLeft = upLefts[0];
            var upRight = upRights[0];
            
            var leftBoundary = new Segment(downLeft, upLeft);
            var (leftIntersect, leftC, leftH) = GetBoundaryIntersect(leftBoundary, horizontalThroughPoint);
            
            var rightBoundary = new Segment(downRight, upRight);
            var (rightIntersect, rightC, rightH) = GetBoundaryIntersect(rightBoundary, horizontalThroughPoint);

            double h;
            double c;
            if (leftIntersect == rightIntersect)
            {
                c = leftC;
                h = leftH;
            }
            else
            {
                var totalIntersectDistance = GetDistance(leftIntersect, rightIntersect);
                var distanceToTarget = GetDistance(leftIntersect, targetPoint);
                var interpolationDistance = distanceToTarget / totalIntersectDistance;
                c = Interpolation.Interpolate(leftC, rightC, interpolationDistance);
                h = Interpolation.Interpolate(leftH, rightH, interpolationDistance);
            }

            return (h, c);
        }
    }
    
    private static ((double x, double y) intersect, double c, double h) GetBoundaryIntersect(Segment boundary, Line throughTarget)
    {
        (double x, double y) intersect;
        double c;
        double h;
        
        if (boundary.IsSingularity)
        {
            c = boundary.Start.Chroma;
            h = boundary.Start.HueDegrees;
            intersect = (boundary.Start.X, boundary.Start.Y);
        }
        else
        {
            intersect = boundary.Line.GetIntersect(throughTarget);
            var boundaryTotalLength = GetDistance(boundary.Start.Point, boundary.End.Point);
            var boundaryToIntersectLength = GetDistance(boundary.Start.Point, intersect);
            var distance = boundaryToIntersectLength / boundaryTotalLength;
            c = Interpolation.Interpolate(boundary.Start.Chroma, boundary.End.Chroma, distance);
            h = Interpolation.Interpolate(boundary.Start.HueDegrees, boundary.End.HueDegrees, distance);
        }

        return (intersect, c, h);
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
        var bandLocation = degrees / DegreesPerHueLetter;
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

    private static Line GetLine(Data point1, Data point2)
    {
        return Line.FromPoints((point1.X, point1.Y), (point2.X, point2.Y));
    }
    
    private record Segment(Data Start, Data End)
    {
        internal readonly Data Start = Start;
        internal readonly Data End = End;
        internal readonly Line Line = Line.FromPoints(Start.Point, End.Point);
        internal readonly bool IsSingularity = Start == End;
    }
}