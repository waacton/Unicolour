using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Oklch : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double L => First;
    public double C => Second;
    public double H => Third;
    public double ConstrainedH => ConstrainedThird;
    protected override double ConstrainedThird => H.Modulo(360.0);
    internal override bool IsGreyscale => L is <= 0.0 or >= 1.0 || C <= 0.0;

    public Oklch(double l, double c, double h) : this(l, c, h, ColourHeritage.None) {}
    internal Oklch(double l, double c, double h, ColourHeritage heritage) : base(l, c, h, heritage) {}
    
    protected override string String => UseAsHued ? $"{L:F2} {C:F2} {H:F1}°" : $"{L:F2} {C:F2} —°";
    public override string ToString() => base.ToString();
    
    /*
     * OKLCH is a transform of OKLAB 
     * Forward: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     * Reverse: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    internal static Oklch FromOklab(Oklab oklab)
    {
        var (l, c, h) = ToLchTriplet(oklab.L, oklab.A, oklab.B);
        return new Oklch(l, c, h, ColourHeritage.From(oklab));
    }
    
    internal static Oklab ToOklab(Oklch oklch)
    {
        var (l, a, b) = FromLchTriplet(oklch.ConstrainedTriplet);
        return new Oklab(l, a, b, ColourHeritage.From(oklch));
    }
}