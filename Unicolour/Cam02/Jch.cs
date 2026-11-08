using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Cam02;

public record Jch : ColourTriplet
{
    public double J => this[0];
    public double C => this[1];
    public double H => this[2];
    
    public Jch(double j, double c, double h) : this(j, c, h, Limitation.None) {}
    public Jch(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Jch(double j, double c, double h, Limitation limitation) : base(j, c, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {C:F2} {H:F1}°" : $"{J:F2} {C:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}