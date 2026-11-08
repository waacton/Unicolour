using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Cam02;

public record Jmh : ColourTriplet
{
    public double J => this[0];
    public double M => this[1];
    public double H => this[2];
    
    public Jmh(double j, double m, double h) : this(j, m, h, Limitation.None) {}
    public Jmh(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Jmh(double j, double m, double h, Limitation limitation) : base(j, m, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {M:F2} {H:F1}°" : $"{J:F2} {M:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}