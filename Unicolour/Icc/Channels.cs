namespace Wacton.Unicolour.Icc;

public record Channels(params double[] Values)
{
    public double[] Values { get; } = Values;
    public string ColourSpace { get; private set; } = "unknown";
    public string? Error { get; private set; }

    internal bool IsNaN => Limitation == Limitation.NaN;
    
    private Limitation LimitationBaseline = Limitation.None;
    private Limitation Limitation
    {
        get
        {
            if (LimitationBaseline == Limitation.NaN || Values.Any(double.IsNaN)) return Limitation.NaN;
            if (LimitationBaseline == Limitation.Achromatic) return Limitation.Achromatic; // cannot infer achromatic from ICC channels (e.g. FOGRA39 0.5 0.5 0.5 0.5 is not grey)
            return Limitation.None;
        }
    }
    
    /*
     * ICC channels are a transform of XYZ (in terms of Unicolour implementation)
     * Forward: https://color.org/icc_specs2.xalter
     * Reverse: https://color.org/icc_specs2.xalter
     * (enjoy 100+ pages of dense technical details 🤪)
     */
    
    internal static Channels FromXyz(Xyz xyz, ChromaticAdaptor chromaticAdaptor, IccConfiguration iccConfig)
    {
        var profile = iccConfig.Profile!;
        var intent = iccConfig.Intent;
        
        double[] channels;
        string? error = null;
        try
        {
            channels = profile.FromXyz(xyz, intent, chromaticAdaptor);
        }
        catch (Exception e)
        {
            channels = Enumerable.Range(0, 15).Select(_ => double.NaN).ToArray();
            error = e.Message;
        }

        return new Channels(channels)
        {
            ColourSpace = profile.Header.DataColourSpace,
            LimitationBaseline = xyz.Limitation,
            Error = error
        };
    }

    internal static Xyz ToXyz(Channels channels, ChromaticAdaptor chromaticAdaptor, IccConfiguration iccConfig)
    {
        var profile = iccConfig.Profile!;
        var intent = iccConfig.Intent;
        channels.ColourSpace = profile.Header.DataColourSpace;

        Xyz xyz;
        try
        {
            xyz = profile.ToXyz(channels.Values, intent, chromaticAdaptor);
        }
        catch (Exception e)
        {
            xyz = new Xyz(double.NaN, double.NaN, double.NaN, chromaticAdaptor.WhitePoint);
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
        var (r, g, b) = rgb;
        var black = 1 - rgb.ToArray().Max();
        var scale = 1 - black;
        var c = scale == 0.0 ? 0 : (1 - r - black) / scale;
        var m = scale == 0.0 ? 0 : (1 - g - black) / scale;
        var y = scale == 0.0 ? 0 : (1 - b - black) / scale;
        return new Channels(c, m, y, black)
        {
            ColourSpace = UncalibratedCmyk,
            LimitationBaseline = rgb.Limitation
        };
    }

    internal static Rgb UncalibratedToRgb(Channels channels)
    {
        channels.ColourSpace = UncalibratedCmyk;
        var cmyk = channels.Values;
        double Get(double[] array, int i) => i < array.Length ? array[i] : 0.0;
        var (c, m, y, black) = (Get(cmyk, 0), Get(cmyk, 1), Get(cmyk, 2), Get(cmyk, 3));
        var r = 1 - (c * (1 - black) + black);
        var g = 1 - (m * (1 - black) + black);
        var b = 1 - (y * (1 - black) + black);
        return new Rgb(r, g, b);
    }
    
    public override string ToString()
    {
        var values = $"{string.Join(" ", Values.Select(x => $"{x:F4}"))} {ColourSpace}";
        return IsNaN ? $"NaN [{values}]" : values;
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