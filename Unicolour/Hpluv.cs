using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Hpluv : ColourTriplet
{
    public double H => this[0];
    public double S => this[1];
    public double L => this[2];
    
    public Hpluv(double h, double s, double l) : this(h, s, l, Limitation.None) {}
    public Hpluv(double l) : this(0, 0, l, Limitation.Achromatic) {}
    internal Hpluv(double h, double s, double l, Limitation limitation) : base(h, s, l, 0, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {S:F1}% {L:F1}%" : $"{NoHue}° {S:F1}% {L:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HPLUV is a transform of LCHUV 
     * Forward: https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L397
     * Reverse: https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L380
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    internal static Hpluv FromLchuv(Lchuv lchuv)
    {
        var (l, c, h) = lchuv.WithHueModulo();
        (var s, l) = l switch
        {
            > 99.9999999 => (0, 100),
            < 0.00000001 => (0, 0),
            _ => (c / CalculateMaxChroma(l) * 100, l)
        };
        
        return new Hpluv(h, s, l, lchuv.Limitation);
    }
    
    internal static Lchuv ToLchuv(Hpluv hpluv)
    {
        var (h, s, l) = hpluv.WithHueModulo();
        (var c, l) = l switch
        {
            > 99.9999999 => (0, 100),
            < 0.00000001 => (0, 0),
            _ => (s == 0 ? 0 : CalculateMaxChroma(l) / 100 * s, l)
        };
        
        return new Lchuv(l, c, h, hpluv.Limitation);
    }
    
    private static double CalculateMaxChroma(double lightness)
    {
        return Hsluv.GetBoundingLines(lightness).Select(DistanceFromOrigin).Min();
    }
    
    private static double DistanceFromOrigin(Line line) => Math.Abs(line.Intercept) / Math.Sqrt(Math.Pow(line.Slope, 2) + 1);
}