namespace Wacton.Unicolour;

public record Hsb : ColourRepresentation
{
    protected override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double B => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedS => ConstrainedSecond;
    public double ConstrainedB => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => S.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => B.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => S <= 0.0 || B <= 0.0;

    public Hsb(double h, double s, double b) : this(h, s, b, ColourHeritage.None) {}
    internal Hsb(double h, double s, double b, ColourHeritage heritage) : base(h, s, b, heritage) {}

    protected override string FirstString => UseAsHued ? $"{H:F1}°" : "—°";
    protected override string SecondString => $"{S * 100:F1}%";
    protected override string ThirdString => $"{B * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HSB is a transform of RGB
     * Forward: https://en.wikipedia.org/wiki/HSL_and_HSV#From_RGB
     * Reverse: https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB
     */
    
    internal static Hsb FromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb.ConstrainedTriplet;
        var components = new[] { r, g, b };
        var xMax = components.Max();
        var xMin = components.Min();
        var chroma = xMax - xMin;

        var h = GetHue(r, g, b);
        var v = xMax;
        var s = v == 0 ? 0 : chroma / v;
        return new Hsb(h.Modulo(360.0), s, v, ColourHeritage.From(rgb));
    }
    
    internal static Rgb ToRgb(Hsb hsb)
    {
        var (h, s, v) = hsb.ConstrainedTriplet;
        var hPrime = h / 60.0;
        var chroma = v * s;
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
        return new Rgb(r, g, b, ColourHeritage.From(hsb));
    }

    internal static double GetHue(double r, double g, double b)
    {
        var components = new[] { r, g, b };
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