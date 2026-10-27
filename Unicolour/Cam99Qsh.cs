using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam99Qsh : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double Q => First;
    public double S => Second;
    public double H => Third;
    
    protected override bool IsTripletAchromatic => S <= 0;
    
    public Cam99Qsh(double q, double s, double h) : this(q, s, h, Limitation.None) {}
    public Cam99Qsh(double q) : this(q, 0, 0, Limitation.Achromatic) {}
    internal Cam99Qsh(double q, double s, double h, Limitation limitation) : base(q, s, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{Q:F2} {S:F2} {H:F1}°" : $"{Q:F2} {S:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}