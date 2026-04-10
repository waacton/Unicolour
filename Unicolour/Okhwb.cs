namespace Wacton.Unicolour;

public record Okhwb : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double W => Second;
    public double B => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Okhwb(double h, double w, double b) : this(h, w, b, Limitation.None) {}
    public Okhwb(double w) : this(0, w, 1 - w, Limitation.Achromatic) {}
    internal Okhwb(double h, double w, double b, Limitation limitation) : base(h, w, b, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {W * 100:F1}% {B * 100:F1}%" : $"{Utils.NoHue}° {W * 100:F1}% {B * 100:F1}%";
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
        var (h, s, v) = okhsv.WithHueModulo();
        var w = (1 - s) * v;
        var b = 1 - v;
        return new Okhwb(h, w, b, okhsv.Limitation);
    }
    
    internal static Okhsv ToOkhsv(Okhwb okhwb)
    {
        var (h, w, b) = okhwb.WithHueModulo();

        double v, s;
        if (w + b > 1.0)
        {
            v = w / (w + b);
            s = 0;
        }
        else
        {
            v = 1 - b;
            s = v == 0.0 ? 0 : 1 - w / v;
        }
        
        return new Okhsv(h, s, v, okhwb.Limitation);
    }
}