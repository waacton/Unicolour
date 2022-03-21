namespace Wacton.Unicolour;

public record Hsb
{
    private readonly bool explicitHue;
    public double H { get; }
    public double S { get; }
    public double B { get; }
    public ColourTuple Tuple => new(H, S, B);
    
    // RGB(0,0,0) is black, but has no explicit hue (and don't want to assume red)
    // HSB(0,0,0) is black, but want to acknowledge the explicit red hue of 0
    // HSB(240,0,0) is black, but want to acknowledge the explicit blue of 180
    public bool HasHue => explicitHue || S > 0.0 && B > 0.0;
    
    public Hsb(double h, double s, double b) : this(h, s, b, true) {}

    internal Hsb(double h, double s, double b, bool explicitHue)
    {
        h.Guard(0.0, 360.0, "Hue");
        s.Guard(0.0, 1.0, "Saturation");
        b.Guard(0.0, 1.0, "Brightness");

        H = h;
        S = s;
        B = b;
        this.explicitHue = explicitHue;
    }

    public override string ToString() => $"{(HasHue ? Math.Round(H, 1) : "—")}° {Math.Round(S * 100, 1)}% {Math.Round(B * 100, 1)}%";
}