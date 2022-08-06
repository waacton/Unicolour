namespace Wacton.Unicolour.Tests.Utils;

using System.Collections.Generic;
using System.Linq;

public class TestColour
{
    public string? Name { get; init; }
    public string? Hex { get; init; }
    public ColourTriplet? Rgb { get; init; }
    public ColourTriplet? RgbLinear { get; init; }
    public ColourTriplet? Hsl { get; init; }
    public ColourTriplet? Hsb { get; init; }
    public ColourTriplet? Xyz { get; init; }
    public ColourTriplet? Lab { get; init; }
    public ColourTriplet? Lchab { get; init; }
    public ColourTriplet? Luv { get; init; }
    public ColourTriplet? Lchuv { get; init; }
    public Tolerances? Tolerances { get; init; }
    public bool IsRgbConstrained { get; init; } = true;
    public bool IsRgbLinearConstrained { get; init; } = true;

    /*
     * ColorMine and SixLabors behave strangely with HSL
     * from what I can tell, they almost certainly convert HSB -> RGB -> HSL
     * and as a result they have some oddities:
     * 1) they set hue to 0 if there is no HSB saturation (since the intermediate RGB gets rounded to monochrome, the hue gets lost in conversion)
     * 2) SixLabors sets saturation to 0 if RGB chroma (max - min) is < 0.001
     * 3) ColorMine truncates intermediate RGB 255 values during conversion, resulting in full saturation if any RGB 255 value is < 1 (truncated to 0)
     * --- no point in trying to compensate for their lossy conversions, just ignore them in the test
     *
     * ColorMine, SixLabors, and Colourful all behave slightly differently when converting monochrome RGB -> LCH
     * which is understandable; the hue to use when there is no actual hue is somewhat arbitrary
     */
    public bool ExcludeFromHsxTests => ExcludeFromHsxTestReasons.Any();
    public List<string> ExcludeFromHsxTestReasons { get; init; } = new();
    
    /*
     * SixLabors clamps chroma, causing issues converting LUV -> LCH when LUV has extreme U values
     */
    public bool ExcludeFromLchTests => ExcludeFromLchTestReasons.Any();
    public List<string> ExcludeFromLchTestReasons { get; init; } = new();

    public override string ToString() => $"Name:[{Name}] · Hex[{Hex}]";
}

public record Tolerances 
{
    public double Rgb, RgbLinear, Hsb, Hsl, Xyz, Lab, Lchab, Luv, Lchuv;
}
