using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam99Qmh : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double Q => First;
    public double M => Second;
    public double H => Third;
    
    protected override bool IsTripletAchromatic => M <= 0;
    
    public Cam99Qmh(double q, double m, double h) : this(q, m, h, Limitation.None) {}
    public Cam99Qmh(double q) : this(q, 0, 0, Limitation.Achromatic) {}
    internal Cam99Qmh(double q, double m, double h, Limitation limitation) : base(q, m, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{Q:F2} {M:F2} {H:F1}°" : $"{Q:F2} {M:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}