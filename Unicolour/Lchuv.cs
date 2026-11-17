using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Lchuv : ColourTriplet
{
    public double L => this[0];
    public double C => this[1];
    public double H => this[2];
    
    public Lchuv(double l, double c, double h) : this(l, c, h, Limitation.None) {}
    public Lchuv(double l) : this(l, 0, 0, Limitation.Achromatic) {}
    internal Lchuv(double l, double c, double h, Limitation limitation) : base(l, c, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{L:F2} {C:F2} {H:F1}°" : $"{L:F2} {C:F2} {NoHue}°";
    public override string ToString() => base.ToString();
    
    /*
     * LCHUV is a transform of LUV 
     * Forward: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     * Reverse: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     */
    
    internal static Lchuv FromLuv(Luv luv)
    {
        var (l, c, h) = ToLchTriplet(luv);
        return new Lchuv(l, c, h, luv.Limitation);
    }
    
    internal static Luv ToLuv(Lchuv lchuv)
    {
        var (l, u, v) = FromLchTriplet(lchuv);
        return new Luv(l, u, v, lchuv.Limitation);
    }
}