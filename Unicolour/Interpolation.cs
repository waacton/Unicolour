namespace Wacton.Unicolour;

public static class Interpolation
{
    public static Unicolour InterpolateRgb(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Rgb, start, end, distance);
    public static Unicolour InterpolateHsb(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Hsb, start, end, distance);
    public static Unicolour InterpolateHsl(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Hsl, start, end, distance);
    public static Unicolour InterpolateHwb(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Hwb, start, end, distance);
    public static Unicolour InterpolateXyz(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Xyz, start, end, distance);
    public static Unicolour InterpolateXyy(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Xyy, start, end, distance);
    public static Unicolour InterpolateLab(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Lab, start, end, distance);
    public static Unicolour InterpolateLchab(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Lchab, start, end, distance);
    public static Unicolour InterpolateLuv(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Luv, start, end, distance);
    public static Unicolour InterpolateLchuv(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Lchuv, start, end, distance);
    public static Unicolour InterpolateHsluv(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Hsluv, start, end, distance);
    public static Unicolour InterpolateHpluv(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Hpluv, start, end, distance);
    public static Unicolour InterpolateIctcp(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Ictcp, start, end, distance);
    public static Unicolour InterpolateJzazbz(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Jzazbz, start, end, distance);
    public static Unicolour InterpolateJzczhz(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Jzczhz, start, end, distance);
    public static Unicolour InterpolateOklab(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Oklab, start, end, distance);
    public static Unicolour InterpolateOklch(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Oklch, start, end, distance);
    public static Unicolour InterpolateCam02(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Cam02, start, end, distance);
    public static Unicolour InterpolateCam16(this Unicolour start, Unicolour end, double distance) => Interpolate(ColourSpace.Cam16, start, end, distance);

    internal static Unicolour Interpolate(ColourSpace colourSpace, Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var startRepresentation = startColour.GetRepresentation(colourSpace);
        var endRepresentation = endColour.GetRepresentation(colourSpace);
        
        var (start, end) = GetTriplets(startRepresentation, endRepresentation);
        var triplet = InterpolateTriplet(start, end, distance).WithHueModulo();
        var (first, second, third) = triplet;
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        
        var explicitlyGreyscale = !startRepresentation.IsEffectivelyHued && !endRepresentation.IsEffectivelyHued;
        var colourMode = explicitlyGreyscale ? ColourMode.ExplicitGreyscale : ColourMode.Default(triplet.HueIndex);
        return GetConstructor(colourSpace).Invoke(startColour.Config, colourMode, first, second, third, alpha);
    }

    private static (ColourTriplet start, ColourTriplet end) GetTriplets(ColourRepresentation startRepresentation, ColourRepresentation endRepresentation)
    {
        var startTriplet = startRepresentation.Triplet;
        var endTriplet = endRepresentation.Triplet;
        return startRepresentation.HasHueAxis ? GetTripletsWithHue(startRepresentation, endRepresentation) : (startTriplet, endTriplet);
    }

    private static (ColourTriplet start, ColourTriplet end) GetTripletsWithHue(ColourRepresentation startColourRepresentation, ColourRepresentation endColourRepresentation)
    {
        var startTriplet = startColourRepresentation.Triplet;
        var endTriplet = endColourRepresentation.Triplet;
        
        (ColourTriplet, ColourTriplet) HueResult(double startHue, double endHue) => (
            startTriplet.WithHueOverride(startHue), 
            endTriplet.WithHueOverride(endHue));

        var startHasHue = startColourRepresentation.IsEffectivelyHued;
        var endHasHue = endColourRepresentation.IsEffectivelyHued;
        var ignoreHue = !startHasHue && !endHasHue;
        
        // don't change hue if one colour is greyscale (e.g. black n/a° to green 120° should always stay at hue 120°)
        var startHue = ignoreHue || startHasHue ? startTriplet.HueValue() : endTriplet.HueValue();
        var endHue = ignoreHue || endHasHue ? endTriplet.HueValue() : startTriplet.HueValue();
    
        if (startHue > endHue)
        {
            var endViaZero = endHue + 360;
            var interpolateViaZero = Math.Abs(startHue - endViaZero) < Math.Abs(startHue - endHue);
            return HueResult(startHue, interpolateViaZero ? endViaZero : endHue);
        }
    
        if (endHue > startHue)
        {
            var startViaZero = startHue + 360;
            var interpolateViaZero = Math.Abs(endHue - startViaZero) < Math.Abs(endHue - startHue);
            return HueResult(interpolateViaZero ? startViaZero : startHue, endHue);
        }
    
        return HueResult(startHue, endHue);
    }

    private static ColourTriplet InterpolateTriplet(ColourTriplet start, ColourTriplet end, double distance)
    {
        var first = Interpolate(start.First, end.First, distance);
        var second = Interpolate(start.Second, end.Second, distance);
        var third = Interpolate(start.Third, end.Third, distance);
        return new(first, second, third, start.HueIndex);
    }

    private static double Interpolate(double startValue, double endValue, double distance)
    {
        var difference = endValue - startValue;
        return startValue + (difference * distance);
    }
    
    private static void GuardConfiguration(Unicolour unicolour1, Unicolour unicolour2)
    {
        if (unicolour1.Config != unicolour2.Config)
        {
            throw new InvalidOperationException("Can only interpolate unicolours with the same configuration reference");
        }
    }

    private delegate Unicolour UnicolourConstructor(Configuration config, ColourMode colourMode, double first, double second, double third, double alpha = 1.0);
    private static UnicolourConstructor GetConstructor(ColourSpace colourSpace)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => Unicolour.FromRgb,
            ColourSpace.Hsb => Unicolour.FromHsb,
            ColourSpace.Hsl => Unicolour.FromHsl,
            ColourSpace.Hwb => Unicolour.FromHwb,
            ColourSpace.Xyz => Unicolour.FromXyz,
            ColourSpace.Xyy => Unicolour.FromXyy,
            ColourSpace.Lab => Unicolour.FromLab,
            ColourSpace.Lchab => Unicolour.FromLchab,
            ColourSpace.Luv => Unicolour.FromLuv,
            ColourSpace.Lchuv => Unicolour.FromLchuv,
            ColourSpace.Hsluv => Unicolour.FromHsluv,
            ColourSpace.Hpluv => Unicolour.FromHpluv,
            ColourSpace.Ictcp => Unicolour.FromIctcp,
            ColourSpace.Jzazbz => Unicolour.FromJzazbz,
            ColourSpace.Jzczhz => Unicolour.FromJzczhz,
            ColourSpace.Oklab => Unicolour.FromOklab,
            ColourSpace.Oklch => Unicolour.FromOklch,
            ColourSpace.Cam02 => Unicolour.FromCam02,
            ColourSpace.Cam16 => Unicolour.FromCam16,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
}