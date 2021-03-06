namespace Wacton.Unicolour;

public record Lchab
{
    public double L { get; }
    public double C { get; }
    public double H { get; }
    public ColourTriplet Triplet => new(L, C, H);

    public double ConstrainedH => H.Modulo(360.0);
    public ColourTriplet ConstrainedTriplet => new(L, C, ConstrainedH);
    
    // RGB(0,0,0) is black, but has no explicit hue (and don't want to assume red)
    // LCH(0,0,0) is black, but want to acknowledge the explicit red hue of 0
    // LCH(0,0,240) is black, but want to acknowledge the explicit blue of 240
    public bool HasHue => HasExplicitHue || !IsMonochrome;
    internal bool IsMonochrome => ConvertedFromMonochrome || C <= 0.0 || L is <= 0.0 or >= 100.0;
    internal bool HasExplicitHue { get; }
    internal bool ConvertedFromMonochrome { get; }
    
    public Lchab(double l, double c, double h) : this(l, c, h, true, false) {}
    internal Lchab(double l, double c, double h, bool hasExplicitHue, bool convertedFromMonochrome)
    {
        L = l;
        C = c;
        H = h;
        HasExplicitHue = hasExplicitHue;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }

    public override string ToString() => $"{Math.Round(L, 2)}% {Math.Round(C, 2)} {(HasHue ? Math.Round(H, 1) : "—")}°";
}