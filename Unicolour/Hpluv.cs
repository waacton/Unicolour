﻿namespace Wacton.Unicolour;

public record Hpluv : ColourRepresentation
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

    public Hpluv(double h, double s, double l) : this(h, s, l, ColourHeritage.None) {}
    internal Hpluv(double h, double s, double l, ColourHeritage heritage) : base(h, s, l, heritage) {}

    protected override string FirstString => UseAsHued ? $"{H:F1}°" : "—°";
    protected override string SecondString => $"{S:F1}%";
    protected override string ThirdString => $"{L:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HPLUV is a transform of LCHUV 
     * Forward: https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L397
     * Reverse: https://github.com/hsluv/hsluv-haxe/blob/master/src/hsluv/Hsluv.hx#L380
     */
        
    internal static Hpluv FromLchuv(Lchuv lchuv)
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
                var maxChroma = CalculateMaxChroma(lchLightness);
                saturation = chroma / maxChroma * 100;
                lightness = lchLightness;
                break;
            }
        }
        
        return new Hpluv(hue, saturation, lightness, ColourHeritage.From(lchuv));
    }
    
    internal static Lchuv ToLchuv(Hpluv hpluv)
    {
        var hue = hpluv.ConstrainedH;
        var saturation = hpluv.S;
        var hslLightness = hpluv.L;
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
                var maxChroma = CalculateMaxChroma(hslLightness);
                chroma = maxChroma / 100 * saturation;
                lightness = hslLightness;
                break;
            }
        }
        
        return new Lchuv(lightness, chroma, hue, ColourHeritage.From(hpluv));
    }
    
    private static double CalculateMaxChroma(double lightness)
    {
        return Hsluv.GetBoundingLines(lightness).Select(DistanceFromOrigin).Min();
    }
    
    private static double DistanceFromOrigin(Line line) => Math.Abs(line.Intercept) / Math.Sqrt(Math.Pow(line.Slope, 2) + 1);
}