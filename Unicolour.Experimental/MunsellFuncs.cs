using static Wacton.Unicolour.Experimental.Munsell;


namespace Wacton.Unicolour.Experimental;

internal static class MunsellFuncs
{
    // TODO: use white point from illuminant after testing
    // private static Chromaticity WhitePoint = Illuminant.C.GetWhitePoint(Observer.Degree2).ToChromaticity();
    private static Chromaticity WhitePoint = new(0.31006, 0.31616);
    private static readonly XyzConfiguration XyzConfig = new(WhitePoint.ToWhitePoint());
    
    internal static Xyy ToXyy(Munsell munsell)
    {
        var h = munsell.Hue;
        var v = munsell.V;
        var c = munsell.C;
        
        var luminance = GetLuminance(v);

        Chromaticity chromaticity;
        
        // TODO: what if V is out of range?
        var lowerNodeV = NodeValues.Last(nodeV => nodeV <= v);
        var upperNodeV = NodeValues.First(nodeV => nodeV >= v);
        if (lowerNodeV == upperNodeV)
        {
            chromaticity = GetXyForValue(lowerNodeV, c, h);
        }
        else
        {
            var lower = GetXyForValue(lowerNodeV, c, h);
            var upper = GetXyForValue(upperNodeV, c, h);

            var lowerLuminance = GetLuminance(lowerNodeV);
            var upperLuminance = GetLuminance(upperNodeV);
            var distance = (luminance - lowerLuminance) / (upperLuminance - lowerLuminance);
            var x = Interpolation.Linear(lower.X, upper.X, distance);
            var y = Interpolation.Linear(lower.Y, upper.Y, distance);
            chromaticity = new(x, y);
        }

        return new Xyy(chromaticity.X, chromaticity.Y, luminance);
    }
    
    private static Chromaticity GetXyForValue(double nodeV, double c, MunsellHue h)
    {
        if (nodeV == 0) return WhitePoint;
        
        var scaled = h.Degrees / DegreesPerHueNumber; // maps 0-360 to 0-40 (10 letter bands with 4 numbers per band)
        MunsellHue lowerH = new(Math.Floor(scaled) * DegreesPerHueNumber);
        MunsellHue upperH = new(Math.Ceiling(scaled) * DegreesPerHueNumber);
        
        // interpolation along the chroma ovoid is the core of this algorithm
        // so 2 nodes of the same chroma between hues are required for this to work
        var maxC = new[] { MaxChroma(nodeV, lowerH), MaxChroma(nodeV, upperH) }.Min();
        
        // TODO: unsure if using max chroma only and then extrapolating from hue is acceptable
        //       doesn't seem roundtrippable with the more accurate algorithm, but could fall back to my basic interpolation?
        // var lowerNodeC = c > maxC ? NodeChromas.Last(nodeC => nodeC < maxC) : NodeChromas.Last(nodeC => nodeC <= c);
        // var upperNodeC = c > maxC ? maxC : NodeChromas.First(nodeC => nodeC >= c);
        
        if (c > maxC) throw new NotImplementedException($"No data for both {lowerH} and {upperH} at chroma {c} (max chroma {maxC})");
        var lowerNodeC = NodeChromas.Last(nodeC => nodeC <= c);
        var upperNodeC = NodeChromas.First(nodeC => nodeC >= c);
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
            if (nodeC == 0) return WhitePoint;
            
            // consecutive hues, same chroma
            var node1 = Nodes.Value.SingleOrDefault(x => x.IsMatch(lowerH, nodeV, nodeC));
            var node2 = Nodes.Value.SingleOrDefault(x => x.IsMatch(upperH, nodeV, nodeC));
            if (node1 == node2)
            {
                var exact = Nodes.Value.SingleOrDefault(x => x.IsMatch(lowerH, nodeV, nodeC));
                return exact.Point;
            }

            var polar1 = LineSegment.Polar(WhitePoint, node1.Point);
            var polar2 = LineSegment.Polar(WhitePoint, node2.Point);

            var hueDistance = (h.Degrees - lowerH.Degrees) / (upperH.Degrees - lowerH.Degrees);
            var angle = Interpolation.Linear(polar1.angle, polar2.angle, hueDistance);
            var angleDistance = (angle - polar1.angle) / (polar2.angle - polar1.angle);

            // because lower and upper hues are consecutive, if they are both on segments of radial interpolation
            // can assume they are on the same segment, and radial interpolation should be used
            var isLowerHueOnRadialInterpolationSegment = IsOnRadialInterpolationHueSegment(nodeV, nodeC, lowerH);
            var isUpperHueOnRadialInterpolationSegment = IsOnRadialInterpolationHueSegment(nodeV, nodeC, upperH);
            var useRadialInterpolation = isLowerHueOnRadialInterpolationSegment && isUpperHueOnRadialInterpolationSegment;
            if (useRadialInterpolation)
            {
                var r = Interpolation.Linear(polar1.radius, polar2.radius, angleDistance);
                var x = WhitePoint.X + r * Math.Cos(angle);
                var y = WhitePoint.Y + r * Math.Sin(angle);
                return new(x, y);
            }
            else
            {
                var x = Interpolation.Linear(node1.X, node2.X, angleDistance);
                var y = Interpolation.Linear(node1.Y, node2.Y, angleDistance);
                return new(x, y);
            }
        }
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
                6 => h.IsBetween((2.5, "G"), (2.5, "R")) || h.IsBetween((7.5, "P"), (7.5, "BG")),
                8 or 10 => h.IsBetween((2.5, "G"), (2.5, "R")) || h.IsBetween((5, "P"), (10, "BG")),
                12 or 14 => h.IsBetween((2.5, "G"), (2.5, "R")) || h.IsBetween((2.5, "P"), (10, "BG")),
                >= 16 => h.IsBetween((2.5, "G"), (2.5, "R")) || h.IsBetween((10, "PB"), (10, "BG")),
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
    
