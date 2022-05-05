namespace Wacton.Unicolour;

internal static class Measurement
{
    public static string GetHex(this Unicolour colour)
    {
        var (r255, g255, b255) = colour.Rgb.ConstrainedTriplet255;
        return $"#{(int)r255:X2}{(int)g255:X2}{(int)b255:X2}";
    }
    
    public static bool CanBeDisplayed(this Unicolour colour)
    {
        var (r, g, b) = colour.Rgb.TripletLinear;
        return r is >= 0 and <= 1.0 && g is >= 0 and <= 1.0 && b is >= 0 and <= 1.0;
    }
    
    // https://www.w3.org/TR/WCAG21/#dfn-relative-luminance
    public static double RelativeLuminance(this Unicolour colour)
    {
        var (r, g, b) = colour.Rgb.ConstrainedTripletLinear;
        return 0.2126 * r + 0.7152 * g + 0.0722 * b;
    }
}