namespace Wacton.Unicolour;

public abstract record ColourRepresentation : ColourBase // TODO: merge with ColourTriplet?!
{
    // public virtual bool Equals(ColourRepresentation? other)
    // {
    //     if (other is null) return false;
    //     if (ReferenceEquals(this, other)) return true;
    //     return base.Equals(other) && HueIndex == other.HueIndex && String == other.String;
    // }
    //
    // public override int GetHashCode()
    // {
    //     unchecked
    //     {
    //         int hashCode = base.GetHashCode();
    //         hashCode = (hashCode * 397) ^ HueIndex.GetHashCode();
    //         hashCode = (hashCode * 397) ^ String.GetHashCode();
    //         return hashCode;
    //     }
    // }

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

