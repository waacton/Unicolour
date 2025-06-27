namespace Wacton.Unicolour.Experimental;

public partial record Munsell
{
    public double HueNumber { get; private set; }
    public double? Value { get; private set; }
    public double? Chroma { get; private set; }
    public string? HueLetter { get; private set; }

    public Munsell(double hueNumber, double value, double chroma, string hueLetter)
    {
        HueNumber = hueNumber;
        Value = value;
        Chroma = chroma;
        HueLetter = hueLetter;
    }

    public Munsell(double greyNumber)
    {
        HueNumber = greyNumber;
        Value = null;
        Chroma = null;
        HueLetter = null;
    }

    // TODO: handle grey ("N" hue, or 0 chroma)
    public static Xyy ToXyy(Munsell munsell)
    {
        var value = munsell.Value;
        if (!value.HasValue) throw new NotImplementedException();
        
        var v = (double)value;
        // following ASTM standard practice https://doi.org/10.1520/D1535-14R18
        var luminance = 1.1914 * v - 0.22533 * Math.Pow(v, 2) + 0.23352 * Math.Pow(v, 3) - 0.020484 * Math.Pow(v, 4) + 0.00081939 * Math.Pow(v, 5);
        var (x, y) = munsell.GetValueBoundaries();
        return new Xyy(x, y, luminance / 100.0);
    }
    
    public static Munsell FromXyy(Xyy xyy)
    {
        var v = GetValueFromLuminance(xyy.Luminance * 100);
        var lowerV = Values.Last(item => item <= v);
        var upperV = Values.First(item => item >= v);
        
        var (x, y) = xyy.Chromaticity;

        (double h, double c) GetHCFromHorizontals(double boundValue)
        {
            var closestPoints = Data.Value.Where(data => data.v == boundValue).OrderBy(item => GetDistance((x, y), (item.x, item.y))).Take(50).ToList();

            var closestUpRight = closestPoints.First(data => data.x >= x && data.y >= y);
            var closestDownRight = closestPoints.First(data => data.x >= x && data.y <= y);
            var closestUpLeft = closestPoints.First(data => data.x <= x && data.y >= y);
            var closestDownLeft = closestPoints.First(data => data.x <= x && data.y <= y);
            
            var upperHorizontal = Line.FromPoints((closestUpLeft.x, closestUpLeft.y), (closestUpRight.x, closestUpRight.y));
            var lowerHorizontal = Line.FromPoints((closestDownLeft.x, closestDownLeft.y), (closestDownRight.x, closestDownRight.y));
            var vertical = Line.FromPoints((x, y), (x, y + 1));

            var upperIntersect = upperHorizontal.GetIntersect(vertical);
            var totalUpperDistance = GetDistance((closestUpLeft.x, closestUpLeft.y), (closestUpRight.x, closestUpRight.y));
            var distanceToUpperIntersect = GetDistance((closestUpLeft.x, closestUpLeft.y), upperIntersect);
            var upperInterpolationDistance = distanceToUpperIntersect / totalUpperDistance;
            var upperC = Interpolation.Interpolate(closestUpLeft.c, closestUpRight.c, upperInterpolationDistance);
            var upperH = Interpolation.Interpolate(closestUpLeft.h, closestUpRight.h, upperInterpolationDistance);
            
            var lowerIntersect = lowerHorizontal.GetIntersect(vertical);
            var totalLowerDistance = GetDistance((closestDownLeft.x, closestDownLeft.y), (closestDownRight.x, closestDownRight.y));
            var distanceToLowerIntersect = GetDistance((closestDownLeft.x, closestDownRight.y), lowerIntersect);
            var lowerInterpolationDistance = distanceToLowerIntersect / totalLowerDistance;
            var lowerC = Interpolation.Interpolate(closestDownLeft.c, closestDownRight.c, lowerInterpolationDistance);
            var lowerH = Interpolation.Interpolate(closestDownLeft.h, closestDownRight.h, lowerInterpolationDistance);

            var totalIntersectDistance = GetDistance(lowerIntersect, upperIntersect);
            var distanceToTarget = GetDistance(lowerIntersect, (x, y));
            var interpolationDistance = distanceToTarget / totalIntersectDistance;
            var c = Interpolation.Interpolate(lowerC, upperC, interpolationDistance);
            var h = Interpolation.Interpolate(lowerH, upperH, interpolationDistance);
            return (h, c);
        }
        
        (double h, double c) GetHCFromVerticals(double boundValue)
        {
            var closestPoints = Data.Value.Where(data => data.v == boundValue).OrderBy(item => GetDistance((x, y), (item.x, item.y))).Take(50).ToList();

            var closestUpRight = closestPoints.First(data => data.x >= x && data.y >= y);
            var closestDownRight = closestPoints.First(data => data.x >= x && data.y <= y);
            var closestUpLeft = closestPoints.First(data => data.x <= x && data.y >= y);
            var closestDownLeft = closestPoints.First(data => data.x <= x && data.y <= y);
            
            var leftVertical = Line.FromPoints((closestUpLeft.x, closestUpLeft.y), (closestDownLeft.x, closestDownLeft.y));
            var rightVertical = Line.FromPoints((closestUpRight.x, closestUpRight.y), (closestDownRight.x, closestDownRight.y));
            var horizontal = Line.FromPoints((x, y), (x + 1, y));

            var leftIntersect = leftVertical.GetIntersect(horizontal);
            var totalLeftDistance = GetDistance((closestUpLeft.x, closestUpLeft.y), (closestDownLeft.x, closestDownLeft.y));
            var distanceToLeftIntersect = GetDistance((closestUpLeft.x, closestUpLeft.y), leftIntersect);
            var leftInterpolationDistance = distanceToLeftIntersect / totalLeftDistance;
            var leftC = Interpolation.Interpolate(closestUpLeft.c, closestDownLeft.c, leftInterpolationDistance);
            var leftH = Interpolation.Interpolate(closestUpLeft.h, closestDownLeft.h, leftInterpolationDistance);
            
            var rightIntersect = rightVertical.GetIntersect(horizontal);
            var totalRightDistance = GetDistance((closestUpRight.x, closestUpRight.y), (closestDownRight.x, closestDownRight.y));
            var distanceToRightIntersect = GetDistance((closestUpRight.x, closestUpRight.y), rightIntersect);
            var rightInterpolationDistance = distanceToRightIntersect / totalRightDistance;
            var rightC = Interpolation.Interpolate(closestUpRight.c, closestDownRight.c, rightInterpolationDistance);
            var rightH = Interpolation.Interpolate(closestUpRight.h, closestDownRight.h, rightInterpolationDistance);

            var totalIntersectDistance = GetDistance(leftIntersect, rightIntersect);
            var distanceToTarget = GetDistance(leftIntersect, (x, y));
            var interpolationDistance = distanceToTarget / totalIntersectDistance;
            var c = Interpolation.Interpolate(leftC, rightC, interpolationDistance);
            var h = Interpolation.Interpolate(leftH, rightH, interpolationDistance);
            
            return (h, c);
        }

        var (lowerH, lowerC) = GetHCFromHorizontals(lowerV);
        var (upperH, upperC) = GetHCFromHorizontals(upperV);

        var vDistance = v - lowerV;
        var interpolatedH = Interpolation.Interpolate(lowerH, upperH, vDistance);
        var interpolatedC = Interpolation.Interpolate(lowerC, upperC, vDistance);
        
        var (lowerH2, lowerC2) = GetHCFromVerticals(lowerV);
        var (upperH2, upperC2) = GetHCFromVerticals(upperV);

        var vDistance2 = v - lowerV;
        var interpolatedH2 = Interpolation.Interpolate(lowerH2, upperH2, vDistance);
        var interpolatedC2 = Interpolation.Interpolate(lowerC2, upperC2, vDistance);
        
        return new Munsell(interpolatedH, v, interpolatedC, "X");
    }

