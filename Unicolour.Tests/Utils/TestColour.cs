using System.Linq;

namespace Wacton.Unicolour.Tests.Utils;

public class TestColour
{
    public string? Name { get; init; }
    public string? Hex { get; init; }
    public ColourTriplet? Rgb { get; init; }
    public ColourTriplet? Hsl { get; init; }
    public ColourTriplet? Hsb { get; init; }
    public ColourTriplet? Xyz { get; init; }
    public ColourTriplet? Luv { get; init; }
    public ColourTriplet? Lchuv { get; init; }
    public ColourTriplet? Hsluv { get; init; }
    public ColourTriplet? Hpluv { get; init; }

    public override string ToString() => string.Join(" · ", new[] { Hex, Name }.Where(x => !string.IsNullOrEmpty(x)));
}