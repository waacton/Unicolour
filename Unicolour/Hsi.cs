namespace Wacton.Unicolour;

public record Hsi : ColourRepresentation
{
    protected override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double I => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedS => ConstrainedSecond;
    public double ConstrainedI => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => S.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => I.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => S <= 0.0 || I <= 0.0;

    public Hsi(double h, double s, double i) : this(h, s, i, ColourHeritage.None) {}
    internal Hsi(double h, double s, double i, ColourHeritage heritage) : base(h, s, i, heritage) {}

    protected override string FirstString => UseAsHued ? $"{H:F1}°" : "—°";
    protected override string SecondString => $"{S * 100:F1}%";
    protected override string ThirdString => $"{I * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * HSI is a transform of RGB
     * Forward: https://en.wikipedia.org/wiki/HSL_and_HSV#Formal_derivation
     * Reverse: https://en.wikipedia.org/wiki/HSL_and_HSV#HSI_to_RGB
     */
    
    internal static Hsi FromRgb(Rgb rgb)
    {
        // avoid constrained values because HSI can easily be out of RGB gamut
        var (r, g, b) = rgb.Triplet;
        var components = new[] { r, g, b };
        var xMin = components.Min();

        var h = Hsb.GetHue(r, g, b);
        var i = (r + g + b) / 3.0;
        var s = i == 0.0 ? 0 : 1 - xMin / i;
        return new Hsi(h.Modulo(360.0), s, i, ColourHeritage.From(rgb));
    }
    
    internal static Rgb ToRgb(Hsi hsi)
    {
        // avoid constrained values because HSI can easily be out of RGB gamut
        var (_, s, i) = hsi.Triplet;
        var h = hsi.ConstrainedH;
        var hPrime = h / 60;
        var z = 1 - Math.Abs(hPrime % 2 - 1);
        var chroma = 3 * i * s / (1 + z);
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
        return new Rgb(r, g, b, ColourHeritage.From(hsi));
    }
}