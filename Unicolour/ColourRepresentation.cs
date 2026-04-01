namespace Wacton.Unicolour;

public abstract record ColourRepresentation
{
    protected internal abstract int? HueIndex { get; }
    internal bool HasHueComponent => HueIndex != null;
    
    protected readonly double First;
    protected readonly double Second;
    protected readonly double Third;
    public ColourTriplet Triplet => new(First, Second, Third, HueIndex);
    public (double, double, double) Tuple => (First, Second, Third);
    
    protected abstract bool IsAchromatic { get; }
    
    /*
     * limitation is used to enforce behaviour on downstream conversions, achromaticity in particular
     * e.g. RGB 1 1 1 converts to LAB 99.999... 0 -2E-14
     * - the original RGB values represent an achromatic colour (grey)
     * - the result LAB values represent a chromatic colour, because B != 0
     * - in reality, because LAB was calculated from achromatic RGB, it must inherit that achromaticity, even if the values suggest chromatic
     * this is mostly useful to ensure correct interpolation and distinguishes between e.g.
     * - A) HSB 0 0 0 from direct user input (red with no saturation or brightness)
     * - B) HSB 0 0 0 from RGB 0 0 0 (achromatic black where the hue value is irrelevant and 0 is a fallback value)
     * - mixing A with HSB 240 1 1 (blue) should traverse the hues between 0 (red) and 240 (blue)
     * - mixing B with HSB 240 1 1 (blue) should ignore 0 (red) hue and use 240 (blue) hue for all interpolated colours
     * so it's important to note that greyscale != achromatic
     * a colour like HSB 180 0 0 looks grey but still has chroma information encoded (green hue)
     * the achromatic limitation flags when chroma information has truly been lost, and cannot be recovered
     */
    internal Limitation LimitationBaseline { get; }
    internal Limitation Limitation
    {
        get
        {
            if (LimitationBaseline == Limitation.NaN || Triplet.WithHueModulo().ToArray().Any(double.IsNaN)) return Limitation.NaN;
            if (LimitationBaseline == Limitation.Achromatic || IsAchromatic) return Limitation.Achromatic;
            return Limitation.None;
        }
    }
    
    internal ColourRepresentation(double first, double second, double third, Limitation limitationBaseline)
    {
        First = first;
        Second = second;
        Third = third;
        LimitationBaseline = limitationBaseline;
    }
    
    internal ColourTriplet WithHueModulo() => Triplet.WithHueModulo();
    
    public double[] ToArray() => [First, Second, Third];
    
    public void Deconstruct(out double first, out double second, out double third)
    {
        (first, second, third) = Tuple;
    }
    
    protected abstract string String { get; }
    public override string ToString() => Limitation == Limitation.NaN ? $"NaN [{String}]" : String;
}

