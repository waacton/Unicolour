namespace Wacton.Unicolour.Example.Web;

internal static class Utils
{
    internal static string ToCss(Unicolour colour, double alpha)
    {
        if (HasConversionError(colour))
        {
            return "transparent";
        }

        var (r, g, b) = colour.Rgb.ConstrainedTriplet;
        return $"rgb({r * 100}% {g * 100}% {b * 100}% / {alpha}%)";
    }

    internal static bool HasConversionError(Unicolour colour)
    {
        // if the constrained hex has no value, RGB is invalid (likely a NaN during conversion)
        return colour.Rgb.Byte255.ConstrainedHex == "-";
    }
}