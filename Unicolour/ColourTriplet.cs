namespace Wacton.Unicolour;

public record ColourTriplet : ColourBase
{
    public double First => this[0];
    public double Second => this[1];
    public double Third => this[2];
    public int? HueIndex { get; }
    internal bool HasHueComponent => HueIndex != null;
    
    public (double, double, double) Tuple => (this[0], this[1], this[2]);

    // triplet does not have enough context to know if values are achromatic
    // the assumption is that it is false unless derived classes explicitly say otherwise
    // (i.e. a colour defined using all 3 coordinates of a hue-based system, by definition, has hue and chroma - even if it cannot be detected)
    protected override bool HasAchromaticValues => false;

    internal ColourTriplet(double first, double second, double third, int hueIndex) : this(first, second, third, hueIndex, Limitation.Unset) {}
    internal ColourTriplet(double first, double second, double third, Limitation limitation) : this(first, second, third, null, limitation) {}
    internal ColourTriplet(double first, double second, double third, int? hueIndex = null, Limitation limitation = Limitation.Unset) : base([first, second, third], limitation)
    {
        HueIndex = hueIndex;
    }
    
    internal double HueValue()
    {
        return HueIndex switch
        {
            0 => First,
            2 => Third,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    internal ColourTriplet WithHueOverride(double hue)
    {
        return HueIndex switch
        {
            0 => new(hue, Second, Third, HueIndex, Limitation),
            2 => new(First, Second, hue, HueIndex, Limitation),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    internal ColourTriplet WithHueMap(Func<double, double> hueMap)
    {
        return HueIndex switch
        {
            0 => new(hueMap(First), Second, Third, HueIndex, Limitation),
            2 => new(First, Second, hueMap(Third), HueIndex, Limitation),
            null => new(First, Second, Third, HueIndex, Limitation),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    internal ColourTriplet WithHueModulo(bool allow360 = false)
    {
        return HueIndex switch
        {
            0 => new(First.Modulo(360, allow360), Second, Third, HueIndex, Limitation),
            2 => new(First, Second, Third.Modulo(360, allow360), HueIndex, Limitation),
            null => new(First, Second, Third, HueIndex, Limitation),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    internal ColourTriplet WithPremultipliedAlpha(double alpha)
    {
        return HueIndex switch
        {
            0 => new(First, Second * alpha, Third * alpha, HueIndex, Limitation),
            2 => new(First * alpha, Second * alpha, Third, HueIndex, Limitation),
            null => new(First * alpha, Second * alpha, Third * alpha, HueIndex, Limitation),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    internal ColourTriplet WithUnpremultipliedAlpha(double alpha)
    {
        if (alpha == 0)
        {
            alpha = 1.0;
        }
        
        return HueIndex switch
        {
            0 => new(First, Second / alpha, Third / alpha, HueIndex, Limitation),
            2 => new(First / alpha, Second / alpha, Third, HueIndex, Limitation),
            null => new(First / alpha, Second / alpha, Third / alpha, HueIndex, Limitation),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public void Deconstruct(out double first, out double second, out double third)
    {
        (first, second, third) = (this[0], this[1], this[2]);
    }
    
    protected virtual string String => $"{First} {Second} {Third}";
    public override string ToString() => IsNaN ? $"NaN [{String}]" : String;
}