namespace Wacton.Unicolour;

public record Rgb255 : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public int R => (int)First;
    public int G => (int)Second;
    public int B => (int)Third;
    
    protected override bool IsAchromatic => R == G && G == B;
    
    public Rgb255 Clipped => new(R.Clamp(0, 255), G.Clamp(0, 255), B.Clamp(0, 255), Limitation);
    
    internal bool IsInGamut => !IsNaN && Triplet == Clipped.Triplet;
    public string Hex => !IsInGamut ? "-" : $"#{R:X2}{G:X2}{B:X2}";

    public Rgb255(double r, double g, double b) : this(r, g, b, Limitation.None) {}
    public Rgb255(double grey) : this(grey, grey, grey, Limitation.Achromatic) {}
    internal Rgb255(double r, double g, double b, Limitation limitation) : base(r, g, b, limitation) {}
    
    protected override string String => $"{R} {G} {B}";
    public override string ToString() => base.ToString();
}