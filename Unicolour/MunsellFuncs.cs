namespace Wacton.Unicolour;

internal static class MunsellFuncs
{
    private static readonly WhitePoint WhitePoint = Illuminant.C.GetWhitePoint(Observer.Degree2);
    private static readonly Chromaticity WhitePointChromaticity = WhitePoint.ToChromaticity();
    private static readonly XyzConfiguration XyzConfig = new(WhitePoint);
    
    internal static Xyy ToXyy(Munsell munsell)
    {
        var (degrees, v, c) = munsell.ConstrainedTriplet;
        var h = new MunsellHue(degrees);
        var bounds = munsell.Bounds;

        var chromaticity = GetChromaticity(h, v, c, bounds);
        var luminance = GetLuminance(v);
        return new Xyy(chromaticity.X, chromaticity.Y, luminance, munsell.Heritage);
    }

    private static Chromaticity GetChromaticity(MunsellHue h, double v, double c, MunsellBounds bounds)
    {
        var lowerNodeV = bounds.LowerV;
        var upperNodeV = bounds.UpperV;
        if (lowerNodeV == upperNodeV)
        {
            return GetXyForValue(lowerNodeV, c, h, bounds, isLowerV: true);
        }
        
        var lower = GetXyForValue(lowerNodeV, c, h, bounds, isLowerV: true);
        var upper = GetXyForValue(upperNodeV, c, h, bounds, isLowerV: false);

        var luminance = GetLuminance(v);
        var lowerLuminance = GetLuminance(lowerNodeV);
        var upperLuminance = GetLuminance(upperNodeV);
        var distance = (luminance - lowerLuminance) / (upperLuminance - lowerLuminance);
        var x = Interpolation.Linear(lower.X, upper.X, distance);
        var y = Interpolation.Linear(lower.Y, upper.Y, distance);
        return new(x, y);
    }
    
    private static Chromaticity GetXyForValue(double nodeV, double c, MunsellHue h, MunsellBounds bounds, bool isLowerV)
    {
        if (nodeV == 0) return WhitePointChromaticity;

        var (lowerH, upperH) = (bounds.LowerH, bounds.UpperH);
        var unwrappedH = Hue.Unwrap(lowerH.Degrees, h.Degrees);
        var hueDistance = Math.Abs(unwrappedH.start - unwrappedH.end) / Node.DegreesPerHueNumber;

        var (lowerNodeC, upperNodeC) = isLowerV ? bounds.BoundC.lowerV : bounds.BoundC.upperV;
        if (lowerNodeC == upperNodeC)
        {
            return GetXyForC(lowerNodeC);
        }

        var lower = GetXyForC(lowerNodeC);
        var upper = GetXyForC(upperNodeC);
        var distance = (c - lowerNodeC) / (upperNodeC - lowerNodeC);
        var x = Interpolation.Linear(lower.X, upper.X, distance);
        var y = Interpolation.Linear(lower.Y, upper.Y, distance);
        return new(x, y);

        Chromaticity GetXyForC(double nodeC)
        {
            if (nodeC == 0) return WhitePointChromaticity;
            
            var lowerXy = MunsellCache.Lookup(lowerH, nodeV, nodeC);
            var upperXy = MunsellCache.Lookup(upperH, nodeV, nodeC);
            
            // extension to algorithm to handle edge cases
            // and allow extrapolation in cases where only one of the hues has chroma data
            // (typically when value is very small)
            if (lowerXy == null || upperXy == null)
            {
                if (lowerXy == null && upperXy == null) return new(double.NaN, double.NaN);
                return (lowerXy ?? upperXy)!;
            }

            if (lowerXy == upperXy)
            {
                return lowerXy;
            }

            var lowerPolar = LineSegment.Polar(WhitePointChromaticity, lowerXy);
            var upperPolar = LineSegment.Polar(WhitePointChromaticity, upperXy);
            (lowerPolar.angle, upperPolar.angle) = Hue.Unwrap(lowerPolar.angle, upperPolar.angle);
            var angle = Interpolation.Linear(lowerPolar.angle, upperPolar.angle, hueDistance);
            var angleDistance = (angle - lowerPolar.angle) / (upperPolar.angle - lowerPolar.angle);

            // because lower and upper hues are consecutive, if they are both on segments of radial interpolation
            // can assume they are on the same segment, and radial interpolation should be used
            var isLowerHueOnRadialInterpolationSegment = IsOnRadialInterpolationHueSegment(nodeV, nodeC, lowerH);
            var isUpperHueOnRadialInterpolationSegment = IsOnRadialInterpolationHueSegment(nodeV, nodeC, upperH);
            var useRadialInterpolation = isLowerHueOnRadialInterpolationSegment && isUpperHueOnRadialInterpolationSegment;
            if (useRadialInterpolation)
            {
                var r = Interpolation.Linear(lowerPolar.radius, upperPolar.radius, angleDistance);
                var x = WhitePointChromaticity.X + r * Math.Cos(Utils.ToRadians(angle));
                var y = WhitePointChromaticity.Y + r * Math.Sin(Utils.ToRadians(angle));
                return new(x, y);
            }
            else
            {
                var x = Interpolation.Linear(lowerXy.X, upperXy.X, angleDistance);
                var y = Interpolation.Linear(lowerXy.Y, upperXy.Y, angleDistance);
                return new(x, y);
            }
        }
    }
    
