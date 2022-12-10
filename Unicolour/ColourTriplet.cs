namespace Wacton.Unicolour;

public record ColourTriplet(double First, double Second, double Third, int? HueIndex = null)
{
    public double First { get; } = First;
    public double Second { get; } = Second;
    public double Third { get; } = Third;
    public (double, double, double) Tuple => (First, Second, Third);
    public int? HueIndex { get; } = HueIndex;
    
    internal double HueValue()
    {
        return HueIndex switch
        {
            0 => First,
            2 => Third,
            _ => throw new ArgumentException()
        };
    }
    
    internal ColourTriplet WithHueOverride(double hue)
    {
        return HueIndex switch
        {
            0 => new(hue, Second, Third, HueIndex),
            2 => new(First, Second, hue, HueIndex),
            _ => throw new ArgumentException()
        };
    }

    internal ColourTriplet WithHueModulo()
    {
        return HueIndex switch
        {
            0 => new(First.Modulo(360), Second, Third, HueIndex),
            2 => new(First, Second, Third.Modulo(360), HueIndex),
            _ => new(First, Second, Third, HueIndex)
        };
    }
    
    public override string ToString() => Tuple.ToString();

    // need a custom deconstruct to ignore the nullable hue
    public void Deconstruct(out double first, out double second, out double third)
    {
        first = First;
        second = Second;
        third = Third;
    }
}