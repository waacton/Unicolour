namespace Wacton.Unicolour;

internal static class Interpolation
{
    internal static Unicolour Mix(Unicolour startColour, Unicolour endColour, ColourSpace colourSpace, double distance, HueSpan hueSpan, bool premultiplyAlpha)
    {
        var (start, end, config) = AdjustConfig(startColour, endColour);
        
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

        var (startTriplet, endTriplet) = GetTripletsToInterpolate(
            (startRepresentation, startAlpha), 
            (endRepresentation, endAlpha),
            hueSpan, premultiplyAlpha, mapToDegree);
        
        var triplet = InterpolateTriplet(startTriplet, endTriplet, distance).WithHueModulo().WithDegreeMap(mapFromDegree);
        var alpha = Linear(startColour.Alpha.ConstrainedA, endColour.Alpha.ConstrainedA, distance);
        
        if (premultiplyAlpha)
        {
            triplet = triplet.WithUnpremultipliedAlpha(alpha);
        }
        
        var heritage = ColourHeritage.From(startRepresentation, endRepresentation);
        var (first, second, third) = triplet;
        
        return new Unicolour(config, heritage, colourSpace, first, second, third, alpha);
    }
    
    internal static IEnumerable<Unicolour> Palette(Unicolour startColour, Unicolour endColour, ColourSpace colourSpace, int count, HueSpan hueSpan, bool premultiplyAlpha)
    {
        count = Math.Max(count, 0);
        var (start, end, _) = AdjustConfig(startColour, endColour); // saves doing this N times via Mix() if configs are different
        
        var palette = new List<Unicolour>();
        for (var i = 0; i < count; i++)
        {
            var distance = count == 1 ? 0.5 : i / (double)(count - 1);
            palette.Add(Mix(start, end, colourSpace, distance, hueSpan, premultiplyAlpha));
        }

        return palette;
    }
    
    private static (Unicolour start, Unicolour end, Configuration config) AdjustConfig(Unicolour start, Unicolour end)
    {
        var config = start.Configuration;
        return end.Configuration == config 
            ? (start, end, config) 
            : (start, end.ConvertToConfiguration(config), config);
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
        
        (startHue, endHue) = Hue.Unwrap(startHue, endHue, hueSpan);
        var adjustedStartHue = startTriplet.WithHueOverride(startHue);
        var adjustedEndHue = endTriplet.WithHueOverride(endHue);
        return (adjustedStartHue, adjustedEndHue);
    }
    
    private static ColourTriplet InterpolateTriplet(ColourTriplet start, ColourTriplet end, double distance)
    {
        var first = Linear(start.First, end.First, distance);
        var second = Linear(start.Second, end.Second, distance);
        var third = Linear(start.Third, end.Third, distance);
        return new(first, second, third, start.HueIndex);
    }

    internal static double Linear(double startValue, double endValue, double distance)
    {
        var difference = endValue - startValue;
        return startValue + difference * distance;
    }
}