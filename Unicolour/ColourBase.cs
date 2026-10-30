namespace Wacton.Unicolour;

public abstract record ColourBase
{
    public readonly double[] Values;
    public double this[int index] => Values[index];
    
    protected abstract bool IsTripletAchromatic { get; } // TODO: rename
    
    internal bool IsAchromatic => Limitation == Limitation.Achromatic;
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
            if (LimitationBaseline == Limitation.NaN || Values.Any(double.IsNaN)) return Limitation.NaN;
            if (LimitationBaseline == Limitation.Achromatic || IsTripletAchromatic) return Limitation.Achromatic;
            return Limitation.None;
        }
    }

    internal ColourBase(double[] values, Limitation limitationBaseline)
    {
        Values = values;
        LimitationBaseline = limitationBaseline;
    }
    
    // arrays use reference equality so needs overriding to check only values 
    public virtual bool Equals(ColourBase? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Values.SequenceEqual(other.Values) && LimitationBaseline == other.LimitationBaseline;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Values.Aggregate(0, (current, value) => (current * 397) ^ value.GetHashCode());
            hashCode = (hashCode * 397) ^ (int)LimitationBaseline;
            return hashCode;
        }
    }
}