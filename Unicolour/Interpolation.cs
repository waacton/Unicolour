namespace Wacton.Unicolour;

internal static class Interpolation
{
    internal static Unicolour Mix(Unicolour startColour, Unicolour endColour, ColourSpace colourSpace, double distance, HueSpan hueSpan, bool premultiplyAlpha)
    {
        (Unicolour start, Unicolour end, Configuration config) = AdjustConfiguration(startColour, endColour);
        
        var startRepresentation = start.GetRepresentation(colourSpace);
        var startAlpha = start.Alpha;
        var endRepresentation = end.GetRepresentation(colourSpace);
        var endAlpha = end.Alpha;

        Func<double, double> mapToDegree = colourSpace == ColourSpace.Wxy
            ? value => Wxy.WavelengthToDegree(value, config.Xyz)
            : value => value;
        
        Func<double, double> mapFromDegree = colourSpace == ColourSpace.Wxy
            ? value => Wxy.DegreeToWavelength(value, config.Xyz)
            : value => value;


        (ColourTriplet startTriplet, ColourTriplet endTriplet) = GetTripletsToInterpolate(
            (startRepresentation, startAlpha), 
            (endRepresentation, endAlpha),
            hueSpan, premultiplyAlpha, mapToDegree);
        
        var triplet = InterpolateTriplet(startTriplet, endTriplet, distance).WithHueModulo().WithDegreeMap(mapFromDegree);
        var alpha = Interpolate(startColour.Alpha.ConstrainedA, endColour.Alpha.ConstrainedA, distance);
        
        if (premultiplyAlpha)
        {
            triplet = triplet.WithUnpremultipliedAlpha(alpha);
        }
        
        var heritage = ColourHeritage.From(startRepresentation, endRepresentation);
        var (first, second, third) = triplet;
        
        return new Unicolour(config, heritage, colourSpace, first, second, third, alpha);
    }
    
    private static (Unicolour start, Unicolour end, Configuration config) AdjustConfiguration(Unicolour start, Unicolour end)
    {
        var config = start.Config;
        return end.Config == config ? (start, end, config) : (start, end.ConvertToConfiguration(config), config);
    }
    
    private static (ColourTriplet start, ColourTriplet end) GetTripletsToInterpolate(
        (ColourRepresentation representation, Alpha alpha) start, 
        (ColourRepresentation representation, Alpha alpha) end,
        HueSpan hueSpan, bool premultiplyAlpha, Func<double, double> mapToDegree)
    {
        ColourTriplet startTriplet;
        ColourTriplet endTriplet;
        
        // these can't give different answers since they use the same colour space
        // (except by reflection, in which case an error would be thrown when later trying to read the hue component)
        var hasHueComponent = start.representation.HasHueComponent || end.representation.HasHueComponent;
        if (hasHueComponent)
        {
            (startTriplet, endTriplet) = GetTripletsWithHue(start.representation, end.representation, hueSpan, mapToDegree);
        }
        else
        {
            startTriplet = start.representation.Triplet;
            endTriplet = end.representation.Triplet;
        }
        
        if (premultiplyAlpha)
        {
            startTriplet = startTriplet.WithPremultipliedAlpha(start.alpha.ConstrainedA);
            endTriplet = endTriplet.WithPremultipliedAlpha(end.alpha.ConstrainedA);
        }

        return (startTriplet, endTriplet);
    }
    
    private static (ColourTriplet start, ColourTriplet end) GetTripletsWithHue(
        ColourRepresentation startRepresentation, ColourRepresentation endRepresentation, 
        HueSpan hueSpan, Func<double, double> mapToDegree)
    {
        var startTriplet = startRepresentation.Triplet.WithDegreeMap(mapToDegree).WithHueModulo(allow360: true);
        var endTriplet = endRepresentation.Triplet.WithDegreeMap(mapToDegree).WithHueModulo(allow360: true);

        var startHasHue = startRepresentation.UseAsHued;
        var endHasHue = endRepresentation.UseAsHued;
        var ignoreHue = !startHasHue && !endHasHue;
        
        // don't change hue if one colour is greyscale (e.g. black n/a° to green 120° should always stay at hue 120°)
        var startHue = ignoreHue || startHasHue ? startTriplet.HueValue() : endTriplet.HueValue();
        var endHue = ignoreHue || endHasHue ? endTriplet.HueValue() : startTriplet.HueValue();
        
        (startHue, endHue) = AdjustHues(startHue, endHue, hueSpan);
        var adjustedStartHue = startTriplet.WithHueOverride(startHue);
        var adjustedEndHue = endTriplet.WithHueOverride(endHue);
        return (adjustedStartHue, adjustedEndHue);
    }

    private static (double start, double end) AdjustHues(double start, double end, HueSpan hueSpan)
    {
        return hueSpan switch
        {
            HueSpan.Shorter => (end - start) switch
            {
                > 180 => (start + 360, end),
                < -180 => (start, end + 360),
                _ => (start, end)
            },
            HueSpan.Longer => (end - start) switch
            {
                > 0 and < 180 => (start + 360, end),
                > -180 and <= 0 => (start, end + 360),
                _ => (start, end)
            },
            HueSpan.Increasing => (start, end < start ? end + 360 : end),
            HueSpan.Decreasing => (start < end ? start + 360 : start, end),
            _ => throw new ArgumentOutOfRangeException(nameof(hueSpan), hueSpan, null)
        };
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
        return startValue + difference * distance;
    }
}

public enum HueSpan
{
    Shorter,
    Longer,
    Increasing,
    Decreasing
}