    private static double GetDistance((double x, double y) point1, (double x, double y) point2)
    {
        return Math.Sqrt(Math.Pow(point2.x - point1.x, 2) + Math.Pow(point2.y - point1.y, 2));
    }

    private static double GetValueFromLuminance(double y)
    {
        return y <= 0.9
            ? 0.87445 * Math.Pow(y, 0.9967)
            : 2.49268 * Math.Pow(y, 1 / 3.0) - 1.5614 - 0.985 / (Math.Pow(0.1073 * y - 3.084, 2) + 7.54)
              + 0.0133 / Math.Pow(y, 2.3) + 0.0084 * Math.Sin(4.1 * Math.Pow(y, 1 / 3.0) + 1)
              + 0.0221 / y * Math.Sin(0.39 * (y - 2))
              - 0.0037 / (0.44 * y) * Math.Sin(1.28 * (y - 0.53));
    }
    
    private Chromaticity GetValueBoundaries()
    {
        var lowerV = Values.Last(x => x <= Value);
        var upperV = Values.First(x => x >= Value);
        
        var lower = GetChromaBoundaries(lowerV);
        var upper = GetChromaBoundaries(upperV);
        
        // TODO: check that distance is correct
        // TODO: ensure works when null chroma
        var distance = upperV - lowerV == 0 ? 0 : ((double)Value - lowerV) / (upperV - lowerV);
        var x = Interpolation.Interpolate(lower.X, upper.X, distance);
        var y = Interpolation.Interpolate(lower.Y, upper.Y, distance);
        return new Chromaticity(x, y);
    }
    
    private Chromaticity GetChromaBoundaries(double boundValue)
    {
        var lowerC = Chromas.Last(x => x <= Chroma);
        var upperC = Chromas.First(x => x >= Chroma);
        
        var lower = GetHueBoundaries(boundValue, lowerC);
        var upper = GetHueBoundaries(boundValue, upperC);
        
        // TODO: check that distance is correct
        // TODO: ensure works when null chroma
        var distance = upperC - lowerC == 0 ? 0 : ((double)Chroma - lowerC) / (upperC - lowerC);
        var x = Interpolation.Interpolate(lower.X, upper.X, distance);
        var y = Interpolation.Interpolate(lower.Y, upper.Y, distance);
        return new Chromaticity(x, y);
    }

    private Chromaticity GetHueBoundaries(double boundValue, double boundChroma)
    {
        var lowerH = HueNumbers.Last(x => x <= HueNumber);
        var upperH = HueNumbers.First(x => x >= HueNumber);
        
        // TODO: if H <= 2.5, set to 10 of previous band
        // TODO: backing hue likely to be 0 - 360, so need a conversion to/from
        // ReSharper disable CompareOfFloatsByEqualityOperator
        var lower = Data.Value.Single(x => x.h == lowerH && x.letter == HueLetter && x.v == boundValue && x.c == boundChroma);
        var upper = Data.Value.Single(x => x.h == upperH && x.letter == HueLetter && x.v == boundValue && x.c == boundChroma);
        // ReSharper restore CompareOfFloatsByEqualityOperator
        
        var distance = upperH - lowerH == 0 ? 0 : (HueNumber - lowerH) / (upperH - lowerH);
        var x = Interpolation.Interpolate(lower.x, upper.x, distance);
        var y = Interpolation.Interpolate(lower.y, upper.y, distance);
        return new Chromaticity(x, y);
    }

    public override string ToString() => $"{HueNumber}{HueLetter} {Value:F1}/{Chroma:F1}";
}

