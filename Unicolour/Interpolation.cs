namespace Wacton.Unicolour;

internal static class Interpolation
{
    // TODO: include alpha as optional
    public static Unicolour InterpolateHsb(this Unicolour startColour, Unicolour endColour, double distance)
    {
        var startHsb = startColour.Hsb;
        var endHsb = endColour.Hsb;

        // don't use hue if one colour is monochrome (e.g. black n/a° to green 120° should always stay at hue 120°)
        var noHue = !startHsb.HasHue && !endHsb.HasHue;
        var startHue = noHue || startHsb.HasHue ? startHsb.H : endHsb.H;
        var endHue = noHue || endHsb.HasHue ? endHsb.H : startHsb.H;

        var forwardStart = startHue;
        var forwardEnd = endHue;
        var backwardStart = Math.Min(startHue, endHue) + 360;
        var backwardEnd = Math.Max(startHue, endHue);
        
        var interpolateForward = Math.Abs(forwardStart - forwardEnd) <= Math.Abs(backwardStart - backwardEnd);
        startHue = interpolateForward ? forwardStart : backwardStart;
        endHue = interpolateForward ? forwardEnd : backwardEnd;

        var h = Interpolate(startHue, endHue, distance);
        var s = Interpolate(startHsb.S, endHsb.S, distance);
        var b = Interpolate(startHsb.B, endHsb.B, distance);
        return Unicolour.FromHsb(h.Modulo(360), s, b);
    }
    
    // TODO: include alpha as optional
    public static Unicolour InterpolateRgb(this Unicolour startColour, Unicolour endColour, double distance)
    {
        var startRgb = startColour.Rgb;
        var endRgb = endColour.Rgb;

        var r = Interpolate(startRgb.R, endRgb.R, distance);
        var g = Interpolate(startRgb.G, endRgb.G, distance);
        var b = Interpolate(startRgb.B, endRgb.B, distance);
        return Unicolour.FromRgb(r, g, b);
    }
    
    private static double Interpolate(double startValue, double endValue, double distance)
    {
        var difference = endValue - startValue;
        return startValue + (difference * distance);
    }
}