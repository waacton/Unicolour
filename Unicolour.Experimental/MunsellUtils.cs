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
            var closestDownLeft = downLefts[0];
            var closestDownRight = downRights[0];
            
            (double x, double y) lowerIntersect;
            double lowerC;
            double lowerH;
            if (closestDownLeft == closestDownRight)
            {
                lowerC = closestDownLeft.c;
                lowerH = Munsell.ToDegrees(closestDownLeft.h, closestDownLeft.letter);
                lowerIntersect = (closestDownLeft.x, closestDownLeft.y);
            }
            else
            {
                lowerIntersect = lowerHorizontal.GetIntersect(verticalThroughPoint);
                var totalLowerDistance = GetDistance((closestDownLeft.x, closestDownLeft.y), (closestDownRight.x, closestDownRight.y));
                var distanceToLowerIntersect = GetDistance((closestDownLeft.x, closestDownRight.y), lowerIntersect);
                var lowerInterpolationDistance = distanceToLowerIntersect / totalLowerDistance;
                lowerC = Interpolation.Interpolate(closestDownLeft.c, closestDownRight.c, lowerInterpolationDistance);
                lowerH = Interpolation.Interpolate(Munsell.ToDegrees(closestDownLeft.h, closestDownLeft.letter), Munsell.ToDegrees(closestDownRight.h, closestDownRight.letter), lowerInterpolationDistance);
            }
            
            var closestUpLeft = upLefts[0];
            var closestUpRight = upRights[0];
            
            (double x, double y) upperIntersect;
            double upperC;
            double upperH;
            if (closestUpLeft == closestUpRight)
            {
                upperC = closestUpLeft.c;
                upperH = Munsell.ToDegrees(closestUpLeft.h, closestUpLeft.letter);
                upperIntersect = (closestUpLeft.x, closestUpRight.y);
            }
            else
            {
                upperIntersect = upperHorizontal.GetIntersect(verticalThroughPoint);
                var totalUpperDistance = GetDistance((closestUpLeft.x, closestUpLeft.y), (closestUpRight.x, closestUpRight.y));
                var distanceToUpperIntersect = GetDistance((closestUpLeft.x, closestUpLeft.y), upperIntersect);
                var upperInterpolationDistance = distanceToUpperIntersect / totalUpperDistance;
                upperC = Interpolation.Interpolate(closestUpLeft.c, closestUpRight.c, upperInterpolationDistance);
                upperH = Interpolation.Interpolate(Munsell.ToDegrees(closestUpLeft.h, closestUpLeft.letter), Munsell.ToDegrees(closestUpRight.h, closestUpRight.letter), upperInterpolationDistance);
            }

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
            throw new NotImplementedException("Implement as above for verticals");
        }
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