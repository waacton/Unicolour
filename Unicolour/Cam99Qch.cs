using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam99Qch : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double Q => First;
    public double C => Second;
    public double H => Third;
    
    protected override bool IsTripletAchromatic => C <= 0;
    
    public Cam99Qch(double q, double c, double h) : this(q, c, h, Limitation.None) {}
    public Cam99Qch(double q) : this(q, 0, 0, Limitation.Achromatic) {}
    internal Cam99Qch(double q, double c, double h, Limitation limitation) : base(q, c, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{Q:F2} {C:F2} {H:F1}°" : $"{Q:F2} {C:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}