    internal static (double lower, double upper) ToIntervals(double number, double interval)
    {
        var scaled = number / interval;
        var lower = Math.Floor(scaled) * interval;
        var upper = Math.Ceiling(scaled) * interval;
        return (Math.Round(lower, 1), Math.Round(upper, 1));
    }

    // TODO: REVIEW CAREFULLY! very easy place for a mistake to be made during transcript
    private static bool IsOnRadialInterpolationHueSegment(double nodeV, double nodeC, MunsellHue h)
    {
        return nodeV switch
        {
            1 => nodeC switch
            {
                2 => h.IsBetween((10, "Y"), (5, "YR")) || h.IsBetween((5, "P"), (10, "BG")),
                4 => h.IsBetween((7.5, "Y"), (2.5, "YR")) || h.IsBetween((10, "PB"), (7.5, "BG")),
                6 => h.IsBetween((10, "PB"), (5, "BG")),
                8 => h.IsBetween((7.5, "PB"), (7.5, "B")),
                >= 10 => h.IsBetween((7.5, "PB"), (2.5, "PB")),
                _ => false
            },
            2 => nodeC switch
            {
                2 => h.IsBetween((7.5, "Y"), (5, "YR")) || h.IsBetween((10, "PB"), (7.5, "PB")),
                4 => h.IsBetween((10, "Y"), (2.5, "YR")) || h.IsBetween((10, "PB"), (2.5, "B")),
                6 => h.IsBetween((2.5, "Y"), (7.5, "R")) || h.IsBetween((10, "PB"), (2.5, "B")),
                8 => h.IsBetween((5, "YR"), (7.5, "R")) || h.IsBetween((10, "PB"), (10, "BG")),
                >= 10 => h.IsBetween((7.5, "PB"), (5, "B")),
                _ => false
            },
            3 => nodeC switch
            {
                2 => h.IsBetween((7.5, "GY"), (10, "R")) || h.IsBetween((5, "P"), (5, "B")),
                4 => h.IsBetween((7.5, "GY"), (5, "R")) || h.IsBetween((2.5, "PB"), (5, "BG")),
                6 or 8 or 10 => h.IsBetween((7.5, "GY"), (7.5, "R")) || h.IsBetween((2.5, "P"), (7.5, "BG")),
                >= 12 => h.IsBetween((2.5, "G"), (7.5, "R")) || h.IsBetween((10, "PB"), (7.5, "BG")),
                _ => false
            },
            4 => nodeC switch
            {
                2 or 4 => h.IsBetween((2.5, "G"), (7.5, "R")) || h.IsBetween((5, "P"), (7.5, "BG")),
                6 or 8 => h.IsBetween((10, "GY"), (7.5, "R")) || h.IsBetween((2.5, "P"), (7.5, "BG")),
                >= 10 => h.IsBetween((10, "GY"), (7.5, "R")) || h.IsBetween((10, "PB"), (7.5, "BG")),
                _ => false
            },
            5 => nodeC switch
            {
                2 => h.IsBetween((7.5, "GY"), (5, "R")) || h.IsBetween((5, "P"), (5, "BG")),
                4 or 6 or 8 => h.IsBetween((2.5, "G"), (2.5, "R")) || h.IsBetween((5, "P"), (5, "BG")),
                >= 10 => h.IsBetween((2.5, "G"), (2.5, "R")) || h.IsBetween((2.5, "P"), (5, "BG")),
                _ => false
            },
            6 => nodeC switch
            {
                2 or 4 => h.IsBetween((7.5, "GY"), (5, "R")) || h.IsBetween((7.5, "P"), (5, "BG")),
                6 => h.IsBetween((2.5, "G"), (5, "R")) || h.IsBetween((7.5, "P"), (7.5, "BG")),
                8 or 10 => h.IsBetween((2.5, "G"), (5, "R")) || h.IsBetween((5, "P"), (10, "BG")),
                12 or 14 => h.IsBetween((2.5, "G"), (5, "R")) || h.IsBetween((2.5, "P"), (10, "BG")),
                >= 16 => h.IsBetween((2.5, "G"), (5, "R")) || h.IsBetween((10, "PB"), (10, "BG")),
                _ => false
            },
            7 => nodeC switch
            {
                2 or 4 or 6 => h.IsBetween((2.5, "G"), (5, "R")) || h.IsBetween((5, "P"), (10, "BG")),
                8 => h.IsBetween((2.5, "G"), (2.5, "R")) || h.IsBetween((2.5, "P"), (10, "BG")),
                10 => h.IsBetween((5, "Y"), (5, "R")) || h.IsBetween((2.5, "G"), (10, "Y")) || h.IsBetween((2.5, "P"), (10, "BG")),
                12 => h.IsBetween((7.5, "Y"), (7.5, "R")) || h.IsBetween((2.5, "G"), (10, "Y")) || h.IsBetween((2.5, "P"), (10, "PB")),
                >= 14 => h.IsBetween((5, "YR"), (7.5, "R")) || h.IsBetween((10, "GY"), (2.5, "GY")) || h.IsBetween((2.5, "P"), (10, "BG")),
                _ => false
            },
            8 => nodeC switch
            {
                2 or 4 or 6 or 8 or 10 or 12 => h.IsBetween((10, "GY"), (5, "R")) || h.IsBetween((5, "P"), (10, "BG")),
                >= 14 => h.IsBetween((5, "YR"), (5, "R")) || h.IsBetween((10, "GY"), (2.5, "GY")) || h.IsBetween((5, "P"), (10, "BG")),
                _ => false
            },
            9 => nodeC switch
            {
                2 or 4 => h.IsBetween((10, "GY"), (5, "R")) || h.IsBetween((10, "PB"), (5, "BG")),
                6 or 8 or 10 or 12 or 14 => h.IsBetween((2.5, "G"), (5, "R")),
                >= 16 => h.IsBetween((2.5, "G"), (5, "GY")),
                _ => false
            },
            _ => false
        };
    }

