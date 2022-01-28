namespace Wacton.Unicolour.Tests.Lookups;

internal class TestColour
{
    public string? Name;
    public string? Hex;
    public (double r, double g, double b) Rgb;
    public (double r, double g, double b) RgbLinear;
    public (double h, double s, double l) Hsl;
    public (double h, double s, double b) Hsb;
    public (double x, double y, double z) Xyz;
    public (double l, double a, double b) Lab;
    
    public override string ToString() => $"{Name} · {Hex} · {Hsb}";
}