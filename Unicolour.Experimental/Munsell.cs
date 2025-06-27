namespace Wacton.Unicolour.Experimental;

// TODO: double check luminance equations, not particularly accurate roundtrip
// TODO: handle grey ("N" hue e.g. N 5/ ... or 0 chroma e.g. 10YR 5/0)
// TODO: clamp hue between 0 - 10, clamp value to 10, handle extreme chroma values
// TODO: handle boundary cases
//       - input xy is beyond all x-values in the dataset (in either direction; need to extrapolate horizontals from 2x closest points in other direction)
//       - input xy is beyond all y-values in the dataset (in either direction; need to extrapolate verticals from 2x closest points in other direction)
//       - input xy is beyond all x- and y-values in the dataset (form 2 segments from the 4 points in the same direction (how to choose pairing?) and extrapolate)
// TODO: construct using string (accounting for both standard and grey variants, allowing decimals)
public partial record Munsell
{
    public double HueNumber { get; private set; }
    public double? Value { get; private set; }
    public double? Chroma { get; private set; }
    public string? HueLetter { get; private set; }
    public double HueDegrees { get; }

    public Munsell(double hueNumber, double value, double chroma, string hueLetter)
    {
        HueNumber = hueNumber;
        Value = value;
        Chroma = chroma;
        HueLetter = hueLetter;
        HueDegrees = ToDegrees(hueNumber, hueLetter);
    }

    public Munsell(double greyNumber)
    {
        HueNumber = greyNumber;
        Value = null;
        Chroma = null;
        HueLetter = null;
    }

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

            (double x, double y) upperIntersect;
            double upperC;
            double upperH;
            if (closestUpLeft == closestUpRight)
            {
                upperC = closestUpLeft.c;
                upperH = ToDegrees(closestUpLeft.h, closestUpLeft.letter);
                upperIntersect = (closestUpLeft.x, closestUpRight.y);
            }
            else
            {
                upperIntersect = upperHorizontal.GetIntersect(vertical);
                var totalUpperDistance = GetDistance((closestUpLeft.x, closestUpLeft.y), (closestUpRight.x, closestUpRight.y));
                var distanceToUpperIntersect = GetDistance((closestUpLeft.x, closestUpLeft.y), upperIntersect);
                var upperInterpolationDistance = distanceToUpperIntersect / totalUpperDistance;
                upperC = Interpolation.Interpolate(closestUpLeft.c, closestUpRight.c, upperInterpolationDistance);
                upperH = Interpolation.Interpolate(ToDegrees(closestUpLeft.h, closestUpLeft.letter), ToDegrees(closestUpRight.h, closestUpRight.letter), upperInterpolationDistance);
            }
            
            (double x, double y) lowerIntersect;
            double lowerC;
            double lowerH;
            if (closestDownLeft == closestDownRight)
            {
                lowerC = closestDownLeft.c;
                lowerH = ToDegrees(closestDownLeft.h, closestDownLeft.letter);
                lowerIntersect = (closestDownLeft.x, closestDownLeft.y);
            }
            else
            {
                lowerIntersect = lowerHorizontal.GetIntersect(vertical);
                var totalLowerDistance = GetDistance((closestDownLeft.x, closestDownLeft.y), (closestDownRight.x, closestDownRight.y));
                var distanceToLowerIntersect = GetDistance((closestDownLeft.x, closestDownRight.y), lowerIntersect);
                var lowerInterpolationDistance = distanceToLowerIntersect / totalLowerDistance;
                lowerC = Interpolation.Interpolate(closestDownLeft.c, closestDownRight.c, lowerInterpolationDistance);
                lowerH = Interpolation.Interpolate(ToDegrees(closestDownLeft.h, closestDownLeft.letter), ToDegrees(closestDownRight.h, closestDownRight.letter), lowerInterpolationDistance);
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
                var distanceToTarget = GetDistance(lowerIntersect, (x, y));
                var interpolationDistance = distanceToTarget / totalIntersectDistance;
                c = Interpolation.Interpolate(lowerC, upperC, interpolationDistance);
                h = Interpolation.Interpolate(lowerH, upperH, interpolationDistance);
            }

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
            var leftH = Interpolation.Interpolate(ToDegrees(closestUpLeft.h, closestUpLeft.letter), ToDegrees(closestDownLeft.h, closestDownLeft.letter), leftInterpolationDistance);
            
            var rightIntersect = rightVertical.GetIntersect(horizontal);
            var totalRightDistance = GetDistance((closestUpRight.x, closestUpRight.y), (closestDownRight.x, closestDownRight.y));
            var distanceToRightIntersect = GetDistance((closestUpRight.x, closestUpRight.y), rightIntersect);
            var rightInterpolationDistance = distanceToRightIntersect / totalRightDistance;
            var rightC = Interpolation.Interpolate(closestUpRight.c, closestDownRight.c, rightInterpolationDistance);
            var rightH = Interpolation.Interpolate(ToDegrees(closestUpRight.h, closestUpRight.letter), ToDegrees(closestDownRight.h, closestDownRight.letter), rightInterpolationDistance);

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
        var h = FromDegrees(Interpolation.Interpolate(lowerH, upperH, vDistance));
        var c = Interpolation.Interpolate(lowerC, upperC, vDistance);
        
