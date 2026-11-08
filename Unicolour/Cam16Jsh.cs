using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam16Jsh : ColourTriplet
{
    public double J => this[0];
    public double S => this[1];
    public double H => this[2];
    
    public Cam16Jsh(double j, double s, double h) : this(j, s, h, Limitation.None) {}
    public Cam16Jsh(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Cam16Jsh(double j, double s, double h, Limitation limitation) : base(j, s, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {S:F2} {H:F1}°" : $"{J:F2} {S:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}