    internal static double GetLuminance(double v)
    {
        if (v <= 0) return 0;
        var y = 1.1914 * v - 0.22533 * Math.Pow(v, 2) + 0.23352 * Math.Pow(v, 3) - 0.020484 * Math.Pow(v, 4) + 0.00081939 * Math.Pow(v, 5);
        return y / 100.0;
    }

    /*
     * the maximum error of the core Y -> V function is 0.0035 (https://doi.org/10.1002/col.5080170308)
     * which isn't bad but compounds during roundtrip conversions and causes algorithm of H and C to interpolate away from the actual V by a small amount
     * however, this error provides a range of potential V from a given Y (e.g. if result V = 5, actual V is between 4.9965 to 5.0035)
     * which can be interpolated using exact Y calculations for a more accurate result (error of 0.000005, obtained from testing)
     * and this process can be repeated for this new max error for even greater accuracy
     * though at a depth of 3 iterations the max error is 5e-15, and further iteration yields no improvement
     */
    internal static readonly double[] IterationDepthError = { 0.0035, 0.000005, 0.00000000005, 0.000000000000005 };
    internal static double GetValue(double y, int iterationDepth = 3)
    {
        if (y <= 0) return 0;

        if (iterationDepth == 0)
        {
            y *= 100;
            return y <= 0.9
                ? 0.87445 * Math.Pow(y, 0.9967)
                : 2.49268 * Math.Pow(y, 1 / 3.0) - 1.5614 - 0.985 / (Math.Pow(0.1073 * y - 3.084, 2) + 7.54)
                  + 0.0133 / Math.Pow(y, 2.3) + 0.0084 * Math.Sin(4.1 * Math.Pow(y, 1 / 3.0) + 1)
                  + 0.0221 / y * Math.Sin(0.39 * (y - 2))
                  - 0.0037 / (0.44 * y) * Math.Sin(1.28 * (y - 0.53));
        }

        iterationDepth--;
        var vEstimate = GetValue(y, iterationDepth);
        var error = IterationDepthError[iterationDepth];
        var vLower = Math.Max(vEstimate - error, 0);
        var vUpper = vEstimate + error;
        
        var yLower = GetLuminance(vLower);
        var yUpper = GetLuminance(vUpper);
        var totalDistance = yUpper - yLower;
        var distance = totalDistance == 0 ? 0 : (y - yLower) / totalDistance;
        return Interpolation.Linear(vLower, vUpper, distance);
    }

