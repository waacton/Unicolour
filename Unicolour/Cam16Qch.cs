using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam16Qch : ColourTriplet
{
    public double Q => this[0];
    public double C => this[1];
    public double H => this[2];
    
    public Cam16Qch(double q, double c, double h) : this(q, c, h, Limitation.None) {}
    public Cam16Qch(double q) : this(q, 0, 0, Limitation.Achromatic) {}
    internal Cam16Qch(double q, double c, double h, Limitation limitation) : base(q, c, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{Q:F2} {C:F2} {H:F1}°" : $"{Q:F2} {C:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}