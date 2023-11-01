namespace Wacton.Unicolour;

internal static class Interpolation
{
    internal static Unicolour Mix(ColourSpace colourSpace, Unicolour startColour, Unicolour endColour, double distance)
    {
        GuardConfiguration(startColour, endColour);
        
        var startRepresentation = startColour.GetRepresentation(colourSpace);
        var endRepresentation = endColour.GetRepresentation(colourSpace);
        
        var (start, end) = GetTriplets(startRepresentation, endRepresentation);
        var triplet = InterpolateTriplet(start, end, distance).WithHueModulo();
        var (first, second, third) = triplet;
        var alpha = Interpolate(startColour.Alpha.A, endColour.Alpha.A, distance);
        var heritage = ColourHeritage.From(startRepresentation, endRepresentation);
        
        return GetConstructor(colourSpace).Invoke(startColour.Config, heritage, first, second, third, alpha);
    }

    private static (ColourTriplet start, ColourTriplet end) GetTriplets(ColourRepresentation startRepresentation, ColourRepresentation endRepresentation)
    {
        var startTriplet = startRepresentation.Triplet;
        var endTriplet = endRepresentation.Triplet;
        return startRepresentation.HasHueAxis ? GetTripletsWithHue(startRepresentation, endRepresentation) : (startTriplet, endTriplet);
    }

    private static (ColourTriplet start, ColourTriplet end) GetTripletsWithHue(ColourRepresentation startRepresentation, ColourRepresentation endRepresentation)
    {
        var startTriplet = startRepresentation.Triplet;
        var endTriplet = endRepresentation.Triplet;
        
        (ColourTriplet, ColourTriplet) HueResult(double startHue, double endHue) => (
            startTriplet.WithHueOverride(startHue), 
            endTriplet.WithHueOverride(endHue));

        var startHasHue = startRepresentation.UseAsHued;
        var endHasHue = endRepresentation.UseAsHued;
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

    internal static double Interpolate(double startValue, double endValue, double distance)
    {
        var difference = endValue - startValue;
        return startValue + (difference * distance);
    }
    
    private static void GuardConfiguration(Unicolour unicolour1, Unicolour unicolour2)
    {
        if (unicolour1.Config != unicolour2.Config)
        {
            throw new InvalidOperationException("Can only mix unicolours with the same configuration reference");
        }
    }

    private delegate Unicolour UnicolourConstructor(Configuration config, ColourHeritage heritage, double first, double second, double third, double alpha = 1.0);
    private static UnicolourConstructor GetConstructor(ColourSpace colourSpace)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => Unicolour.FromRgb,
            ColourSpace.RgbLinear => Unicolour.FromRgbLinear,
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
            ColourSpace.Hct => Unicolour.FromHct,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
}