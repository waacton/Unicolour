using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Lchab : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double L => First;
    public double C => Second;
    public double H => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Lchab(double l, double c, double h) : this(l, c, h, Limitation.None) {}
    public Lchab(double l) : this(l, 0, 0, Limitation.Achromatic) {}
    internal Lchab(double l, double c, double h, Limitation limitation) : base(l, c, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{L:F2} {C:F2} {H:F1}°" : $"{L:F2} {C:F2} {NoHue}°";
    public override string ToString() => base.ToString();
    
    /*
     * LCHAB is a transform of LAB 
     * Forward: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     * Reverse: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     */
    
    internal static Lchab FromLab(Lab lab)
    {
        var (l, c, h) = ToLchTriplet(lab.Triplet);
        return new Lchab(l, c, h, lab.Limitation);
    }
    
    internal static Lab ToLab(Lchab lchab)
    {
        var (l, a, b) = FromLchTriplet(lchab.Triplet);
        return new Lab(l, a, b, lchab.Limitation);
    }
}