        // TODO: consider taking verticals into account as well? maybe result is average of horizontal and vertical calcs?
        //       also, in boundary case where there are no data points further in an x-direction, could try using verticals instead
        //       (if there are no suitable horizontals or verticals bounding the point, would need to extrapolate from 2 points in one direction?)
        // var (lowerH2, lowerC2) = GetHCFromVerticals(lowerV);
        // var (upperH2, upperC2) = GetHCFromVerticals(upperV);
        //
        // var vDistance2 = v - lowerV;
        // var h2 = FromDegrees(Interpolation.Interpolate(lowerH2, upperH2, vDistance));
        // var c2 = Interpolation.Interpolate(lowerC2, upperC2, vDistance);
        
        return new Munsell(h.number, v, c, h.letter);
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

        if (lowerV == upperV)
        {
            return GetChromaBoundaries(lowerV);
        }
        
        var lower = GetChromaBoundaries(lowerV);
        var upper = GetChromaBoundaries(upperV);
        
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
        
        if (lowerC == upperC)
        {
            return GetHueBoundaries(boundValue, lowerC);
        }
        
        var lower = GetHueBoundaries(boundValue, lowerC);
        var upper = GetHueBoundaries(boundValue, upperC);
        
        // TODO: ensure works when null chroma
        var distance = upperC - lowerC == 0 ? 0 : ((double)Chroma - lowerC) / (upperC - lowerC);
        var x = Interpolation.Interpolate(lower.X, upper.X, distance);
        var y = Interpolation.Interpolate(lower.Y, upper.Y, distance);
        return new Chromaticity(x, y);
    }

    private Chromaticity GetHueBoundaries(double boundValue, double boundChroma)
    {
        var hueResolution = DegreesPerBand / HueNumbers.Length; // in each band of 36 degrees... 2.5 = 9 degrees, 5 = 18 degrees, 7.5 = 27 degrees, 10 = 36 degrees
        var scaledHue = HueDegrees / hueResolution; // maps 0 - 360 to 0 - 40 (10 bands with 4 hue points per band)
        var lowerDegrees = Math.Floor(scaledHue) * hueResolution;
        var upperDegrees = Math.Ceiling(scaledHue) * hueResolution;

        var lowerH = FromDegrees(lowerDegrees);
        var upperH = FromDegrees(upperDegrees);
        
        if (lowerH == upperH)
        {
            var exact = Data.Value.Single(x => x.h == lowerH.number && x.letter == lowerH.letter && x.v == boundValue && x.c == boundChroma);
            return new Chromaticity(exact.x, exact.y);
        }
        
        // ReSharper disable CompareOfFloatsByEqualityOperator
        var lower = Data.Value.Single(x => x.h == lowerH.number && x.letter == lowerH.letter && x.v == boundValue && x.c == boundChroma);
        var upper = Data.Value.Single(x => x.h == upperH.number && x.letter == upperH.letter && x.v == boundValue && x.c == boundChroma);
        // ReSharper restore CompareOfFloatsByEqualityOperator
        
        var distance = upperDegrees - lowerDegrees == 0 ? 0 : (HueDegrees - lowerDegrees) / (upperDegrees - lowerDegrees);
        var x = Interpolation.Interpolate(lower.x, upper.x, distance);
        var y = Interpolation.Interpolate(lower.y, upper.y, distance);
        return new Chromaticity(x, y);
    }

    // 360/0 = 10R · 36 = 10YR · 72 = 10Y · 108 = 10GY · 144 = 10G
    // 180 = 10BG · 216 = 10B · 252 = 10PB · 288 = 10P · 324 = 10RP
    // (treat 10R == 0YR etc)
    private static readonly string[] Bands = { "YR", "Y", "GY", "G", "BG", "B", "PB", "P", "RP", "R" };
    private const double DegreesPerBand = 36;
    private static double ToDegrees(double hueNumber, string hueLetter)
    {
        var bandIndex = Array.IndexOf(Bands, hueLetter);

        var minDegrees = bandIndex * DegreesPerBand;
        var maxDegrees = (bandIndex + 1) * DegreesPerBand;
        var distance = hueNumber / 10.0; // maps 0 - 10 to 0 - 1
        return Interpolation.Interpolate(minDegrees, maxDegrees, distance);
    }

    private static (double number, string letter) FromDegrees(double degrees)
    {
        var bandLocation = degrees / DegreesPerBand;
        var bandIndex = (int)Math.Truncate(bandLocation);
        var hueLetter = Bands[bandIndex];
        var hueNumber = (bandLocation - bandIndex) * 10;

        if (hueNumber == 0)
        {
            bandIndex = bandIndex == 0 ? Bands.Length - 1 : bandIndex - 1;
            hueLetter = Bands[bandIndex];
            hueNumber = 10;
        }
        
        return (hueNumber, hueLetter);
    }

    public override string ToString()
    {
        var roundedDegrees = Math.Round(HueDegrees);
        var roundedHue = FromDegrees(roundedDegrees);
        return $"{roundedHue.number:F1}{roundedHue.letter} {Value:F1}/{Chroma:F1} ({HueDegrees:F2}°)";
    } 
}

