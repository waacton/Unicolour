using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Jzczhz : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double J => First;
    public double C => Second;
    public double H => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Jzczhz(double j, double c, double h) : this(j, c, h, Limitation.None) {}
    internal Jzczhz(double j, double c, double h, Limitation limitation) : base(j, c, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{J:F3} {C:F3} {H:F1}°" : $"{J:F3} {C:F3} {NoHue}°";
    public override string ToString() => base.ToString();
    
    /*
     * JZCZHZ is a transform of JZAZBZ 
     * Forward: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     * Reverse: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     */
    
    internal static Jzczhz FromJzazbz(Jzazbz jzazbz)
    {
        var (jz, cz, hz) = ToLchTriplet(jzazbz.Triplet);
        return new Jzczhz(jz, cz, hz, jzazbz.Limitation);
    }
    
    internal static Jzazbz ToJzazbz(Jzczhz jzczhz)
    {
        var (jz, az, bz) = FromLchTriplet(jzczhz.Triplet);
        return new Jzazbz(jz, az, bz, jzczhz.Limitation);
    }
}