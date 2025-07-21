namespace Wacton.Unicolour;

public record Hsl : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double L => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedS => ConstrainedSecond;
    public double ConstrainedL => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => S.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => L.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => S <= 0.0 || L is <= 0.0 or >= 1.0;

    public Hsl(double h, double s, double l) : this(h, s, l, ColourHeritage.None) {}
    internal Hsl(double h, double s, double l, ColourHeritage heritage) : base(h, s, l, heritage) {}
    
    protected override string String => UseAsHued ? $"{H:F1}° {S * 100:F1}% {L * 100:F1}%" : $"—° {S * 100:F1}% {L * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HSL is a transform of HSB (in terms of Unicolour implementation)
     * Forward: https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_HSL
     * Reverse: https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_HSV
     */
    
    internal static Hsl FromHsb(Hsb hsb)
    {
        var (h, sv, v) = hsb.ConstrainedTriplet;
        var l = v * (1 - sv / 2);
        var sl = l is > 0.0 and < 1.0
            ? (v - l) / Math.Min(l, 1 - l)
            : 0;

        return new Hsl(h, sl, l, ColourHeritage.From(hsb));
    }
    
    internal static Hsb ToHsb(Hsl hsl)
    {
        var (h, sl, l) = hsl.ConstrainedTriplet;
        var v = l + sl * Math.Min(l, 1 - l);
        var sv = v > 0.0
            ? 2 * (1 - l / v)
            : 0;

        return new Hsb(h, sv, v, ColourHeritage.From(hsl));
    }
}