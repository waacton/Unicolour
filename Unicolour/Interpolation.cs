namespace Wacton.Unicolour;

public static class Interpolation
{
    private static void GuardConfiguration(Unicolour colour1, Unicolour colour2)
    {
        if (colour1.Config != colour2.Config)
        {
            throw new InvalidOperationException("Can only interpolate unicolours with the same configuration reference");
        }
    }
    
    public static Unicolour InterpolateRgb(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);

        var startRgb = startColour.Rgb;
        var endRgb = endColour.Rgb;
        var startAlpha = startColour.Alpha;
        var endAlpha = endColour.Alpha;

        var r = Interpolate(startRgb.R, endRgb.R, distance);
        var g = Interpolate(startRgb.G, endRgb.G, distance);
        var b = Interpolate(startRgb.B, endRgb.B, distance);
        var a = Interpolate(startAlpha.A, endAlpha.A, distance);
        return Unicolour.FromRgb(startColour.Config, r, g, b, a);
    }
    
    public static Unicolour InterpolateHsb(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var startHsb = startColour.Hsb;
        var endHsb = endColour.Hsb;
        var startAlpha = startColour.Alpha;
        var endAlpha = endColour.Alpha;

        var (startHue, endHue) = GetHuePoints((startHsb.HasHue, startHsb.H), (endHsb.HasHue, endHsb.H));
        var h = Interpolate(startHue, endHue, distance);
        var s = Interpolate(startHsb.S, endHsb.S, distance);
        var b = Interpolate(startHsb.B, endHsb.B, distance);
        var a = Interpolate(startAlpha.A, endAlpha.A, distance);
        return Unicolour.FromHsb(startColour.Config, h.Modulo(360), s, b, a);
    }

    public static Unicolour InterpolateHsl(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var startHsl = startColour.Hsl;
        var endHsl = endColour.Hsl;
        var startAlpha = startColour.Alpha;
        var endAlpha = endColour.Alpha;

        var (startHue, endHue) = GetHuePoints((startHsl.HasHue, startHsl.H), (endHsl.HasHue, endHsl.H));
        var h = Interpolate(startHue, endHue, distance);
        var s = Interpolate(startHsl.S, endHsl.S, distance);
        var l = Interpolate(startHsl.L, endHsl.L, distance);
        var a = Interpolate(startAlpha.A, endAlpha.A, distance);
        return Unicolour.FromHsl(startColour.Config, h.Modulo(360), s, l, a);
    }

    private static (double startHue, double endHue) GetHuePoints((bool hasHue, double hueValue) start, (bool hasHue, double hueValue) end)
    {
        // don't use hue if one colour is monochrome (e.g. black n/a° to green 120° should always stay at hue 120°)
        var noHue = !start.hasHue && !end.hasHue;
        var startHue = noHue || start.hasHue ? start.hueValue : end.hueValue;
        var endHue = noHue || end.hasHue ? end.hueValue : start.hueValue;
    
        if (startHue > endHue)
        {
            var endViaRed = endHue + 360;
            var interpolateViaRed = Math.Abs(startHue - endViaRed) < Math.Abs(startHue - endHue);
            return (startHue, interpolateViaRed ? endViaRed : endHue);
        }
    
        if (endHue > startHue)
        {
            var startViaRed = startHue + 360;
            var interpolateViaRed = Math.Abs(endHue - startViaRed) < Math.Abs(endHue - startHue);
            return (interpolateViaRed ? startViaRed : startHue, endHue);
        }
    
        return (startHue, endHue);
    }

    private static double Interpolate(double startValue, double endValue, double distance)
    {
        var difference = endValue - startValue;
        return startValue + (difference * distance);
    }
}