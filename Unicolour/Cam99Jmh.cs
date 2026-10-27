using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Cam99Jmh : ColourRepresentation
{
    protected internal override int? HueIndex => 2;
    public double J => First;
    public double M => Second;
    public double H => Third;
    
    protected override bool IsTripletAchromatic => M <= 0;
    
    public Cam99Jmh(double j, double m, double h) : this(j, m, h, Limitation.None) {}
    public Cam99Jmh(double j) : this(j, 0, 0, Limitation.Achromatic) {}
    internal Cam99Jmh(double j, double m, double h, Limitation limitation) : base(j, m, h, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {M:F2} {H:F1}°" : $"{J:F2} {M:F2} {NoHue}°";
    public override string ToString() => base.ToString();
}