using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Hsluv : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double L => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsTripletAchromatic => false;
    
    public Hsluv(double h, double s, double l) : this(h, s, l, Limitation.None) {}
    public Hsluv(double l) : this(0, 0, l, Limitation.Achromatic) {}
    internal Hsluv(double h, double s, double l, Limitation limitation) : base(h, s, l, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {S:F1}% {L:F1}%" : $"{NoHue}° {S:F1}% {L:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HSLUV is a transform of LCHUV 
     * Forward: https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L363
     * Reverse: https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L346
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    internal static Hsluv FromLchuv(Lchuv lchuv)
    {
        var (l, c, h) = lchuv.WithHueModulo();
        (var s, l) = l switch
        {
            > 99.9999999 => (0, 100),
            < 0.00000001 => (0, 0),
            _ => (c / CalculateMaxChroma(l, h) * 100, l)
        };
        
        return new Hsluv(h, s, l, lchuv.Limitation);
    }
    
    internal static Lchuv ToLchuv(Hsluv hsluv)
    {
        var (h, s, l) = hsluv.WithHueModulo();
        (var c, l) = l switch
        {
            > 99.9999999 => (0, 100),
            < 0.00000001 => (0, 0),
            _ => (s == 0 ? 0 : CalculateMaxChroma(l, h) / 100 * s, l)
        };
        
        return new Lchuv(l, c, h, hsluv.Limitation);
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
        var matrixR = Matrix.From(3.240969941904521, -1.537383177570093, -0.498610760293);
        var matrixG = Matrix.From(-0.96924363628087, 1.87596750150772, 0.041555057407175);
        var matrixB = Matrix.From(0.055630079696993, -0.20397695888897, 1.056971514242878);
        
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
            return [new Line(slope0, intercept0), new Line(slope1, intercept1)];
        }

        List<Line> lines = [];
        lines.AddRange(CalculateLines(matrixR));
        lines.AddRange(CalculateLines(matrixG));
        lines.AddRange(CalculateLines(matrixB));
        return lines;
    }
}