    internal static Munsell FromXyy(Xyy xyy)
    {
        var searchResult = Find(xyy);
        var (h, v, c) = searchResult.Converged
            ? searchResult.Iterations.Last().Munsell
            : searchResult.Iterations.OrderBy(x => x.Delta).First().Munsell;

        return new Munsell(h, v, c, xyy.Heritage) { XyyToMunsellSearchResult = searchResult };
    }

    private static XyyToMunsellSearchResult Find(Xyy xyy)
    {
        var lch = Lchab.FromLab(Lab.FromXyz(Xyy.ToXyz(xyy), XyzConfig));

        double initialH;
        double initialC;

        // when LCH is very close to greyscale, LCH values do not providing a meaningful starting point
        // (in particular LCH.C / 5.5 approximation no longer works, resulting in chroma modification of x100,000s, rapidly trending to infinity)
        // so fall back to a less extreme starting point in those cases
        // TODO: is there a better heuristic when LCH is not useful? worth considering, but this is already passing tests
        //       e.g. could use the target angle directly for hue, and target radius for chroma?
        if (lch.C < 0.00005)
        {
            // initialH = (360 - target.angle) + 90 + Node.DegreesPerHueNumber * 2;
            // initialH = initialH.Modulo(360);
            // initialC = target.radius;
            initialH = lch.H;
            initialC = 1;
        }
        else
        {
            initialH = lch.H;
            initialC = lch.C / 5.5;
        }
        
        var munsell = new Munsell(initialH, GetValue(xyy.Luminance), initialC);
        var target = LineSegment.Polar(WhitePointChromaticity, xyy.Chromaticity);
        var iterations = new List<XyyToMunsellIteration>();
        var converged = false;

        if (lch.IsNaN)
        {
            var notNumberIteration = new XyyToMunsellIteration(new Munsell(double.NaN, GetValue(xyy.Luminance), double.NaN), double.NaN);
            iterations.Add(notNumberIteration);
            converged = true;
        }

        const double convergenceThreshold = 0.00001;
        const int maxIterations = 10;
        
        while (!converged && iterations.Count < maxIterations)
        {
            munsell = ModifyHue(munsell, target.angle);
            munsell = ModifyChroma(munsell, target.radius);
            var delta = LineSegment.Distance(xyy.Chromaticity, ToXyy(munsell).Chromaticity);
            converged = delta <= convergenceThreshold;
            iterations.Add(new(munsell, delta));
        }
        
        return new XyyToMunsellSearchResult(iterations, converged);
    }
    
