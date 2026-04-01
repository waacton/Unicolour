using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Hsl : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double L => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Hsl(double h, double s, double l) : this(h, s, l, Limitation.None) {}
    internal Hsl(double h, double s, double l, Limitation limitation) : base(h, s, l, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {S * 100:F1}% {L * 100:F1}%" : $"{NoHue}° {S * 100:F1}% {L * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HSL is a transform of HSB (in terms of Unicolour implementation)
     * Forward: https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_HSL
     * Reverse: https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_HSV
     */
    
    internal static Hsl FromHsb(Hsb hsb)
    {
        var (h, sv, v) = hsb.WithHueModulo();
        var l = v * (1 - sv / 2);
        var sl = l is 0.0 or 1.0 ? 0 : (v - l) / Math.Min(l, 1 - l);
        return new Hsl(h, sl, l, hsb.Limitation);
    }
    
    internal static Hsb ToHsb(Hsl hsl)
    {
        var (h, sl, l) = hsl.WithHueModulo();
        var v = l + sl * Math.Min(l, 1 - l);
        var sv = v == 0.0 ? 0 : 2 * (1 - l / v);
        return new Hsb(h, sv, v, hsl.Limitation);
    }
}