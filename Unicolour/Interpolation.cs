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

        Func<double, double> mapToHue = colourSpace == ColourSpace.Wxy
            ? value => Hue.FromWavelength(value, config.Xyz)
            : value => value;
        
        Func<double, double> mapFromHue = colourSpace == ColourSpace.Wxy
            ? value => Hue.ToWavelength(value, config.Xyz)
            : value => value;

        var (startTriplet, endTriplet) = GetTripletsToInterpolate(
            (startRepresentation, startAlpha), 
            (endRepresentation, endAlpha),
            hueSpan, premultiplyAlpha, mapToHue);
        
        var triplet = InterpolateTriplet(startTriplet, endTriplet, distance).WithHueModulo().WithHueMap(mapFromHue);
        var alpha = Linear(startColour.Alpha.Clipped.A, endColour.Alpha.Clipped.A, distance);
        
        if (premultiplyAlpha)
        {
            triplet = triplet.WithUnpremultipliedAlpha(alpha);
        }
        
        var limitation = InterpolateLimitation(startRepresentation, endRepresentation);
        var (first, second, third) = triplet;
        return new Unicolour(config, colourSpace, first, second, third, alpha, limitation);
    }
    
    internal static IEnumerable<Unicolour> Palette(Unicolour startColour, Unicolour endColour, ColourSpace colourSpace, int count, HueSpan hueSpan, bool premultiplyAlpha)
    {
        count = Math.Max(count, 0);
        var (start, end, _) = AdjustConfig(startColour, endColour); // saves doing this N times via Mix() if configs are different
        
        List<Unicolour> palette = [];
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
        HueSpan hueSpan, bool premultiplyAlpha, Func<double, double> mapToHue)
    {
        ColourTriplet startTriplet;
        ColourTriplet endTriplet;
        
        // these can't give different answers since they use the same colour space
        // (except by reflection, in which case an error would be thrown when later trying to read the hue component)
        var hasHueComponent = start.representation.HasHueComponent || end.representation.HasHueComponent;
        if (hasHueComponent)
        {
            (startTriplet, endTriplet) = GetTripletsWithHue(start.representation, end.representation, hueSpan, mapToHue);
        }
        else
        {
            startTriplet = start.representation.Triplet;
            endTriplet = end.representation.Triplet;
        }
        
        if (premultiplyAlpha)
        {
            startTriplet = startTriplet.WithPremultipliedAlpha(start.alpha.Clipped.A);
            endTriplet = endTriplet.WithPremultipliedAlpha(end.alpha.Clipped.A);
        }

        return (startTriplet, endTriplet);
    }
    
    private static (ColourTriplet start, ColourTriplet end) GetTripletsWithHue(
        ColourRepresentation startRepresentation, ColourRepresentation endRepresentation, 
        HueSpan hueSpan, Func<double, double> mapToHue)
    {
        var startTriplet = startRepresentation.Triplet.WithHueMap(mapToHue).WithHueModulo(allow360: true);
        var endTriplet = endRepresentation.Triplet.WithHueMap(mapToHue).WithHueModulo(allow360: true);

        double startHue, endHue;
        if (startRepresentation.IsAchromatic ^ endRepresentation.IsAchromatic)
        {
            // if only one colour is achromatic, use only the hue of the chromatic colour
            // e.g. black n/a° to green 120° should always stay at hue 120°
            startHue = startRepresentation.IsAchromatic ? endTriplet.HueValue() : startTriplet.HueValue();
            endHue = endRepresentation.IsAchromatic ? startTriplet.HueValue() : endTriplet.HueValue();
        }
        else
        {
            // if both colours are chromatic, interpolate between hues as normal
            // if both colours are achromatic, there is no 'correct' hue, so interpolate between the powerless hue values
            startHue = startTriplet.HueValue();
            endHue = endTriplet.HueValue();
        }
        
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

    private static Limitation InterpolateLimitation(ColourRepresentation colour1, ColourRepresentation colour2)
    {
        var first = colour1.Limitation;
        var second = colour2.Limitation;

        var eitherNaN = first == Limitation.NaN || second == Limitation.NaN;
        if (eitherNaN) return Limitation.NaN;
        
        var bothAchromatic = first == Limitation.Achromatic && second == Limitation.Achromatic;
        if (bothAchromatic) return Limitation.Achromatic;
        
        return Limitation.None;
    }
}