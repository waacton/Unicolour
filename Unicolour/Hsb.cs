using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Hsb : ColourTriplet
{
    public double H => this[0];
    public double S => this[1];
    public double B => this[2];
    
    public Hsb(double h, double s, double b) : this(h, s, b, Limitation.None) {}
    public Hsb(double b) : this(0, 0, b, Limitation.Achromatic) {}
    internal Hsb(double h, double s, double b, Limitation limitation) : base(h, s, b, 0, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {S * 100:F1}% {B * 100:F1}%" : $"{NoHue}° {S * 100:F1}% {B * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HSB is a transform of RGB
     * Forward: https://en.wikipedia.org/wiki/HSL_and_HSV#From_RGB
     * Reverse: https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB
     */
    
    internal static Hsb FromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb;
        var xMax = rgb.Values.Max();
        var xMin = rgb.Values.Min();
        var chroma = xMax - xMin;

        var h = GetHue(r, g, b).WithHueModulo();
        var v = xMax;
        var s = v == 0 ? 0 : chroma / v;
        return new Hsb(h, s, v, rgb.Limitation);
    }
    
    internal static Rgb ToRgb(Hsb hsb)
    {
        var (h, s, v) = hsb.WithHueModulo();
        var hPrime = h / 60.0;
        var chroma = v * s;
        chroma = Math.Max(chroma, 0); // RGB only represents non-negative chroma (unclear what negative chroma would even mean)
        var x = chroma * (1 - Math.Abs(hPrime % 2 - 1));

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

        var m = v - chroma;
        var (r, g, b) = (r1 + m, g1 + m, b1 + m);
        return new Rgb(r, g, b, hsb.Limitation);
    }

    internal static double GetHue(double r, double g, double b)
    {
        double[] components = [r, g, b];
        var xMax = components.Max();
        var xMin = components.Min();
        var chroma = xMax - xMin;

        if (chroma == 0.0) return 0;
        if (xMax == r) return 60 * (0 + (g - b) / chroma);
        if (xMax == g) return 60 * (2 + (b - r) / chroma);
        if (xMax == b) return 60 * (4 + (r - g) / chroma);
        return double.NaN;
    }
}