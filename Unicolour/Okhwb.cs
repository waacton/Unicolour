namespace Wacton.Unicolour;

public record Okhwb : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double W => Second;
    public double B => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedW => ConstrainedSecond;
    public double ConstrainedB => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => W.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => B.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => ConstrainedW + ConstrainedB >= 1.0;

    public Okhwb(double h, double w, double b) : this(h, w, b, ColourHeritage.None) {}
    internal Okhwb(double h, double w, double b, ColourHeritage heritage) : base(h, w, b, heritage) {}
    
    protected override string String => UseAsHued ? $"{H:F1}° {W * 100:F1}% {B * 100:F1}%" : $"—° {W * 100:F1}% {B * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * OKHWB is a transform of OKHSV
     * Forward: https://bottosson.github.io/posts/colorpicker/#okhwb
     * Reverse: https://bottosson.github.io/posts/colorpicker/#okhwb
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    internal static Okhwb FromOkhsv(Okhsv okhsv)
    {
        var (h, s, v) = okhsv.ConstrainedTriplet;
        var w = (1 - s) * v;
        var b = 1 - v;
        return new Okhwb(h, w, b, ColourHeritage.From(okhsv));
    }
    
    internal static Okhsv ToOkhsv(Okhwb okhwb)
    {
        var (h, w, b) = okhwb.ConstrainedTriplet;

        double v;
        double s;
        if (okhwb.IsGreyscale)
        {
            v = w / (w + b);
            s = 0;
        }
        else
        {
            v = 1 - b;
            s = v == 0.0 ? 0 : 1 - w / v;
        }
        
        return new Okhsv(h, s, v, ColourHeritage.From(okhwb));
    }
}