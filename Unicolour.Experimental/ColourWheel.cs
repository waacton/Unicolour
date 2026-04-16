namespace Wacton.Unicolour.Experimental;

public abstract class ColourWheel
{
    public abstract Unicolour Pure(double hue);
    public abstract Unicolour Tint(double hue, double weight);
    public abstract Unicolour Shade(double hue, double weight);
    public abstract Unicolour Tone(double hue, double weight);

    public static ColourWheel From(Pigment red, Pigment yellow, Pigment blue, Pigment white, Pigment black)
    {
        return new PigmentColourWheel(red, yellow, blue, white, black);
    }
    
    public static ColourWheel From(ColourSpace colourSpace, Unicolour reference)
    {
        return new HueColourWheel(colourSpace, reference);
    }

    public Unicolour[] Harmony(double hue, Harmony harmony)
    {
        return harmony switch
        {
            Experimental.Harmony.Monochromatic => [Shade(hue, 1), Shade(hue, 0.5), Pure(hue), Tint(hue, 0.5), Tint(hue, 1)],
            Experimental.Harmony.MonochromaticTint => [Tint(hue, 0), Tint(hue, 0.25), Tint(hue, 0.5), Tint(hue, 0.75), Tint(hue, 1)],
            Experimental.Harmony.MonochromaticShade => [Shade(hue, 0), Shade(hue, 0.25), Shade(hue, 0.5), Shade(hue, 0.75), Shade(hue, 1)],
            Experimental.Harmony.MonochromaticTone => [Tone(hue, 0), Tone(hue, 0.25), Tone(hue, 0.5), Tone(hue, 0.75), Tone(hue, 1)],
            Experimental.Harmony.Analogous => [Pure(hue - 30), Pure(hue), Pure(hue + 30)],
            Experimental.Harmony.Complementary => [Pure(hue), Pure(hue + 180)],
            Experimental.Harmony.SplitComplementary => [Pure(hue), Pure(hue + 150), Pure(hue - 150)],
            Experimental.Harmony.Triadic => [Pure(hue), Pure(hue + 120), Pure(hue - 120)],
            Experimental.Harmony.TetradicRectangle => [Pure(hue), Pure(hue + 60), Pure(hue + 180), Pure(hue + 240)],
            Experimental.Harmony.TetradicSquare => [Pure(hue), Pure(hue + 90), Pure(hue + 180), Pure(hue + 270)],
            _ => throw new ArgumentOutOfRangeException(nameof(harmony), harmony, null)
        };
    }
}