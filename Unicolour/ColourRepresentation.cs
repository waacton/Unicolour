namespace Wacton.Unicolour;

public abstract record ColourRepresentation : ColourBase // TODO: merge with ColourTriplet?!
{
    protected internal abstract int? HueIndex { get; }
    internal bool HasHueComponent => HueIndex != null;

    protected double First => Values[0];
    protected double Second => Values[1];
    protected double Third => Values[2];
    public ColourTriplet Triplet => new(First, Second, Third, HueIndex);
    public (double, double, double) Tuple => (First, Second, Third);
    
    internal ColourRepresentation(double first, double second, double third, Limitation limitationBaseline) : base([first, second, third], limitationBaseline)
    {
    }
    
    internal ColourTriplet WithHueModulo() => Triplet.WithHueModulo();
    
    public double[] ToArray() => Values; // TODO: unnecessary
    
    public void Deconstruct(out double first, out double second, out double third)
    {
        (first, second, third) = Tuple;
    }
    
    protected abstract string String { get; }
    public override string ToString() => IsNaN ? $"NaN [{String}]" : String;
}

