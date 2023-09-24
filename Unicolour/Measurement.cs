namespace Wacton.Unicolour;

internal static class Measurement
{
    internal static string GetHex(this Unicolour colour)
    {
        if (!colour.IsDisplayable) return "-";
        var byte255 = colour.Rgb.Byte255;
        if (byte255.UseAsNaN) return "-";
        var (r255, g255, b255) = byte255.ConstrainedTriplet;
        return $"#{(int)r255:X2}{(int)g255:X2}{(int)b255:X2}";
    }
    
    internal static bool GetIsDisplayable(this Unicolour colour)
    {
        var rgbLinear = colour.Rgb.Linear;
        if (rgbLinear.UseAsNaN) return false;
        var (r, g, b) = colour.Rgb.Linear.Triplet;
        return r is >= 0 and <= 1.0 && g is >= 0 and <= 1.0 && b is >= 0 and <= 1.0;
    }
    
    // https://www.w3.org/TR/WCAG21/#dfn-relative-luminance
    internal static double GetRelativeLuminance(this Unicolour colour)
    {
        var rgbLinear = colour.Rgb.Linear;
        if (rgbLinear.UseAsNaN) return double.NaN;
        var (r, g, b) = rgbLinear.ConstrainedTriplet;
        return 0.2126 * r + 0.7152 * g + 0.0722 * b;
    }
    
    internal static IEnumerable<ColourDescription> GetDescriptions(this Unicolour colour)
    {
        return ColourDescription.Get(colour.Hsl);
    }

    internal static Temperature GetTemperature(this Unicolour colour)
    {
        return Temperature.Get(colour);
    }
}