    private static int MaxChroma(double nodeV, MunsellHue nodeH)
    {
        for (var i = NodeChromas.Length - 1; i >= 0; i--)
        {
            var nodeC = NodeChromas[i];
            var result = Nodes.Value.SingleOrDefault(x => x.IsMatch(nodeH, nodeV, nodeC));
            if (result != null) return nodeC;
        }

        throw new NotImplementedException($"No chroma found for {nodeH}, {nodeV})!");
    }

    internal static double GetLuminance(double v)
    {
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
        var vLower = vEstimate - error;
        var vUpper = vEstimate + error;
        
        var yLower = GetLuminance(vLower);
        var yUpper = GetLuminance(vUpper);
        var distance = (y - yLower) / (yUpper - yLower);
        return Interpolation.Linear(vLower, vUpper, distance);
    }

    internal static Munsell FromXyy(Xyy xyy)
    {
        var target = LineSegment.Polar(WhitePoint, xyy.Chromaticity, degrees: true);
        var lch = Lchab.FromLab(Lab.FromXyz(Xyy.ToXyz(xyy), XyzConfig));
        
        var munsell = new Munsell(lch.H, GetValue(xyy.Luminance), lch.C / 5.5);
        var delta = double.MaxValue;
        var iterations = 0;
        
        do
        {
            munsell = ModifyHue(munsell, target.angle);
            munsell = ModifyChroma(munsell, target.radius);
            delta = LineSegment.Distance(xyy.Chromaticity, ToXyy(munsell).Chromaticity);
            iterations++;
        } while (delta > 0.000001 && iterations < 20);

        return munsell;
    }
    
    // note: "angle" in this method refers to polar coordinates of (x, y), not to the hue degrees
    private static Munsell ModifyHue(Munsell munsell, double targetAngle)
    {
        // TODO: deconstruction when a colour representation
        var v = munsell.V;
        var c = munsell.C;
        
        var (_, angle) = Polar(munsell);
        var hueStep = targetAngle - angle;
        var isIncreasing = hueStep > 0;

        (Munsell munsell, double angle) start;
        (Munsell munsell, double angle) end = (munsell, angle);

        do
        {
            start = end;
            var adjustedMunsell = new Munsell(end.munsell.Hue.Degrees + hueStep, v, c);
            end = (adjustedMunsell, Polar(adjustedMunsell).angle);
        } while (isIncreasing ? end.angle < targetAngle : end.angle > targetAngle);
        
        var distance = (targetAngle - start.angle) / (end.angle - start.angle);
        var h = Interpolation.Linear(start.munsell.Hue.Degrees, end.munsell.Hue.Degrees, distance);
        return new Munsell(h, v, c);
    }
    
    private static Munsell ModifyChroma(Munsell munsell, double targetRadius)
    {
        // TODO: deconstruction when a colour representation
        var h = munsell.Hue.Degrees;
        var v = munsell.V;
        
        var (radius, _) = Polar(munsell);
        var chromaFactor = targetRadius / radius;
        var isIncreasing = targetRadius > radius;
        
        (Munsell munsell, double radius) start;
        (Munsell munsell, double radius) end = (munsell, radius);

        do
        {
            start = end;
            var adjustedMunsell = new Munsell(h, v, end.munsell.C * chromaFactor);
            end = (adjustedMunsell, Polar(adjustedMunsell).radius);
        } while (isIncreasing ? end.radius < targetRadius : end.radius > targetRadius);
        
        var distance = (targetRadius - start.radius) / (end.radius - start.radius);
        var c = Interpolation.Linear(start.munsell.C, end.munsell.C, distance);
        return new Munsell(h, v, c);
    }
    
    private static (double radius, double angle) Polar(Munsell munsell)
    {
        return LineSegment.Polar(WhitePoint, ToXyy(munsell).Chromaticity, degrees: true);
    }
}