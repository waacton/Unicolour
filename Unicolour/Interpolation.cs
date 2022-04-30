namespace Wacton.Unicolour;

public static class Interpolation
{
    public static Unicolour InterpolateRgb(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var (r, g, b) = InterpolateTriplet(startColour.Rgb.Triplet, endColour.Rgb.Triplet, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromRgb(startColour.Config, r, g, b, alpha);
    }

    public static Unicolour InterpolateHsb(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        var startHsb = startColour.Hsb;
        var endHsb = endColour.Hsb;
        
        var (start, end) = GetHueBasedTriplets((startHsb.HasHue, startHsb.Triplet), (endHsb.HasHue, endHsb.Triplet), 0);
        var (h,s, b) = InterpolateTriplet(start, end, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromHsb(startColour.Config, h.Modulo(360), s, b, alpha);
    }

    public static Unicolour InterpolateHsl(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        var startHsl = startColour.Hsl;
        var endHsl = endColour.Hsl;

        var (start, end) = GetHueBasedTriplets((startHsl.HasHue, startHsl.Triplet), (endHsl.HasHue, endHsl.Triplet), 0);
        var (h,s, l) = InterpolateTriplet(start, end, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromHsl(startColour.Config, h.Modulo(360), s, l, alpha);
    }

    public static Unicolour InterpolateXyz(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var (x, y, z) = InterpolateTriplet(startColour.Xyz.Triplet, endColour.Xyz.Triplet, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromXyz(startColour.Config, x, y, z, alpha);
    }
    
    public static Unicolour InterpolateLab(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var (l, a, b) = InterpolateTriplet(startColour.Lab.Triplet, endColour.Lab.Triplet, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromLab(startColour.Config, l, a, b, alpha);
    }
    
    public static Unicolour InterpolateLchab(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        var startLchab = startColour.Lchab;
        var endLchab = endColour.Lchab;
        
        var (start, end) = GetHueBasedTriplets((startLchab.HasHue, startLchab.Triplet), (endLchab.HasHue, endLchab.Triplet), 2);
        var (l,c, h) = InterpolateTriplet(start, end, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromLchab(startColour.Config, l, c, h.Modulo(360), alpha);
    }
    
    public static Unicolour InterpolateLuv(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var (l, u, v) = InterpolateTriplet(startColour.Luv.Triplet, endColour.Luv.Triplet, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromLuv(startColour.Config, l, u, v, alpha);
    }
    
    public static Unicolour InterpolateLchuv(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        var startLchuv = startColour.Lchuv;
        var endLchuv = endColour.Lchuv;
        
        var (start, end) = GetHueBasedTriplets((startLchuv.HasHue, startLchuv.Triplet), (endLchuv.HasHue, endLchuv.Triplet), 2);
        var (l,c, h) = InterpolateTriplet(start, end, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromLchuv(startColour.Config, l, c, h.Modulo(360), alpha);
    }
    
    public static Unicolour InterpolateOklab(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var (l, a, b) = InterpolateTriplet(startColour.Oklab.Triplet, endColour.Oklab.Triplet, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromOklab(startColour.Config, l, a, b, alpha);
    }
    
    public static Unicolour InterpolateOklch(this Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        var startOklch = startColour.Oklch;
        var endOklch = endColour.Oklch;
        
        var (start, end) = GetHueBasedTriplets((startOklch.HasHue, startOklch.Triplet), (endOklch.HasHue, endOklch.Triplet), 2);
        var (l,c, h) = InterpolateTriplet(start, end, distance);
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        return Unicolour.FromOklch(startColour.Config, l, c, h.Modulo(360), alpha);
    }
    
    private static (ColourTriplet startHue, ColourTriplet endHue) GetHueBasedTriplets(
        (bool hasHue, ColourTriplet triplet) start, 
        (bool hasHue, ColourTriplet triplet) end,
        int hueIndex)
    {
        double Hue(ColourTriplet triplet)
        {
            return hueIndex switch
            {
                0 => triplet.First,
                2 => triplet.Third,
                _ => throw new ArgumentException()
            };
        }

        ColourTriplet TripletWithHue(ColourTriplet triplet, double hue)
        {
            var (first, second, third) = triplet;
            return hueIndex switch
            {
                0 => new(hue, second, third),
                2 => new(first, second, hue),
                _ => throw new ArgumentException()
            };
        }
        
        (ColourTriplet, ColourTriplet) Result(double startHue, double endHue) => (
            TripletWithHue(start.triplet, startHue), 
            TripletWithHue(end.triplet, endHue));

        // don't use hue if one colour is monochrome (e.g. black n/a° to green 120° should always stay at hue 120°)
        var noHue = !start.hasHue && !end.hasHue;
        var startHue = noHue || start.hasHue ? Hue(start.triplet) : Hue(end.triplet);
        var endHue = noHue || end.hasHue ? Hue(end.triplet) : Hue(start.triplet);
    
        if (startHue > endHue)
        {
            var endViaZero = endHue + 360;
            var interpolateViaZero = Math.Abs(startHue - endViaZero) < Math.Abs(startHue - endHue);
            return Result(startHue, interpolateViaZero ? endViaZero : endHue);
        }
    
        if (endHue > startHue)
        {
            var startViaZero = startHue + 360;
            var interpolateViaZero = Math.Abs(endHue - startViaZero) < Math.Abs(endHue - startHue);
            return Result(interpolateViaZero ? startViaZero : startHue, endHue);
        }
    
        return Result(startHue, endHue);
    }

    private static ColourTriplet InterpolateTriplet(ColourTriplet start, ColourTriplet end, double distance)
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