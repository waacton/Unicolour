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
        var potentialLowers = Data.Value.Where(data => data.v == lowerV).OrderBy(item => GetDistance((x, y), (item.x, item.y))).ToList();
        var potentialUppers = Data.Value.Where(data => data.v == upperV).OrderBy(item => GetDistance((x, y), (item.x, item.y))).ToList();

        var lowerA = potentialLowers[0];
        var lowerB = potentialLowers[1];

        var lowerTotalLength = GetDistance((lowerA.x, lowerA.y), (lowerB.x, lowerB.y));
        var lowerDistanceFromA = GetDistance((x, y), (lowerA.x, lowerA.y));
        var lowerChroma = Interpolation.Interpolate(lowerA.c, lowerB.c, lowerDistanceFromA / lowerTotalLength);
        
        var upperA = potentialUppers[0];
        var upperB = potentialUppers[1];

        var upperTotalLength = GetDistance((upperA.x, upperA.y), (upperB.x, upperB.y));
        var upperDistanceFromA = GetDistance((x, y), (upperA.x, upperA.y));
        var upperChroma = Interpolation.Interpolate(upperA.c, upperB.c, upperDistanceFromA / upperTotalLength);

        var interpolatedChroma = Interpolation.Interpolate(lowerChroma, upperChroma, v - lowerV);
        
        // TODO: for hue... convert to 0-360, interpolate, convert back to notation?
        var hueNumber = lowerA.h;
        var hueLetter = lowerA.letter;
        return new Munsell(hueNumber, v, interpolatedChroma, hueLetter);
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

