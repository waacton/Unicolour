namespace Wacton.Unicolour.Tests.Utils;

public record AlphaTriplet(ColourTriplet Triplet, double Alpha)
{
    internal (double first, double second, double third, double alpha) Tuple => (Triplet.First, Triplet.Second, Triplet.Third, Alpha);
    public override string ToString() => Tuple.ToString();
}