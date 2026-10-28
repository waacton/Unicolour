using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam02Jsh : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double J => First;
    public double S => Second;
    public double H => Third;
    
    protected override bool IsTripletAchromatic => S <= 0;
    
    public Cam02Jsh(double j, double s, double h) : this(j, s, h, Limitation.None) {}
    public Cam02Jsh(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Cam02Jsh(double j, double s, double h, Limitation limitation) : base(j, s, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {S:F2} {H:F1}°" : $"{J:F2} {S:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}