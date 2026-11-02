using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam02Qmh : ColourTriplet
{
    public double Q => this[0];
    public double M => this[1];
    public double H => this[2];
    
    protected override bool HasAchromaticValues => M <= 0;
    
    public Cam02Qmh(double q, double m, double h) : this(q, m, h, Limitation.None) {}
    public Cam02Qmh(double q) : this(q, 0, 0, Limitation.Achromatic) {}
    internal Cam02Qmh(double q, double m, double h, Limitation limitation) : base(q, m, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{Q:F2} {M:F2} {H:F1}°" : $"{Q:F2} {M:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}