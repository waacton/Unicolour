namespace Wacton.Unicolour.Tests.Utils;

using System.Collections.Generic;
using System.Linq;

internal class TestColour
{
    public string? Name { get; init; }
    public string? Hex { get; init; }
    public ColourTuple? Rgb { get; init; }
    public ColourTuple? RgbLinear { get; init; }
    public ColourTuple? Hsl { get; init; }
    public ColourTuple? Hsb { get; init; }
    public ColourTuple? Xyz { get; init; }
    public ColourTuple? Lab { get; init; }
    public Tolerances? Tolerances { get; init; }

    /*
     * both ColorMine and SixLabors behave strangely with HSL
     * from what I can tell, they almost certainly convert HSB -> RGB -> HSL
     * and as a result they have some oddities when it comes to HSL:
     * 1) they set hue to 0 if there is no HSB saturation (since the intermediate RGB is greyscale, the hue gets lost in conversion)
     * 2) SixLabors sets saturation to 0 if RGB chroma (max - min) is < 0.001
     * 3) ColorMine truncates intermediate RGB 255 values during conversion, resulting in full saturation if any RGB 255 value is < 1 (truncated to 0)
     * --- no point in trying to compensate for their lossy conversions, just ignore them in the test
     */
    public bool ExcludeFromHueBasedTest => ExcludeFromHueBasedTestReasons.Any();
    public List<string> ExcludeFromHueBasedTestReasons { get; init; } = new();

    public override string ToString() => $"Name:[{Name}] · Hex[{Hex}] · Rgb[{Rgb}] · Hsb[{Hsb}]";
}

internal record Tolerances 
{
    public double Rgb, RgbLinear, Hsb, Hsl, Xyz, Lab;
}
