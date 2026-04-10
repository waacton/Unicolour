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
    internal bool IsNaN => Limitation == Limitation.NaN;
    
    /*
     * ℹ️
     * limitation is NOT intended for use outside of interpolation and unit tests
     * conversions should make decisions based on the actual values, not based on propagated limitations
     * colour spaces such as CAM02, CAM16, HCT seem to have a different perception of what achromatic colour is
     * (i.e. xyY white point chromaticity is converted to a CAM that has non-negligible chroma, and CAM-UCS of 0 chroma is not mapped to white point chromaticity)
     * so while it is used to make hue-handling decisions during interpolation, it should not be used to bypass conversion formula
     * ℹ️
     * ----------
     * limitation is used to maintain consistent behaviour of downstream representations
     * e.g. if double.NaN is somehow turned into a 0 during conversion, still want NaN-checks to return true
     * e.g. RGB 0 0 0 is achromatic but HSB 0 0 0 is not (it has a specific hue value of 0 = red)
     *      a colour defined using RGB 0 0 0 and interpolated through HSB should ignore the hue value (source colour is achromatic)
     *      a colour defined using HSB 0 0 0 and interpolated through HSB should include the hue value (source colour is grey but has an explicit hue of 0)
     * e.g. RGB 1 1 1 is achromatic and LUV 100 0 0 is achromatic
     *      but the conversion formula gives RGB 1 1 1 --> LUV 99.99999999999999, -7.216449660063516E-14, 0
     *      and while -7E-14 is small, it is not zero, so LUV itself will not report achromatic limitation
     *      in order to interpolate through LCHUV correctly, the original achromatic limitation has to be passed from RGB -> RGB-Linear -> XYZ -> LUV -> LCHUV
     * ----------     
     * 💡 greyscale != achromatic · achromatic limitation flags when chroma information has truly been lost, and cannot be recovered
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
    public override string ToString() => IsNaN ? $"NaN [{String}]" : String;
}

