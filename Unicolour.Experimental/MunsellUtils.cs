namespace Wacton.Unicolour.Experimental;

internal static class MunsellUtils
{
    internal static (double h, double c) GetHueAndChroma((double x, double y) targetPoint, double valueDataPoint)
    {
        var dataForV = Munsell.Data.Value.Where(data => data.v == valueDataPoint).ToArray(); //.OrderBy(item => GetDistance((x, y), (item.x, item.y))).ToList();
        
        var downLefts = dataForV.Where(data => data.x <= targetPoint.x && data.y <= targetPoint.y).OrderBy(data => GetDistance(targetPoint, (data.x, data.y))).ToArray();
        var downRights = dataForV.Where(data => data.x >= targetPoint.x && data.y <= targetPoint.y).OrderBy(data => GetDistance(targetPoint, (data.x, data.y))).ToArray();
        var upLefts = dataForV.Where(data => data.x <= targetPoint.x && data.y >= targetPoint.y).OrderBy(data => GetDistance(targetPoint, (data.x, data.y))).ToArray();
        var upRights = dataForV.Where(data => data.x >= targetPoint.x && data.y >= targetPoint.y).OrderBy(data => GetDistance(targetPoint, (data.x, data.y))).ToArray();

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
            c = boundary.StartDataPoint.c;
            h = Munsell.ToDegrees(boundary.StartDataPoint.h, boundary.StartDataPoint.letter);
            intersect = (boundary.StartDataPoint.x, boundary.StartDataPoint.y);
        }
        else
        {
            intersect = boundary.Line.GetIntersect(throughTarget);
            var boundaryTotalLength = GetDistance(boundary.Start, boundary.End);
            var boundaryToIntersectLength = GetDistance(boundary.Start, intersect);
            var interpolationDistance = boundaryToIntersectLength / boundaryTotalLength;

            var startC = boundary.StartDataPoint.c;
            var endC = boundary.EndDataPoint.c;
            c = Interpolation.Interpolate(startC, endC, interpolationDistance);

            var startH = Munsell.ToDegrees(boundary.StartDataPoint.h, boundary.StartDataPoint.letter);
            var endH = Munsell.ToDegrees(boundary.EndDataPoint.h, boundary.EndDataPoint.letter);
            h = Interpolation.Interpolate(startH, endH, interpolationDistance);
        }

        return (intersect, c, h);
    }

    private record Segment((double h, string letter, double v, int c, double x, double y, double luminance) StartDataPoint, (double h, string letter, double v, int c, double x, double y, double luminance) EndDataPoint)
    {
        internal readonly (double h, string letter, double v, int c, double x, double y, double luminance) StartDataPoint = StartDataPoint;
        internal readonly (double h, string letter, double v, int c, double x, double y, double luminance) EndDataPoint = EndDataPoint;
        internal readonly (double x, double y) Start = (StartDataPoint.x, StartDataPoint.y);
        internal readonly (double x, double y) End = (EndDataPoint.x, EndDataPoint.y);
        internal readonly Line Line = Wacton.Unicolour.Line.FromPoints((StartDataPoint.x, StartDataPoint.y), (EndDataPoint.x, EndDataPoint.y));
        internal readonly bool IsSingularity = StartDataPoint == EndDataPoint;
    }
    
    private static double GetDistance((double x, double y) point1, (double x, double y) point2)
    {
        return Math.Sqrt(Math.Pow(point2.x - point1.x, 2) + Math.Pow(point2.y - point1.y, 2));
    }

    private static Line GetLine(
        (double h, string letter, double v, int c, double x, double y, double luminance) point1,
        (double h, string letter, double v, int c, double x, double y, double luminance) point2)
    {
        return Line.FromPoints((point1.x, point1.y), (point2.x, point2.y));
    }
}