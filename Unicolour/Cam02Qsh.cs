using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam02Qsh : ColourTriplet
{
    public double Q => this[0];
    public double S => this[1];
    public double H => this[2];
    
    protected override bool HasAchromaticValues => false;
    
    public Cam02Qsh(double q, double s, double h) : this(q, s, h, Limitation.None) {}
    public Cam02Qsh(double q) : this(q, 0, 0, Limitation.Achromatic) {}
    internal Cam02Qsh(double q, double s, double h, Limitation limitation) : base(q, s, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{Q:F2} {S:F2} {H:F1}°" : $"{Q:F2} {S:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}