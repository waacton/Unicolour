namespace Wacton.Unicolour;

public abstract record ColourRepresentation
{
    protected readonly double First;
    protected readonly double Second;
    protected readonly double Third;
    internal abstract ColourSpace ColourSpace { get; }
    protected abstract int? HueIndex { get; }
    internal bool HasHueAxis => HueIndex != null;
    public ColourTriplet Triplet => new(First, Second, Third, HueIndex);
    internal ColourMode ColourMode { get; }

    protected virtual double ConstrainedFirst => First;
    protected virtual double ConstrainedSecond => Second;
    protected virtual double ConstrainedThird => Third;
    public ColourTriplet ConstrainedTriplet => new(ConstrainedFirst, ConstrainedSecond, ConstrainedThird, HueIndex);

    /*
     * a representation may be non-greyscale according to its values
     * but is effectively greyscale if it was generated from a representation that was greyscale
     * e.g. RGB(1,1,1) is greyscale and converts to LAB(99.99999999999999, 0, -2.220446049250313E-14)
     * LAB doesn't report as greyscale since B != 0 (and I don't want to make assumptions via precision-based tolerance comparison)
     * but it should be considered greyscale since the source RGB representation definitely is
     */
    internal abstract bool IsGreyscale { get; }
    internal bool IsEffectivelyGreyscale => ColourMode == ColourMode.ExplicitGreyscale || (!IsEffectivelyNaN && IsGreyscale);
    
    /*
     * a representation is considered "hued" when it has a hue axis (e.g. HSL / LCH) and is not greyscale
     * enabling differentiation between representations where:
     * a) a hue value is meaningful ------------------------------ e.g. HSB(0,0,0) = red with no saturation or brightness
     * b) a hue value is used as a fallback when there is no hue - e.g. RGB(0,0,0) -> HSB(0,0,0) = black with no red
     * which is essential for proper interpolation;
     * [RGB(0,0,0) black -> RGB(0,0,255) blue] via HSB is [HSB(0,0,0) red with no colour -> HSB(240,1,1) blue with full colour]
     * but the interpolation should only start at the red hue if the value 0 was provided by the user (FromHsb instead of FromRgb)
     */
    internal bool IsHued => HasHueAxis && !IsEffectivelyGreyscale;
    internal bool IsEffectivelyHued => ColourMode == ColourMode.ExplicitHue || (!IsEffectivelyNaN && IsHued);
    
    internal bool IsNaN => double.IsNaN(First) || double.IsNaN(Second) || double.IsNaN(Third);
    internal bool IsEffectivelyNaN => ColourMode == ColourMode.ExplicitNaN || IsNaN;

    internal ColourRepresentation(double first, double second, double third, ColourMode colourMode)
    {
        First = first;
        Second = second;
        Third = third;

        if (colourMode == ColourMode.Unset)
        {
            ColourMode = IsNaN ? ColourMode.ExplicitNaN : ColourMode.Default(HueIndex);
        }
        else
        {
            ColourMode = colourMode;
        }
    }

    protected abstract string FirstString { get; }
    protected abstract string SecondString { get; }
    protected abstract string ThirdString { get; }

    public override string ToString()
    {
        var values = $"{FirstString} {SecondString} {ThirdString}";
        return IsEffectivelyNaN ? $"NaN [{values}]" : values;
    }
}

