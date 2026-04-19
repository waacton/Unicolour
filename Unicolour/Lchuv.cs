using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Lchuv : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double L => First;
    public double C => Second;
    public double H => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsTripletAchromatic => false;
    
    public Lchuv(double l, double c, double h) : this(l, c, h, Limitation.None) {}
    public Lchuv(double l) : this(l, 0, 0, Limitation.Achromatic) {}
    internal Lchuv(double l, double c, double h, Limitation limitation) : base(l, c, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{L:F2} {C:F2} {H:F1}°" : $"{L:F2} {C:F2} {NoHue}°";
    public override string ToString() => base.ToString();
    
    /*
     * LCHUV is a transform of LUV 
     * Forward: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     * Reverse: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     */
    
    internal static Lchuv FromLuv(Luv luv)
    {
        var (l, c, h) = ToLchTriplet(luv.Triplet);
        return new Lchuv(l, c, h, luv.Limitation);
    }
    
    internal static Luv ToLuv(Lchuv lchuv)
    {
        var (l, u, v) = FromLchTriplet(lchuv.Triplet);
        return new Luv(l, u, v, lchuv.Limitation);
    }
}