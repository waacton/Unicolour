using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Oklch : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double L => First;
    public double C => Second;
    public double H => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Oklch(double l, double c, double h) : this(l, c, h, Limitation.None) {}
    public Oklch(double l) : this(l, 0, 0, Limitation.Achromatic) {}
    internal Oklch(double l, double c, double h, Limitation limitation) : base(l, c, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{L:F2} {C:F2} {H:F1}°" : $"{L:F2} {C:F2} {NoHue}°";
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
        var (l, c, h) = ToLchTriplet(oklab.Triplet);
        return new Oklch(l, c, h, oklab.Limitation);
    }
    
    internal static Oklab ToOklab(Oklch oklch)
    {
        var (l, a, b) = FromLchTriplet(oklch.Triplet);
        return new Oklab(l, a, b, oklch.Limitation);
    }
}