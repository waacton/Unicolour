using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam99Ucs : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double J => First;
    public double A => Second;
    public double B => Third;
    
    protected override bool IsTripletAchromatic => A == 0.0 && B == 0.0;
    
    public Cam99Ucs(double j, double a, double b) : this(j, a, b, Limitation.None) {}
    public Cam99Ucs(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Cam99Ucs(double j, double a, double b, Limitation limitation) : base(j, a, b, limitation) {}

    protected override string String => $"{J:F2} {A:+0.00;-0.00;0.00} {B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * CAM-UCS is a transform of CAM 
     * Forward: https://doi.org/10.1007/978-1-4419-6190-7_2 · https://doi.org/10.1002/col.20227 · https://doi.org/10.48550/arXiv.1802.06067
     * Reverse: https://doi.org/10.1007/978-1-4419-6190-7_2 · https://doi.org/10.1002/col.20227 · https://doi.org/10.48550/arXiv.1802.06067
     */
    
    internal static Cam99Ucs FromCam99(Cam99 cam99)
    {
        var (j, m, h) = cam99.Jmh;
        j = 1.7 * j / (1 + 0.007 * j);
        m = Math.Log(1 + 0.0228 * m) / 0.0228;
        (j, var a, var b) = FromLchTriplet(new(j, m, h));
        return new Cam99Ucs(j, a, b, cam99.Jmh.Limitation);
    }
    
    internal static Cam99 ToCam99(Cam99Ucs ucs, CamConfiguration camConfig)
    {
        var (j, a, b) = ucs;
        j = j / (1.7 - 0.007 * j);
        (j, var m, var h) = ToLchTriplet(new(j, a, b));
        m = (Math.Exp(0.0228 * m) - 1) / 0.0228;
        var view = Cam99.ViewingConditions(camConfig);
        return new Cam99(new Cam99.JMH(j, m, h, ucs.Limitation), view, ucs.Limitation);
    }
}
