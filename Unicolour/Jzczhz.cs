namespace Wacton.Unicolour;

public record Jzczhz
{
    public double J { get; }
    public double C { get; }
    public double H { get; }
    public ColourTriplet Triplet => new(J, C, H);

    public double ConstrainedH => H.Modulo(360.0);
    public ColourTriplet ConstrainedTriplet => new(J, C, ConstrainedH);
    
    // RGB(0,0,0) is black, but has no explicit hue (and don't want to assume red)
    // JzCzHz(0,0,0) is black, but want to acknowledge the explicit red hue of 0
    // JzCzHz(0,0,240) is black, but want to acknowledge the explicit blue of 240
    // I'm assuming JCH has the same monochrome behaviour as LCH
    // i.e. no chroma, no lightness, or full lightness
    // (paper says lightness J is 0 - 1 but seems like it's a scaling of their plot of Rec.2020 gamut - in my tests maxes out after ~0.17)
    public bool HasHue => HasExplicitHue || !IsMonochrome;
    internal bool IsMonochrome => ConvertedFromMonochrome || C <= 0.0 || J is <= 0.0 or >= 1.0;
    internal bool HasExplicitHue { get; }
    internal bool ConvertedFromMonochrome { get; }
    
    public Jzczhz(double j, double c, double h) : this(j, c, h, true, false) {}
    internal Jzczhz(double j, double c, double h, bool hasExplicitHue, bool convertedFromMonochrome)
    {
        J = j;
        C = c;
        H = h;
        HasExplicitHue = hasExplicitHue;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }

    public override string ToString() => $"{Math.Round(J, 3)} {Math.Round(C, 3)} {(HasHue ? Math.Round(H, 1) : "—")}°";
}