using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam02Jch : ColourTriplet
{
    public double J => this[0];
    public double C => this[1];
    public double H => this[2];
    
    protected override bool HasAchromaticValues => C <= 0;
    
    public Cam02Jch(double j, double c, double h) : this(j, c, h, Limitation.None) {}
    public Cam02Jch(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Cam02Jch(double j, double c, double h, Limitation limitation) : base(j, c, h, 2, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {C:F2} {H:F1}°" : $"{J:F2} {C:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}