namespace Wacton.Unicolour;

public record Hsb
{
    public double H { get; }
    public double S { get; }
    public double B { get; }
    public ColourTriplet Triplet => new(H, S, B);

    public double ConstrainedH => H.Modulo(360.0);
    public double ConstrainedS => S.Clamp(0.0, 1.0);
    public double ConstrainedB => B.Clamp(0.0, 1.0);
    public ColourTriplet ConstrainedTriplet => new(ConstrainedH, ConstrainedS, ConstrainedB);
    
    // RGB(0,0,0) is black, but has no explicit hue (and don't want to assume red)
    // HSB(0,0,0) is black, but want to acknowledge the explicit red hue of 0
    // HSB(240,0,0) is black, but want to acknowledge the explicit blue of 240
    public bool HasHue => HasExplicitHue || !IsMonochrome;
    internal bool IsMonochrome => ConvertedFromMonochrome || S <= 0.0 || B <= 0.0;
    internal bool HasExplicitHue { get; }
    internal bool ConvertedFromMonochrome { get; }
    
    public Hsb(double h, double s, double b) : this(h, s, b, true, false) {}
    internal Hsb(double h, double s, double b, bool hasExplicitHue, bool convertedFromMonochrome)
    {
        H = h;
        S = s;
        B = b;
        HasExplicitHue = hasExplicitHue;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }

    public override string ToString() => $"{(HasHue ? Math.Round(H, 1) : "—")}° {Math.Round(S * 100, 1)}% {Math.Round(B * 100, 1)}%";
}