namespace Wacton.Unicolour;

public record Rgb255 : ColourTriplet
{
    public int R => (int)Values[0];
    public int G => (int)Values[1];
    public int B => (int)Values[2];
    
    protected override bool HasAchromaticValues => R == G && G == B;
    
    public Rgb255 Clipped => new(R.Clamp(0, 255), G.Clamp(0, 255), B.Clamp(0, 255), Limitation);
    
    internal bool IsInGamut => !IsNaN && this == Clipped;
    public string Hex => !IsInGamut ? "-" : $"#{R:X2}{G:X2}{B:X2}";

    public Rgb255(double r, double g, double b) : this(r, g, b, Limitation.None) {}
    public Rgb255(double grey) : this(grey, grey, grey, Limitation.Achromatic) {}
    internal Rgb255(double r, double g, double b, Limitation limitation) : base(r, g, b, limitation) {}
    
    protected override string String => $"{R} {G} {B}";
    public override string ToString() => base.ToString();
}