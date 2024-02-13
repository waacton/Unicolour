namespace Wacton.Unicolour;

public record Hsluv : ColourRepresentation
{
    protected override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double L => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedS => ConstrainedSecond;
    public double ConstrainedL => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => S.Clamp(0.0, 100.0);
    protected override double ConstrainedThird => L.Clamp(0.0, 100.0);
    internal override bool IsGreyscale => S <= 0.0 || L is <= 0.0 or >= 100.0;

    public Hsluv(double h, double s, double l) : this(h, s, l, ColourHeritage.None) {}
    internal Hsluv(double h, double s, double l, ColourHeritage heritage) : base(h, s, l, heritage) {}

    protected override string FirstString => UseAsHued ? $"{H:F1}°" : "—°";
    protected override string SecondString => $"{S:F1}%";
    protected override string ThirdString => $"{L:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HSLUV is a transform of LCHUV 
     * Forward: https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L363
     * Reverse: https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L346
     */
    
    internal static Hsluv FromLchuv(Lchuv lchuv)
    {
        var (lchLightness, chroma, hue) = lchuv.ConstrainedTriplet;
        double saturation;
        double lightness;

        switch (lchLightness)
        {
            case > 99.9999999:
                saturation = 0.0;
                lightness = 100.0;
                break;
            case < 0.00000001:
                saturation = 0.0;
                lightness = 0.0;
                break;
            default:
            {
                var maxChroma = CalculateMaxChroma(lchLightness, hue);
                saturation = chroma / maxChroma * 100;
                lightness = lchLightness;
                break;
            }
        }
        
        return new Hsluv(hue, saturation, lightness, ColourHeritage.From(lchuv));
    }
    
    internal static Lchuv ToLchuv(Hsluv hsluv)
    {
        var hue = hsluv.ConstrainedH;
        var saturation = hsluv.S;
        var hslLightness = hsluv.L;
        double lightness;
        double chroma;

        switch (hslLightness)
        {
            case > 99.9999999:
                lightness = 100.0;
                chroma = 0.0;
                break;
            case < 0.00000001:
                lightness = 0.0;
                chroma = 0.0;
                break;
            default:
            {
                var maxChroma = CalculateMaxChroma(hslLightness, hue);
                chroma = maxChroma / 100 * saturation;
                lightness = hslLightness;
                break;
            }
        }
        
        return new Lchuv(lightness, chroma, hue, ColourHeritage.From(hsluv));
    }

    private static double CalculateMaxChroma(double lightness, double hue)
    {
        var hueRad = hue / 360 * Math.PI * 2;
        return GetBoundingLines(lightness).Select(x => DistanceFromOriginAngle(hueRad, x)).Min();
    }
    
    private static double DistanceFromOriginAngle(double theta, Line line)
    {
        var distance = line.Intercept / (Math.Sin(theta) - line.Slope * Math.Cos(theta));
        return distance < 0 ? double.PositiveInfinity : distance;
    }
    
    // https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L249
    internal static IEnumerable<Line> GetBoundingLines(double l)
    {
        const double kappa = 903.2962962;
        const double epsilon = 0.0088564516;
        var matrixR = Matrix.FromTriplet(3.240969941904521, -1.537383177570093, -0.498610760293);
        var matrixG = Matrix.FromTriplet(-0.96924363628087, 1.87596750150772, 0.041555057407175);
        var matrixB = Matrix.FromTriplet(0.055630079696993, -0.20397695888897, 1.056971514242878);
        
        var sub1 = Math.Pow(l + 16, 3) / 1560896;
        var sub2 = sub1 > epsilon ? sub1 : l / kappa;

        IEnumerable<Line> CalculateLines(Matrix matrix)
        {
            var s1 = sub2 * (284517 * matrix[0, 0] - 94839 * matrix[2, 0]);
            var s2 = sub2 * (838422 * matrix[2, 0] + 769860 * matrix[1, 0] + 731718 * matrix[0, 0]);
            var s3 = sub2 * (632260 * matrix[2, 0] - 126452 * matrix[1, 0]);
        
            var slope0 = s1 / s3;
            var intercept0 = s2 * l / s3;
            var slope1 = s1 / (s3 + 126452);
            var intercept1 = (s2 - 769860) * l / (s3 + 126452);
            return new[] { new Line(slope0, intercept0), new Line(slope1, intercept1) };
        }

        var lines = new List<Line>();
        lines.AddRange(CalculateLines(matrixR));
        lines.AddRange(CalculateLines(matrixG));
        lines.AddRange(CalculateLines(matrixB));
        return lines;
    }
}