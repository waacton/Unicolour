namespace Wacton.Unicolour;

public static class Interpolation
{
    public static Unicolour InterpolateRgb(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var (r, g, b) = InterpolateTuple(startColour.Rgb.Tuple, endColour.Rgb.Tuple, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromRgb(startColour.Config, r, g, b, alpha);
    }

    public static Unicolour InterpolateHsb(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        var startHsb = startColour.Hsb;
        var endHsb = endColour.Hsb;
        
        var (start, end) = GetHueBasedTuples((startHsb.HasHue, startHsb.Tuple), (endHsb.HasHue, endHsb.Tuple));
        var (h,s, b) = InterpolateTuple(start, end, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromHsb(startColour.Config, h.Modulo(360), s, b, alpha);
    }

    public static Unicolour InterpolateHsl(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        var startHsl = startColour.Hsl;
        var endHsl = endColour.Hsl;

        var (start, end) = GetHueBasedTuples((startHsl.HasHue, startHsl.Tuple), (endHsl.HasHue, endHsl.Tuple));
        var (h,s, l) = InterpolateTuple(start, end, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromHsl(startColour.Config, h.Modulo(360), s, l, alpha);
    }
    
    public static Unicolour InterpolateXyz(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var (x, y, z) = InterpolateTuple(startColour.Xyz.Tuple, endColour.Xyz.Tuple, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromXyz(startColour.Config, x, y, z, alpha);
    }
    
    public static Unicolour InterpolateLab(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var (l, a, b) = InterpolateTuple(startColour.Lab.Tuple, endColour.Lab.Tuple, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromLab(startColour.Config, l, a, b, alpha);
    }

    private static (ColourTuple startHue, ColourTuple endHue) GetHueBasedTuples((bool hasHue, ColourTuple tuple) start, (bool hasHue, ColourTuple tuple) end)
    {
        double Hue(ColourTuple tuple) => tuple.First;

        (ColourTuple, ColourTuple) Result(double startHue, double endHue) => (
            new(startHue, start.tuple.Second, start.tuple.Third), 
            new(endHue, end.tuple.Second, end.tuple.Third));
        
        // don't use hue if one colour is monochrome (e.g. black n/a° to green 120° should always stay at hue 120°)
        var noHue = !start.hasHue && !end.hasHue;
        var startHue = noHue || start.hasHue ? Hue(start.tuple) : Hue(end.tuple);
        var endHue = noHue || end.hasHue ? Hue(end.tuple) : Hue(start.tuple);
    
        if (startHue > endHue)
        {
            var endViaRed = endHue + 360;
            var interpolateViaRed = Math.Abs(startHue - endViaRed) < Math.Abs(startHue - endHue);
            return Result(startHue, interpolateViaRed ? endViaRed : endHue);
        }
    
        if (endHue > startHue)
        {
            var startViaRed = startHue + 360;
            var interpolateViaRed = Math.Abs(endHue - startViaRed) < Math.Abs(endHue - startHue);
            return Result(interpolateViaRed ? startViaRed : startHue, endHue);
        }
    
        return Result(startHue, endHue);
    }

    private static ColourTuple InterpolateTuple(ColourTuple start, ColourTuple end, double distance)
    {
        var first = Interpolate(start.First, end.First, distance);
        var second = Interpolate(start.Second, end.Second, distance);
        var third = Interpolate(start.Third, end.Third, distance);
        return new(first, second, third);
    }

    private static double Interpolate(double startValue, double endValue, double distance)
    {
        var difference = endValue - startValue;
        return startValue + (difference * distance);
    }
    
    private static void GuardConfiguration(Unicolour colour1, Unicolour colour2)
    {
        if (colour1.Config != colour2.Config)
        {
            throw new InvalidOperationException("Can only interpolate unicolours with the same configuration reference");
        }
    }
}