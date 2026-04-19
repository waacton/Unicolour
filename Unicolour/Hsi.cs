using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Hsi : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double I => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsTripletAchromatic => false;
    
    public Hsi(double h, double s, double i) : this(h, s, i, Limitation.None) {}
    public Hsi(double i) : this(0, 0, i, Limitation.Achromatic) {}
    internal Hsi(double h, double s, double i, Limitation limitation) : base(h, s, i, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {S * 100:F1}% {I * 100:F1}%" : $"{NoHue}° {S * 100:F1}% {I * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HSI is a transform of RGB
     * Forward: https://en.wikipedia.org/wiki/HSL_and_HSV#Formal_derivation
     * Reverse: https://en.wikipedia.org/wiki/HSL_and_HSV#HSI_to_RGB
     */
    
    internal static Hsi FromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb;
        var components = rgb.ToArray();
        var xMin = components.Min();

        var h = Hsb.GetHue(r, g, b).WithHueModulo();
        var i = (r + g + b) / 3.0;
        var s = i == 0.0 ? 0 : 1 - xMin / i;
        return new Hsi(h, s, i, rgb.Limitation);
    }
    
    internal static Rgb ToRgb(Hsi hsi)
    {
        var (h, s, i) = hsi.WithHueModulo();
        var hPrime = h / 60;
        var z = 1 - Math.Abs(hPrime % 2 - 1);
        var chroma = 3 * i * s / (1 + z);
        chroma = Math.Max(chroma, 0); // RGB only represents non-negative chroma (unclear what negative chroma would even mean)
        var x = chroma * z;

        var (r1, g1, b1) = hPrime switch
        {
            < 1 => (chroma, x, 0.0),
            < 2 => (x, chroma, 0.0),
            < 3 => (0.0, chroma, x),
            < 4 => (0.0, x, chroma),
            < 5 => (x, 0.0, chroma),
            < 6 => (chroma, 0.0, x),
            _ => (0.0, 0.0, 0.0)
        };

        var m = i * (1 - s);
        var (r, g, b) = (r1 + m, g1 + m, b1 + m);
        return new Rgb(r, g, b, hsi.Limitation);
    }
}