using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Cam02;

public record Qch : ColourTriplet
{
    public double Q => this[0];
    public double C => this[1];
    public double H => this[2];
    
    public Qch(double q, double c, double h) : this(q, c, h, Limitation.None) {}
    public Qch(double q) : this(q, 0, 0, Limitation.Achromatic) {}
    internal Qch(double q, double c, double h, Limitation limitation) : base(q, c, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{Q:F2} {C:F2} {H:F1}°" : $"{Q:F2} {C:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}