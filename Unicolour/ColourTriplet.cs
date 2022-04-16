namespace Wacton.Unicolour;

public record ColourTriplet(double First, double Second, double Third)
{
    public double First { get; } = First;
    public double Second { get; } = Second;
    public double Third { get; } = Third;
    public (double, double, double) Tuple => (First, Second, Third);
    public override string ToString() => Tuple.ToString();
}