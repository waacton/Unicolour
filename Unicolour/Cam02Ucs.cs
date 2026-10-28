using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam02Ucs : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double J => First;
    public double A => Second;
    public double B => Third;
    
    protected override bool IsTripletAchromatic => A == 0.0 && B == 0.0;
    
    public Cam02Ucs(double j, double a, double b) : this(j, a, b, Limitation.None) {}
    public Cam02Ucs(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Cam02Ucs(double j, double a, double b, Limitation limitation) : base(j, a, b, limitation) {}

    protected override string String => $"{J:F2} {A:+0.00;-0.00;0.00} {B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * CAM-UCS is a transform of CAM 
     * Forward: https://doi.org/10.1007/978-1-4419-6190-7_2 · https://doi.org/10.1002/col.20227 · https://doi.org/10.48550/arXiv.1802.06067
     * Reverse: https://doi.org/10.1007/978-1-4419-6190-7_2 · https://doi.org/10.1002/col.20227 · https://doi.org/10.48550/arXiv.1802.06067
     */
    
    internal static Cam02Ucs FromCam02(Cam02 cam02)
    {
        var (j, m, h) = cam02.Jmh;
        j = 1.7 * j / (1 + 0.007 * j);
        m = Math.Log(1 + 0.0228 * m) / 0.0228;
        (j, var a, var b) = FromLchTriplet(new(j, m, h));
        return new Cam02Ucs(j, a, b, cam02.Jmh.Limitation);
    }
    
    internal static Cam02 ToCam02(Cam02Ucs ucs, Cam.ViewingConditions view)
    {
        var (j, a, b) = ucs;
        j = j / (1.7 - 0.007 * j);
        (j, var m, var h) = ToLchTriplet(new(j, a, b));
        m = (Math.Exp(0.0228 * m) - 1) / 0.0228;
        return new Cam02(new Cam02Jmh(j, m, h, ucs.Limitation), view);
    }
}