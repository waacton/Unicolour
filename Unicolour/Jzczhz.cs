using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Jzczhz : ColourTriplet
{
    public double J => this[0];
    public double C => this[1];
    public double H => this[2];
    
    public Jzczhz(double j, double c, double h) : this(j, c, h, Limitation.None) {}
    public Jzczhz(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Jzczhz(double j, double c, double h, Limitation limitation) : base(j, c, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{J:F3} {C:F3} {H:F1}°" : $"{J:F3} {C:F3} {NoHue}°";
    public override string ToString() => base.ToString();
    
    /*
     * JZCZHZ is a transform of JZAZBZ 
     * Forward: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     * Reverse: https://en.wikipedia.org/wiki/CIELAB_color_space#CIEHLC_cylindrical_model
     */
    
    internal static Jzczhz FromJzazbz(Jzazbz jzazbz)
    {
        var (jz, cz, hz) = ToLchTriplet(jzazbz);
        return new Jzczhz(jz, cz, hz, jzazbz.Limitation);
    }
    
    internal static Jzazbz ToJzazbz(Jzczhz jzczhz)
    {
        var (jz, az, bz) = FromLchTriplet(jzczhz);
        return new Jzazbz(jz, az, bz, jzczhz.Limitation);
    }
}