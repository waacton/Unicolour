using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Oklrch : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double L => First;
    public double C => Second;
    public double H => Third;
    public double ConstrainedH => ConstrainedThird;
    protected override double ConstrainedThird => H.Modulo(360.0);
    internal override bool IsGreyscale => L is <= 0.0 or >= 1.0 || C <= 0.0;

    public Oklrch(double l, double c, double h) : this(l, c, h, ColourHeritage.None) {}
    internal Oklrch(double l, double c, double h, ColourHeritage heritage) : base(l, c, h, heritage) {}
    
    protected override string String => UseAsHued ? $"{L:F2} {C:F2} {H:F1}°" : $"{L:F2} {C:F2} —°";
    public override string ToString() => base.ToString();
    
    /*
     * OKLrCH is a transform of OKLrAB 
     * Forward: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     * Reverse: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    internal static Oklrch FromOklrab(Oklrab oklrab)
    {
        var (l, c, h) = ToLchTriplet(oklrab.L, oklrab.A, oklrab.B);
        return new Oklrch(l, c, h, ColourHeritage.From(oklrab));
    }
    
    internal static Oklrab ToOklrab(Oklrch oklrch)
    {
        var (l, a, b) = FromLchTriplet(oklrch.ConstrainedTriplet);
        return new Oklrab(l, a, b, ColourHeritage.From(oklrch));
    }
}