    // note: "angle" in this method refers to polar coordinates of (x, y), not to the hue degrees
    private static Munsell ModifyHue(Munsell munsell, double targetAngle)
    {
        var (_, v, c) = munsell;
        HueToAngleData start = null!;
        HueToAngleData end = new(munsell, targetAngle);
        if (end.IsWhitePoint)
        {
            return new Munsell(v);
        }
        
        var hueStep = end.Unwrapped.targetAngle - end.Unwrapped.angle;
        var converged = false;
        while (!converged)
        {
            start = end;
            end = new HueToAngleData(new(start.Munsell.Hue.Degrees + hueStep, v, c), targetAngle);
            converged = start.IsBelowTarget && end.IsAboveTarget || start.IsAboveTarget && end.IsBelowTarget;
        }

        var (startAngle, endAngle) = Hue.Unwrap(start.Angle, end.Angle);
        var totalDistance = endAngle - startAngle; // extremely rare but end can be directly on target
        var distance = totalDistance == 0 ? 0 : (start.Unwrapped.targetAngle - start.Unwrapped.angle) / totalDistance;
        
        var (startHue, endHue) = Hue.Unwrap(start.Munsell.Hue.Degrees, end.Munsell.Hue.Degrees);
        var h = Interpolation.Linear(startHue, endHue, distance).Modulo(360);
        return new Munsell(h, v, c);
    }
    
    private static Munsell ModifyChroma(Munsell munsell, double targetRadius)
    {
        var (h, v, _) = munsell;
        ChromaToRadiusData start = null!;
        ChromaToRadiusData end = new(munsell, targetRadius);
        if (end.IsWhitePoint)
        {
            return new Munsell(v);
        }
        
        var chromaFactor = end.TargetRadius / end.Radius;
        var converged = false;
        while (!converged)
        {
            start = end;
            end = new ChromaToRadiusData(new(h, v, start.Munsell.C * chromaFactor), targetRadius);
            converged = start.IsBelowTarget && end.IsAboveTarget || start.IsAboveTarget && end.IsBelowTarget;
        }

        var totalDistance = end.Radius - start.Radius; // extremely rare but end can be directly on target
        var distance = totalDistance == 0 ? 0 : (start.TargetRadius - start.Radius) / totalDistance;
        var c = Interpolation.Linear(start.Munsell.C, end.Munsell.C, distance);
        return new Munsell(h, v, c);
    }
    
    private static (double radius, double angle) Polar(Munsell munsell) => LineSegment.Polar(WhitePointChromaticity, ToXyy(munsell).Chromaticity);
    
    private record HueToAngleData
    {
        internal Munsell Munsell { get; }
        internal double Angle { get; }
        internal double Radius { get; }
        internal bool IsWhitePoint => Radius == 0.0;
        internal double TargetAngle { get; }
        internal (double angle, double targetAngle) Unwrapped { get; }
        internal bool IsBelowTarget => Unwrapped.angle <= Unwrapped.targetAngle;
        internal bool IsAboveTarget => Unwrapped.angle >= Unwrapped.targetAngle;
        
        internal HueToAngleData(Munsell munsell, double targetAngle)
        {
            Munsell = munsell;
            (Radius, Angle) = Polar(munsell);
            TargetAngle = targetAngle;
            Unwrapped = Hue.Unwrap(Angle, TargetAngle); 
        }
        
        public override string ToString() => $"{Munsell} ({Angle} vs. {TargetAngle}) --> ({Unwrapped.angle} vs. {Unwrapped.targetAngle})";
    }
    
    private record ChromaToRadiusData
    {
        internal Munsell Munsell { get; }
        internal double Angle { get; }
        internal double Radius { get; }
        internal bool IsWhitePoint => Radius == 0.0;
        internal double TargetRadius { get; }
        internal bool IsBelowTarget => Radius <= TargetRadius;
        internal bool IsAboveTarget => Radius >= TargetRadius;
        
        internal ChromaToRadiusData(Munsell munsell, double targetRadius)
        {
            Munsell = munsell;
            (Radius, Angle) = Polar(munsell);
            TargetRadius = targetRadius;
        }
        
        public override string ToString() => $"{Munsell} ({Radius} vs. {TargetRadius})";
    }
    
    internal record XyyToMunsellSearchResult(List<XyyToMunsellIteration> Iterations, bool Converged)
    {
        internal List<XyyToMunsellIteration> Iterations { get; } = Iterations;
        internal bool Converged { get; } = Converged;
        public override string ToString() => $"Iterations:{Iterations.Count} · Converged:{Converged}";
    }
    
    internal record XyyToMunsellIteration(Munsell Munsell, double Delta)
    {
        internal Munsell Munsell { get; } = Munsell;
        internal double Delta { get; } = Delta;
        public override string ToString() => $"{Munsell} · Delta:{Delta}";
    }
}