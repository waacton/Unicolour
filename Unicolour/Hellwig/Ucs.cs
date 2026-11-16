using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Hellwig;

public record Ucs : ColourTriplet
{
    public double J => this[0];
    public double A => this[1];
    public double B => this[2];
    
    protected override bool HasAchromaticValues => A == 0.0 && B == 0.0;
    
    public Ucs(double j, double a, double b) : this(j, a, b, Limitation.None) {}
    public Ucs(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Ucs(double j, double a, double b, Limitation limitation) : base(j, a, b, limitation) {}

    protected override string String => $"{J:F2} {A:+0.00;-0.00;0.00} {B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    internal static Ucs FromCam(Cam cam)
    {
        var (j, m, h) = cam.Jmh;
        j = 1.7 * j / (1 + 0.007 * j);
        m = Math.Log(1 + 0.0228 * m) / 0.0228;
        (j, var a, var b) = FromLchTriplet(new(j, m, h));
        return new Ucs(j, a, b, cam.Jmh.Limitation);
    }
    
    internal static Cam ToCam(Ucs ucs, ViewingConditions view)
    {
        var (j, a, b) = ucs;
        j = j / (1.7 - 0.007 * j);
        (j, var m, var h) = ToLchTriplet(new(j, a, b));
        m = (Math.Exp(0.0228 * m) - 1) / 0.0228;
        return new Cam(new Jmh(j, m, h, ucs.Limitation), view);
    }
}