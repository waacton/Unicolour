using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Hpluv : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double L => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Hpluv(double h, double s, double l) : this(h, s, l, Limitation.None) {}
    internal Hpluv(double h, double s, double l, Limitation limitation) : base(h, s, l, limitation) {}

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
        var maxC = CalculateMaxChroma(l);
        var s = lchuv.Limitation == Limitation.Achromatic || double.IsNaN(maxC) ? 0 : c / maxC * 100;
        return new Hpluv(h, s, l, lchuv.Limitation);
    }
    
    internal static Lchuv ToLchuv(Hpluv hpluv)
    {
        var (h, s, l) = hpluv.WithHueModulo();
        var maxC = CalculateMaxChroma(l);
        var c = double.IsNaN(maxC) || s == 0 ? 0 : maxC / 100 * s;
        return new Lchuv(l, c, h, hpluv.Limitation);
    }
    
    private static double CalculateMaxChroma(double lightness)
    {
        return Hsluv.GetBoundingLines(lightness).Select(DistanceFromOrigin).Min();
    }
    
    private static double DistanceFromOrigin(Line line) => Math.Abs(line.Intercept) / Math.Sqrt(Math.Pow(line.Slope, 2) + 1);
}