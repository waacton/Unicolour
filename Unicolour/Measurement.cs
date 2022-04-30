namespace Wacton.Unicolour;

internal static class Measurement
{
    // https://www.w3.org/TR/WCAG21/#dfn-relative-luminance
    public static double RelativeLuminance(this Unicolour colour)
    {
        var (r, g, b) = colour.Rgb.TripletLinear;
        return 0.2126 * r + 0.7152 * g + 0.0722 * b;
    }
    
    // TODO: excitation purity? https://en.wikipedia.org/wiki/Colorfulness#Excitation_purity
}