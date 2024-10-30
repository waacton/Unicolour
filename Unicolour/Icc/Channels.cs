namespace Wacton.Unicolour.Icc;

public record Channels(params double[] Values)
{
    public double[] Values { get; } = Values;
    public string ColourSpace { get; private set; } = "unknown";
    public string? Error { get; private set; } 

    private ColourHeritage Heritage = ColourHeritage.None;
    private bool UseAsNaN => Heritage == ColourHeritage.NaN || Values.Any(double.IsNaN);
    
    /*
     * ICC channels are a transform of XYZ (in terms of Unicolour implementation)
     * Forward: https://color.org/icc_specs2.xalter
     * Reverse: https://color.org/icc_specs2.xalter
     * (enjoy 100+ pages of dense technical details 🤪)
     */
    
    internal static Channels FromXyz(Xyz xyz, IccConfiguration iccConfig, XyzConfiguration xyzConfig)
    {
        var profile = iccConfig.Profile!;
        var intent = iccConfig.Intent;
        
        double[] channels;
        string? error = null;
        try
        {
            channels = profile.FromXyz(xyz, xyzConfig, intent);
        }
        catch (Exception e)
        {
            channels = Enumerable.Range(0, 15).Select(_ => double.NaN).ToArray();
            error = e.Message;
        }

        return new Channels(channels)
        {
            ColourSpace = profile.Header.DataColourSpace,
            Heritage = ColourHeritage.From(xyz),
            Error = error
        };
    }

    internal static Xyz ToXyz(Channels channels, IccConfiguration iccConfig, XyzConfiguration xyzConfig)
    {
        var profile = iccConfig.Profile!;
        var intent = iccConfig.Intent;
        channels.ColourSpace = profile.Header.DataColourSpace;

        Xyz xyz;
        try
        {
            xyz = profile.ToXyz(channels.Values, xyzConfig, intent);
        }
        catch (Exception e)
        {
            xyz = new Xyz(double.NaN, double.NaN, double.NaN);
            channels.Error = e.Message;
        }

        return xyz;
    }
    
    /*
     * uncalibrated CMYK is a transform of RGB
     * Forward: https://www.w3.org/TR/css-color-5/#cmyk-rgb
     * Reverse: https://www.w3.org/TR/css-color-5/#cmyk-rgb
     */

    internal const string UncalibratedCmyk = "uncalibrated CMYK";

    internal static Channels UncalibratedFromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb.ConstrainedTriplet.Tuple;
        var black = 1 - new[] { r, g, b }.Max();
        var c = black >= 1.0 ? 0 : (1 - r - black) / (1 - black);
        var m = black >= 1.0 ? 0 : (1 - g - black) / (1 - black);
        var y = black >= 1.0 ? 0 : (1 - b - black) / (1 - black);
        return new Channels(c, m, y, black)
        {
            ColourSpace = UncalibratedCmyk,
            Heritage = ColourHeritage.From(rgb)
        };
    }

    internal static Rgb UncalibratedToRgb(Channels channels)
    {
        channels.ColourSpace = UncalibratedCmyk;
        var cmyk = channels.Values;
        double Get(double[] array, int i) => i < array.Length ? array[i] : 0.0;
        var (c, m, y, black) = (Get(cmyk, 0), Get(cmyk, 1), Get(cmyk, 2), Get(cmyk, 3));
        var r = 1 - Math.Min(1, c * (1 - black) + black);
        var g = 1 - Math.Min(1, m * (1 - black) + black);
        var b = 1 - Math.Min(1, y * (1 - black) + black);
        return new Rgb(r, g, b);
    }
    
    public override string ToString()
    {
        var values = $"{string.Join(" ", Values.Select(x => $"{x:F4}"))} {ColourSpace}";
        return UseAsNaN ? $"NaN [{values}]" : values;
    }

    public virtual bool Equals(Channels? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Values.SequenceEqual(other.Values) && ColourSpace == other.ColourSpace;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var valuesHashCode = Values.Aggregate(0, (hashCode, value) => (hashCode * 397) ^ value.GetHashCode());
            return (valuesHashCode * 397) ^ ColourSpace.GetHashCode();
        }
    }
}