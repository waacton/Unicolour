using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Oklrch : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double L => First;
    public double C => Second;
    public double H => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsTripletAchromatic => false;
    
    public Oklrch(double l, double c, double h) : this(l, c, h, Limitation.None) {}
    public Oklrch(double l) : this(l, 0, 0, Limitation.Achromatic) {}
    internal Oklrch(double l, double c, double h, Limitation limitation) : base(l, c, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{L:F2} {C:F2} {H:F1}°" : $"{L:F2} {C:F2} {NoHue}°";
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
        var (l, c, h) = ToLchTriplet(oklrab.Triplet);
        return new Oklrch(l, c, h, oklrab.Limitation);
    }
    
    internal static Oklrab ToOklrab(Oklrch oklrch)
    {
        var (l, a, b) = FromLchTriplet(oklrch.Triplet);
        return new Oklrab(l, a, b, oklrch.Limitation);
    }
}