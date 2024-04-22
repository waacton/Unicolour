namespace Wacton.Unicolour.Example.Web;

internal static class Utils
{
    internal static string ToCss(Unicolour unicolour, double alpha)
    {
        if (HasConversionError(unicolour))
        {
            return "transparent";
        }

        var (r, g, b) = unicolour.Rgb.ConstrainedTriplet;
        return $"rgb({r * 100}% {g * 100}% {b * 100}% / {alpha}%)";
    }

    internal static bool HasConversionError(Unicolour unicolour)
    {
        // if the constrained hex has no value, RGB is invalid (likely a NaN during conversion)
        return unicolour.Rgb.Byte255.ConstrainedHex == "-";
    }
}