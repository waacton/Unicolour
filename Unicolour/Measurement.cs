namespace Wacton.Unicolour;

internal static class Measurement
{
    internal static string Hex(this Unicolour colour)
    {
        var byte255 = colour.Rgb.Byte255;
        if (byte255.IsEffectivelyNaN) return "-";
        var (r255, g255, b255) = byte255.ConstrainedTriplet;
        return $"#{(int)r255:X2}{(int)g255:X2}{(int)b255:X2}";
    }
    
    internal static bool IsDisplayable(this Unicolour colour)
    {
        var rgbLinear = colour.Rgb.Linear;
        if (rgbLinear.IsEffectivelyNaN) return false;
        var (r, g, b) = colour.Rgb.Linear.Triplet;
        return r is >= 0 and <= 1.0 && g is >= 0 and <= 1.0 && b is >= 0 and <= 1.0;
    }
    
    // https://www.w3.org/TR/WCAG21/#dfn-relative-luminance
    internal static double RelativeLuminance(this Unicolour colour)
    {
        var rgbLinear = colour.Rgb.Linear;
        if (rgbLinear.IsEffectivelyNaN) return double.NaN;
        var (r, g, b) = rgbLinear.ConstrainedTriplet;
        return 0.2126 * r + 0.7152 * g + 0.0722 * b;
    }
    
    internal static IEnumerable<ColourDescription> Description(this Unicolour colour)
    {
        var hsl = colour.Hsl;
        if (hsl.IsEffectivelyNaN) return new List<ColourDescription> { ColourDescription.NotApplicable };

        var (h, s, l) = hsl.ConstrainedTriplet;
        switch (l)
        {
            case <= 0: return new List<ColourDescription> { ColourDescription.Black };
            case >= 1: return new List<ColourDescription> { ColourDescription.White };
        }

        var lightness = l switch
        {
            < 0.20 => ColourDescription.Shadow,
            < 0.40 => ColourDescription.Dark,
            < 0.60 => ColourDescription.Pure,
            < 0.80 => ColourDescription.Light,
            _ => ColourDescription.Pale
        };

        if (hsl.IsEffectivelyGreyscale) return new List<ColourDescription> { lightness, ColourDescription.Grey };

        var strength = s switch
        {
            < 0.20 => ColourDescription.Faint,
            < 0.40 => ColourDescription.Weak,
            < 0.60 => ColourDescription.Mild,
            < 0.80 => ColourDescription.Strong,
            _ => ColourDescription.Vibrant
        };
        
        var hue = h switch
        {
            < 15 => ColourDescription.Red,
            < 45 => ColourDescription.Orange,
            < 75 => ColourDescription.Yellow,
            < 105 => ColourDescription.Chartreuse,
            < 135 => ColourDescription.Green,
            < 165 => ColourDescription.Mint,
            < 195 => ColourDescription.Cyan,
            < 225 => ColourDescription.Azure,
            < 255 => ColourDescription.Blue,
            < 285 => ColourDescription.Violet,
            < 315 => ColourDescription.Magenta,
            < 345 => ColourDescription.Rose,
            _ => ColourDescription.Red
        };

        return new List<ColourDescription> { lightness, strength, hue };
    }
}