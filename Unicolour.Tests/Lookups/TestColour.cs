namespace Wacton.Unicolour.Tests.Lookups;

internal class TestColour
{
    public string? Name { get; init; }
    public string? Hex { get; init; }
    public (double r, double g, double b)? Rgb { get; init; }
    public (double r, double g, double b)? RgbLinear { get; init; }
    public (double h, double s, double l)? Hsl { get; init; }
    public (double h, double s, double b)? Hsb { get; init; }
    public (double x, double y, double z)? Xyz { get; init; }
    public (double l, double a, double b)? Lab { get; init; }
    
    public override string ToString() => $"Name:[{Name}] · Hex[{Hex}] · Rgb[{Rgb}] · Hsb[{Hsb}]";
}