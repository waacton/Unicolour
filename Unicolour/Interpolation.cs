namespace Wacton.Unicolour;

internal static class Interpolation
{
    internal static Unicolour Mix(Unicolour colour1, Unicolour colour2, ColourSpace colourSpace, double distance, HueSpan hueSpan, bool premultiplyAlpha)
    {
        (colour1, colour2, var config) = AdjustConfig(colour1, colour2);

        var from = new Endpoint(colour1.GetTriplet(colourSpace), colour1.Alpha);
        var to = new Endpoint(colour2.GetTriplet(colourSpace), colour2.Alpha);

        Func<double, double> mapToHue = colourSpace == ColourSpace.Wxy
            ? value => Hue.FromWavelength(value, config.Xyz)
            : value => value;
        
        Func<double, double> mapFromHue = colourSpace == ColourSpace.Wxy
            ? value => Hue.ToWavelength(value, config.Xyz)
            : value => value;

        (from, to) = AdaptTriplets(from, to, hueSpan, premultiplyAlpha, mapToHue);
        var ((first, second, third), alpha, limitation) = Interpolate(from, to, distance, premultiplyAlpha, mapFromHue);
        return new Unicolour(config, colourSpace, first, second, third, alpha, limitation);
    }
    
    internal static IEnumerable<Unicolour> Palette(Unicolour colour1, Unicolour colour2, ColourSpace colourSpace, int count, HueSpan hueSpan, bool premultiplyAlpha)
    {
        count = Math.Max(count, 0);
        (colour1, colour2, _) = AdjustConfig(colour1, colour2); // saves doing this N times via Mix() if configs are different
        
        List<Unicolour> palette = [];
        for (var i = 0; i < count; i++)
        {
            var distance = count == 1 ? 0.5 : i / (double)(count - 1);
            palette.Add(Mix(colour1, colour2, colourSpace, distance, hueSpan, premultiplyAlpha));
        }

        return palette;
    }
    
    private static (Unicolour colour1, Unicolour colour2, Configuration config) AdjustConfig(Unicolour colour1, Unicolour colour2)
    {
        var config = colour1.Configuration;
        return colour2.Configuration == config 
            ? (colour1, colour2, config) 
            : (colour1, colour2.ConvertToConfiguration(config), config);
    }
    
    private static (Endpoint from, Endpoint to) AdaptTriplets(Endpoint from, Endpoint to, HueSpan hueSpan, bool premultiplyAlpha, Func<double, double> mapToHue)
    {
        var triplet1 = from.Triplet;
        var triplet2 = to.Triplet;
        var alpha1 = from.Alpha;
        var alpha2 = to.Alpha;
        
        // these can't give different answers since they use the same colour space
        // (except by reflection, in which case an error would be thrown when later trying to read the hue component)
        var hasHueComponent = from.Triplet.HasHueComponent || to.Triplet.HasHueComponent;
        if (hasHueComponent)
        {
            triplet1 = triplet1.WithHueMap(mapToHue).WithHueModulo(allow360: true);
            triplet2 = triplet2.WithHueMap(mapToHue).WithHueModulo(allow360: true);

            double hue1, hue2;
            if (triplet1.IsAchromatic ^ triplet2.IsAchromatic)
            {
                // if only one colour is achromatic, use only the hue of the chromatic colour
                // e.g. black n/a° to green 120° should always stay at hue 120°
                hue1 = triplet1.IsAchromatic ? triplet2.HueValue() : triplet1.HueValue();
                hue2 = triplet2.IsAchromatic ? triplet1.HueValue() : triplet2.HueValue();
            }
            else
            {
                // if both colours are chromatic, interpolate between hues as normal
                // if both colours are achromatic, there is no 'correct' hue, so interpolate between the powerless hue values
                hue1 = triplet1.HueValue();
                hue2 = triplet2.HueValue();
            }
        
            (hue1, hue2) = Hue.Unwrap(hue1, hue2, hueSpan);
            triplet1 = triplet1.WithHueOverride(hue1);
            triplet2 = triplet2.WithHueOverride(hue2);
        }
        
        if (premultiplyAlpha)
        {
            triplet1 = triplet1.WithPremultipliedAlpha(alpha1.Clipped.A);
            triplet2 = triplet2.WithPremultipliedAlpha(alpha2.Clipped.A);
        }

        return (new Endpoint(triplet1, alpha1), new Endpoint(triplet2, alpha2));
    }
    
    private static (ColourTriplet triplet, double alpha, Limitation limitation) Interpolate(
        Endpoint from, Endpoint to, double distance, bool premultiplyAlpha, Func<double, double> mapFromHue)
    {
        var first = Linear(from.Triplet.First, to.Triplet.First, distance);
        var second = Linear(from.Triplet.Second, to.Triplet.Second, distance);
        var third = Linear(from.Triplet.Third, to.Triplet.Third, distance);
        var alpha = Linear(from.Alpha.Clipped.A, to.Alpha.Clipped.A, distance);
        var limitation = InterpolateLimitation(from.Triplet.Limitation, to.Triplet.Limitation);

        var triplet = new ColourTriplet(first, second, third, from.Triplet.HueIndex, limitation);
        triplet = triplet.WithHueModulo().WithHueMap(mapFromHue);
        if (premultiplyAlpha)
        {
            triplet = triplet.WithUnpremultipliedAlpha(alpha);
        }
        
        return (triplet, alpha, limitation);
    }

    internal static double Linear(double from, double to, double distance)
    {
        var difference = to - from;
        return from + difference * distance;
    }

    private static Limitation InterpolateLimitation(Limitation from, Limitation to)
    {
        // Limitation.Unset only occurs when constructing ColourTriplet directly
        // instead of derived classes like Rgb, Hsb, etc. which default to Limitation.None
        // should not be possible so something has gone wrong if it happens
        var eitherUnset = from == Limitation.Unset || to == Limitation.Unset;
        if (eitherUnset) throw new InvalidOperationException("Limitation must be set for interpolation");

        var eitherNaN = from == Limitation.NaN || to == Limitation.NaN;
        if (eitherNaN) return Limitation.NaN;
        
        var bothAchromatic = from == Limitation.Achromatic && to == Limitation.Achromatic;
        if (bothAchromatic) return Limitation.Achromatic;
        
        return Limitation.None;
    }
    
    private record Endpoint(ColourTriplet Triplet, Alpha Alpha)
    {
        internal ColourTriplet Triplet { get; } = Triplet;
        internal Alpha Alpha { get; } = Alpha;
    }
}