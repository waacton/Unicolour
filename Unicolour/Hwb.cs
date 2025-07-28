namespace Wacton.Unicolour;

public record Hwb : ColourRepresentation
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

    public Hwb(double h, double w, double b) : this(h, w, b, ColourHeritage.None) {}
    internal Hwb(double h, double w, double b, ColourHeritage heritage) : base(h, w, b, heritage) {}
    
    protected override string String => UseAsHued ? $"{H:F1}° {W * 100:F1}% {B * 100:F1}%" : $"—° {W * 100:F1}% {B * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HWB is a transform of HSB (in terms of Unicolour implementation)
     * Forward: https://en.wikipedia.org/wiki/HWB_color_model#Conversion
     * Reverse: https://en.wikipedia.org/wiki/HWB_color_model#Conversion
     */
    
    internal static Hwb FromHsb(Hsb hsb)
    {
        var (h, s, v) = hsb.ConstrainedTriplet;
        var w = (1 - s) * v;
        var b = 1 - v;
        return new Hwb(h, w, b, ColourHeritage.From(hsb));
    }
    
    internal static Hsb ToHsb(Hwb hwb)
    {
        var (h, w, b) = hwb.ConstrainedTriplet;

        double v;
        double s;
        if (hwb.IsGreyscale)
        {
            v = w / (w + b);
            s = 0;
        }
        else
        {
            v = 1 - b;
            s = v == 0.0 ? 0 : 1 - w / v;
        }
        
        return new Hsb(h, s, v, ColourHeritage.From(hwb));
    }
}