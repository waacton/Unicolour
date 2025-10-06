namespace Wacton.Unicolour.Datasets;

// https://github.com/bastibe/twilight
public class TwilightShifted : Colourmap
{
    internal TwilightShifted()
    {
    }
    
    public override Unicolour Map(double x) => InterpolateColourTable(Lookup, x);
    public override string ToString() => nameof(TwilightShifted);

    public static readonly IEnumerable<Unicolour> Lookup =
        Twilight.Lookup.Skip(Twilight.Lookup.Count() / 2)
            .Concat(Twilight.Lookup.Take(Twilight.Lookup.Count() / 2))
            .Reverse();
}