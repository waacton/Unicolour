namespace Wacton.Unicolour;

internal static class Interpolation
{
    private record Endpoint(ColourTriplet Triplet, Alpha Alpha)
    {
        internal ColourTriplet Triplet { get; } = Triplet;
        internal Alpha Alpha { get; } = Alpha;
    }

    internal static Unicolour Mix(Unicolour startColour, Unicolour endColour, ColourSpace colourSpace, double distance, HueSpan hueSpan, bool premultiplyAlpha)
    {
        (startColour, endColour, var config) = AdjustConfig(startColour, endColour);

        var from = new Endpoint(startColour.GetTriplet(colourSpace), startColour.Alpha);
        var to = new Endpoint(endColour.GetTriplet(colourSpace), endColour.Alpha);

        Func<double, double> mapToHue = colourSpace == ColourSpace.Wxy
            ? value => Hue.FromWavelength(value, config.Xyz)
            : value => value;
        
        Func<double, double> mapFromHue = colourSpace == ColourSpace.Wxy
            ? value => Hue.ToWavelength(value, config.Xyz)
            : value => value;

        (from, to) = AdaptCoordinates(from, to, hueSpan, premultiplyAlpha, mapToHue);
        var ((first, second, third), alpha, limitation) = Interpolate(from, to, distance, premultiplyAlpha, mapFromHue);
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
    
    private static (Endpoint start, Endpoint end) AdaptCoordinates(Endpoint from, Endpoint to, HueSpan hueSpan, bool premultiplyAlpha, Func<double, double> mapToHue)
    {
        ColourTriplet startTriplet;
        ColourTriplet endTriplet;
        
        // these can't give different answers since they use the same colour space
        // (except by reflection, in which case an error would be thrown when later trying to read the hue component)
        var hasHueComponent = from.Triplet.HasHueComponent || to.Triplet.HasHueComponent;
        if (hasHueComponent)
        {
            (startTriplet, endTriplet) = GetTripletsWithHue(from.Triplet, to.Triplet, hueSpan, mapToHue);
        }
        else
        {
            startTriplet = from.Triplet;
            endTriplet = to.Triplet;
        }
        
        if (premultiplyAlpha)
        {
            startTriplet = startTriplet.WithPremultipliedAlpha(from.Alpha.Clipped.A);
            endTriplet = endTriplet.WithPremultipliedAlpha(to.Alpha.Clipped.A);
        }

        return (new Endpoint(startTriplet, from.Alpha), new Endpoint(endTriplet, to.Alpha));
    }
    
    private static (ColourTriplet start, ColourTriplet end) GetTripletsWithHue(
        ColourTriplet startRepresentation, ColourTriplet endRepresentation, 
        HueSpan hueSpan, Func<double, double> mapToHue)
    {
        var startTriplet = startRepresentation.WithHueMap(mapToHue).WithHueModulo(allow360: true);
        var endTriplet = endRepresentation.WithHueMap(mapToHue).WithHueModulo(allow360: true);

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
    
    private static (ColourTriplet triplet, double alpha, Limitation limitation) Interpolate(
        Endpoint from, Endpoint to, double distance, bool premultiplyAlpha, Func<double, double> mapFromHue)
    {
        var first = Linear(from.Triplet.First, to.Triplet.First, distance);
        var second = Linear(from.Triplet.Second, to.Triplet.Second, distance);
        var third = Linear(from.Triplet.Third, to.Triplet.Third, distance);
        var alpha = Linear(from.Alpha.Clipped.A, to.Alpha.Clipped.A, distance);
        var limitation = InterpolateLimitation(from.Triplet, to.Triplet);

        var triplet = new ColourTriplet(first, second, third, limitation);
        triplet = triplet.WithHueModulo().WithHueMap(mapFromHue);
        if (premultiplyAlpha)
        {
            triplet = triplet.WithUnpremultipliedAlpha(alpha);
        }
        
        return (triplet, alpha, limitation);
    }

    internal static double Linear(double startValue, double endValue, double distance)
    {
        var difference = endValue - startValue;
        return startValue + difference * distance;
    }

    private static Limitation InterpolateLimitation(ColourTriplet colour1, ColourTriplet colour2)
    {
        var first = colour1.Limitation;
        var second = colour2.Limitation;

        // Limitation.Unknown only occurs when constructing ColourTriplet directly
        // instead of derived classes like Rgb, Hsb, etc. which default to Limitation.None
        // should not be possible so something has gone wrong if it happens
        var eitherUnknown = first == Limitation.Unset || second == Limitation.Unset;
        if (eitherUnknown) throw new InvalidOperationException("Limitation must be known for interpolation");

        var eitherNaN = first == Limitation.NaN || second == Limitation.NaN;
        if (eitherNaN) return Limitation.NaN;
        
        var bothAchromatic = first == Limitation.Achromatic && second == Limitation.Achromatic;
        if (bothAchromatic) return Limitation.Achromatic;
        
        return Limitation.None;
    }
}