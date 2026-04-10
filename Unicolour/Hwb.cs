using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Hwb : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double W => Second;
    public double B => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Hwb(double h, double w, double b) : this(h, w, b, Limitation.None) {}
    public Hwb(double w) : this(0, w, 1 - w, Limitation.Achromatic) {}
    internal Hwb(double h, double w, double b, Limitation limitation) : base(h, w, b, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {W * 100:F1}% {B * 100:F1}%" : $"{NoHue}° {W * 100:F1}% {B * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HWB is a transform of HSB (in terms of Unicolour implementation)
     * Forward: https://en.wikipedia.org/wiki/HWB_color_model#Conversion
     * Reverse: https://en.wikipedia.org/wiki/HWB_color_model#Conversion
     */
    
    internal static Hwb FromHsb(Hsb hsb)
    {
        var (h, s, v) = hsb.WithHueModulo();
        var w = (1 - s) * v;
        var b = 1 - v;
        return new Hwb(h, w, b, hsb.Limitation);
    }
    
    internal static Hsb ToHsb(Hwb hwb)
    {
        var (h, w, b) = hwb.WithHueModulo();

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
        
        return new Hsb(h, s, v, hwb.Limitation);
    }
}