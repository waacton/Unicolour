namespace Wacton.Unicolour;
using static Hue;

public partial record Munsell : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double V => Second;
    public double C => Third;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => Math.Max(V, 0);
    protected override double ConstrainedThird => Math.Max(C, 0);
    internal override bool IsGreyscale => V <= 0.0 || C <= 0.0;

    public Munsell(double h1, string h2, double v, double c) : this(FromMunsell(h1, h2), v, c, ColourHeritage.None) { }
    public Munsell(double v) : this(0, v, 0, ColourHeritage.Greyscale) { }
    internal Munsell(double h, double v, double c) : this(h, v, c, ColourHeritage.None) { }
    internal Munsell(double h, double v, double c, ColourHeritage heritage) : base(h, v, c, heritage) { }
    
    public (double number, string letter) HueNotation => ToMunsell(H);
    protected override string String => UseAsHued ? $"{HueNotation.number:0.##}{HueNotation.letter} {V:0.##}/{C:0.##}" : $"N {V:0.##}/";
    public override string ToString() => base.ToString();
    
    /*
     * Munsell is a transform of XYY
     * Forward: http://dx.doi.org/10.1002/col.20715
     * Reverse: http://dx.doi.org/10.1002/col.20715
     * ----------
     * it would be slightly less computation to treat as a transform of XYZ instead of XYY, and adapt white point from XYZ directly
     * however, this has the minor benefit of retaining the actual XYY conversion within Unicolour when configured with Illuminant C
     * whereas converting to XYZ as part of the transform would always discard chromaticities with negative Y
     * e.g. 5P 10/50 --> XYY 0.0930 -0.0380 1.0000 --> XYZ 0.0000 0.0000 0.0000
     *   vs 5P 10/50 --> XYZ 0.0000  0.0000 0.0000 --> XYY 0.3101 0.3161 0.0000
     */
    
    private static readonly XyzConfiguration XyzConfigC = new(Illuminant.C, Observer.Degree2);
    private static WhitePoint WhitePoint => XyzConfigC.WhitePoint;
    private static Chromaticity WhiteChromaticity => XyzConfigC.WhiteChromaticity;
    
    internal static Munsell FromXyy(Xyy xyy, XyzConfiguration xyzConfig)
    {
        Xyy adaptedXyy;
        if (xyzConfig.WhitePoint != WhitePoint)
        {
            var xyz = Xyy.ToXyz(xyy);
            var adaptedXyz = Adaptation.WhitePoint(xyz, xyzConfig.WhitePoint, WhitePoint, xyzConfig.AdaptationMatrix);
            adaptedXyy = Xyy.FromXyz(adaptedXyz, WhiteChromaticity);
        }
        else
        {
            adaptedXyy = xyy;
        }

        double h, v, c;
        XyyToMunsellSearchResult? searchResult = null;
        if (xyy.UseAsGreyscale)
        {
            (h, v, c) = (0, GetValue(xyy.Luminance), 0);
        }
        else
        {
            ((h, v, c), searchResult) = FindMunsell(adaptedXyy);
        }

        return new Munsell(h, v, c, ColourHeritage.From(xyy)) { XyyToMunsellSearchResult = searchResult };
    }
    
    internal static Xyy ToXyy(Munsell munsell, XyzConfiguration xyzConfig)
    {
        var (h, v, c) = munsell.ConstrainedTriplet;
        var bounds = GetBounds(munsell);

        var chromaticity = munsell.UseAsGreyscale ? WhiteChromaticity : GetChromaticity(h, v, c, bounds);
        var adaptedXyy = new Xyy(chromaticity.X, chromaticity.Y, GetLuminance(v));
        
        Xyy xyy;
        if (xyzConfig.WhitePoint != WhitePoint)
        {
            var adaptedXyz = Xyy.ToXyz(adaptedXyy);
            var xyz = Adaptation.WhitePoint(adaptedXyz, WhitePoint, xyzConfig.WhitePoint, xyzConfig.AdaptationMatrix);
            xyy = Xyy.FromXyz(xyz, WhiteChromaticity);
        }
        else
        {
            xyy = adaptedXyy;
        }

        var (x, y, luminance) = xyy;
        return new Xyy(x, y, luminance, ColourHeritage.From(munsell));
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
        var distance = (y - yLower) / (yUpper - yLower);
        return Interpolation.Linear(vLower, vUpper, distance);
    }
    
    internal static double GetLuminance(double v)
    {
        if (v <= 0) return 0;
        var y = 1.1914 * v - 0.22533 * Math.Pow(v, 2) + 0.23352 * Math.Pow(v, 3) - 0.020484 * Math.Pow(v, 4) + 0.00081939 * Math.Pow(v, 5);
        return y / 100.0;
    }
    
    private static (Munsell munsell, XyyToMunsellSearchResult searchResult) FindMunsell(Xyy xyy)
    {
        var lch = Lchab.FromLab(Lab.FromXyz(Xyy.ToXyz(xyy), XyzConfigC));
        var target = LineSegment.Polar(WhiteChromaticity, xyy.Chromaticity);
        if (lch.IsNaN || double.IsInfinity(target.radius))
        {
            return (new Munsell(double.NaN, GetValue(xyy.Luminance), double.NaN), new XyyToMunsellSearchResult(double.NaN, false));
        }

        double initialH;
        double initialC;
        if (lch.C < 0.00005)
        {
            // when LCH is very close to greyscale, LCH values do not providing a meaningful starting point
            // (in particular LCH.C / 5.5 approximation no longer works, resulting in chroma modification of x100,000s, rapidly trending to infinity)
            // so fall back to a less extreme starting point in those cases
            // note: could potentially find a better heuristic (e.g. use target angle for hue and target radius for chroma)
            // which might speed up convergence, but this is passing tests so more than suitable for now
            initialH = lch.H;
            initialC = 1;
        }
        else
        {
            initialH = lch.H;
            initialC = lch.C / 5.5;
        }
        
        var munsell = new Munsell(initialH, GetValue(xyy.Luminance), initialC);
        var iterations = 0;
        var closestResult = new XyyToMunsellSearchResult(double.MaxValue, false);
        var converged = false;

        const double convergenceThreshold = 0.00001;
        const int maxIterations = 10;
        
        while (!converged && iterations < maxIterations)
        {
            munsell = ModifyHue(munsell, target.angle);
            munsell = ModifyChroma(munsell, target.radius);
            var delta = LineSegment.Distance(xyy.Chromaticity, ToXyy(munsell, XyzConfigC).Chromaticity);
            converged = delta <= convergenceThreshold;
            iterations++;
            
            var result = new XyyToMunsellSearchResult(delta, converged);
            if (result.Delta < closestResult.Delta || double.IsNaN(closestResult.Delta))
            {
                closestResult = result;
            }
        }

        return (munsell, closestResult);
    }
    
    private static Chromaticity GetChromaticity(double h, double v, double c, Bounds bounds)
    {
        var (lowerV, upperV) = (bounds.LowerV, bounds.UpperV);
        if (lowerV == upperV)
        {
            return GetChromaticityForV(lowerV, c, h, bounds, isLowerV: true);
        }
        
        var lower = GetChromaticityForV(lowerV, c, h, bounds, isLowerV: true);
        var upper = GetChromaticityForV(upperV, c, h, bounds, isLowerV: false);

        var luminance = GetLuminance(v);
        var lowerLuminance = GetLuminance(lowerV);
        var upperLuminance = GetLuminance(upperV);
        var distance = (luminance - lowerLuminance) / (upperLuminance - lowerLuminance);
        var x = Interpolation.Linear(lower.X, upper.X, distance);
        var y = Interpolation.Linear(lower.Y, upper.Y, distance);
        return new(x, y);
    }
    
    private static Chromaticity GetChromaticityForV(double nodeV, double c, double h, Bounds bounds, bool isLowerV)
    {
        if (nodeV == 0) return WhiteChromaticity;
        
        var (lowerC, upperC) = isLowerV ? bounds.BoundC.forLowerV : bounds.BoundC.forUpperV;
        if (lowerC == upperC)
        {
            return GetChromaticityForVC(nodeV, lowerC, h, bounds);
        }

        var lower = GetChromaticityForVC(nodeV, lowerC, h, bounds);
        var upper = GetChromaticityForVC(nodeV, upperC, h, bounds);
        var distance = (c - lowerC) / (upperC - lowerC);
        var x = Interpolation.Linear(lower.X, upper.X, distance);
        var y = Interpolation.Linear(lower.Y, upper.Y, distance);
        return new(x, y);
    }
    
    private static Chromaticity GetChromaticityForVC(double nodeV, double nodeC, double h, Bounds bounds)
    {
        if (nodeC == 0) return WhiteChromaticity;
        
        var (lowerH, upperH) = (bounds.LowerH, bounds.UpperH);
        var unwrappedH = Unwrap(lowerH, h);
        var hueDistance = Math.Abs(unwrappedH.start - unwrappedH.end) / DegreesPerHueNumber;
        
        var lowerChromaticity = Node.Lookup(lowerH, nodeV, nodeC);
        var upperChromaticity = Node.Lookup(upperH, nodeV, nodeC);
        
        // extension to algorithm to handle edge cases
        // and allow extrapolation in cases where only one of the hues has chroma data
        // (typically when value is very small)
        if (lowerChromaticity == null || upperChromaticity == null)
        {
            if (lowerChromaticity == null && upperChromaticity == null) return new(double.NaN, double.NaN);
            return (lowerChromaticity ?? upperChromaticity)!;
        }

        if (lowerChromaticity == upperChromaticity)
        {
            return lowerChromaticity;
        }

        var lowerPolar = LineSegment.Polar(WhiteChromaticity, lowerChromaticity);
        var upperPolar = LineSegment.Polar(WhiteChromaticity, upperChromaticity);
        (lowerPolar.angle, upperPolar.angle) = Unwrap(lowerPolar.angle, upperPolar.angle);
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
            var x = WhiteChromaticity.X + r * Math.Cos(Utils.ToRadians(angle));
            var y = WhiteChromaticity.Y + r * Math.Sin(Utils.ToRadians(angle));
            return new(x, y);
        }
        else
        {
            var x = Interpolation.Linear(lowerChromaticity.X, upperChromaticity.X, angleDistance);
            var y = Interpolation.Linear(lowerChromaticity.Y, upperChromaticity.Y, angleDistance);
            return new(x, y);
        }
    }
        
    // easy minor performance gain would be to precalculate degrees of these munsell hues
    internal static bool IsOnRadialInterpolationHueSegment(double nodeV, double nodeC, double h)
    {
        return nodeV switch
        {
            1 => nodeC switch
            {
                2 => IsBetween(h, (10, "Y"), (5, "YR")) || IsBetween(h, (5, "P"), (10, "BG")),
                4 => IsBetween(h, (7.5, "Y"), (2.5, "YR")) || IsBetween(h, (10, "PB"), (7.5, "BG")),
                6 => IsBetween(h, (10, "PB"), (5, "BG")),
                8 => IsBetween(h, (7.5, "PB"), (7.5, "B")),
                >= 10 => IsBetween(h, (7.5, "PB"), (2.5, "PB")),
                _ => false
            },
            2 => nodeC switch
            {
                2 => IsBetween(h, (7.5, "Y"), (5, "YR")) || IsBetween(h, (10, "PB"), (7.5, "PB")),
                4 => IsBetween(h, (10, "Y"), (2.5, "YR")) || IsBetween(h, (10, "PB"), (2.5, "B")),
                6 => IsBetween(h, (2.5, "Y"), (7.5, "R")) || IsBetween(h, (10, "PB"), (2.5, "B")),
                8 => IsBetween(h, (5, "YR"), (7.5, "R")) || IsBetween(h, (10, "PB"), (10, "BG")),
                >= 10 => IsBetween(h, (7.5, "PB"), (5, "B")),
                _ => false
            },
            3 => nodeC switch
            {
                2 => IsBetween(h, (7.5, "GY"), (10, "R")) || IsBetween(h, (5, "P"), (5, "B")),
                4 => IsBetween(h, (7.5, "GY"), (5, "R")) || IsBetween(h, (2.5, "PB"), (5, "BG")),
                6 or 8 or 10 => IsBetween(h, (7.5, "GY"), (7.5, "R")) || IsBetween(h, (2.5, "P"), (7.5, "BG")),
                >= 12 => IsBetween(h, (2.5, "G"), (7.5, "R")) || IsBetween(h, (10, "PB"), (7.5, "BG")),
                _ => false
            },
            4 => nodeC switch
            {
                2 or 4 => IsBetween(h, (2.5, "G"), (7.5, "R")) || IsBetween(h, (5, "P"), (7.5, "BG")),
                6 or 8 => IsBetween(h, (10, "GY"), (7.5, "R")) || IsBetween(h, (2.5, "P"), (7.5, "BG")),
                >= 10 => IsBetween(h, (10, "GY"), (7.5, "R")) || IsBetween(h, (10, "PB"), (7.5, "BG")),
                _ => false
            },
            5 => nodeC switch
            {
                2 => IsBetween(h, (7.5, "GY"), (5, "R")) || IsBetween(h, (5, "P"), (5, "BG")),
                4 or 6 or 8 => IsBetween(h, (2.5, "G"), (2.5, "R")) || IsBetween(h, (5, "P"), (5, "BG")),
                >= 10 => IsBetween(h, (2.5, "G"), (2.5, "R")) || IsBetween(h, (2.5, "P"), (5, "BG")),
                _ => false
            },
            6 => nodeC switch
            {
                2 or 4 => IsBetween(h, (7.5, "GY"), (5, "R")) || IsBetween(h, (7.5, "P"), (5, "BG")),
                6 => IsBetween(h, (2.5, "G"), (5, "R")) || IsBetween(h, (7.5, "P"), (7.5, "BG")),
                8 or 10 => IsBetween(h, (2.5, "G"), (5, "R")) || IsBetween(h, (5, "P"), (10, "BG")),
                12 or 14 => IsBetween(h, (2.5, "G"), (5, "R")) || IsBetween(h, (2.5, "P"), (10, "BG")),
                >= 16 => IsBetween(h, (2.5, "G"), (5, "R")) || IsBetween(h, (10, "PB"), (10, "BG")),
                _ => false
            },
            7 => nodeC switch
            {
                2 or 4 or 6 => IsBetween(h, (2.5, "G"), (5, "R")) || IsBetween(h, (5, "P"), (10, "BG")),
                8 => IsBetween(h, (2.5, "G"), (5, "R")) || IsBetween(h, (2.5, "P"), (10, "BG")),
                10 => IsBetween(h, (5, "Y"), (5, "R")) || IsBetween(h, (2.5, "G"), (10, "Y")) || IsBetween(h, (2.5, "P"), (10, "BG")),
                12 => IsBetween(h, (7.5, "Y"), (7.5, "R")) || IsBetween(h, (2.5, "G"), (10, "Y")) || IsBetween(h, (2.5, "P"), (10, "PB")),
                >= 14 => IsBetween(h, (5, "YR"), (7.5, "R")) || IsBetween(h, (10, "GY"), (2.5, "GY")) || IsBetween(h, (2.5, "P"), (10, "PB")),
                _ => false
            },
            8 => nodeC switch
            {
                2 or 4 or 6 or 8 or 10 or 12 => IsBetween(h, (10, "GY"), (5, "R")) || IsBetween(h, (5, "P"), (10, "BG")),
                >= 14 => IsBetween(h, (5, "YR"), (5, "R")) || IsBetween(h, (10, "GY"), (2.5, "GY")) || IsBetween(h, (5, "P"), (10, "BG")),
                _ => false
            },
            9 => nodeC switch
            {
                2 or 4 => IsBetween(h, (10, "GY"), (5, "R")) || IsBetween(h, (10, "PB"), (5, "BG")),
                6 or 8 or 10 or 12 or 14 => IsBetween(h, (2.5, "G"), (5, "R")),
                >= 16 => IsBetween(h, (2.5, "G"), (5, "GY")),
                _ => false
            },
            _ => false
        };
    }
    
    // only for potential debugging or diagnostics
    internal XyyToMunsellSearchResult? XyyToMunsellSearchResult;
}

internal record XyyToMunsellSearchResult(double Delta, bool Converged)
{
    internal double Delta { get; } = Delta;
    internal bool Converged { get; } = Converged;
    public override string ToString() => $"Delta:{Delta} · Converged:{Converged}";
}