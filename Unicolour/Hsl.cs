namespace Wacton.Unicolour;

public record Hsl
{
    private readonly bool explicitHue;
    public double H { get; }
    public double S { get; }
    public double L { get; }
    public ColourTriplet Triplet => new(H, S, L);
    
    public double ConstrainedH => H.Modulo(360.0);
    public double ConstrainedS => S.Clamp(0.0, 1.0);
    public double ConstrainedL => L.Clamp(0.0, 1.0);
    public ColourTriplet ConstrainedTriplet => new(ConstrainedH, ConstrainedS, ConstrainedL);
    
    public bool HasHue => explicitHue || S > 0.0 && L is > 0.0 and < 1.0;
    
    public Hsl(double h, double s, double l) : this(h, s, l, true) {}
    internal Hsl(double h, double s, double l, bool explicitHue)
    {
        H = h;
        S = s;
        L = l;
        this.explicitHue = explicitHue;
    }

    public override string ToString() => $"{(HasHue ? Math.Round(H, 1) : "—")}° {Math.Round(S * 100, 1)}% {Math.Round(L * 100, 1)}%";
}